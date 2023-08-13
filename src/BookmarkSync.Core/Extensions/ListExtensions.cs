using BookmarkSync.Core.Entities;

namespace BookmarkSync.Core.Extensions;

public static class ListExtensions
{
    public static List<Bookmark>? RemoveAllFromIgnoredAccounts(
        this List<Bookmark>? bookmarks,
        List<string> ignoredAccounts)
    {
        if (bookmarks is null) return bookmarks;
        bookmarks.RemoveAll(b => ignoredAccounts.Contains(b.Account.Name));
        return bookmarks;
    }
}
