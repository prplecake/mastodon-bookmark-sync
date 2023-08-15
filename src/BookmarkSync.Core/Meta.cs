using System.Net.Http.Headers;
using System.Reflection;

namespace BookmarkSync.Core;

public static class Meta
{
    private static readonly Assembly? Assembly = Assembly.GetEntryAssembly();
    private static readonly Type? GitVersionInformationType = Assembly?.GetType("GitVersionInformation");
    public const string Name = "mastodon-bookmark-sync";
    public static readonly string
        Version = GitVersionInformationType?.GetField("SemVer")?.GetValue(null)?.ToString() ?? "2.0";
    public static readonly ProductInfoHeaderValue UserAgent = new(Name, Version);
}
