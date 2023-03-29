using System;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BookmarkSync.CLI;

public class Program
{
    private static IConfiguration? _configuration;
    public static int Main(string[] args)
    {
        _configuration = SetupConfiguration(args);
        IConfigManager configManager = new ConfigManager(_configuration);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        try
        {
            Log.Information("Starting host");
            BuildHost(args, configManager).Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.Information("Stopping host");
            configManager.SaveToFile();
            Log.CloseAndFlush();
        }
    }
    public static IHost BuildHost(string[] args, IConfigManager configManager) =>
        new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(configManager);

                services.AddHostedService<BookmarkSyncService>();
            })
            .UseSerilog()
            .Build();
    private static IConfiguration SetupConfiguration(string[] args) => new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();
}
