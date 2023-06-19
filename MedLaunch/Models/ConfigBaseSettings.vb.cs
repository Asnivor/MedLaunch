using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Nintendo Virtual Boy
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string vb__3dmode { get; set; }
        public bool? vb__3dreverse { get; set; }
        public bool? vb__allow_draw_skip { get; set; }
        public string vb__anaglyph__lcolor { get; set; }
        public string vb__anaglyph__preset { get; set; }
        public string vb__anaglyph__rcolor { get; set; }
        public string vb__cpu_emulation { get; set; }
        public string vb__default_color { get; set; }
        public bool? vb__disable_parallax { get; set; }
        public bool? vb__input__instant_read_hack { get; set; }
        public bool? vb__instant_display_hack { get; set; }
        public int? vb__liprescale { get; set; }                                // 1 through 10
        public int? vb__sidebyside__separation { get; set; }                    // 0 though 1024

        public string vb__debugger__disfontsize { get; set; }
        public string vb__debugger__memcharenc { get; set; }

        public bool? vb__enable { get; set; }
        public bool? vb__forcemono { get; set; }
        public string vb__shader { get; set; }
        public int? vb__scanlines { get; set; }
        public string vb__special { get; set; }
        public string vb__stretch { get; set; }
        public bool? vb__tblur { get; set; }
        public bool? vb__tblur__accum { get; set; }
        public double? vb__tblur__accum__amount { get; set; }
        public string vb__videoip { get; set; }
        public int? vb__xres { get; set; }                            // 0 through 65536
        public double? vb__xscale { get; set; }                       // 0.01 through 256
        public double? vb__xscalefs { get; set; }                     // 0.01 through 256
        public int? vb__yres { get; set; }                            // 0 through 65536
        public double? vb__yscale { get; set; }                       // 0.01 through 256
        public double? vb__yscalefs { get; set; }                     // 0.01 through 256

        public double? vb__ledonscale { get; set; }                   // 1 through 2    

        public bool? vb__shader__goat__fprog { get; set; }
        public double? vb__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string vb__shader__goat__pat { get; set; }
        public bool? vb__shader__goat__slen { get; set; }
        public double? vb__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? vb__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_vb(ConfigBaseSettings c)
        {
            c.vb__3dmode = "anaglyph";                            // placed
            c.vb__3dreverse = false;                              // placed
            c.vb__allow_draw_skip = false;                        // placed
            c.vb__anaglyph__lcolor = "0xffba00";                  // placed
            c.vb__anaglyph__preset = "red_blue";                   // placed
            c.vb__anaglyph__rcolor = "0x00baff";                  // placed
            c.vb__cpu_emulation = "fast";                         // placed
            c.vb__default_color = "0xF0F0F0";                     // placed
            c.vb__disable_parallax = false;                       // placed
            c.vb__input__instant_read_hack = true;                // placed
            c.vb__instant_display_hack = false;                   // placed
            c.vb__liprescale = 2;                                 // placed
            c.vb__sidebyside__separation = 0;                     // placed
            c.vb__debugger__disfontsize = "5x7";                   // placed
            c.vb__debugger__memcharenc = "shift_jis";              // placed

            c.vb__enable = true;
            c.vb__forcemono = false;                                // control placed
            c.vb__shader = "none";                               // control placed
            c.vb__scanlines = 0;                                    // control placed
            c.vb__special = "none";                                 // control placed
            c.vb__stretch = "aspect_mult2";                         // control placed
            c.vb__tblur = false;                                    // control placed
            c.vb__tblur__accum = false;                             // control placed
            c.vb__tblur__accum__amount = 50;                        // control placed
            c.vb__videoip = "0";                                    // control placed
            c.vb__xres = 0;                                         // control placed
            c.vb__xscale = 2;                                       // control placed
            c.vb__xscalefs = 1;                                     // control placed
            c.vb__yres = 0;                                         // control placed
            c.vb__yscale = 2;                                       // control placed
            c.vb__yscalefs = 1;                                     // control placed

            c.vb__ledonscale = 1.75;

            c.vb__shader__goat__fprog = false;
            c.vb__shader__goat__hdiv = 0.50;
            c.vb__shader__goat__pat = "goatron";
            c.vb__shader__goat__slen = true;
            c.vb__shader__goat__tp = 0.50;
            c.vb__shader__goat__vdiv = 0.50;
        }
    }
}
