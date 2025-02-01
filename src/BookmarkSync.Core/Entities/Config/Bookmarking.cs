namespace BookmarkSync.Core.Entities.Config;

public class Bookmarking : ConfigurationBase
{
    public string? ApiToken { get; set; }
    public string? Service { get; set; }
    public string? ApiVersion { get; set; }
}
