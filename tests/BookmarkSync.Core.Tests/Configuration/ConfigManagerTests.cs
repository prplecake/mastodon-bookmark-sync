using System.Configuration;
using System.Text;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Extensions;
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
    [ExpectedException(typeof(ConfigurationErrorsException))]
    public void GetConfigValue_InvalidKey()
    {
        // Arrange
        var expected = "Pinner";

        // Act
        string? actual = _configManager?.GetConfigValue("Apps:Bookmarking:Service");

        // Assert - Exception
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
    [TestMethod]
    public void TestConfigManagerWithIgnoredAccountsConfigured()
    {
        var jsonString = @"
{
    ""App"": {
        ""Bookmarking"": {
            ""Service"": ""LinkAce""
        },
        ""IgnoredAccounts"": [
            ""@prplecake@social.example.com"",
            ""flipper@social.example.com""
        ],
    },
    ""Instances"": [
        ""https://compostintraining.club""
    ]
}";
        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            .Build();

        var configManager = new ConfigManager(config);

        // Assert
        Assert.IsNotNull(configManager.App.IgnoredAccounts);
        foreach (string? account in configManager.App.IgnoredAccounts)
        {
            Assert.IsFalse(account.HasLeadingAt());
        }
    }
}
