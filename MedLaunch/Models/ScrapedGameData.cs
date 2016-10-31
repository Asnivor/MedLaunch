using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class ScrapedGameData
    {
        public string Title { get; set; }
        public string Overview { get; set; }
        public List<string> AlternateTitles { get; set; }
        public List<string> Genres { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Coop { get; set; }
        public string ESRB { get; set; }
        public string Players { get; set; }
        public string Released { get; set; }
        public string Platform { get; set; }
    }

    public class ScrapedGameObject
    {
        public int GdbId { get; set; }
        public ScrapedGameData Data { get; set; }
        public List<string> FrontCovers { get; set; }
        public List<string> BackCovers { get; set; }
        public List<string> Medias { get; set; }
        public List<string> Banners { get; set; }
        public List<string> Screenshots { get; set; }
        public List<string> PromoArts { get; set; }
        public List<string> FanArts { get; set; }
        public List<string> Manuals { get; set; }

        public ScrapedGameObject()
        {
            FrontCovers = new List<string>();
            BackCovers = new List<string>();
            Medias = new List<string>();
            Banners = new List<string>();
            Screenshots = new List<string>();
            PromoArts = new List<string>();
            FanArts = new List<string>();
            Manuals = new List<string>();
        }
    }
}
