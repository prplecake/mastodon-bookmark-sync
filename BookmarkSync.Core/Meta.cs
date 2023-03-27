using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;

namespace BookmarkSync.Core;

public class Meta
{
    public const string Name = "mastodon-bookmark-sync";
    public const string Version = "2.0";
    public static readonly ProductInfoHeaderValue UserAgent = new(Name, Version);
}
