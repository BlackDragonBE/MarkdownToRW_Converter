using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonMarkdown
{
    public static class DragonHelperUtility
    {
        public static string CleanPath(string path)
        {
            return path.Replace("\"", "").Replace("'", "").Trim();
        }

        public static string SurroundWithQuotes(string str)
        {
            return '"' + str + '"';
        }

        public static string SurroundWithSingleQuotes(string str)
        {
            return '"' + str + '"';
        }

        public static string BatchReplaceText(string text, List<string> originals, List<string> replacements)
        {
            string newText = text;

            for (var i = 0; i < originals.Count; i++)
            {
                string original = originals[i];
                string replacement = replacements[i];
                newText = newText.Replace(original, replacement);
            }

            return newText;
        }

        public static void QuickWriteFile(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
                sw.Flush();
                sw.Close();
            }
        }

        public static String GetFullPathWithoutExtension(String path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        public static void OpenFileInDefaultApplication(string path)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {SurroundWithQuotes(path)}"));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", SurroundWithSingleQuotes(path));
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", SurroundWithSingleQuotes(path));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
