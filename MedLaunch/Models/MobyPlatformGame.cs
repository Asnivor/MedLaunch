using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class MobyPlatformGame
    {
        public string Title { get; set; }
        public string UrlName { get; set; }
        public int SystemId { get; set; }
        public string PlatformName { get; set; }
        


        // return single game based on UrlName
        public static MobyPlatformGame GetGame(string UrlName)
        {

            using (var db = new MyDbContext())
            {
                /*
                MobyPlatformGame g = (from a in db.MobyPlatformGame
                                      where a.UrlName == UrlName
                                      select a).FirstOrDefault();
                                       */
                return new MobyPlatformGame();
               
            }
        }

        // return all games
        public static List<MobyPlatformGame> GetGames()
        {
            using (var db = new MyDbContext())
            {
                /*
                List<MobyPlatformGame> g = (from a in db.MobyPlatformGame
                                            select a).ToList();
                                            */
                return new List<MobyPlatformGame>();

            }
        }

        // upsert list of games to database
        public static int[] SaveToDatabase(List<MobyPlatformGame> games)
        {
            using (var db = new MyDbContext())
            {

                int added = 0;
                int updated = 0;

                /*

                // get current database context
                var current = db.MobyPlatformGame.AsNoTracking().ToList();

                List<MobyPlatformGame> toAdd = new List<MobyPlatformGame>();
                List<MobyPlatformGame> toUpdate = new List<MobyPlatformGame>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    
                    MobyPlatformGame t = (from a in current
                                          where (a.PlatformName == g.PlatformName && a.UrlName == g.UrlName)
                                          select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); added++; }
                    else
                    {
                       
                        toUpdate.Add(g);
                        updated++;
                    }
                }
                db.MobyPlatformGame.UpdateRange(toUpdate);
                db.MobyPlatformGame.AddRange(toAdd);
                db.SaveChanges();

                //MessageBox.Show(added + " added, " + updated + " updated.");
                */
                return new int[] { added, updated };
            }

        }
    }
}
