using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Gameboy Color
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string gb__system_type { get; set; }                 // auto dmg cgb agb

        public bool? gb__enable { get; set; }
        public bool? gb__forcemono { get; set; }
        public string gb__shader { get; set; }
        public int? gb__scanlines { get; set; }
        public string gb__special { get; set; }
        public string gb__stretch { get; set; }
        public bool? gb__tblur { get; set; }
        public bool? gb__tblur__accum { get; set; }
        public double? gb__tblur__accum__amount { get; set; }
        public string gb__videoip { get; set; }
        public int? gb__xres { get; set; }                            // 0 through 65536
        public double? gb__xscale { get; set; }                       // 0.01 through 256
        public double? gb__xscalefs { get; set; }                     // 0.01 through 256
        public int? gb__yres { get; set; }                            // 0 through 65536
        public double? gb__yscale { get; set; }                       // 0.01 through 256
        public double? gb__yscalefs { get; set; }                     // 0.01 through 256

        public bool? gb__shader__goat__fprog { get; set; }
        public double? gb__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string gb__shader__goat__pat { get; set; }
        public bool? gb__shader__goat__slen { get; set; }
        public double? gb__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? gb__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_gb(ConfigBaseSettings c)
        {
            c.gb__system_type = "auto";                               // placed

            c.gb__enable = true;
            c.gb__forcemono = false;                                // control placed
            c.gb__shader = "none";                               // control placed
            c.gb__scanlines = 0;                                    // control placed
            c.gb__special = "none";                                 // control placed
            c.gb__stretch = "aspect_mult2";                         // control placed
            c.gb__tblur = false;                                    // control placed
            c.gb__tblur__accum = false;                             // control placed
            c.gb__tblur__accum__amount = 50;                        // control placed
            c.gb__videoip = "0";                                    // control placed
            c.gb__xres = 0;                                         // control placed
            c.gb__xscale = 6;                                       // control placed
            c.gb__xscalefs = 1;                                     // control placed
            c.gb__yres = 0;                                         // control placed
            c.gb__yscale = 6;                                       // control placed
            c.gb__yscalefs = 1;                                     // control placed

            c.gb__shader__goat__fprog = false;
            c.gb__shader__goat__hdiv = 0.50;
            c.gb__shader__goat__pat = "goatron";
            c.gb__shader__goat__slen = true;
            c.gb__shader__goat__tp = 0.50;
            c.gb__shader__goat__vdiv = 0.50;
        }
    }
}
