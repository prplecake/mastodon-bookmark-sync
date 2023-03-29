using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using BookmarkSync.Core;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Entities.Config;
using Newtonsoft.Json;
using Serilog;

namespace BookmarkSync.Infrastructure.Services.Mastodon;

public class ApiClient
{
    private readonly HttpClient _client = new();
    private readonly Instance _instance;
    private static readonly ILogger _logger = Log.ForContext<ApiClient>();
    public ApiClient(Instance instance)
    {
        _instance = instance;
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _client.DefaultRequestHeaders.UserAgent.Add(Meta.UserAgent);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _instance.AccessToken);
    }
    public async Task<List<Bookmark>?> GetBookmarks()
    {
        _logger.Debug("Running {Method}", "GetBookmarks");
        var requestUri = $"{_instance.Uri}/api/v1/bookmarks";
        var response = await _client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<List<Bookmark>>(responseContent);

        return obj;
    }
    public async Task DeleteBookmark(Bookmark bookmark)
    {
        _logger.Debug("Running {Method}", "DeleteBookmark");
        var requestUri = $"{_instance.Uri}/api/v1/statuses/{bookmark.Id}/unbookmark";
        var response = await _client.PostAsync(requestUri, null);
        response.EnsureSuccessStatusCode();
        _logger.Information("Bookmark {BookmarkId} deleted successfully", bookmark.Id);
    }
}
