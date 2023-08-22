using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using BookmarkSync.Core;
using BookmarkSync.Core.Entities.Config;
using Newtonsoft.Json;

namespace BookmarkSync.Infrastructure.Services.Mastodon;

public class MastodonService
{
    private static readonly ILogger _logger = Log.ForContext<MastodonService>();
    private readonly HttpClient _client;
    private Instance _instance;
    public MastodonService(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.UserAgent.Add(Meta.UserAgent);
        _client = client;
    }
    public void SetInstance(Instance instance)
    {
        _instance = instance;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _instance.AccessToken);
    }
    public async Task DeleteBookmark(Bookmark bookmark)
    {
        _logger.Debug("Running {Method}", "DeleteBookmark");
        var requestUri = $"{_instance.Uri}/api/v1/statuses/{bookmark.Id}/unbookmark";
        var response = await _client.PostAsync(requestUri, null);
        if (response.IsSuccessStatusCode)
        {
            _logger.Information("Bookmark {BookmarkId} deleted successfully", bookmark.Id);
        }
        else
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    _logger.Warning(
                        "Couldn't delete bookmark due to 403 Forbidden error." +
                        " Does the access token have write:bookmarks permissions?");
                    break;
                default:
                    _logger.Warning(
                        "Couldn't delete bookmark. Status code: {StatusCode}\r\nResponse: {@Response}",
                        response.StatusCode,
                        response.Content);
                    break;
            }
        }
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
}
