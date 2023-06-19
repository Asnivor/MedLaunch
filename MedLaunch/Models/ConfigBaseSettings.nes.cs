using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Nintendo Entertainment System
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? nes__clipsides { get; set; }
        public bool? nes__correct_aspect { get; set; }
        public bool? nes__fnscan { get; set; }
        public bool? nes__gg { get; set; }
        public string nes__ggrom { get; set; }
        public string nes__input__fcexp { get; set; }
        public string nes__input__port1 { get; set; }
        public string nes__input__port2 { get; set; }
        public string nes__input__port3 { get; set; }
        public string nes__input__port4 { get; set; }
        public bool? nes__n106bs { get; set; }
        public bool? nes__no8lim { get; set; }
        public bool? nes__nofs { get; set; }
        public double? nes__ntsc__brightness { get; set; }          // -1 through 1
        public double? nes__ntsc__contrast { get; set; }            // -1 through 1
        public double? nes__ntsc__hue { get; set; }                 // -1 through 1
        public bool? nes__ntsc__matrix { get; set; }
        public double? nes__ntsc__matrix__0 { get; set; }           // -2 through 2
        public double? nes__ntsc__matrix__1 { get; set; }           // -2 through 2
        public double? nes__ntsc__matrix__2 { get; set; }           // -2 through 2
        public double? nes__ntsc__matrix__3 { get; set; }           // -2 through 2
        public double? nes__ntsc__matrix__4 { get; set; }           // -2 through 2
        public double? nes__ntsc__matrix__5 { get; set; }           // -2 through 2
        public bool? nes__ntsc__mergefields { get; set; }
        public string nes__ntsc__preset { get; set; }               // disabled composite svideo rgb monochrome
        public double? nes__ntsc__saturation { get; set; }          // -1 through 1
        public double? nes__ntsc__sharpness { get; set; }          // -1 through 1
        public bool? nes__ntscblitter { get; set; }
        public bool? nes__pal { get; set; }
        public int? nes__slend { get; set; }                        // 0 through 239
        public int? nes__slendp { get; set; }                        // 0 through 239
        public int? nes__slstart { get; set; }                        // 0 through 239
        public int? nes__slstartp { get; set; }                        // 0 through 239
        public double? nes__sound_rate_error { get; set; }               // 0.0000001 through 0.01
        public int? nes__soundq { get; set; }                       // -2 through 3
        public string nes__debugger__disfontsize { get; set; }
        public string nes__debugger__memcharenc { get; set; }

        public bool? nes__enable { get; set; }
        public string nes__shader { get; set; }
        public int? nes__scanlines { get; set; }
        public string nes__special { get; set; }
        public string nes__stretch { get; set; }
        public bool? nes__tblur { get; set; }
        public bool? nes__tblur__accum { get; set; }
        public double? nes__tblur__accum__amount { get; set; }
        public string nes__videoip { get; set; }
        public int? nes__xres { get; set; }                            // 0 through 65536
        public double? nes__xscale { get; set; }                       // 0.01 through 256
        public double? nes__xscalefs { get; set; }                     // 0.01 through 256
        public int? nes__yres { get; set; }                            // 0 through 65536
        public double? nes__yscale { get; set; }                       // 0.01 through 256
        public double? nes__yscalefs { get; set; }                     // 0.01 through 256

        public bool? nes__shader__goat__fprog { get; set; }
        public double? nes__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string nes__shader__goat__pat { get; set; }
        public bool? nes__shader__goat__slen { get; set; }
        public double? nes__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? nes__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_nes(ConfigBaseSettings c)
        {
            c.nes__clipsides = false;                                 // placed
            c.nes__correct_aspect = false;                            // placed
            c.nes__fnscan = true;                                     // placed
            c.nes__gg = false;                                        // placed
            c.nes__ggrom = "gg.rom";
            c.nes__input__fcexp = "none";                             // placed
            c.nes__input__port1 = "gamepad";                          // placed
            c.nes__input__port2 = "gamepad";                          // placed
            c.nes__input__port3 = "gamepad";                          // placed
            c.nes__input__port4 = "gamepad";                          // placed
            c.nes__n106bs = false;                                    // placed
            c.nes__no8lim = false;                                    // placed
            c.nes__nofs = false;                                      // placed
            c.nes__ntsc__brightness = 0;                              // placed
            c.nes__ntsc__contrast = 0;                                // placed
            c.nes__ntsc__hue = 0;                                     // placed
            c.nes__ntsc__matrix = false;                              // placed
            c.nes__ntsc__matrix__0 = 1.539;                           // placed
            c.nes__ntsc__matrix__1 = -0.622;                          // placed
            c.nes__ntsc__matrix__2 = -0.571;                          // placed
            c.nes__ntsc__matrix__3 = -0.185;                          // placed
            c.nes__ntsc__matrix__4 = 0;                               // placed
            c.nes__ntsc__matrix__5 = 2;                               // placed
            c.nes__ntsc__mergefields = false;                         // placed
            c.nes__ntsc__preset = "disabled";                         // placed
            c.nes__ntsc__saturation = 0;                              // placed
            c.nes__ntsc__sharpness = 0;                               // placed
            c.nes__ntscblitter = false;                               // placed
            c.nes__pal = false;                                       // placed
            c.nes__slend = 231;                                       // placed
            c.nes__slendp = 239;                                      // placed
            c.nes__slstart = 8;                                       // placed
            c.nes__slstartp = 0;                                      // placed
            c.nes__sound_rate_error = 0.00004;                        // placed
            c.nes__soundq = 0;                                        // placed
            c.nes__debugger__disfontsize = "5x7";                      // placed
            c.nes__debugger__memcharenc = "cp437";                     // placed

            c.nes__enable = true;
            c.nes__shader = "none";                               // control placed
            c.nes__scanlines = 0;                                    // control placed
            c.nes__special = "none";                                 // control placed
            c.nes__stretch = "aspect_mult2";                         // control placed
            c.nes__tblur = false;                                    // control placed
            c.nes__tblur__accum = false;                             // control placed
            c.nes__tblur__accum__amount = 50;                        // control placed
            c.nes__videoip = "0";                                    // control placed
            c.nes__xres = 0;                                         // control placed
            c.nes__xscale = 4;                                       // control placed
            c.nes__xscalefs = 1;                                     // control placed
            c.nes__yres = 0;                                         // control placed
            c.nes__yscale = 4;                                       // control placed
            c.nes__yscalefs = 1;                                     // control placed

            c.nes__shader__goat__fprog = false;
            c.nes__shader__goat__hdiv = 0.50;
            c.nes__shader__goat__pat = "goatron";
            c.nes__shader__goat__slen = true;
            c.nes__shader__goat__tp = 0.50;
            c.nes__shader__goat__vdiv = 0.50;
        }
    }
}
