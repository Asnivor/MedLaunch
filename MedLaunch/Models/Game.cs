using MedLaunch.Classes;
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

        public static void SetStartedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastPlayed = DateTime.Now;
            
            SetGame(game);
        }
        public static void SetFinishedPlaying(int gameId)
        {
            Game game = GetGame(gameId);
            game.gameLastFinished = DateTime.Now;
            SetGame(game);

            SetTotalGameTime(gameId);
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
            }
        }


        public static void SetGame(Game game)
        {
            using (var cfDef = new MyDbContext())
            {
                cfDef.Entry(game).State = Microsoft.Data.Entity.EntityState.Modified;
                cfDef.SaveChanges();
                GamesLibData.ForceUpdate();
            }
        }

        public static void DeleteGame(Game game)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.Remove(game);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
            }

        }

        public static void DeleteGames(List<Game> games)
        {
            using (var cont = new MyDbContext())
            {
                cont.Game.RemoveRange(games);
                cont.SaveChanges();
                GamesLibData.ForceUpdate();
            }
        }
    }    
}
