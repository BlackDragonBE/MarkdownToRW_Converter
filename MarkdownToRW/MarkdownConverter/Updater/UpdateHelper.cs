using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
                        ")\n\nRelease notes:\n" + newestRelease.body + "\n\nDownload: " + newestRelease.html_url);
                    return newestRelease;
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
                
                release = JsonConvert.DeserializeObject<GithubRelease>(response.Result.Content.ReadAsStringAsync().Result);

            }
            catch (Exception e)
            {
                Console.WriteLine("Update check failed:\n" + e);
            }

            return release;
        }

        public static void ExtractZipToFolder(string zipPath, string extractFolderPath)
        {
            if (!Directory.Exists(extractFolderPath))
            {
                Directory.CreateDirectory(extractFolderPath);
            }

            ZipFile.ExtractToDirectory(zipPath, extractFolderPath);
        }
    }
}