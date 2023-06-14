using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.MobyGames.API
{
    public class Game
    {
        List<AlternateTitle> alternate_titles { get; set; }
        string description { get; set; }    
        int game_id { get; set; }
        List<Genre> genres { get; set; }    
        decimal moby_score { get; set; }
        string moby_url { get; set; }
        int num_votes { get; set; } 
        string official_url { get; set ; }
        List<Platform> platforms { get; set; }
        SampleCover sample_cover { get; set; }
        List<SampleScreenshot> sample_screenshots { get; set; }
        string title { get; set; }  
    }
}
