using BookmarkSync.Core.Utilities;

namespace BookmarkSync.Core.Entities;

public class Bookmark
{
    public Account? Account { get; set; }
    private string? _content;
    public string? Content
    {
        get => _content != null ? HtmlUtilities.ConvertToPlainText(_content) : "";
        set => _content = value;
    }
    public string? Id { get; set; }
    public string? Uri { get; set; }
    public string? Visibility { get; set; }
}
