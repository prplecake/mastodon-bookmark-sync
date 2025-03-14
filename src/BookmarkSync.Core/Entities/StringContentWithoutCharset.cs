using System.Text;

namespace BookmarkSync.Core.Entities;

public class StringContentWithoutCharset : StringContent
{
    public StringContentWithoutCharset(string content) : base(content)
    {
    }

    public StringContentWithoutCharset(string content, Encoding encoding) : base(content, encoding)
    {
        Headers.ContentType.CharSet = null;
    }

    public StringContentWithoutCharset(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
    {
        Headers.ContentType.CharSet = null;
    }

    public StringContentWithoutCharset(string content, string mediaType) : base(content, Encoding.UTF8, mediaType)
    {
        Headers.ContentType.CharSet = null;
    }
}
