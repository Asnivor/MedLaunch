using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class ScraperMaster
    {
        public int GamesDbId { get; set; }
        public int MedLaunchSystemId { get; set; }
        public ScraperTGDB TGDBData { get; set; }
        public ScraperMoby MobyData { get; set; }
        public List<string> IDDBManual { get; set; }

        public ScraperMaster()
        {
            TGDBData = new ScraperTGDB();
            MobyData = new ScraperMoby();
        }
    }

    public class ScraperTGDB
    {
        public string GamesDBTitle { get; set; }
        public string GamesDBPlatformName { get; set; }
    }

    public class ScraperMoby
    {
        public string MobyTitle { get; set; }
        public string MobyURLName { get; set; }
        public string MobyPlatformName { get; set; }
    }

    
}
