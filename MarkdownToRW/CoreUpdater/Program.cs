using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace CoreUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = args[0];
            string zipPath = args[1];
            string thisFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("Starting update...");
            Console.WriteLine("Removing previous version...");

            DeleteAllFilesInDirectory(folderPath);

            string previousPath = folderPath;
            folderPath = Directory.GetParent(previousPath) + Path.GetFileNameWithoutExtension(zipPath);
            Directory.Move(previousPath, folderPath);

            Console.WriteLine("Unzipping update: " + zipPath);
            
            ZipFile.ExtractToDirectory(zipPath, folderPath);

            Console.WriteLine("Cleaning up...");
            File.Delete(zipPath);

            Console.WriteLine("Update succesful! Press any key to quit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void DeleteAllFilesInDirectory(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
