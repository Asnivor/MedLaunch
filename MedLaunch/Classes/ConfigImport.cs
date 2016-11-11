using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using System.IO;
using System.Reflection;
using MahApps.Metro.Controls.Dialogs;

namespace MedLaunch.Classes
{
    public class ConfigImport
    {
        // Properties
        public Paths _Paths { get; set; }        
        public ConfigNetplaySettings _ConfigNetplaySettings { get; set; }
        public ConfigServerSettings _ConfigServerSettings { get; set; }
        public ConfigBaseSettings _ConfigBaseSettings { get; set; }
        public ConfigBaseSettings _ConfigGbSettings { get; set; }
        public ConfigBaseSettings _ConfigGbaSettings { get; set; }
        public ConfigBaseSettings _ConfigLynxSettings { get; set; }
        public ConfigBaseSettings _ConfigMdSettings { get; set; }
        public ConfigBaseSettings _ConfigGgSettings { get; set; }        
        public ConfigBaseSettings _ConfigNgpSettings { get; set; }
        public ConfigBaseSettings _ConfigPceSettings { get; set; }
        public ConfigBaseSettings _ConfigPcfxSettings { get; set; }
        public ConfigBaseSettings _ConfigPsxSettings { get; set; }
        public ConfigBaseSettings _ConfigSmsSettings { get; set; }
        public ConfigBaseSettings _ConfigNesSettings { get; set; }
        public ConfigBaseSettings _ConfigSnesSettings { get; set; }
        public ConfigBaseSettings _ConfigSsSettings { get; set; }
        public ConfigBaseSettings _ConfigVbSettings { get; set; }
        public ConfigBaseSettings _ConfigWswanSettings { get; set; }
        public ConfigBaseSettings _ConfigSnes_faustSettings { get; set; }
        public ConfigBaseSettings _ConfigPce_fastSettings { get; set; }

        // contructor
        public ConfigImport()
        {
            // populate config info from database
            _Paths = Paths.GetPaths();
            _ConfigNetplaySettings = ConfigNetplaySettings.GetNetplay();
            _ConfigServerSettings = ConfigServerSettings.GetServer(100);
            _ConfigBaseSettings = ConfigBaseSettings.GetConfig(2000000000);
            _ConfigGbSettings = ConfigBaseSettings.GetConfig(2000000001);
            _ConfigGbaSettings = ConfigBaseSettings.GetConfig(2000000002);
            _ConfigLynxSettings = ConfigBaseSettings.GetConfig(2000000003);
            _ConfigMdSettings = ConfigBaseSettings.GetConfig(2000000004);
            _ConfigGgSettings = ConfigBaseSettings.GetConfig(2000000005);
            _ConfigNgpSettings = ConfigBaseSettings.GetConfig(2000000006);
            _ConfigPceSettings = ConfigBaseSettings.GetConfig(2000000007);
            _ConfigPcfxSettings = ConfigBaseSettings.GetConfig(2000000008);
            _ConfigPsxSettings = ConfigBaseSettings.GetConfig(2000000009);
            _ConfigSmsSettings = ConfigBaseSettings.GetConfig(2000000010);
            _ConfigNesSettings = ConfigBaseSettings.GetConfig(2000000011);
            _ConfigSnesSettings = ConfigBaseSettings.GetConfig(2000000012);
            _ConfigSsSettings = ConfigBaseSettings.GetConfig(2000000013);
            _ConfigVbSettings = ConfigBaseSettings.GetConfig(2000000014);
            _ConfigWswanSettings = ConfigBaseSettings.GetConfig(2000000015);
            _ConfigSnes_faustSettings = ConfigBaseSettings.GetConfig(2000000016);
            _ConfigPce_fastSettings = ConfigBaseSettings.GetConfig(2000000017);
        }

        public void ImportConfigsFromDisk(ProgressDialogController controller)
        {
            if (controller != null)
                controller.SetMessage("Importing Mednafen Configs from disk\nPlease wait.....");

            _ConfigBaseSettings = ConfigBaseSettings.GetConfig(2000000000);

            // get a list of current systems
            List<GSystem> systems = GSystem.GetSystems();

            // first import base config
            ImportBaseConfigFromDisk(null);            

            // now iterate through each system and search/import system specific config files
            foreach (GSystem sys in systems)
            {
                ImportSystemConfigFromDisk(null, sys);                
            }
        }

        public void ImportBaseConfigFromDisk(ProgressDialogController controller)
        {
            string cfgPath = _Paths.mednafenExe + @"\mednafen-09x.cfg";
            var config = LoadConfigFromDisk(cfgPath);
            if (config.Count > 0)
            {
                // data was returned - begin import
                if (controller != null)
                    controller.SetMessage("Importing mednafen-09x.cfg");
                ParseConfigIncoming(config, null);

                // now save to the database

                if (controller != null)
                    controller.SetMessage("Saving base settings to database");

                ConfigBaseSettings.SetConfig(_ConfigBaseSettings);
                ConfigNetplaySettings.SetNetplay(_ConfigNetplaySettings);
                ConfigServerSettings.SaveToDatabase(_ConfigServerSettings);
            }
        }

        public void ImportSystemConfigFromDisk(ProgressDialogController controller, GSystem sys)
        {
            _ConfigBaseSettings = ConfigBaseSettings.GetConfig(2000000000 + sys.systemId);
            if (_ConfigBaseSettings == null)
            {
                // invalid config id
                return;
            }

            string configPath = _Paths.mednafenExe + @"\" + sys.systemCode + ".cfg";
            var specCfg = LoadConfigFromDisk(configPath);
            if (specCfg.Count == 0)
                return;
            // data was returned - begin import
            if (controller != null)
                controller.SetMessage("Importing " + sys.systemCode + ".cfg");
            ParseConfigIncoming(specCfg, sys);

            // set to enabled
            _ConfigBaseSettings.isEnabled = true;

            ConfigBaseSettings.SetConfig(_ConfigBaseSettings);
        }

        public void ParseConfigIncoming(List<string> cfg, GSystem sys)
        {
            // iterate through each line
            foreach (string s in cfg.Where(a => (a != "" || a != "\r" || a != "\n")))
            {
                // split to array based on whitespace
                string st = s.TrimEnd().Replace("\r", "").Replace("\n\r", "").Replace("\r\n", "").Replace("\n", "");
                string[] arr = st.Split(' ');
                // normal (non controller) settings should only have 2 items in the array
                if (arr.Length != 2)
                {
                    continue;
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
                PropertyInfo n = _ConfigNetplaySettings.GetType().GetProperty(propName);
                if (n != null)
                {
                    // property was found - update config
                    InitWindow.SetPropertyValue(_ConfigNetplaySettings, n, null, arr[1]);
                    continue;
                }

                // look for property in configserversettings
                PropertyInfo ser = _ConfigServerSettings.GetType().GetProperty(propName);
                if (ser != null)
                {
                    // property was found - update config
                    InitWindow.SetPropertyValue(_ConfigServerSettings, ser, null, arr[1]);
                    continue;
                }

            }

            
        }

        public List<string> LoadConfigFromDisk(string path)
        {
            // check whether file exists
            if (!File.Exists(path))
                return new List<string>();

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

            
        }

        public static T ChangeType<T>(object value)
        {
            Type conversionType = typeof(T);
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) { return default(T); }

                conversionType = Nullable.GetUnderlyingType(conversionType); ;
            }

            return (T)Convert.ChangeType(value, conversionType);
        }


    }
}
