using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB
{
    public class DAT_Rom
    {
        public int rid { get; set; }
        public string romName { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string developmentStatus { get; set; }
        public string otherFlags { get; set; }
        public string cloneOf { get; set; }
        public string copyright { get; set; }
        public string size { get; set; }
        public string crc { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public string year { get; set; }
        public string publisher { get; set; }
        public int datProviderId { get; set; }

    }
}
