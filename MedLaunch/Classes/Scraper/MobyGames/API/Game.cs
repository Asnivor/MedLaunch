using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.MobyGames.API
{
    public class Game
    {
        public List<AlternateTitle> alternate_titles { get; set; }
        public string description { get; set; }
        public int game_id { get; set; }
        public List<Genre> genres { get; set; }
        public decimal? moby_score { get; set; }
        public string moby_url { get; set; }
        public int num_votes { get; set; }
        public string official_url { get; set ; }
        public List<Platform> platforms { get; set; }
        public SampleCover sample_cover { get; set; }
        public List<SampleScreenshot> sample_screenshots { get; set; }
        public string title { get; set; }  
    }
}
