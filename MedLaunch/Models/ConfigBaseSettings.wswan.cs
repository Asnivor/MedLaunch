using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    /// <summary>
    /// Bandai Wonderswan
    /// </summary>
    public partial class ConfigBaseSettings
    {
        public int? wswan__bday { get; set; }                                   // 1 through 31
        public string wswan__blood { get; set; }
        public int? wswan__bmonth { get; set; }                                 // 1 though 12
        public int? wswan__byear { get; set; }                                  // 0 through 9999
        public string wswan__input__builtin { get; set; }
        public string wswan__language { get; set; }
        public string wswan__name { get; set; }
        //public bool? wswan__rotateinput { get; set; }
        public string wswan__sex { get; set; }

        public bool? wswan__enable { get; set; }
        public bool? wswan__forcemono { get; set; }
        public string wswan__shader { get; set; }
        public int? wswan__scanlines { get; set; }
        public string wswan__special { get; set; }
        public string wswan__stretch { get; set; }
        public bool? wswan__tblur { get; set; }
        public bool? wswan__tblur__accum { get; set; }
        public double? wswan__tblur__accum__amount { get; set; }
        public string wswan__videoip { get; set; }
        public int? wswan__xres { get; set; }                            // 0 through 65536
        public double? wswan__xscale { get; set; }                       // 0.01 through 256
        public double? wswan__xscalefs { get; set; }                     // 0.01 through 256
        public int? wswan__yres { get; set; }                            // 0 through 65536
        public double? wswan__yscale { get; set; }                       // 0.01 through 256
        public double? wswan__yscalefs { get; set; }                     // 0.01 through 256

        public string wswan__debugger__disfontsize { get; set; }
        public string wswan__debugger__memcharenc { get; set; }

        public bool? wswan__shader__goat__fprog { get; set; }
        public double? wswan__shader__goat__hdiv { get; set; }           // -2.00 through 2.00
        public string wswan__shader__goat__pat { get; set; }
        public bool? wswan__shader__goat__slen { get; set; }
        public double? wswan__shader__goat__tp { get; set; }              // 0.00 through 1.00
        public double? wswan__shader__goat__vdiv { get; set; }           // -2.00 through 2.00

        


        public static void GetDefaults_wswan(ConfigBaseSettings c)
        {
            c.wswan__bday = 23;                                   // placed
            c.wswan__blood = "o";                                 // placed
            c.wswan__bmonth = 6;                                  // placed
            c.wswan__byear = 1989;                                // placed
            c.wswan__language = "english";
            c.wswan__name = "Mednafen";                           // placed
            //c.wswan__rotateinput = false;                         // placed
            c.wswan__sex = "female";                              // placed

            c.wswan__debugger__disfontsize = "5x7";
            c.wswan__debugger__memcharenc = "shift_jis";

            c.wswan__enable = true;
            c.wswan__forcemono = false;                                // control placed
            c.wswan__shader = "none";                               // control placed
            c.wswan__scanlines = 0;                                    // control placed
            c.wswan__special = "none";                                 // control placed
            c.wswan__stretch = "aspect_mult2";                         // control placed
            c.wswan__tblur = false;                                    // control placed
            c.wswan__tblur__accum = false;                             // control placed
            c.wswan__tblur__accum__amount = 50;                        // control placed
            c.wswan__videoip = "0";                                    // control placed
            c.wswan__xres = 0;                                         // control placed
            c.wswan__xscale = 4;                                       // control placed
            c.wswan__xscalefs = 1;                                     // control placed
            c.wswan__yres = 0;                                         // control placed
            c.wswan__yscale = 4;                                       // control placed
            c.wswan__yscalefs = 1;                                     // control placed

            c.wswan__shader__goat__fprog = false;
            c.wswan__shader__goat__hdiv = 0.50;
            c.wswan__shader__goat__pat = "goatron";
            c.wswan__shader__goat__slen = true;
            c.wswan__shader__goat__tp = 0.50;
            c.wswan__shader__goat__vdiv = 0.50;

            c.wswan__input__builtin = "gamepad";
        }
    }
}
