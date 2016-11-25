using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.TOSEC.Models
{
    public class ToSecObject
    {
        public int SystemId { get; set; }
        public string Name { get; set; }
        public string RomName { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string CRC { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Copyright { get; set; }
        public string DevelopmentStatus { get; set; }
        public string OtherFlags { get; set; }
    }
}
