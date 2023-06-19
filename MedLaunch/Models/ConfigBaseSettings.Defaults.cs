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
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace MedLaunch.Models
{
    public partial class ConfigBaseSettings
    {
        public static ConfigBaseSettings GetConfigDefaults()
        {
            ConfigBaseSettings cfbs = new ConfigBaseSettings
            {
                UpdatedTime = DateTime.Now,
                systemIdent = 0,
                isEnabled = true,
                affinity__cd = "0x0000000000000000",                // control placed
                affinity__emu = "0x0000000000000000",               // control placed
                affinity__video = "0x0000000000000000",             // control placed
                autosave = false,                                   // control placed
                cd__image_memcache = false,                         // control placed
                cd__m3u__disc_limit = 25,
                cd__m3u__recursion_limit = 9,
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
                input__joystick__axis_threshold = 75.00,               // control placed
                input__joystick__global_focus = true,               // control placed
                nothrottle = false,                                 // control placed
                osd__alpha_blend = true,                            // control placed
                osd__message_display_time = 2500,                   // control placed
                osd__state_display_time = 2000,                     // control placed

                // new FPS options
                fps__autoenable = false,
                fps__bgcolor = "0x80000000",
                fps__font = "5x7",
                fps__position = "upper_left",
                fps__scale = 1,
                fps__textcolor = "0xFFFFFFFF",


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
                video__cursorvis = "hidden",
                video__deinterlacer = "weave",                      // control placed
                video__disable_composition = true,                  // control placed
                video__driver = "default",                           // control placed
                video__force_bbclear = false,                           // control placed
                video__frameskip = true,                            // control placed
                video__fs = false,                                  // control placed
                video__glvsync = true,                              // control placed
                video__glformat = "auto",

                video__fs__display = -1,                          // -1 through 32767

                // generic system specific settings
                __enable = true,
                __forcemono = false,                                // control placed
                __shader = "none",                               // control placed
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

                // lynx
                lynx__lowpass = true,                                   // placed
                lynx__rotateinput = true,                               // placed

                lynx__enable = true,
                lynx__forcemono = false,                                // control placed
                lynx__shader = "none",                               // control placed
                lynx__scanlines = 0,                                    // control placed
                lynx__special = "none",                                 // control placed
                lynx__stretch = "aspect_mult2",                         // control placed
                lynx__tblur = false,                                    // control placed
                lynx__tblur__accum = false,                             // control placed
                lynx__tblur__accum__amount = 50,                        // control placed
                lynx__videoip = "0",                                    // control placed
                lynx__xres = 0,                                         // control placed
                lynx__xscale = 6,                                       // control placed
                lynx__xscalefs = 1,                                     // control placed
                lynx__yres = 0,                                         // control placed
                lynx__yscale = 6,                                       // control placed
                lynx__yscalefs = 1,                                     // control placed

                lynx__shader__goat__fprog = false,
                lynx__shader__goat__hdiv = 0.50,
                lynx__shader__goat__pat = "goatron",
                lynx__shader__goat__slen = true,
                lynx__shader__goat__tp = 0.50,
                lynx__shader__goat__vdiv = 0.50,

                // gameboy
                gb__system_type = "auto",                               // placed

                gb__enable = true,
                gb__forcemono = false,                                // control placed
                gb__shader = "none",                               // control placed
                gb__scanlines = 0,                                    // control placed
                gb__special = "none",                                 // control placed
                gb__stretch = "aspect_mult2",                         // control placed
                gb__tblur = false,                                    // control placed
                gb__tblur__accum = false,                             // control placed
                gb__tblur__accum__amount = 50,                        // control placed
                gb__videoip = "0",                                    // control placed
                gb__xres = 0,                                         // control placed
                gb__xscale = 6,                                       // control placed
                gb__xscalefs = 1,                                     // control placed
                gb__yres = 0,                                         // control placed
                gb__yscale = 6,                                       // control placed
                gb__yscalefs = 1,                                     // control placed

                gb__shader__goat__fprog = false,
                gb__shader__goat__hdiv = 0.50,
                gb__shader__goat__pat = "goatron",
                gb__shader__goat__slen = true,
                gb__shader__goat__tp = 0.50,
                gb__shader__goat__vdiv = 0.50,

                // gameboy advance
                gba__bios = "",

                gba__enable = true,
                gba__forcemono = false,                                // control placed
                gba__shader = "none",                               // control placed
                gba__scanlines = 0,                                    // control placed
                gba__special = "none",                                 // control placed
                gba__stretch = "aspect_mult2",                         // control placed
                gba__tblur = false,                                    // control placed
                gba__tblur__accum = false,                             // control placed
                gba__tblur__accum__amount = 50,                        // control placed
                gba__videoip = "0",                                    // control placed
                gba__xres = 0,                                         // control placed
                gba__xscale = 4,                                       // control placed
                gba__xscalefs = 1,                                     // control placed
                gba__yres = 0,                                         // control placed
                gba__yscale = 4,                                       // control placed
                gba__yscalefs = 1,                                     // control placed

                gba__shader__goat__fprog = false,
                gba__shader__goat__hdiv = 0.50,
                gba__shader__goat__pat = "goatron",
                gba__shader__goat__slen = true,
                gba__shader__goat__tp = 0.50,
                gba__shader__goat__vdiv = 0.50,

                // neogeo pocket
                npg__language = "english",                              // placed

                ngp__enable = true,
                ngp__forcemono = false,                                // control placed
                ngp__shader = "none",                               // control placed
                ngp__scanlines = 0,                                    // control placed
                ngp__special = "none",                                 // control placed
                ngp__stretch = "aspect_mult2",                         // control placed
                ngp__tblur = false,                                    // control placed
                ngp__tblur__accum = false,                             // control placed
                ngp__tblur__accum__amount = 50,                        // control placed
                ngp__videoip = "0",                                    // control placed
                ngp__xres = 0,                                         // control placed
                ngp__xscale = 6,                                       // control placed
                ngp__xscalefs = 1,                                     // control placed
                ngp__yres = 0,                                         // control placed
                ngp__yscale = 6,                                       // control placed
                ngp__yscalefs = 1,                                     // control placed

                ngp__shader__goat__fprog = false,
                ngp__shader__goat__hdiv = 0.50,
                ngp__shader__goat__pat = "goatron",
                ngp__shader__goat__slen = true,
                ngp__shader__goat__tp = 0.50,
                ngp__shader__goat__vdiv = 0.50,

                // NES
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

                nes__enable = true,
                nes__shader = "none",                               // control placed
                nes__scanlines = 0,                                    // control placed
                nes__special = "none",                                 // control placed
                nes__stretch = "aspect_mult2",                         // control placed
                nes__tblur = false,                                    // control placed
                nes__tblur__accum = false,                             // control placed
                nes__tblur__accum__amount = 50,                        // control placed
                nes__videoip = "0",                                    // control placed
                nes__xres = 0,                                         // control placed
                nes__xscale = 4,                                       // control placed
                nes__xscalefs = 1,                                     // control placed
                nes__yres = 0,                                         // control placed
                nes__yscale = 4,                                       // control placed
                nes__yscalefs = 1,                                     // control placed

                nes__shader__goat__fprog = false,
                nes__shader__goat__hdiv = 0.50,
                nes__shader__goat__pat = "goatron",
                nes__shader__goat__slen = true,
                nes__shader__goat__tp = 0.50,
                nes__shader__goat__vdiv = 0.50,

                // PCE fast
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

                pce_fast__enable = true,
                pce_fast__forcemono = false,                                // control placed
                pce_fast__shader = "none",                               // control placed
                pce_fast__scanlines = 0,                                    // control placed
                pce_fast__special = "none",                                 // control placed
                pce_fast__stretch = "aspect_mult2",                         // control placed
                pce_fast__tblur = false,                                    // control placed
                pce_fast__tblur__accum = false,                             // control placed
                pce_fast__tblur__accum__amount = 50,                        // control placed
                pce_fast__videoip = "1",                                    // control placed
                pce_fast__xres = 0,                                         // control placed
                pce_fast__xscale = 3,                                       // control placed
                pce_fast__xscalefs = 1,                                     // control placed
                pce_fast__yres = 0,                                         // control placed
                pce_fast__yscale = 3,                                       // control placed
                pce_fast__yscalefs = 1,                                     // control placed

                pce_fast__shader__goat__fprog = false,
                pce_fast__shader__goat__hdiv = 0.50,
                pce_fast__shader__goat__pat = "goatron",
                pce_fast__shader__goat__slen = true,
                pce_fast__shader__goat__tp = 0.50,
                pce_fast__shader__goat__vdiv = 0.50,

                // PCE
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

                pce__shader__goat__fprog = false,
                pce__shader__goat__hdiv = 0.50,
                pce__shader__goat__pat = "goatron",
                pce__shader__goat__slen = true,
                pce__shader__goat__tp = 0.50,
                pce__shader__goat__vdiv = 0.50,

                pce__enable = true,
                pce__forcemono = false,                                // control placed
                pce__shader = "none",                               // control placed
                pce__scanlines = 0,                                    // control placed
                pce__special = "none",                                 // control placed
                pce__stretch = "aspect_mult2",                         // control placed
                pce__tblur = false,                                    // control placed
                pce__tblur__accum = false,                             // control placed
                pce__tblur__accum__amount = 50,                        // control placed
                pce__videoip = "1",                                    // control placed
                pce__xres = 0,                                         // control placed
                pce__xscale = 3,                                       // control placed
                pce__xscalefs = 1,                                     // control placed
                pce__yres = 0,                                         // control placed
                pce__yscale = 3,                                       // control placed
                pce__yscalefs = 1,                                     // control placed

                // PC-FX
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

                pcfx__enable = true,
                pcfx__forcemono = false,                                // control placed
                pcfx__shader = "none",                               // control placed
                pcfx__scanlines = 0,                                    // control placed
                pcfx__special = "none",                                 // control placed
                pcfx__stretch = "aspect_mult2",                         // control placed
                pcfx__tblur = false,                                    // control placed
                pcfx__tblur__accum = false,                             // control placed
                pcfx__tblur__accum__amount = 50,                        // control placed
                pcfx__videoip = "1",                                    // control placed
                pcfx__xres = 0,                                         // control placed
                pcfx__xscale = 3,                                       // control placed
                pcfx__xscalefs = 1,                                     // control placed
                pcfx__yres = 0,                                         // control placed
                pcfx__yscale = 3,                                       // control placed
                pcfx__yscalefs = 1,                                     // control placed

                pcfx__shader__goat__fprog = false,
                pcfx__shader__goat__hdiv = 0.50,
                pcfx__shader__goat__pat = "goatron",
                pcfx__shader__goat__slen = true,
                pcfx__shader__goat__tp = 0.50,
                pcfx__shader__goat__vdiv = 0.50,

                // Game Gear

                gg__enable = true,
                gg__forcemono = false,                                // control placed
                gg__shader = "none",                               // control placed
                gg__scanlines = 0,                                    // control placed
                gg__special = "none",                                 // control placed
                gg__stretch = "aspect_mult2",                         // control placed
                gg__tblur = false,                                    // control placed
                gg__tblur__accum = false,                             // control placed
                gg__tblur__accum__amount = 50,                        // control placed
                gg__videoip = "0",                                    // control placed
                gg__xres = 0,                                         // control placed
                gg__xscale = 6,                                       // control placed
                gg__xscalefs = 1,                                     // control placed
                gg__yres = 0,                                         // control placed
                gg__yscale = 6,                                       // control placed
                gg__yscalefs = 1,                                     // control placed

                gg__shader__goat__fprog = false,
                gg__shader__goat__hdiv = 0.50,
                gg__shader__goat__pat = "goatron",
                gg__shader__goat__slen = true,
                gg__shader__goat__tp = 0.50,
                gg__shader__goat__vdiv = 0.50,

                // Mster system
                sms__fm = true,                                             // placed
                sms__territory = "export",                                   // placed

                sms__enable = true,
                sms__forcemono = false,                                // control placed
                sms__shader = "none",                               // control placed
                sms__scanlines = 0,                                    // control placed
                sms__special = "none",                                 // control placed
                sms__stretch = "aspect_mult2",                         // control placed
                sms__tblur = false,                                    // control placed
                sms__tblur__accum = false,                             // control placed
                sms__tblur__accum__amount = 50,                        // control placed
                sms__videoip = "0",                                    // control placed
                sms__xres = 0,                                         // control placed
                sms__xscale = 4,                                       // control placed
                sms__xscalefs = 1,                                     // control placed
                sms__yres = 0,                                         // control placed
                sms__yscale = 4,                                       // control placed
                sms__yscalefs = 1,                                     // control placed

                sms__slend = 239,                                        // control placed
                sms__slendp = 239,                                       // control placed
                sms__slstartp = 0,                                       // control placed
                sms__slstart = 0,                                       // control placed

                sms__shader__goat__fprog = false,
                sms__shader__goat__hdiv = 0.50,
                sms__shader__goat__pat = "goatron",
                sms__shader__goat__slen = true,
                sms__shader__goat__tp = 0.50,
                sms__shader__goat__vdiv = 0.50,

                // Mega Drive
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

                md__enable = true,
                md__forcemono = false,                                // control placed
                md__shader = "none",                               // control placed
                md__scanlines = 0,                                    // control placed
                md__special = "none",                                 // control placed
                md__stretch = "aspect_mult2",                         // control placed
                md__tblur = false,                                    // control placed
                md__tblur__accum = false,                             // control placed
                md__tblur__accum__amount = 50,                        // control placed
                md__videoip = "1",                                    // control placed
                md__xres = 0,                                         // control placed
                md__xscale = 3,                                       // control placed
                md__xscalefs = 1,                                     // control placed
                md__yres = 0,                                         // control placed
                md__yscale = 3,                                       // control placed
                md__yscalefs = 1,                                     // control placed

                md__shader__goat__fprog = false,
                md__shader__goat__hdiv = 0.50,
                md__shader__goat__pat = "goatron",
                md__shader__goat__slen = true,
                md__shader__goat__tp = 0.50,
                md__shader__goat__vdiv = 0.50,

                // saturn
                ss__bios_jp = "sega_101.bin",
                ss__bios_na_eu = "mpr-17933.bin",
                ss__bios_sanity = true,                                     // placed
                ss__cart = "auto",                                          // placed
                ss__cart__auto_default = "backup",							// placed
                ss__cart__kof95_path = "mpr-18811-mx.ic1",
                ss__cart__ultraman_path = "mpr-19367-mx.ic1",
                ss__cd_sanity = true,                                       // placed
                ss__input__mouse_sensitivity = 0.5,                         // placed
                ss__input__port1 = "gamepad",                               // placed
                ss__input__port1__gun_chairs = "0xFF0000",
                ss__input__port2 = "gamepad",                               // placed
                ss__input__port2__gun_chairs = "0x00FF00",
                ss__input__port3 = "gamepad",                               // placed
                ss__input__port3__gun_chairs = "0xFF00FF",
                ss__input__port4 = "gamepad",                               // placed
                ss__input__port4__gun_chairs = "0xFF8000",
                ss__input__port5 = "gamepad",                               // placed
                ss__input__port5__gun_chairs = "0xFFFF00",
                ss__input__port6 = "gamepad",                               // placed
                ss__input__port6__gun_chairs = "0x00FFFF",
                ss__input__port7 = "gamepad",                               // placed
                ss__input__port7__gun_chairs = "0x0080FF",
                ss__input__port8 = "gamepad",                               // placed
                ss__input__port8__gun_chairs = "0x8000FF",
                ss__input__port9 = "gamepad",                               // placed
                ss__input__port9__gun_chairs = "0xFF80FF",
                ss__input__port10 = "gamepad",                              // placed
                ss__input__port10__gun_chairs = "0x00FF80",
                ss__input__port11 = "gamepad",                              // placed
                ss__input__port11__gun_chairs = "0x8080FF",
                ss__input__port12 = "gamepad",                              // placed
                ss__input__port12__gun_chairs = "0xFF8080",
                //ss__midsync = false,                                        // placed
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

                ss__enable = true,
                ss__forcemono = false,                                // control placed
                ss__shader = "none",                               // control placed
                ss__scanlines = 0,                                    // control placed
                ss__special = "none",                                 // control placed
                ss__stretch = "aspect_mult2",                         // control placed
                ss__tblur = false,                                    // control placed
                ss__tblur__accum = false,                             // control placed
                ss__tblur__accum__amount = 50,                        // control placed
                ss__videoip = "1",                                    // control placed
                ss__xres = 0,                                         // control placed
                ss__xscale = 3,                                       // control placed
                ss__xscalefs = 1,                                     // control placed
                ss__yres = 0,                                         // control placed
                ss__yscale = 3,                                       // control placed
                ss__yscalefs = 1,                                     // control placed

                ss__correct_aspect = true,                              // control placed
                ss__h_blend = false,                                    // control placed
                ss__h_overscan = true,                                  // control placed

                ss__shader__goat__fprog = false,
                ss__shader__goat__hdiv = 0.50,
                ss__shader__goat__pat = "goatron",
                ss__shader__goat__slen = true,
                ss__shader__goat__tp = 0.50,
                ss__shader__goat__vdiv = 0.50,

                ss__input__sport1__multitap = false,
                ss__input__sport2__multitap = false,

                /* moved into controls section
                ss__input__port10__3dpad__mode__defpos = "digital",
                ss__input__port11__3dpad__mode__defpos = "digital",
                ss__input__port12__3dpad__mode__defpos = "digital",
                ss__input__port1__3dpad__mode__defpos = "digital",
                ss__input__port2__3dpad__mode__defpos = "digital",
                ss__input__port3__3dpad__mode__defpos = "digital",
                ss__input__port4__3dpad__mode__defpos = "digital",
                ss__input__port5__3dpad__mode__defpos = "digital",
                ss__input__port6__3dpad__mode__defpos = "digital",
                ss__input__port7__3dpad__mode__defpos = "digital",
                ss__input__port8__3dpad__mode__defpos = "digital",
                ss__input__port9__3dpad__mode__defpos = "digital",
                */


                // playstation
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
                //psx__input__port1__analogjoy__axis_scale = 1,               // placed
                //psx__input__port1__dualanalog__axis_scale = 1,          // placed
                //psx__input__port1__dualshock__axis_scale = 1,   // placed
                psx__input__port1__gun_chairs = "0xFF0000",     // placed
                psx__input__port1__memcard = true,              // placed
                psx__input__port2 = "gamepad",  // placed
                //psx__input__port2__analogjoy__axis_scale = 1,   // placed
                //psx__input__port2__dualanalog__axis_scale = 1,  // placed
                //psx__input__port2__dualshock__axis_scale = 1,   // placed
                psx__input__port2__gun_chairs = "0xFF0000", // placed
                psx__input__port2__memcard = true,  // placed
                psx__input__port3 = "gamepad",  // placed
                //psx__input__port3__analogjoy__axis_scale = 1,   // placed
                //psx__input__port3__dualanalog__axis_scale = 1,  // placed
                //psx__input__port3__dualshock__axis_scale = 1,   // placed
                psx__input__port3__gun_chairs = "0xFF0000", // placed
                psx__input__port3__memcard = true,  // placed
                psx__input__port4 = "gamepad",  // placed
                //psx__input__port4__analogjoy__axis_scale = 1,   // placed
                //psx__input__port4__dualanalog__axis_scale = 1,  // placed
                //psx__input__port4__dualshock__axis_scale = 1,// placed
                psx__input__port4__gun_chairs = "0xFF0000", // placed
                psx__input__port4__memcard = true,  // placed
                psx__input__port5 = "gamepad",  // placed
                //psx__input__port5__analogjoy__axis_scale = 1,   // placed
                //psx__input__port5__dualanalog__axis_scale = 1,  // placed
                //psx__input__port5__dualshock__axis_scale = 1,   // placed
                psx__input__port5__gun_chairs = "0xFF0000", // placed
                psx__input__port5__memcard = true,  // placed
                psx__input__port6 = "gamepad",  // placed
                //psx__input__port6__analogjoy__axis_scale = 1,   // placed
                //psx__input__port6__dualanalog__axis_scale = 1,  // placed
                //psx__input__port6__dualshock__axis_scale = 1,   // placed
                psx__input__port6__gun_chairs = "0xFF0000", // placed
                psx__input__port6__memcard = true,  // placed
                psx__input__port7 = "gamepad",  // placed
                //psx__input__port7__analogjoy__axis_scale = 1,   // placed
                //psx__input__port7__dualanalog__axis_scale = 1,  // placed
                //psx__input__port7__dualshock__axis_scale = 1,   // placed
                psx__input__port7__gun_chairs = "0xFF0000", // placed
                psx__input__port7__memcard = true,  // placed
                psx__input__port8 = "gamepad",  // placed
                //psx__input__port8__analogjoy__axis_scale = 1,   // placed
                //psx__input__port8__dualanalog__axis_scale = 1,  // placed
                //psx__input__port8__dualshock__axis_scale = 1,   // placed
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

                psx__enable = true,
                psx__forcemono = false,                                // control placed
                psx__shader = "none",                               // control placed
                psx__scanlines = 0,                                    // control placed
                psx__special = "none",                                 // control placed
                psx__stretch = "aspect_mult2",                         // control placed
                psx__tblur = false,                                    // control placed
                psx__tblur__accum = false,                             // control placed
                psx__tblur__accum__amount = 50,                        // control placed
                psx__videoip = "1",                                    // control placed
                psx__xres = 0,                                         // control placed
                psx__xscale = 3,                                       // control placed
                psx__xscalefs = 1,                                     // control placed
                psx__yres = 0,                                         // control placed
                psx__yscale = 3,                                       // control placed
                psx__yscalefs = 1,                                     // control placed

                psx__input__analog_mode_ct__compare = "0x0F09",

                psx__shader__goat__fprog = false,
                psx__shader__goat__hdiv = 0.50,
                psx__shader__goat__pat = "goatron",
                psx__shader__goat__slen = true,
                psx__shader__goat__tp = 0.50,
                psx__shader__goat__vdiv = 0.50,

                psx_shared_memcards = false,

                // snes faust

                snes_faust__affinity__msu1__audio = "0x0000000000000000",
                snes_faust__affinity__msu1__data = "0x0000000000000000",
                snes_faust__affinity__ppu = "0x0000000000000000",
                snes_faust__correct_aspect = true,
                snes_faust__cx4__clock_rate = 100,                  // placed              
                snes_faust__h_filter = "none",                      // placed
                snes_faust__input__mouse_sensitivity = 0.50,        // placed                
                snes_faust__input__port1 = "gamepad",               // placed
                snes_faust__input__port2 = "gamepad",               // placed
                snes_faust__input__port3 = "gamepad",               // placed
                snes_faust__input__port4 = "gamepad",               // placed
                snes_faust__input__port5 = "gamepad",               // placed
                snes_faust__input__port6 = "gamepad",               // placed
                snes_faust__input__port7 = "gamepad",               // placed
                snes_faust__input__port8 = "gamepad",               // placed
                snes_faust__input__sport1__multitap = false,
                snes_faust__input__sport2__multitap = false,
                snes_faust__msu1__resamp_quality = 4,               // placed
                snes_faust__region = "auto",                        // placed
                snes_faust__renderer = "st",                        // placed
                snes_faust__resamp_quality = 3,                     // placed
                snes_faust__resamp_rate_error = 0.000035,           // placed
                snes_faust__slend = 223,                            // placed
                snes_faust__slendp = 238,                           // placed
                snes_faust__slstart = 223,                          // placed
                snes_faust__slstartp = 238,                         // placed
                snes_faust__spex = false,                           // placed
                snes_faust__spex__sound = true,                     // placed
                snes_faust__superfx__clock_rate = 100,
                snes_faust__superfx__icache = false,

                snes_faust__enable = true,
                snes_faust__forcemono = false,                                // control placed
                snes_faust__shader = "none",                                    // control placed
                snes_faust__scanlines = 0,                                    // control placed
                snes_faust__special = "none",                                 // control placed
                snes_faust__stretch = "aspect_mult2",                         // control placed
                snes_faust__tblur = false,                                    // control placed
                snes_faust__tblur__accum = false,                             // control placed
                snes_faust__tblur__accum__amount = 50,                        // control placed
                snes_faust__videoip = "1",                                    // control placed
                snes_faust__xres = 0,                                         // control placed
                snes_faust__xscale = 3,                                       // control placed
                snes_faust__xscalefs = 1,                                     // control placed
                snes_faust__yres = 0,                                         // control placed
                snes_faust__yscale = 3,                                       // control placed
                snes_faust__yscalefs = 1,                                     // control placed

                snes_faust__shader__goat__fprog = false,
                snes_faust__shader__goat__hdiv = 0.50,
                snes_faust__shader__goat__pat = "goatron",
                snes_faust__shader__goat__slen = true,
                snes_faust__shader__goat__tp = 0.50,
                snes_faust__shader__goat__vdiv = 0.50,

                // snes
                snes__apu__resamp_quality = 5,                      // placed
                snes__correct_aspect = false,                       // placed     

                snes__input__port1__multitap = false,               // placed
                snes__input__port2__multitap = false,               // placed
                snes__input__port1 = "gamepad",                     // placed
                snes__input__port2 = "gamepad",                     // placed
                /*     
                snes__input__port3 = "gamepad",                     // placed
                snes__input__port4 = "gamepad",                     // placed
                snes__input__port5 = "gamepad",                     // placed
                snes__input__port6 = "gamepad",                     // placed
                snes__input__port7 = "gamepad",                     // placed
                snes__input__port8 = "gamepad",
                */
                snes__mouse_sensitivity = 0.5,                      // placed

                snes__enable = true,
                snes__forcemono = false,                                // control placed
                snes__shader = "none",                               // control placed
                snes__scanlines = 0,                                    // control placed
                snes__special = "none",                                 // control placed
                snes__stretch = "aspect_mult2",                         // control placed
                snes__tblur = false,                                    // control placed
                snes__tblur__accum = false,                             // control placed
                snes__tblur__accum__amount = 50,                        // control placed
                snes__videoip = "0",                                    // control placed
                snes__xres = 0,                                         // control placed
                snes__xscale = 4,                                       // control placed
                snes__xscalefs = 1,                                     // control placed
                snes__yres = 0,                                         // control placed
                snes__yscale = 4,                                       // control placed
                snes__yscalefs = 1,                                     // control placed

                snes__shader__goat__fprog = false,
                snes__shader__goat__hdiv = 0.50,
                snes__shader__goat__pat = "goatron",
                snes__shader__goat__slen = true,
                snes__shader__goat__tp = 0.50,
                snes__shader__goat__vdiv = 0.50,

                snes__h_blend = false,



                // virtual boy
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

                vb__enable = true,
                vb__forcemono = false,                                // control placed
                vb__shader = "none",                               // control placed
                vb__scanlines = 0,                                    // control placed
                vb__special = "none",                                 // control placed
                vb__stretch = "aspect_mult2",                         // control placed
                vb__tblur = false,                                    // control placed
                vb__tblur__accum = false,                             // control placed
                vb__tblur__accum__amount = 50,                        // control placed
                vb__videoip = "0",                                    // control placed
                vb__xres = 0,                                         // control placed
                vb__xscale = 2,                                       // control placed
                vb__xscalefs = 1,                                     // control placed
                vb__yres = 0,                                         // control placed
                vb__yscale = 2,                                       // control placed
                vb__yscalefs = 1,                                     // control placed

                vb__ledonscale = 1.75,

                vb__shader__goat__fprog = false,
                vb__shader__goat__hdiv = 0.50,
                vb__shader__goat__pat = "goatron",
                vb__shader__goat__slen = true,
                vb__shader__goat__tp = 0.50,
                vb__shader__goat__vdiv = 0.50,

                // wonderswan
                wswan__bday = 23,                                   // placed
                wswan__blood = "o",                                 // placed
                wswan__bmonth = 6,                                  // placed
                wswan__byear = 1989,                                // placed
                wswan__language = "english",
                wswan__name = "Mednafen",                           // placed
                //wswan__rotateinput = false,                         // placed
                wswan__sex = "female",                              // placed

                wswan__debugger__disfontsize = "5x7",
                wswan__debugger__memcharenc = "shift_jis",

                wswan__enable = true,
                wswan__forcemono = false,                                // control placed
                wswan__shader = "none",                               // control placed
                wswan__scanlines = 0,                                    // control placed
                wswan__special = "none",                                 // control placed
                wswan__stretch = "aspect_mult2",                         // control placed
                wswan__tblur = false,                                    // control placed
                wswan__tblur__accum = false,                             // control placed
                wswan__tblur__accum__amount = 50,                        // control placed
                wswan__videoip = "0",                                    // control placed
                wswan__xres = 0,                                         // control placed
                wswan__xscale = 4,                                       // control placed
                wswan__xscalefs = 1,                                     // control placed
                wswan__yres = 0,                                         // control placed
                wswan__yscale = 4,                                       // control placed
                wswan__yscalefs = 1,                                     // control placed

                wswan__shader__goat__fprog = false,
                wswan__shader__goat__hdiv = 0.50,
                wswan__shader__goat__pat = "goatron",
                wswan__shader__goat__slen = true,
                wswan__shader__goat__tp = 0.50,
                wswan__shader__goat__vdiv = 0.50,

                wswan__input__builtin = "gamepad",


            };

            return cfbs;
        }
    }
}
