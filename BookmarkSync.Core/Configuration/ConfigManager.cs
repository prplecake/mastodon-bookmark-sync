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
    App? App { get; set; }
    IConfiguration Configuration { get; set; }
    List<Instance>? Instances { get; set; }
    string GetConfigValue(string key);
    void SaveToFile();
}
public class ConfigManager : IConfigManager
{
    public ConfigManager(
        IConfiguration configuration)
    {
        Configuration = configuration;
        App = Configuration.GetSection("App").Get<App>();
        Instances = Configuration.GetSection("Instances").Get<List<Instance>>();

        if (App is not null && !App.IsValid())
        {
            throw new InvalidConfigurationException();
        }
    }
    public IConfiguration Configuration { get; set; }
    public List<Instance>? Instances { get; set; }
    public App? App { get; set; }
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
