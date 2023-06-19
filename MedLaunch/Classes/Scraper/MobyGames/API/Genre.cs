using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.MobyGames.API
{
    public class Genre
    {
        public string genre_category { get; set; }
        public int genre_category_id { get; set; }
        public int genre_id { get; set; }
        public string genre_name { get; set; }

        //"genre_category": "Basic Genres", 
        //  "genre_category_id": 1, 
        //  "genre_id": 2, 
        //  "genre_name": "Adventure"
    }
}
