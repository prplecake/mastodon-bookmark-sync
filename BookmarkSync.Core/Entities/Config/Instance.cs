namespace BookmarkSync.Core.Entities.Config;

public class Instance : ConfigurationBase
{
    public string AccessToken { get; set; }
    public bool DeleteBookmarks { get; set; }
    public string Uri { get; set; }
    /// <inheritdoc />
    public override string ToString() => Uri;
}
