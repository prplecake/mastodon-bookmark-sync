namespace BookmarkSync.Core.Attributes;

/// <summary>
///     Sets a configuration property to be required.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class ConfigRequiredAttribute : Attribute { }
