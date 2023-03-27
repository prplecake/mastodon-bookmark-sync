using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities.Config;
using BookmarkSync.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BookmarkSync.Infrastructure.Services;

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
        _logger.Information("The current time is: {CurrentTime}", DateTimeOffset.UtcNow);

        if (_instances == null || _instances.Count == 0)
        {
            _logger.Information("No instances configured");
            return;
        }
        foreach (var instance in _instances)
        {
            _logger.Information("Processing {Instance}", instance);
            _logger.Debug("Setting up Mastodon API client");
            var client = new Mastodon.ApiClient(instance);
            // Get bookmarks from mastodon account
            var bookmarks = await client.GetBookmarks();

            if (bookmarks == null || bookmarks.Count == 0)
            {
                _logger.Information("No bookmarks received");
                return;
            }
            foreach (var bookmark in bookmarks)
            {
                // Save bookmarks to bookmarking service
                var result = await _bookmarkingService.Save(bookmark);

                if (instance.DeleteBookmarks && result.IsSuccessStatusCode)
                {
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
