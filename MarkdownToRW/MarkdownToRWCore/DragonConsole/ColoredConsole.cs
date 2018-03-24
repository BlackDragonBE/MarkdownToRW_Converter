using System;

namespace MarkdownToRWCore.DragonConsole
{
    public static class ColoredConsole
    {
        public static void WriteLineWithColor(string text, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;

        }
    }
}
