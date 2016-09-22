using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class Game
    {
        public int gameId { get; set; }
        public string gamePath { get; set; }
        public string gameName { get; set; }
        public DateTime gameLastPlayed { get; set; }
        public int systemId { get; set; }
        public GameSystem GameSystem { get; set; }
        public bool isFavorite { get; set; }
        public int configId { get; set; }
        public bool hidden { get; set; }
    }
}
