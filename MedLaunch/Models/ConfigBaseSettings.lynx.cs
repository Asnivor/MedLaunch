using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Atari Lynx
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? lynx__lowpass { get; set; }
        public bool? lynx__rotateinput { get; set; }

        public bool? lynx__enable { get; set; }
        public bool? lynx__forcemono { get; set; }
        public string lynx__shader { get; set; }
        public int? lynx__scanlines { get; set; }
        public string lynx__special { get; set; }
        public string lynx__stretch { get; set; }
        public bool? lynx__tblur { get; set; }
        public bool? lynx__tblur__accum { get; set; }
        public double? lynx__tblur__accum__amount { get; set; }
        public string lynx__videoip { get; set; }
        public int? lynx__xres { get; set; }                            // 0 through 65536
        public double? lynx__xscale { get; set; }                       // 0.01 through 256
        public double? lynx__xscalefs { get; set; }                     // 0.01 through 256
        public int? lynx__yres { get; set; }                            // 0 through 65536
        public double? lynx__yscale { get; set; }                       // 0.01 through 256
        public double? lynx__yscalefs { get; set; }                     // 0.01 through 256

        public bool? lynx__shader__goat__fprog { get; set; }
        public double? lynx__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string lynx__shader__goat__pat { get; set; }
        public bool? lynx__shader__goat__slen { get; set; }
        public double? lynx__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? lynx__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_lynx(ConfigBaseSettings c)
        {
            c.lynx__lowpass = true;                                   // placed
            c.lynx__rotateinput = true;                               // placed

            c.lynx__enable = true;
            c.lynx__forcemono = false;                                // control placed
            c.lynx__shader = "none";                                    // control placed
            c.lynx__scanlines = 0;                                    // control placed
            c.lynx__special = "none";                                 // control placed
            c.lynx__stretch = "aspect_mult2";                         // control placed
            c.lynx__tblur = false;                                    // control placed
            c.lynx__tblur__accum = false;                             // control placed
            c.lynx__tblur__accum__amount = 50;                        // control placed
            c.lynx__videoip = "0";                                    // control placed
            c.lynx__xres = 0;                                         // control placed
            c.lynx__xscale = 6;                                       // control placed
            c.lynx__xscalefs = 1;                                     // control placed
            c.lynx__yres = 0;                                         // control placed
            c.lynx__yscale = 6;                                       // control placed
            c.lynx__yscalefs = 1;                                     // control placed

            c.lynx__shader__goat__fprog = false;
            c.lynx__shader__goat__hdiv = 0.50;
            c.lynx__shader__goat__pat = "goatron";
            c.lynx__shader__goat__slen = true;
            c.lynx__shader__goat__tp = 0.50;
            c.lynx__shader__goat__vdiv = 0.50;
        }
    }
}
