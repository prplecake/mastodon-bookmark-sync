using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using BookmarkSync.Core.Entities.Config;
using CiT.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BookmarkSync.Core.Configuration;

public interface IConfigManager
{
    IConfiguration Configuration { get; set; }
    Instance[] Instances { get; set; }
    App App { get; set; }
    string GetConfigValue(string key);
    void SaveToFile();
}
public class ConfigManager : IConfigManager
{
    public ConfigManager(
        IConfiguration configuration)
    {
        Configuration = configuration;
        List<Instance> instance = Configuration.GetSection("Instances").Get<List<Instance>>();
        App app = Configuration.GetSection("App").Get<App>();
        var instances = Configuration.GetSection("Instances").Get<Instance[]>();
        App = app;

        if (!App.IsValid())
        {
            throw new InvalidConfigurationException();
        }
    }
    public IConfiguration Configuration { get; set; }
    public Instance[] Instances { get; set; }
    public App App { get; set; }
    public string GetConfigValue(string key)
    {
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
    }
}
