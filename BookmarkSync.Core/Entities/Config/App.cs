using CiT.Common.Attributes;

namespace BookmarkSync.Core.Entities.Config;

public class App : ConfigurationBase
{
    [ConfigRequired] public Bookmarking Bookmarking { get; set; } = null!;
    public DateTime LastSynced { get; set; }
}
