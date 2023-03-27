using BookmarkSync.Core.Configuration;
using BookmarkSync.Infrastructure.Services;
using BookmarkSync.Infrastructure.Services.Pinboard;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Infrastructure.Tests.Services;

[TestClass]
public class BookmarkingServiceTests
{
    [TestMethod]
    public void GetBookmarkingService_Pinboard()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Pinboard"
            },
            {
                "App:Bookmarking:ApiToken", "secret:123456789"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager);

        // Assert
        Assert.AreEqual(obj.GetType(), typeof(PinboardBookmarkingService));
        Assert.IsInstanceOfType(obj, typeof(PinboardBookmarkingService));
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GetBookmarkingService_Exception()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Nonsense"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);

        // Act
        var unused = BookmarkingService.GetBookmarkingService(configManager);

        // Assert - Exception
    }
}
