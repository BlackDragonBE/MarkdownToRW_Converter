using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DragonMarkdown.Utility;
using HtmlAgilityPack;
using Markdig;
using Markdig.Helpers;

namespace DragonMarkdown
{
    public class Converter
    {
        public static string ConvertMarkdownStringToHtml(string markdown)
        {           
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
            AddClassToImages(ref output);
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

            // Final cleanup
            output = output.Replace("<div></div>", "");

            output = output.Trim();
            return output;
        }

        public static void ConvertMarkdownFileToHtmlFile(string markdownFilePath, string htmlFilePath)
        {
            using (StreamReader sr = new StreamReader(markdownFilePath))
            {
                string html = ConvertMarkdownStringToHtml(sr.ReadToEnd());
                using (StreamWriter sw = new StreamWriter(htmlFilePath))
                {
                    sw.Write(html);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public static void AddClassToImages(ref string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");

            if (imgNodes == null)
            {
                return;
            }

            for (var i = 0; i < imgNodes.Count; i++)
            {
                HtmlNode node = imgNodes[i];

                if (i == 0) // First image should be right aligned, it's the 250x250 image
                {
                    HtmlAttribute classAttribute = doc.CreateAttribute("class", "alignright size-full");
                    node.Attributes.Add(classAttribute);
                }
                else
                {
                    HtmlAttribute classAttribute = doc.CreateAttribute("class", "aligncenter size-full");
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

            if (linkNodes == null)
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

        private static void ConvertSpoilers(ref string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection divNodes = doc.DocumentNode.SelectNodes("//div");

            foreach (HtmlNode divNode in divNodes)
            {
                if (divNode.InnerHtml.StartsWith("\n<em>Spoiler:"))
                {
                    string spoilerTitle = divNode.ChildNodes[1].InnerText.Split(':')[1].Trim();
                    divNode.RemoveChild(divNode.ChildNodes[1]);
                    divNode.Attributes.Add("title",spoilerTitle);
                    divNode.Name = "spoiler";

                    string inner = divNode.InnerHtml;
                    string newOuter = divNode.OuterHtml;

                    newOuter = newOuter.Replace(inner, "");
                    newOuter = newOuter.Replace("<", "[");
                    newOuter = newOuter.Replace(">", "]");
                    //newOuter = newOuter.Replace("INNER", inner);

                    var newNode = HtmlNode.CreateNode(newOuter);
                    newNode.InnerHtml = newNode.InnerHtml.Replace("][", "]" + inner.Trim() + "[");
                    divNode.ParentNode.ReplaceChild(newNode, divNode);
                    
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

        public static string ReplaceLocalImageLinksWithUrls(string markdownPath, string htmlPath, bool onlyUpdateHtml,
            string markdownText, List<string> localImagePaths, List<string> imageUrls)
        {
            markdownText = DragonUtil.BatchReplaceText(markdownText, localImagePaths, imageUrls);
            var htmlText = ConvertMarkdownStringToHtml(markdownText);

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

            return markdownText;
        }
    }



    public struct ImageLinkData
    {
        public string FullImagePath;
        public string LocalImagePath;
    }


}