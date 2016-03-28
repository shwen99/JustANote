using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JustANote
{
    public class Settings
    {
        public static readonly Settings Default;

        static Settings()
        {
            if (File.Exists(".\\settings.xml"))
            {
                using (var reader = File.OpenText(".\\settings.xml"))
                {
                    var serializer = new XmlSerializer(typeof(Settings));
                    Default = (Settings)serializer.Deserialize(reader);
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

            using (var writer = new StreamWriter(".\\settings.xml", false))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
