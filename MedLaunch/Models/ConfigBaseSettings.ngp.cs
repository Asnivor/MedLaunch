using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// NeoGeo Pocket Color
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public string npg__language { get; set; }                   // english japanese

        public bool? ngp__enable { get; set; }
        public bool? ngp__forcemono { get; set; }
        public string ngp__shader { get; set; }
        public int? ngp__scanlines { get; set; }
        public string ngp__special { get; set; }
        public string ngp__stretch { get; set; }
        public bool? ngp__tblur { get; set; }
        public bool? ngp__tblur__accum { get; set; }
        public double? ngp__tblur__accum__amount { get; set; }
        public string ngp__videoip { get; set; }
        public int? ngp__xres { get; set; }                            // 0 through 65536
        public double? ngp__xscale { get; set; }                       // 0.01 through 256
        public double? ngp__xscalefs { get; set; }                     // 0.01 through 256
        public int? ngp__yres { get; set; }                            // 0 through 65536
        public double? ngp__yscale { get; set; }                       // 0.01 through 256
        public double? ngp__yscalefs { get; set; }                     // 0.01 through 256

        public bool? ngp__shader__goat__fprog { get; set; }
        public double? ngp__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string ngp__shader__goat__pat { get; set; }
        public bool? ngp__shader__goat__slen { get; set; }
        public double? ngp__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? ngp__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_ngp(ConfigBaseSettings c)
        {
            c.npg__language = "english";                              // placed

            c.ngp__enable = true;
            c.ngp__forcemono = false;                                // control placed
            c.ngp__shader = "none";                               // control placed
            c.ngp__scanlines = 0;                                    // control placed
            c.ngp__special = "none";                                 // control placed
            c.ngp__stretch = "aspect_mult2";                         // control placed
            c.ngp__tblur = false;                                    // control placed
            c.ngp__tblur__accum = false;                             // control placed
            c.ngp__tblur__accum__amount = 50;                        // control placed
            c.ngp__videoip = "0";                                    // control placed
            c.ngp__xres = 0;                                         // control placed
            c.ngp__xscale = 6;                                       // control placed
            c.ngp__xscalefs = 1;                                     // control placed
            c.ngp__yres = 0;                                         // control placed
            c.ngp__yscale = 6;                                       // control placed
            c.ngp__yscalefs = 1;                                     // control placed

            c.ngp__shader__goat__fprog = false;
            c.ngp__shader__goat__hdiv = 0.50;
            c.ngp__shader__goat__pat = "goatron";
            c.ngp__shader__goat__slen = true;
            c.ngp__shader__goat__tp = 0.50;
            c.ngp__shader__goat__vdiv = 0.50;
        }
    }
}
