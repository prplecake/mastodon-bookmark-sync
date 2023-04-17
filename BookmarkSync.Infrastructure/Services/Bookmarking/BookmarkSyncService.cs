using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Entities.Config;
using BookmarkSync.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BookmarkSync.Infrastructure.Services.Bookmarking;

public class BookmarkSyncService : IHostedService
{
    private readonly IHostApplicationLifetime _host;
    private readonly List<Instance>? _instances;
    private readonly IBookmarkingService _bookmarkingService;
    private static readonly ILogger _logger = Log.ForContext<BookmarkSyncService>();
    public BookmarkSyncService(IHostApplicationLifetime host, IConfigManager configManager)
    {
        _bookmarkingService = BookmarkingService.GetBookmarkingService(configManager);
        _host = host;
        _instances = configManager.Instances;
    }
    /// <inheritdoc />
    public async Task StartAsync(CancellationToken stoppingToken)
    {
        if (_instances == null || _instances.Count == 0)
        {
            _logger.Warning("No instances configured");
            return;
        }
        foreach (var instance in _instances)
        {
            _logger.Information("Processing {Instance}", instance);
            _logger.Debug("Setting up Mastodon API client");
            var client = new Mastodon.ApiClient(instance);
            // Get bookmarks from mastodon account
            List<Bookmark>? bookmarks = null;
            try
            {
                bookmarks = (await client.GetBookmarks())?
                    .Where(b => b.Visibility is not ("private" or "direct"))
                    .ToList();
            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, "Failed to retrieve bookmarks from {Instance}", instance);
            }

            if (bookmarks == null || bookmarks.Count == 0)
            {
                _logger.Information("No bookmarks received");
                continue;
            }
            foreach (var bookmark in bookmarks)
            {
                // Save bookmarks to bookmarking service
                HttpResponseMessage result;
                try
                {
                    result = await _bookmarkingService.Save(bookmark);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to save bookmark");
                    continue;
                }

                if (instance.DeleteBookmarks && result.IsSuccessStatusCode)
                {
                    _logger.Information("Deleting bookmark");
                    await client.DeleteBookmark(bookmark);
                }
            }
        }

        // Finish task
        _host.StopApplication();
    }
    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
