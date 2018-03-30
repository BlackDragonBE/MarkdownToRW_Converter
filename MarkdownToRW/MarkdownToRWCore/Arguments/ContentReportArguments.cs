using System;
using System.Collections.Generic;
using System.Text;
using PowerArgs;

namespace MarkdownToRWCore.Arguments
{
    public class ContentReportArguments
    {
        [ArgRequired(PromptIfMissing = true)]
        [ArgDescription("Markdown file path.")]
        [ArgPosition(1)]
        public string MarkdownPath { get; set; }

    }
}
