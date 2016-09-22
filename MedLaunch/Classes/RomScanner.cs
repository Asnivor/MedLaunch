using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public static class RomScanner
    {
        public static List<GameSystem> GetSystems()
        {
            List<GameSystem> systems = new List<GameSystem>();
            using (var sysCon = new MyDbContext())
            {
                var sys = from s in sysCon.GameSystem
                          select s;
                foreach (GameSystem g in sys)
                {
                    systems.Add(g);
                }
                return systems;
            }
        }

        public static string GetPath(int systemId)
        {
            // check whether path exists

            string path = "";

            using (var sysPath = new MyDbContext())
            {
                var sPath = (from p in sysPath.Paths
                             select p).FirstOrDefault();

                switch (systemId)
                {
                    case 1:
                        path = sPath.systemGb;
                        break;
                    case 2:
                        path = sPath.systemGba;
                        break;
                    case 3:
                        path = sPath.systemLynx;
                        break;
                    case 4:
                        path = sPath.systemMd;
                        break;
                    case 5:
                        path = sPath.systemGg;
                        break;
                    case 6:
                        path = sPath.systemNgp;
                        break;
                    case 7:
                        path = sPath.systemPce;
                        break;
                    case 8:
                        path = sPath.systemPcfx;
                        break;
                    case 9:
                        path = sPath.systemPsx;
                        break;
                    case 10:
                        path = sPath.systemSms;
                        break;
                    case 11:
                        path = sPath.systemNes;
                        break;
                    case 12:
                        path = sPath.systemSnes;
                        break;
                    case 13:
                        path = sPath.systemSs;
                        break;
                    case 14:
                        path = sPath.systemVb;
                        break;
                    case 15:
                        path = sPath.systemWswan;
                        break;
                    default:
                        path = "";
                        break;
                }

                return path;        
            }
        }

        // return number of files found in a directory (without recursion)
        public static int CountFiles(string path)
        {
            int fileCount = 0;
            try
            {
                fileCount = Directory.EnumerateFiles(@path).Count();
            }
            catch { }
            
            return fileCount;
        }

        // get a list of files from a directory
        public static System.Collections.Generic.IEnumerable<string> GetFiles(string path)
        {
            var files = Directory.GetFiles(@path);
            return files;
        }

        // attempt to add game to Game database
        public static int AddGame(Rom systemRom, string fullPath, string relPath, string fileName, string extension, string romName)
        {
            // check whether ROM already exists in database
            using (var romContext = new MyDbContext())
            {
                var rom = (from r in romContext.Game
                          where (r.gameName == romName) && (r.GameSystem.systemId == systemRom.gameSystem.systemId)
                          select r).SingleOrDefault();

                if (rom != null)
                {
                    // Rom already exists in database. Check whether it needs updating
                    if (rom.gamePath == relPath)
                    {
                        // path is correct in database - skip updating
                        return 0;
                    }
                    else
                    {
                        // path is incorrect in database - update record
                        Game g = rom;
                        g.gamePath = relPath;

                        UpdateRom(g);
                        return 2;
                    }
                }
                else
                {
                    // Rom does not exist. Add to database.
                    Game g = new Game();
                    g.gameName = romName;
                    g.gamePath = relPath;
                    g.systemId = systemRom.gameSystem.systemId;
                    //g.GameSystem.systemId = systemRom.gameSystem.systemId;
                    g.isFavorite = false;
                    g.configId = 1;
                    g.hidden = false;

                    InsertRom(g);
                    return 1;
                }
            }
            

        }

        private static void InsertRom(Game rom)
        {
            using (var iR = new MyDbContext())
            {
                iR.Game.Add(rom);
                iR.SaveChanges();
                iR.Dispose();
            }
        }
        private static void UpdateRom(Game rom)
        {
            using (var uR = new MyDbContext())
            {
                uR.Game.Update(rom);
                uR.SaveChanges();
                uR.Dispose();
            }
        }

        // get favorite status
        public static int GetFavoriteStatus(int Id)
        {
            using (var romContext = new MyDbContext())
            {
                var rom = (from r in romContext.Game
                           where r.gameId == Id
                           select r).SingleOrDefault();

                if (rom != null)
                {
                    if (rom.isFavorite == true)
                    {
                        romContext.Dispose();
                        Debug.WriteLine("FAVOTIE!");
                        return 1;
                    }
                    else
                    {
                        romContext.Dispose();
                        return 0;
                    }
                }
                else
                { 
                romContext.Dispose();
                return 0;
                }

            }
        }

        // update favorites toggle
        public static void FavoriteToggle(int Id)
        {
            using (var romaContext = new MyDbContext())
            {
                Game rom = (from r in romaContext.Game
                            where r.gameId == Id
                            select r).SingleOrDefault();

                if (rom != null)
                {
                    if (GetFavoriteStatus(Id) == 1)
                    {
                        // Rom is marked as a favorite - make isFavorite as false
                        rom.isFavorite = false;
                    }
                    else
                    {
                        // rom is not marked as favorite - make isFavorite true
                        rom.isFavorite = true;
                    }
                }

                // update ROM
                UpdateRom(rom);

                romaContext.Dispose();
            }
        }
    }
}
