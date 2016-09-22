using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using MedLaunch.Models;
using MedLaunch.Classes;


namespace MedLaunch.Classes
{
    public class GameListView
    {
        public GameListView()
        {

        }

        public GameListView(int gameId, string gamePath, string gameName, DateTime gameLastPlayed, string systemName, string systemDescription, int systemId)
        {
            this.GameId = gameId;
            this.GamePath = gamePath;
            this.GameName = gameName;
            this.GameLastPlayed = gameLastPlayed;
            this.SystemName = systemName;
            this.SystemDescription = systemDescription;
            this.SystemId = systemId;
        }

        public int GameId { get; set; }
        public string GamePath { get; set; }
        public string GameName { get; set; }
        public DateTime GameLastPlayed { get; set; }
        public string SystemName { get; set; }
        public string SystemDescription { get; set; }
        public int SystemId { get; set; }
    }

    public class DataGridGamesView
    {
        public DataGridGamesView()
        { }

        public DataGridGamesView(int gameId, string gameName, string gameLastPlayed, string systemName, bool isFavorite)
        {
            this.ID = gameId;
            //this.Path = gamePath;
            this.Game = gameName;
            
            
            this.System = systemName;
            this.LastPlayed = gameLastPlayed;
            this.Favorite = isFavorite;
            //this.SystemDescription = systemDescription;
            //this.SystemId = systemId;
        }

        public int ID { get; set; }
        //public string Path { get; set; }
        public string Game { get; set; }
        
        public string System { get; set; }
        public string LastPlayed { get; set; }
        public bool Favorite { get; set; }
        //public string SystemDescription { get; set; }
        //public int SystemId { get; set; }
    }
}
