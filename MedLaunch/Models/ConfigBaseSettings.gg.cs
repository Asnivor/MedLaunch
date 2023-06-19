using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Sega Game Gear
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? gg__enable { get; set; }
        public bool? gg__forcemono { get; set; }
        public string gg__shader { get; set; }
        public int? gg__scanlines { get; set; }
        public string gg__special { get; set; }
        public string gg__stretch { get; set; }
        public bool? gg__tblur { get; set; }
        public bool? gg__tblur__accum { get; set; }
        public double? gg__tblur__accum__amount { get; set; }
        public string gg__videoip { get; set; }
        public int? gg__xres { get; set; }                            // 0 through 65536
        public double? gg__xscale { get; set; }                       // 0.01 through 256
        public double? gg__xscalefs { get; set; }                     // 0.01 through 256
        public int? gg__yres { get; set; }                            // 0 through 65536
        public double? gg__yscale { get; set; }                       // 0.01 through 256
        public double? gg__yscalefs { get; set; }                     // 0.01 through 256

        public bool? gg__shader__goat__fprog { get; set; }
        public double? gg__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string gg__shader__goat__pat { get; set; }
        public bool? gg__shader__goat__slen { get; set; }
        public double? gg__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? gg__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_gg(ConfigBaseSettings c)
        {
            c.gg__enable = true;
            c.gg__forcemono = false;                                // control placed
            c.gg__shader = "none";                               // control placed
            c.gg__scanlines = 0;                                    // control placed
            c.gg__special = "none";                                 // control placed
            c.gg__stretch = "aspect_mult2";                         // control placed
            c.gg__tblur = false;                                    // control placed
            c.gg__tblur__accum = false;                             // control placed
            c.gg__tblur__accum__amount = 50;                        // control placed
            c.gg__videoip = "0";                                    // control placed
            c.gg__xres = 0;                                         // control placed
            c.gg__xscale = 6;                                       // control placed
            c.gg__xscalefs = 1;                                     // control placed
            c.gg__yres = 0;                                         // control placed
            c.gg__yscale = 6;                                       // control placed
            c.gg__yscalefs = 1;                                     // control placed

            c.gg__shader__goat__fprog = false;
            c.gg__shader__goat__hdiv = 0.50;
            c.gg__shader__goat__pat = "goatron";
            c.gg__shader__goat__slen = true;
            c.gg__shader__goat__tp = 0.50;
            c.gg__shader__goat__vdiv = 0.50;
        }
    }
}
