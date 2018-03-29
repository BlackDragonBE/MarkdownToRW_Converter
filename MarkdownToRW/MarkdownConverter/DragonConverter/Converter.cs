using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DragonMarkdown.Utility;
using HtmlAgilityPack;
using Markdig;

namespace DragonMarkdown.DragonConverter
{
    public class Converter
    {
        public static string ConvertMarkdownStringToHtml(string markdown, ConverterOptions options = null, string rootPath = null)
        {
            if (options == null)
            {
                options = new ConverterOptions();
            }

            //MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseEmphasisExtras().UseCustomContainers().Build();


            string output = Markdown.ToHtml(markdown, pipeline);
            output = WebUtility.HtmlDecode(output);

            // HTML readability improvements & RW specific changes

            // Code
            output = output.Replace("<pre><code class=", "\r\n<pre lang=");
            output = output.Replace("lang-", "");
            output = output.Replace("language-", "");
            output = output.Replace("</code></pre>", "</pre>\r\n");

            // Add attributes
            AddClassToImages(ref output, options.FirstImageIsAlignedRight, rootPath);
            AddExtraAttributesToLinks(ref output);

            // Make new lines consistent across platforms
            output = output.Replace("\r\n", "|||");
            output = output.Replace("|||", "\n");

            // Text
            output = output.Replace("<p>", "\n");
            output = output.Replace("<br>", "\n");
            output = output.Replace("</p>", "");
            output = output.Replace("<h1", "\n<h1");
            output = output.Replace("<h2", "\n<h2");
            output = output.Replace("<h3", "\n<h3");
            output = output.Replace("<h4", "\n<h4");
            output = output.Replace("<em>", "<i>");
            output = output.Replace("</em>", "</i>");
            output = output.Replace("<strong>", "<em>");
            output = output.Replace("</strong>", "</em>");

            // List
            //output = output.Replace("<ul>", "\n<ul>");
            //output = output.Replace("<ol>", "\n<ol>");

            //// Note
            output = output.Replace("</blockquote>", "</div>");
            output = output.Replace("<blockquote>\n", "\n<blockquote>");
            output = output.Replace("<blockquote>\n<em>Note", "<div class=\"note\">\n<em>Note");
            output = output.Replace("<blockquote>", "<div>");

            // Spoiler
            ConvertSpoilers(ref output);


            if (options.ReplaceImageWithAltWithCaption)
            {
                ConvertImagesWithAltToCaptions(ref output);
            }

            // Final cleanup
            output = output.Replace("<div></div>", "");

            output = output.Trim();
            return output;
        }

