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
        public int gdbid { get; set; }


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
    }
}
