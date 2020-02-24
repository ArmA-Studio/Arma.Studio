using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Arma.Studio
{
    [XmlRoot("config")]
    public class Configuration
    {
        public bool UserIdentificationDialogWasDisplayed { get; set; } = false;
        public bool OptOutOfReportingAndUpdates { get; set; } = false;
        public string UserIdentifier { get; set; } = String.Empty;
        public Guid UserGuid { get; set; } = default;


        [XmlIgnore]
        public static Configuration Instance { get; private set; }
        private Configuration()
        {
        }
        public static void Load(string fpath)
        {
            if (!File.Exists(fpath))
            {
                Instance = new Configuration();
                return;
            }

            try
            {
                using (var stream = File.Open(fpath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(Configuration));
                    Instance = serializer.Deserialize(stream) as Configuration;
                    if (Instance == null)
                    {
                        Instance = new Configuration();
                    }
                }
            }
            catch (Exception ex)
            {
                App.DisplayOperationFailed(ex, Properties.Language.Configuration_ExceptionDuringLoad);
                Instance = new Configuration();
            }
            if (Instance.UserGuid == default)
            {
                Instance.UserGuid = Guid.NewGuid();
                Save(fpath);
            }
        }
        public static void Save(string fpath)
        {
            var dir = Path.GetDirectoryName(fpath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (var stream = File.Open(fpath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(stream, Instance);
            }
        }
    }

}
