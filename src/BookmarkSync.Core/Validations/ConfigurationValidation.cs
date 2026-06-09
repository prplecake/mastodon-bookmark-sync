using BookmarkSync.Core.Attributes;

namespace BookmarkSync.Core.Validations;

public static class ConfigurationValidation
{
    /// <summary>
    ///     Checks all required properties (set by the ConfigRequiredAttribute) of the provided <paramref name="obj" /> whether
    ///     any are null or empty.
    ///     See also:
    ///     https://stackoverflow.com/questions/22683040/how-to-check-all-properties-of-an-object-whether-null-or-empty/22683141#22683141
    /// </summary>
    /// <param name="obj">The object to check for any null or empty properties.</param>
    /// <returns>True if any required properties are null or empty, otherwise False.</returns>
    public static bool IsAnyNullOrEmpty(this object obj)
    {
        foreach (var pi in obj.GetType().GetProperties())
        {
            if (Attribute.IsDefined(pi, typeof(ConfigRequiredAttribute)) ||
                Attribute.IsDefined(obj.GetType(), typeof(ConfigRequiredAttribute)))
            {
                object? value = pi.GetValue(obj);
                if (value is null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
