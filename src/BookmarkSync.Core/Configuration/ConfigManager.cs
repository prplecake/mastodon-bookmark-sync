using System.Configuration;
using BookmarkSync.Core.Entities.Config;
using BookmarkSync.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Core.Configuration;

public interface IConfigManager
{
    App App { get; }
    List<Instance>? Instances { get; }
    string GetConfigValue(string key);
    void SaveToFile();
}
public class ConfigManager : IConfigManager
{
    private static readonly ILogger _logger = Log.ForContext<ConfigManager>();
    public ConfigManager(
        IConfiguration configuration)
    {
        Configuration = configuration;
        App = Configuration.GetSection("App").Get<App>() ?? throw new InvalidOperationException();
        Instances = Configuration.GetSection("Instances").Get<List<Instance>>();

        if (App.IgnoredAccounts is not null) CleanUpIgnoredAccounts();

        if (!App.IsValid())
        {
            _logger.Error("App configuration is invalid");
            throw new InvalidConfigurationException();
        }
    }
    private IConfiguration Configuration { get; }
    public List<Instance>? Instances { get; }
    public App App { get; }
    public string GetConfigValue(string key)
    {
        _logger.Debug("Running {Method} for key: {Key}", "GetConfigValue", key);
        string? value = Configuration[key];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ConfigurationErrorsException(
                $"An invalid key was provided: key: {key}");
        }
        return value;
    }
    public void SaveToFile()
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
        throw new NotImplementedException();
    }
    private void CleanUpIgnoredAccounts()
    {
        App.IgnoredAccounts = (from account in App.IgnoredAccounts select account.RemoveLeadingAt()).ToList();
    }
}
