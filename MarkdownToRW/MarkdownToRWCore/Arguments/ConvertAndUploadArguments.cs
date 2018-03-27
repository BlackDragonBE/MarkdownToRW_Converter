using System;
using PowerArgs;

namespace MarkdownToRWCore
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [TabCompletion]
    public class ConvertAndUploadArguments
    {
        [ArgRequired(PromptIfMissing = true)]
        [ArgShortcut("i")]
        [ArgShortcut("input")]
        [ArgDescription("The path to the markdown file. (e.g. 'C:\\Users\\Me\\Desktop\\file.md')")]
        [PromptIfEmpty]
        [ArgPosition(1)]
        public string MarkdownPath { get; set; }

        [ArgShortcut("htmlFolder")]
        [ArgShortcut("o")]
        [ArgDescription(
            "(optional) HTML output path. Include .html in the path like so: PATH-TO-HTML-FOLDER/FILE.html. If this argument is omitted, the html file is saved next to the markdown file.")]
        public string HtmlPath { get; set; }

        [ArgDefaultValue(true)]
        [ArgShortcut("onlyHTML")]
        [ArgDescription(
            "After uploading the images, should only the html file be updated with new links? If false, the original markdown will be updated as well. (true/false)")]
        [ArgPosition(2)]
        public bool OnlyUpdateHtmlFile { get; set; }

        [ArgDescription(
            "(optional) Should the first image be right aligned? This is useful for the 250x250 image at the top of tutorials.")]
        [DefaultValue(true)]
        [ArgShortcut("rt")]
        public bool FirstImageRightAligned { get; set; }

        [ArgDescription("(optional) Should all images with an alt text be converted to captions?")]
        [DefaultValue(true)]
        [ArgShortcut("alt")]
        public bool ConvertImageWithAltToCaption { get; set; }

        [ArgShortcut("user")]
        [ArgDescription("RW WordPress Username.")]
        [ArgRequired(PromptIfMissing = true)]
        [ArgPosition(3)]
        public string Username { get; set; }

        [ArgShortcut("pw")]
        [ArgDescription("RW WordPress Password.")]
        [ArgRequired(PromptIfMissing = true)]
        [ArgPosition(4)]
        public string Password { get; set; }
    }
}