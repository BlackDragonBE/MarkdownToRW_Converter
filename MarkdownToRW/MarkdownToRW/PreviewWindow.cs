using System;
using System.IO;
using System.Windows.Forms;
using Awesomium.Core;
using MarkdownToRW.Properties;

namespace MarkdownToRW
{
    public partial class PreviewWindow : Form
    {
        private readonly string _htmlPath = "";

        public PreviewWindow(string htmlPath)
        {
            InitializeComponent();
            _htmlPath = htmlPath;
        }

        private void PreviewWindow_Load(object sender, EventArgs e)
        {
            string folderPath = Path.GetDirectoryName(_htmlPath);
            WebCore.BaseDirectory = folderPath;
            WebPreview.LoadFile("tmp.html");
        }

        private void PreviewWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            WebPreview.Close();
            File.Delete(_htmlPath);
        }
    }
}