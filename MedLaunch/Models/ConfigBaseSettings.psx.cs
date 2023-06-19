using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Sony Playstation
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string psx__bios_eu { get; set; }
        public string psx__bios_jp { get; set; }
        public string psx__bios_na { get; set; }
        public bool? psx__bios_sanity { get; set; }
        public bool? psx__cd_sanity { get; set; }
        public int? psx__dbg_level { get; set; }                            // 0 through 4
        public bool? psx__h_overscan { get; set; }
        public bool? psx__input__analog_mode_ct { get; set; }
        public double? psx__input__mouse_sensitivity { get; set; }         // 0.25 through 4

        public string psx__input__port1 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port1__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port1__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port1__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port1__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port1__memcard { get; set; }

        public string psx__input__port2 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port2__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port2__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port2__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port2__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port2__memcard { get; set; }

        public string psx__input__port3 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port3__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port3__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port3__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port3__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port3__memcard { get; set; }

        public string psx__input__port4 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port4__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port4__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port4__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port4__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port4__memcard { get; set; }

        public string psx__input__port5 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port5__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port5__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port5__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port5__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port5__memcard { get; set; }

        public string psx__input__port6 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port6__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port6__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port6__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port6__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port6__memcard { get; set; }

        public string psx__input__port7 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port7__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port7__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port7__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port7__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port7__memcard { get; set; }

        public string psx__input__port8 { get; set; }
        /* removed 2018-03-01
        public double? psx__input__port8__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port8__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port8__dualshock__axis_scale { get; set; }       // 1 through 1.5
        */
        public string psx__input__port8__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port8__memcard { get; set; }

        public bool? psx__input__pport1__multitap { get; set; }
        public bool? psx__input__pport2__multitap { get; set; }
        public bool? psx__region_autodetect { get; set; }
        public string psx__region_default { get; set; }
        public int? psx__slend { get; set; }                                // 0 through 239
        public int? psx__slendp { get; set; }                               // 0 through 287
        public int? psx__slstart { get; set; }                                // 0 through 239
        public int? psx__slstartp { get; set; }                               // 0 through 287
        public int? psx__spu__resamp_quality { get; set; }                      // 0 through 10

        public string psx__debugger__disfontsize { get; set; }
        public string psx__debugger__memcharenc { get; set; }

        public bool? psx__enable { get; set; }
        public bool? psx__forcemono { get; set; }
        public string psx__shader { get; set; }
        public int? psx__scanlines { get; set; }
        public string psx__special { get; set; }
        public string psx__stretch { get; set; }
        public bool? psx__tblur { get; set; }
        public bool? psx__tblur__accum { get; set; }
        public double? psx__tblur__accum__amount { get; set; }
        public string psx__videoip { get; set; }
        public int? psx__xres { get; set; }                            // 0 through 65536
        public double? psx__xscale { get; set; }                       // 0.01 through 256
        public double? psx__xscalefs { get; set; }                     // 0.01 through 256
        public int? psx__yres { get; set; }                            // 0 through 65536
        public double? psx__yscale { get; set; }                       // 0.01 through 256
        public double? psx__yscalefs { get; set; }                     // 0.01 through 256

        public string psx__input__analog_mode_ct__compare { get; set; }

        public bool? psx__shader__goat__fprog { get; set; }
        public double? psx__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string psx__shader__goat__pat { get; set; }
        public bool? psx__shader__goat__slen { get; set; }
        public double? psx__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? psx__shader__goat__vdiv { get; set; }           // -2.00 through 2.00

        public bool? psx_shared_memcards { get; set; }


        public static void GetDefaults_psx(ConfigBaseSettings c)
        {
            c.psx__bios_eu = "scph5502.bin";
            c.psx__bios_jp = "scph5500.bin";
            c.psx__bios_na = "scph5501.bin";
            c.psx__bios_sanity = true;                                    // placed
            c.psx__cd_sanity = true;                                      // placed
            c.psx__dbg_level = 0;                                         // placed
            c.psx__h_overscan = true;                                     // placed
            c.psx__input__analog_mode_ct = false;                         // placed
            c.psx__input__mouse_sensitivity = 1;                          // placed
            c.psx__input__port1 = "gamepad";
            //c.psx__input__port1__analogjoy__axis_scale = 1;               // placed
            //c.psx__input__port1__dualanalog__axis_scale = 1;          // placed
            //c.psx__input__port1__dualshock__axis_scale = 1;   // placed
            c.psx__input__port1__gun_chairs = "0xFF0000";     // placed
            c.psx__input__port1__memcard = true;              // placed
            c.psx__input__port2 = "gamepad";  // placed
            //c.psx__input__port2__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port2__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port2__dualshock__axis_scale = 1;   // placed
            c.psx__input__port2__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port2__memcard = true;  // placed
            c.psx__input__port3 = "gamepad";  // placed
            //c.psx__input__port3__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port3__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port3__dualshock__axis_scale = 1;   // placed
            c.psx__input__port3__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port3__memcard = true;  // placed
            c.psx__input__port4 = "gamepad";  // placed
            //c.psx__input__port4__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port4__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port4__dualshock__axis_scale = 1;// placed
            c.psx__input__port4__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port4__memcard = true;  // placed
            c.psx__input__port5 = "gamepad";  // placed
            //c.psx__input__port5__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port5__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port5__dualshock__axis_scale = 1;   // placed
            c.psx__input__port5__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port5__memcard = true;  // placed
            c.psx__input__port6 = "gamepad";  // placed
            //c.psx__input__port6__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port6__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port6__dualshock__axis_scale = 1;   // placed
            c.psx__input__port6__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port6__memcard = true;  // placed
            c.psx__input__port7 = "gamepad";  // placed
            //c.psx__input__port7__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port7__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port7__dualshock__axis_scale = 1;   // placed
            c.psx__input__port7__gun_chairs = "0xFF0000"; // placed
            c.psx__input__port7__memcard = true;  // placed
            c.psx__input__port8 = "gamepad";  // placed
            //c.psx__input__port8__analogjoy__axis_scale = 1;   // placed
            //c.psx__input__port8__dualanalog__axis_scale = 1;  // placed
            //c.psx__input__port8__dualshock__axis_scale = 1;   // placed
            c.psx__input__port8__gun_chairs = "0xFF0000";         // placed
            c.psx__input__port8__memcard = true;                  // placed
            c.psx__input__pport1__multitap = false;               // placed
            c.psx__input__pport2__multitap = false;               // placed
            c.psx__region_autodetect = true;                      // placed
            c.psx__region_default = "jp";                         // placed
            c.psx__slend = 239;                                   // placed
            c.psx__slendp = 287;                                  // placed
            c.psx__slstart = 0;                                   // placed
            c.psx__slstartp = 0;                                  // placed
            c.psx__spu__resamp_quality = 5;                       // placed
            c.psx__debugger__disfontsize = "5x7";                  // placed
            c.psx__debugger__memcharenc = "shift_jis";             // placed

            c.psx__enable = true;
            c.psx__forcemono = false;                                // control placed
            c.psx__shader = "none";                               // control placed
            c.psx__scanlines = 0;                                    // control placed
            c.psx__special = "none";                                 // control placed
            c.psx__stretch = "aspect_mult2";                         // control placed
            c.psx__tblur = false;                                    // control placed
            c.psx__tblur__accum = false;                             // control placed
            c.psx__tblur__accum__amount = 50;                        // control placed
            c.psx__videoip = "1";                                    // control placed
            c.psx__xres = 0;                                         // control placed
            c.psx__xscale = 3;                                       // control placed
            c.psx__xscalefs = 1;                                     // control placed
            c.psx__yres = 0;                                         // control placed
            c.psx__yscale = 3;                                       // control placed
            c.psx__yscalefs = 1;                                     // control placed

            c.psx__input__analog_mode_ct__compare = "0x0F09";

            c.psx__shader__goat__fprog = false;
            c.psx__shader__goat__hdiv = 0.50;
            c.psx__shader__goat__pat = "goatron";
            c.psx__shader__goat__slen = true;
            c.psx__shader__goat__tp = 0.50;
            c.psx__shader__goat__vdiv = 0.50;

            c.psx_shared_memcards = false;
        }
    }
}
