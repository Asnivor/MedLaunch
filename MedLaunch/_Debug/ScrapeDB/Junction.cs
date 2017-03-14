using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.ScrapeDB
{
    public class Junction
    {
        public int gid { get; set; }
        public int? mid { get; set; }

        public static Junction GetJunction(int gid)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.Junction
                             where g.gid == gid
                             select g).First();
                return cData;
            }
        }

        public static void UpdateToDatabase(Junction junction)
        {
            using (var db = new AsniScrapeAdminDbContext())
            {
                db.Junction.Update(junction);                
                db.SaveChanges();
            }
        }

        /// <summary>
        /// save list of junction entries to the database
        /// </summary>
        /// <param name="games"></param>
        public static void SaveToDatabase(List<Junction> games)
        {
            using (var db = new AsniScrapeAdminDbContext())
            {
                // get current database context
                var current = db.Junction.AsNoTracking().ToList();

                List<Junction> toAdd = new List<Junction>();
                List<Junction> toUpdate = new List<Junction>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    Junction t = (from a in current
                                  where a.gid == g.gid
                                  select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else
                    {

                        toUpdate.Add(g);
                    }
                }
                db.Junction.UpdateRange(toUpdate);
                db.Junction.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// save list of junction entries to the database
        /// </summary>
        /// <param name="games"></param>
        public static void SaveToDatabaseNoUpdate(List<Junction> games)
        {
            using (var db = new AsniScrapeAdminDbContext())
            {
                // get current database context
                var current = db.Junction.AsNoTracking().ToList();

                List<Junction> toAdd = new List<Junction>();
                List<Junction> toUpdate = new List<Junction>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    Junction t = (from a in current
                                  where a.gid == g.gid
                                  select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else
                    {
                        //toUpdate.Add(g);
                    }
                }
                //db.Junction.UpdateRange(toUpdate);
                db.Junction.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// save list of junction entries to the database
        /// </summary>
        /// <param name="games"></param>
        public static void SaveToDatabaseNoUpdate(List<int> gids)
        {
            using (var db = new AsniScrapeAdminDbContext())
            {
                // get current database context
                var current = db.Junction.AsNoTracking().ToList();

                List<Junction> toAdd = new List<Junction>();
                List<Junction> toUpdate = new List<Junction>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (int g in gids)
                {
                    Junction t = (from a in current
                                  where a.gid == g
                                  select a).SingleOrDefault();
                    if (t == null)
                    {
                        Junction j = new Junction();
                        j.gid = g;
                        toAdd.Add(j);
                    }
                    else
                    {
                        //toUpdate.Add(g);
                    }
                }
                //db.Junction.UpdateRange(toUpdate);
                db.Junction.AddRange(toAdd);
                db.SaveChanges();
            }
        }
    }    
}
