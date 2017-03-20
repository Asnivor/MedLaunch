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

        public DateTime LastPlayed { get; set; }
        public DateTime LastFinished { get; set; }
        public Double TotalPlayTime { get; set; }
        public int TimesPlayed { get; set; }

        public int SystemId { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }        

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


        }

        
    }
}
