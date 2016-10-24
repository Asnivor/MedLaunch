using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Data.Entity;
using System.Reflection;
using MahApps.Metro.Controls;

namespace MedLaunch.Models
{
    public class ConfigBaseSettings
    {
        public int ConfigId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool? isEnabled { get; set; }
        public int? systemIdent { get; set; }

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

        // generic system specific settings -   <system>.setting
        public bool? __enable { get; set; }
        public bool? __forcemono { get; set; }                      // force mono output
        public string __pixshader { get; set; }
        public int? __scanlines { get; set; }                       // -100 through 100
        public string __special { get; set; }
        public string __stretch { get; set; }
        public bool? __tblur { get; set; }
        public bool? __tblur__accum { get; set; }
        public double? __tblur__accum__amount { get; set; }         // 0 through 100
        public string __videoip { get; set; }
        public int? __xres { get; set; }                            // 0 through 65536
        public double? __xscale { get; set; }                       // 0.01 through 256
        public double? __xscalefs { get; set; }                     // 0.01 through 256
        public int? __yres { get; set; }                            // 0 through 65536
        public double? __yscale { get; set; }                       // 0.01 through 256
        public double? __yscalefs { get; set; }                     // 0.01 through 256

        // system specific settings

        // Atari Lynx
        public bool? lynx__lowpass { get; set; }
        public bool? lynx__rotateinput { get; set; }

        // Gameboy
        public string gb__system_type { get; set; }                 // auto dmg cgb agb

        // Gameboy Advance
        public string gba__bios { get; set; }

        // NeoGeo Pocket Color
        public string npg__language { get; set; }                   // english japanese

        // NES
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

        // PC-Engine (Fast)
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
        

        // PC-Engine
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

        // PC-FX
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

        // Sega GameGear

        // Sega Master System
        public bool? sms__fm { get; set; }
        public string sms__territory { get; set; }

        // Sega Megadrive
        public string md__cdbios { get; set; }
        public bool? md__correct_aspect { get; set; }
        public bool? md__input__auto { get; set; }
        public double? md__input__mouse_sensitivity { get; set; }               // 0.25 through 4
        public string md__input__multitap { get; set; }
        public string md__input__port1 { get; set; }
        public string md__input__port2 { get; set; }
        public string md__input__port3 { get; set; }
        public string md__input__port4 { get; set; }
        public string md__input__port5 { get; set; }
        public string md__input__port6 { get; set; }
        public string md__input__port7 { get; set; }
        public string md__input__port8 { get; set; }
        public string md__region { get; set; }
        public string md__reported_region { get; set; }
        public string md__debugger__disfontsize { get; set; }
        public string md__debugger__memcharenc { get; set; }


        // Sega Saturn
        public string ss__bios_jp { get; set; }
        public string ss__bios_na_eu { get; set; }
        public bool? ss__bios_sanity { get; set; }
        public string ss__cart { get; set; }
        public string ss__cart__kof95_path { get; set; }
        public string ss__cart__ultraman_path { get; set; }
        public bool? ss__cd_sanity { get; set; }
        public double? ss__input__mouse_sensitivity { get; set; }           // 0.125 through 2
        public string ss__input__port1 { get; set; }
        public string ss__input__port2 { get; set; }
        public string ss__input__port3 { get; set; }
        public string ss__input__port4 { get; set; }
        public string ss__input__port5 { get; set; }
        public string ss__input__port6 { get; set; }
        public string ss__input__port7 { get; set; }
        public string ss__input__port8 { get; set; }
        public string ss__input__port9 { get; set; }
        public string ss__input__port10 { get; set; }
        public string ss__input__port11 { get; set; }
        public string ss__input__port12 { get; set; }
        public bool? ss__midsync { get; set; }
        public bool? ss__region_autodetect { get; set; }
        public string ss__region_default { get; set; }
        public int? ss__scsp__resamp_quality { get; set; }                  // 0 through 10
        public int? ss__slend { get; set; }                                 //0 through 239
        public int? ss__slendp { get; set; }                                // -16 through 271
        public int? ss__slstart { get; set; }                                 //0 through 239
        public int? ss__slstartp { get; set; }                                // -16 through 271
        public bool? ss__smpc__autortc { get; set; }
        public string ss__smpc__autortc__lang { get; set; }
        public string ss__debugger__disfontsize { get; set; }
        public string ss__debugger__memcharenc { get; set; }


        // Sony Playstation
        public string psx__bios_eu { get; set; }
        public string psx__bios_jp { get; set; }
        public string psx__bios_na { get; set; }
        public bool? psx__bios_sanity { get; set; }
        public bool? psx__cd_sanity { get; set; }
        public int? psx__dbg_level { get; set; }                            // 0 through 4
        public bool? psx__h_overscan { get; set; }
        public bool? psx__input__analog_mode_ct { get; set; }
        public double? psx__input__mouse_sensitivity { get; set; }         // 0.25 through 4

