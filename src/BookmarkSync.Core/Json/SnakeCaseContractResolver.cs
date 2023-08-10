using BookmarkSync.Core.Extensions;
using Newtonsoft.Json.Serialization;

namespace BookmarkSync.Core.Json;

public class SnakeCaseContractResolver : DefaultContractResolver
{
    public static readonly SnakeCaseContractResolver Instance = new();
    override protected string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToSnakeCase();
    }
}
