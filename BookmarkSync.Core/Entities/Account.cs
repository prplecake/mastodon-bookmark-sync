using Newtonsoft.Json;

namespace BookmarkSync.Core.Entities;

public class Account
{
    [JsonProperty("acct")] public string? Name { get; set; }
    public Account(string name)
    {
        Name = name;
    }
    public Account() { }
    /// <inheritdoc />
    public override string ToString() => Name ?? GetType().Name;
}