        public string psx__input__port1 { get; set; }
        public double? psx__input__port1__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port1__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port1__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port1__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port1__memcard { get; set; }

        public string psx__input__port2 { get; set; }
        public double? psx__input__port2__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port2__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port2__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port2__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port2__memcard { get; set; }

        public string psx__input__port3 { get; set; }
        public double? psx__input__port3__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port3__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port3__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port3__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port3__memcard { get; set; }

        public string psx__input__port4 { get; set; }
        public double? psx__input__port4__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port4__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port4__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port4__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port4__memcard { get; set; }

        public string psx__input__port5 { get; set; }
        public double? psx__input__port5__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port5__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port5__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port5__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port5__memcard { get; set; }

        public string psx__input__port6 { get; set; }
        public double? psx__input__port6__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port6__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port6__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port6__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port6__memcard { get; set; }

        public string psx__input__port7 { get; set; }
        public double? psx__input__port7__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port7__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port7__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port7__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port7__memcard { get; set; }

        public string psx__input__port8 { get; set; }
        public double? psx__input__port8__analogjoy__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port8__dualanalog__axis_scale { get; set; }       // 1 through 1.5
        public double? psx__input__port8__dualshock__axis_scale { get; set; }       // 1 through 1.5
        public string psx__input__port8__gun_chairs { get; set; }                     // 0x000000 through 0x1000000
        public bool? psx__input__port8__memcard { get; set; }

        public bool? psx__input__pport1__multitap { get; set; }
        public bool? psx__input__pport2__multitap { get; set; }
        public bool? psx__region_autodetect { get; set; }
        public string psx__region_default { get; set; }
        public int? psx__slend { get; set; }                                // 0 through 239
        public int? psx__slendp { get; set; }                               // 0 through 287
        public int? psx__slstart { get; set; }                                // 0 through 239
        public int? psx__slstartp { get; set; }                               // 0 through 287
        public int? psx__spu__resamp_quality { get; set; }                      // 0 through 10

        public string psx__debugger__disfontsize { get; set; }
        public string psx__debugger__memcharenc { get; set; }

        // SNES (Faust)
        public string snes_faust__input__port1 { get; set; }
        public string snes_faust__input__port2 { get; set; }
        public int? snes_faust__resamp_quality { get; set; }                    // 0 through 5
        public double? snes_faust__resamp_rate_error { get; set; }              // 0.0000001 through 0.0015
        public bool? snes_faust__spex { get; set; }
        public bool? snes_faust__spex__sound { get; set; }

        // SNES
        public int? snes__apu__resamp_quality { get; set; }                     // 0 through 10
        public bool? snes__correct_aspect { get; set; }
        public string snes__input__port1 { get; set; }
        public bool? snes__input__port1__multitap { get; set; }
        public string snes__input__port2 { get; set; }
        public bool? snes__input__port2__multitap { get; set; }
        public double? snes__mouse_sensitivity { get; set; }                    // 0.125 through 2

        // Virtual Boy
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

        // WonderSwan
        public int? wswan__bday { get; set; }                                   // 1 through 31
        public string wswan__blood { get; set; }
        public int? wswan__bmonth { get; set; }                                 // 1 though 12
        public int? wswan__byear { get; set; }                                  // 0 through 9999
        public string wswan__language { get; set; }
        public string wswan__name { get; set; }
        public bool? wswan__rotateinput { get; set; }
        public string wswan__sex { get; set; }

        public string wswan__debugger__disfontsize { get; set; }
        public string wswan__debugger__memcharenc { get; set; }

