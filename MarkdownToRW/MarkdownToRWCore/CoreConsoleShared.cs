using System;
using System.Collections.Generic;
using System.Text;

namespace MarkdownToRWCore
{
    public static class CoreConsoleShared
    {
        public static void WriteIntro()
        {
            Console.WriteLine("");
            Console.WriteLine(" ┌───────────────────────────────────────────────────────────────────┐");
            Console.WriteLine(" │  _____         _     _                  _____        _____ _ _ _  │");
            Console.WriteLine(" │ |     |___ ___| |_ _| |___ _ _ _ ___   |_   _|___   | __  | | | | │");
            Console.WriteLine(" │ | | | | .'|  _| '_| . | . | | | |   |    | | | . |  |    ─| | | | │");
            Console.WriteLine(" │ |_|_|_|__,|_| |_,_|___|___|_____|_|_|    |_| |___|  |__|__|_____| │");
            Console.WriteLine(" │                                                                   │");
            Console.WriteLine(" │ CORE VERSION                                                vx.xx │".Replace("x.xx","1.01"));
            Console.WriteLine(" └───────────────────────────────────────────────────────────────────┘");
            Console.WriteLine(" ");
            Console.WriteLine("Markdown To RW Wordpress HTML Converter [.NET Core Version]");
            Console.WriteLine("Made by Eric Van de Kerckhove (BlackDragonBE)");
            Console.WriteLine("");
        }
    }
}
