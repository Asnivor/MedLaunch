using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class ConfigNetplaySettings
    {
        public int ConfigNPId { get; set; }
        public String netplay__console__font { get; set; }          // 5x7 6x9 6x12 6x13 9x18
        public int? netplay__console__lines { get; set; }            // 5 through 64
        public int? netplay__console__scale { get; set; }            // 0 through 16        
        public int? netplay__localplayers { get; set; }             // 0 through 16
        public String netplay__nick { get; set; }

    }
}
