using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using BookmarkSync.Core;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Interfaces;
using BookmarkSync.Infrastructure.Services.Pinboard;

namespace BookmarkSync.Infrastructure.Services;

public abstract class BookmarkingService
{
    /// <summary>
    /// The HttpClient object.
    /// </summary>
    protected static readonly HttpClient Client = new();
    protected BookmarkingService()
    {
        // Setup HttpClient
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        Client.DefaultRequestHeaders.UserAgent.Add(Meta.UserAgent);
    }
    /// <summary>
    /// The API auth token..
    /// </summary>
    protected string ApiToken { get; init; } = null!;
    /// <summary>
    /// The Api URL.
    /// </summary>
    protected string ApiUri { get; init; } = null!;
    public static IBookmarkingService GetBookmarkingService(IConfigManager configManager)
    {
        return configManager.App.Bookmarking.Service switch
        {
            "Pinboard" => new PinboardBookmarkingService(configManager),
            _ => throw new InvalidOperationException("Bookmark service either not provided or unknown")
        };
    }
}
