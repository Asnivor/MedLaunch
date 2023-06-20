using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Sega Master System
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public bool? sms__fm { get; set; }
        public int? sms__slstart { get; set; }
        public int? sms__slend { get; set; }
        public int? sms__slstartp { get; set; }
        public int? sms__slendp { get; set; }
        public string sms__territory { get; set; }

        public bool? sms__enable { get; set; }
        public bool? sms__forcemono { get; set; }
        public string sms__shader { get; set; }
        public int? sms__scanlines { get; set; }
        public string sms__special { get; set; }
        public string sms__stretch { get; set; }
        public bool? sms__tblur { get; set; }
        public bool? sms__tblur__accum { get; set; }
        public double? sms__tblur__accum__amount { get; set; }
        public string sms__videoip { get; set; }
        public int? sms__xres { get; set; }                            // 0 through 65536
        public double? sms__xscale { get; set; }                       // 0.01 through 256
        public double? sms__xscalefs { get; set; }                     // 0.01 through 256
        public int? sms__yres { get; set; }                            // 0 through 65536
        public double? sms__yscale { get; set; }                       // 0.01 through 256
        public double? sms__yscalefs { get; set; }                     // 0.01 through 256        

        public bool? sms__shader__goat__fprog { get; set; }
        public double? sms__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string sms__shader__goat__pat { get; set; }
        public bool? sms__shader__goat__slen { get; set; }
        public double? sms__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? sms__shader__goat__vdiv { get; set; }           // -2.00 through 2.00


        public static void GetDefaults_sms(ConfigBaseSettings c)
        {
            c.sms__fm = true;                                             // placed
            c.sms__territory = "export";                                   // placed

            c.sms__enable = true;
            c.sms__forcemono = false;                                // control placed
            c.sms__shader = "none";                               // control placed
            c.sms__scanlines = 0;                                    // control placed
            c.sms__special = "none";                                 // control placed
            c.sms__stretch = "aspect_mult2";                         // control placed
            c.sms__tblur = false;                                    // control placed
            c.sms__tblur__accum = false;                             // control placed
            c.sms__tblur__accum__amount = 50;                        // control placed
            c.sms__videoip = "0";                                    // control placed
            c.sms__xres = 0;                                         // control placed
            c.sms__xscale = 4;                                       // control placed
            c.sms__xscalefs = 1;                                     // control placed
            c.sms__yres = 0;                                         // control placed
            c.sms__yscale = 4;                                       // control placed
            c.sms__yscalefs = 1;                                     // control placed

            c.sms__slend = 239;                                        // control placed
            c.sms__slendp = 239;                                       // control placed
            c.sms__slstartp = 0;                                       // control placed
            c.sms__slstart = 0;                                       // control placed

            c.sms__shader__goat__fprog = false;
            c.sms__shader__goat__hdiv = 0.50;
            c.sms__shader__goat__pat = "goatron";
            c.sms__shader__goat__slen = true;
            c.sms__shader__goat__tp = 0.50;
            c.sms__shader__goat__vdiv = 0.50;
        }
    }
}
