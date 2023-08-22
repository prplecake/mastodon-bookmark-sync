using BookmarkSync.Core.Entities.Config;
using BookmarkSync.Core.Extensions;
using BookmarkSync.Infrastructure.Services.Mastodon;
using Microsoft.Extensions.Hosting;

namespace BookmarkSync.Infrastructure.Services.Bookmarking;

public class BookmarkSyncService : IHostedService
{
    private static readonly ILogger _logger = Log.ForContext<BookmarkSyncService>();
    private readonly IBookmarkingService _bookmarkingService;
    private readonly IHostApplicationLifetime _host;
    private readonly List<string> _ignoredAccounts;
    private readonly List<Instance>? _instances;
    private readonly MastodonService _mastodonService;
    public BookmarkSyncService(IHostApplicationLifetime host, IConfigManager configManager, HttpClient client, MastodonService mastodonService)
    {
        _bookmarkingService = BookmarkingService.GetBookmarkingService(configManager, client);
        _host = host;
        _instances = configManager.Instances;
        _ignoredAccounts = configManager.App.IgnoredAccounts;
        _mastodonService = mastodonService;
    }
    /// <inheritdoc/>
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
            _mastodonService.SetInstance(instance);
            // Get bookmarks from mastodon account
            List<Bookmark>? bookmarks = null;
            try
            {
                bookmarks = (await _mastodonService.GetBookmarks())?
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

            // Remove any bookmarks from accounts in the IgnoredAccounts list
            bookmarks.RemoveAllFromIgnoredAccounts(_ignoredAccounts);

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
                    await _mastodonService.DeleteBookmark(bookmark);
                }
            }
        }

        // Finish task
        _host.StopApplication();
    }
    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
