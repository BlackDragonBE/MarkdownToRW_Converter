using System;
using System.IO;
using DragonMarkdown;
using PowerArgs;

namespace MarkdownToRWCore
{
    // PowerArgs usage:
    // https://github.com/adamabdelhamed/PowerArgs

    // A class that describes the command line arguments for this program
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class MyArgs
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        // This argument is required and if not specified the user will 
        // be prompted.
        [ArgRequired(PromptIfMissing = true), ArgShortcut("i"), ArgDescription("The path to the markdown file.")]
        public string MarkdownPath { get; set; }

        [ArgRequired(PromptIfMissing = true), ArgShortcut("s"), ArgDescription("Put the html file in the same directory as the markdown file?")]
        public bool SameDir { get; set; }

        // This non-static Main method will be called and it will be able to access the parsed and populated instance level properties.
        public void Main()
        {
            Console.WriteLine("You entered '{0}' and '{1}'", this.MarkdownPath, this.SameDir);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                InteractiveConsole.StartInteractive();
            }
            else
            {
                try
                {
                    Args.InvokeMain<MyArgs>(args);
                }
                catch (ArgException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("");
                    Console.WriteLine("Enter -? for help on the proper usage.");
                }
            }
/*
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
*/
        }





    }
}
