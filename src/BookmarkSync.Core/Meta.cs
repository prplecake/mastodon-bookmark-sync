using System.Net.Http.Headers;

namespace BookmarkSync.Core;

public static class Meta
{
    private const string
        Name = "mastodon-bookmark-sync",
        Version = "2.0";
    public static readonly ProductInfoHeaderValue UserAgent = new(Name, Version);
}
