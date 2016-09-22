using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class Paths
    {
        public int pathId { get; set; }
        public string mednafenExe { get; set; }                     // Path to the Mednafen EXE

        //public string systemCdplay { get; set; }                    // CD-DA Player
        public string systemGb { get; set; }                        // Gameboy Color
        public string systemGba { get; set; }                       // Gamboy Advance
        public string systemLynx { get; set; }                      // Atari Lynx
        public string systemMd { get; set; }                        // Sega Genesis/MegaDrive
        public string systemGg { get; set; }                        // Sega Game Gear
        public string systemNgp { get; set; }                       // NeoGeo Pocket Color
        public string systemPce { get; set; }                       // PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx
        public string systemPcfx { get; set; }                      // PC-FX
        public string systemPsx { get; set; }                       // Sony PlayStation 
        public string systemSms { get; set; }                       // Sega Master System
        public string systemNes { get; set; }                       // Nintendo Entertainment System/Famicon
        public string systemSnes { get; set; }                      // Super Nintendo Entertainment System/Super Famicom
        public string systemSs { get; set; }                        // Sega Saturn
        //public string systemSsfplay { get; set; }                   // Sega Saturn Sound Format Player
        public string systemVb { get; set; }                        // Virtual Boy
        public string systemWswan { get; set; }                     // WonderSwan
    }
}
