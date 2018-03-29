using System;
using System.Collections.Generic;
using System.IO;
using DragonMarkdown;
using DragonMarkdown.DragonConverter;
using DragonMarkdown.DragonWordPressXml.Responses;
using DragonMarkdown.Utility;
using MarkdownToRWCore.DragonConsole;

namespace MarkdownToRWCore
{
    public static class InteractiveConsole
    {
        public static void StartInteractive()
        {
            Console.WriteLine(" +-------------------------------------+");
            Console.WriteLine(" |                                     |");
            Console.WriteLine(" |          Interactive Wizard         |");
            Console.WriteLine(" |                                     |");
            Console.WriteLine(" +-------------------------------------+");

            string markdownPath = null;
            string htmlPath = null;

            while (markdownPath == null)
            {
                Console.WriteLine("Please provide the path to the markdown file:");
                markdownPath = CoreConsoleShared.GetExistingFilePath();
            }

            string firstImageRight = null;

            while (firstImageRight != "y" && firstImageRight != "n")
            {
                Console.WriteLine(
                    "Should the first image be right aligned? This is useful for the 250x250 image at the top of tutorials. (y/n)");
                firstImageRight = Console.ReadLine();
            }

            string convertImagesWithAlt = null;

            while (convertImagesWithAlt != "y" && convertImagesWithAlt != "n")
            {
                Console.WriteLine(
                    "Should all images with an alt text be converted to captions? (y/n)");
                convertImagesWithAlt = Console.ReadLine();
            }

            string sameFolder = null;

            while (sameFolder != "y" && sameFolder != "n")
            {
                Console.WriteLine(
                    "Do you want to output the HTML file containing the WordPress ready code to the same folder as the markdown file? (y/n)");
                sameFolder = Console.ReadLine();
            }

            if (sameFolder == "y")
            {
                htmlPath = DragonUtil.GetFullPathWithoutExtension(markdownPath) + ".html";
            }
            else
            {
                while (htmlPath == null || htmlPath == markdownPath)
                {
                    if (htmlPath == markdownPath)
                    {
                        Console.WriteLine(
                            "Output file can't be the same as the input file! This WILL lead to data loss.");
                    }

                    Console.WriteLine("Please provide the location and name for the output html file:");
                    Console.WriteLine("(e.g. C:\\MyNewFile.html)");
                    htmlPath = CoreConsoleShared.GetNewFilePath();
                }
            }

            string uploadImages = null;

            while (uploadImages != "y" && uploadImages != "n" && uploadImages != "only html")
            {
                Console.WriteLine(
                    "Do you want to upload all images and update the links in both the markdown file and the generated html? (y/only html/n)");
                uploadImages = Console.ReadLine();
            }

            string ready = null;

            while (ready != "y" && ready != "n")
            {
                Console.WriteLine("Ready to start the conversion!");
                Console.WriteLine("");
                Console.WriteLine("Parameters:");
                Console.WriteLine("Markdown file path (input): " + markdownPath);
                Console.WriteLine("Generated HTML file path (output): " + htmlPath);
                Console.WriteLine("Upload images and update markdown and generated HTML file: " + uploadImages);
                Console.WriteLine("");
                Console.WriteLine("Are these parameters correct? (y/n)");
                ready = Console.ReadLine();
            }

            if (ready == "n")
            {
                Console.WriteLine("Conversion aborted.");
                return;
            }

            ConverterOptions options = new ConverterOptions
            {
                FirstImageIsAlignedRight = CoreConsoleShared.YesNoToBool(firstImageRight)
            };

            switch (uploadImages)
            {
                case "y":
                    DoConversion(markdownPath, htmlPath, true, false, options);
                    break;
                case "only html":
                    DoConversion(markdownPath, htmlPath, true, true, options);
                    break;
                case "n":
                    DoConversion(markdownPath, htmlPath, false, false, options);
                    break;
            }
        }

        private static void DoConversion(string markdownPath, string htmlPath, bool uploadImages, bool onlyUpdateHtml, ConverterOptions options)
        {
            Console.WriteLine("------------");
            Console.WriteLine("Converting...");
            Converter.ConvertMarkdownFileToHtmlFile(markdownPath, htmlPath, options);
            Console.WriteLine("Markdown converted!");

            if (uploadImages)
            {
                UploadImages(markdownPath, htmlPath, onlyUpdateHtml);
            }

            ConversionFinished(htmlPath);
        }

        private static bool UploadImages(string markdownPath, string htmlPath, bool onlyUpdateHtml)
        {
            Console.WriteLine("------------");
            GetProfileResponse user = null;
            Console.WriteLine("Starting image upload. Please enter your RW WordPress credentials:");

            while (user == null)
            {
                user = AskForCredentials(user);
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
                Console.WriteLine("No images found. Skipping upload.");
                ConversionFinished(htmlPath);
                return true;
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
            Converter.ReplaceLocalImageLinksWithUrls(markdownPath, markdownText, htmlPath, htmlText, onlyUpdateHtml, localImagePaths, imageUrls);
            return true;
        }

        private static GetProfileResponse AskForCredentials(GetProfileResponse user)
        {
            Console.WriteLine("Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Pasword:");
            string password = Console.ReadLine();

            WordPressConnector.InitializeWordPress(username, password);
            user = WordPressConnector.GetUserProfile();

            if (user == null)
            {
                ColoredConsole.WriteLineWithColor("Incorrect credentials / can't connect to RW WordPress.", ConsoleColor.Red);
                Console.WriteLine("Please try again.");
            }
            return user;
        }

        private static void ConversionFinished(string generatedHtmlPath)
        {
            Console.WriteLine("------------");
            Console.WriteLine("All tasks completed! Thanks for using this app! :]");

            string openHtml = null;

            while (openHtml != "y" && openHtml != "n")
            {
                Console.WriteLine(
                    "Do you want to open the generated HTML file so you can copy its source to the WordPress post now? (y/n)");
                openHtml = Console.ReadLine();
            }

            if (openHtml == "y")
            {
                CoreConsoleShared.OpenHtmlResult(generatedHtmlPath);
            }

            CoreConsoleShared.PauseAndQuit();
        }


    }
}