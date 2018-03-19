using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DragonMarkdown
{
    public class PreviewCreator
    {
        public static string CreateHtmlPreviewFromMarkdown(string markdown)
        {
            string output = "";

            output = PrepareHtmlForPreview(Converter.ConvertMarkdownStringToHtml(markdown));

            return output;
        }

        private static string PrepareHtmlForPreview(string html)
        {
            string output = html;

            output = PreviewHelper.css + output;
            output = output.Replace("\n\n", "<p>");
            output += "</div></body></html>";

            return output;
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