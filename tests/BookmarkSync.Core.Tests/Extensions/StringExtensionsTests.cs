using BookmarkSync.Core.Extensions;

namespace BookmarkSync.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("@prplecake", true)]
    [DataRow("flipper", false)]
    public void HasLeadingAt_Success(string input, bool expected)
    {
        // Act
        bool actual = input.HasLeadingAt();

        // Assert
        Assert.AreEqual(expected, actual);
    }
    [DataTestMethod]
    [DataRow("@prplecake@compostintraining.club", "prplecake@compostintraining.club")]
    [DataRow("flipper@compostintraining.club", "flipper@compostintraining.club")]
    public void RemoveLeadingAt_Success(string input, string expected)
    {
        // Act
        string actual = input.RemoveLeadingAt();

        // Assert
        Assert.AreEqual(expected, actual);
    }
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
