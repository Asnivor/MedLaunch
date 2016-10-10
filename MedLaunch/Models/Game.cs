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
