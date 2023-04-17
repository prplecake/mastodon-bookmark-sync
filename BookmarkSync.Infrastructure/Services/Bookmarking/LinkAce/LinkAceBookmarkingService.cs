using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;

public class LinkAceBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<LinkAceBookmarkingService>();
    public LinkAceBookmarkingService(IConfigManager configManager)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException("Missing API token");
        string linkAceUri = configManager.GetConfigValue("App:Bookmarking:LinkAceUri") ??
                            throw new InvalidOperationException("Missing LinkAce Uri");
        ApiUri = $"{linkAceUri}/api/v1/links";
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
    }
    /// <inheritdoc />
    public async Task<HttpResponseMessage> Save(Bookmark bookmark)
    {
        // Prep payload
        Dictionary<string, object> payload = new()
        {
            {
                "url", bookmark.Uri
            },
            {
                "title", bookmark.Content
            },
            {
                "tags", bookmark.DefaultTags
            },
            {
                "is_private", true
            },
            {
                "check_disabled", true
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