        public static void ConvertMarkdownFileToHtmlFile(string markdownFilePath, string htmlFilePath,
            ConverterOptions options = null)
        {
            using (StreamReader sr = new StreamReader(markdownFilePath))
            {
                string html = ConvertMarkdownStringToHtml(sr.ReadToEnd(), options, Path.GetDirectoryName(markdownFilePath));
                using (StreamWriter sw = new StreamWriter(htmlFilePath))
                {
                    sw.Write(html);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private static void AddClassToImages(ref string html, bool firstImageRightAligned, string rootPath = null)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
            string size = "full";

            if (imgNodes == null || imgNodes.Count == 0)
            {
                return;
            }

            for (var i = 0; i < imgNodes.Count; i++)
            {
                HtmlNode node = imgNodes[i];

                // If root path is known, check if images are too big for full size class
                if (rootPath != null)
                {
                    var imageSize = ImageHelper.GetDimensions(GetFullFilePath(node.Attributes["src"].Value, rootPath));

                    if (imageSize.Width > 700)
                    {
                        size = "large";
                    }
                    else
                    {
                        size = "full";
                    }
                }

                if (i == 0 && firstImageRightAligned) // First image should be right aligned, it's the 250x250 image
                {
                    HtmlAttribute classAttribute = doc.CreateAttribute("class", "alignright size-" + size);
                    node.Attributes.Add(classAttribute);
                }
                else
                {
                    HtmlAttribute classAttribute = doc.CreateAttribute("class", "aligncenter size-" + size);
                    node.Attributes.Add(classAttribute);
                }
            }

            html = doc.DocumentNode.OuterHtml;
        }

        private static void AddExtraAttributesToLinks(ref string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//a");

            if (linkNodes == null || linkNodes.Count == 0)
            {
                return;
            }

            for (var i = 0; i < linkNodes.Count; i++)
            {
                HtmlNode node = linkNodes[i];

                HtmlAttribute relAttribute = doc.CreateAttribute("rel", "noopener");
                node.Attributes.Add(relAttribute);

                HtmlAttribute targetAttribute = doc.CreateAttribute("target", "_blank");
                node.Attributes.Add(targetAttribute);
            }

            html = doc.DocumentNode.OuterHtml;
        }

        private static void ConvertImagesWithAltToCaptions(ref string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");

            if (imgNodes == null || imgNodes.Count == 0)
            {
                return;
            }

            foreach (HtmlNode imgNode in imgNodes)
            {
                if (imgNode.Attributes["alt"] != null && imgNode.Attributes["alt"].Value != "")
                {
                    HtmlNode parent = imgNode.ParentNode;

                    HtmlDocument newDoc = new HtmlDocument();
                    HtmlNode newElement = newDoc.CreateElement("caption");
                    newElement.SetAttributeValue("align", imgNode.Attributes["class"].Value);
                    newElement.AppendChild(imgNode);
                    newElement.InnerHtml += imgNode.Attributes["alt"].Value;

                    parent.ReplaceChild(newElement, imgNode);

                    ReplaceOuterHtmlWithSquareBrackets(newElement);
                }
            }

            html = doc.DocumentNode.OuterHtml;
        }

        private static void ConvertSpoilers(ref string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection divNodes = doc.DocumentNode.SelectNodes("//div");

            if (divNodes == null || divNodes.Count == 0)
            {
                return;
            }

            foreach (HtmlNode divNode in divNodes)
            {
                if (divNode.InnerHtml.StartsWith("\n<em>Spoiler:"))
                {
                    string spoilerTitle = divNode.ChildNodes[1].InnerText.Split(':')[1].Trim();
                    divNode.RemoveChild(divNode.ChildNodes[1]);
                    divNode.Attributes.Add("title", spoilerTitle);
                    divNode.Name = "spoiler";

                    ReplaceOuterHtmlWithSquareBrackets(divNode);
                }
            }

            html = doc.DocumentNode.OuterHtml;
        }

        public static List<ImageLinkData> FindAllImageLinksInHtml(string html, string rootFolder)
        {
            List<ImageLinkData> links = new List<ImageLinkData>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");

            if (imgNodes == null)
            {
                return links;
            }

            foreach (HtmlNode node in imgNodes)
            {
                // Skip if web link
                if (node.GetAttributeValue("src", "").StartsWith("http") ||
                    node.GetAttributeValue("src", "").StartsWith("www"))
                {
                    continue;
                }

                string localPath = node.GetAttributeValue("src", null);
                string fullPath = rootFolder + "/" + localPath;

                // Check if file exists
                if (File.Exists(fullPath))
                {
                    ImageLinkData imageData = new ImageLinkData
                    {
                        LocalImagePath = localPath,
                        FullImagePath = fullPath
                    };

                    links.Add(imageData);
                }
            }

            return links;
        }

        public static MarkdownAndHtml ReplaceLocalImageLinksWithUrls(string markdownPath, string htmlPath, bool onlyUpdateHtml,
            string markdownText, List<string> localImagePaths, List<string> imageUrls, string htmlText)
        {
            markdownText = DragonUtil.BatchReplaceText(markdownText, localImagePaths, imageUrls);
            htmlText = DragonUtil.BatchReplaceText(htmlText, localImagePaths, imageUrls);
            //var htmlText = ConvertMarkdownStringToHtml(markdownText,);

            if (htmlPath != null)
            {
                DragonUtil.QuickWriteFile(htmlPath, htmlText);
                Console.WriteLine("Replaced HTML!");
            }

            if (!onlyUpdateHtml)
            {
                DragonUtil.QuickWriteFile(markdownPath, markdownText);
                Console.WriteLine("Replaced Markdown!");
            }

            return new MarkdownAndHtml {Markdown = markdownText, Html = htmlText};
        }

        private static void ReplaceOuterHtmlWithSquareBrackets(HtmlNode node)
        {
            string inner = node.InnerHtml;
            string newOuter = node.OuterHtml;

            newOuter = newOuter.Replace(inner, "");
            newOuter = newOuter.Replace("<", "[");
            newOuter = newOuter.Replace(">", "]");
            //newOuter = newOuter.Replace("INNER", inner);

            var newNode = HtmlNode.CreateNode(newOuter);
            newNode.InnerHtml = newNode.InnerHtml.Replace("][", "]" + inner.Trim() + "[");
            node.ParentNode.ReplaceChild(newNode, node);
        }

        public static string GetFullFilePath(string localFilePath, string rootPath)
        {
            return rootPath + "/" + localFilePath;
        }
    }


    public struct ImageLinkData
    {
        public string FullImagePath;
        public string LocalImagePath;
    }
}