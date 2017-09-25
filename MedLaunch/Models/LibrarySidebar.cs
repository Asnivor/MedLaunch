using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;

namespace MedLaunch.Models
{
    public class LibrarySidebar
    {
        private MyDbContext db;

        // Properties
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string GamePath { get; set; }
        public bool IsFavorite { get; set; }

        public bool ManEdit { get; set; }

        public DateTime LastPlayed { get; set; }
        public DateTime LastFinished { get; set; }
        public Double TotalPlayTime { get; set; }
        public int TimesPlayed { get; set; }

        public int SystemId { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }

        public int gdbid { get; set; }

        public string AlternateTitles { get; set; }
        public string Genres { get; set; }
        public string Coop { get; set; }
        public string Developer { get; set; }
        public string ESRB { get; set; }
        public string Overview { get; set; }
        public string Players { get; set; }
        public string Publisher { get; set; }
        public string Year { get; set; }

        public string Copyright { get; set; }
        public string Country { get; set; }
        public string DevelopmentStatus { get; set; }
        public string Language { get; set; }
        public string OtherFlags { get; set; }

        // Constructor
        public LibrarySidebar(int gameId)
        {
            db = new MyDbContext();

            // populate public properties
            GameId = gameId;

            Game game = (from g in db.Game
                         where g.gameId == gameId
                         select g).SingleOrDefault();
            if (game == null)
            {
                return;
            }

            GameName = game.gameName;
            GamePath = game.gamePath;
            IsFavorite = game.isFavorite;

            LastPlayed = game.gameLastPlayed;
            LastFinished = game.gameLastFinished;
            TotalPlayTime = game.gameTime;
            TimesPlayed = game.timesPlayed;

            SystemId = game.systemId;
            SystemCode = GSystem.GetSystemCode(SystemId);
            SystemName = GSystem.GetSystemName(SystemId);
            SystemDescription = GSystem.GetSystemDesc(SystemId);

            if (game.gdbId != null)
                gdbid = game.gdbId.Value;

            AlternateTitles = game.AlternateTitles;
            Genres = game.Genres;
            Coop = game.Coop;
            Developer = game.Developer;
            ESRB = game.ESRB;
            Overview = game.Overview;
            Players = game.Players;
            Publisher = game.Publisher;
            Year = game.Year;

            Copyright = game.Copyright;
            Country = game.Country;
            DevelopmentStatus = game.DevelopmentStatus;
            Language = game.Language;
            OtherFlags = game.OtherFlags;

            if (game.ManualEditSet == true)
            {
                ManEdit = true;
            }
            else
            {
                ManEdit = false;
            }
        }

        
    }
}
