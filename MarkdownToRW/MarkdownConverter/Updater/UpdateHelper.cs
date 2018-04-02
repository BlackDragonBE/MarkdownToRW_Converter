using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DragonMarkdown.Utility;
using Newtonsoft.Json;

namespace DragonMarkdown.Updater
{
    public static class UpdateHelper
    {
        private static decimal _thisVersion;

        private static readonly string RELEASE_URL =
            @"https://api.github.com/repos/BlackDragonBE/MarkdownToRW_Converter/releases/latest";

        public static GithubRelease CheckForUpdates()
        {
            _thisVersion = DragonVersion.VERSION;
            Console.WriteLine("Checking for new version...");

            GithubRelease newestRelease = null;

            newestRelease = GetNewestReleaseInfo();

            if (newestRelease != null)
            {
                var githubNewestVersion = Convert.ToDecimal(newestRelease.tag_name);

                if (githubNewestVersion > _thisVersion)
                {
                    Console.WriteLine("New version available!\n" + newestRelease.name + " (Current: v" + _thisVersion +
                                      ")\n\nRelease notes:\n" + newestRelease.body + "\n\nDownload: " +
                                      newestRelease.html_url);
                    return newestRelease;
                }
                Console.WriteLine("Application is up to date!");
            }
            else
            {
                Console.WriteLine("Couldn't download release info.");
            }

            return null;
        }

        private static GithubRelease GetNewestReleaseInfo()
        {
            GithubRelease release = null;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "MarkdownToRW_Converter");
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<HttpResponseMessage> response = client.GetAsync(RELEASE_URL);

                release = JsonConvert.DeserializeObject<GithubRelease>(response.Result.Content.ReadAsStringAsync()
                    .Result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Update check failed:\n" + e);
            }

            return release;
        }

        public static void ExtractZipToFolder(string zipPath, string extractFolderPath)
        {
            if (Directory.Exists(extractFolderPath))
            {
                Directory.Delete(extractFolderPath, true);
            }

            Directory.CreateDirectory(extractFolderPath);


            ZipFile.ExtractToDirectory(zipPath, extractFolderPath);
        }

        public static async Task UpdateApp(GithubRelease release, IProgress<SimpleTaskProgress> taskProgress)
        {
            Console.WriteLine("Starting update...");
            List<GithubRelease.Asset> assets = release.assets.Where(asset => asset.name.Contains("GUI")).ToList();

            GithubRelease.Asset downloadAsset = GetAssetMatchingRunningVersion(assets);

            string downloadUrl = downloadAsset.browser_download_url;
            string zipPath = Directory.GetParent(DragonUtil.CurrentDirectory).FullName + "/" + downloadAsset.name;
            string updaterFolder = Directory.GetParent(DragonUtil.CurrentDirectory).FullName + "/CoreUpdater";

            if (downloadUrl == null)
            {
                return;
            }

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            
            Console.WriteLine("Downloading " + downloadUrl + "...");
            var progress = new Progress<SimpleTaskProgress>();
            progress.ProgressChanged += (sender, simpleTaskProgress) => taskProgress.Report(simpleTaskProgress);
            await DragonUtil.DownloadFile(downloadUrl, zipPath, "MarkdownToRW_Converter", downloadAsset.size / 1024, progress);
            
            // extract updater zip to parent folder
            ExtractZipToFolder(DragonUtil.CurrentDirectory + "/CoreUpdater.zip", updaterFolder);

            // run updater, point to this folder & downloaded zip
            if (DragonUtil.CurrentOperatingSystem.IsMacOS() || DragonUtil.CurrentOperatingSystem.IsLinux())
            {
                DragonUtil.TryToMakeExecutable(updaterFolder + "/CoreUpdater");
            }

            RunCoreUpdater(updaterFolder, zipPath);

            // close app
            Environment.Exit(0);
        }


        private static void RunCoreUpdater(string updaterFolder, string zipPath)
        {
            Console.WriteLine("Starting updater process...");

            ProcessStartInfo processInfo = new ProcessStartInfo()
            {
                ErrorDialog = true,
                UseShellExecute = true
            };

            if (DragonUtil.IsRunningPortable())
            {
                processInfo.FileName = "dotnet";
                //processInfo.Arguments = "dotnet'";
                processInfo.Arguments += DragonUtil.SurroundWithQuotes(updaterFolder + "/CoreUpdater.dll");
            }
            else
            {
                if (DragonUtil.CurrentOperatingSystem.IsWindows())
                {
                    processInfo.FileName = DragonUtil.SurroundWithQuotes(updaterFolder + "/CoreUpdater.exe");
                }
                else if (DragonUtil.CurrentOperatingSystem.IsMacOS())
                {
                    processInfo.FileName = DragonUtil.SurroundWithSingleQuotes(updaterFolder + "/CoreUpdater");
                }
                else if (DragonUtil.CurrentOperatingSystem.IsLinux())
                {
                    processInfo.FileName = DragonUtil.SurroundWithSingleQuotes(updaterFolder + "/CoreUpdater");
                }
            }

            processInfo.Arguments += " " + DragonUtil.SurroundWithSingleQuotes(DragonUtil.CurrentDirectory) + " " +
                                    DragonUtil.SurroundWithSingleQuotes(zipPath);

            Console.WriteLine(processInfo.FileName + processInfo.Arguments);

            Process process = new Process { StartInfo = processInfo };
            process.Start();
        }

        private static GithubRelease.Asset GetAssetMatchingRunningVersion(List<GithubRelease.Asset> assets)
        {
            GithubRelease.Asset releaseAsset = null;
            if (DragonUtil.IsRunningPortable())
            {
                Console.WriteLine("Portable version");
                releaseAsset = assets.Find(asset => asset.name.Contains("Portable"));
            }
            else
            {
                if (DragonUtil.CurrentOperatingSystem.IsWindows())
                {
                    releaseAsset = assets.Find(asset => asset.name.Contains("Windows"));
                }
                else if (DragonUtil.CurrentOperatingSystem.IsMacOS())
                {
                    releaseAsset = assets.Find(asset => asset.name.Contains("macOS"));
                }
                else if (DragonUtil.CurrentOperatingSystem.IsLinux())
                {
                    releaseAsset = assets.Find(asset => asset.name.Contains("linux"));
                }
            }
            return releaseAsset;
        }
    }
}