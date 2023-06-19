using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// SNES
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public int? snes__apu__resamp_quality { get; set; }                     // 0 through 10
        public bool? snes__correct_aspect { get; set; }
        public bool? snes__input__port1__multitap { get; set; }
        public bool? snes__input__port2__multitap { get; set; }
        public string snes__input__port1 { get; set; }
        public string snes__input__port2 { get; set; }
        /*
        public string snes__input__port3 { get; set; }
        public string snes__input__port4 { get; set; }
        public string snes__input__port5 { get; set; }
        public string snes__input__port6 { get; set; }
        public string snes__input__port7 { get; set; }
        public string snes__input__port8 { get; set; }  
        */

        public double? snes__mouse_sensitivity { get; set; }                    // 0.125 through 2

        public bool? snes__enable { get; set; }
        public bool? snes__forcemono { get; set; }
        public string snes__shader { get; set; }
        public int? snes__scanlines { get; set; }
        public string snes__special { get; set; }
        public string snes__stretch { get; set; }
        public bool? snes__tblur { get; set; }
        public bool? snes__tblur__accum { get; set; }
        public double? snes__tblur__accum__amount { get; set; }
        public string snes__videoip { get; set; }
        public int? snes__xres { get; set; }                            // 0 through 65536
        public double? snes__xscale { get; set; }                       // 0.01 through 256
        public double? snes__xscalefs { get; set; }                     // 0.01 through 256
        public int? snes__yres { get; set; }                            // 0 through 65536
        public double? snes__yscale { get; set; }                       // 0.01 through 256
        public double? snes__yscalefs { get; set; }                     // 0.01 through 256

        public bool? snes__shader__goat__fprog { get; set; }
        public double? snes__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string snes__shader__goat__pat { get; set; }
        public bool? snes__shader__goat__slen { get; set; }
        public double? snes__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? snes__shader__goat__vdiv { get; set; }           // -2.00 through 2.00

        public bool? snes__h_blend { get; set; }


        public static void GetDefaults_snes(ConfigBaseSettings c)
        {
            c.snes__apu__resamp_quality = 5;                      // placed
            c.snes__correct_aspect = false;                       // placed     

            c.snes__input__port1__multitap = false;               // placed
            c.snes__input__port2__multitap = false;               // placed
            c.snes__input__port1 = "gamepad";                     // placed
            c.snes__input__port2 = "gamepad";                     // placed
            /*     
            c.snes__input__port3 = "gamepad";                     // placed
            c.snes__input__port4 = "gamepad";                     // placed
            c.snes__input__port5 = "gamepad";                     // placed
            c.snes__input__port6 = "gamepad";                     // placed
            c.snes__input__port7 = "gamepad";                     // placed
            c.snes__input__port8 = "gamepad";
            */
            c.snes__mouse_sensitivity = 0.5;                      // placed

            c.snes__enable = true;
            c.snes__forcemono = false;                                // control placed
            c.snes__shader = "none";                               // control placed
            c.snes__scanlines = 0;                                    // control placed
            c.snes__special = "none";                                 // control placed
            c.snes__stretch = "aspect_mult2";                         // control placed
            c.snes__tblur = false;                                    // control placed
            c.snes__tblur__accum = false;                             // control placed
            c.snes__tblur__accum__amount = 50;                        // control placed
            c.snes__videoip = "0";                                    // control placed
            c.snes__xres = 0;                                         // control placed
            c.snes__xscale = 4;                                       // control placed
            c.snes__xscalefs = 1;                                     // control placed
            c.snes__yres = 0;                                         // control placed
            c.snes__yscale = 4;                                       // control placed
            c.snes__yscalefs = 1;                                     // control placed

            c.snes__shader__goat__fprog = false;
            c.snes__shader__goat__hdiv = 0.50;
            c.snes__shader__goat__pat = "goatron";
            c.snes__shader__goat__slen = true;
            c.snes__shader__goat__tp = 0.50;
            c.snes__shader__goat__vdiv = 0.50;

            c.snes__h_blend = false;
        }

    }
}
