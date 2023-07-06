using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.Linkding;

public class LinkdingBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<LinkdingBookmarkingService>();
    public LinkdingBookmarkingService(IConfigManager configManager)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException("Missing API token");
        string linkdingUri = configManager.GetConfigValue("App:Bookmarking:LinkdingUri") ??
                            throw new InvalidOperationException("Missing linkding Uri");
        ApiUri = $"{linkdingUri}/api/bookmarks/";
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", ApiToken);
    }
    /// <inheritdoc />
    public async Task<HttpResponseMessage> Save(Bookmark bookmark)
    {
        // Prep payload
        Dictionary<string, object?> payload = new()
        {
            {
                "url", bookmark.Uri
            },
            {
                "title", bookmark.Content
            },
            {
                "tag_names", bookmark.DefaultTags
            },
            {
                "unread", false
            },
            {
                "shared", false
            }
        };
        var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var response = await Client.PostAsync(ApiUri, stringContent);
        response.EnsureSuccessStatusCode();
        _logger.Debug("Response status: {StatusCode}", response.StatusCode);
        return response;
    }
}
