using System;
using PowerArgs;

namespace MarkdownToRWCore
{
    // PowerArgs usage:
    // https://github.com/adamabdelhamed/PowerArgs

    // A class that describes the command line arguments for this program

    internal class Program
    {
        private static void Main(string[] args)
        {
            CoreConsoleShared.WriteIntro();
            Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<MarkDownToRWProgram>());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                "Tab completion mode is active. Choose an action to execute and provide the needed arguments or only call the action and fill in the arguments seperately. You can also run actions from a console or terminal directly.");
            Console.WriteLine("Choose action: ");
            Console.ResetColor();
            Args.InvokeAction<MarkDownToRWProgram>(args);

            Console.ReadKey();
        }
    }
}