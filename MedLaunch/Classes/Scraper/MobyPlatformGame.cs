using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper
{
    public class MobyPlatformGame
    {
        public string Title { get; set; }
        public string UrlName { get; set; }
        public int SystemId { get; set; }
        public string PlatformName { get; set; }
    }
}
