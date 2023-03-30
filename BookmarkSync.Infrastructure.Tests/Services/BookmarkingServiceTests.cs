using BookmarkSync.Core.Configuration;
using BookmarkSync.Infrastructure.Services.Bookmarking;
using BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;
using BookmarkSync.Infrastructure.Services.Bookmarking.Pinboard;
using BookmarkSync.Infrastructure.Services.Bookmarking.Raindropio;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Infrastructure.Tests.Services;

[TestClass]
public class BookmarkingServiceTests
{
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
        Assert.AreEqual(typeof(PinboardBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(PinboardBookmarkingService));
    }
    [TestMethod]
    public void GetBookmarkingService_LinkAce()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "LinkAce"
            },
            {
                "App:Bookmarking:ApiToken", "secret:123456789"
            },
            {
                "App:Bookmarking:LinkAceUri", "https://your-linkace-url.com"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager);

        // Assert
        Assert.AreEqual(typeof(LinkAceBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(LinkAceBookmarkingService));
    }
    [TestMethod]
    public void GetBookmarkingService_Raindropio()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Raindropio"
            },
            {
                "App:Bookmarking:ApiToken", "228CCE89-7E7B-4D15-936A-39892AE86110"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager);

        // Assert
        Assert.AreEqual(typeof(RaindropioBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(RaindropioBookmarkingService));
    }
}