        public static ConfigBaseSettings GetConfigDefaults()
        {
            ConfigBaseSettings cfbs = new ConfigBaseSettings
            {
                UpdatedTime = DateTime.Now,
                systemIdent = 0,
                isEnabled = true,

                autosave = false,                                   // control placed
                cd__image_memcache = false,                         // control placed
                cheats = true,                                      // control placed
                debugger__autostepmode = false,
                ffnosound = false,                                  // control placed
                ffspeed = 4,                                        // control placed
                fftoggle = false,                                   // control placed

                filesys__path_cheat = "cheats",
                filesys__path_firmware = "firmware",
                filesys__path_movie = "mcm",
                filesys__path_palette = "palettes",
                filesys__path_pgconfig = "pgconfig",
                filesys__path_sav = "sav",
                filesys__path_savbackup = "b",
                filesys__path_snap = "snaps",
                filesys__path_state = "mcs",

                filesys__state_comp_level = 6,
                filesys__untrusted_fip_check = true,
                input__autofirefreq = 3,                            // control placed
                input__ckdelay = 0,                                 // control placed
                input__joystick__axis_threshold = 75,               // control placed
                input__joystick__global_focus = true,               // control placed
                nothrottle = false,                                 // control placed
                osd__alpha_blend = true,                            // control placed
                osd__message_display_time = 2500,                   // control placed
                osd__state_display_time = 2000,                     // control placed
                qtrecord__h_double_threshold = 256,
                qtrecord__w_double_threshold = 384,
                qtrecord__vcodec = "cscd",
                sfspeed = 0.75,                                     // control placed
                sftoggle = false,                                   // control placed
                sound = true,                                       // control placed
                sound__buffer_time = 0,                             // control placed
                sound__device = "default",
                sound__driver = "default",                          // control placed
                sound__period_time = 0,                             // control placed
                sound__rate = 48000,                                // control placed
                sound__volume = 100,                                // control placed
                srwframes = 600,                                    // control placed
                video__blit_timesync = true,                        // control placed
                video__deinterlacer = "weave",                      // control placed
                video__disable_composition = true,                  // control placed
                video__driver = "opengl",                           // control placed
                video__frameskip = true,                            // control placed
                video__fs = false,                                  // control placed
                video__glvsync = true,                              // control placed

                // generic system specific settings
                __enable = true,
                __forcemono = false,                                // control placed
                __pixshader = "none",                               // control placed
                __scanlines = 0,                                    // control placed
                __special = "none",                                 // control placed
                __stretch = "aspect_mult2",                         // control placed
                __tblur = false,                                    // control placed
                __tblur__accum = false,                             // control placed
                __tblur__accum__amount = 50,                        // control placed
                __videoip = "0",                                    // control placed
                __xres = 0,                                         // control placed
                __xscale = 1,                                       // control placed
                __xscalefs = 1,                                     // control placed
                __yres = 0,                                         // control placed
                __yscale = 1,                                       // control placed
                __yscalefs = 1,                                      // control placed

                // system specific settings
                lynx__lowpass = true,                                   // placed
                lynx__rotateinput = true,                               // placed

                gb__system_type = "auto",                               // placed

                gba__bios = "",

                npg__language = "english",                              // placed

                nes__clipsides = false,                                 // placed
                nes__correct_aspect = false,                            // placed
                nes__fnscan = true,                                     // placed
                nes__gg = false,                                        // placed
                nes__ggrom = "gg.rom",
                nes__input__fcexp = "none",                             // placed
                nes__input__port1 = "gamepad",                          // placed
                nes__input__port2 = "gamepad",                          // placed
                nes__input__port3 = "gamepad",                          // placed
                nes__input__port4 = "gamepad",                          // placed
                nes__n106bs = false,                                    // placed
                nes__no8lim = false,                                    // placed
                nes__nofs = false,                                      // placed
                nes__ntsc__brightness = 0,                              // placed
                nes__ntsc__contrast = 0,                                // placed
                nes__ntsc__hue = 0,                                     // placed
                nes__ntsc__matrix = false,                              // placed
                nes__ntsc__matrix__0 = 1.539,                           // placed
                nes__ntsc__matrix__1 = -0.622,                          // placed
                nes__ntsc__matrix__2 = -0.571,                          // placed
                nes__ntsc__matrix__3 = -0.185,                          // placed
                nes__ntsc__matrix__4 = 0,                               // placed
                nes__ntsc__matrix__5 = 2,                               // placed
                nes__ntsc__mergefields = false,                         // placed
                nes__ntsc__preset = "disabled",                         // placed
                nes__ntsc__saturation = 0,                              // placed
                nes__ntsc__sharpness = 0,                               // placed
                nes__ntscblitter = false,                               // placed
                nes__pal = false,                                       // placed
                nes__slend = 231,                                       // placed
                nes__slendp = 239,                                      // placed
                nes__slstart = 8,                                       // placed
                nes__slstartp = 0,                                      // placed
                nes__sound_rate_error = 0.00004,                        // placed
                nes__soundq = 0,                                        // placed
                nes__debugger__disfontsize = "5x7",                      // placed
                nes__debugger__memcharenc = "cp437",                     // placed

                pce_fast__adpcmlp = false,                              // placed
                pce_fast__adpcmvolume = 100,                            // placed
                pce_fast__arcadecard = true,                            // placed
                pce_fast__cdbios = "syscard3.pce",
                pce_fast__cddavolume = 100,                             // placed
                pce_fast__cdpsgvolume = 100,                            // placed
                pce_fast__cdspeed = 1,                                  // placed
                pce_fast__correct_aspect = true,                        // placed
                pce_fast__disable_softreset = false,                    // placed
                pce_fast__forcesgx = false,                             // placed
                pce_fast__input__port1 = "gamepad",                     // placed
                pce_fast__input__port2 = "gamepad",                     // placed
                pce_fast__input__port3 = "gamepad",                     // placed
                pce_fast__input__port4 = "gamepad",                     // placed
                pce_fast__input__port5 = "gamepad",                     // placed
                pce_fast__mouse_sensitivity = 0.5,                      // placed
                pce_fast__nospritelimit = false,                        // placed
                pce_fast__ocmultiplier = 1,                             // placed
                pce_fast__slend = 235,                                  // placed
                pce_fast__slstart = 4,                                  // placed
                pce__debugger__disfontsize = "5x7",                      // placed
                pce__debugger__memcharenc = "shift_jis",                 // placed

                pce__adpcmextraprec = false,                            // placed
                pce__adpcmvolume = 100,                                 // placed
                pce__arcadecard = true,                                 // placed
                pce__cdbios = "syscard3.pce",
                pce__cddavolume = 100,                                  // placed
                pce__cdpsgvolume = 100,                                 // placed
                pce__disable_bram_cd = false,                           // placed
                pce__disable_bram_hucard = false,                       // placed
                pce__disable_softreset = false,                         // placed
                pce__forcesgx = false,                                  // placed
                pce__gecdbios = "gecard.pce",
                pce__h_overscan = false,                                // placed
                pce__input__multitap = true,                            // placed
                pce__input__port1 = "gamepad",                          // placed
                pce__input__port2 = "gamepad",                          // placed
                pce__input__port3 = "gamepad",                          // placed
                pce__input__port4 = "gamepad",                          // placed
                pce__input__port5 = "gamepad",                          // placed
                pce__mouse_sensitivity = 0.5,                           // placed
                pce__nospritelimit = false,                             // placed
                pce__psgrevision = "match",                             // placed
                pce__resamp_quality = 3,
                pce__resamp_rate_error = 0.0000009,
                pce__slend = 235,
                pce__slstart = 4,

                pcfx__adpcm__emulate_buggy_codec = false,                   // placed
                pcfx__adpcm__suppress_channel_reset_clicks = true,          // placed
                pcfx__bios = "pcfx.rom",
                pcfx__cdspeed = 2,                                          // placed
                pcfx__cpu_emulation = "auto",                               // placed
                pcfx__disable_bram = false,                                 // placed
                pcfx__disable_softreset = false,                            // placed
                pcfx__fxscsi = null,
                pcfx__high_dotclock_width = "1024",                         // placed
                pcfx__input__port1 = "gamepad",                             // placed
                pcfx__input__port1__multitap = false,                       // placed
                pcfx__input__port2 = "gamepad",                             // placed
                pcfx__input__port2__multitap = false,                       // placed
                pcfx__input__port3 = "gamepad",                             // placed
                pcfx__input__port4 = "gamepad",                             // placed
                pcfx__input__port5 = "gamepad",                             // placed
                pcfx__input__port6 = "gamepad",                             // placed
                pcfx__input__port7 = "gamepad",                             // placed
                pcfx__input__port8 = "gamepad",                             // placed
                pcfx__mouse_sensitivity = 1.25,                             // placed
                pcfx__nospritelimit = false,                                // placed
                pcfx__rainbow__chromaip = false,                            // placed
                pcfx__resamp_quality = 3,                                   // placed
                pcfx__resamp_rate_error = 0.0000009,                        // placed
                pcfx__slend = 235,                                          // placed
                pcfx__slstart = 4,                                          // placed
                pcfx__debugger__disfontsize = "5x7",                         // palced
                pcfx__debugger__memcharenc = "shift_jis",                    // placed

                sms__fm = true,                                             // placed
                sms__territory = "export",                                   // placed

                md__cdbios = "us_scd1_9210.bin",
                md__correct_aspect = true,                                  // placed
                md__input__auto = true,                                     // placed
                md__input__mouse_sensitivity = 1,                           // placed
                md__input__multitap = "none",                               // placed
                md__input__port1 = "gamepad",                               // placed
                md__input__port2 = "gamepad",                               // placed
                md__input__port3 = "gamepad",                               // placed
                md__input__port4 = "gamepad",                               // placed
                md__input__port5 = "gamepad",                               // placed
                md__input__port6 = "gamepad",                               // placed
                md__input__port7 = "gamepad",                               // placed
                md__input__port8 = "gamepad",                               // placed
                md__region = "game",                                        // placed                
                md__reported_region = "same",                               // placed 
                md__debugger__disfontsize = "5x7",
                md__debugger__memcharenc = "shift_jis",

                ss__bios_jp = "sega_101.bin",
                ss__bios_na_eu = "mpr-17933.bin",
                ss__bios_sanity = true,                                     // placed
                ss__cart = "auto",                                          // placed
                ss__cart__kof95_path = "mpr-18811-mx.ic1",
                ss__cart__ultraman_path = "mpr-19367-mx.ic1",
                ss__cd_sanity = true,                                       // placed
                ss__input__mouse_sensitivity = 0.5,                         // placed
                ss__input__port1 = "gamepad",                               // placed
                ss__input__port2 = "gamepad",                               // placed
                ss__input__port3 = "gamepad",                               // placed
                ss__input__port4 = "gamepad",                               // placed
                ss__input__port5 = "gamepad",                               // placed
                ss__input__port6 = "gamepad",                               // placed
                ss__input__port7 = "gamepad",                               // placed
                ss__input__port8 = "gamepad",                               // placed
                ss__input__port9 = "gamepad",                               // placed
                ss__input__port10 = "gamepad",                              // placed
                ss__input__port11 = "gamepad",                              // placed
                ss__input__port12 = "gamepad",                              // placed
                ss__midsync = false,                                        // placed
                ss__region_autodetect = true,                               // placed
                ss__region_default = "jp",                                  // placed
                ss__scsp__resamp_quality = 4,                               // placed
                ss__slend = 239,                                            // placed
                ss__slendp = 255,                                           // placed
                ss__slstart = 0,                                            // placed
                ss__slstartp = 0,                                           // placed
                ss__smpc__autortc = true,                                   // placed
                ss__smpc__autortc__lang = "english",                        // placed
                ss__debugger__disfontsize = "5x7",                           // placed
                ss__debugger__memcharenc = "SJIS",                           // placed

                psx__bios_eu = "scph5502.bin",
                psx__bios_jp = "scph5500.bin",
                psx__bios_na = "scph5501.bin",
                psx__bios_sanity = true,                                    // placed
                psx__cd_sanity = true,                                      // placed
                psx__dbg_level = 0,                                         // placed
                psx__h_overscan = true,                                     // placed
                psx__input__analog_mode_ct = false,                         // placed
                psx__input__mouse_sensitivity = 1,                          // placed
                psx__input__port1 = "gamepad",
                psx__input__port1__analogjoy__axis_scale = 1,               // placed
                psx__input__port1__dualanalog__axis_scale = 1,          // placed
                psx__input__port1__dualshock__axis_scale = 1,   // placed
                psx__input__port1__gun_chairs = "0xFF0000",     // placed
                psx__input__port1__memcard = true,              // placed
                psx__input__port2 = "gamepad",  // placed
                psx__input__port2__analogjoy__axis_scale = 1,   // placed
                psx__input__port2__dualanalog__axis_scale = 1,  // placed
                psx__input__port2__dualshock__axis_scale = 1,   // placed
                psx__input__port2__gun_chairs = "0xFF0000", // placed
                psx__input__port2__memcard = true,  // placed
                psx__input__port3 = "gamepad",  // placed
                psx__input__port3__analogjoy__axis_scale = 1,   // placed
                psx__input__port3__dualanalog__axis_scale = 1,  // placed
                psx__input__port3__dualshock__axis_scale = 1,   // placed
                psx__input__port3__gun_chairs = "0xFF0000", // placed
                psx__input__port3__memcard = true,  // placed
                psx__input__port4 = "gamepad",  // placed
                psx__input__port4__analogjoy__axis_scale = 1,   // placed
                psx__input__port4__dualanalog__axis_scale = 1,  // placed
                psx__input__port4__dualshock__axis_scale = 1,// placed
                psx__input__port4__gun_chairs = "0xFF0000", // placed
                psx__input__port4__memcard = true,  // placed
                psx__input__port5 = "gamepad",  // placed
                psx__input__port5__analogjoy__axis_scale = 1,   // placed
                psx__input__port5__dualanalog__axis_scale = 1,  // placed
                psx__input__port5__dualshock__axis_scale = 1,   // placed
                psx__input__port5__gun_chairs = "0xFF0000", // placed
                psx__input__port5__memcard = true,  // placed
                psx__input__port6 = "gamepad",  // placed
                psx__input__port6__analogjoy__axis_scale = 1,   // placed
                psx__input__port6__dualanalog__axis_scale = 1,  // placed
                psx__input__port6__dualshock__axis_scale = 1,   // placed
                psx__input__port6__gun_chairs = "0xFF0000", // placed
                psx__input__port6__memcard = true,  // placed
                psx__input__port7 = "gamepad",  // placed
                psx__input__port7__analogjoy__axis_scale = 1,   // placed
                psx__input__port7__dualanalog__axis_scale = 1,  // placed
                psx__input__port7__dualshock__axis_scale = 1,   // placed
                psx__input__port7__gun_chairs = "0xFF0000", // placed
                psx__input__port7__memcard = true,  // placed
                psx__input__port8 = "gamepad",  // placed
                psx__input__port8__analogjoy__axis_scale = 1,   // placed
                psx__input__port8__dualanalog__axis_scale = 1,  // placed
                psx__input__port8__dualshock__axis_scale = 1,   // placed
                psx__input__port8__gun_chairs = "0xFF0000",         // placed
                psx__input__port8__memcard = true,                  // placed
                psx__input__pport1__multitap = false,               // placed
                psx__input__pport2__multitap = false,               // placed
                psx__region_autodetect = true,                      // placed
                psx__region_default = "jp",                         // placed
                psx__slend = 239,                                   // placed
                psx__slendp = 287,                                  // placed
                psx__slstart = 0,                                   // placed
                psx__slstartp = 0,                                  // placed
                psx__spu__resamp_quality = 5,                       // placed
                psx__debugger__disfontsize = "5x7",                  // placed
                psx__debugger__memcharenc = "shift_jis",             // placed


                snes_faust__input__port1 = "gamepad",               // placed
                snes_faust__input__port2 = "gamepad",               // placed
                snes_faust__resamp_quality = 3,                     // placed
                snes_faust__resamp_rate_error = 0.000035,           // placed
                snes_faust__spex = false,                           // placed
                snes_faust__spex__sound = true,                     // placed

                snes__apu__resamp_quality = 5,                      // placed
                snes__correct_aspect = false,                       // placed
                snes__input__port1 = "gamepad",                     // placed
                snes__input__port1__multitap = false,               // placed
                snes__input__port2 = "gamepad",                     // placed
                snes__input__port2__multitap = false,               // placed
                snes__mouse_sensitivity = 0.5,                      // placed

                vb__3dmode = "anaglyph",                            // placed
                vb__3dreverse = false,                              // placed
                vb__allow_draw_skip = false,                        // placed
                vb__anaglyph__lcolor = "0xffba00",                  // placed
                vb__anaglyph__preset = "red_blue",                   // placed
                vb__anaglyph__rcolor = "0x00baff",                  // placed
                vb__cpu_emulation = "fast",                         // placed
                vb__default_color = "0xF0F0F0",                     // placed
                vb__disable_parallax = false,                       // placed
                vb__input__instant_read_hack = true,                // placed
                vb__instant_display_hack = false,                   // placed
                vb__liprescale = 2,                                 // placed
                vb__sidebyside__separation = 0,                     // placed
                vb__debugger__disfontsize = "5x7",                   // placed
                vb__debugger__memcharenc = "shift_jis",              // placed

                wswan__bday = 23,                                   // placed
                wswan__blood = "o",                                 // placed
                wswan__bmonth = 6,                                  // placed
                wswan__byear = 1989,                                // placed
                wswan__language = "english",
                wswan__name = "Mednafen",                           // placed
                wswan__rotateinput = false,                         // placed
                wswan__sex = "female",                              // placed

                wswan__debugger__disfontsize = "5x7",
                wswan__debugger__memcharenc = "shift_jis",
                

            };

            return cfbs;
        }

