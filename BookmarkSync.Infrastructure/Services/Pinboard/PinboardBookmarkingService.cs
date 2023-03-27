using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Interfaces;
using Serilog;

namespace BookmarkSync.Infrastructure.Services.Pinboard;

public class PinboardBookmarkingService : BookmarkingService, IBookmarkingService
{
    private const int PinboardDescriptionMaxLength = 255;
    private static readonly ILogger _logger = Log.ForContext<PinboardBookmarkingService>();
    public PinboardBookmarkingService(IConfigManager configManager)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException();
        ApiUri = "https://api.pinboard.in/v1/posts/add";
    }
    /// <inheritdoc />
    public async Task<HttpResponseMessage> Save(Bookmark bookmark)
    {
        // Prep bookmark
        string? extended = null;
        if (bookmark.Content!.Length >= PinboardDescriptionMaxLength)
        {
            string? trimmedDescription = bookmark.Content[..PinboardDescriptionMaxLength];
            extended = bookmark.Content;
            bookmark.Content = trimmedDescription;
        }

        var builder = new UriBuilder(ApiUri);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["auth_token"] = ApiToken;
        query["description"] = bookmark.Content;
        query["url"] = bookmark.Uri;
        query["shared"] = "no";
        if (!string.IsNullOrEmpty(extended))
        {
            query["extended"] = extended;
        }
        query["tags"] = string.Join(" ", $"via:@{bookmark.Account}", "via:mastodon-bookmark-sync");
        builder.Query = query.ToString();
        var requestUri = builder.ToString();
        _logger.Debug("Request URI: {RequestUri}", requestUri);

        return await Client.GetAsync(requestUri);
    }
}
