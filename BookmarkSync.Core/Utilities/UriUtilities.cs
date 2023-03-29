using System;
using System.Text.RegularExpressions;
using Serilog;

namespace BookmarkSync.Core.Utilities;

public static class UriUtilities
{
    private static readonly ILogger _logger = Log.ForContext(typeof(UriUtilities));
    /// <summary>
    /// Returns a URI without a (/https?/) protocol.
    /// </summary>
    /// <param name="uri">The URI to process.</param>
    /// <returns>A URI without a protocol or trailing slash.</returns>
    public static string RemoveProto(this string uri)
    {
        _logger.Debug("Running {Method} for {Uri}", "RemoveProto", uri);
        // https://stackoverflow.com/questions/10306690/what-is-a-regular-expression-which-will-match-a-valid-domain-name-without-a-subd/26987741#26987741
        const string pattern =
            @"(?:https?:\/\/)?((((?!-))(xn--|_)?[a-z0-9-]{0,61}[a-z0-9]{1,1}\.)*(xn--)?([a-z0-9][a-z0-9\-]{0,60}|[a-z0-9-]{1,30}\.[a-z]{2,}))";
        var m = Regex.Match(uri, pattern, RegexOptions.IgnoreCase);
        if (m.Success) return m.Groups[1].Value;
        _logger.Error("Could not process {Uri}", uri);
        throw new ArgumentException();
    }
}
