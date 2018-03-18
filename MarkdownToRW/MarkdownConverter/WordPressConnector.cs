using System;
using System.Collections.Generic;
using System.IO;
using System.Net;


namespace DragonMarkdown
{
    public static class WordPressConnector
    {
        private static readonly WordPressSiteConfig _config = new WordPressSiteConfig();

        public static void InitializeWordPress(string username, string password)
        {
            _config.Username = username;
            _config.Password = password;
            _config.BaseUrl = "https://www.raywenderlich.com";

            try
            {
                using (WordPressClient client = new WordPressClient(_config))
                {
                    string name = client.GetProfile().FirstName;
                    Console.WriteLine(name);
                }
            }
            catch (Exception exception)
            {
                Console.Write(exception);
            }
        }

        public static User GetUserProfile()
        {
            //WordPressSiteConfig config = new WordPressSiteConfig();
            //config.Username = username;
            //config.Password = password;
            //config.BaseUrl = "https://www.raywenderlich.com";

            try
            {
                using (WordPressClient client = new WordPressClient(_config))
                {
                    return client.GetProfile();
                }
            }
            catch (Exception exception)
            {
                Console.Write("Profile couldn't be loaded: " + exception);
            }

            return null;
        }

        public static void TestConnection()
        {
            try
            {
                WebRequest wr = WebRequest.Create("https://raywenderlich.com");
                Stream stream = wr.GetResponse().GetResponseStream();

                if (new StreamReader(stream).ReadToEnd() != "")
                {
                    Console.WriteLine("Connection to RW OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fullFilePaths">A list of full file paths that need to be uploaded.</param>
        /// <param name="localFilePaths">A list of local file paths</param>
        /// <returns>A dictionary of LocalFilePath & UploadResult that can be used for replacing or other operations.</returns>
        public static Dictionary<string, UploadResult> UploadFiles(List<string> fullFilePaths,
            List<string> localFilePaths)
        {
            Dictionary<string, UploadResult> fileLinkDictionary = new Dictionary<string, UploadResult>();

            for (var i = 0; i < fullFilePaths.Count; i++)
            {
                var result = UploadFile(fullFilePaths[i]);
                fileLinkDictionary.Add(localFilePaths[i], result);
            }

            return fileLinkDictionary;
        }

        public static UploadResult UploadFile(string path)
        {
            using (WordPressClient client = new WordPressClient(_config))
            {
                //string mimeType = "image/" + Path.GetExtension(path).ToLower();
                string mimeType = MimeMapper.MimeMap[Path.GetExtension(path).ToLower()];

                var image = Data.CreateFromFilePath(path, mimeType);

                var result = client.UploadFile(image);
                return result;
            }
        }

        /// <summary>
        /// Delete a post, attachment or page by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            using (WordPressClient client = new WordPressClient(_config))
            {
                return client.DeletePost(id);
            }
        }
    }
}