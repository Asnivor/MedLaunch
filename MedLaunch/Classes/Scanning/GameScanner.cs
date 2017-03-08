using Asnitech.Launch.Common;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows;
using Ookii.Dialogs.Wpf;
using Microsoft.Win32;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.Scraper.DAT.NOINTRO.Models;
using MedLaunch.Classes.IO;
using MedLaunch.Classes.Scraper.DAT.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using SharpCompress;
using SharpCompress.Archives;

namespace MedLaunch.Classes
{
    public class GameScanner
    {
        public static MyDbContext db;

        // constructor
        public GameScanner()
        {
            // load master dat from disk
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\DATMaster.json";
            DAT = JsonConvert.DeserializeObject<IEnumerable<DATMerge>>(File.ReadAllText(filePath));

            db = new MyDbContext();            

            Games = (from g in db.Game
                     select g).ToList();

            Paths = (from p in db.Paths
                     where p.pathId == 1
                     select p).ToList().SingleOrDefault();

            Systems = GSystem.GetSystems();

            RomSystems = new List<GSystem>();
            DiskSystems = new List<GSystem>();

            // populate RomSystems and DiskSystems
            foreach (GSystem gs in Systems)
            {
                // exlude non-path systems
                if (gs.systemId == 16 || gs.systemId == 17)
                    continue;

                // populate disksystems
                if (gs.systemId == 18           // pcecd
                    || gs.systemId == 8         // pcfx
                    || gs.systemId == 9         // psx
                    || gs.systemId == 13)       // Saturn
                    DiskSystems.Add(gs);
                else
                    RomSystems.Add(gs);
            }

            RomSystemsWithPaths = new List<GSystem>();
            DiskSystemsWithPaths = new List<GSystem>();

            // populate RomSystemsWithPaths with only entries that only have Rom paths set (and are not non-path systems like snes_faust and pce_fast) and where ROM directories are valid
            foreach (var sys in RomSystems)
            {
                if (GetPath(sys.systemId) == null || GetPath(sys.systemId) == "" || !Directory.Exists(GetPath(sys.systemId)))
                {
                    continue;
                }
                RomSystemsWithPaths.Add(sys);
            }
            /*
            for (int i = 1; RomSystems.Count >= i; i++)
            {
                if (GetPath(i) == null || GetPath(i) == "")
                    continue;

                MessageBoxResult result2 = MessageBox.Show(RomSystems[i - 1].systemName);
                RomSystemsWithPaths.Add(RomSystems[i - 1]); 
            }
            */
            // populate DiskSystemsWithPaths with only entries that only have Disk paths set (and are not non-path systems like snes_faust and pce_fast)
            foreach (var sys in DiskSystems)
            {
                if (GetPath(sys.systemId) == null || GetPath(sys.systemId) == "")
                {
                    continue;
                }
                DiskSystemsWithPaths.Add(sys);
            }

            // per system lists
            GamesGB = (from g in Games
                      where g.systemId == 1
                      select g).ToList();

            GamesGBA = (from g in Games
                       where g.systemId == 2
                       select g).ToList();

            GamesLYNX = (from g in Games
                        where g.systemId == 3
                        select g).ToList();

            GamesMD = (from g in Games
                         where g.systemId == 4
                         select g).ToList();

            GamesGG = (from g in Games
                       where g.systemId == 5
                       select g).ToList();

            GamesNGP = (from g in Games
                       where g.systemId == 6
                       select g).ToList();

            GamesPCE = (from g in Games
                       where g.systemId == 7
                       select g).ToList();

            GamesPCFX = (from g in Games
                       where g.systemId == 8
                       select g).ToList();

            GamesPSX = (from g in Games
                       where g.systemId == 9
                       select g).ToList();

            GamesSMS = (from g in Games
                       where g.systemId == 10
                       select g).ToList();

            GamesNES = (from g in Games
                       where g.systemId == 11
                       select g).ToList();

            GamesSNES = (from g in Games
                       where g.systemId == 12
                       select g).ToList();

            GamesSS = (from g in Games
                       where g.systemId == 13
                       select g).ToList();

            GamesVB = (from g in Games
                       where g.systemId == 14
                       select g).ToList();

            GamesWSWAN = (from g in Games
                       where g.systemId == 15
                       select g).ToList();

            GamesPCECD = (from g in Games
                          where g.systemId == 18
                          select g).ToList();



            RomsToUpdate = new List<Game>();
            RomsToAdd = new List<Game>();
            DisksToUpdate = new List<Game>();
            DisksToAdd = new List<Game>();
            AddedStats = 0;
            HiddenStats = 0;
            UpdatedStats = 0;
            UntouchedStats = 0;
        }

