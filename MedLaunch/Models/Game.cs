using MedLaunch.Classes;
using MedLaunch.Classes.GamesLibrary;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Models
{
    public class Game
    {
        public int gameId { get; set; }
        public string gamePath { get; set; }
        public string gameName { get; set; }
        public string archiveGame { get; set; }
        public string gameNameFromDAT { get; set; }
        public string romNameFromDAT { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string DevelopmentStatus { get; set; }
        public string Copyright { get; set; }
        public string OtherFlags { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Year { get; set; }
        public DateTime gameLastPlayed { get; set; }
        public DateTime gameLastFinished { get; set; }
        public int timesPlayed { get; set; }
        public double gameTime { get; set; }
        public int systemId { get; set; }
        //public GSystem GameSystem { get; set; }
        public bool isFavorite { get; set; }
        public int configId { get; set; }
        public bool hidden { get; set; }
        public string disks { get; set; }
        public bool isDiskBased { get; set; }
        public bool? isScraped { get; set; }
        public int? gdbId { get; set; }
        public string CRC32 { get; set; }

        public static Game GetGame(int gameId)
        {
            using (var context = new MyDbContext())
            {
                var cData = (from g in context.Game
                             where g.gameId == gameId
                             select g).SingleOrDefault();
                return cData;
            }
        }

        public static List<Game> GetGames(int systemId)
        {
            using (var context = new MyDbContext())
            {
                var cData = (from g in context.Game
                             where g.systemId == systemId
                             select g);
                return cData.ToList();
            }
        }

        public static List<Game> GetGames()
        {
            using (var context = new MyDbContext())
            {
                var cData = (from g in context.Game
                             select g);
                return cData.ToList();
            }
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
                    //GameListBuilder.UpdateFlag();
                }
            }
        }

        // remove all roms from db for a certain system
        public static void RemoveRoms(int sysId)
        {
            MessageBoxResult result = MessageBox.Show("This operation will wipe out ALL the " + GSystem.GetSystemName(sysId) + " games in your library database (but they will not be deleted from disk)\n\nAre you sure you wish to continue?", "WARNING", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                // get all roms for specific system
                using (var context = new MyDbContext())
                {
                    List<Game> roms = (from a in context.Game
                                       where a.systemId == sysId
                                       select a).ToList();
                    Game.DeleteGames(roms);
                    //GameListBuilder.UpdateFlag();
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
                   // GameListBuilder.UpdateFlag();
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
                //GameListBuilder.UpdateFlag();
                romaContext.Dispose();
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
                        //Debug.WriteLine("FAVOTIE!");
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
                        //GameListBuilder.UpdateFlag();
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
                    //GameListBuilder.UpdateFlag();
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


        public static void SaveToDatabase(List<Game> games)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.Game.AsNoTracking().ToList();

                List<Game> toAdd = new List<Game>();
                List<Game> toUpdate = new List<Game>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    Game t = (from a in current
                                     where a.gameId == g.gameId
                                     select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.Game.UpdateRange(toUpdate);
                db.Game.AddRange(toAdd);
                db.SaveChanges();
               // GameListBuilder.UpdateFlag();
            }
        }

        public static void SaveToDatabase(List<Game> games, bool init)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.Game.AsNoTracking().ToList();

                List<Game> toAdd = new List<Game>();
                List<Game> toUpdate = new List<Game>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    Game t = (from a in current
                              where a.gameId == g.gameId
                              select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.Game.UpdateRange(toUpdate);
                db.Game.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        public static void SetStartedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastPlayed = DateTime.Now;
            
            SetGame(game);
           // GameListBuilder.UpdateFlag();
        }
        public static void SetFinishedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastFinished = DateTime.Now;
            SetGame(game);

            SetTotalGameTime(gameId);
           // GameListBuilder.UpdateFlag();
        }

        public static void SetTotalGameTime(int gameId)
        {
            Game game = GetGame(gameId);
            double currentTotalTime = game.gameTime;
            TimeSpan ts = game.gameLastFinished - game.gameLastPlayed;

            if (ts.TotalMinutes < 0.05)
            {
                // game time was negative (so maybe there was a crash) or less than 3 seconds (so maybe game didnt launch correctly)
            }
            else
            {
                // this looks correct - add it to the currentTotalTime and update the database
                double newTotalTime = currentTotalTime + ts.TotalMinutes;
                game.gameTime = newTotalTime;
                game.timesPlayed++;
                SetGame(game);
               // GameListBuilder.UpdateFlag();
            }
        }


        public static void SetGame(Game game)
        {
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(game).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
                GamesLibData.ForceUpdate();
               // GameListBuilder.UpdateFlag();
            }
        }

        public static void SetGame(Game game, bool init)
        {
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(game).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
            }
        }

        public static void DeleteGame(Game game)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.Remove(game);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
                //GameListBuilder.UpdateFlag();
            }

        }

        public static void DeleteGames(List<Game> games)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.RemoveRange(games);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
               // GameListBuilder.UpdateFlag();
            }
        }

        public static void SetGdbId(int GameId, int GdbId)
        {
            Game game = GetGame(GameId);
            game.gdbId = GdbId;
            SetGame(game);
           // GameListBuilder.UpdateFlag();
        }
    }    
}
