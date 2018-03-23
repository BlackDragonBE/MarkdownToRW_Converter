using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MarkdownToRWGUI
{
    public static class SettingsManager
    {
        private static readonly string _fileName = "settings.xml";

        public static Settings LoadSettings()
        {
            Settings s = new Settings();

            if (File.Exists(_fileName))
            {
                var serializer = new XmlSerializer(typeof(Settings));

                using (var reader = XmlReader.Create(_fileName))
                {
                    var settings = (Settings) serializer.Deserialize(reader);
                    return settings;
                }
            }
            else
            {
                Console.WriteLine("No settings found, creating default one.");
                SaveSettings(new Settings());
            }

            return s;
        }

        public static void SaveSettings(Settings settings)
        {
            var serializer = new XmlSerializer(typeof(Settings));

            using (var writer = XmlWriter.Create(_fileName))
            {
                serializer.Serialize(writer, settings);
            }
        }
    }
}
