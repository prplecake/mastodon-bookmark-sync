using BookmarkSync.Core.Entities;

namespace BookmarkSync.Core.Tests.Entities;

[TestClass]
public class BookmarkTests
{
    [TestMethod]
    public void App_HasProperties()
    {
        // Arrange
        Bookmark obj = new();

        // Assert
        Assert.AreEqual(5, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("Account"));
        Assert.IsTrue(obj.HasProperty("Content"));
        Assert.IsTrue(obj.HasProperty("Id"));
        Assert.IsTrue(obj.HasProperty("Uri"));
        Assert.IsTrue(obj.HasProperty("Visibility"));
    }
}
