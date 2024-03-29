using System.Configuration;
using System.Text;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Extensions;
using CiT.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Core.Tests.Configuration;

[TestClass]
public class ConfigManagerTests
{
    [TestInitialize]
    public void Init()
    {
        var yamlString = @"
App:
  Bookmarking:
    Service: Pinboard
Instances:
  - Uri: https://compostintraining.club
";
        _config = new ConfigurationBuilder()
            .AddYamlStream(new MemoryStream(Encoding.UTF8.GetBytes(yamlString)))
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
        var yamlString = @"
App:
  Bookmarking:
    Service: LinkAce
  IgnoredAccounts:
    - ""@prplecake@social.example.com""
    - flipper@social.example.com
Instances: 
  - Uri: https://compostintraining.club
";
        var config = new ConfigurationBuilder()
            .AddYamlStream(new MemoryStream(Encoding.UTF8.GetBytes(yamlString)))
            .Build();

        var configManager = new ConfigManager(config);

        // Assert
        Assert.IsNotNull(configManager.App.IgnoredAccounts);
        foreach (string? account in configManager.App.IgnoredAccounts)
        {
            Assert.IsFalse(account.HasLeadingAt());
        }
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidConfigurationException))]
    public void Test_ConfigManager_With_Invalid_Config()
    {
        var yamlString = @"
App:
  IgnoredAccounts:
    - ""@prplecake@social.example.com""
    - flipper@social.example.com
Instances:
  - Uri: https://compostintraining.club
";
        var config = new ConfigurationBuilder()
            .AddYamlStream(new MemoryStream(Encoding.UTF8.GetBytes(yamlString)))
            .Build();

        var configManager = new ConfigManager(config);

        // Assert - Exception: InvalidConfigurationException
    }
    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void ConfigManager_SaveToFile_Throws_NotImplementedException()
    {
        // Act
        _configManager?.SaveToFile();

        // Assert - Exception: NotImplementedException
    }
}
