using BookmarkSync.Core.Entities.Config;

namespace BookmarkSync.Core.Tests.Entities.Config;

[TestClass]
public class BookmarkingTests
{
    [TestMethod]
    public void Bookmarking_HasProperties()
    {
        // Arrange
        Bookmarking obj = new();

        // Assert
        Assert.AreEqual(2, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("ApiToken"));
        Assert.IsTrue(obj.HasProperty("Service"));

        Assert.IsTrue(obj.HasMethod("IsValid"));
    }
}
