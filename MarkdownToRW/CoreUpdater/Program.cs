using System;
using System.IO;
using System.IO.Compression;

namespace CoreUpdater
{
    class Program
    {
        public static void Main(string[] args)
        {
            DoUpdate(args);
        }

        private static void DoUpdate(string[] args)
        {
            try
            {
                string folderPath = args[0];
                string zipPath = args[1];

                Console.WriteLine("Starting update...");

                string previousPath = folderPath;
                folderPath = Directory.GetParent(previousPath) + "/" + Path.GetFileNameWithoutExtension(zipPath);

                Directory.CreateDirectory(folderPath);

                Console.WriteLine("Unzipping update: " + zipPath);

                ZipFile.ExtractToDirectory(zipPath, folderPath);

                Console.WriteLine("Cleaning up...");

                File.Delete(zipPath);

                try
                {
                    Directory.Delete(previousPath, true);
                }
                catch (Exception)
                {
                    Console.WriteLine("Previous installation couldn't be fully removed, please remove this folder manually: " +
                                      previousPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed:");
                Console.WriteLine(e);
            }

            Console.WriteLine("Update succesful!");
        }
    }
}
