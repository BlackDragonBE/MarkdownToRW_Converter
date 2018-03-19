using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using DragonMarkdown;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MarkdownToRW
{
    public partial class MainForm : Form
    {
        private static readonly string VERSION = "1.00";

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
            txtHtml.Text = Converter.ConvertMarkdownStringToHtml(txtMarkdown.Text);
        }

        private void btnShowPreview_Click(object sender, EventArgs e)
        {
            if (_markdownPath == null)
            {
                MessageBox.Show("No markdown loaded! Please open a markdown file first.", "No markdown");
                return;
            }

            string htmlPath = CreatePreviewHtml();

            PreviewCreator.OpenFileInDefaultApplication(htmlPath);
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
                    txtHtml.Text = Converter.ConvertMarkdownStringToHtml(txtMarkdown.Text);
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

            PreviewCreator.CreateHtmlPreviewFileFromMarkdown(txtMarkdown.Text, folderPath + "/tmp.html");

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