using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using System.IO;
using System.Reflection;

namespace MedLaunch.Classes
{
    public class ConfigImport
    {
        // Properties
        public Paths _Paths { get; set; }
        public ConfigBaseSettings _ConfigBaseSettings { get; set; }
        public ConfigNetplaySettings _ConfigNetplaySettings { get; set; }
        public ConfigServerSettings _ConfigServerSettings { get; set; }

        // contructor
        public ConfigImport()
        {
            // populate config info from database
            _Paths = Paths.GetPaths();
            _ConfigNetplaySettings = ConfigNetplaySettings.GetNetplay();
            _ConfigServerSettings = ConfigServerSettings.GetServer(100);
            _ConfigBaseSettings = ConfigBaseSettings.GetConfig(2000000000);
        }

        public void ImportBaseConfig()
        {
            _ConfigBaseSettings = ConfigBaseSettings.GetConfig(2000000000);
        }

        public void LoadConfigFromDisk(int configId)
        {
            // get path
            string path = "";
            if (configId == 2000000000)
            {
                // base config
                path = _Paths.mednafenExe + "\\" + "mednafen-09x.cfg";
            }
            else
            {
                path = "";
            }

            List<string> configlist = LoadConfigFromDisk(path);
            if (configlist == null) { return; }

            // iterate through each line
            foreach (string s in configlist)
            {
                // split to array based on whitespace
                string st = s.TrimEnd();
                string[] arr = st.Split(' ');
                // normal (non controller) settings should only have 2 items in the array
                if (arr.Length != 2)
                {
                    return;
                }
                string propName = arr[0].Replace(".", "__");
                string propValue = arr[1];

                // look for property in the configbasesettings
                PropertyInfo p = _ConfigBaseSettings.GetType().GetProperty(propName);
                if (p != null)
                {
                    // property was found - update Config
                    InitWindow.SetPropertyValue(_ConfigBaseSettings, p, null, arr[1]);
                    continue;
                }
                // look for property in confignetplaysettings
                p = _ConfigNetplaySettings.GetType().GetProperty(propName);
                if (p != null)
                {
                    // property was found - update config
                    //InitWindow.SetPropertyValue(_ConfigNetplaySettings, p, null, arr[1]);
                }
                
            }
        }

        public List<string> LoadConfigFromDisk(string path)
        {
            // check whether file exists
            if (!File.Exists(path))
                return null;

            List<string> Config = new List<string>();

            // import text from config
            string text = File.ReadAllText(path);

            // make sure all line endings are \n
            text.Replace("\n\r", "\n").Replace("\r\n", "\n");
            List<string> textList = text.Split('\n').ToList();

            // first pass to remove comments
            Config = textList.Where(a => !a.StartsWith(";")).ToList();

            return Config;
        }
    }
}
