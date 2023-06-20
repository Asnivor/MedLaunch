using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Sega Saturn
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string ss__affinity__vdp2 { get; set; }
        public string ss__bios_jp { get; set; }
        public string ss__bios_na_eu { get; set; }
        public bool? ss__bios_sanity { get; set; }
        public string ss__bios_stv_eu { get; set; }
        public string ss__bios_stv_jp { get; set; }
        public string ss__bios_stv_na { get; set; }
        public string ss__cart { get; set; }
        public string ss__cart__auto_default { get; set; }
        public string ss__cart__kof95_path { get; set; }
        public string ss__cart__ultraman_path { get; set; }
        public bool? ss__cd_sanity { get; set; }
        public bool? ss__correct_aspect { get; set; }
        public bool? ss__h_blend { get; set; }
        public bool? ss__h_overscan { get; set; }
        public double? ss__input__mouse_sensitivity { get; set; }           // 0.125 through 2
        public string ss__input__port1 { get; set; }
        public string ss__input__port1__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port2 { get; set; }
        public string ss__input__port2__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port3 { get; set; }
        public string ss__input__port3__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port4 { get; set; }
        public string ss__input__port4__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port5 { get; set; }
        public string ss__input__port5__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port6 { get; set; }
        public string ss__input__port6__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port7 { get; set; }
        public string ss__input__port7__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port8 { get; set; }
        public string ss__input__port8__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port9 { get; set; }
        public string ss__input__port9__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port10 { get; set; }
        public string ss__input__port10__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port11 { get; set; }
        public string ss__input__port11__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public string ss__input__port12 { get; set; }
        public string ss__input__port12__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        /* moved into CONTROLS section - 2018-03-01
        public string ss__input__port1__3dpad__mode__defpos { get; set; }
        public string ss__input__port2__3dpad__mode__defpos { get; set; }
        public string ss__input__port3__3dpad__mode__defpos { get; set; }
        public string ss__input__port4__3dpad__mode__defpos { get; set; }
        public string ss__input__port5__3dpad__mode__defpos { get; set; }
        public string ss__input__port6__3dpad__mode__defpos { get; set; }
        public string ss__input__port7__3dpad__mode__defpos { get; set; }
        public string ss__input__port8__3dpad__mode__defpos { get; set; }
        public string ss__input__port9__3dpad__mode__defpos { get; set; }
        public string ss__input__port10__3dpad__mode__defpos { get; set; }
        public string ss__input__port11__3dpad__mode__defpos { get; set; }
        public string ss__input__port12__3dpad__mode__defpos { get; set; }
        */

        //public bool? ss__midsync { get; set; }

        public bool? ss__input__sport1__multitap { get; set; }
        public bool? ss__input__sport2__multitap { get; set; }

        public bool? ss__region_autodetect { get; set; }
        public string ss__region_default { get; set; }
        public int? ss__scsp__resamp_quality { get; set; }                  // 0 through 10
        public int? ss__slend { get; set; }                                 //0 through 239
        public int? ss__slendp { get; set; }                                // -16 through 271
        public int? ss__slstart { get; set; }                                 //0 through 239
        public int? ss__slstartp { get; set; }                                // -16 through 271
        public bool? ss__smpc__autortc { get; set; }
        public string ss__smpc__autortc__lang { get; set; }
        
        public string ss__debugger__disfontsize { get; set; }
        public string ss__debugger__memcharenc { get; set; }       

        public bool? ss__enable { get; set; }
        public bool? ss__forcemono { get; set; }
        public string ss__shader { get; set; }
        public int? ss__scanlines { get; set; }
        public string ss__special { get; set; }
        public string ss__stretch { get; set; }
        public bool? ss__tblur { get; set; }
        public bool? ss__tblur__accum { get; set; }
        public double? ss__tblur__accum__amount { get; set; }
        public string ss__videoip { get; set; }
        public int? ss__xres { get; set; }                            // 0 through 65536
        public double? ss__xscale { get; set; }                       // 0.01 through 256
        public double? ss__xscalefs { get; set; }                     // 0.01 through 256
        public int? ss__yres { get; set; }                            // 0 through 65536
        public double? ss__yscale { get; set; }                       // 0.01 through 256
        public double? ss__yscalefs { get; set; }                     // 0.01 through 256

            

        public bool? ss__shader__goat__fprog { get; set; }
        public double? ss__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string ss__shader__goat__pat { get; set; }
        public bool? ss__shader__goat__slen { get; set; }
        public double? ss__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? ss__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_ss(ConfigBaseSettings c)
        {
            c.ss__affinity__vdp2 = "0x0000000000000000";
            c.ss__bios_jp = "sega_101.bin";
            c.ss__bios_na_eu = "mpr-17933.bin";
            c.ss__bios_sanity = true;                                     // placed
            c.ss__bios_stv_eu = "epr-17954a.ic8";
            c.ss__bios_stv_jp = "epr-20091.ic8";
            c.ss__bios_stv_na = "epr-17952a.ic8";
            c.ss__cart = "auto";                                          // placed
            c.ss__cart__auto_default = "backup";							// placed
            c.ss__cart__kof95_path = "mpr-18811-mx.ic1";
            c.ss__cart__ultraman_path = "mpr-19367-mx.ic1";
            c.ss__cd_sanity = true;                                       // placed
            c.ss__input__mouse_sensitivity = 0.5;                         // placed
            c.ss__input__port1 = "gamepad";                               // placed
            c.ss__input__port1__gun_chairs = "0xFF0000";
            c.ss__input__port2 = "gamepad";                               // placed
            c.ss__input__port2__gun_chairs = "0x00FF00";
            c.ss__input__port3 = "gamepad";                               // placed
            c.ss__input__port3__gun_chairs = "0xFF00FF";
            c.ss__input__port4 = "gamepad";                               // placed
            c.ss__input__port4__gun_chairs = "0xFF8000";
            c.ss__input__port5 = "gamepad";                               // placed
            c.ss__input__port5__gun_chairs = "0xFFFF00";
            c.ss__input__port6 = "gamepad";                               // placed
            c.ss__input__port6__gun_chairs = "0x00FFFF";
            c.ss__input__port7 = "gamepad";                               // placed
            c.ss__input__port7__gun_chairs = "0x0080FF";
            c.ss__input__port8 = "gamepad";                               // placed
            c.ss__input__port8__gun_chairs = "0x8000FF";
            c.ss__input__port9 = "gamepad";                               // placed
            c.ss__input__port9__gun_chairs = "0xFF80FF";
            c.ss__input__port10 = "gamepad";                              // placed
            c.ss__input__port10__gun_chairs = "0x00FF80";
            c.ss__input__port11 = "gamepad";                              // placed
            c.ss__input__port11__gun_chairs = "0x8080FF";
            c.ss__input__port12 = "gamepad";                              // placed
            c.ss__input__port12__gun_chairs = "0xFF8080";
            //c.ss__midsync = false;                                        // placed
            c.ss__region_autodetect = true;                               // placed
            c.ss__region_default = "jp";                                  // placed
            c.ss__scsp__resamp_quality = 4;                               // placed
            c.ss__slend = 239;                                            // placed
            c.ss__slendp = 255;                                           // placed
            c.ss__slstart = 0;                                            // placed
            c.ss__slstartp = 0;                                           // placed
            c.ss__smpc__autortc = true;                                   // placed
            c.ss__smpc__autortc__lang = "english";                        // placed
            c.ss__debugger__disfontsize = "5x7";                           // placed
            c.ss__debugger__memcharenc = "SJIS";                           // placed

            c.ss__enable = true;
            c.ss__forcemono = false;                                // control placed
            c.ss__shader = "none";                               // control placed
            c.ss__scanlines = 0;                                    // control placed
            c.ss__special = "none";                                 // control placed
            c.ss__stretch = "aspect_mult2";                         // control placed
            c.ss__tblur = false;                                    // control placed
            c.ss__tblur__accum = false;                             // control placed
            c.ss__tblur__accum__amount = 50;                        // control placed
            c.ss__videoip = "1";                                    // control placed
            c.ss__xres = 0;                                         // control placed
            c.ss__xscale = 3;                                       // control placed
            c.ss__xscalefs = 1;                                     // control placed
            c.ss__yres = 0;                                         // control placed
            c.ss__yscale = 3;                                       // control placed
            c.ss__yscalefs = 1;                                     // control placed

            c.ss__correct_aspect = true;                              // control placed
            c.ss__h_blend = false;                                    // control placed
            c.ss__h_overscan = true;                                  // control placed

            c.ss__shader__goat__fprog = false;
            c.ss__shader__goat__hdiv = 0.50;
            c.ss__shader__goat__pat = "goatron";
            c.ss__shader__goat__slen = true;
            c.ss__shader__goat__tp = 0.50;
            c.ss__shader__goat__vdiv = 0.50;

            c.ss__input__sport1__multitap = false;
            c.ss__input__sport2__multitap = false;

            /* moved into controls section
            c.ss__input__port10__3dpad__mode__defpos = "digital";
            c.ss__input__port11__3dpad__mode__defpos = "digital";
            c.ss__input__port12__3dpad__mode__defpos = "digital";
            c.ss__input__port1__3dpad__mode__defpos = "digital";
            c.ss__input__port2__3dpad__mode__defpos = "digital";
            c.ss__input__port3__3dpad__mode__defpos = "digital";
            c.ss__input__port4__3dpad__mode__defpos = "digital";
            c.ss__input__port5__3dpad__mode__defpos = "digital";
            c.ss__input__port6__3dpad__mode__defpos = "digital";
            c.ss__input__port7__3dpad__mode__defpos = "digital";
            c.ss__input__port8__3dpad__mode__defpos = "digital";
            c.ss__input__port9__3dpad__mode__defpos = "digital";
            */
        }
    }
}
