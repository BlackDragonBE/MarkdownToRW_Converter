using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DragonMarkdown.Utility
{
    public static class DragonUtil
    {
        public static class CurrentOperatingSystem
        {
            public static bool IsWindows() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            public static bool IsMacOS() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            public static bool IsLinux() =>
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }



        public static string RemoveAllQuotes(string path)
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

        public static string GetFullPathWithoutExtension(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        public static void OpenFileInDefaultApplication(string path)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    //Process.Start(new ProcessStartInfo("cmd", "/c " + SurroundWithQuotes("start " + SurroundWithSingleQuotes(path))));
                    ProcessStartInfo p = new ProcessStartInfo(path) {UseShellExecute = true};
                    Process.Start(p);
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

        /// <summary>
        /// Returns true if the app doesn't poop itself while trying to write a file to the folder.
        /// </summary>
        /// <returns></returns>
        public static bool CheckFolderWritePermission(string folderPath)
        {
            try
            {
                string filePath = folderPath + "/test.txt";
                
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("test");
                    sw.Flush();
                    sw.Close();
                }

                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}