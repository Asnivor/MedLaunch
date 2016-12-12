using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.Models
{
    public class DATMerge
    {
        public int SystemId { get; set; }
        public string GameName { get; set; }
        public List<RomEntry> Roms { get; set; }
        public string Year { get; set; }        
        public string Publisher { get; set; }  

        public DATMerge()
        {
            Roms = new List<RomEntry>();
        }
    }

    public class RomEntry
    {
        public string RomName { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string DevelopmentStatus { get; set; }
        public string OtherFlags { get; set; }
        public string CloneOf { get; set; }
        public string Copyright { get; set; }
        public string Size { get; set; }
        public string CRC { get; set; }
        public string MD5 { get; set; }
        public string SHA1 { get; set; }
    }
}
    
