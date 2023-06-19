using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// PC-Engine
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? pce__adpcmextraprec { get; set; }
        public int? pce__adpcmvolume { get; set; }                  // 0 through 200
        public bool? pce__arcadecard { get; set; }
        public string pce__cdbios { get; set; }
        public int? pce__cddavolume { get; set; }               // 0 thorugh 200
        public int? pce__cdpsgvolume { get; set; }              // 0 thorugh 200
        public bool? pce__disable_bram_cd { get; set; }
        public bool? pce__disable_bram_hucard { get; set; }
        public bool? pce__disable_softreset { get; set; }
        public bool? pce__forcesgx { get; set; }
        public string pce__gecdbios { get; set; }
        public bool? pce__h_overscan { get; set; }
        public bool? pce__input__multitap { get; set; }
        public string pce__input__port1 { get; set; }
        public string pce__input__port2 { get; set; }
        public string pce__input__port3 { get; set; }
        public string pce__input__port4 { get; set; }
        public string pce__input__port5 { get; set; }
        public double? pce__mouse_sensitivity { get; set; }         // 0.125 through 2
        public bool? pce__nospritelimit { get; set; }
        public string pce__psgrevision { get; set; }
        public int? pce__resamp_quality { get; set; }               // 0 through 5
        public double? pce__resamp_rate_error { get; set; }         //          0.0000001 throgh        0.0000350
        public int? pce__slend { get; set; }                        // 0 through 239
        public int? pce__slstart { get; set; }                      // 0 through 239
        public string pce__debugger__disfontsize { get; set; }
        public string pce__debugger__memcharenc { get; set; }

        public bool? pce__enable { get; set; }
        public bool? pce__forcemono { get; set; }
        public string pce__shader { get; set; }
        public int? pce__scanlines { get; set; }
        public string pce__special { get; set; }
        public string pce__stretch { get; set; }
        public bool? pce__tblur { get; set; }
        public bool? pce__tblur__accum { get; set; }
        public double? pce__tblur__accum__amount { get; set; }
        public string pce__videoip { get; set; }
        public int? pce__xres { get; set; }                            // 0 through 65536
        public double? pce__xscale { get; set; }                       // 0.01 through 256
        public double? pce__xscalefs { get; set; }                     // 0.01 through 256
        public int? pce__yres { get; set; }                            // 0 through 65536
        public double? pce__yscale { get; set; }                       // 0.01 through 256
        public double? pce__yscalefs { get; set; }                     // 0.01 through 256

        public bool? pce__shader__goat__fprog { get; set; }
        public double? pce__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string pce__shader__goat__pat { get; set; }
        public bool? pce__shader__goat__slen { get; set; }
        public double? pce__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? pce__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_pce(ConfigBaseSettings c)
        {
            c.pce__adpcmextraprec = false;                            // placed
            c.pce__adpcmvolume = 100;                                 // placed
            c.pce__arcadecard = true;                                 // placed
            c.pce__cdbios = "syscard3.pce";
            c.pce__cddavolume = 100;                                  // placed
            c.pce__cdpsgvolume = 100;                                 // placed
            c.pce__disable_bram_cd = false;                           // placed
            c.pce__disable_bram_hucard = false;                       // placed
            c.pce__disable_softreset = false;                         // placed
            c.pce__forcesgx = false;                                  // placed
            c.pce__gecdbios = "gecard.pce";
            c.pce__h_overscan = false;                                // placed
            c.pce__input__multitap = true;                            // placed
            c.pce__input__port1 = "gamepad";                          // placed
            c.pce__input__port2 = "gamepad";                          // placed
            c.pce__input__port3 = "gamepad";                          // placed
            c.pce__input__port4 = "gamepad";                          // placed
            c.pce__input__port5 = "gamepad";                          // placed
            c.pce__mouse_sensitivity = 0.5;                           // placed
            c.pce__nospritelimit = false;                             // placed
            c.pce__psgrevision = "match";                             // placed
            c.pce__resamp_quality = 3;
            c.pce__resamp_rate_error = 0.0000009;
            c.pce__slend = 235;
            c.pce__slstart = 4;

            c.pce__shader__goat__fprog = false;
            c.pce__shader__goat__hdiv = 0.50;
            c.pce__shader__goat__pat = "goatron";
            c.pce__shader__goat__slen = true;
            c.pce__shader__goat__tp = 0.50;
            c.pce__shader__goat__vdiv = 0.50;

            c.pce__enable = true;
            c.pce__forcemono = false;                                // control placed
            c.pce__shader = "none";                               // control placed
            c.pce__scanlines = 0;                                    // control placed
            c.pce__special = "none";                                 // control placed
            c.pce__stretch = "aspect_mult2";                         // control placed
            c.pce__tblur = false;                                    // control placed
            c.pce__tblur__accum = false;                             // control placed
            c.pce__tblur__accum__amount = 50;                        // control placed
            c.pce__videoip = "1";                                    // control placed
            c.pce__xres = 0;                                         // control placed
            c.pce__xscale = 3;                                       // control placed
            c.pce__xscalefs = 1;                                     // control placed
            c.pce__yres = 0;                                         // control placed
            c.pce__yscale = 3;                                       // control placed
            c.pce__yscalefs = 1;                                     // control placed
        }
    }
}
