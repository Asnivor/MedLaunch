using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// SNES Faust
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string snes_faust__affinity__msu1__audio { get; set; }   // 0x0000000000000000 through 0xFFFFFFFFFFFFFFFF
        public string snes_faust__affinity__msu1__data { get; set; }    // 0x0000000000000000 through 0xFFFFFFFFFFFFFFFF
        public string snes_faust__affinity__ppu { get; set; }           // 0x0000000000000000 through 0xFFFFFFFFFFFFFFFF
        public bool? snes_faust__correct_aspect { get; set; }
        public int? snes_faust__cx4__clock_rate { get; set; }           // 100 through 500
        public string snes_faust__h_filter { get; set; }
        public double? snes_faust__input__mouse_sensitivity { get; set; }
        public string snes_faust__input__port1 { get; set; }
        public string snes_faust__input__port2 { get; set; }
        public string snes_faust__input__port3 { get; set; }
        public string snes_faust__input__port4 { get; set; }
        public string snes_faust__input__port5 { get; set; }
        public string snes_faust__input__port6 { get; set; }
        public string snes_faust__input__port7 { get; set; }
        public string snes_faust__input__port8 { get; set; }
        public bool snes_faust__input__sport1__multitap { get; set; }
        public bool snes_faust__input__sport2__multitap { get; set; }
        public int? snes_faust__msu1__resamp_quality { get; set; }              // 0 through 5
        public string snes_faust__region { get; set; }
        public string snes_faust__renderer { get; set; }
        public int? snes_faust__resamp_quality { get; set; }                    // 0 through 5        
        public double? snes_faust__resamp_rate_error { get; set; }              // 0.0000001 through 0.0015
        public int? snes_faust__slend { get; set; }                             // 0 through 223
        public int? snes_faust__slendp { get; set; }                            // 0 through 238
        public int? snes_faust__slstart { get; set; }                           // 0 through 223
        public int? snes_faust__slstartp { get; set; }                          // 0 through 238
        public bool? snes_faust__spex { get; set; }
        public bool? snes_faust__spex__sound { get; set; }
        public int? snes_faust__superfx__clock_rate { get; set; }               // 25 through 500
        public bool snes_faust__superfx__icache { get; set; }

        public bool? snes_faust__enable { get; set; }
        public bool? snes_faust__forcemono { get; set; }
        public string snes_faust__shader { get; set; }
        public int? snes_faust__scanlines { get; set; }
        public string snes_faust__special { get; set; }
        public string snes_faust__stretch { get; set; }
        public bool? snes_faust__tblur { get; set; }
        public bool? snes_faust__tblur__accum { get; set; }
        public double? snes_faust__tblur__accum__amount { get; set; }
        public string snes_faust__videoip { get; set; }
        public int? snes_faust__xres { get; set; }                            // 0 through 65536
        public double? snes_faust__xscale { get; set; }                       // 0.01 through 256
        public double? snes_faust__xscalefs { get; set; }                     // 0.01 through 256
        public int? snes_faust__yres { get; set; }                            // 0 through 65536
        public double? snes_faust__yscale { get; set; }                       // 0.01 through 256
        public double? snes_faust__yscalefs { get; set; }                     // 0.01 through 256

        public bool? snes_faust__shader__goat__fprog { get; set; }
        public double? snes_faust__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string snes_faust__shader__goat__pat { get; set; }
        public bool? snes_faust__shader__goat__slen { get; set; }
        public double? snes_faust__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? snes_faust__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_snes_faust(ConfigBaseSettings c)
        {
            c.snes_faust__affinity__msu1__audio = "0x0000000000000000";
            c.snes_faust__affinity__msu1__data = "0x0000000000000000";
            c.snes_faust__affinity__ppu = "0x0000000000000000";
            c.snes_faust__correct_aspect = true;
            c.snes_faust__cx4__clock_rate = 100;                  // placed              
            c.snes_faust__h_filter = "none";                      // placed
            c.snes_faust__input__mouse_sensitivity = 0.50;        // placed                
            c.snes_faust__input__port1 = "gamepad";               // placed
            c.snes_faust__input__port2 = "gamepad";               // placed
            c.snes_faust__input__port3 = "gamepad";               // placed
            c.snes_faust__input__port4 = "gamepad";               // placed
            c.snes_faust__input__port5 = "gamepad";               // placed
            c.snes_faust__input__port6 = "gamepad";               // placed
            c.snes_faust__input__port7 = "gamepad";               // placed
            c.snes_faust__input__port8 = "gamepad";               // placed
            c.snes_faust__input__sport1__multitap = false;
            c.snes_faust__input__sport2__multitap = false;
            c.snes_faust__msu1__resamp_quality = 4;               // placed
            c.snes_faust__region = "auto";                        // placed
            c.snes_faust__renderer = "st";                        // placed
            c.snes_faust__resamp_quality = 3;                     // placed
            c.snes_faust__resamp_rate_error = 0.000035;           // placed
            c.snes_faust__slend = 223;                            // placed
            c.snes_faust__slendp = 238;                           // placed
            c.snes_faust__slstart = 223;                          // placed
            c.snes_faust__slstartp = 238;                         // placed
            c.snes_faust__spex = false;                           // placed
            c.snes_faust__spex__sound = true;                     // placed
            c.snes_faust__superfx__clock_rate = 100;
            c.snes_faust__superfx__icache = false;

            c.snes_faust__enable = true;
            c.snes_faust__forcemono = false;                                // control placed
            c.snes_faust__shader = "none";                                    // control placed
            c.snes_faust__scanlines = 0;                                    // control placed
            c.snes_faust__special = "none";                                 // control placed
            c.snes_faust__stretch = "aspect_mult2";                         // control placed
            c.snes_faust__tblur = false;                                    // control placed
            c.snes_faust__tblur__accum = false;                             // control placed
            c.snes_faust__tblur__accum__amount = 50;                        // control placed
            c.snes_faust__videoip = "1";                                    // control placed
            c.snes_faust__xres = 0;                                         // control placed
            c.snes_faust__xscale = 3;                                       // control placed
            c.snes_faust__xscalefs = 1;                                     // control placed
            c.snes_faust__yres = 0;                                         // control placed
            c.snes_faust__yscale = 3;                                       // control placed
            c.snes_faust__yscalefs = 1;                                     // control placed

            c.snes_faust__shader__goat__fprog = false;
            c.snes_faust__shader__goat__hdiv = 0.50;
            c.snes_faust__shader__goat__pat = "goatron";
            c.snes_faust__shader__goat__slen = true;
            c.snes_faust__shader__goat__tp = 0.50;
            c.snes_faust__shader__goat__vdiv = 0.50;
        }
    }
}
