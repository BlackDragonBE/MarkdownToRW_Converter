using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EasyHttp.Http;
using Newtonsoft.Json;

namespace MarkdownToRW
{
    public static class UpdateHelper
    {
        private static decimal _thisVersion;

        private static readonly string RELEASE_URL =
            @"https://api.github.com/repos/BlackDragonBE/MarkdownToRW_Converter/releases/latest";

        public static void CheckForUpdates(string version)
        {
            _thisVersion = Convert.ToDecimal(version);
            Console.WriteLine("Checking for new version...");

            GithubRelease newestRelease = null;

            if (!MonoHelper.IsRunningOnMono) //Regular Windows
            {
                newestRelease = GetNewestRelease();
            }
            else
            {
                newestRelease = GetNewestReleaseMono();
            }

            if (newestRelease != null)
            {
                var githubNewestVersion = Convert.ToDecimal(newestRelease.tag_name);

                if (githubNewestVersion > _thisVersion)
                {
                    if (MessageBox.Show(
                            "New version available!\n" + newestRelease.name + " (Current: " + version +
                            ")\n\nRelease notes:\n" + newestRelease.body +
                            "\n\nClick yes to download and install new release. ", "Update to " + newestRelease.name,
                            MessageBoxButtons.YesNo) == DialogResult.Yes
                    )
                    {
                        StartAppUpdate(newestRelease);
                    }
                }
                else
                {
                    Console.WriteLine("Application is up to date!");
                }
            }
            else
            {
                Console.WriteLine("Couldn't download release info.");
            }
        }

        public static void DoUpdateCleanup()
        {
            // Delete updater & DotNetZip from folder above if they exist
            string updater = Directory.GetParent(Application.StartupPath) + "/RWUpdater.exe";
            string ziplib = Directory.GetParent(Application.StartupPath) + "/DotNetZip.dll";

            try
            {
                if (File.Exists(updater))
                {
                    File.SetAttributes(updater, FileAttributes.Normal);
                    File.Delete(updater);
                    Console.WriteLine("Removed old updater");
                }

                if (File.Exists(ziplib))
                {
                    File.SetAttributes(ziplib, FileAttributes.Normal);
                    File.Delete(ziplib);
                    Console.WriteLine("Removed old zip DLL for updater");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update cleanup failed. Please delete RWUpdater.exe & DotNetZip.dll from the parent folder manually.\nError: " + e);
            }

        }

        private static void StartAppUpdate(GithubRelease newestRelease)
        {
            // Copy updater.exe  to folder above
            // Run updater.exe on folder above
            // Updater gets current folder & path to zip as arguments & starts
            // This app exists

            // Preparation
            Console.WriteLine("Downloading new release...");
            string zipPath = Directory.GetParent(Application.StartupPath) + "/" + newestRelease.assets[0].name;
            string newUpdaterPath = Directory.GetParent(Application.StartupPath) + "/RWUpdater.exe";

            //Download update
            MonoHelper.DownloadFile(newestRelease.assets[0].browser_download_url, zipPath,
                "MarkdownToRW_Converter");


            // Copy updater to folder above
            if (File.Exists(newUpdaterPath))
            {
                File.Delete(newUpdaterPath);
            }

            File.Copy(Application.StartupPath + "/RWUpdater.exe", newUpdaterPath);
            File.Copy(Application.StartupPath + "/DotNetZip.dll",
                Directory.GetParent(Application.StartupPath) + "/DotNetZip.dll");

            // Run updater & quit
            Process.Start(newUpdaterPath,
                MonoHelper.SurroundWithQuotes(Application.StartupPath) + " " + MonoHelper.SurroundWithQuotes(zipPath));
            Environment.Exit(0);
        }

        private static GithubRelease GetNewestRelease()
        {
            GithubRelease release = null;

            try
            {
                var http = new HttpClient();
                http.Request.UserAgent = "MarkdownToRW_Converter";
                http.Request.Accept = HttpContentTypes.ApplicationJson;
                var response = http.Get(RELEASE_URL);
                release = response.StaticBody<GithubRelease>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Update check failed:\n" + e);
            }

            return release;
        }

        private static GithubRelease GetNewestReleaseMono()
        {
            string outputFilePath = Application.StartupPath + @"/release.json";
            GithubRelease rel = null;

            if (File.Exists(Application.StartupPath + "/release.json"))
            {
                File.Delete(Application.StartupPath + "/release.json");
            }

            MonoHelper.DownloadFile(
                "https://api.github.com/repos/BlackDragonBE/MarkdownToRW_Converter/releases/latest", outputFilePath,
                "MarkdownToRW_Converter");

            // Read json
            if (File.Exists(Application.StartupPath + "/release.json"))
            {
                var json = "";
                using (StreamReader sr = new StreamReader(outputFilePath.Replace("\"", "")))
                {
                    json = sr.ReadToEnd();
                }

                rel = JsonConvert.DeserializeObject<GithubRelease>(json);

                File.Delete(Application.StartupPath + "/release.json");
            }

            return rel;
        }
    }
}