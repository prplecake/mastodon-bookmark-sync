using BookmarkSync.Core;
using BookmarkSync.Core.Configuration;
using BookmarkSync.Infrastructure.Services.Bookmarking;
using BookmarkSync.Infrastructure.Services.Mastodon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BookmarkSync.CLI;

public static class Program
{
    private static IConfiguration? _configuration;
    public static int Main(string[] args)
    {
        if (args.Contains("version"))
        {
            Console.WriteLine("{0} {1}", Meta.Name, Meta.Version);
            return 0;
        }
        _configuration = SetupConfiguration(args);
        IConfigManager configManager = new ConfigManager(_configuration);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        try
        {
            Log.Information("Starting host");
            Log.Information("{Name} {Version}", Meta.Name, Meta.Version);
            BuildHost(configManager).Run();
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
            // configManager.SaveToFile();
            Log.CloseAndFlush();
        }
    }
    private static IHost BuildHost(IConfigManager configManager) =>
        new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(configManager);
                services.AddHostedService<BookmarkSyncService>();
                services.AddHttpClient<MastodonService>();
            })
            .UseSerilog()
            .Build();
    private static IConfiguration SetupConfiguration(string[] args) => new ConfigurationBuilder()
        .AddYamlFile("config.yaml", false, true)
        .AddYamlFile($"config.{Environment.GetEnvironmentVariable("MBS_ENVIRONMENT") ?? "Production"}.yaml", false, true)
        .AddCommandLine(args)
        .Build();
}
