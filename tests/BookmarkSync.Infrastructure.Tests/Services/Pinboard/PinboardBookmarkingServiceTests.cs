using System.Net;
using BookmarkSync.Core.Entities;
using Moq;
using Moq.Protected;

namespace BookmarkSync.Infrastructure.Tests.Services.Pinboard;

[TestClass]
public class PinboardBookmarkingServiceTests
{
    [TestInitialize]
    public void Init()
    {
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Pinboard"
            },
            {
                "App:Bookmarking:ApiToken", "token123456"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
        IConfigManager configManager = new ConfigManager(configuration);
        _configManager = configManager;
    }
    private IConfigManager _configManager;
    private readonly string _pinboardUri = "https://api.pinboard.in";
    [TestMethod]
    public async Task Pinboard_Save_Success()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString()
                        .StartsWith(_pinboardUri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

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
    public async Task Pinboard_Save_Success_Long_Title()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString()
                        .StartsWith(_pinboardUri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);
        var bookmarkingService = BookmarkingService.GetBookmarkingService(_configManager, httpClient);
        var bookmark = new Bookmark
        {
            Uri = "https://example.com",
            // ReSharper disable StringLiteralTypo
            Content =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus suscipit, urna quis consectetur sodales, ante enim auctor leo, ut tincidunt nibh tortor ac magna. Aenean suscipit tincidunt tincidunt. Curabitur et eros elit. Duis consequat felis justo, sit amet semper est scelerisque ac. Aenean ac. "
            // ReSharper restore StringLiteralTypo
        };

        // Act
        var result = await bookmarkingService.Save(bookmark);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccessStatusCode);
    }
}
