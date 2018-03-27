using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToRW
{
    public class ImageUploadData
    {
        public string MarkdownPath;
        public List<string> LocalImagePaths = new List<string>();
        public List<string> FullImagePaths = new List<string>();
        public List<string> ImageUrls = new List<string>();

        public string OldMarkdown;
        public string NewMarkdown;
    }
}
