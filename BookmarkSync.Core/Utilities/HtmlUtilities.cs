using System;
using System.IO;
using HtmlAgilityPack;

namespace BookmarkSync.Core.Utilities;

public static class HtmlUtilities
{
    public static string ConvertToPlainText(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var sw = new StringWriter();
        ConvertTo(doc.DocumentNode, sw);

        sw.Flush();
        return sw.ToString();
    }
    private static void ConvertTo(HtmlNode node, TextWriter outText)
    {
        string html;
        switch (node.NodeType)
        {
            case HtmlNodeType.Comment:
                // don't output comments
                break;
            case HtmlNodeType.Document:
                ConvertContentTo(node, outText);
                break;
            
            case HtmlNodeType.Text:
                // script and style must not be output
                string parentName = node.ParentNode.Name;
                if (parentName is "script" or "style")
                    break;
                
                // get text
                html = ((HtmlTextNode)node).Text;
                
                // is it a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                    break;
                
                // check the text is meaningful and not a bunch of whitespace
                if (html.Trim().Length > 0)
                {
                    outText.Write(HtmlEntity.DeEntitize(html));
                }
                break;
            
            case HtmlNodeType.Element:
                switch (node.Name)
                {
                    case "p":
                        // treat paragraphs as crlf
                        outText.Write("\r\n");
                        break;
                    case "br":
                        outText.Write("\r\n");
                        break;
                }

                if (node.HasChildNodes)
                {
                    ConvertContentTo(node, outText);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private static void ConvertContentTo(HtmlNode node, TextWriter outText)
    {
        foreach (var subnode in node.ChildNodes)
        {
            ConvertTo(subnode, outText);
        }
    }
}
