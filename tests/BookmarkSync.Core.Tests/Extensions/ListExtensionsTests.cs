using BookmarkSync.Core.Extensions;

namespace BookmarkSync.Core.Tests.Extensions;

[TestClass]
public class ListExtensionsTests
{
    [TestMethod]
    public void RemoveAllFromIgnoredAccounts_Empty_IgnoredAccounts_Returns_Unchanged_Bookmarks_List()
    {
        // Arrange
        var bookmarks = new List<Bookmark>() { NewBookmark(), NewBookmark(), NewBookmark()};

        // Act
        var result = bookmarks.RemoveAllFromIgnoredAccounts(new List<string>());

        // Assert
        CollectionAssert.AreEqual(bookmarks, result);
    }
    [TestMethod]
    public void RemoveAllFromIgnoredAccounts_Removes_Expected()
    {
        // Arrange
        var bookmarks = new List<Bookmark>() { NewBookmark(), NewBookmark(), NewBookmark()};
        bookmarks.AddRange(new[] { NewBookmark("@remove@example.com"), NewBookmark("remove@example.com") });

        Assert.AreEqual(5, bookmarks.Count);

        // Act
        var result = bookmarks.RemoveAllFromIgnoredAccounts(new List<string>() { "@remove@example.com" });

        // Assert
        CollectionAssert.AreEqual(bookmarks, result);
    }
    [TestMethod]
    public void RemoveAllFromIgnoredAccounts_Null_Bookmarks_List_Returns_Same()
    {
        // Arrange
        List<Bookmark>? bookmarks = null;

        // Act
        var result = bookmarks.RemoveAllFromIgnoredAccounts(new List<string>());

        // Assert
        Assert.IsNull(result);
    }
    private Bookmark NewBookmark(string? account = null)
    {
        return new Bookmark
        { Account = new Account(account ?? "@test@example.com"),
          Content = "Good post",
          Id = TestHelpers.RandomString(16) };
    }
}
