namespace DragonMarkdown.DragonWordPressXml
{
    /// <summary>
    /// Configuration for WordPress site to connect to. Be sure to include www in the URL. E.g.: https://www.mywordpress.com
    /// </summary>
    public class WordPressConfig
    {
        public string BlogID = "1";
        public string Password;
        public string Username;
        public string WordPressURL; // Be sure to include www as well! e.g.: https://www.mywordpress.com

        public WordPressConfig(string wordPressUrl, string username, string password, string blogId = "1")
        {
            WordPressURL = wordPressUrl;
            Username = username;
            Password = password;
            BlogID = blogId;
        }

        public string RequestUrl => WordPressURL + "/xmlrpc.php";
    }
}