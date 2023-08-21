using BookmarkSync.Core.Utilities;

namespace BookmarkSync.Core.Tests.Utilities;

[TestClass]
public class UriUtilitiesTests
{
    [DataTestMethod]
    [DataRow("https://example.com")]
    [DataRow("https://example.com/")]
    public void UriUtilities_HttpsProto(string uri)
    {
        string actual = uri.RemoveProto();
        Assert.AreEqual("example.com", actual);
    }
    [DataTestMethod]
    [DataRow("http://example.com")]
    [DataRow("http://example.com/")]
    public void UriUtilities_HttpProto(string uri)
    {
        string actual = uri.RemoveProto();
        Assert.AreEqual("example.com", actual);
    }
    [DataTestMethod]
    [DataRow("example.com")]
    [DataRow("example.com/")]
    public void UriUtilities_NoProto(string uri)
    {
        string actual = uri.RemoveProto();
        Assert.AreEqual("example.com", actual);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UrlUtilities_RemoveProto_EmptyString()
    {
        // Act
        string.Empty.RemoveProto();

        // Assert - Exception: ArgumentException
    }
}
