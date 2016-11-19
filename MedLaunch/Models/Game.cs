using MedLaunch.Classes;
using MedLaunch.Classes.GamesLibrary;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class Game
    {
        public int gameId { get; set; }
        public string gamePath { get; set; }
        public string gameName { get; set; }
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
                GameListBuilder.UpdateFlag();
            }
        }

        public static void SetStartedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastPlayed = DateTime.Now;
            
            SetGame(game);
            GameListBuilder.UpdateFlag();
        }
        public static void SetFinishedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastFinished = DateTime.Now;
            SetGame(game);

            SetTotalGameTime(gameId);
            GameListBuilder.UpdateFlag();
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
                GameListBuilder.UpdateFlag();
            }
        }


        public static void SetGame(Game game)
        {
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(game).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
                GamesLibData.ForceUpdate();
                GameListBuilder.UpdateFlag();
            }
        }

        public static void DeleteGame(Game game)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.Remove(game);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
                GameListBuilder.UpdateFlag();
            }

        }

        public static void DeleteGames(List<Game> games)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.RemoveRange(games);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
                GameListBuilder.UpdateFlag();
            }
        }
    }    
}
