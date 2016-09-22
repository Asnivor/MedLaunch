using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class GlobalSettings
    {
        public int settingsId { get; set; }                         // always 1
        public bool? databaseGenerated { get; set; }                // set to true to avoid regeneration at runtime
        public bool? fullScreen { get; set; }                       // force fullscreen when running the emulator
        public bool? fullGuiScreen { get; set; }                    // force fullscreen on startup in GUI launcher
        public bool? bypassConfig { get; set; }                     // ignore GUI launcher configs and just launch with the mednafen text config files
        public bool? enableNetplay { get; set; }                    // enable netplay
        public int? serverSelected { get; set; }                    // stores the currently selected server ID

    }
}
