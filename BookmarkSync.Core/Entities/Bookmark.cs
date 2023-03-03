namespace BookmarkSync.Core.Entities;

public class Bookmark
{
    public Account? Account { get; set; }
    public string? Content { get; set; }
    public string? Id { get; set; }
    public string? Uri { get; set; }
    public string? Visibility { get; set; }
}
