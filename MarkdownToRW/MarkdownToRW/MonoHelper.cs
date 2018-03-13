using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MarkdownToRW
{
    public static class MonoHelper
    {
        public static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsRunningOnMac;
        public static bool IsRunningOnLinux;
        public static bool IsRunningOnWindows;

        // Detection modified from
        // https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/

        public static void DetectOperatingSystem()
        {
            IsRunningOnWindows = Path.DirectorySeparatorChar == '\\';

            if (IsRunningOnWindows)
            {
                Console.WriteLine("Running on Windows");
                return;
            }

            string UnixName = ReadProcessOutput("uname");

            if (UnixName.Contains("Darwin"))
            {
                IsRunningOnMac = true;
                Console.WriteLine("Running on macOS");
            }
            else
            {
                IsRunningOnLinux = true;
                Console.WriteLine("Running on linux");
            }
        }
        
        // Mac clipboard workaround from
        // https://andydunkel.net/2017/02/23/windows_forms_on_osx_clipboard_not_working/

        public static void CopyToMacClipboard(string textToCopy)
        {
            Console.WriteLine("Attempting copy to clipboard on macOS...");

            try
            {
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo("pbcopy", "-pboard general -Prefer txt");
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.Start();
                    p.StandardInput.Write(textToCopy);
                    p.StandardInput.Close();
                    p.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't copy to macOS clipboard:\n" + e);
            }
        }

        public static string PasteFromMacClipboard()
        {
            Console.WriteLine("Attempting paste from clipboard on macOS...");
            string pasteText;

            try
            {
                using (var p = new Process())
                {

                    p.StartInfo = new ProcessStartInfo("pbpaste", "-pboard general");
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.Start();
                    pasteText = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't paste from macOS clipboard:\n" + e);
                return null;
            }

            return pasteText;
        }

        private static string ReadProcessOutput(string name)
        {
            return ReadProcessOutput(name, null);
        }

        private static string ReadProcessOutput(string name, string args)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                if (args != null && args != "") p.StartInfo.Arguments = " " + args;
                p.StartInfo.FileName = name;
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (output == null) output = "";
                output = output.Trim();
                return output;
            }
            catch
            {
                return "";
            }
        }
    }
}