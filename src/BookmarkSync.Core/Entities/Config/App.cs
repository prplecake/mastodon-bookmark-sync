namespace BookmarkSync.Core.Entities.Config;

public class App : ConfigurationBase
{
    [ConfigRequired] public Bookmarking Bookmarking { get; set; } = null!;
    public List<string>? IgnoredAccounts { get; set; }
    public DateTime LastSynced { get; set; }
}
