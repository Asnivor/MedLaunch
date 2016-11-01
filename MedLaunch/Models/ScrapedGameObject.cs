using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class ScrapedGameObject
    {
        public int GdbId { get; set; }
        public ScrapedGameData Data { get; set; }
        public List<string> FrontCovers { get; set; }
        public List<string> BackCovers { get; set; }
        public List<string> Medias { get; set; }
        public List<string> Banners { get; set; }
        public List<string> Screenshots { get; set; }
        public List<string> FanArts { get; set; }
        public List<string> Manuals { get; set; }

        public ScrapedGameObject()
        {
            FrontCovers = new List<string>();
            BackCovers = new List<string>();
            Medias = new List<string>();
            Banners = new List<string>();
            Screenshots = new List<string>();
            FanArts = new List<string>();
            Manuals = new List<string>();
        }
    }

    public class ScrapedGameObjectWeb
    {
        public int GdbId { get; set; }
        public ScrapedGameData Data { get; set; }
        public List<string> FrontCovers { get; set; }
        public List<string> BackCovers { get; set; }
        public List<string> Medias { get; set; }
        public List<string> Banners { get; set; }
        public List<string> Screenshots { get; set; }
        public List<string> FanArts { get; set; }
        public List<string> Manuals { get; set; }

        public ScrapedGameObjectWeb()
        {
            FrontCovers = new List<string>();
            BackCovers = new List<string>();
            Medias = new List<string>();
            Banners = new List<string>();
            Screenshots = new List<string>();
            FanArts = new List<string>();
            Manuals = new List<string>();
        }
    }
}
