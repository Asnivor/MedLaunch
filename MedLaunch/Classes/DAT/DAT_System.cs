using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.DAT
{
    public class DAT_System
    {
        public int pid { get; set; }
        public string platformName { get; set; }


        /// <summary>
        /// return a list with all gamesdb.net platforms
        /// </summary>
        /// <returns></returns>
        public static List<DAT_System> GetSystems()
        {
            using (var context = new AsniDATDbContext())
            {
                var cData = (from g in context.DAT_System
                             select g);
                return cData.ToList();
            }
        }
    }
}
