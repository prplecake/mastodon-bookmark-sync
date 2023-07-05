namespace BookmarkSync.Core.Tests.Entities.Config;

[TestClass]
public class InstanceTests
{
    [TestMethod]
    public void Instance_HasProperties()
    {
        // Arrange
        Instance obj = new();

        // Assert
        Assert.AreEqual(3, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("AccessToken"));
        Assert.IsTrue(obj.HasProperty("DeleteBookmarks"));
        Assert.IsTrue(obj.HasProperty("Uri"));

        Assert.IsTrue(obj.HasMethod("IsValid"));
    }
    [TestMethod]
    public void Instance_ToString_Is_Uri()
    {
        // Arrange
        Instance obj = new()
        {
            Uri = "https://compostintraining.club"
        };

        // Assert
        Assert.AreEqual(obj.ToString(), obj.Uri);
    }
}
