using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReleaseGenerator
{

    public class Release
    {
        public string Version { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public List<string> Changelog { get; set; }
    }
}
