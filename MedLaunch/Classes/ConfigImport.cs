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
    /// <summary>
    /// Handles the import of all mednafen config files
    /// </summary>
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

        public string MednafenConfigName { get; set; }

        // contructor
        public ConfigImport()
        {
            // populate config info from database
            _Paths = Paths.GetPaths();
            _ConfigNetplaySettings = ConfigNetplaySettings.GetNetplay();
            _ConfigServerSettings = new ConfigServerSettings(); // ConfigServerSettings.GetServer(100);
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

            if (VersionChecker.Instance.CurrentMedVerDesc.MajorINT > 0)
            {
                MednafenConfigName = @"mednafen.cfg";
            }
            else
            {
                MednafenConfigName = @"\mednafen-09x.cfg";
            }
        }

        public void ImportAll(ProgressDialogController controller)
        {
            if (controller != null)
                controller.SetMessage("Importing Mednafen Configs from disk\nPlease wait.....");

            // import mednafen-09x.cfg into relevant config files
            ImportBaseConfigFromDisk(null);

            // get any system specific .cfg files
            List<GSystem> systems = GSystem.GetSystems();
            foreach (GSystem sys in systems)
            {
                ImportSystemConfigFromDisk(null, sys);
            }

            // now save to database
            SaveToDatabase();
        }

        public void SaveToDatabase()
        {
            ConfigBaseSettings.SetConfig(_ConfigGbaSettings);
            ConfigBaseSettings.SetConfig(_ConfigGbSettings);
            ConfigBaseSettings.SetConfig(_ConfigGgSettings);
            ConfigBaseSettings.SetConfig(_ConfigLynxSettings);
            ConfigBaseSettings.SetConfig(_ConfigMdSettings);
            ConfigBaseSettings.SetConfig(_ConfigNesSettings);
            ConfigBaseSettings.SetConfig(_ConfigNgpSettings);
            ConfigBaseSettings.SetConfig(_ConfigPceSettings);
            ConfigBaseSettings.SetConfig(_ConfigPce_fastSettings);
            ConfigBaseSettings.SetConfig(_ConfigPcfxSettings);
            ConfigBaseSettings.SetConfig(_ConfigPsxSettings);
            ConfigBaseSettings.SetConfig(_ConfigSmsSettings);
            ConfigBaseSettings.SetConfig(_ConfigSnesSettings);
            ConfigBaseSettings.SetConfig(_ConfigSnes_faustSettings);
            ConfigBaseSettings.SetConfig(_ConfigSsSettings);
            ConfigBaseSettings.SetConfig(_ConfigVbSettings);
            ConfigBaseSettings.SetConfig(_ConfigWswanSettings);

            ConfigNetplaySettings.SetNetplay(_ConfigNetplaySettings);

            ConfigServerSettings.SaveToDatabase(_ConfigServerSettings);
        }

        public void ImportBaseConfigFromDisk(ProgressDialogController controller)
        {
            string cfgPath = _Paths.mednafenExe + MednafenConfigName;// @"\mednafen-09x.cfg";
            var config = LoadConfigFromDisk(cfgPath);
            if (config.Count > 0)
            {
                // data was returned - begin import
                if (controller != null)
                    controller.SetMessage("Importing " + MednafenConfigName); // mednafen -09x.cfg");
                ParseConfigIncoming(config, 0);                
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
            ParseConfigIncoming(specCfg, 2000000000 + sys.systemId);
        }

        public void ParseConfigIncoming(List<string> cfg, int confId)
        {
            // iterate through each line
            foreach (string s in cfg.Where(a => (a != "" || a != "\r" || a != "\n")))
            {
                // split to array based on whitespace
                string st = s.Trim().Replace("\r", "").Replace("\n\r", "").Replace("\r\n", "").Replace("\n", "");
                string[] arr = st.Split(' ');
                // normal (non controller) settings should only have 2 items in the array
                if (arr.Length != 2)
                {
                    continue;
                }
                string propName = arr[0].Replace(".", "__");
                string propValue = arr[1];

                // ignore .keys
                if (propName.StartsWith("."))
                    continue;                
                
                // look for property in the configbasesettings
                PropertyInfo p = _ConfigBaseSettings.GetType().GetProperty(propName);
                if (p != null)
                {
                    if (confId == 0)
                    {
                        // this is for the base config                    

                        // filter out the system specific entries and update the correct config object
                        if (s.StartsWith("gb."))
                        {
                            InitWindow.SetPropertyValue(_ConfigGbSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("gba."))
                        {
                            InitWindow.SetPropertyValue(_ConfigGbaSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("lynx."))
                        {
                            InitWindow.SetPropertyValue(_ConfigLynxSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("md."))
                        {
                            InitWindow.SetPropertyValue(_ConfigMdSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("gg."))
                        {
                            InitWindow.SetPropertyValue(_ConfigGgSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("ngp."))
                        {
                            InitWindow.SetPropertyValue(_ConfigNgpSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("pce."))
                        {
                            InitWindow.SetPropertyValue(_ConfigPceSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("pcfx."))
                        {
                            InitWindow.SetPropertyValue(_ConfigPcfxSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("psx."))
                        {
                            InitWindow.SetPropertyValue(_ConfigPsxSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("sms."))
                        {
                            InitWindow.SetPropertyValue(_ConfigSmsSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("nes."))
                        {
                            InitWindow.SetPropertyValue(_ConfigNesSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("snes."))
                        {
                            InitWindow.SetPropertyValue(_ConfigSnesSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("ss."))
                        {
                            InitWindow.SetPropertyValue(_ConfigSsSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("vb."))
                        {
                            InitWindow.SetPropertyValue(_ConfigVbSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("wswan."))
                        {
                            InitWindow.SetPropertyValue(_ConfigWswanSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("snes_faust."))
                        {
                            InitWindow.SetPropertyValue(_ConfigSnes_faustSettings, p, null, arr[1]);
                            continue;
                        }
                        if (s.StartsWith("pce_fast."))
                        {
                            InitWindow.SetPropertyValue(_ConfigPce_fastSettings, p, null, arr[1]);
                            continue;
                        }

                        // now we should just be left with generic config commands. At this time we will apply them to all configs
                        InitWindow.SetPropertyValue(_ConfigGbSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigGbaSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigLynxSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigMdSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigGgSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigNgpSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigPceSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigPcfxSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigPsxSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigSmsSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigNesSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigSnesSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigSsSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigVbSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigWswanSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigSnes_faustSettings, p, null, arr[1]);
                        InitWindow.SetPropertyValue(_ConfigPce_fastSettings, p, null, arr[1]);
                        continue;
                    }

                    else
                    {
                        // this is a system.cfg file
                        switch (confId)
                        {
                            case 2000000001:
                                InitWindow.SetPropertyValue(_ConfigGbSettings, p, null, arr[1]);
                                break;
                            case 2000000002:
                                InitWindow.SetPropertyValue(_ConfigGbaSettings, p, null, arr[1]);
                                break;
                            case 2000000003:
                                InitWindow.SetPropertyValue(_ConfigLynxSettings, p, null, arr[1]);
                                break;
                            case 2000000004:
                                InitWindow.SetPropertyValue(_ConfigMdSettings, p, null, arr[1]);
                                break;
                            case 2000000005:
                                InitWindow.SetPropertyValue(_ConfigGgSettings, p, null, arr[1]);
                                break;
                            case 2000000006:
                                InitWindow.SetPropertyValue(_ConfigNgpSettings, p, null, arr[1]);
                                break;
                            case 2000000007:
                                InitWindow.SetPropertyValue(_ConfigPceSettings, p, null, arr[1]);
                                break;
                            case 2000000008:
                                InitWindow.SetPropertyValue(_ConfigPcfxSettings, p, null, arr[1]);
                                break;
                            case 2000000009:
                                InitWindow.SetPropertyValue(_ConfigPsxSettings, p, null, arr[1]);
                                break;
                            case 2000000010:
                                InitWindow.SetPropertyValue(_ConfigSmsSettings, p, null, arr[1]);
                                break;
                            case 2000000011:
                                InitWindow.SetPropertyValue(_ConfigNesSettings, p, null, arr[1]);
                                break;
                            case 2000000012:
                                InitWindow.SetPropertyValue(_ConfigSnesSettings, p, null, arr[1]);
                                break;
                            case 2000000013:
                                InitWindow.SetPropertyValue(_ConfigSsSettings, p, null, arr[1]);
                                break;
                            case 2000000014:
                                InitWindow.SetPropertyValue(_ConfigVbSettings, p, null, arr[1]);
                                break;
                            case 2000000015:
                                InitWindow.SetPropertyValue(_ConfigWswanSettings, p, null, arr[1]);
                                break;
                            case 2000000016:
                                InitWindow.SetPropertyValue(_ConfigSnes_faustSettings, p, null, arr[1]);
                                break;
                            case 2000000017:
                                InitWindow.SetPropertyValue(_ConfigPce_fastSettings, p, null, arr[1]);
                                break;
                        }
                    }
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
                path = _Paths.mednafenExe + "\\" + MednafenConfigName; // "mednafen-09x.cfg";
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
