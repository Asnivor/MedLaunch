using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.ScrapeDB
{
    public class MOBY_Platform
    {
        public int pid { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public string listURL { get; set; }

        /// <summary>
        /// return a list with all moby platforms
        /// </summary>
        /// <returns></returns>
        public static List<MOBY_Platform> GetPlatforms()
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.MOBY_Platform
                             select g);
                return cData.ToList();
            }
        }
    }
}
