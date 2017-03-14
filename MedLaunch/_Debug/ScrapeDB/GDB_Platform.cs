using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace MedLaunch._Debug.ScrapeDB
{
    public class GDB_Platform
    {
        public int pid { get; set; }
        public string name { get; set; }
        public string alias { get; set; }

        /// <summary>
        /// return a list with all gamesdb.net platforms
        /// </summary>
        /// <returns></returns>
        public static List<GDB_Platform> GetPlatforms()
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.GDB_Platform
                             select g);
                return cData.ToList();
            }
        }
    }
}
