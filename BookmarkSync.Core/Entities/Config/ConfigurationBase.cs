using CiT.Common.Validations;

namespace BookmarkSync.Core.Entities.Config;

public class ConfigurationBase
{
    public bool IsValid() => !this.IsAnyNullOrEmpty();
}
