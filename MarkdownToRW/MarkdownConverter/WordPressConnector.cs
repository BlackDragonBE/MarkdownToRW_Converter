using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DragonMarkdown.DragonWordPressXml;
using DragonMarkdown.DragonWordPressXml.Requests;
using DragonMarkdown.DragonWordPressXml.Responses;

namespace DragonMarkdown
{
    public static class WordPressConnector
    {
        private static WordPressConfig _config;

        public static void InitializeWordPress(string username, string password)
        {
            _config = new WordPressConfig("https://www.raywenderlich.com", username, password);
        }

        public static void TestConnection()
        {
            try
            {
                WebRequest wr = WebRequest.Create("https://www.raywenderlich.com");
                Stream stream = wr.GetResponse().GetResponseStream();

                if (new StreamReader(stream).ReadToEnd() != "")
                    Console.WriteLine("Connection to RW OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static GetProfileResponse GetUserProfile()
        {
            try
            {
                DragonWordPressClient client = new DragonWordPressClient(_config);
                return client.SendGetProfileRequest(new GetProfileRequest());
            }
            catch (Exception exception)
            {
                Console.Write("Profile couldn't be loaded: " + exception);
            }

            return null;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="fullFilePaths">A list of full file paths that need to be uploaded.</param>
        /// <param name="localFilePaths">A list of local file paths</param>
        /// <returns>A dictionary of LocalFilePath & UploadFileResponse that can be used for replacing or other operations.</returns>
        public static Dictionary<string, UploadFileResponse> UploadFiles(List<string> fullFilePaths,
            List<string> localFilePaths)
        {
            Dictionary<string, UploadFileResponse> fileLinkDictionary = new Dictionary<string, UploadFileResponse>();

            for (var i = 0; i < fullFilePaths.Count; i++)
            {
                var result = UploadFile(fullFilePaths[i]);
                fileLinkDictionary.Add(localFilePaths[i], result);
            }

            return fileLinkDictionary;
        }

        public static UploadFileResponse UploadFile(string path)
        {
            DragonWordPressClient client = new DragonWordPressClient(_config);

            return client.SendUploadFileRequest(new UploadFileRequest(path));
        }

        /// <summary>
        ///     Delete a post, attachment or page by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            DragonWordPressClient client = new DragonWordPressClient(_config);
            return client.SendDeletePostRequest(new DeletePostRequest(id)).Success;
        }
    }
}