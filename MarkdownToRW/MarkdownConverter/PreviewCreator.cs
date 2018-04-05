using System;
using System.Collections.Generic;
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
        public static string CreateHtmlPreviewFromMarkdown(string markdown, string markdownFolder)
        {
            string output = "";

            output = PrepareHtmlForPreview(Converter.ConvertMarkdownStringToHtml(markdown, prepareForPreview: true), markdownFolder);

            return output;
        }

        private static string PrepareHtmlForPreview(string html, string markdownFolder)
        {
            string output = html;

            output = PreviewHelper.css + output;
            output = output.Replace("\n\n", "<p>");

            output += "</div></body></html>";

            FixCode(ref output);
            ReplaceImageSources(ref output, markdownFolder);

            // Syntax Highlighting
            //output = output.Replace("!!!DEFAULT_CSS_PATH!!!", "file://" + Path.GetFullPath("js_highlight/styles/default.css"));
            output = output.Replace("!!!DEFAULT_CSS_PATH!!!", "file://" + Path.GetFullPath("js_highlight/styles/atom-one-light.css"));
            output = output.Replace("!!!PACK_JS_PATH!!!", "file://" + Path.GetFullPath("js_highlight/highlight.pack.js"));

            // PDF
            output = output.Replace("$[=p=]", "<div class=\"pb_after\"></div>");
            output = output.Replace("$[===]", "");
            output = output.Replace("$[=s=]", "");

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

        private static void ReplaceImageSources(ref string html, string folder)
        {
            var imageData = Converter.FindAllImageLinksInHtml(html, folder);

            List<string> localPaths = new List<string>();
            List<string> fullPaths = new List<string>();

            foreach (ImageLinkData data in imageData)
            {
                localPaths.Add(data.LocalImagePath);
                fullPaths.Add(data.FullImagePath);
            }

            html = DragonUtil.BatchReplaceText(html, localPaths, fullPaths);
        }

        public static void CreateHtmlPreviewFileFromMarkdown(string markdown, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(CreateHtmlPreviewFromMarkdown(markdown, Path.GetDirectoryName(filePath)));
                sw.Flush();
                sw.Close();
            }
        }
    }
}