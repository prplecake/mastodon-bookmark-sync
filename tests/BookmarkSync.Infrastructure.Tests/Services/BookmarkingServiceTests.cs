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
        HttpClient httpClient = new();

        // Act
        var unused = BookmarkingService.GetBookmarkingService(configManager, httpClient);

        // Assert - Exception
    }
    [TestMethod]
    public void GetBookmarkingService_Briefkasten()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Briefkasten"
            },
            {
                "App:Bookmarking:ApiToken", "ABC123DEF456"
            },
            {
                "App:Bookmarking:BriefkastenUri", "https://briefkastenhq.com"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);
        HttpClient httpClient = new();

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager, httpClient);

        // Assert
        Assert.AreEqual(typeof(BriefkastenBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(BriefkastenBookmarkingService));
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
        HttpClient httpClient = new();

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager, httpClient);

        // Assert
        Assert.AreEqual(typeof(LinkAceBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(LinkAceBookmarkingService));
    }
    [TestMethod]
    public void GetBookmarkingService_Linkding()
    {
        // Arrange
        var config = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "linkding"
            },
            {
                "App:Bookmarking:ApiToken", "3781368521fd4fae365dac100f28dbce533a4f9a"
            },
            {
                "App:Bookmarking:LinkdingUri", "https://your-linkace-url.com"
            }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();

        IConfigManager configManager = new ConfigManager(configuration);
        HttpClient httpClient = new();

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager, httpClient);

        // Assert
        Assert.AreEqual(typeof(LinkdingBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(LinkdingBookmarkingService));
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
        HttpClient httpClient = new();

        // Act
        var obj = BookmarkingService.GetBookmarkingService(configManager, httpClient);

        // Assert
        Assert.AreEqual(typeof(PinboardBookmarkingService), obj.GetType());
        Assert.IsInstanceOfType(obj, typeof(PinboardBookmarkingService));
    }
}
