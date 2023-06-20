using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public partial class ConfigBaseSettings
    {
        public string apple2__input__port1 { get; set; }
        public string apple2__input__port2 { get; set; }

        public double? apple2__video__brightness { get; set; }                  // -1.0 through 1.0
        public int? apple2__video__color_lumafilter { get; set; }               // -3 through 3
        public bool? apple2__video__color_smooth { get; set; }
        public double? apple2__video__contrast { get; set; }                    // -1.0 through 1.0

        // forcemono
        public double? apple2__video__hue { get; set; }                         // -1.0 through 1.0
        public string apple2__video__matrix { get; set; }
        public bool? apple2__video__mixed_text_mono { get; set; }
        public string apple2__video__mode { get; set; }

        public int? apple2__video__mono_lumafilter { get; set; }                // -3 through 7
        public double? apple2__video__saturation { get; set; }                  // -1 through 1

        public bool? apple2__enable { get; set; }
        public int? apple2__scanlines { get; set; }
        public string apple2__shader { get; set; }
        public string apple2__special { get; set; }
        public string apple2__stretch { get; set; }
        public bool? apple2__tblur { get; set; }
        public bool? apple2__tblur__accum { get; set; }
        public double? apple2__tblur__accum__amount { get; set; }
        public string apple2__videoip { get; set; }
        public int? apple2__xres { get; set; }                            // 0 through 65536
        public double? apple2__xscale { get; set; }                       // 0.01 through 256
        public double? apple2__xscalefs { get; set; }                     // 0.01 through 256
        public int? apple2__yres { get; set; }                            // 0 through 65536
        public double? apple2__yscale { get; set; }                       // 0.01 through 256
        public double? apple2__yscalefs { get; set; }                     // 0.01 through 256

        public bool? apple2__shader__goat__fprog { get; set; }
        public double? apple2__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string apple2__shader__goat__pat { get; set; }
        public bool? apple2__shader__goat__slen { get; set; }
        public double? apple2__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? apple2__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_apple2(ConfigBaseSettings c)
        {
            c.apple2__input__port1 = "joystick";
            c.apple2__input__port2 = "paddle";

            c.apple2__video__brightness = 0;
            c.apple2__video__color_lumafilter = -3;
            c.apple2__video__color_smooth = false;
            c.apple2__video__contrast = 0;
            c.apple2__video__mixed_text_mono = false;
            c.apple2__video__hue = 0;
            c.apple2__video__matrix = "mednafen";

            c.apple2__video__mode = "composite";

            c.apple2__video__mono_lumafilter = 5;
            c.apple2__video__saturation = 0;

            c.apple2__enable = true;
            c.apple2__shader = "none";                               // control placed
            c.apple2__scanlines = 0;                                    // control placed
            c.apple2__special = "none";                                 // control placed
            c.apple2__stretch = "aspect_mult2";                         // control placed
            c.apple2__tblur = false;                                    // control placed
            c.apple2__tblur__accum = false;                             // control placed
            c.apple2__tblur__accum__amount = 50;                        // control placed
            c.apple2__videoip = "0";                                    // control placed
            c.apple2__xres = 0;                                         // control placed
            c.apple2__xscale = 2;                                       // control placed
            c.apple2__xscalefs = 1;                                     // control placed
            c.apple2__yres = 0;                                         // control placed
            c.apple2__yscale = 2;                                       // control placed
            c.apple2__yscalefs = 1;                                     // control placed

            c.apple2__shader__goat__fprog = false;
            c.apple2__shader__goat__hdiv = 0.50;
            c.apple2__shader__goat__pat = "goatron";
            c.apple2__shader__goat__slen = true;
            c.apple2__shader__goat__tp = 0.50;
            c.apple2__shader__goat__vdiv = 0.50;
        }
    }
}
