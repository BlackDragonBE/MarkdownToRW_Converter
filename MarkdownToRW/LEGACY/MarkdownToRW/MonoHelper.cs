using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using DragonMarkdown;
using DragonMarkdown.Utility;

namespace MarkdownToRW
{
    public enum Platform
    {
        Windows,
        MonoWindows,
        MonoMac,
        MonoLinux
    }

    public static class MonoHelper
    {
        public static bool IsRunningOnMac;
        public static bool IsRunningOnLinux;
        public static bool IsRunningOnWindows;
        public static Platform HostOS = Platform.Windows;
        public static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;

        // Detection modified from
        // https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/

        public static void DetectOperatingSystem()
        {
            IsRunningOnWindows = Path.DirectorySeparatorChar == '\\';

            if (IsRunningOnWindows)
            {
                if (IsRunningOnMono)
                {
                    Console.WriteLine("Running on Mono Windows");
                    HostOS = Platform.MonoWindows;
                }
                else
                {
                    Console.WriteLine("Running on Windows");
                    HostOS = Platform.Windows;
                }
                return;
            }

            string UnixName = ReadProcessOutput("uname");

            if (UnixName.Contains("Darwin"))
            {
                IsRunningOnMac = true;
                Console.WriteLine("Running on macOS");
                HostOS = Platform.MonoMac;
            }
            else
            {
                IsRunningOnLinux = true;
                Console.WriteLine("Running on linux");
                HostOS = Platform.MonoLinux;
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

        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static string ReadProcessOutput(string name)
        {
            return ReadProcessOutput(name, null);
        }

        public static string ReadProcessOutput(string name, string args)
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

        public static bool DownloadFileWindows(string fileToDownload, string savePath, string userAgent = "")
        {
            Console.WriteLine("Downloading " + fileToDownload);

            string agent =
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

            if (userAgent != "")
            {
                agent = userAgent;
            }

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", agent);
                client.DownloadFile(fileToDownload, savePath);
            }

            return File.Exists(savePath);
        }

        public static bool DownloadFileMonoWindows(string fileToDownload, string savePath, string userAgent = "")
        {
            Console.WriteLine("Downloading " + fileToDownload);

            string userAgentFormatted =
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

            if (userAgent != "")
            {
                userAgentFormatted = '"' + userAgent + '"';
            }

            ReadProcessOutput(Application.StartupPath + "/wget.exe",
                @"--user-agent=" + userAgentFormatted + " -O " + DragonUtil.SurroundWithQuotes(savePath) + " " + fileToDownload);

            return File.Exists(savePath);
        }

        public static bool DownloadFileMonoMac(string fileToDownload, string savePath, string userAgent = "")
        {
            Console.WriteLine("Downloading " + fileToDownload);
            string userAgentFormatted =
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

            if (userAgent != "")
            {
                userAgentFormatted = '"' + userAgent + '"';
            }

            ReadProcessOutput("curl",
                "-L -A " + userAgentFormatted + " -o " + DragonUtil.SurroundWithQuotes(savePath) + " " + fileToDownload);

            return File.Exists(savePath);
        }

        public static bool DownloadFileMonoLinux(string fileToDownload, string savePath, string userAgent = "")
        {
            Console.WriteLine("Downloading " + fileToDownload);
            string userAgentFormatted =
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

            if (userAgent != "")
            {
                userAgentFormatted = '"' + userAgent + '"';
            }

            ReadProcessOutput("wget",
                @"--user-agent=" + userAgentFormatted + " -O " + DragonUtil.SurroundWithQuotes(savePath) + " " + fileToDownload);

            return File.Exists(savePath);
        }

        public static bool DownloadFile(string fileToDownload, string savePath, string userAgent = "")
        {
            switch (HostOS)
            {
                case Platform.Windows:
                    return DownloadFileWindows(fileToDownload, savePath, userAgent);
                case Platform.MonoWindows:
                    return DownloadFileMonoWindows(fileToDownload, savePath, userAgent);
                case Platform.MonoMac:
                    return DownloadFileMonoMac(fileToDownload, savePath, userAgent);
                case Platform.MonoLinux:
                    return DownloadFileMonoLinux(fileToDownload, savePath, userAgent);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}