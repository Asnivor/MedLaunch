using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Enums;
using MedLaunch.Classes;

namespace MedLaunch.Models
{
    public class ConfigBaseSettings
    {
        public int ConfigId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool? autosave { get; set; }                       // autosave
        public bool? cd__image_memcache { get; set; }             // cd.image_memcache
        public bool? cheats { get; set; }                         // cheats
        public bool? debugger__autostepmode { get; set; }         // debugger.autostepmode
        public bool? ffnosound { get; set; }                      // ffnosound
        public double? ffspeed { get; set; }                            // ffspeed                          (1 through 15)
        public bool? fftoggle { get; set; }                       // fftoggle
        public string filesys__path_cheat { get; set; }             // filesys.path_cheat
        public string filesys__path_firmware { get; set; }          // filesys.path_firmware
        public string filesys__path_movie { get; set; }             // filesys.path_movie
        public string filesys__path_palette { get; set; }           // filesys.path_palette
        public string filesys__path_pgconfig { get; set; }          // filesys.path_pgconfig
        public string filesys__path_sav { get; set; }               // filesys.path_sav
        public string filesys__path_savbackup { get; set; }         // filesys.path_savbackup
        public string filesys__path_snap { get; set; }              // filesys.path_snap
        public string filesys__path_state { get; set; }             // filesys.path_state
        public int? filesys__state_comp_level { get; set; }          // filesys.state_comp_level         (-1 through 9)
        public bool? filesys__untrusted_fip_check { get; set; }   // filesys.untrusted_fip_check
        public int? input__autofirefreq { get; set; }                // input.autofirefreq               (0 through 1000)
        public int? input__ckdelay { get; set; }                     // 0 through 99999
        public decimal? input__joystick__axis_threshold { get; set; }    // 0 through 100
        public bool? input__joystick__global_focus { get; set; }
        public bool? nothrottle { get; set; }
        public bool? osd__alpha_blend { get; set; }
        public int? osd__message_display_time { get; set; }          // 0 through 15000
        public int? osd__state_display_time { get; set; }            // 0 through 15000
        public int? qtrecord__h_double_threshold { get; set; }       // 0 through 1073741824
        public string qtrecord__vcodec { get; set; }                // raw cscd png
        public int? qtrecord__w_double_threshold { get; set; }       // 0 through 1073741824
        public double? sfspeed { get; set; }                        // 0.25 through 1
        public bool? sftoggle { get; set; }
        public bool? sound { get; set; }
        public int? sound__buffer_time { get; set; }                 // 0 through 1000
        public string sound__device { get; set; }                   // default
        public string sound__driver { get; set; }                   // default alsa oss wasapish dsound wasapi sdl jack
        public int? sound__period_time { get; set; }                 // 0 through 100000
        public int? sound__rate { get; set; }                        // 22050 through 192000
        public int? sound__volume { get; set; }                      // 0 through 150
        public int? srwframes { get; set; }                          // 10 through 99999
        public bool? video__blit_timesync { get; set; }
        public string video__deinterlacer { get; set; }             // weave bob bob_offset
        public bool? video__disable_composition { get; set; }
        public string video__driver { get; set; }                    // opengl sdl overlay
        public bool? video__frameskip { get; set; }
        public bool? video__fs { get; set; }
        public bool? video__glvsync { get; set; }
    }
}
