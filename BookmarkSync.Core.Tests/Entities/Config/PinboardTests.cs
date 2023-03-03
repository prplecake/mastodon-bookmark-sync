using BookmarkSync.Core.Entities.Config;

namespace BookmarkSync.Core.Tests.Entities.Config;

[TestClass]
public class PinboardTests
{
    [TestMethod]
    public void Pinboard_HasProperties()
    {
        // Arrange
        Pinboard obj = new();

        // Assert
        Assert.AreEqual(0, obj.PropertyCount());

        Assert.IsTrue(obj.HasMethod("IsValid"));
    }
}
