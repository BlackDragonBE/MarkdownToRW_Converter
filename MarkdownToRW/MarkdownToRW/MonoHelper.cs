using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MarkdownToRW
{
    public static class MonoHelper
    {
        public static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsRunningOnMac => Environment.OSVersion.Platform == PlatformID.MacOSX;
        public static bool IsRunningOnLinux => Environment.OSVersion.Platform == PlatformID.Unix;

        // Mac clipboard workaround from
        // https://andydunkel.net/2017/02/23/windows_forms_on_osx_clipboard_not_working/

        public static void CopyToMacClipboard(string textToCopy)
        {
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
                Console.WriteLine(e);
                MessageBox.Show("Couldn't copy to clipboard:\n" + e);
            }
        }

        public static string PasteFromMacClipboard()
        {
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
                Console.WriteLine(e);
                return null;
            }

            return pasteText;
        }
    }
}