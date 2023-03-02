namespace BookmarkSync.Core.Entities;

public class Account
{
    public string Name { get; set; }
    /// <inheritdoc />
    public override string ToString() => Name;
}
