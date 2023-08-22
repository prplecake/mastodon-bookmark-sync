using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.Briefkasten;

public class BriefkastenBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<BriefkastenBookmarkingService>();
    public BriefkastenBookmarkingService(IConfigManager configManager, HttpClient client) : base(client)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException("Missing API token");
        string briefkastenUri = configManager.GetConfigValue("App:Bookmarking:BriefkastenUri") ??
                                throw new InvalidOperationException("Missing Briefkasten Uri");
        ApiUri = $"{briefkastenUri}/api/bookmarks";
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
                "userId", ApiToken
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
