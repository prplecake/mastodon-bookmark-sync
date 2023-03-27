using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Entities.Config;
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
        _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("mastodon-bookmark-sync",
            FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _instance.AccessToken);
    }
    public async Task<List<Bookmark>?> GetBookmarks()
    {
        _logger.Debug("Running {Method}", "GetBookmarks");
        return null;
    }
    public async Task DeleteBookmark(Bookmark bookmark)
    {
        _logger.Debug("Running {Method}", "DeleteBookmark");
        throw new NotImplementedException();
    }
}
