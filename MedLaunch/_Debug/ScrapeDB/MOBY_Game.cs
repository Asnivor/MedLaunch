using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.ScrapeDB
{
    public class MOBY_Game
    {
        public int mid { get; set; }
        public string gameTitle { get; set; }
        public string alias { get; set; }
        public string releaseYear { get; set; }
        public int pid { get; set; }

        /// <summary>
        /// return single mobygame entry based on alias name and platform id
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static MOBY_Game GetGame(string alias, int pid)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.MOBY_Game
                             where g.alias == alias && g.pid == pid
                             select g).FirstOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// return list of all moby games
        /// </summary>
        /// <returns></returns>
        public static List<MOBY_Game> GetGames()
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.MOBY_Game
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all moby games for a specific platform
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public static List<MOBY_Game> GetGames(int platformId)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.MOBY_Game
                             where g.pid == platformId
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return single moby game based on mid
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static MOBY_Game GetGame(int mid)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.MOBY_Game
                             where g.mid == mid
                             select g).FirstOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// add a single game entry
        /// </summary>
        /// <param name="game"></param>
        public static void AddGame(MOBY_Game game)
        {
            using (var aG = new AsniScrapeAdminDbContext())
            {
                aG.MOBY_Game.Add(game);
                aG.SaveChanges();
                aG.Dispose();
            }
        }

        /// <summary>
        /// update a single game entry
        /// </summary>
        /// <param name="game"></param>
        public static void UpdateGame(MOBY_Game game)
        {
            using (var uG = new AsniScrapeAdminDbContext())
            {
                uG.MOBY_Game.Update(game);
                uG.SaveChanges();
                uG.Dispose();
            }
        }

        /// <summary>
        /// save list of games to database (add or update logic included)
        /// </summary>
        /// <param name="games"></param>
        public static void SaveToDatabase(List<MOBY_Game> games)
        {
            using (var db = new AsniScrapeAdminDbContext())
            {
                // get current database context
                var current = db.MOBY_Game.AsNoTracking().ToList();

                List<MOBY_Game> toAdd = new List<MOBY_Game>();
                List<MOBY_Game> toUpdate = new List<MOBY_Game>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    // games will generally be passed with mid = 0
                    if (g.mid > 0)
                    {
                        MOBY_Game t = (from a in current
                                       where (a.mid == g.mid)
                                       select a).SingleOrDefault();
                        if (t == null) { toAdd.Add(g); }
                        else { toUpdate.Add(g); }
                    }
                    else
                    {
                        MOBY_Game t = (from a in current
                                       where (a.alias == g.alias && a.pid == g.pid)
                                       select a).SingleOrDefault();
                        if (t == null) { toAdd.Add(g); }
                        else
                        {
                            MOBY_Game m = t;
                            m.gameTitle = g.gameTitle;
                            m.alias = g.alias;
                            m.pid = g.pid;
                            m.releaseYear = g.releaseYear;
                            toUpdate.Add(m);
                        }
                    }
                    
                }
                db.MOBY_Game.UpdateRange(toUpdate);
                db.MOBY_Game.AddRange(toAdd);
                db.SaveChanges();
            }
        }

    }


}
