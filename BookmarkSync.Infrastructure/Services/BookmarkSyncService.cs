using System;
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
    private readonly IBookmarkingService _bookmarkingService;
    private static readonly ILogger _logger = Log.ForContext<BookmarkSyncService>();
    public BookmarkSyncService(IHostApplicationLifetime host, IConfigManager configManager)
    {
        _bookmarkingService = BookmarkingService.GetBookmarkingService(configManager);
        _host = host;
    }
    /// <inheritdoc />
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.Information("The current time is: {CurrentTime}", DateTimeOffset.UtcNow);

        // Get bookmarks from mastodon account
        // TODO
        
        // Save bookmarks to bookmarking service
        _bookmarkingService.Save(new Bookmark());

        // Finish task
        _host.StopApplication();
        return Task.CompletedTask;
    }
    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
