using System.Net;
using BookmarkSync.Core.Entities;
using Moq;
using Moq.Protected;

namespace BookmarkSync.Infrastructure.Tests.Services.Linkding;

[TestClass]
public class LinkdingBookmarkingServiceTests
{
    [TestInitialize]
    public void Init()
    {
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "linkding"
            },
            {
                "App:Bookmarking:ApiToken", "token123456"
            },
            {
                "App:Bookmarking:LinkdingUri", "https://links.example.com"
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
    public void Linkding_Save_Success()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post && r.RequestUri.ToString()
                        .StartsWith(_configManager.GetConfigValue("App:Bookmarking:LinkdingUri"))),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);
        var bookmarkingService = BookmarkingService.GetBookmarkingService(_configManager, httpClient);
        var bookmark = new Bookmark
        {
            Uri = "https://example.com"
        };

        // Act
        var result = bookmarkingService.Save(bookmark).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccessStatusCode);
    }
}
