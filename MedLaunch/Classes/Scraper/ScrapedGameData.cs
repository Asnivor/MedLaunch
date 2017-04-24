using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper
{
    public class ScrapedGameData
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public List<string> AlternateTitles { get; set; }
        public List<string> Genres { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Coop { get; set; }
        public string ESRB { get; set; }
        public string Players { get; set; }
        public string Released { get; set; }
        public string Platform { get; set; }
    }

    
}
