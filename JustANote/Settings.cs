using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace JustANote
{
    public class Settings
    {
        public static readonly Settings Default;

        private static readonly string SettingFile = @".\settings.xml";

        static Settings()
        {
            if (File.Exists(SettingFile))
            {
                using (var reader = File.OpenText(SettingFile))
                {
                    var serializer = new XmlSerializer(typeof (Settings));
                    Default = (Settings) serializer.Deserialize(reader);
                    Default.Note = Default.Note.Replace("\n", "\r\n");
                }
            }
            else
            {
                Default = new Settings();
            }
        }

        public Rectangle NoteWindow { get; set; }
        
        public string Note { get; set; }

        public string OrgFile { get; set; }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(Settings));
            Note = Note.Replace("\r\n", "\n");
            using (var writer = new StreamWriter(SettingFile, false))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
