using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{ 
    public class GSystem
    {
        // constructors
        public GSystem()
        {
            // default empty
        }
        public GSystem(int _systemId)
        {
            // Set GSystem based on systemId
            GSystem gsystem = (from g in GetSystems()
                               where g.systemId == _systemId
                               select g).FirstOrDefault();
            systemId = gsystem.systemId;
            systemCode = gsystem.systemCode;
            systemName = gsystem.systemName;
            systemDescription = gsystem.systemDescription;
            supportedFileExtensions = gsystem.supportedFileExtensions;
            supportedArchiveExtensions = gsystem.supportedArchiveExtensions;

        }
        public GSystem(string _systemCode)
        {
            // Set GSystem based on systemCode
            GSystem gsystem = (from g in GetSystems()
                               where g.systemCode == _systemCode
                               select g).FirstOrDefault();
            systemId = gsystem.systemId;
            systemCode = gsystem.systemCode;
            systemName = gsystem.systemName;
            systemDescription = gsystem.systemDescription;
            supportedFileExtensions = gsystem.supportedFileExtensions;
            supportedArchiveExtensions = gsystem.supportedArchiveExtensions;
        }

        public static string GetSystemName(int _systemId)
        {
            string _systemName = (from a in GetSystems()
                                 where a.systemId == _systemId
                                 select a.systemName).FirstOrDefault();
            return _systemName;
        }

        public static string GetSystemDesc(int _systemId)
        {
            string _systemDesc = (from a in GetSystems()
                                 where a.systemId == _systemId
                                 select a.systemDescription).FirstOrDefault();
            return _systemDesc;
        }

        public static string GetSystemCode(int _systemId)
        {
            string _systemCode = (from a in GetSystems()
                                 where a.systemId == _systemId
                                 select a.systemCode).FirstOrDefault();
            return _systemCode;
        }

        public static List<GSystem> GetSystems()
        {
            List<GSystem> gSystems = new List<GSystem>
                {
                new GSystem { systemId = 1, systemCode = "gb", systemName = "Nintendo Gameboy/Gameboy Color",
                        supportedFileExtensions = ".gb,.gba,.gbc", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Game Boy Color is Nintendo's successor to the 8-bit Game Boy handheld game console. It features a color screen and is slightly thicker and taller than the Game Boy Pocket. As with the original Game Boy, it has an 8-bit processor." },

                new GSystem { systemId = 2, systemCode = "gba", systemName = "Nintendo Gameboy Advance",
                        supportedFileExtensions = ".gba,.gb,.gbc", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Game Boy Advance, (often shortened to GBA) is a 32-bit handheld video game console developed, manufactured, and marketed by Nintendo. It is the successor to the Game Boy Color." },

                new GSystem { systemId = 3, systemCode = "lynx", systemName = "Atari Lynx",
                        supportedFileExtensions =".lnx", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Atari Lynx is a 16-bit handheld game console that was released by Atari Corporation in September 1989. The Lynx holds the distinction of being the world's first handheld electronic game with a color LCD. The system is also notable for its forward-looking features, advanced graphics, and ambidextrous layout." },

                new GSystem { systemId = 4, systemCode = "md", systemName = "Sega Mega Drive/Genesis",
                    supportedFileExtensions = ".bin,.gen,.rom", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Sega Genesis is a fourth-generation video game console developed and produced by Sega. It was originally released in Japan in 1988 as Mega Drive, then in North America in 1989 as Sega Genesis, and in Europe, Australia and other PAL regions in 1990 as Mega Drive." },

                new GSystem { systemId = 5, systemCode = "gg", systemName = "Sega Game Gear",
                    supportedFileExtensions = ".gg", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Sega Game Gear was Sega's first handheld game console. It was the third commercially available color handheld console, after the Atari Lynx and the TurboExpress. Work began on the console in 1989 under the codename \"Project Mercury\", following Sega's policy at the time of codenaming their systems after planets." },

                new GSystem { systemId = 6, systemCode = "ngp", systemName = "NeoGeo Pocket Color",
                    supportedFileExtensions = ".ngc", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Neo Geo Pocket Color is a 16-bit colour handheld video game console manufactured by SNK. It is a successor to SNK's monochrome Neo Geo Pocket handheld which debuted in 1998 in Japan. The Neo Geo Pocket Color was SNK's last video game console, and is backwards compatible with the Neo Geo Pocket" },

                new GSystem { systemId = 7, systemCode = "pce", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx",
                    supportedFileExtensions = ".pce", supportedArchiveExtensions = ".zip",
                        systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                new GSystem { systemId = 8, systemCode = "pcfx", systemName = "PC-FX",
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The PC-FX is a 32-bit home video game console made by NEC Corporation. It was released in Japan on December 23, 1994, just weeks after Sony's PlayStation and a month after the Sega Saturn." },

                new GSystem { systemId = 9, systemCode = "psx", systemName = "Sony PlayStation",
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The PlayStation , officially abbreviated as PS; unofficially referred to as the PSX or PS1) is a 32-bit fifth-generation video game console first released by Sony Computer Entertainment in Japan on December 3, 1994. The PlayStation was the first of the PlayStation series of consoles and handheld game devices." },

                new GSystem { systemId = 10, systemCode = "sms", systemName = "Sega Master System",
                    supportedFileExtensions = ".sms,.bin", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Master System (abbreviated to SMS) is a third-generation video game console that was manufactured and released by Sega in 1985 in Japan (as the Sega Mark III), 1986 in North America and 1987 in Europe." },

                new GSystem { systemId = 11, systemCode = "nes", systemName = "NES/Famicon",
                    supportedFileExtensions = ".nes,.unf,.fds", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Nintendo Entertainment System (also abbreviated as NES or simply called Nintendo) is an 8-bit video game console that was released by Nintendo in North America during 1985, in Europe during 1986 and Australia in 1987" },

                new GSystem { systemId = 12, systemCode = "snes", systemName = "SNES/Super Famicom",
                    supportedFileExtensions = ".smc,.sfc", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Super Nintendo Entertainment System was Nintendo's second home console, following the Nintendo Entertainment System (NES). The console introduced advanced graphics and sound capabilities compared with other consoles at the time. Additionally, development of a variety of enhancement chips (which were integrated on game circuit boards) helped to keep it competitive in the marketplace." },

                new GSystem { systemId = 13, systemCode = "ss", systemName = "Sega Saturn",
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The Sega Saturn is a 32-bit fifth-generation video game console that was first released by Sega on November 22, 1994 in Japan, May 11, 1995 in North America, and July 8, 1995 in Europe. The system was discontinued in North America and Europe in 1998, and in 2000 in Japan. While it was popular in Japan, the Saturn failed to gain a similar market share in North America and Europe against its main competitors: Sony's PlayStation and the Nintendo 64." },

                new GSystem { systemId = 14, systemCode = "vb", systemName = "Nintendo Virtual Boy",
                    supportedFileExtensions = ".vb", supportedArchiveExtensions = ".zip",
                        systemDescription = "The Virtual Boy was a table-top video game console developed and manufactured by Nintendo. It was the first video game console that was supposed to be capable of displaying \"true 3D graphics\" out of the box, in a form of virtual reality." },

                new GSystem { systemId = 15, systemCode = "wswan", systemName = "Bandai WonderSwan",
                    supportedFileExtensions = ".ws,.wsc", supportedArchiveExtensions = ".zip",
                        systemDescription = "The WonderSwan Color was released on December 9, 2000 in Japan, and was a moderate success. The original WonderSwan had only a black and white screen. Although the WonderSwan Color was slightly larger and heavier (7 mm and 2 g) compared to the original WonderSwan, the color version featured 512KB of RAM and a larger color LCD screen. In addition, the WonderSwan Color is compatible with the original WonderSwan library of games." },

                new GSystem { systemId = 16, systemCode = "snes_faust", systemName = "SNES/Super Famicom (Faust Emulation)",
                    supportedFileExtensions = "", supportedArchiveExtensions = ".zip",
                         systemDescription = "The Super Nintendo Entertainment System was Nintendo's second home console, following the Nintendo Entertainment System (NES). The console introduced advanced graphics and sound capabilities compared with other consoles at the time. Additionally, development of a variety of enhancement chips (which were integrated on game circuit boards) helped to keep it competitive in the marketplace." },

                new GSystem { systemId = 17, systemCode = "pce_fast", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx (Fast Emulation)",
                    supportedFileExtensions = ".pce", supportedArchiveExtensions = ".zip",
                       systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                new GSystem { systemId = 18, systemCode = "pcecd", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)",
                    supportedFileExtensions = ".cue,.ccd,.toc,", supportedArchiveExtensions = "",
                        systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                };
            return gSystems;
        }

        // Properties
        public int systemId { get; set; }
        public string systemCode { get; set; }
        public string systemName { get; set; }
        public string systemDescription { get; set; }
        public string supportedFileExtensions { get; set; }
        public string supportedArchiveExtensions { get; set; }
    }
}
