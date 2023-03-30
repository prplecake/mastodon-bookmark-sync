using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace BookmarkSync.Infrastructure.Services.Bookmarking.Raindropio;

public class RaindropioBookmarkingService : BookmarkingService, IBookmarkingService
{
    private const int RaindropioTitleMaxLength = 1000;
    private static readonly ILogger _logger = Log.ForContext<RaindropioBookmarkingService>();
    public RaindropioBookmarkingService(IConfigManager configManager)
    {
        ApiToken = configManager.App.Bookmarking.ApiToken ?? throw new InvalidOperationException();
        ApiUri = "https://api.raindrop.io/rest/v1/raindrop";
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
    }
    /// <inheritdoc />
    public async Task<HttpResponseMessage> Save(Bookmark bookmark)
    {
        Dictionary<string, object> payload = new()
        {
            {
                "link", bookmark.Uri
            },
            {
                "title", bookmark.Content
            },
            {
                "tags", bookmark.DefaultTags
            }
        };
        if (bookmark.Content.Length >= RaindropioTitleMaxLength)
        {
            payload["title"] = bookmark.Content[..RaindropioTitleMaxLength];
            payload["excerpt"] = bookmark.Content;
        }
        var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var response = await Client.PostAsync(ApiUri, stringContent);
        response.EnsureSuccessStatusCode();
        _logger.Debug("Response status: {StatusCode}", response.StatusCode);
        return response;
    }
}
