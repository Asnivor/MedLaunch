using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.ScrapeDB
{
    public class GDB_Game
    {
        public int gid { get; set; }
        public string gameTitle { get; set; }
        public string releaseYear { get; set; }
        public int pid { get; set; }

        /// <summary>
        /// return list of all gdb games
        /// </summary>
        /// <returns></returns>
        public static List<GDB_Game> GetGames()
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.GDB_Game
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all gdb games for a specific platform
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public static List<GDB_Game> GetGames(int platformId)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.GDB_Game
                             where g.pid == platformId
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return single gdb game based on gid
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static GDB_Game GetGame(int gid)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.GDB_Game
                             where g.gid == gid
                             select g).FirstOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// add a single game entry
        /// </summary>
        /// <param name="game"></param>
        public static void AddGame(GDB_Game game)
        {
            using (var aG = new AsniScrapeAdminDbContext())
            {
                aG.GDB_Game.Add(game);
                aG.SaveChanges();
                aG.Dispose();
            }
        }

        /// <summary>
        /// update a single game entry
        /// </summary>
        /// <param name="game"></param>
        public static void UpdateGame(GDB_Game game)
        {
            using (var uG = new AsniScrapeAdminDbContext())
            {
                uG.GDB_Game.Update(game);
                uG.SaveChanges();
                uG.Dispose();
            }
        }

        /// <summary>
        /// save list of games to database (add or update logic included)
        /// </summary>
        /// <param name="games"></param>
        public static void SaveToDatabase(List<GDB_Game> games)
        {
            // list to hold junction table info
            List<int> jIds = new List<int>();

            using (var db = new AsniScrapeAdminDbContext())
            {
                // get current database context
                var current = db.GDB_Game.AsNoTracking().ToList();

                List<GDB_Game> toAdd = new List<GDB_Game>();
                List<GDB_Game> toUpdate = new List<GDB_Game>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    jIds.Add(g.gid);

                    GDB_Game t = (from a in current
                              where a.gid == g.gid
                              select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.GDB_Game.UpdateRange(toUpdate);
                db.GDB_Game.AddRange(toAdd);
                db.SaveChanges();
            }

            // now update the junction table if neccesary
            Junction.SaveToDatabaseNoUpdate(jIds);
        }
    }
}
