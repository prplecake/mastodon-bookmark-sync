using BookmarkSync.Core.Extensions;

namespace BookmarkSync.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("Example", "example")]
    [DataRow("TestCase", "test_case")]
    [DataRow("Example2", "example2")]
    public void ToSnakeCase_Success(string input, string expected)
    {
        // Act
        string actual = input.ToSnakeCase();

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
