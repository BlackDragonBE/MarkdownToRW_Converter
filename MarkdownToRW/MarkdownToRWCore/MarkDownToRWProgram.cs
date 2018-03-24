using System;
using System.Collections.Generic;
using System.IO;
using DragonMarkdown;
using DragonMarkdown.DragonWordPressXml.Responses;
using DragonMarkdown.Utility;
using MarkdownToRWCore.DragonConsole;
using PowerArgs;

namespace MarkdownToRWCore
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [TabCompletion]
    public class MarkDownToRWProgram
    {
        [HelpHook]
        [ArgShortcut("-?")]
        [ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod]
        [ArgDescription("Start the interactive wizard. (recommended)")]
        public void Wizard()
        {
            InteractiveConsole.StartInteractive();
        }

        [ArgActionMethod]
        [ArgDescription("Convert a markdown file to RW WordPress ready HTML.")]
        public void SimpleConvert(ConvertArguments args)
        {
            ConvertToHtml(args.MarkdownPath, args.HtmlPath);
            CoreConsoleShared.PauseAndQuit();
        }

        [ArgActionMethod]
        [ArgDescription("Convert a markdown file to RW WordPress ready HTML, upload its images to WordPress and replace local image paths with the new URLs.")]
        public void ConvertAndUpload(ConvertAndUploadArguments args)
        {
            string htmlPath = ConvertToHtml(args.MarkdownPath, args.HtmlPath);

            Console.WriteLine("Starting image upload...");

            if (UploadImages(args.MarkdownPath, htmlPath, args.OnlyUpdateHtmlFile, args.Username, args.Password))
            {
                Console.WriteLine("Uploaded all images succesfully! The links were also replaced. Enjoy!");
            }
            else
            {
                ColoredConsole.WriteLineWithColor("ERROR: Something went wrong while uploading.", ConsoleColor.Red);
            }

            CoreConsoleShared.PauseAndQuit();
        }

        [ArgActionMethod]
        [ArgShortcut("q")]
        [ArgShortcut("-q")]
        [ArgDescription("Exits the application.")]
        public void Quit()
        {
            Environment.Exit(0);
        }

        public string ConvertToHtml(string markdownPath, string htmlPath)
        {
            Console.WriteLine("Converting " + markdownPath + " to RW WordPress ready HTML...");

            if (htmlPath == null)
            {
                htmlPath = DragonUtil.GetFullPathWithoutExtension(markdownPath) + ".html";
            }

            if (DragonUtil.CheckFolderWritePermission(Path.GetDirectoryName(htmlPath)))
            {
                Converter.ConvertMarkdownFileToHtmlFile(markdownPath, htmlPath);
                Console.WriteLine("Saved HTML to custom location: " + htmlPath);
            }
            else
            {
                ColoredConsole.WriteLineWithColor("Conversion aborted, can't write to " + htmlPath, ConsoleColor.Red);
            }

            return htmlPath;
        }

        private bool UploadImages(string markdownPath, string htmlPath, bool onlyUpdateHtml, string username, string password)
        {
            Console.WriteLine("Logging into RW WordPress..");
            WordPressConnector.InitializeWordPress(username, password);
            GetProfileResponse user = WordPressConnector.GetUserProfile();

            if (user == null)
            {
                ColoredConsole.WriteLineWithColor("Login failed. Please check your credentials and internet connection.", ConsoleColor.Red);
            }

            Console.WriteLine("");
            Console.WriteLine("Login succesful! Thanks for using the app, " + user.FirstName + "!");
            Console.WriteLine("Gathering files...");
            // Get image paths
            List<string> fullImagePaths = new List<string>();
            List<string> localImagePaths = new List<string>();


            string markdownText = "";
            string htmlText = "";

            using (StreamReader sr = new StreamReader(markdownPath))
            {
                markdownText = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(htmlPath))
            {
                htmlText = sr.ReadToEnd();
            }

            var links = Converter.FindAllImageLinksInHtml(htmlText, Path.GetDirectoryName(htmlPath));

            if (links.Count == 0)
            {
                Console.WriteLine("No images found. Aborting upload");
                return false;
            }

            foreach (ImageLinkData link in links)
            {
                fullImagePaths.Add(link.FullImagePath);
                localImagePaths.Add(link.LocalImagePath);
            }

            Console.WriteLine("");
            Console.WriteLine(fullImagePaths.Count + " image paths found:");

            foreach (string path in fullImagePaths)
            {
                Console.WriteLine(path + " (" + new FileInfo(path).Length / 1024 + " kb)");
            }

            List<string> imageUrls = new List<string>();
            List<string> imageIDs = new List<string>();

            // Upload images
            for (var i = 0; i < fullImagePaths.Count; i++)
            {
                string path = fullImagePaths[i];

                Console.WriteLine("Uploading: " + " (" + (i + 1) + "/" + fullImagePaths.Count + ")" + path + "...");

                var result = WordPressConnector.UploadFile(path);

                if (result != null)
                {
                    imageUrls.Add(result.FileResponseStruct.Url);
                    imageIDs.Add(result.FileResponseStruct.Id.ToString());
                }
                else
                {
                    Console.WriteLine("Image upload failed! Aborting upload and going into file cleanup mode...");
                    CoreConsoleShared.StartFileDeletion(imageIDs);
                    return false;
                }
            }

            // Update markdown & html
            Console.WriteLine("Starting link replacer...");
            Converter.ReplaceLocalImageLinksWithUrls(markdownPath, htmlPath, onlyUpdateHtml, markdownText, localImagePaths,
                imageUrls);
            return true;
        }

    }
}