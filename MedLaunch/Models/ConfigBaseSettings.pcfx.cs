using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// PC-FX
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? pcfx__adpcm__emulate_buggy_codec { get; set; }
        public bool? pcfx__adpcm__suppress_channel_reset_clicks { get; set; }
        public string pcfx__bios { get; set; }
        public int? pcfx__cdspeed { get; set; }                     // 2 through 10
        public string pcfx__cpu_emulation { get; set; }
        public bool? pcfx__disable_bram { get; set; }
        public bool? pcfx__disable_softreset { get; set; }
        public string pcfx__fxscsi { get; set; }
        public string pcfx__high_dotclock_width { get; set; }
        public string pcfx__input__port1 { get; set; }
        public bool? pcfx__input__port1__multitap { get; set; }
        public string pcfx__input__port2 { get; set; }
        public bool? pcfx__input__port2__multitap { get; set; }
        public string pcfx__input__port3 { get; set; }
        public string pcfx__input__port4 { get; set; }
        public string pcfx__input__port5 { get; set; }
        public string pcfx__input__port6 { get; set; }
        public string pcfx__input__port7 { get; set; }
        public string pcfx__input__port8 { get; set; }
        public double? pcfx__mouse_sensitivity { get; set; }            // 0.3125 through 5
        public bool? pcfx__nospritelimit { get; set; }
        public bool? pcfx__rainbow__chromaip { get; set; }
        public int? pcfx__resamp_quality { get; set; }                  // 0 through 5
        public double? pcfx__resamp_rate_error { get; set; }             // 0.0000001 through 0.0000350
        public int? pcfx__slend { get; set; }                           // 0 through 239
        public int? pcfx__slstart { get; set; }                         // 0 through 239
        public string pcfx__debugger__disfontsize { get; set; }
        public string pcfx__debugger__memcharenc { get; set; }

        public bool? pcfx__enable { get; set; }
        public bool? pcfx__forcemono { get; set; }
        public string pcfx__shader { get; set; }
        public int? pcfx__scanlines { get; set; }
        public string pcfx__special { get; set; }
        public string pcfx__stretch { get; set; }
        public bool? pcfx__tblur { get; set; }
        public bool? pcfx__tblur__accum { get; set; }
        public double? pcfx__tblur__accum__amount { get; set; }
        public string pcfx__videoip { get; set; }
        public int? pcfx__xres { get; set; }                            // 0 through 65536
        public double? pcfx__xscale { get; set; }                       // 0.01 through 256
        public double? pcfx__xscalefs { get; set; }                     // 0.01 through 256
        public int? pcfx__yres { get; set; }                            // 0 through 65536
        public double? pcfx__yscale { get; set; }                       // 0.01 through 256
        public double? pcfx__yscalefs { get; set; }                     // 0.01 through 256

        public bool? pcfx__shader__goat__fprog { get; set; }
        public double? pcfx__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string pcfx__shader__goat__pat { get; set; }
        public bool? pcfx__shader__goat__slen { get; set; }
        public double? pcfx__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? pcfx__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_pcfx(ConfigBaseSettings c)
        {
            c.pcfx__adpcm__emulate_buggy_codec = false;                   // placed
            c.pcfx__adpcm__suppress_channel_reset_clicks = true;          // placed
            c.pcfx__bios = "pcfx.rom";
            c.pcfx__cdspeed = 2;                                          // placed
            c.pcfx__cpu_emulation = "auto";                               // placed
            c.pcfx__disable_bram = false;                                 // placed
            c.pcfx__disable_softreset = false;                            // placed
            c.pcfx__fxscsi = null;
            c.pcfx__high_dotclock_width = "1024";                         // placed
            c.pcfx__input__port1 = "gamepad";                             // placed
            c.pcfx__input__port1__multitap = false;                       // placed
            c.pcfx__input__port2 = "gamepad";                             // placed
            c.pcfx__input__port2__multitap = false;                       // placed
            c.pcfx__input__port3 = "gamepad";                             // placed
            c.pcfx__input__port4 = "gamepad";                             // placed
            c.pcfx__input__port5 = "gamepad";                             // placed
            c.pcfx__input__port6 = "gamepad";                             // placed
            c.pcfx__input__port7 = "gamepad";                             // placed
            c.pcfx__input__port8 = "gamepad";                             // placed
            c.pcfx__mouse_sensitivity = 1.25;                             // placed
            c.pcfx__nospritelimit = false;                                // placed
            c.pcfx__rainbow__chromaip = false;                            // placed
            c.pcfx__resamp_quality = 3;                                   // placed
            c.pcfx__resamp_rate_error = 0.0000009;                        // placed
            c.pcfx__slend = 235;                                          // placed
            c.pcfx__slstart = 4;                                          // placed
            c.pcfx__debugger__disfontsize = "5x7";                         // palced
            c.pcfx__debugger__memcharenc = "shift_jis";                    // placed

            c.pcfx__enable = true;
            c.pcfx__forcemono = false;                                // control placed
            c.pcfx__shader = "none";                               // control placed
            c.pcfx__scanlines = 0;                                    // control placed
            c.pcfx__special = "none";                                 // control placed
            c.pcfx__stretch = "aspect_mult2";                         // control placed
            c.pcfx__tblur = false;                                    // control placed
            c.pcfx__tblur__accum = false;                             // control placed
            c.pcfx__tblur__accum__amount = 50;                        // control placed
            c.pcfx__videoip = "1";                                    // control placed
            c.pcfx__xres = 0;                                         // control placed
            c.pcfx__xscale = 3;                                       // control placed
            c.pcfx__xscalefs = 1;                                     // control placed
            c.pcfx__yres = 0;                                         // control placed
            c.pcfx__yscale = 3;                                       // control placed
            c.pcfx__yscalefs = 1;                                     // control placed

            c.pcfx__shader__goat__fprog = false;
            c.pcfx__shader__goat__hdiv = 0.50;
            c.pcfx__shader__goat__pat = "goatron";
            c.pcfx__shader__goat__slen = true;
            c.pcfx__shader__goat__tp = 0.50;
            c.pcfx__shader__goat__vdiv = 0.50;
        }
    }
}
