﻿using System;
using System.Threading.Tasks;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Core.Interfaces;
using BookmarkSync.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookmarkSync.CLI;

public class Program
{
    private static IConfiguration? _configuration;
    public async static Task Main(string[] args)
    {
        _configuration = SetupConfiguration(args);
        IConfigManager configManager = new ConfigManager(_configuration);

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton(configManager);

                services.AddTransient<IBookmarkingService, PinboardBookmarkingService>();
            })
            .Build();
        await host.StartAsync();
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

        // Do work here
        Console.WriteLine("Hello, world!");

        // Shutdown
        configManager.SaveToFile();

        lifetime.StopApplication();
        await host.WaitForShutdownAsync();
    }
    private static IConfiguration SetupConfiguration(string[] args) => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();
}
