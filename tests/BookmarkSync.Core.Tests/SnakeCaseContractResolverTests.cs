using BookmarkSync.Core.Json;

namespace BookmarkSync.Core.Tests;

[TestClass]
public class SnakeCaseContractResolverTests
{
    [DataTestMethod]
    [DataRow("clientId", "client_id")]
    public void ResolvePropertyName_IsSnakeCase(string input, string expected)
    {
        // Arrange
        var resolver = SnakeCaseContractResolver.Instance;

        // Act
        var actual = resolver.GetResolvedPropertyName(input);

        // Assert
        Assert.AreEqual(expected, actual);
    }
}
