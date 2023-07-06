using BookmarkSync.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Core.Tests.Configuration;

[TestClass]
public class ConfigManagerTests
{
    [TestInitialize]
    public void Init()
    {
        var inMemoryConfig = new Dictionary<string, string?>
        {
            {
                "App:Bookmarking:Service", "Pinboard"
            },
            {
                "Instances:0:Uri", "https://compostintraining.club"
            }
        };
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfig)
            .Build();

        _configManager = new ConfigManager(_config);
    }
    private IConfigManager? _configManager;
    private IConfiguration? _config;
    [TestMethod]
    public void ConfigManager_HasProperties()
    {
        // Assert
        Assert.AreEqual(2, _configManager?.PropertyCount());
        Assert.IsTrue(_configManager?.HasProperty("Instances"));
        Assert.IsTrue(_configManager?.HasProperty("App"));

        Assert.IsTrue(_configManager?.HasMethod("GetConfigValue"));
    }
    [TestMethod]
    public void GetConfigValue_Success()
    {
        // Arrange
        var expected = "Pinboard";

        // Act
        string? actual = _configManager?.GetConfigValue("App:Bookmarking:Service");

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
