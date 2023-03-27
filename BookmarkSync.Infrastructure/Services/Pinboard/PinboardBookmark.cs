namespace BookmarkSync.Infrastructure.Services.Pinboard;

public class PinboardBookmark
{
    public string Description { get; set; }
    public string ExtendedDescription { get; set; } // TODO: check this name
    public bool Shared { get; set; } = false;
    public string[]? Tags { get; set; }
    public string Uri { get; set; }
    public string? GetFormattedTags()
        => Tags != null ? string.Join(" ", Tags) : null;
}
