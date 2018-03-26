using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DragonMarkdown;
using DragonMarkdown.Utility;
using MarkdownToRWCore.DragonConsole;

namespace MarkdownToRWCore
{
    public static class CoreConsoleShared
    {
        public static void WriteIntro()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine(" ┌───────────────────────────────────────────────────────────────────┐");
            Console.WriteLine(" │  _____         _     _                  _____        _____ _ _ _  │");
            Console.WriteLine(" │ |     |___ ___| |_ _| |___ _ _ _ ___   |_   _|___   | __  | | | | │");
            Console.WriteLine(" │ | | | | .'|  _| '_| . | . | | | |   |    | | | . |  |    ─| | | | │");
            Console.WriteLine(" │ |_|_|_|__,|_| |_,_|___|___|_____|_|_|    |_| |___|  |__|__|_____| │");
            Console.WriteLine(" │                                                                   │");
            Console.WriteLine(" │ CORE CONSOLE VERSION                                        vx.xx │".Replace("x.xx", DragonVersion.VERSION.ToString()));
            Console.WriteLine(" └───────────────────────────────────────────────────────────────────┘");

            Console.Write("                    Developed by Eric Van de Kerckhove (BlackDragonBE)");
            Console.WriteLine("");
            Console.WriteLine("");

        }

        public static void OpenHtmlResult(string generatedHtmlPath)
        {
            Console.WriteLine("Opening HTML file in default application...");
            DragonUtil.OpenFileInDefaultApplication(generatedHtmlPath);
        }

        public static string GetExistingFilePath()
        {
            string path = DragonUtil.RemoveAllQuotes(Console.ReadLine());

            if (File.Exists(path))
            {
                return path;
            }

            ColoredConsole.WriteLineWithColor("File " + path + " doesn't exist!", ConsoleColor.Red);
            Console.WriteLine("");
            return null;
        }

        public static string GetNewFilePath()
        {
            string path = DragonUtil.RemoveAllQuotes(Console.ReadLine());

            var directoryName = Path.GetDirectoryName(path);

            if (Directory.Exists(directoryName) &&
                DragonUtil.CheckFolderWritePermission(directoryName))
            {
                return path;
            }

            ColoredConsole.WriteLineWithColor("Invalid folder, can't write to to: " + directoryName, ConsoleColor.Red);

            Console.WriteLine("");
            return null;
        }

        public static void StartFileDeletion(List<string> imageIDs)
        {
            Console.WriteLine("------------------------------------------------");

            Console.WriteLine("               _                            __ ");
            Console.WriteLine(" ____         | |_      _____         _    |  |");
            Console.WriteLine("|    \\ ___ ___|_| |_   |  _  |___ ___|_|___|  |");
            Console.WriteLine("|  |  | . |   | |  _|  |   __| .'|   | |  _|__|");
            Console.WriteLine("|____/|___|_|_| |_|    |__|  |__,|_|_|_|___|__|");
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------");

            Console.WriteLine("Open source Hacked Core Restorative Automatic Program (OH CRAP) v2");
            Console.WriteLine("------------");
            Console.WriteLine("Deleting uploaded images...");

            foreach (string iD in imageIDs)
            {
                var result = WordPressConnector.Delete(Convert.ToInt32(iD));
                if (result)
                {
                    Console.WriteLine("Deleted file with id " + iD);
                }
                else
                {
                    ColoredConsole.WriteLineWithColor("Failed to delete file with id " + iD, ConsoleColor.Red);
                }
            }

            Console.WriteLine("Cleanup complete! Press any key to exit.");
            Console.ReadKey();
        }

        public static void QuitConsole()
        {
            Environment.Exit(0);
        }

        public static void PauseAndQuit()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            QuitConsole();
        }

        public static bool YesNoToBool(string yesNo)
        {
            if (yesNo.ToLower() == "y" || yesNo.ToLower() == "yes")
            {
                return true;
            }

            return false;
        }
    }
}
