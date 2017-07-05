using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.PSXDATACENTER
{
    public class ImportPsxDataCenterdata
    {
        /// <summary>
        /// process psxdatacenter DB for every system
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> Go()
        {
            // create empty list
            List<DAT_Rom> l = new List<DAT_Rom>();

            // get all psx games from debug database
            List<PSX_Games> games = PSX_Games.GetGames();

            // iterate through each game and parse the information into a new object
            foreach (var g in games)
            {
                DAT_Rom dr = new DAT_Rom();
                dr.country = g.region;
                dr.datProviderId = 5;
                dr.developer = g.developer;
                dr.language = g.languages;
                dr.pid = 10;
                dr.publisher = g.publisher;
                dr.year = g.year;
                dr.otherFlags = g.serial;

                // names
                dr.romName = g.name;
                dr.name = g.name
                    .Replace("[Disc 1]", "")
                    .Replace("[Disc 2]", "")
                    .Replace("[Disc 3]", "")
                    .Replace("[Disc 4]", "")
                    .Replace("[Disc 5]", "")
                    .Replace("[Disc 6]", "")
                    .Replace("[Disc 7]", "")
                    .Replace("[Disc 8]", "")
                    .Replace("[Disc 9]", "")
                    .Replace("[Disc 10]", "")
                    .Trim();

                l.Add(dr);
            }
            
            l.Distinct();
            return l;
        }

        
    }
}
