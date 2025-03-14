using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Web;
using BookmarkSync.Core.Json;
using Newtonsoft.Json;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;

public class LinkAceBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<LinkAceBookmarkingService>();
    private readonly string _linkAceUri;
    public LinkAceBookmarkingService(IConfigManager configManager, HttpClient client) : base(client)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException("Missing API token");
        _linkAceUri = configManager.GetConfigValue("App:Bookmarking:LinkAceUri") ??
                      throw new InvalidOperationException("Missing LinkAce Uri");
        string version = configManager.App.Bookmarking.ApiVersion ?? "v2";
        ApiUri = $"{_linkAceUri}/api/{version}";
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
    }
    /// <inheritdoc/>
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
                "tags", bookmark.DefaultTags
            },
            {
                "is_private", true
            },
            {
                "check_disabled", true
            }
        };
        var stringContent = new StringContentWithoutCharset(JsonConvert.SerializeObject(payload),
            MediaTypeNames.Application.Json);

        // Check for existing bookmarks with the same URL.
        var uri = $"{ApiUri}/search/links?query={bookmark.Uri}&limit=-1";
        var linksResponse = await Client.GetAsync(uri);
        linksResponse.EnsureSuccessStatusCode();
        string responseContent = await linksResponse.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<LinkAceApiSearchResponse>(responseContent,
            new JsonSerializerSettings
            {
                ContractResolver = SnakeCaseContractResolver.Instance
            });

        HttpResponseMessage? response;
        var existingLink = responseObj?.Data?.Where(b => b.Url == bookmark.Uri).FirstOrDefault();
        if (existingLink != null)
        {
            // Bookmark exists in LinkAce, try to update.
            _logger.Information("Bookmark {Uri} exists in LinkAce, updating...", bookmark.Uri);
            response = await Client.PatchAsync($"{ApiUri}/links/{existingLink.Id}", stringContent);
            response.EnsureSuccessStatusCode();
            _logger.Debug("Response status: {StatusCode}", response.StatusCode);
            return response;
        }

        response = await Client.PostAsync($"{ApiUri}/links", stringContent);
        response.EnsureSuccessStatusCode();
        _logger.Debug("Response status: {StatusCode}", response.StatusCode);
        return response;
    }
}
