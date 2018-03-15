using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CommonMark;
using HtmlAgilityPack;
using MarkdownToRW.Properties;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MarkdownToRW
{
    public partial class MainForm : Form
    {
        private static readonly string VERSION = "0.93";

        private string _markdownPath;

        public MainForm()
        {
            InitializeComponent();
            Console.WriteLine("Detecting OS...");
            MonoHelper.DetectOperatingSystem();

            ServicePointManager.ServerCertificateValidationCallback = MonoHelper.Validator;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            Text += " v" + VERSION + " on " + Environment.OSVersion.VersionString;

            if (MonoHelper.IsRunningOnMono)
            {
                Text += " [MONO]";
            }

            UpdateHelper.DoUpdateCleanup();
            UpdateHelper.CheckForUpdates(VERSION);
        }

        private void OpenImageUploadWindow()
        {
            //Extract images
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(txtHtml.Text);
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");

            if (imgNodes == null)
            {
                MessageBox.Show("No image paths found!");
                return;
            }

            ImageUploadData imageData = new ImageUploadData
            {
                MarkdownPath = _markdownPath,
                OldMarkdown = txtMarkdown.Text
            };

            foreach (HtmlNode node in imgNodes)
            {
                // Skip if web link
                if (node.GetAttributeValue("src", "").StartsWith("http") ||
                    node.GetAttributeValue("src", "").StartsWith("www"))
                {
                    continue;
                }

                string localPath = node.GetAttributeValue("src", null);
                string fullPath = Path.GetDirectoryName(_markdownPath) + "/" + localPath;

                // Check if file exists
                if (File.Exists(fullPath))
                {
                    imageData.LocalImagePaths.Add(localPath);
                    imageData.FullImagePaths.Add(fullPath);
                }
                else
                {
                    MessageBox.Show("File not found: " + localPath + "\nThis file will be skipped.");
                }
            }

            if (imageData.FullImagePaths.Count == 0)
            {
                MessageBox.Show("No local image paths found!");
                return;
            }

            ImageUploadWindow uploadWindow = new ImageUploadWindow(imageData);

            if (uploadWindow.ShowDialog() == DialogResult.OK)
            {
                UseDataResults(uploadWindow.ImageUploadData);
            }
        }

        private void UseDataResults(ImageUploadData data)
        {
            txtMarkdown.Text = data.NewMarkdown;
            ConvertMarkdownToHtml();
        }

        private void ConvertMarkdownToHtml()
        {
            string output = CommonMarkConverter.Convert(txtMarkdown.Text);
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

            // Text
            output = output.Replace("<p>", "\r\n");
            output = output.Replace("<br>", "\r\n");
            output = output.Replace("</p>", "");
            output = output.Replace("<h1", "\r\n<h1"); // h1 is not supported, replace with h2
            output = output.Replace("<h2", "\r\n<h2");
            output = output.Replace("<h3", "\r\n<h3");
            output = output.Replace("<h4", "\r\n<h4"); // h4 is not supported, replace with h3
            output = output.Replace("<strong>", "<em>");
            output = output.Replace("</strong>", "</em" +
                                                 ">");

            // List
            output = output.Replace("<ul>", "\r\n<ul>");
            output = output.Replace("<ol>", "\r\n<ol>");

            //// Note
            output = output.Replace("</blockquote>", "</div>");
            output = output.Replace("<blockquote>\r\n", "\r\n<blockquote>");
            output = output.Replace("<blockquote>\r\n<em>Note", "<div class=\"note\">\r\n<em>Note");
            output = output.Replace("<blockquote>", "<div>");

            // Spoiler
            output = output.Replace("<blockquote>\r\n<em>Spoiler", "<div class=\"spoiler\">\r\n<em>Spoiler");
            output = output.Replace("<div class=\"spoiler\">", "[spoiler title=\"Solution\"]");
            // TODO: replace first </div> found after "<div class=\"spoiler\">" with </spoiler> somehow for all spoilers

            // Final cleanup
            output = output.Replace("<div></div>", "");

            output = output.Trim();
            txtHtml.Text = output;
        }

        private void AddClassToImages(ref string html)
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

        private void AddExtraAttributesToLinks(ref string html)
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

        private void btnShowPreview_Click(object sender, EventArgs e)
        {
            if (_markdownPath == null)
            {
                MessageBox.Show("No markdown loaded! Please open a markdown file first.", "No markdown");
                return;
            }

            string htmlPath = CreatePreviewHtml();

            if (!MonoHelper.IsRunningOnMono)
            {
                PreviewWindow previewWindow = new PreviewWindow(htmlPath);
                previewWindow.ShowDialog();
            }
            else
            {
                // No solution for opening in-app webbrowser in mono yet, open html file in standard application
                Process.Start(htmlPath);
            }
        }

        private void btnOpenMarkdown_Click(object sender, EventArgs e)
        {
            if (openMarkdownDialog.ShowDialog() == DialogResult.OK)
            {
                _markdownPath = openMarkdownDialog.FileName;
                lblMarkdownPath.Text = _markdownPath;

                using (StreamReader sr = new StreamReader(_markdownPath))
                {
                    txtMarkdown.Text = sr.ReadToEnd();
                    ConvertMarkdownToHtml();
                }
            }
        }

        private void btnCopyClipboard_Click(object sender, EventArgs e)
        {
            if (txtHtml.Text != "")
            {
                Clipboard.SetText(txtHtml.Text);

                if (MonoHelper.IsRunningOnMac)
                {
                    MonoHelper.CopyToMacClipboard(txtHtml.Text);
                }
            }
        }

        private void btnWordpress_Click(object sender, EventArgs e)
        {
            OpenImageUploadWindow();
        }

        private string CreatePreviewHtml()
        {
            string folderPath = Path.GetDirectoryName(_markdownPath);

            using (StreamWriter sw = new StreamWriter(folderPath + "/tmp.html"))
            {
                sw.Write(Resources.rwCSS);
                //sw.Write(_originalHtml);
                sw.Write(PrepareHtmlForPreview());
                sw.Write("</div></body></html>");
                sw.Flush();
                sw.Close();
            }

            return folderPath + "/tmp.html";
        }

        private string PrepareHtmlForPreview()
        {
            string html = txtHtml.Text;

            html = html.Replace("\n\n", "<p>");

            return html;
        }

        private void DeletePreviewHtml()
        {
            if (File.Exists(_markdownPath))
            {
                string folderPath = Path.GetDirectoryName(_markdownPath);
                File.Delete(folderPath + "/tmp.html");
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeletePreviewHtml();
        }

        private void lblMarkdownPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_markdownPath);
        }
    }
}