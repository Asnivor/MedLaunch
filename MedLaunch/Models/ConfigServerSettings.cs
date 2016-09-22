using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class ConfigServerSettings
    {
        public int ConfigServerId { get; set; }
        public string ConfigServerDesc { get; set; }
        public String netplay__gamekey { get; set; }
        public String netplay__host { get; set; }
        public String netplay__password { get; set; }
        public int? netplay__port { get; set; }                      // 1 through 65535   

    }
}
