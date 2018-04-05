using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            output = output.Replace("!!!DEFAULT_CSS_PATH!!!", Path.GetFullPath("js_highlight/styles/atom-one-light.css"));
            output = output.Replace("!!!PACK_JS_PATH!!!", Path.GetFullPath("js_highlight/highlight.pack.js"));

            // PDF
            output = PrepareHtmlForPdf(output, markdownFolder);

            return output;
        }

        private static string PrepareHtmlForPdf(string html, string rootPath)
        {
            html = html.Replace("$[=p=]", "<div class=\"pb_after\"></div>");
            html = html.Replace("$[===]", "");
            html = html.Replace("$[=s=]", "");

            //int fullA4Width = 597;
            //int fullA4Height = 824;

            int fullA4Width = 860;
            int fullA4Height = 1186;

            // Search all images with an alt
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@alt]");

            if (imgNodes != null && imgNodes.Count > 0)
            {
                for (var i = 0; i < imgNodes.Count; i++)
                {
                    HtmlNode node = imgNodes[i];

                    bool useWidth = true;
                    string alt = node.Attributes["alt"].Value;

                    int sizePercent = 100;

                    if (alt.Contains("width"))
                    {
                        useWidth = true;
                        sizePercent = Convert.ToInt32(alt.Split(new[] { "width=" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('%')[0]);
                    }
                    else if (alt.Contains("height"))
                    {
                        useWidth = false;
                        sizePercent = Convert.ToInt32(alt.Split(new[] { "height=" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('%')[0]);
                    }
                    else
                    {
                        return html;
                    }

                    Size imageSize = ImageHelper.GetDimensions(node.Attributes["src"].Value);

                    double newWidth;
                    double newHeight;
                    double widthRatio = imageSize.Width / (double)imageSize.Height;
                    double heightRatio = imageSize.Height / (double)imageSize.Width;

                    if (useWidth)
                    {
                        //newWidth = imageSize.Width * (sizePercent / 100f);
                        //newHeight = imageSize.Height * (sizePercent / 100f);

                        newWidth = fullA4Width * (sizePercent / 100f);
                        newHeight = newWidth * heightRatio;
                    }
                    else
                    {
                        newHeight = fullA4Height * (sizePercent / 100f);
                        newWidth = newHeight * widthRatio;
                    }


                    node.Attributes.Add("width", newWidth.ToString());
                    node.Attributes.Add("height", newHeight.ToString());
                }
            }

            return doc.DocumentNode.OuterHtml;
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