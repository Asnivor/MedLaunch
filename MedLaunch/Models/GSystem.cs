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

        public static int GetSystemIdSubFirst(string systemName)
        {
            var subsys = (from a in GetSubSystems()
                          where a.systemName == systemName
                          select a).FirstOrDefault();

            if (subsys == null)
            {
                // no sub system detected
                int _sysid = (from a in GetSystems()
                              where a.systemName == systemName
                              select a.systemId).FirstOrDefault();
                return _sysid;
            }
            else
            {
                // sub sys deetected - return parent
                return subsys.parentId;
            }
        }

        public static int GetSystemId(string systemName)
        {
            int _sysid = (from a in GetSystems()
                                  where a.systemName == systemName
                                  select a.systemId).FirstOrDefault();
            return _sysid;
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

        public List<int> GetGamesDBId(int _systemId)
        {
            List<int> _gamesdbid = (from a in GetSystems()
                                  where a.systemId == _systemId
                                  select a.theGamesDBPlatformId).FirstOrDefault();
            return _gamesdbid;
        }

        public static string ReturnGamesDBPlatformName(int GamesDbPlatformId)
        {
            string r = "";
            switch (GamesDbPlatformId)
            {
                case 4:
                    r = "Nintendo Game Boy";
                    break;
                case 41:
                    r = "Nintendo Game Boy Color";
                    break;
                case 5:
                    r = "Nintendo Game Boy Advance";
                    break;
                case 4924:
                    r = "Atari Lynx";
                    break;
                case 36:
                    r = "Sega Mega Drive";
                    break;
                case 18:
                    r = "Sega Genesis";
                    break;
                case 20:
                    r = "Sega Game Gear";
                    break;
                case 4923:
                    r = "Neo Geo Pocket Color";
                    break;
                case 4922:
                    r = "Neo Geo Pocket";
                    break;
                case 34:
                    r = "TurboGrafx 16";
                    break;
                case 4955:
                    r = "TurboGrafx CD";
                    break;
                case 4930:
                    r = "PC-FX";
                    break;
                case 10:
                    r = "Sony Playstation";
                    break;
                case 35:
                    r = "Sega Master System";
                    break;
                case 7:
                    r = "Nintendo Entertainment System (NES)";
                    break;
                case 6:
                    r = "Super Nintendo (SNES)";
                    break;
                case 17:
                    r = "Sega Saturn";
                    break;
                case 4918:
                    r = "Nintendo Virtual Boy";
                    break;
                case 4925:
                    r = "WonderSwan";
                    break;
                case 4926:
                    r = "WonderSwan Color";
                    break;
                case 4936:
                    r = "Famicon Disk System";
                    break;

                default:
                    break;
            }
            return r;
        }

        public static int GetMedLaunchSystemIdFromGDBPlatformId(int pid)
        {
            var gsys = GetSystems();
            foreach (var sys in gsys)
            {
                foreach (int i in sys.theGamesDBPlatformId)
                {
                    if (i == pid)
                    {
                        return sys.systemId;
                    }
                }
            }

            return 0;
        }

        public static int GetMedLaunchSubIdFromGDBPlatformId(int pid)
        {
            var gsys = GetSystems();
            foreach (var sys in gsys)
            {
                foreach (int i in sys.theGamesDBPlatformId)
                {
                    if (i == pid)
                    {
                        return sys.systemId;
                    }
                }
            }

            return 0;
        }

        public static string GetSubSystemName(int subId)
        {
            var sys = (from a in GetSubSystems()
                       where a.systemId == subId
                       select a).FirstOrDefault();

            if (sys == null)
                return null;

            return sys.systemName;
        }

        public static int GetSubSystemId(int parent, string extension)
        {
            List<GSubSystem> search = (from a in GetSubSystems()
                                       where a.parentId == parent
                                       select a).ToList();
            if (search.Count() == 0)
                return 0;

            foreach (var thing in search)
            {
                var ext = (from a in search
                           where a.supportedFileExtensions.Contains(extension)
                           select a).FirstOrDefault();

                if (ext == null)
                    continue;

                return ext.systemId;
            }
            return 0;
        }

        public static List<GSubSystem> GetSubSystems()
        {
            List<GSubSystem> gSystems = new List<GSubSystem>
                {
                new GSubSystem { systemId = 1, parentId = 1, theGamesDBPlatformId = new List<int> { 4 }, systemCode = "gb", systemName = "Nintendo Gameboy",
                 MobyPlatformName = new List<string> { "gameboy" },
                        supportedFileExtensions = ".gb", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Game Boy, is an 8-bit handheld video game device developed and manufactured by Nintendo. It was released in Japan on April 21, 1989, in North America in August 1989, and in Europe on September 28, 1990. It is the first handheld console in the Game Boy line." },

                new GSubSystem { systemId = 2, parentId = 1, theGamesDBPlatformId = new List<int> { 41 }, systemCode = "gb", systemName = "Nintendo Gameboy Color",
                 MobyPlatformName = new List<string> { "gameboy-color" },
                        supportedFileExtensions = ".gbc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Game Boy Color is Nintendo's successor to the 8-bit Game Boy handheld game console. It features a color screen and is slightly thicker and taller than the Game Boy Pocket. As with the original Game Boy, it has an 8-bit processor." },

                new GSubSystem { systemId = 3, parentId = 11, theGamesDBPlatformId = new List<int>  { 7 }, systemCode = "nes", systemName = "NES/Famicon",
                MobyPlatformName = new List<string> { "nes" },
                    supportedFileExtensions = ".nes,.unf", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Nintendo Entertainment System (also abbreviated as NES or simply called Nintendo) is an 8-bit video game console that was released by Nintendo in North America during 1985, in Europe during 1986 and Australia in 1987" },

                new GSubSystem { systemId = 4, parentId = 11, theGamesDBPlatformId = new List<int>  { 4936 }, systemCode = "nes", systemName = "Famicon Disk System",
                MobyPlatformName = new List<string> { "nes" },
                    supportedFileExtensions = ".fds", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Family Disk System was released on February 21, 1986 by Nintendo as a peripheral for the Nintendo Family Computer console in Japan. It uses proprietary floppy disks (called \"Disk Cards\") for data storage" },


                new GSubSystem { systemId = 5, parentId = 6, theGamesDBPlatformId = new List<int>  { 4922 },systemCode = "ngp", systemName = "NeoGeo Pocket",
                MobyPlatformName = new List<string> { "neo-geo-pocket" },
                    supportedFileExtensions = ".ngp", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Neo Geo Pocket is a monochrome handheld video game console released by SNK. It was the company's first handheld system and is part of the Neo Geo family. It debuted in Japan in late 1998, however never saw a western release, being exclusive to Japan and smaller Asian markets such as Hong Kong." },

                new GSubSystem { systemId = 6, parentId = 6, theGamesDBPlatformId = new List<int>  { 4923 },systemCode = "ngp", systemName = "NeoGeo Pocket Color",
                MobyPlatformName = new List<string> { "neo-geo-pocket-color" },
                    supportedFileExtensions = ".ngc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Neo Geo Pocket Color is a 16-bit colour handheld video game console manufactured by SNK. It is a successor to SNK's monochrome Neo Geo Pocket handheld which debuted in 1998 in Japan. The Neo Geo Pocket Color was SNK's last video game console, and is backwards compatible with the Neo Geo Pocket" },

                new GSubSystem { systemId = 7, parentId = 15, theGamesDBPlatformId = new List<int>  { 4925 }, systemCode = "wswan", systemName = "WonderSwan",
                MobyPlatformName = new List<string> { "wonderswan", "wonderswan-color" },
                    supportedFileExtensions = ".ws", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The WonderSwan was a line of handheld game consoles produced in Japan by Bandai. It was developed by Gunpei Yokoi's company Koto and Bandai. The WonderSwan was made to compete with the Neo Geo Pocket Color and the market leader Nintendo's Game Boy Color (even though the developer for the WonderSwan, Gunpei Yokoi, developed the original Nintendo Game Boy)." },

                new GSubSystem { systemId = 8, parentId = 15, theGamesDBPlatformId = new List<int>  { 4926 }, systemCode = "wswan", systemName = "WonderSwan Color",
                MobyPlatformName = new List<string> { "wonderswan", "wonderswan-color" },
                    supportedFileExtensions = ".wsc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The WonderSwan Color was released on December 9, 2000 in Japan, and was a moderate success. The original WonderSwan had only a black and white screen. Although the WonderSwan Color was slightly larger and heavier (7 mm and 2 g) compared to the original WonderSwan, the color version featured 512KB of RAM and a larger color LCD screen. In addition, the WonderSwan Color is compatible with the original WonderSwan library of games." },

                };
            return gSystems;
        }

        public static List<GSystem> GetSystems()
        {
            List<GSystem> gSystems = new List<GSystem>
                {
                new GSystem { systemId = 1, theGamesDBPlatformId = new List<int> { 4, 41 }, systemCode = "gb", systemName = "Nintendo Gameboy/Gameboy Color",
                 MobyPlatformName = new List<string> { "gameboy", "gameboy-color" },
                        supportedFileExtensions = ".gb,.gba,.gbc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Game Boy Color is Nintendo's successor to the 8-bit Game Boy handheld game console. It features a color screen and is slightly thicker and taller than the Game Boy Pocket. As with the original Game Boy, it has an 8-bit processor." },

                new GSystem { systemId = 2, theGamesDBPlatformId = new List<int>  { 5 }, systemCode = "gba", systemName = "Nintendo Gameboy Advance",
                MobyPlatformName = new List<string> { "gameboy-advance" },
                        supportedFileExtensions = ".gba,.gb,.gbc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Game Boy Advance, (often shortened to GBA) is a 32-bit handheld video game console developed, manufactured, and marketed by Nintendo. It is the successor to the Game Boy Color." },

                new GSystem { systemId = 3, theGamesDBPlatformId = new List<int>  { 4924 }, systemCode = "lynx", systemName = "Atari Lynx",
                MobyPlatformName = new List<string> { "lynx" },
                        supportedFileExtensions =".lnx", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Atari Lynx is a 16-bit handheld game console that was released by Atari Corporation in September 1989. The Lynx holds the distinction of being the world's first handheld electronic game with a color LCD. The system is also notable for its forward-looking features, advanced graphics, and ambidextrous layout." },

                new GSystem { systemId = 4, theGamesDBPlatformId = new List<int>  { 36, 18 }, systemCode = "md", systemName = "Sega Mega Drive/Genesis",
                MobyPlatformName = new List<string> { "genesis" },
                    supportedFileExtensions = ".bin,.gen,.rom,.md,.smd", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Sega Genesis is a fourth-generation video game console developed and produced by Sega. It was originally released in Japan in 1988 as Mega Drive, then in North America in 1989 as Sega Genesis, and in Europe, Australia and other PAL regions in 1990 as Mega Drive." },

                new GSystem { systemId = 5, theGamesDBPlatformId = new List<int>  { 20 }, systemCode = "gg", systemName = "Sega Game Gear",
                MobyPlatformName = new List<string> { "game-gear" },
                    supportedFileExtensions = ".gg", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Sega Game Gear was Sega's first handheld game console. It was the third commercially available color handheld console, after the Atari Lynx and the TurboExpress. Work began on the console in 1989 under the codename \"Project Mercury\", following Sega's policy at the time of codenaming their systems after planets." },

                new GSystem { systemId = 6, theGamesDBPlatformId = new List<int>  { 4923, 4922 },systemCode = "ngp", systemName = "NeoGeo Pocket Color",
                MobyPlatformName = new List<string> { "neo-geo-pocket", "neo-geo-pocket-color" },
                    supportedFileExtensions = ".ngc,.ngp", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Neo Geo Pocket Color is a 16-bit colour handheld video game console manufactured by SNK. It is a successor to SNK's monochrome Neo Geo Pocket handheld which debuted in 1998 in Japan. The Neo Geo Pocket Color was SNK's last video game console, and is backwards compatible with the Neo Geo Pocket" },

                new GSystem { systemId = 7, theGamesDBPlatformId = new List<int>  { 34, 4955 }, systemCode = "pce", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx",
                MobyPlatformName = new List<string> { "turbo-grafx" },
                    supportedFileExtensions = ".pce, .sgx", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                new GSystem { systemId = 8, theGamesDBPlatformId = new List<int>  { 4930 }, systemCode = "pcfx", systemName = "PC-FX",
                MobyPlatformName = new List<string> { "pc-fx" },
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The PC-FX is a 32-bit home video game console made by NEC Corporation. It was released in Japan on December 23, 1994, just weeks after Sony's PlayStation and a month after the Sega Saturn." },

                new GSystem { systemId = 9, theGamesDBPlatformId = new List<int>  { 10 }, systemCode = "psx", systemName = "Sony PlayStation",
                MobyPlatformName = new List<string> { "playstation" },
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The PlayStation , officially abbreviated as PS; unofficially referred to as the PSX or PS1) is a 32-bit fifth-generation video game console first released by Sony Computer Entertainment in Japan on December 3, 1994. The PlayStation was the first of the PlayStation series of consoles and handheld game devices." },

                new GSystem { systemId = 10, theGamesDBPlatformId = new List<int>  { 35 }, systemCode = "sms", systemName = "Sega Master System",
                MobyPlatformName = new List<string> { "sega-master-system" },
                    supportedFileExtensions = ".sms,.bin", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Master System (abbreviated to SMS) is a third-generation video game console that was manufactured and released by Sega in 1985 in Japan (as the Sega Mark III), 1986 in North America and 1987 in Europe." },

                new GSystem { systemId = 11, theGamesDBPlatformId = new List<int>  { 7, 4936 }, systemCode = "nes", systemName = "NES/Famicon",
                MobyPlatformName = new List<string> { "nes" },
                    supportedFileExtensions = ".nes,.unf,.fds", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Nintendo Entertainment System (also abbreviated as NES or simply called Nintendo) is an 8-bit video game console that was released by Nintendo in North America during 1985, in Europe during 1986 and Australia in 1987" },

                new GSystem { systemId = 12, theGamesDBPlatformId = new List<int>  { 6 }, systemCode = "snes", systemName = "SNES/Super Famicom",
                MobyPlatformName = new List<string> { "snes" },
                    supportedFileExtensions = ".smc,.sfc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Super Nintendo Entertainment System was Nintendo's second home console, following the Nintendo Entertainment System (NES). The console introduced advanced graphics and sound capabilities compared with other consoles at the time. Additionally, development of a variety of enhancement chips (which were integrated on game circuit boards) helped to keep it competitive in the marketplace." },

                new GSystem { systemId = 13, theGamesDBPlatformId = new List<int>  { 17 }, systemCode = "ss", systemName = "Sega Saturn",
                MobyPlatformName = new List<string> { "sega-saturn" },
                    supportedFileExtensions = ".m3u,.cue,.ccd,.toc", supportedArchiveExtensions = "",
                        systemDescription = "The Sega Saturn is a 32-bit fifth-generation video game console that was first released by Sega on November 22, 1994 in Japan, May 11, 1995 in North America, and July 8, 1995 in Europe. The system was discontinued in North America and Europe in 1998, and in 2000 in Japan. While it was popular in Japan, the Saturn failed to gain a similar market share in North America and Europe against its main competitors: Sony's PlayStation and the Nintendo 64." },

                new GSystem { systemId = 14, theGamesDBPlatformId = new List<int>  { 4918 }, systemCode = "vb", systemName = "Nintendo Virtual Boy",
                MobyPlatformName = new List<string> { "virtual-boy" },
                    supportedFileExtensions = ".vb", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The Virtual Boy was a table-top video game console developed and manufactured by Nintendo. It was the first video game console that was supposed to be capable of displaying \"true 3D graphics\" out of the box, in a form of virtual reality." },

                new GSystem { systemId = 15, theGamesDBPlatformId = new List<int>  { 4925, 4926 }, systemCode = "wswan", systemName = "Bandai WonderSwan",
                MobyPlatformName = new List<string> { "wonderswan", "wonderswan-color" },
                    supportedFileExtensions = ".ws,.wsc", supportedArchiveExtensions = ".zip,.7z",
                        systemDescription = "The WonderSwan Color was released on December 9, 2000 in Japan, and was a moderate success. The original WonderSwan had only a black and white screen. Although the WonderSwan Color was slightly larger and heavier (7 mm and 2 g) compared to the original WonderSwan, the color version featured 512KB of RAM and a larger color LCD screen. In addition, the WonderSwan Color is compatible with the original WonderSwan library of games." },

                new GSystem { systemId = 16, theGamesDBPlatformId = new List<int>  { 6 }, systemCode = "snes_faust", systemName = "SNES/Super Famicom (Faust Emulation)",
                MobyPlatformName = new List<string> { "snes" },
                    supportedFileExtensions = "", supportedArchiveExtensions = ".zip,.7z",
                         systemDescription = "The Super Nintendo Entertainment System was Nintendo's second home console, following the Nintendo Entertainment System (NES). The console introduced advanced graphics and sound capabilities compared with other consoles at the time. Additionally, development of a variety of enhancement chips (which were integrated on game circuit boards) helped to keep it competitive in the marketplace." },

                new GSystem { systemId = 17, theGamesDBPlatformId = new List<int>  { 34, 4955 }, systemCode = "pce_fast", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx (Fast Emulation)",
                MobyPlatformName = new List<string> { "turbo-grafx" },
                    supportedFileExtensions = ".pce, .sgx", supportedArchiveExtensions = ".zip,.7z",
                       systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                new GSystem { systemId = 18, theGamesDBPlatformId = new List<int>  { 34, 4955 }, systemCode = "pcecd", systemName = "PC Engine (CD)/TurboGrafx 16 (CD)",
                MobyPlatformName = new List<string> { "turbografx-cd" },
                    supportedFileExtensions = ".cue,.ccd,.toc,", supportedArchiveExtensions = "",
                        systemDescription = "TurboGrafx-16, fully titled as TurboGrafx-16 Entertainment SuperSystem and known in Japan as the PC Engine, is a video game console developed by Hudson Soft and NEC, released in Japan on October 30, 1987, and in North America on August 29, 1989." },

                };
            return gSystems;
        }

        public static bool IsFileAllowed(string fileName, int systemId)
        {
            HashSet<string> exts = GetAllowedFileExtensions(systemId);
            bool isAllowed = false;
            foreach (string ext in exts)
            {
                //MessageBoxResult result3 = MessageBox.Show("Allowed extensions for systemid " + systemId + " extention: " + ext);
                if (fileName.EndsWith(ext))
                    isAllowed = true;
            }
            return isAllowed;
        }

        public static HashSet<string> GetAllowedFileExtensions(int systemId)
        {
            var exts = (from g in GSystem.GetSystems()
                        where g.systemId == systemId
                        select g).SingleOrDefault();
            string archive = exts.supportedArchiveExtensions;
            string nonArchive = exts.supportedFileExtensions;

            HashSet<string> supported = new HashSet<string>();
            char c = ',';
            string[] aSplit = archive.Split(c);
            string[] nSplit = nonArchive.Split(c);
            foreach (string s in aSplit) { supported.Add(s); }
            foreach (string s in nSplit) { supported.Add(s); }

            return supported;
        }

        public static HashSet<string> GetAllowedSubFileExtensions(int systemId)
        {
            var exts = (from g in GSystem.GetSubSystems()
                        where g.systemId == systemId
                        select g).SingleOrDefault();
            string archive = exts.supportedArchiveExtensions;
            string nonArchive = exts.supportedFileExtensions;

            HashSet<string> supported = new HashSet<string>();
            char c = ',';
            string[] aSplit = archive.Split(c);
            string[] nSplit = nonArchive.Split(c);
            foreach (string s in aSplit) { supported.Add(s); }
            foreach (string s in nSplit) { supported.Add(s); }

            return supported;
        }

        // Properties
        public int systemId { get; set; }
        public string systemCode { get; set; }
        public string systemName { get; set; }
        public string systemDescription { get; set; }
        public string supportedFileExtensions { get; set; }
        public string supportedArchiveExtensions { get; set; }
        public List<int> theGamesDBPlatformId { get; set; }
        public List<string> MobyPlatformName { get; set; }
    }

    public class GSubSystem
    {
        public int systemId { get; set; }
        public int parentId { get; set; }
        public string systemCode { get; set; }
        public string systemName { get; set; }
        public string systemDescription { get; set; }
        public string supportedFileExtensions { get; set; }
        public string supportedArchiveExtensions { get; set; }
        public List<int> theGamesDBPlatformId { get; set; }
        public List<string> MobyPlatformName { get; set; }
    }
}
