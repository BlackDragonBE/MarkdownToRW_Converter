using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using CommonMark;
using HtmlAgilityPack;
using MarkdownToRW.Properties;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MarkdownToRW
{
    public partial class MainForm : Form
    {
        private string _markdownPath;
        private string _originalHtml;

        public MainForm()
        {
            InitializeComponent();

            if (MonoHelper.IsRunningOnMono)
            {
                Text += " [MONO]";
                AddRwCertificateForMono();
            }
        }

        private static void AddRwCertificateForMono()
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            WebRequest wr = WebRequest.Create("https://raywenderlich.com");
            Stream stream = wr.GetResponse().GetResponseStream();
            Console.WriteLine(new StreamReader(stream).ReadToEnd());
        }

        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
                OldMarkdown = twtMarkdown.Text
            };

            foreach (HtmlNode node in imgNodes)
            {
                if (node.GetAttributeValue("src", "").StartsWith("http"))
                {
                    continue;
                }

                string localPath = node.GetAttributeValue("src", null);

                imageData.LocalImagePaths.Add(localPath);
                imageData.FullImagePaths.Add(Path.GetDirectoryName(_markdownPath) + "/" + localPath);
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
            twtMarkdown.Text = data.NewMarkdown;
            ConvertMarkdownToHtml();
        }

        private void ConvertMarkdownToHtml()
        {
            string output = CommonMarkConverter.Convert(twtMarkdown.Text);
            _originalHtml = output;
            output = WebUtility.HtmlDecode(output);

            // HTML readability improvements & RW specific changes

            output = output.Replace("<p>", "\n");
            output = output.Replace("<br>", "\n");
            output = output.Replace("</p>", "");
            output = output.Replace("<h1", "\n<h1");
            output = output.Replace("<h2", "\n<h2");
            output = output.Replace("<h3", "\n<h3");
            output = output.Replace("<h4", "\n<h4");
            output = output.Replace("<strong>", "<em>");
            output = output.Replace("</strong>", "</em" +
                                                 ">");
            output = output.Replace("<blockquote>", "<div class=\"note\">");
            output = output.Replace("</blockquote>", "</div>\n");
            output = output.Replace("<pre><code class=\"lang-cs\">", "\n<pre lang=\"csharp\">");
            output = output.Replace("</code></pre>", "</pre>\n");
            //output = output.Replace("</li>", "</li>\n");
            output = output.Replace("<ul>", "\n<ul>");
            //output = output.Replace("</ul>", "</ul>\n");
            //output = output.Replace("</ol>", "</ol>\n");
            output = output.Replace("<ol>", "\n<ol>");

            AddClassToImages(ref output);

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
                    twtMarkdown.Text = sr.ReadToEnd();
                    ConvertMarkdownToHtml();
                }
            }
        }

        private void btnCopyClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtHtml.Text);
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
                sw.Write(_originalHtml);
                sw.Write("</body></html>");
                sw.Flush();
                sw.Close();
            }

            return folderPath + "/tmp.html";
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