using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public class GameListItem
    {
        public int GamesDBId { get; set; }
        public string GameName { get; set; }
        public int Matches { get; set; }
        public int Percentage { get; set; }
        public string Platform { get; set; }
    }
}
