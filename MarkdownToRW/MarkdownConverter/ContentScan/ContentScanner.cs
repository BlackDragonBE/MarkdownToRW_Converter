using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMarkdown.ContentScan
{
    public class ScanResults
    {
        public int WordCount;
        public List<string> ProblemsFound = new List<string>();
    }
    
    public static class ContentScanner
    {
        public static ScanResults ScanMarkdown(string markdownText)
        {
            ScanResults results = new ScanResults();

            CountWords(ref results, markdownText);
            ScanSectionTitles(ref results, markdownText);
            
            return results;
        }

        public static string ParseScanrResults(ScanResults results)
        {
            string output = "Markdown Report";
            output += "-------------";
            output += "Word Count: " + results.WordCount;
            output += "\n\n";
            output += "Problems found:\n";

            foreach (string p in results.ProblemsFound)
            {
                output += "- " + p + "\n";
            }
            
            
            return output;
        }

        private static void CountWords(ref ScanResults results, string text)
        {
            char[] delimiters = { ' ', '\r', '\n','*','_','[',']','(',')' };
            results.WordCount = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            if (results.WordCount > 4000)
            {
                results.ProblemsFound.Add("Your text contains " + results.WordCount +". This is above the maximum word limit of 4000.");
            }
        }

        private static void ScanSectionTitles(ref ScanResults results, string markdown)
        {
            List<string> neededSections = new List<string>();
            neededSections.Add("Getting Started");
            neededSections.Add("Where To Go From Here");

            foreach (string neededSection in neededSections)
            {
                if (!markdown.Contains(neededSection))
                {
                    results.ProblemsFound.Add("Section not found: " + neededSection);
                }
            }
        }
    }
}
