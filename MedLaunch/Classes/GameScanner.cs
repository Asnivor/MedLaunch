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
        private MyDbContext db;

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
        public List<Game> Games { get; private set; }
        public Paths Paths { get; private set; }
        public List<GSystem> Systems { get; private set; }
        public List<GSystem> RomSystems { get; private set; }
        public List<GSystem> DiskSystems { get; private set; }
        public List<GSystem> RomSystemsWithPaths { get; private set; }
        public List<GSystem> DiskSystemsWithPaths { get; private set; }
        public List<Game> GamesGB { get; private set; }
        public List<Game> GamesGBA { get; private set; }
        public List<Game> GamesLYNX { get; private set; }
        public List<Game> GamesMD { get; private set; }
        public List<Game> GamesGG { get; private set; }
        public List<Game> GamesNGP { get; private set; }
        public List<Game> GamesPCE { get; private set; }
        public List<Game> GamesPCFX { get; private set; }
        public List<Game> GamesPSX { get; private set; }
        public List<Game> GamesSMS { get; private set; }
        public List<Game> GamesNES { get; private set; }
        public List<Game> GamesSNES { get; private set; }
        public List<Game> GamesSS { get; private set; }
        public List<Game> GamesVB { get; private set; }
        public List<Game> GamesWSWAN { get; private set; }
        public List<Game> GamesPCECD { get; private set; }

        public List<Paths> NonNullPaths { get; private set; }

        public List<Game> RomsToUpdate { get; set; }
        public List<Game> RomsToAdd { get; set; }

        public List<Game> DisksToUpdate { get; set; }
        public List<Game> DisksToAdd { get; set; }

        public List<Game> MarkedAsHidden { get; set; }

        public int AddedStats { get; set; }
        public int HiddenStats { get; set; }
        public int UpdatedStats { get; set; }
        public int UntouchedStats { get; set; }

        public IEnumerable<DATMerge> DAT { get; set; }

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
                    path = Paths.systemPce;
                    break;
                default:
                    path = "";
                    break;
            }
            return path;
        }
        // remove ALL GAMES from db for all systems
        public static void RemoveAllGames()
        {
            MessageBoxResult result = MessageBox.Show("This operation will wipe out ALL the games in your games library database (but they will not be deleted from disk)\n\nAre you sure you wish to continue?", "WARNING", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                // get all roms for specific system
                using (var context = new MyDbContext())
                {
                    List<Game> roms = (from a in context.Game
                                       select a).ToList();
                    Game.DeleteGames(roms);
                    GameListBuilder.UpdateFlag();
                    
                }
            }            
        }

        // remove all roms from db for a certain system
        public static void RemoveRoms(int sysId)
        {
            MessageBoxResult result = MessageBox.Show("This operation will wipe out ALL the " + GSystem.GetSystemName(sysId) +  " games in your library database (but they will not be deleted from disk)\n\nAre you sure you wish to continue?", "WARNING", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                // get all roms for specific system
                using (var context = new MyDbContext())
                {
                    List<Game> roms = (from a in context.Game
                                       where a.systemId == sysId
                                       select a).ToList();
                    Game.DeleteGames(roms);
                    GameListBuilder.UpdateFlag();
                }
            }                         
        }

        // remove all disk-based games from db for a certain system
        public static void RemoveDisks(int sysId)
        {
            MessageBoxResult result = MessageBox.Show("This operation will wipe out ALL the " + GSystem.GetSystemName(sysId) + " games in your library database (but they will not be deleted from disk)\n\nAre you sure you wish to continue?", "WARNING", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                // get all disks for specific system
                using (var context = new MyDbContext())
                {
                    List<Game> disks = (from a in context.Game
                                        where a.systemId == sysId
                                        select a).ToList();
                    Game.DeleteGames(disks);
                    GameListBuilder.UpdateFlag();
                }
            }                
        }

        // return number of files found in a directory and sub-directory (based on usingRecursion bool)
        public static int CountFiles(string path, bool usingRecursion)
        {
            int fileCount = 0;
            try
            {
                if (usingRecursion == true)
                    fileCount = Directory.EnumerateFiles(@path, "*.*", SearchOption.AllDirectories).Count();
                else
                    fileCount = Directory.EnumerateFiles(@path, "*.*").Count();
            }
            catch { }

            return fileCount;
        }

        // Start ROM scan and import process for specific system
        public void BeginRomImport(int systemId, ProgressDialogController dialog)
        {
            // get path to ROM folder
            string romFolderPath = GetPath(systemId);
            //MessageBoxResult result2 = MessageBox.Show(romFolderPath);
            // get allowed file types for this particular system
            HashSet<string> exts = GetAllowedFileExtensions(systemId);

            // get a list of games for this system currently already in the database
            List<Game> presentGames = (from g in Games
                                       where g.systemId == systemId
                                       select g).ToList();


            // get all files from romfolderpath and sub directories that have an allowed extension
            IEnumerable < string > romFiles = GetFiles(romFolderPath, true);

            // if romfiles is null break
            if (romFiles == null)
                return;

            List<string> allowedFiles = new List<string>();
            foreach (string s in exts)
            {
                
                foreach (string p in romFiles)
                {
                    if (p.EndsWith(s))
                    {
                        //MessageBoxResult result5 = MessageBox.Show(p);
                        allowedFiles.Add(p);
                    }
                    
                }
                /*
                List<string> fi = (from a in romFiles
                         where a.EndsWith(s)
                         select a).ToList();
                if (fi == null || fi.Count < 1) { continue; }
                allowedFiles.AddRange(fi);       
                */             
            }

            // calculate the number of files to be processed
            int numFiles = allowedFiles.Count;
            // set the progress bar limits
            //dialog.Minimum = 0;
            //dialog.Maximum = numFiles;
            int progress = 0;
            // set base dialog message
            string strBase = "Scanning: ";


            // create new final list to be populated with approved files
            List<Game> finalGames = new List<Game>();

            

            // now we have a list of allowed files, loop through them
            foreach (string file in allowedFiles)
            {
                // get the relative path
                string relPath = PathUtil.GetRelativePath(romFolderPath, file);
                // get just the filename
                string fileName = System.IO.Path.GetFileName(file);
                // get just the extension
                string extension = System.IO.Path.GetExtension(file).ToLower();
                // get rom name wihout extension
                string romName = fileName.Replace(extension, "");

                // update UI
                progress++;
                string uiUpdate = strBase + fileName + "\n(" + progress + " of " + numFiles + ")";
                dialog.SetMessage(uiUpdate);
                //dialog.SetProgress(progress);

                Game newGame = new Game();
                string hash = String.Empty;

                // inspect archive files
                if (extension == ".zip" || extension == ".7z")
                {
                    bool isAllowed = false;
                    try
                    {
                        Archiving arch = new Archiving(file, systemId);
                        arch.ProcessArchive();
                        hash = arch.MD5Hash;
                        isAllowed = arch.IsAllowed;
                        if (hash == null)
                        {
                            continue;
                        }
                    }
                    catch (System.IO.InvalidDataException ex)
                    {
                        // problem with the archive file
                    }
                    finally { }
                    
                    if (isAllowed == false) { continue; }
                }
                else
                {
                    // file is not an archive - calculate md5
                    //hash = Crypto.Crc32.ComputeFileHash(file);
                    hash = Crypto.checkMD5(file);
                }
               

                // check whether game already exists (by gameName and systemId)
                Game chkGame = (from g in Games
                                where g.systemId == systemId && g.gameName == romName
                                select g).FirstOrDefault();

                // lookup game in master dat
                //var sysFilter = DAT.Where(p => p.SystemId == systemId);
                // var lookup = DAT.Where(p => p.Roms.Any(x => x.MD5.ToUpper().Trim() == hash.ToUpper().Trim())).ToList();

                //var lookup = DAT.Where(p => p.Roms.Any(x => x.MD5.ToUpper() == hash)).ToList();
                string nHash = hash.ToUpper().Trim().ToString();
                List<DATMerge> lookup = (from i in DAT
                              where i.SystemId == systemId && i.Roms.Any(l => l.MD5.ToUpper().Trim() == hash)
                                         select i).ToList();

                if (chkGame == null)
                {
                    // does not already exist - create new game
                    newGame.configId = 1;

                    if (lookup != null && lookup.Count > 0)
                    {
                        newGame.gameNameFromDAT = lookup.First().GameName;
                        newGame.Publisher = lookup.First().Publisher;
                        newGame.Year = lookup.First().Year;

                        // get rom we are interested in
                        var rom = (from ro in lookup.First().Roms
                                   where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                                   select ro).First();
                        newGame.romNameFromDAT = rom.RomName;
                        newGame.Copyright = rom.Copyright;
                        newGame.Country = rom.Country;
                        newGame.DevelopmentStatus = rom.DevelopmentStatus;
                        newGame.Language = rom.Language;
                        newGame.OtherFlags = rom.OtherFlags;

                        if (rom.Year != null && rom.Year != "")
                        {
                            newGame.Year = rom.Year;
                        }
                        if (rom.Publisher != null && rom.Publisher != "")
                        {
                            newGame.Publisher = rom.Publisher;
                        }
                        
                    }

                    newGame.gameName = romName;
                    newGame.gamePath = relPath;
                    newGame.hidden = false;
                    newGame.isDiskBased = false;
                    newGame.isFavorite = false;
                    newGame.systemId = systemId;
                    newGame.CRC32 = hash;

                    // add to finaGames list
                    RomsToAdd.Add(newGame);
                    // increment the added counter
                    AddedStats++;
                }
                else
                {
                    // matching game found - update it
                    if (chkGame.gamePath == relPath && chkGame.hidden == false && chkGame.CRC32 == hash)
                    {
                        //nothing to update - increment untouched counter
                        UntouchedStats++;
                    }
                    else
                    {
                        newGame = chkGame;
                        // update path in case it has changed location
                        newGame.gamePath = relPath;
                        // mark as not hidden
                        newGame.hidden = false;

                        newGame.CRC32 = hash;
                        if (lookup != null && lookup.Count > 0)
                        {
                            newGame.gameNameFromDAT = lookup.First().GameName;
                            newGame.Publisher = lookup.First().Publisher;
                            newGame.Year = lookup.First().Year;

                            // get rom we are interested in
                            var rom = (from ro in lookup.First().Roms
                                       where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                                       select ro).First();
                            newGame.romNameFromDAT = rom.RomName;
                            newGame.Copyright = rom.Copyright;
                            newGame.Country = rom.Country;
                            newGame.DevelopmentStatus = rom.DevelopmentStatus;
                            newGame.Language = rom.Language;
                            newGame.OtherFlags = rom.OtherFlags;

                            if (rom.Year != null && rom.Year != "")
                            {
                                newGame.Year = rom.Year;
                            }
                            if (rom.Publisher != null && rom.Publisher != "")
                            {
                                newGame.Publisher = rom.Publisher;
                            }
                        }

                        // add to finalGames list
                        RomsToUpdate.Add(newGame);
                        // increment updated counter
                        UpdatedStats++;
                    }

                    // remove game from presentGames list - remaining games in this list will be marked as hidden at the end
                    presentGames.Remove(chkGame);                

                }
            }   
            
            // whatever games are left in the presentGames list should be marked as hidden as they have not been found
            if (presentGames.Count > 0)
            {
                foreach (Game g in presentGames)
                {
                    g.hidden = true;
                    RomsToUpdate.Add(g);
                    
                }
            }

            GameListBuilder.UpdateFlag();

        }

        public void SaveToDatabase()
        {
            using (var ndb = new MyDbContext())
            {
                db.AddRange(RomsToAdd);
                db.UpdateRange(RomsToUpdate);

                db.AddRange(DisksToAdd);
                db.UpdateRange(DisksToUpdate);

                db.SaveChanges();

                GamesLibData.ForceUpdate();
            }
                
               
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

        // get a list of files from a directory and sub-directory (based on usingRecursion bool)
        public static System.Collections.Generic.IEnumerable<string> GetFiles(string path, bool usingRecursion)
        {
            if (usingRecursion == true)
            {
                //MessageBoxResult result = MessageBox.Show(path);

                // check first whether directory exists
                if (Directory.Exists(@path))
                {
                    var files = Directory.GetFiles(@path, "*.*", SearchOption.AllDirectories);
                    return files;
                }
                else
                {
                    // directory no longer exists - return null
                    return null;
                }            
                
            }
            else
            {
                var files = Directory.GetFiles(@path, "*.*");
                return files;
            }
            
        }

        // mark all ROMS from a system as hidden (as long as it is not a disk based game)
        public void MarkAllRomsAsHidden(int systemId)
        {
            List<Game> gamesAll = (from g in Games
                                where g.systemId == systemId
                               select g).ToList();
            List<Game> games = (from g in gamesAll
                                   where g.isDiskBased == false
                                   select g).ToList();

            if (games == null)
            {
                // no games found
            }
            else
            {
                // iterate through each game
                foreach (Game game in games)
                {
                    Game newGame = game;
                    if (newGame.hidden == false)
                    {
                        newGame.hidden = true;
                        // add to GamesToUpdate to be processed later
                        RomsToUpdate.Add(newGame);
                        HiddenStats++;
                        GameListBuilder.UpdateFlag();

                    }
                    else
                    {
                        // game is already marked as hidden
                        UntouchedStats++;
                    }
                }
            }            
        }
        
        // mark single game as hidden
        public void MarkRomAsHidden(int gameId)
        {
            Game game = (from g in Games
                                where g.gameId == gameId
                                select g).ToList().SingleOrDefault();
            if (game == null)
            {
                // no game found
            }
            else
            {                
                Game newGame = game;
                if (newGame.hidden == false)
                {
                    newGame.hidden = true;
                    // add to GamesToUpdate to be processed later
                    RomsToUpdate.Add(newGame);
                    HiddenStats++;
                    GameListBuilder.UpdateFlag();
                } 
                else
                {
                    // game is already hidden
                    UntouchedStats++;
                }               
            }
        }





        public static List<GSystem> GetSystems()
        {
            List<GSystem> systems = new List<GSystem>();
            using (var sysCon = new MyDbContext())
            {
                var sys = GSystem.GetSystems();
                foreach (GSystem g in sys)
                {
                    systems.Add(g);
                }
                return systems;
            }
        }


        // attempt to add game to Game database
        public static int AddGame(Rom systemRom, string fullPath, string relPath, string fileName, string extension, string romName)
        {
            // check whether ROM already exists in database
            using (var romContext = new MyDbContext())
            {
                var rom = (from r in romContext.Game
                          where (r.gameName == romName) && (r.systemId == systemRom.gameSystem.systemId)
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
                        GameListBuilder.UpdateFlag();
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
                    GameListBuilder.UpdateFlag();
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
                GameListBuilder.UpdateFlag();
                romaContext.Dispose();
            }
        }
        public void BeginManualImport(int sysId)
        {
            // Start manual import process for a game based on sysId
            DiskGameFile gameFile = SelectGameFile(sysId);
            if (gameFile == null)
            {
                MessageBox.Show("No valid file was selected", "MedLaunch: Error");
                return;
            }
            else
            {
                // Add or update the returned GameFile to the database
                InsertOrUpdateDisk(gameFile, sysId);
                SaveToDatabase();
                MessageBox.Show("Game: " + gameFile.FileName + " has added to (or updated in) the library", "MedLaunch: Import or Update Completed");
                GameListBuilder.UpdateFlag();
            }          

        }

        public DiskGameFile SelectGameFile(int sysId)
        {
            // get allowed file types for this system
            List<string> exts = (GetAllowedFileExtensions(sysId)).ToList();

            // convert allowed types to filter string
            string filter = "";
            string fStart = "Allowed Types (";
            string fEnd = "";
            foreach (string i in exts)
            {
                if (i == "") { continue; }
                    
                fStart += "*" + i + ",";
                fEnd += "*" + i + ";";
            }
            char comma = ',';
            char semi = ';';
            filter = (fStart.TrimEnd(comma)) + ")|" + (fEnd.TrimEnd(semi));
            //MessageBox.Show(filter);

            // open the file dialog showing only allowed file types - multi-select enabled
            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Multiselect = true;
            filePath.Filter = filter;
            filePath.Title = "Select a single (or multiple in the same directory) .cue or .ccd or .toc file(s)";
            filePath.ShowDialog();

            if (filePath.FileNames.Length > 0)
            {
                // file(s) have been selected
                List<string> files = filePath.FileNames.ToList();

                // single or multiple files?
                if (files.Count > 1)
                {
                    // Create a list of GameFile Objects to process
                    List<DiskGameFile> games = new List<DiskGameFile>();

                    // iterate through each game
                    foreach (string game in files)
                    {
                        // Create a new DiskGameFile instance with all path details
                        DiskGameFile g = new DiskGameFile(game, sysId);
                        // add to list
                        games.Add(g);
                    }

                    // process the list and create an m3u playlist file - all selected files have to be in the same directory
                    List<DiskGameFile> ordered = (from a in games
                                   select a).OrderBy(a => a.FileName).ToList();

                    // check whether an m3u playlist file already exists
                    var firstEntry = (from a in ordered
                                       select a).First();

                    // create string for the new m3u path
                    string m3uPath = firstEntry.FolderPath + "\\" + firstEntry.GameName + ".m3u";
                    //MessageBox.Show(m3uPath);

                    // create GameFIle object for the m3u playlist
                    DiskGameFile mf = new DiskGameFile(m3uPath, sysId);

                    // Attempt M3U creation
                    bool create = CreateM3uPlaylist(ordered, m3uPath);

                    if (create == false)
                    {
                        // user cancelled import, return null
                        return null;
                    }
                    else
                    {
                        // method returned true - begin to import m3u to games library
                        return mf;                       
                    }
                }
                else
                {
                    // single file selected - create GameFile object and return it
                    DiskGameFile g = new DiskGameFile(files[0], sysId, true);
                    return g;
                }
            }
            else
            {
                // no files selected - return empty string
                return null;
            }
        }

        public void InsertOrUpdateDisk(DiskGameFile f, int sysId)
        {
            // check whether game already exists (by gameName and systemId)
            Game chkGame = (from g in Games
                            where g.systemId == sysId && g.gameName == f.GameName
                            select g).SingleOrDefault();

            // create new Game object for import
            Game newGame = new Game();

            string hash = string.Empty;

            // calculate MD5 checksum hashes            
            if (f.Extension.Contains("m3u"))
            {
                // this is an m3u playlist - inspect and get relevant cue files
                string[] lines = File.ReadAllLines(f.FullPath);

                if (lines.Length > 0)
                {
                    // get hash for first cue/toc/ccd
                    hash = Crypto.checkMD5(f.FolderPath + "\\" + lines[0]);
                }
            }
            else
            {
                hash = Crypto.checkMD5(f.FullPath);
            }

                 

            // lookup md5 in master DAT
            List<DATMerge> lookup = (from i in DAT
                                     where i.SystemId == sysId && i.Roms.Any(l => l.MD5.ToUpper().Trim() == hash)
                                     select i).ToList();

            if (chkGame == null)
            {
                // does not already exist - create new game
                newGame.configId = 1;
                newGame.gameName = f.GameName;
                newGame.gamePath = f.FullPath;
                newGame.hidden = false;
                newGame.isDiskBased = true;
                newGame.isFavorite = false;
                newGame.systemId = sysId;

                if (lookup != null && lookup.Count > 0)
                {
                    newGame.gameNameFromDAT = lookup.First().GameName;
                    newGame.Publisher = lookup.First().Publisher;
                    newGame.Year = lookup.First().Year;

                    // get rom we are interested in
                    var rom = (from ro in lookup.First().Roms
                               where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                               select ro).First();
                    newGame.romNameFromDAT = rom.RomName;
                    newGame.Copyright = rom.Copyright;
                    newGame.Country = rom.Country;
                    newGame.DevelopmentStatus = rom.DevelopmentStatus;
                    newGame.Language = rom.Language;
                    newGame.OtherFlags = rom.OtherFlags;

                    if (rom.Year != null && rom.Year != "")
                    {
                        newGame.Year = rom.Year;
                    }
                    if (rom.Publisher != null && rom.Publisher != "")
                    {
                        newGame.Publisher = rom.Publisher;
                    }

                }

                // add to finaGames list
                DisksToAdd.Add(newGame);
                // increment the added counter
                AddedStats++;
                GameListBuilder.UpdateFlag();
            }
            else
            {
                // matching game found - update it
                if ((chkGame.gamePath == f.FullPath || chkGame.gamePath == f.FullPath) && chkGame.hidden == false)
                {
                    //nothing to update - Path is either identical either absoultely or relatively (against the systemPath set in the database)
                    UntouchedStats++;
                }
                else
                {
                    newGame = chkGame;
                    // update path in case it has changed location
                    newGame.gamePath = f.FullPath;
                    // mark as not hidden
                    newGame.hidden = false;
                    newGame.isDiskBased = true;

                    if (lookup != null && lookup.Count > 0)
                    {
                        newGame.gameNameFromDAT = lookup.First().GameName;
                        newGame.Publisher = lookup.First().Publisher;
                        newGame.Year = lookup.First().Year;

                        // get rom we are interested in
                        var rom = (from ro in lookup.First().Roms
                                   where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                                   select ro).First();
                        newGame.romNameFromDAT = rom.RomName;
                        newGame.Copyright = rom.Copyright;
                        newGame.Country = rom.Country;
                        newGame.DevelopmentStatus = rom.DevelopmentStatus;
                        newGame.Language = rom.Language;
                        newGame.OtherFlags = rom.OtherFlags;

                        if (rom.Year != null && rom.Year != "")
                        {
                            newGame.Year = rom.Year;
                        }
                        if (rom.Publisher != null && rom.Publisher != "")
                        {
                            newGame.Publisher = rom.Publisher;
                        }

                    }

                    // add to finalGames list
                    DisksToUpdate.Add(newGame);
                    // increment updated counter
                    UpdatedStats++;
                    GameListBuilder.UpdateFlag();
                }

            }
        }

        public bool CreateM3uPlaylist(List<DiskGameFile> files, string m3uPath)
        {
            // does the file already exist
            if (!File.Exists(m3uPath))
            {
                // file does not exist - create file and populate
                using (StreamWriter sw = File.CreateText(m3uPath))
                {
                    foreach (var f in files)
                    {
                        sw.WriteLine(f.FileName);
                    }
                }
                return true;
            }
            else
            {
                // file already exists - check whether we want to overwrite or not
                MessageBoxResult result = MessageBox.Show("File Name: " + Path.GetFileName(m3uPath) + "\n\nDo you want to replace this file?\n(Yes overwrites, No uses existing file, Cancel aborts the import process)", "M3U Playlist File Already Exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    // dont overwrite, just return
                    return true;
                }
                if (result == MessageBoxResult.Cancel)
                {
                    // cancel import process
                    return false;
                }
                if (result == MessageBoxResult.Yes)
                {
                    // overwrite

                    // clear file first
                    File.Create(m3uPath).Close();

                    // create and populate file
                    using (StreamWriter sw = File.CreateText(m3uPath))
                    {
                        foreach (var f in files)
                        {
                            sw.WriteLine(f.FileName);
                        }
                    }
                    return true;
                }
                return false;
            }
        }

    }
}
