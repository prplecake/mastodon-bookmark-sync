using System.Net;
using System.Net.Mime;
using System.Text;
using BookmarkSync.Core.Entities;
using Moq;
using Moq.Protected;

namespace BookmarkSync.Infrastructure.Tests.Services.LinkAce;

[TestClass]
public class LinkAceBookmarkingServiceTests
{
    [TestInitialize]
    public void Init()
    {
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "LinkAce"
            },
            {
                "App:Bookmarking:ApiToken", "token123456"
            },
            {
                "App:Bookmarking:LinkAceUri", "https://links.example.com/"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
        IConfigManager configManager = new ConfigManager(configuration);
        _configManager = configManager;
    }
    private IConfigManager _configManager;
    [TestMethod]
    public async Task LinkAce_Save_Success()
    {
        // Arrange
        var getHttpResponse = new HttpResponseMessage();
        getHttpResponse.StatusCode = HttpStatusCode.OK;
        getHttpResponse.Content = new StringContent(@"
{
    ""current_page"": 1,
        ""data"": [],
        ""first_page_url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
        ""from"": null,
        ""last_page"": 1,
        ""last_page_url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
        ""links"": [
        {
            ""url"": null,
            ""label"": ""&laquo; Previous"",
            ""active"": false
        },
        {
            ""url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
            ""label"": ""1"",
            ""active"": true
        },
        {
            ""url"": null,
            ""label"": ""Next &raquo;"",
            ""active"": false
        }
        ],
        ""next_page_url"": null,
        ""path"": ""https://links.fminus.co/api/v1/search/links"",
        ""per_page"": 24,
        ""prev_page_url"": null,
        ""to"": null,
        ""total"": 0
    }
", Encoding.UTF8, MediaTypeNames.Application.Json);
        var postHttpResponse = new HttpResponseMessage();
        postHttpResponse.StatusCode = HttpStatusCode.OK;

        Mock<HttpMessageHandler> mockHandler = new();
        // GET
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString()
                        .StartsWith(_configManager.GetConfigValue("App:Bookmarking:LinkAceUri"))),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(getHttpResponse);
        // POST
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post && r.RequestUri.ToString()
                        .StartsWith(_configManager.GetConfigValue("App:Bookmarking:LinkAceUri"))),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(postHttpResponse);

        var httpClient = new HttpClient(mockHandler.Object);
        var bookmarkingService = BookmarkingService.GetBookmarkingService(_configManager, httpClient);
        var bookmark = new Bookmark
        {
            Uri = "https://example.com"
        };

        // Act
        var result = await bookmarkingService.Save(bookmark);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccessStatusCode);
    }
    [TestMethod]
    public async Task LinkAce_Save_Success_Link_Exists()
    {
        // Arrange
        var getHttpResponse = new HttpResponseMessage();
        getHttpResponse.StatusCode = HttpStatusCode.OK;
        getHttpResponse.Content = new StringContent(@"
{
    ""current_page"": 1,
    ""data"": [
        {
            ""id"": 1098,
            ""user_id"": 1,
            ""url"": ""https://photos.jrgnsn.net/album/classic-atreus-build"",
            ""title"": ""Classic Atreus Build - jphotos"",
            ""description"": ""Pictures from my Atreus keyboard build"",
            ""is_private"": false,
            ""created_at"": ""2020-09-21T21:39:11.000000Z"",
            ""updated_at"": ""2023-06-09T16:16:37.000000Z"",
            ""deleted_at"": null,
            ""icon"": ""link"",
            ""status"": 1,
            ""check_disabled"": false,
            ""thumbnail"": null,
            ""tags"": [
                {
                    ""id"": 603,
                    ""user_id"": 1,
                    ""name"": ""atreus"",
                    ""is_private"": false,
                    ""created_at"": ""2023-06-09T16:16:36.000000Z"",
                    ""updated_at"": ""2023-06-09T16:16:36.000000Z"",
                    ""deleted_at"": null,
                    ""pivot"": {
                        ""link_id"": 1098,
                        ""tag_id"": 603
                    }
                },
                {
                    ""id"": 234,
                    ""user_id"": 1,
                    ""name"": ""diy"",
                    ""is_private"": false,
                    ""created_at"": ""2023-06-09T16:07:30.000000Z"",
                    ""updated_at"": ""2023-06-09T16:07:30.000000Z"",
                    ""deleted_at"": null,
                    ""pivot"": {
                        ""link_id"": 1098,
                        ""tag_id"": 234
                    }
                },
                {
                    ""id"": 605,
                    ""user_id"": 1,
                    ""name"": ""keyboard"",
                    ""is_private"": false,
                    ""created_at"": ""2023-06-09T16:16:37.000000Z"",
                    ""updated_at"": ""2023-06-09T16:16:37.000000Z"",
                    ""deleted_at"": null,
                    ""pivot"": {
                        ""link_id"": 1098,
                        ""tag_id"": 605
                    }
                }
            ]
        }
    ],
    ""first_page_url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
    ""from"": 1,
    ""last_page"": 1,
    ""last_page_url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
    ""links"": [
        {
            ""url"": null,
            ""label"": ""&laquo; Previous"",
            ""active"": false
        },
        {
            ""url"": ""https://links.fminus.co/api/v1/search/links?page=1"",
            ""label"": ""1"",
            ""active"": true
        },
        {
            ""url"": null,
            ""label"": ""Next &raquo;"",
            ""active"": false
        }
    ],
    ""next_page_url"": null,
    ""path"": ""https://links.fminus.co/api/v1/search/links"",
    ""per_page"": 24,
    ""prev_page_url"": null,
    ""to"": 1,
    ""total"": 1
}
", Encoding.UTF8, MediaTypeNames.Application.Json);
        var postHttpResponse = new HttpResponseMessage();
        postHttpResponse.StatusCode = HttpStatusCode.OK;

        Mock<HttpMessageHandler> mockHandler = new();
        // GET
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString()
                        .StartsWith(_configManager.GetConfigValue("App:Bookmarking:LinkAceUri"))),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(getHttpResponse);
        // POST
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Patch && r.RequestUri.ToString()
                        .StartsWith(_configManager.GetConfigValue("App:Bookmarking:LinkAceUri"))),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(postHttpResponse);

        var httpClient = new HttpClient(mockHandler.Object);
        var bookmarkingService = BookmarkingService.GetBookmarkingService(_configManager, httpClient);
        var bookmark = new Bookmark
        {
            Uri = "https://photos.jrgnsn.net/album/classic-atreus-build"
        };

        // Act
        var result = await bookmarkingService.Save(bookmark);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccessStatusCode);
    }
}
