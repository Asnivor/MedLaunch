using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.Models
{
    public class DATMerge1
    {
        public int SystemId { get; set; }
        public string GameName { get; set; }
        public List<RomEntry1> Roms { get; set; }
        public string Year { get; set; }        
        public string Publisher { get; set; }
        public int GdbId { get; set; }

        public DATMerge1()
        {
            Roms = new List<RomEntry1>();
        }
    }

    public class RomEntry1
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
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string FromDAT { get; set; }
    }
}
    
