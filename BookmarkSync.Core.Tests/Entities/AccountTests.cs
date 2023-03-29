using BookmarkSync.Core.Entities;

namespace BookmarkSync.Core.Tests.Entities;

[TestClass]
public class AccountTests
{
    [TestMethod]
    public void Account_HasProperties()
    {
        // Arrange
        Account obj = new();

        // Assert
        Assert.AreEqual(1, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("Name"));
    }
    [TestMethod]
    public void Account_ToString_Is_Name()
    {
        // Arrange
        Account obj = new()
        {
            Name = "@prplecake@compostintraining.club"
        };

        // Assert
        Assert.AreEqual(obj.ToString(), obj.Name);
    }
}
