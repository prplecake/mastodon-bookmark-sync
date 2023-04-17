namespace BookmarkSync.Core.Tests.Entities;

[TestClass]
public class BookmarkTests
{
    [TestMethod]
    public void Bookmark_HasProperties()
    {
        // Arrange
        Bookmark obj = new();

        // Assert
        Assert.AreEqual(6, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("Account"));
        Assert.IsTrue(obj.HasProperty("Content"));
        Assert.IsTrue(obj.HasProperty("Id"));
        Assert.IsTrue(obj.HasProperty("Uri"));
        Assert.IsTrue(obj.HasProperty("Visibility"));
        Assert.IsTrue(obj.HasProperty("DefaultTags"));
    }
}
