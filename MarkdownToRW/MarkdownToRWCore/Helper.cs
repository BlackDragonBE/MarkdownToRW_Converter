using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarkdownToRWCore
{
    public static class Helper
    {
        public static String GetFullPathWithoutExtension(String path)
        {
            return Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path));
        }
    }
}
