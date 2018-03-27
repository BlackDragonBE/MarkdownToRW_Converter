using DragonMarkdown.DragonConverter;

namespace MarkdownToRWGUI
{
    public class Settings
    {
        public string SettingsVersion;

        // Upload credentials
        public bool ShouldLoadCredentials = false;
        public string Username;
        public string Password;

        // Converter

        public bool RememberConverterSettings = false;
        public bool OutputToHtml = false;

        public ConverterOptions ConverterOptions = new ConverterOptions();
        //public bool FirstImageAlignedRight = true;

    }
}
