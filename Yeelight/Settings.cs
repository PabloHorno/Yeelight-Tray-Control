using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Yeelight
{
    static class Settings
    {
        static private readonly string path = "settings.json";
        static private string Json
        {
            get
            {
               return File.ReadAllText(path);
            }
            set
            {
                File.WriteAllText(path, value);
            }
        }
        static public string IpAddress
        {
            get
            {
               return JsonConvert.DeserializeObject<SettingsContainer>(Json).IpAddress;
            }
            set
            {
                var temp = JsonConvert.DeserializeObject<SettingsContainer>(Json);
                temp.IpAddress = value;
                Json = JsonConvert.SerializeObject(temp);
            }
        }
    }

    class SettingsContainer
    {
        public string IpAddress;
    }
}
