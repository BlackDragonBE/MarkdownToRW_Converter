using System;
using System.IO;
using DragonMarkdown;

namespace MarkdownToRWCore
{
    class Program
    {
        static void Main(string[] args)
        {
            InteractiveConsole.StartInteractive();
            return;

            if (args[0] == "interactive" ||  args.Length < 1)
            {
                Console.WriteLine("No arguments found.\nFirst argument is markdown input path. Optional second argument is html output path.");
                Console.WriteLine("--------------");
                Console.WriteLine("Starting in interactive mode....");
                Console.WriteLine("--------------");

                InteractiveConsole.StartInteractive();
                return;
            }

            string markdownPath = args[0];
            Console.WriteLine("Starting conversion...");

            if (args.Length == 2) // 2 arguments
            {
                Converter.ConvertMarkdownFileToHtmlFile(markdownPath, args[1]);
                Console.WriteLine("Conversion succesful!");
                Console.WriteLine("Saved RW ready html to " + args[1]);
            }
            else if (args.Length == 1)
            {
                string outputPath = DragonHelperUtility.GetFullPathWithoutExtension(markdownPath) + ".html";
                Converter.ConvertMarkdownFileToHtmlFile(markdownPath, outputPath);
                Console.WriteLine("Conversion succesful!");
                Console.WriteLine("Saved RW ready html to " + outputPath);
            }
        }





    }
}
