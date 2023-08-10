namespace BookmarkSync.Infrastructure.Services.Bookmarking.LinkAce;

public class LinkAceApiSearchResponse
{
    public int CurrentPage { get; set; }
    public List<LinkAceBookmark>? Data { get; set; }
    public string? FirstPageUrl { get; set; }
    public int? From { get; set; }
    public int LastPage { get; set; }
    public string? LastPageUrl { get; set; }
    public string? NextPageUrl { get; set; }
    public string Path { get; set; }
    public string PerPage { get; set; }
    public string? PreviousPageUrl { get; set; }
    public int? To { get; set; }
    public int Total { get; set; }
}
public class LinkAceBookmark
{
    public bool CheckDisabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? Description { get; set; }
    public string Icon { get; set; }
    public int Id { get; set; }
    public bool IsPrivate { get; set; }
    public int Status { get; set; }
    public string Title { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Url { get; set; }
    public int UserId { get; set; }
}
