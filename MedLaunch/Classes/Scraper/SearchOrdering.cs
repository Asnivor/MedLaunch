using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper
{
    public class SearchOrdering
    {
        public int Matches { get; set; }
        public ScraperMaster Game { get; set; }
    }

    public class MobySearchOrdering
    {
        public int Matches { get; set; }
        public MobyPlatformGame Game { get; set; }
    }
}
