using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class SearchOrdering
    {
        public int Matches { get; set; }
        public GDBPlatformGame Game { get; set; }
    }

    public class MobySearchOrdering
    {
        public int Matches { get; set; }
        public MobyPlatformGame Game { get; set; }
    }
}
