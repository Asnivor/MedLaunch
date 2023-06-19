using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// PC Engine (fast)
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? pce_fast__adpcmlp { get; set; }
        public int? pce_fast__adpcmvolume { get; set; }             // 0 through 200
        public bool? pce_fast__arcadecard { get; set; }
        public string pce_fast__cdbios { get; set; }
        public int? pce_fast__cddavolume { get; set; }               // 0 thorugh 200
        public int? pce_fast__cdpsgvolume { get; set; }               // 0 thorugh 200
        public int? pce_fast__cdspeed { get; set; }                 // 1 through 100
        public bool? pce_fast__correct_aspect { get; set; }
        public bool? pce_fast__disable_softreset { get; set; }
        public bool pce_fast__forcesgx { get; set; }
        public string pce_fast__input__port1 { get; set; }
        public string pce_fast__input__port2 { get; set; }
        public string pce_fast__input__port3 { get; set; }
        public string pce_fast__input__port4 { get; set; }
        public string pce_fast__input__port5 { get; set; }
        public double? pce_fast__mouse_sensitivity { get; set; }    // 0.125 through 2
        public bool? pce_fast__nospritelimit { get; set; }
        public int? pce_fast__ocmultiplier { get; set; }            // 1 through 100
        public int? pce_fast__slend { get; set; }                   // 0 through 239
        public int? pce_fast__slstart { get; set; }                 // 0 through 239

        public bool? pce_fast__enable { get; set; }
        public bool? pce_fast__forcemono { get; set; }
        public string pce_fast__shader { get; set; }
        public int? pce_fast__scanlines { get; set; }
        public string pce_fast__special { get; set; }
        public string pce_fast__stretch { get; set; }
        public bool? pce_fast__tblur { get; set; }
        public bool? pce_fast__tblur__accum { get; set; }
        public double? pce_fast__tblur__accum__amount { get; set; }
        public string pce_fast__videoip { get; set; }
        public int? pce_fast__xres { get; set; }                            // 0 through 65536
        public double? pce_fast__xscale { get; set; }                       // 0.01 through 256
        public double? pce_fast__xscalefs { get; set; }                     // 0.01 through 256
        public int? pce_fast__yres { get; set; }                            // 0 through 65536
        public double? pce_fast__yscale { get; set; }                       // 0.01 through 256
        public double? pce_fast__yscalefs { get; set; }                     // 0.01 through 256

        public bool? pce_fast__shader__goat__fprog { get; set; }
        public double? pce_fast__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string pce_fast__shader__goat__pat { get; set; }
        public bool? pce_fast__shader__goat__slen { get; set; }
        public double? pce_fast__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? pce_fast__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_pce_fast(ConfigBaseSettings c)
        {
            c.pce_fast__adpcmlp = false;                              // placed
            c.pce_fast__adpcmvolume = 100;                            // placed
            c.pce_fast__arcadecard = true;                            // placed
            c.pce_fast__cdbios = "syscard3.pce";
            c.pce_fast__cddavolume = 100;                             // placed
            c.pce_fast__cdpsgvolume = 100;                            // placed
            c.pce_fast__cdspeed = 1;                                  // placed
            c.pce_fast__correct_aspect = true;                        // placed
            c.pce_fast__disable_softreset = false;                    // placed
            c.pce_fast__forcesgx = false;                             // placed
            c.pce_fast__input__port1 = "gamepad";                     // placed
            c.pce_fast__input__port2 = "gamepad";                     // placed
            c.pce_fast__input__port3 = "gamepad";                     // placed
            c.pce_fast__input__port4 = "gamepad";                     // placed
            c.pce_fast__input__port5 = "gamepad";                     // placed
            c.pce_fast__mouse_sensitivity = 0.5;                      // placed
            c.pce_fast__nospritelimit = false;                        // placed
            c.pce_fast__ocmultiplier = 1;                             // placed
            c.pce_fast__slend = 235;                                  // placed
            c.pce_fast__slstart = 4;                                  // placed
            c.pce__debugger__disfontsize = "5x7";                      // placed
            c.pce__debugger__memcharenc = "shift_jis";                 // placed

            c.pce_fast__enable = true;
            c.pce_fast__forcemono = false;                                // control placed
            c.pce_fast__shader = "none";                               // control placed
            c.pce_fast__scanlines = 0;                                    // control placed
            c.pce_fast__special = "none";                                 // control placed
            c.pce_fast__stretch = "aspect_mult2";                         // control placed
            c.pce_fast__tblur = false;                                    // control placed
            c.pce_fast__tblur__accum = false;                             // control placed
            c.pce_fast__tblur__accum__amount = 50;                        // control placed
            c.pce_fast__videoip = "1";                                    // control placed
            c.pce_fast__xres = 0;                                         // control placed
            c.pce_fast__xscale = 3;                                       // control placed
            c.pce_fast__xscalefs = 1;                                     // control placed
            c.pce_fast__yres = 0;                                         // control placed
            c.pce_fast__yscale = 3;                                       // control placed
            c.pce_fast__yscalefs = 1;                                     // control placed

            c.pce_fast__shader__goat__fprog = false;
            c.pce_fast__shader__goat__hdiv = 0.50;
            c.pce_fast__shader__goat__pat = "goatron";
            c.pce_fast__shader__goat__slen = true;
            c.pce_fast__shader__goat__tp = 0.50;
            c.pce_fast__shader__goat__vdiv = 0.50;
        }
    }
}
