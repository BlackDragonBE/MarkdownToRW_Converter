using System;
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
            Console.WriteLine("Converting " + args.MarkdownPath + " to RW WordPress ready HTML...");

            if (args.HtmlPath != null)
            {
                Console.WriteLine("Saving HTML to custom location: " + args.HtmlPath);
            }
        }

        [ArgActionMethod]
        [ArgDescription("Convert a markdown file to RW WordPress ready HTML, upload its images to WordPress and replace local image paths with the new URLs.")]
        public void ConvertAndUpload(ConvertAndUploadArguments args)
        {
            
        }
    }
}