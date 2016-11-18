using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class LibraryDataGDBLink
    {
        public int GDBId { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Year { get; set; }
        public string Players { get; set; }
        public string Coop { get; set; }
        public string ESRB { get; set; }


        public static LibraryDataGDBLink GetLibraryData(int gamesdbID)
        {
            using (var db = new MyDbContext())
            {
                var c = db.LibraryDataGDBLink.Where(a => a.GDBId == gamesdbID).FirstOrDefault();
                return c;
            }
        }
    }
}
