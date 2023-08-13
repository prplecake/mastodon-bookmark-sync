namespace BookmarkSync.Core.Extensions;

public static class StringExtensions
{
    public static bool HasLeadingAt(this string str)
        => str.StartsWith("@");
    public static string RemoveLeadingAt(this string str)
        => str.HasLeadingAt() ? str[1..] : str;
    public static string ToSnakeCase(this string str)
        => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
}
