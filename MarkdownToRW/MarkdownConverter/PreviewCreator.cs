using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using DragonMarkdown.DragonConverter;
using DragonMarkdown.Utility;
using HtmlAgilityPack;

namespace DragonMarkdown
{
    public class PreviewCreator
    {
        public static string CreateHtmlPreviewFromMarkdown(string markdown)
        {
            string output = "";

            output = PrepareHtmlForPreview(Converter.ConvertMarkdownStringToHtml(markdown, prepareForPreview: true));

            return output;
        }

        private static string PrepareHtmlForPreview(string html)
        {
            string output = html;

            output = PreviewHelper.css + output;
            output = output.Replace("\n\n", "<p>");

            output += "</div></body></html>";

            FixCode(ref output);

            return output;
        }

        private static void FixCode(ref string html)
        {
            // Search all code blocks & use webutility.htmlencode on their content
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection codeNodesFirstPass = doc.DocumentNode.SelectNodes("//pre");

            if (codeNodesFirstPass != null && codeNodesFirstPass.Count > 0)
            {
                foreach (HtmlNode codeNode in codeNodesFirstPass)
                {
                    string newHtml = codeNode.OuterHtml.Replace("<p>", "\n\n").Replace("</p>", "");

                    //codeNode.ParentNode.ReplaceChild(HtmlNode.CreateNode(codeNode.OuterHtml.Replace("<p>", "\n\n").Replace("</p>","")), codeNode);
                    codeNode.ReplaceChildHtml(newHtml);
                }
            }
            
            html = doc.DocumentNode.OuterHtml;
        }

        public static void CreateHtmlPreviewFileFromMarkdown(string markdown, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(CreateHtmlPreviewFromMarkdown(markdown));
                sw.Flush();
                sw.Close();
            }
        }
    }
}