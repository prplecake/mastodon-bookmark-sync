using System.Net.Http.Headers;
using LinkAce.Api;
using LinkAce.Entites;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;

public class LinkAceBookmarkingService : BookmarkingService, IBookmarkingService
{
    private static readonly ILogger _logger = Log.ForContext<LinkAceBookmarkingService>();
    private readonly string _linkAceUri;
    private readonly LinkAceClient _linkAceClient;
    public LinkAceBookmarkingService(IConfigManager configManager, HttpClient client) : base(client)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException("Missing API token");
        _linkAceUri = configManager.GetConfigValue("App:Bookmarking:LinkAceUri") ??
                      throw new InvalidOperationException("Missing LinkAce Uri");
        ApiUri = $"{_linkAceUri}/api/v1/links";
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
        _linkAceClient = new LinkAceClient(_linkAceUri, Client);
    }
    /// <inheritdoc/>
    public async Task<HttpResponseMessage> Save(Bookmark bookmark)
    {
        // Prep payload
        Link payload = new()
        {
            Url = bookmark.Uri,
            Title = bookmark.Content,
            IsPrivate = true,
            CheckDisabled = true,
            Tags = bookmark.DefaultTags
        };

        // Check for existing bookmarks with the same URL.
        HttpResponseMessage? response;
        var existingLinks = await _linkAceClient.SearchLinksByUrl(bookmark.Uri);
        var existingLink = existingLinks?.Data?.Where(b => b.Url == bookmark.Uri).FirstOrDefault();
        if (existingLink != null)
        {
            // Bookmark exists in LinkAce, try to update.
            _logger.Information("Bookmark {Uri} exists in LinkAce, updating...", bookmark.Uri);
            response = await _linkAceClient.UpdateLinkById(existingLink.Id, payload);
            response.EnsureSuccessStatusCode();
            _logger.Debug("Response status: {StatusCode}", response.StatusCode);
            return response;
        }

        response = await _linkAceClient.CreateLink(payload);
        response.EnsureSuccessStatusCode();
        _logger.Debug("Response status: {StatusCode}", response.StatusCode);
        return response;
    }
}
