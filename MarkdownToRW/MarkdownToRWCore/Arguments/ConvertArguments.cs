using PowerArgs;

namespace MarkdownToRWCore
{
    public class ConvertArguments
    {
        [ArgRequired(PromptIfMissing = true)]
        [ArgDescription("Markdown file path.")]
        [ArgPosition(1)]
        public string MarkdownPath { get; set;}

        [ArgDescription("(optional) HTML output path. Include .html in the path like so: PATH-TO-HTML-FOLDER/FILE.html. If this argument is omitted, the html file is saved next to the markdown file.")]
        [ArgPosition(2)]
        public string HtmlPath { get; set; }
    }
}