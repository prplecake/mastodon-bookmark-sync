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
        Assert.AreEqual(3, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("ApiToken"));
        Assert.IsTrue(obj.HasProperty("Service"));
        Assert.IsTrue(obj.HasProperty("ApiVersion"));

        Assert.IsTrue(obj.HasMethod("IsValid"));
    }
}
