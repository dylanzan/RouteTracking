using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RouteTracking.utils
{
    class ConfigUtils
    {
        //init config dict
        public static Dictionary<string, string> ConfigDict = new Dictionary<string, string>();

        public ConfigUtils()
        {
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);

            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                 ConfigDict.Add(key.ToString(), config.AppSettings.Settings[key.ToString()].Value.ToString());
            }
        }

    }
}
