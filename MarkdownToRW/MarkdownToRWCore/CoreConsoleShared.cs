using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DragonMarkdown;

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
            Console.WriteLine(" │ CORE VERSION                                                vx.xx │".Replace("x.xx",DragonVersion.VERSION.ToString()));
            Console.WriteLine(" └───────────────────────────────────────────────────────────────────┘");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                    Developed by Eric Van de Kerckhove (BlackDragonBE)");
            Console.WriteLine("");
            Console.ResetColor();
        }

        public static void OpenHtmlResult(string generatedHtmlPath)
        {
            Console.WriteLine("Opening HTML file in default application...");
            DragonHelperUtility.OpenFileInDefaultApplication(generatedHtmlPath);
        }

        public static string GetExistingFilePath()
        {
            string path = DragonHelperUtility.RemoveAllQuotes(Console.ReadLine());

            if (File.Exists(path))
            {
                return path;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File " + path + " doesn't exist!");
            Console.ResetColor();
            Console.WriteLine("");
            return null;
        }

        public static string GetNewFilePath()
        {
            string path = DragonHelperUtility.RemoveAllQuotes(Console.ReadLine());

            var directoryName = Path.GetDirectoryName(path);

            if (Directory.Exists(directoryName) &&
                DragonHelperUtility.CheckFolderWritePermission(directoryName))
            {
                return path;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid folder, can't write to to: " + directoryName);
            Console.ResetColor();
            Console.WriteLine("");
            return null;
        }
    }
}
