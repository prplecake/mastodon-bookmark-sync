using System.Net.Http.Headers;
using System.Net.Mime;
using BookmarkSync.Core;
using BookmarkSync.Infrastructure.Services.Bookmarking.Briefkasten;
using BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;
using BookmarkSync.Infrastructure.Services.Bookmarking.Linkding;
using BookmarkSync.Infrastructure.Services.Bookmarking.Pinboard;

namespace BookmarkSync.Infrastructure.Services.Bookmarking;

public abstract class BookmarkingService
{
    /// <summary>
    /// The HttpClient object.
    /// </summary>
    protected static HttpClient Client = new();
    protected BookmarkingService(HttpClient client)
    {
        // Setup HttpClient
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.UserAgent.Add(Meta.UserAgent);
        Client = client;
    }
    /// <summary>
    /// The API auth token..
    /// </summary>
    protected string ApiToken { get; init; } = null!;
    /// <summary>
    /// The Api URL.
    /// </summary>
    protected string ApiUri { get; init; } = null!;
    public static IBookmarkingService GetBookmarkingService(IConfigManager configManager, HttpClient client)
    {
        return configManager.App.Bookmarking.Service switch
        {
            "Briefkasten" => new BriefkastenBookmarkingService(configManager, client),
            "LinkAce" => new LinkAceBookmarkingService(configManager, client),
            "Pinboard" => new PinboardBookmarkingService(configManager, client),
            "linkding" => new LinkdingBookmarkingService(configManager, client),
            _ => throw new InvalidOperationException("Bookmark service either not provided or unknown")
        };
    }
}
