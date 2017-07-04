using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.PSXDATACENTER
{
    public class PSX_Games
    {
        public int id { get; set; }
        public string name { get; set; }
        public string serial { get; set; }
        public string region { get; set; }
        public string languages { get; set; }
        public string infoUrl { get; set; }
        public string year { get; set; }
        public string publisher { get; set; }
        public string developer { get; set; }

        // <summary>
        /// return list of all  games
        /// </summary>
        /// <returns></returns>
        public static List<PSX_Games> GetGames()
        {
            using (var context = new PsxDataCenterAdminDbContext())
            {
                var cData = (from g in context.PSX_Games
                             select g);
                return cData.ToList();
            }
        }

        

        public static int[] SaveToDatabase(List<PSX_Games> games)
        {
            // get current rom list
            List<PSX_Games> current = PSX_Games.GetGames();

            int added = 0;
            int updated = 0;

            // create temp objects pre-database actions
            List<PSX_Games> toAdd = new List<PSX_Games>();
            List<PSX_Games> toUpdate = new List<PSX_Games>();

            // iterate through each incoming rom
            foreach (var g in games)
            {
                // attempt rom lookup in current
                var lo = (from a in current
                              where 
                              a.serial == g.serial
                              select a).ToList();

                PSX_Games l = new PSX_Games();

                if (lo.Count == 1)
                    l = lo.Single();

                else
                {
                    // more than one serial number result - some multi-disc games share the same serial number
                    // match on name
                    l = lo.Where(a => a.name == g.name).FirstOrDefault();

                    if (l == null)
                    {
                        // give up
                        continue;
                    }
                }

                if (l == null)
                {
                    // no entry found
                    toAdd.Add(g);
                }
                else
                {
                    PSX_Games game = new PSX_Games();
                    game = l;
                    game.infoUrl = g.infoUrl;
                    game.name = g.name;
                    game.languages = g.languages;
                    game.region = g.region;

                    if (game.publisher == null)
                        game.publisher = g.publisher;
                    if (game.developer == null)
                        game.developer = g.developer;
                    if (game.year == null)
                        game.year = g.year;

                    // entry found

                    if (game != l)
                        toUpdate.Add(game);
                }
            }

            using (var db = new PsxDataCenterAdminDbContext())
            {
                // add new entries
                db.PSX_Games.AddRange(toAdd);

                // update existing entries
                db.PSX_Games.UpdateRange(toUpdate);


                db.SaveChanges();

                added = toAdd.Count();
                updated = toUpdate.Count();

                return new int[] { added, updated };
            }
        }

        public static int[] SaveToDatabase(PSX_Games g)
        {
            // get current rom list
            List<PSX_Games> current = PSX_Games.GetGames();

            int added = 0;
            int updated = 0;

            // create temp objects pre-database actions
            List<PSX_Games> toAdd = new List<PSX_Games>();
            List<PSX_Games> toUpdate = new List<PSX_Games>();

            
            // attempt rom lookup in current
            var lo = (from a in current
                        where
                        a.serial == g.serial
                        select a).ToList();

            PSX_Games l = new PSX_Games();

            if (lo.Count == 1)
                l = lo.Single();

            else
            {
                // more than one serial number result - some multi-disc games share the same serial number
                // match on name
                l = lo.Where(a => a.name == g.name).FirstOrDefault();

                if (l == null)
                {
                    // give up
                    return null;
                }
            }

            if (l == null)
            {
                // no entry found
                toAdd.Add(g);
            }
            else
            {
                PSX_Games game = new PSX_Games();
                game = l;
                game.infoUrl = g.infoUrl;
                game.name = g.name;
                game.languages = g.languages;
                game.region = g.region;

                if (game.publisher == null)
                    game.publisher = g.publisher;
                if (game.developer == null)
                    game.developer = g.developer;
                if (game.year == null)
                    game.year = g.year;

                // entry found
                
                toUpdate.Add(game);
            }
            

            using (var db = new PsxDataCenterAdminDbContext())
            {
                // add new entries
                db.PSX_Games.AddRange(toAdd);

                // update existing entries
                db.PSX_Games.UpdateRange(toUpdate);


                db.SaveChanges();

                added = toAdd.Count();
                updated = toUpdate.Count();

                return new int[] { added, updated };
            }
        }
    }
}
