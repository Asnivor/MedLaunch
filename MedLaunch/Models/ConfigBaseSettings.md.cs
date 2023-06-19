using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Sega MegaDrive / Genesis
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string md__cdbios { get; set; }
        public bool? md__correct_aspect { get; set; }
        public bool? md__input__auto { get; set; }
        public double? md__input__mouse_sensitivity { get; set; }               // 0.25 through 4
        public string md__input__multitap { get; set; }
        public string md__input__port1 { get; set; }
        public string md__input__port2 { get; set; }
        public string md__input__port3 { get; set; }
        public string md__input__port4 { get; set; }
        public string md__input__port5 { get; set; }
        public string md__input__port6 { get; set; }
        public string md__input__port7 { get; set; }
        public string md__input__port8 { get; set; }
        public string md__region { get; set; }
        public string md__reported_region { get; set; }
        public string md__debugger__disfontsize { get; set; }
        public string md__debugger__memcharenc { get; set; }

        public bool? md__enable { get; set; }
        public bool? md__forcemono { get; set; }
        public string md__shader { get; set; }
        public int? md__scanlines { get; set; }
        public string md__special { get; set; }
        public string md__stretch { get; set; }
        public bool? md__tblur { get; set; }
        public bool? md__tblur__accum { get; set; }
        public double? md__tblur__accum__amount { get; set; }
        public string md__videoip { get; set; }
        public int? md__xres { get; set; }                            // 0 through 65536
        public double? md__xscale { get; set; }                       // 0.01 through 256
        public double? md__xscalefs { get; set; }                     // 0.01 through 256
        public int? md__yres { get; set; }                            // 0 through 65536
        public double? md__yscale { get; set; }                       // 0.01 through 256
        public double? md__yscalefs { get; set; }                     // 0.01 through 256

        public bool? md__shader__goat__fprog { get; set; }
        public double? md__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string md__shader__goat__pat { get; set; }
        public bool? md__shader__goat__slen { get; set; }
        public double? md__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? md__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_md(ConfigBaseSettings c)
        {
            c.md__cdbios = "us_scd1_9210.bin";
            c.md__correct_aspect = true;                                  // placed
            c.md__input__auto = true;                                     // placed
            c.md__input__mouse_sensitivity = 1;                           // placed
            c.md__input__multitap = "none";                               // placed
            c.md__input__port1 = "gamepad";                               // placed
            c.md__input__port2 = "gamepad";                               // placed
            c.md__input__port3 = "gamepad";                               // placed
            c.md__input__port4 = "gamepad";                               // placed
            c.md__input__port5 = "gamepad";                               // placed
            c.md__input__port6 = "gamepad";                               // placed
            c.md__input__port7 = "gamepad";                               // placed
            c.md__input__port8 = "gamepad";                               // placed
            c.md__region = "game";                                        // placed                
            c.md__reported_region = "same";                               // placed 
            c.md__debugger__disfontsize = "5x7";
            c.md__debugger__memcharenc = "shift_jis";

            c.md__enable = true;
            c.md__forcemono = false;                                // control placed
            c.md__shader = "none";                               // control placed
            c.md__scanlines = 0;                                    // control placed
            c.md__special = "none";                                 // control placed
            c.md__stretch = "aspect_mult2";                         // control placed
            c.md__tblur = false;                                    // control placed
            c.md__tblur__accum = false;                             // control placed
            c.md__tblur__accum__amount = 50;                        // control placed
            c.md__videoip = "1";                                    // control placed
            c.md__xres = 0;                                         // control placed
            c.md__xscale = 3;                                       // control placed
            c.md__xscalefs = 1;                                     // control placed
            c.md__yres = 0;                                         // control placed
            c.md__yscale = 3;                                       // control placed
            c.md__yscalefs = 1;                                     // control placed

            c.md__shader__goat__fprog = false;
            c.md__shader__goat__hdiv = 0.50;
            c.md__shader__goat__pat = "goatron";
            c.md__shader__goat__slen = true;
            c.md__shader__goat__tp = 0.50;
            c.md__shader__goat__vdiv = 0.50;
        }

    }
}
