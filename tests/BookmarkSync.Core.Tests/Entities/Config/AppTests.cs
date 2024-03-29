namespace BookmarkSync.Core.Tests.Entities.Config;

[TestClass]
public class AppTests
{
    [TestMethod]
    public void App_HasProperties()
    {
        // Arrange
        App obj = new();

        // Assert
        Assert.AreEqual(3, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("Bookmarking"));
        Assert.IsTrue(obj.HasProperty("IgnoredAccounts"));
        Assert.IsTrue(obj.HasProperty("LastSynced"));

        Assert.IsTrue(obj.HasMethod("IsValid"));
    }
}
