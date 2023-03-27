using System;
using System.Threading.Tasks;
using System.Web;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Interfaces;
using Serilog;

namespace BookmarkSync.Infrastructure.Services.Pinboard;

public class PinboardBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<PinboardBookmarkingService>();
    public PinboardBookmarkingService(IConfigManager configManager)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException();
        ApiUri = "https://api.pinboard.in/v1/posts/add";
    }
    /// <inheritdoc />
    public async Task Save(Bookmark bookmark)
    {
        var builder = new UriBuilder(ApiUri);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["auth_token"] = ApiToken;
        // query["description"] = bookmark.Description;
        query["url"] = bookmark.Uri;
        // query["shared"] = bookmark.Shared ? "yes" : "no";
        // query["extended"] = bookmark.ExtendedDescription;
        // query["tags"] = bookmark.GetFormattedTags();
        builder.Query = query.ToString();
        var requestUri = builder.ToString();
        _logger.Debug("Request URI: {RequestUri}", requestUri);

        // var result = await Client.GetAsync(requestUri);
        // result.EnsureSuccessStatusCode();
    }
}
