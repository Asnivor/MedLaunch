using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Data.Entity;
using System.Reflection;
using MahApps.Metro.Controls;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace MedLaunch.Models
{
    public partial class ConfigBaseSettings
    {
        public int ConfigId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool? isEnabled { get; set; }
        public int? systemIdent { get; set; }

        public string affinity__cd { get; set; }                        // CD read threads CPU affinity mask.
        public string affinity__emu { get; set; }                       // Main emulation thread CPU affinity mask.
        public string affinity__video { get; set; }                     // Video blitting thread CPU affinity mask.
        public bool? autosave { get; set; }                             // autosave
        public bool? cd__image_memcache { get; set; }                   // cd.image_memcache
        public int? cd__m3u__disc_limit { get; set; }                   // M3U total number of disc images limit.
        public int? cd__m3u__recursion_limit { get; set; }              // M3U recursion limit.
        public bool? cheats { get; set; }                               // cheats
        public bool? debugger__autostepmode { get; set; }               // debugger.autostepmode
        public bool? ffnosound { get; set; }                            // ffnosound
        public double? ffspeed { get; set; }                            // ffspeed (1 through 15)
        public bool? fftoggle { get; set; }                             // fftoggle
        public string filesys__path_cheat { get; set; }                 // filesys.path_cheat
        public string filesys__path_firmware { get; set; }              // filesys.path_firmware
        public string filesys__path_movie { get; set; }                 // filesys.path_movie
        public string filesys__path_palette { get; set; }               // filesys.path_palette
        public string filesys__path_pgconfig { get; set; }              // filesys.path_pgconfig
        public string filesys__path_sav { get; set; }                   // filesys.path_sav
        public string filesys__path_savbackup { get; set; }             // filesys.path_savbackup
        public string filesys__path_snap { get; set; }                  // filesys.path_snap
        public string filesys__path_state { get; set; }                 // filesys.path_state
        public int? filesys__state_comp_level { get; set; }             // filesys.state_comp_level (-1 through 9)
        public bool? filesys__untrusted_fip_check { get; set; }         // filesys.untrusted_fip_check
        public bool? fps__autoenable { get; set; }
        public string fps__bgcolor { get; set; }                        // 0x00000000 through 0xFFFFFFFF           
        public string fps__font { get; set; }                           //  5x7 6x9 6x12 6x13 9x18
        public string fps__position { get; set; }                       // upper_left  upper_right
        public int? fps__scale { get; set; }                            // 0 through 32
        public string fps__textcolor { get; set; }                      // 0x00000000 through 0xFFFFFFFF
        public int? input__autofirefreq { get; set; }                   // input.autofirefreq               (0 through 1000)
        public int? input__ckdelay { get; set; }                        // 0 through 99999
        public double? input__joystick__axis_threshold { get; set; }    // 0 through 100
        public bool? input__joystick__global_focus { get; set; }
        public bool? nothrottle { get; set; }
        public bool? osd__alpha_blend { get; set; }
        public int? osd__message_display_time { get; set; }             // 0 through 15000
        public int? osd__state_display_time { get; set; }               // 0 through 15000

        public int? qtrecord__h_double_threshold { get; set; }          // 0 through 1073741824
        public string qtrecord__vcodec { get; set; }                    // raw cscd png
        public int? qtrecord__w_double_threshold { get; set; }          // 0 through 1073741824
        public double? sfspeed { get; set; }                            // 0.25 through 15
        public bool? sftoggle { get; set; }
        public bool? sound { get; set; }
        public int? sound__buffer_time { get; set; }                    // 0 through 1000
        public string sound__device { get; set; }                       // default
        public string sound__driver { get; set; }                       // default alsa oss wasapish dsound wasapi sdl jack
        public int? sound__period_time { get; set; }                    // 0 through 100000
        public int? sound__rate { get; set; }                           // 22050 through 192000
        public int? sound__volume { get; set; }                         // 0 through 150
        public int? srwframes { get; set; }                             // 10 through 99999
        public bool? video__blit_timesync { get; set; }
        public string video__cursorvis { get; set; }
        public string video__deinterlacer { get; set; }                 // weave bob bob_offset
        public bool? video__disable_composition { get; set; }
        public string video__driver { get; set; }                       // default opengl, softfb (old: opengl sdl overlay)
        public bool? video__force_bbclear { get; set; }
        public bool? video__frameskip { get; set; }
        public bool? video__fs { get; set; }

        public int? video__fs__display { get; set; }                    // -1 through 32767
        public bool? video__glvsync { get; set; }
        public string video__glformat { get; set; }

        // generic system specific settings -   <system>.setting
        // (are these even used anymore???)
        public bool? __enable { get; set; }
        public bool? __forcemono { get; set; }                      // force mono output
        public string __shader { get; set; }
        public int? __scanlines { get; set; }                       // -100 through 100
        public string __special { get; set; }
        public string __stretch { get; set; }
        public bool? __tblur { get; set; }
        public bool? __tblur__accum { get; set; }
        public double? __tblur__accum__amount { get; set; }         // 0 through 100
        public string __videoip { get; set; }
        public int? __xres { get; set; }                            // 0 through 65536
        public double? __xscale { get; set; }                       // 0.01 through 256
        public double? __xscalefs { get; set; }                     // 0.01 through 256
        public int? __yres { get; set; }                            // 0 through 65536
        public double? __yscale { get; set; }                       // 0.01 through 256
        public double? __yscalefs { get; set; }                     // 0.01 through 256

        /// <summary>
        /// Gets all config defaults for the latest supported mednafen version
        /// </summary>
        /// <returns></returns>
        public static ConfigBaseSettings GetConfigDefaults()
        {
            ConfigBaseSettings cfb = new ConfigBaseSettings
            {
                UpdatedTime = DateTime.Now,
                systemIdent = 0,
                isEnabled = true,
                affinity__cd = "0x0000000000000000",                // control placed
                affinity__emu = "0x0000000000000000",               // control placed
                affinity__video = "0x0000000000000000",             // control placed
                autosave = false,                                   // control placed
                cd__image_memcache = false,                         // control placed
                cd__m3u__disc_limit = 25,
                cd__m3u__recursion_limit = 9,
                cheats = true,                                      // control placed
                debugger__autostepmode = false,
                ffnosound = false,                                  // control placed
                ffspeed = 4,                                        // control placed
                fftoggle = false,                                   // control placed

                filesys__path_cheat = "cheats",
                filesys__path_firmware = "firmware",
                filesys__path_movie = "mcm",
                filesys__path_palette = "palettes",
                filesys__path_pgconfig = "pgconfig",
                filesys__path_sav = "sav",
                filesys__path_savbackup = "b",
                filesys__path_snap = "snaps",
                filesys__path_state = "mcs",

                filesys__state_comp_level = 6,
                filesys__untrusted_fip_check = true,
                input__autofirefreq = 3,                            // control placed
                input__ckdelay = 0,                                 // control placed
                input__joystick__axis_threshold = 75.00,               // control placed
                input__joystick__global_focus = true,               // control placed
                nothrottle = false,                                 // control placed
                osd__alpha_blend = true,                            // control placed
                osd__message_display_time = 2500,                   // control placed
                osd__state_display_time = 2000,                     // control placed

                // new FPS options
                fps__autoenable = false,
                fps__bgcolor = "0x80000000",
                fps__font = "5x7",
                fps__position = "upper_left",
                fps__scale = 1,
                fps__textcolor = "0xFFFFFFFF",

                qtrecord__h_double_threshold = 256,
                qtrecord__w_double_threshold = 384,
                qtrecord__vcodec = "cscd",
                sfspeed = 0.75,                                     // control placed
                sftoggle = false,                                   // control placed
                sound = true,                                       // control placed
                sound__buffer_time = 0,                             // control placed
                sound__device = "default",
                sound__driver = "default",                          // control placed
                sound__period_time = 0,                             // control placed
                sound__rate = 48000,                                // control placed
                sound__volume = 100,                                // control placed
                srwframes = 600,                                    // control placed
                video__blit_timesync = true,                        // control placed
                video__cursorvis = "hidden",
                video__deinterlacer = "weave",                      // control placed
                video__disable_composition = true,                  // control placed
                video__driver = "default",                           // control placed
                video__force_bbclear = false,                           // control placed
                video__frameskip = true,                            // control placed
                video__fs = false,                                  // control placed
                video__glvsync = true,                              // control placed
                video__glformat = "auto",

                video__fs__display = -1,                          // -1 through 32767

                // generic system specific settings
                __enable = true,
                __forcemono = false,                                // control placed
                __shader = "none",                               // control placed
                __scanlines = 0,                                    // control placed
                __special = "none",                                 // control placed
                __stretch = "aspect_mult2",                         // control placed
                __tblur = false,                                    // control placed
                __tblur__accum = false,                             // control placed
                __tblur__accum__amount = 50,                        // control placed
                __videoip = "0",                                    // control placed
                __xres = 0,                                         // control placed
                __xscale = 1,                                       // control placed
                __xscalefs = 1,                                     // control placed
                __yres = 0,                                         // control placed
                __yscale = 1,                                       // control placed
                __yscalefs = 1,                                      // control placed
            };

            GetDefaults_gb(cfb);
            GetDefaults_gba(cfb);
            GetDefaults_gg(cfb);
            GetDefaults_lynx(cfb);
            GetDefaults_md(cfb);
            GetDefaults_nes(cfb);
            GetDefaults_ngp(cfb);
            GetDefaults_pce(cfb);
            GetDefaults_pce_fast(cfb);
            GetDefaults_pcfx(cfb);
            GetDefaults_psx(cfb);
            GetDefaults_sms(cfb);
            GetDefaults_snes(cfb);
            GetDefaults_snes_faust(cfb);
            GetDefaults_ss(cfb);
            GetDefaults_vb(cfb);
            GetDefaults_wswan(cfb);
            GetDefaults_apple2(cfb);

            return cfb;
        }

        public static void SaveToDatabase(List<ConfigBaseSettings> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.ConfigBaseSettings.AsNoTracking().ToList();

                List<ConfigBaseSettings> toAdd = new List<ConfigBaseSettings>();
                List<ConfigBaseSettings> toUpdate = new List<ConfigBaseSettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    ConfigBaseSettings t = (from a in current
                                     where a.ConfigId == g.ConfigId
                                     select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.ConfigBaseSettings.UpdateRange(toUpdate);
                db.ConfigBaseSettings.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        public static void ResetToDefault(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);                
            }

            int configId = baseId + sysId;

            // get defaults
            ConfigBaseSettings defaults = ConfigBaseSettings.GetConfigDefaults();
            defaults.ConfigId = configId;
            defaults.systemIdent = sysId;
            defaults.UpdatedTime = DateTime.Now;

            // Get current config
            ConfigBaseSettings config = ConfigBaseSettings.GetConfig(configId);
            var conf = new ConfigBaseSettings();
            conf = config;


            if (config.isEnabled == true)
            {
                defaults.isEnabled = true;
            }
            else
            {
                defaults.isEnabled = false;
            }


            // set defaults for this config
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(defaults).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
            }
        }

        public static int GetConfigIdFromButtonName(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);
            }
            //MessageBoxResult result = MessageBox.Show(code);

            int configId = baseId + sysId;
            return configId;
        }

        public static string GetConfigFileNameFromConfigId(int configId)
        {
            return "";
        }

        public static void EnableConfigToggle(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);
            }

            int configId = baseId + sysId;

            // Get current config
            ConfigBaseSettings config = ConfigBaseSettings.GetConfig(configId);
            var conf = new ConfigBaseSettings();
            conf = config;


            if (config.isEnabled == true)
            {
                conf.isEnabled = false;
            }
            else
            {
                conf.isEnabled = true;
            }
            // set defaults for this config
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(config).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
                cfDef.Dispose();
            }
            //SetConfig(config);


        }

        public static ConfigBaseSettings GetConfig(int ConfigId)
        {
            using (var context = new MyDbContext())
            {
                var cData = context.ConfigBaseSettings.AsNoTracking().ToList();
                ConfigBaseSettings c = (from a in cData
                                        where a.ConfigId == ConfigId
                                        select a).ToList().SingleOrDefault();
                if (c == null)
                    return null;
                context.Dispose();
                return c;
            }
        }

        public static void SetConfig(ConfigBaseSettings Config)
        {
            using (var cfDef = new MyDbContext())
            {

                cfDef.Entry(Config).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
            }
        }

        public static bool IsConfigEnabled(int systemId)
        {
            int baseId = 2000000000;
            int id = baseId + systemId;

            using (var context = new MyDbContext())
            {
                var c = (from a in context.ConfigBaseSettings
                        where a.ConfigId == id
                        select a).SingleOrDefault();

                if (c.isEnabled == true && c.isEnabled != null)
                    return true;
                else
                    return false;                
            }
        }
        
        public static void SetButtonState(RadioButton rb)
        {
            string rbName = rb.Name.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            if (code == "base")
            {
                rb.IsEnabled = true;
            }
            else
            {
                int systemId = GetSystemIdFromSystemCode(code);

                if (IsConfigEnabled(systemId) == true)
                {
                    rb.IsEnabled = true;
                }
                else
                {
                    rb.IsEnabled = true;
                }
            }
            
        }

        public static int GetSystemIdFromSystemCode(string systemCode)
        {
            GSystem gs = new GSystem(systemCode);
            return gs.systemId;
        }

        public static string ConvertControlNameToConfigName(string controlName)
        {
            return controlName.Replace("cfg_", "");
        }

        // populate settings - bios path controls
        public static void LoadBiosPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            ConfigBaseSettings settings = GetConfig(2000000000);

            SetControlValues(ui, settings, 1);
        }

        // save settings - mednafen bios controls
        public static void SaveBiosPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            //ConfigBaseSettings settings = GetConfig(2000000000);

            // get ALL config settings (as we are saving these to all configs)
            List<ConfigBaseSettings> AllSettings = new List<ConfigBaseSettings>();
            using (var context = new MyDbContext())
            {
                List<ConfigBaseSettings> aset = (from d in context.ConfigBaseSettings
                                                 select d).ToList();
                AllSettings.AddRange(aset);
            }

            // iterate through each config and set all the settings for each config
            foreach (ConfigBaseSettings settings in AllSettings)
            {
                SetControlValues(ui, settings, 2);
            }
        }

        public static void SaveBiosPaths()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get all the path values
            string gba = ((TextBox)mw.FindName("cfg_gba__bios")).Text;
            string pcege = ((TextBox)mw.FindName("cfg_pce__gecdbios")).Text;
            string pcecd = ((TextBox)mw.FindName("cfg_pce__cdbios")).Text;
            string pcefastcd = ((TextBox)mw.FindName("cfg_pce_fast__cdbios")).Text;
            string pcfx = ((TextBox)mw.FindName("cfg_pcfx__bios")).Text;
            string mdcd = ((TextBox)mw.FindName("cfg_md__cdbios")).Text;
            string nesgg = ((TextBox)mw.FindName("cfg_nes__ggrom")).Text;
            string ssjp = ((TextBox)mw.FindName("cfg_ss__bios_jp")).Text;
            string ssnaeu = ((TextBox)mw.FindName("cfg_ss__bios_na_eu")).Text;
            string psxeu = ((TextBox)mw.FindName("cfg_psx__bios_eu")).Text;
            string psxjp = ((TextBox)mw.FindName("cfg_psx__bios_jp")).Text;
            string psxna = ((TextBox)mw.FindName("cfg_psx__bios_na")).Text;

            using (var db = new MyDbContext())
            {
                // get all configs
                var configs = from a in db.ConfigBaseSettings
                              select a;
                foreach (var c in configs)
                {
                    // update each config file
                    c.gba__bios = gba;
                    c.pce__gecdbios = pcege;
                    c.pce__cdbios = pcecd;
                    c.pce_fast__cdbios = pcefastcd;
                    c.pcfx__bios = pcfx;
                    c.md__cdbios = mdcd;
                    c.nes__ggrom = nesgg;
                    c.ss__bios_jp = ssjp;
                    c.ss__bios_na_eu = ssnaeu;
                    c.psx__bios_eu = psxeu;
                    c.psx__bios_jp = psxjp;
                    c.psx__bios_na = psxna;
                    //db.SaveChanges();
                }

                // save changes
                db.SaveChanges();
            }
        }

        // populate settings - mednafen paths controls
        public static void LoadMednafenPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            ConfigBaseSettings settings = GetConfig(2000000000);

            SetControlValues(ui, settings, 1);
        }

        // save settings - mednafen paths controls
        public static void SaveMednafenPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            //ConfigBaseSettings settings = GetConfig(2000000000);

            // get ALL config settings (as we are saving these to all configs)
            List<ConfigBaseSettings> AllSettings = new List<ConfigBaseSettings>();
            using (var context = new MyDbContext())
            {
                List<ConfigBaseSettings> aset = (from d in context.ConfigBaseSettings
                                                        select d).ToList();
                AllSettings.AddRange(aset);
            }

            // iterate through each config and set all the settings for each config
            foreach (ConfigBaseSettings settings in AllSettings)
            {
                SetControlValues(ui, settings, 2);
            }       
        }

        public static void SaveMednafenPaths()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get all the path values
            string cheats = ((TextBox)mw.FindName("cfg_filesys__path_cheat")).Text;
            string firmware = ((TextBox)mw.FindName("cfg_filesys__path_firmware")).Text;
            string movies = ((TextBox)mw.FindName("cfg_filesys__path_movie")).Text;
            string palettes = ((TextBox)mw.FindName("cfg_filesys__path_palette")).Text;
            string pgc = ((TextBox)mw.FindName("cfg_filesys__path_pgconfig")).Text;
            string saves = ((TextBox)mw.FindName("cfg_filesys__path_sav")).Text;
            string savebackup = ((TextBox)mw.FindName("cfg_filesys__path_savbackup")).Text;
            string snapshots = ((TextBox)mw.FindName("cfg_filesys__path_snap")).Text;
            string savestates = ((TextBox)mw.FindName("cfg_filesys__path_state")).Text;

            using (var db = new MyDbContext())
            {
                // get all configs
                var configs = from a in db.ConfigBaseSettings
                              select a;
                foreach (var c in configs)
                {
                    // update each config file
                    c.filesys__path_cheat = cheats;
                    c.filesys__path_firmware = firmware;
                    c.filesys__path_movie = movies;
                    c.filesys__path_palette = palettes;
                    c.filesys__path_pgconfig = pgc;
                    c.filesys__path_sav = saves;
                    c.filesys__path_savbackup = savebackup;
                    c.filesys__path_snap = snapshots;
                    c.filesys__path_state = savestates;
                }

                // save changes
                db.SaveChanges();
            }
        }

        public static void SetControlValues(UIHandler ui, ConfigBaseSettings settings, int LoadOrSave)
        {
            if (LoadOrSave == 1)
            {
                // 1 = load settings
                // Iterate through all controls and set correct values
                // Buttons
                foreach (Button control in ui.Buttons)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // check that the control is actually a config control
                        if (propName.Contains("_"))
                        {
                            // get the property value using reflection
                            var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                            string v = value.ToString();

                            // update wpf control
                            control.Content = v;
                        }
                        else
                        {
                            
                        }

                        
                    }
                }

                // RadioButtons

                // Labels

                // CheckBoxes
                foreach (CheckBox control in ui.CheckBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_") || control.Name.Contains("comboPsx"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        bool v = Convert.ToBoolean(value.ToString());

                        // update wpf control
                        control.IsChecked = v;
                    }
                }

                // TextBoxes
                foreach (TextBox control in ui.TextBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // textbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        string v = value.ToString();

                        // update wpf control
                        control.Text = v;
                    }
                }

                // Sliders
                foreach (Slider control in ui.Sliders)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_") || control.Name.Contains("ALPHA"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        double v = Convert.ToDouble(value.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                        // update wpf control
                        control.Value = v;
                    }
                }

                // Comboxes
                foreach (ComboBox control in ui.ComboBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        string v = value.ToString();

                        // update wpf control
                        control.SelectedValue = v;
                    }
                }

                // NumericUpDowns
                foreach (NumericUpDown control in ui.NumericUpDowns)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        double v = Convert.ToDouble(value.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                        // update wpf control
                        control.Value = v;
                    }
                }

                // ColorPickers
                foreach (ColorPicker control in ui.Colorpickers)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        string v = value.ToString();

                        // update wpf control

                        string n = string.Empty;

                        // strip leading 0x
                        if (v.StartsWith("0x"))
                            n = v.Replace("0x", "");
                        else
                            n = v;

                        /*
                        // handle AARRGGBB values (eg FPS colors)
                        if (n.Length == 8)
                        {
                            // this is AARRGGBB - we need to take the alpha channel value from the first byte
                            // and update the matching alpha channel slider

                            // first find the matching alpha channel control (currently only valid for FPS colors)
                            string ctrlName = control.Name;
                            string lookupName = ctrlName.Replace("cfg_fps__", "cfg_ALPHAfps__");
                            Slider alphaSlider = ui.Sliders.Where(a => a.Name == lookupName).FirstOrDefault();

                            // split the hex value into AA and RRGGBB
                            string AA = n.Substring(0, 2);
                            string RRBBGG = n.Substring(2, 6);

                            // update the colorpicker
                            control.SelectedColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#" + RRBBGG);

                            // update the alpha slider
                            int aVal = int.Parse(AA, System.Globalization.NumberStyles.HexNumber);
                            double aVa = (double)aVal;
                            //alphaSlider.Value = aVa;
                        }
                        else
                        {
                            // this is RRGGBB - update the colorpicker directly with the value
                            control.SelectedColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#" + n);
                        }
                        */

                        control.SelectedColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#" + n);
                    }
                }

            }
            if (LoadOrSave == 2)
            {
                // 2 = save settings
                // Iterate through all controls and set correct values
                // Buttons
                foreach (Button control in ui.Buttons)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // check that the control is actually a config control
                        if (propName.Contains("_"))
                        {
                            // get the control value
                            string v = control.Content.ToString();
                            // update settings object with value
                            PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                            propInfo.SetValue(settings, v, null);
                        }
                    }
                }

                // RadioButtons

                // Labels

                // CheckBoxes
                foreach (CheckBox control in ui.CheckBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_") || control.Name.Contains("comboPsx"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the control value
                        bool? v = control.IsChecked.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        propInfo.SetValue(settings, v, null);
                    }
                }

                // TextBoxes
                foreach (TextBox control in ui.TextBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // textbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the control value
                        string v = control.Text;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        propInfo.SetValue(settings, v, null);
                    }
                }

                // Sliders
                foreach (Slider control in ui.Sliders)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_") || control.Name.Contains("ALPHA"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        double? v = control.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                        {
                            // double is required
                            propInfo.SetValue(settings, v, null);
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                        }
                        if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            // int32 is required
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            propInfo.SetValue(settings, Convert.ToInt32(v), null);
                        }
                    }
                }

                // Comboxes
                foreach (ComboBox control in ui.ComboBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_") || ((ComboBoxItem)control.SelectedItem) == null)
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        //string v = control.SelectedValue.ToString();
                        string v = ((ComboBoxItem)control.SelectedItem).Tag.ToString();
                        //MessageBoxResult aresult = MessageBox.Show(propName + "   -   " + v);

                        // update settings object with string value - 


                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        //MessageBoxResult aresult = MessageBox.Show(propName + "   -   " + propInfo.ToString() + "   -   " + v);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]") || propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                            {
                                // double is required
                                double? d = Convert.ToDouble(v, System.Globalization.CultureInfo.InvariantCulture);
                                propInfo.SetValue(settings, d, null);
                                //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            }
                            if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                            {
                                // int32 is required
                                int? i = Convert.ToInt32(v);
                                //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                                propInfo.SetValue(settings, i, null);
                            }
                        }
                        else
                        {
                            propInfo.SetValue(settings, v, null);
                        }



                    }
                }

                // NumericUpDowns
                foreach (NumericUpDown control in ui.NumericUpDowns)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        double? v = control.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                        {
                            // double is required
                            propInfo.SetValue(settings, v, null);
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                        }
                        if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            // int32 is required
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            propInfo.SetValue(settings, Convert.ToInt32(v), null);
                        }
                    }
                }

                // Colorpickers
                foreach (ColorPicker control in ui.Colorpickers)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "" || control.Name.Contains("Generic__") || control.Name.Contains("tb_"))
                    {
                        // textbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the control values
                        System.Windows.Media.Color color = control.SelectedColor.Value;
                        string hex = new ColorConverter().ConvertToString(color);

                        string v = string.Empty;

                        // we (at the moment) only want the alpha channel for the fps settings                        
                        if (control.Name.Contains("fps"))
                        {
                            // AARRGGBB
                            v = "0x" + hex.Replace("#", "").ToUpper().Substring(0, 8);
                        }
                        else
                        {
                            // RRGGBB
                            v = "0x" + hex.Replace("#", "").ToUpper().Substring(2, 6);
                        }
                        /*
                        if (hex.Length == 9)
                        {
                            v = "0x" + hex.Replace("#", "").ToUpper().Substring(0, 8);
                        }
                        else
                        {
                            v = "0x" + hex.Replace("#", "").ToUpper().Substring(2, 6);
                        }
                        */
                        
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        propInfo.SetValue(settings, v, null);                        
                    }
                }

                // save config
                SetConfig(settings);
            }
            
        }

        

        // Populate config settings for specific System ID  -   WrapPanel as parent     
        public static void LoadControlValues(WrapPanel wp, int configId)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for system
            ConfigBaseSettings settings = GetConfig(configId);

            SetControlValues(ui, settings, 1);           

        }

        // Save config settings for specific System ID  -   WrapPanel as parent     
        public static void SaveControlValues(WrapPanel wp, int configId)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for system
            ConfigBaseSettings settings = GetConfig(configId);

            SetControlValues(ui, settings, 2);

            

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
