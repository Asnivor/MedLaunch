using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB
{
    public class DAT_Game
    {
        public int gid { get; set; }
        public string gameName { get; set; }
        public int pid { get; set; }
        public string year { get; set; }
        public string publisher { get; set; }
        public string developer { get; set; }
        public int? gdbid { get; set; }


        /// <summary>
        /// return list of all top level games
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Game> GetGames()
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Game
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all top level games based on platformid
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Game> GetGames(int pid)
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Game
                             where g.pid == pid
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return dat_game based on gid
        /// </summary>
        /// <returns></returns>
        public static DAT_Game GetGame(int gid)
        {
            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Game
                             where g.gid == gid
                             select g).FirstOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// return dat_game based on gid
        /// </summary>
        /// <returns></returns>
        public static DAT_Game GetGameByMD5(string md5)
        {
            DAT_Rom dr = DAT_Rom.GetRom(md5);
            if (dr == null)
                return null;

            using (var context = new AsniDATAdminDbContext())
            {
                var cData = (from g in context.DAT_Game
                             where g.gid == dr.gid
                             select g).FirstOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// Add a single game (without checking for existing)
        /// </summary>
        /// <param name="game"></param>
        public static void AddGame(DAT_Game game)
        {
            using (var db = new AsniDATAdminDbContext())
            {
                db.DAT_Game.Add(game);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// returns the next free GID
        /// </summary>
        /// <returns></returns>
        public static int GetFirstFreeGID()
        {
            using (var db = new AsniDATAdminDbContext())
            {
                int search = (from a in db.DAT_Game
                                   select a.gid).ToList().Last();

                if (search == 0)
                    return 0;

                int result = search + 1;
                return result;                            
                            
            }
        }

        /// <summary>
        /// Add a single game (without checking for existing) and return the gid
        /// </summary>
        /// <param name="game"></param>
        public static int AddGameReturnGID(DAT_Game game)
        {
            using (var db = new AsniDATAdminDbContext())
            {
                db.DAT_Game.Add(game);
                db.SaveChanges();
            }

            // get the last entry added
            var uGame = (from a in DAT_Game.GetGames()
                         where a.pid == game.pid
                         select a).LastOrDefault();

            if (uGame == null)
                return 0;

            return uGame.gid;
        }

        public static void UpdateGame(DAT_Game game)
        {
            using (var uG = new AsniDATAdminDbContext())
            {
                uG.DAT_Game.Update(game);
                uG.SaveChanges();
            }
        }

        public static int[] SaveToDatabase(List<DAT_Game> games)
        {
            // get current rom list
            List<DAT_Game> current = DAT_Game.GetGames();

            int added = 0;
            int updated = 0;

            // create temp objects pre-database actions
            List<DAT_Game> toAdd = new List<DAT_Game>();
            List<DAT_Game> toUpdate = new List<DAT_Game>();

            // iterate through each incoming rom
            foreach (var g in games)
            {
                // attempt rom lookup in current
                DAT_Game l = (from a in current
                              where a.gid == g.gid
                              select a).SingleOrDefault();

                if (l == null)
                {
                    // no entry found
                    toAdd.Add(g);
                }
                else
                {
                    // entry found
                    toUpdate.Add(g);
                }
            }

            using (var db = new AsniDATAdminDbContext())
            {
                // add new entries
                db.DAT_Game.AddRange(toAdd);

                // update existing entries
                db.DAT_Game.UpdateRange(toUpdate);
                

                db.SaveChanges();

                added = toAdd.Count();
                updated = toUpdate.Count();

                return new int[] { added, updated };
            }
        }
    }
}