        // properties
        public static List<Game> Games { get; private set; }
        public static Paths Paths { get; private set; }
        public static List<GSystem> Systems { get; private set; }
        public static List<GSystem> RomSystems { get; private set; }
        public static List<GSystem> DiskSystems { get; private set; }
        public static List<GSystem> RomSystemsWithPaths { get; private set; }
        public static List<GSystem> DiskSystemsWithPaths { get; private set; }
        public static List<Game> GamesGB { get; private set; }
        public static List<Game> GamesGBA { get; private set; }
        public static List<Game> GamesLYNX { get; private set; }
        public static List<Game> GamesMD { get; private set; }
        public static List<Game> GamesGG { get; private set; }
        public static List<Game> GamesNGP { get; private set; }
        public static List<Game> GamesPCE { get; private set; }
        public static List<Game> GamesPCFX { get; private set; }
        public static List<Game> GamesPSX { get; private set; }
        public static List<Game> GamesSMS { get; private set; }
        public static List<Game> GamesNES { get; private set; }
        public static List<Game> GamesSNES { get; private set; }
        public static List<Game> GamesSS { get; private set; }
        public static List<Game> GamesVB { get; private set; }
        public static List<Game> GamesWSWAN { get; private set; }
        public static List<Game> GamesPCECD { get; private set; }

        public static List<Paths> NonNullPaths { get; private set; }

        public static List<Game> RomsToUpdate { get; set; }
        public static List<Game> RomsToAdd { get; set; }

        public static List<Game> DisksToUpdate { get; set; }
        public static List<Game> DisksToAdd { get; set; }

        public List<Game> MarkedAsHidden { get; set; }

        public static int AddedStats { get; set; }
        public static int HiddenStats { get; set; }
        public static int UpdatedStats { get; set; }
        public static int UntouchedStats { get; set; }

        public static IEnumerable<DATMerge> DAT { get; set; }

        // methods
        public string GetPath(int systemId)
        {
            string path = "";
            switch (systemId)
            {
                case 1:
                    path = Paths.systemGb;
                    break;
                case 2:
                    path = Paths.systemGba;
                    break;
                case 3:
                    path = Paths.systemLynx;
                    break;
                case 4:
                    path = Paths.systemMd;
                    break;
                case 5:
                    path = Paths.systemGg;
                    break;
                case 6:
                    path = Paths.systemNgp;
                    break;
                case 7:
                    path = Paths.systemPce;
                    break;
                case 8:
                    path = Paths.systemPcfx;
                    break;
                case 9:
                    path = Paths.systemPsx;
                    break;
                case 10:
                    path = Paths.systemSms;
                    break;
                case 11:
                    path = Paths.systemNes;
                    break;
                case 12:
                    path = Paths.systemSnes;
                    break;
                case 13:
                    path = Paths.systemSs;
                    break;
                case 14:
                    path = Paths.systemVb;
                    break;
                case 15:
                    path = Paths.systemWswan;
                    break;
                case 18:
                    path = Paths.systemPceCd;
                    break;
                default:
                    path = "";
                    break;
            }
            return path;
        }  


        public static void SaveToDatabase()
        {
            using (db)
            {
                db.AddRange(RomsToAdd);
                db.UpdateRange(RomsToUpdate);

                db.AddRange(DisksToAdd);
                db.UpdateRange(DisksToUpdate);

                db.SaveChanges();

                GamesLibData.ForceUpdate();
            }
        }
    }
}
