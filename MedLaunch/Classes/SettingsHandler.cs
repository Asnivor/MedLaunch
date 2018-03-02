using MedLaunch.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public class SettingsHandler
    {
        // constructor
        public SettingsHandler()
        {
            VH = new SettingsVisualHandler();
            AllSettingsPanels = VH.AllSettingPanels;

            /* Load all settings from database */
            // Paths Table
            Paths = Paths.GetPaths();
            // Global Settings Table
            GSettings = GlobalSettings.GetGlobals();
            // Config Table
            ConfigSettings = ConfigBaseSettings.GetConfig(2000000000);
            // Netplay Table
            NPSettings = ConfigNetplaySettings.GetNetplay();
            // Servers Table            
            ServerSettings = ConfigServerSettings.GetServers();
            
        }

        // properties
        public SettingsVisualHandler VH { get; set; }
        public List<Border> AllSettingsPanels { get; set; }
        public UIHandler MedLaunchSettings { get; set; }
        public UIHandler MednafenPathSettings { get; set; }
        public UIHandler GameFolderSettings { get; set; }
        public UIHandler BiosSettings { get; set; }
        public UIHandler NetplaySettings { get; set; }
        public UIHandler EmulatorSettings { get; set; }

        public Paths Paths { get; set; }
        public ConfigBaseSettings ConfigSettings { get; set; }
        public ConfigNetplaySettings NPSettings { get; set; }
        public List<ConfigServerSettings> ServerSettings { get; set; }
        public ConfigSystemSettings SystemSettings { get; set; }
        public GlobalSettings GSettings { get; set; }

        // methods
        public void SaveAllSettings()
        {
            SettingsVisualHandler sv = new SettingsVisualHandler();
        }

        public void LoadAllSettings()
        {
            SettingsVisualHandler sv = new SettingsVisualHandler();
        }
    }
}
