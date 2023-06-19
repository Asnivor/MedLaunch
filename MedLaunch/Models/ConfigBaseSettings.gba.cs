using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Gameboy Advance
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string gba__bios { get; set; }

        public bool? gba__enable { get; set; }
        public bool? gba__forcemono { get; set; }
        public string gba__shader { get; set; }
        public int? gba__scanlines { get; set; }
        public string gba__special { get; set; }
        public string gba__stretch { get; set; }
        public bool? gba__tblur { get; set; }
        public bool? gba__tblur__accum { get; set; }
        public double? gba__tblur__accum__amount { get; set; }
        public string gba__videoip { get; set; }
        public int? gba__xres { get; set; }                            // 0 through 65536
        public double? gba__xscale { get; set; }                       // 0.01 through 256
        public double? gba__xscalefs { get; set; }                     // 0.01 through 256
        public int? gba__yres { get; set; }                            // 0 through 65536
        public double? gba__yscale { get; set; }                       // 0.01 through 256
        public double? gba__yscalefs { get; set; }                     // 0.01 through 256

        public bool? gba__shader__goat__fprog { get; set; }
        public double? gba__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string gba__shader__goat__pat { get; set; }
        public bool? gba__shader__goat__slen { get; set; }
        public double? gba__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? gba__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_gba(ConfigBaseSettings c)
        {
            c.gba__bios = "";

            c.gba__enable = true;
            c.gba__forcemono = false;                                // control placed
            c.gba__shader = "none";                               // control placed
            c.gba__scanlines = 0;                                    // control placed
            c.gba__special = "none";                                 // control placed
            c.gba__stretch = "aspect_mult2";                         // control placed
            c.gba__tblur = false;                                    // control placed
            c.gba__tblur__accum = false;                             // control placed
            c.gba__tblur__accum__amount = 50;                        // control placed
            c.gba__videoip = "0";                                    // control placed
            c.gba__xres = 0;                                         // control placed
            c.gba__xscale = 4;                                       // control placed
            c.gba__xscalefs = 1;                                     // control placed
            c.gba__yres = 0;                                         // control placed
            c.gba__yscale = 4;                                       // control placed
            c.gba__yscalefs = 1;                                     // control placed

            c.gba__shader__goat__fprog = false;
            c.gba__shader__goat__hdiv = 0.50;
            c.gba__shader__goat__pat = "goatron";
            c.gba__shader__goat__slen = true;
            c.gba__shader__goat__tp = 0.50;
            c.gba__shader__goat__vdiv = 0.50;
        }
    }
}
