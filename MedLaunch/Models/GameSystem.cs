using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class GameSystem
    {
        public int systemId { get; set; }
        public string systemCode { get; set; }
        public string systemName { get; set; }
        public string systemDescription { get; set; }
        public List<Game> Games { get; set; }
    }
}
