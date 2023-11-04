using System.Net;
using System.Net.Mime;
using System.Text;
using BookmarkSync.Core.Entities;
using BookmarkSync.Core.Entities.Config;
using BookmarkSync.Infrastructure.Services.Mastodon;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;

namespace BookmarkSync.Infrastructure.Tests.Services.Mastodon;

[TestClass]
public class MastodonServiceTests
{
    [AssemblyInitialize]
    public static void ConfigureGlobalLogger(TestContext testContext)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.TestCorrelator().CreateLogger();
    }
    [TestMethod]
    public async Task MastodonService_Delete_Bookmark_Success()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;
        var instance = new Instance
        {
            Uri = "https://localhost:3000"
        };

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post && r.RequestUri.ToString().StartsWith(instance.Uri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);

        using (TestCorrelator.CreateContext())
        {
            var mastodonService = new MastodonService(httpClient);
            mastodonService.SetInstance(instance);

            // Act
            await mastodonService.DeleteBookmark(new Bookmark
            {
                Id = "123456"
            });

            // Assert
            var logs = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.IsNotNull(logs);
            var logEvents = logs.ToList();
            Assert.AreEqual(2, logEvents.Count);
            var logMessage = logEvents.Where(e => e.Level == LogEventLevel.Information).ToList()[0];
            Assert.AreEqual(LogEventLevel.Information, logMessage.Level);
            Assert.IsTrue(logMessage.MessageTemplate.ToString().Contains("deleted successfully"));
        }
    }
    [TestMethod]
    public async Task MastodonService_Failed_To_Delete_Bookmark()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.BadRequest;
        var instance = new Instance
        {
            Uri = "https://localhost:3000"
        };

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post && r.RequestUri.ToString().StartsWith(instance.Uri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);

        using (TestCorrelator.CreateContext())
        {
            var mastodonService = new MastodonService(httpClient);
            mastodonService.SetInstance(instance);

            // Act
            await mastodonService.DeleteBookmark(new Bookmark
            {
                Id = "123456"
            });

            // Assert
            var logs = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.IsNotNull(logs);
            var logEvents = logs.ToList();
            Assert.AreEqual(2, logEvents.Count);
            var logMessage = logEvents.Where(e => e.Level == LogEventLevel.Warning).ToList()[0];
            Assert.AreEqual(LogEventLevel.Warning, logMessage.Level);
            Assert.IsTrue(logMessage.MessageTemplate.ToString().Contains("Couldn't delete bookmark"));
            Assert.IsTrue(logMessage.Properties.TryGetValue("StatusCode", out var statusCodeValue));
            var statusCode = (HttpStatusCode?)((ScalarValue)statusCodeValue).Value;
            Assert.AreEqual(httpResponse.StatusCode, statusCode);
        }
    }
    [TestMethod]
    public async Task MastodonService_Failed_To_Delete_Bookmark_403_Forbidden()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.Forbidden;
        var instance = new Instance
        {
            Uri = "https://localhost:3000"
        };

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post && r.RequestUri.ToString().StartsWith(instance.Uri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);

        using (TestCorrelator.CreateContext())
        {
            var mastodonService = new MastodonService(httpClient);
            mastodonService.SetInstance(instance);

            // Act
            await mastodonService.DeleteBookmark(new Bookmark
            {
                Id = "123456"
            });

            // Assert
            var logs = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.IsNotNull(logs);
            var logEvents = logs.ToList();
            Assert.AreEqual(2, logEvents.Count);
            var logMessage = logEvents.Where(e => e.Level == LogEventLevel.Warning).ToList()[0];
            Assert.AreEqual(LogEventLevel.Warning, logMessage.Level);
            Assert.IsTrue(logMessage.MessageTemplate.ToString().Contains("403 Forbidden"));
        }
    }
    [TestMethod]
    public async Task MastodonService_GetBookmarks_Success_Empty_List()
    {
        // Arrange
        var expectedBookmarkList = new List<Bookmark>();
        string json = JsonConvert.SerializeObject(expectedBookmarkList);

        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;
        httpResponse.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        var instance = new Instance
        {
            Uri = "https://localhost:3000"
        };

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString().StartsWith(instance.Uri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);

        using (TestCorrelator.CreateContext())
        {
            var mastodonService = new MastodonService(httpClient);
            mastodonService.SetInstance(instance);

            // Act
            var result = await mastodonService.GetBookmarks();

            // Assert
            var logs = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.IsNotNull(logs);
            var logEvents = logs.ToList();
            Assert.AreEqual(1, logEvents.Count);
            var logMessage = logEvents[0];
            Assert.AreEqual(LogEventLevel.Debug, logMessage.Level);

            Assert.AreEqual(0, result.Count);
        }
    }
    [TestMethod]
    public async Task MastodonService_GetBookmarks_Success_Non_Empty_List()
    {
        // Arrange
        var expectedBookmarkList = new List<Bookmark>
        {
            new(),
            new()
        };
        string json = JsonConvert.SerializeObject(expectedBookmarkList);

        var httpResponse = new HttpResponseMessage();
        httpResponse.StatusCode = HttpStatusCode.OK;
        httpResponse.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        var instance = new Instance
        {
            Uri = "https://localhost:3000"
        };

        Mock<HttpMessageHandler> mockHandler = new();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get && r.RequestUri.ToString().StartsWith(instance.Uri)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var httpClient = new HttpClient(mockHandler.Object);

        using (TestCorrelator.CreateContext())
        {
            var mastodonService = new MastodonService(httpClient);
            mastodonService.SetInstance(instance);

            // Act
            var result = await mastodonService.GetBookmarks();

            // Assert
            var logs = TestCorrelator.GetLogEventsFromCurrentContext();
            Assert.IsNotNull(logs);
            var logEvents = logs.ToList();
            Assert.AreEqual(1, logEvents.Count);
            var logMessage = logEvents[0];
            Assert.AreEqual(LogEventLevel.Debug, logMessage.Level);

            Assert.AreEqual(2, result.Count);
        }
    }
}