        public static void SaveToDatabase(List<ConfigBaseSettings> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.ConfigBaseSettings.AsNoTracking().ToList();

                List<ConfigBaseSettings> toAdd = new List<ConfigBaseSettings>();
                List<ConfigBaseSettings> toUpdate = new List<ConfigBaseSettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    ConfigBaseSettings t = (from a in current
                                     where a.ConfigId == g.ConfigId
                                     select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.ConfigBaseSettings.UpdateRange(toUpdate);
                db.ConfigBaseSettings.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        public static void ResetToDefault(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);                
            }

            int configId = baseId + sysId;

            // get defaults
            ConfigBaseSettings defaults = ConfigBaseSettings.GetConfigDefaults();
            defaults.ConfigId = configId;
            defaults.systemIdent = sysId;
            defaults.UpdatedTime = DateTime.Now;

            // Get current config
            ConfigBaseSettings config = ConfigBaseSettings.GetConfig(configId);
            var conf = new ConfigBaseSettings();
            conf = config;


            if (config.isEnabled == true)
            {
                defaults.isEnabled = true;
            }
            else
            {
                defaults.isEnabled = false;
            }


            // set defaults for this config
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(defaults).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
            }
        }

        public static int GetConfigIdFromButtonName(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);
            }
            //MessageBoxResult result = MessageBox.Show(code);

            int configId = baseId + sysId;
            return configId;
        }

        public static string GetConfigFileNameFromConfigId(int configId)
        {
            return "";
        }

        public static void EnableConfigToggle(string btnName)
        {
            string rbName = btnName.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            int baseId = 2000000000;
            int sysId = 0;
            if (code == "base")
            {
                // do nothing
            }
            else
            {
                sysId = GetSystemIdFromSystemCode(code);
            }

            int configId = baseId + sysId;

            // Get current config
            ConfigBaseSettings config = ConfigBaseSettings.GetConfig(configId);
            var conf = new ConfigBaseSettings();
            conf = config;


            if (config.isEnabled == true)
            {
                conf.isEnabled = false;
            }
            else
            {
                conf.isEnabled = true;
            }
            // set defaults for this config
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(config).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
                cfDef.Dispose();
            }
            //SetConfig(config);


        }

        public static ConfigBaseSettings GetConfig(int ConfigId)
        {
            using (var context = new MyDbContext())
            {
                var cData = context.ConfigBaseSettings.AsNoTracking().ToList();
                ConfigBaseSettings c = (from a in cData
                                        where a.ConfigId == ConfigId
                                        select a).ToList().SingleOrDefault();
                
                context.Dispose();
                return c;
            }
        }

        public static void SetConfig(ConfigBaseSettings Config)
        {
            using (var cfDef = new MyDbContext())
            {

                cfDef.Entry(Config).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
            }
        }

        public static bool IsConfigEnabled(int systemId)
        {
            int baseId = 2000000000;
            int id = baseId + systemId;

            using (var context = new MyDbContext())
            {
                var c = (from a in context.ConfigBaseSettings
                        where a.ConfigId == id
                        select a).SingleOrDefault();

                if (c.isEnabled == true && c.isEnabled != null)
                    return true;
                else
                    return false;                
            }
        }
        
        public static void SetButtonState(RadioButton rb)
        {
            string rbName = rb.Name.ToLower();
            // get system code from name
            string code = rbName.Replace("btnconfig", "").Trim();
            //MessageBoxResult result = MessageBox.Show(code);

            if (code == "base")
            {
                rb.IsEnabled = true;
            }
            else
            {
                int systemId = GetSystemIdFromSystemCode(code);

                if (IsConfigEnabled(systemId) == true)
                {
                    rb.IsEnabled = true;
                }
                else
                {
                    rb.IsEnabled = false;
                }
            }
            
        }

        public static int GetSystemIdFromSystemCode(string systemCode)
        {
            GSystem gs = new GSystem(systemCode);
            return gs.systemId;
        }

        public static string ConvertControlNameToConfigName(string controlName)
        {
            return controlName.Replace("cfg_", "");
        }

        // populate settings - bios path controls
        public static void LoadBiosPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            ConfigBaseSettings settings = GetConfig(2000000000);

            SetControlValues(ui, settings, 1);
        }

        // save settings - mednafen paths controls
        public static void SaveBiosPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            //ConfigBaseSettings settings = GetConfig(2000000000);

            // get ALL config settings (as we are saving these to all configs)
            List<ConfigBaseSettings> AllSettings = new List<ConfigBaseSettings>();
            using (var context = new MyDbContext())
            {
                List<ConfigBaseSettings> aset = (from d in context.ConfigBaseSettings
                                                 select d).ToList();
                AllSettings.AddRange(aset);
            }

            // iterate through each config and set all the settings for each config
            foreach (ConfigBaseSettings settings in AllSettings)
            {
                SetControlValues(ui, settings, 2);
            }
        }

        // populate settings - mednafen paths controls
        public static void LoadMednafenPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            ConfigBaseSettings settings = GetConfig(2000000000);

            SetControlValues(ui, settings, 1);
        }

        // save settings - mednafen paths controls
        public static void SaveMednafenPathValues(StackPanel wp)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for base config
            //ConfigBaseSettings settings = GetConfig(2000000000);

            // get ALL config settings (as we are saving these to all configs)
            List<ConfigBaseSettings> AllSettings = new List<ConfigBaseSettings>();
            using (var context = new MyDbContext())
            {
                List<ConfigBaseSettings> aset = (from d in context.ConfigBaseSettings
                                                        select d).ToList();
                AllSettings.AddRange(aset);
            }

            // iterate through each config and set all the settings for each config
            foreach (ConfigBaseSettings settings in AllSettings)
            {
                SetControlValues(ui, settings, 2);
            }       
        }

        public static void SetControlValues(UIHandler ui, ConfigBaseSettings settings, int LoadOrSave)
        {
            if (LoadOrSave == 1)
            {
                // 1 = load settings
                // Iterate through all controls and set correct values
                // Buttons
                foreach (Button control in ui.Buttons)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // check that the control is actually a config control
                        if (propName.Contains("_"))
                        {
                            // get the property value using reflection
                            var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                            string v = value.ToString();

                            // update wpf control
                            control.Content = v;
                        }
                        else
                        {
                            
                        }

                        
                    }
                }

                // RadioButtons

                // Labels

                // CheckBoxes
                foreach (CheckBox control in ui.CheckBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        bool v = Convert.ToBoolean(value.ToString());

                        // update wpf control
                        control.IsChecked = v;
                    }
                }

                // TextBoxes
                foreach (TextBox control in ui.TextBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // textbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        string v = value.ToString();

                        // update wpf control
                        control.Text = v;
                    }
                }

                // Sliders
                foreach (Slider control in ui.Sliders)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        double v = Convert.ToDouble(value.ToString());

                        // update wpf control
                        control.Value = v;
                    }
                }

                // Comboxes
                foreach (ComboBox control in ui.ComboBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        string v = value.ToString();

                        // update wpf control
                        control.SelectedValue = v;
                    }
                }

                // NumericUpDowns
                foreach (NumericUpDown control in ui.NumericUpDowns)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the property value using reflection
                        var value = settings.GetType().GetProperty(propName).GetValue(settings, null);
                        //bool v = Convert.ToBoolean(value.ToString());
                        double v = Convert.ToDouble(value.ToString());

                        // update wpf control
                        control.Value = v;
                    }
                }
            }
            if (LoadOrSave == 2)
            {
                // 2 = save settings
                // Iterate through all controls and set correct values
                // Buttons
                foreach (Button control in ui.Buttons)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // check that the control is actually a config control
                        if (propName.Contains("_"))
                        {
                            // get the control value
                            string v = control.Content.ToString();
                            // update settings object with value
                            PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                            propInfo.SetValue(settings, v, null);
                        }
                    }
                }

                // RadioButtons

                // Labels

                // CheckBoxes
                foreach (CheckBox control in ui.CheckBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the control value
                        bool? v = control.IsChecked.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        propInfo.SetValue(settings, v, null);
                    }
                }

                // TextBoxes
                foreach (TextBox control in ui.TextBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // textbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");                    
                    }
                    else
                    {
                        // get the control value
                        string v = control.Text;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        propInfo.SetValue(settings, v, null);
                    }
                }

                // Sliders
                foreach (Slider control in ui.Sliders)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        double? v = control.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                        {
                            // double is required
                            propInfo.SetValue(settings, v, null);
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                        }
                        if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            // int32 is required
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            propInfo.SetValue(settings, Convert.ToInt32(v), null);
                        }
                    }
                }

                // Comboxes
                foreach (ComboBox control in ui.ComboBoxes)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        //string v = control.SelectedValue.ToString();
                        string v = ((ComboBoxItem)control.SelectedItem).Tag.ToString();
                        //MessageBoxResult aresult = MessageBox.Show(propName + "   -   " + v);

                        // update settings object with string value - 


                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);
                        //MessageBoxResult aresult = MessageBox.Show(propName + "   -   " + propInfo.ToString() + "   -   " + v);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]") || propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                            {
                                // double is required
                                double? d = Convert.ToDouble(v);
                                propInfo.SetValue(settings, d, null);
                                //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            }
                            if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                            {
                                // int32 is required
                                int? i = Convert.ToInt32(v);
                                //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                                propInfo.SetValue(settings, i, null);
                            }
                        }
                        else
                        {
                            propInfo.SetValue(settings, v, null);
                        }



                    }
                }

                // NumericUpDowns
                foreach (NumericUpDown control in ui.NumericUpDowns)
                {
                    // get config entry name from control name
                    string propName = ConvertControlNameToConfigName(control.Name);
                    //MessageBoxResult result = MessageBox.Show(propName);
                    // make sure name is not null
                    if (control.Name == null || control.Name.Trim() == "")
                    {
                        // checkbox does not have a name set - skip
                        //MessageBoxResult aresult = MessageBox.Show(propName + " IS EMPTY!");
                    }
                    else
                    {
                        // get the control value
                        double? v = control.Value;
                        // update settings object with value
                        PropertyInfo propInfo = settings.GetType().GetProperty(propName);

                        if (propInfo.PropertyType.ToString().Contains("[System.Double]"))
                        {
                            // double is required
                            propInfo.SetValue(settings, v, null);
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                        }
                        if (propInfo.PropertyType.ToString().Contains("[System.Int32]"))
                        {
                            // int32 is required
                            //MessageBoxResult aresult = MessageBox.Show(propInfo.PropertyType.ToString());
                            propInfo.SetValue(settings, Convert.ToInt32(v), null);
                        }
                    }
                }

                // save config
                SetConfig(settings);
            }
            
        }

        

        // Populate config settings for specific System ID  -   WrapPanel as parent     
        public static void LoadControlValues(WrapPanel wp, int configId)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for system
            ConfigBaseSettings settings = GetConfig(configId);

            SetControlValues(ui, settings, 1);           

        }

        // Save config settings for specific System ID  -   WrapPanel as parent     
        public static void SaveControlValues(WrapPanel wp, int configId)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            // get all config settings for system
            ConfigBaseSettings settings = GetConfig(configId);

            SetControlValues(ui, settings, 2);

            

        }

        public static T ChangeType<T>(object value)
        {
            Type conversionType = typeof(T);
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) { return default(T); }

                conversionType = Nullable.GetUnderlyingType(conversionType); ;
            }

            return (T)Convert.ChangeType(value, conversionType);
        }


    }
}
