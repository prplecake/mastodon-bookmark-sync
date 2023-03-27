using BookmarkSync.Infrastructure.Services.Pinboard;

namespace BookmarkSync.Infrastructure.Tests.Services.Pinboard;

[TestClass]
public class PinboardBookmarkTests
{
    [TestMethod]
    public void PinboardBookmark_HasProperties()
    {
        // Arrange
        PinboardBookmark obj = new();
        
        // Assert
        Assert.AreEqual(5, obj.PropertyCount());
        Assert.IsTrue(obj.HasProperty("Description"));
        Assert.IsTrue(obj.HasProperty("ExtendedDescription"));
        Assert.IsTrue(obj.HasProperty("Shared"));
        Assert.IsTrue(obj.HasProperty("Tags"));
        Assert.IsTrue(obj.HasProperty("Uri"));
        
        Assert.IsTrue(obj.HasMethod("GetFormattedTags"));
    }

    [TestMethod]
    public void GetFormattedTags_NullTags()
    {
        // Arrange
        PinboardBookmark obj = new();
        
        // Assert
        Assert.IsNull(obj.GetFormattedTags());
    }
    [TestMethod]
    public void GetFormattedTags_WithTags()
    {
        // Arrange
        PinboardBookmark obj = new()
        {
            Tags = new[]
            {
                "first-tag", "second-tag"
            }
        };
        const string expected = "first-tag second-tag";

        // Assert
        Assert.AreEqual(expected, obj.GetFormattedTags());
    }
}
