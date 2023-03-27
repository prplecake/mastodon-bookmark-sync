using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Interfaces;
using BookmarkSync.Infrastructure.Services.Pinboard;

namespace BookmarkSync.Infrastructure.Services;

public abstract class BookmarkingService
{
    /// <summary>
    /// The HttpClient object.
    /// </summary>
    private static readonly HttpClient Client = new();
    protected BookmarkingService()
    {
        // Setup HttpClient
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        Client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("mastodon-bookmark-sync",
            FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion));
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