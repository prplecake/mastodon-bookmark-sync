using BookmarkSync.Core.Utilities;

namespace BookmarkSync.Core.Tests.Utilities;

[TestClass]
public class HtmlUtilitiesTests
{
    [TestMethod]
    public void ConvertToPlainText_Success()
    {
        // Arrange
        const string
            html = "<p>example paragraph</p>",
            expected = "\r\nexample paragraph";

        // Act
        string actual = HtmlUtilities.ConvertToPlainText(html);
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void ConvertToPlainText_WithBr()
    {
        // Arrange
        const string
            html = "<p>example paragraph<br/>with breaks</p>",
            expected = "\r\nexample paragraph\r\nwith breaks";
        
        // Act
        string actual = HtmlUtilities.ConvertToPlainText(html);
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void ConvertToPlainText_WithComments()
    {
        // Arrange
        const string
            html = """
<!-- html comment -->
html document
""",
            expected = "html document";
        
        // Act
        string actual = HtmlUtilities.ConvertToPlainText(html).Trim();
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    
    [TestMethod]
    public void ConvertToPlainText_WithScripts()
    {
        // Arrange
        const string
            html = """
html document
<script>
console.log("would be javascript");
</script>
""",
            expected = "html document";
        
        // Act
        string actual = HtmlUtilities.ConvertToPlainText(html).Trim();
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void ConvertToPlainText_WithStyle()
    {
        // Arrange
        const string
            html = """
<style>
body
{
    font-size: 1312px;
}
</style>
html document
""",
            expected = "html document";
        
        // Act
        string actual = HtmlUtilities.ConvertToPlainText(html).Trim();
        
        // Assert
        Assert.AreEqual(expected, actual);
    }

}