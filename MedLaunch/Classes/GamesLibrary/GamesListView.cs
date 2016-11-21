using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using MedLaunch.Models;
using MedLaunch.Classes;
using System.ComponentModel;

namespace MedLaunch.Classes
{  
    public class DataGridGamesView : INotifyPropertyChanged
    {
        private int iD;
        public int ID
        {
            get
            {
                return iD;
            }
            set
            {
                if (iD != value)
                {
                    iD = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string game;
        public string Game
        {
            get
            {
                return game;
            }
            set
            {
                if (game != value)
                {
                    game = value;
                    OnPropertyChanged("Game");
                }
            }
        }

        private string system;
        public string System
        {
            get
            {
                return system;
            }
            set
            {
                if (system != value)
                {
                    system = value;
                    OnPropertyChanged("System");
                }
            }
        }

        private string lastPlayed;
        public string LastPlayed
        {
            get
            {
                return lastPlayed;
            }
            set
            {
                if (lastPlayed != value)
                {
                    lastPlayed = value;
                    OnPropertyChanged("LastPlayed");
                }
            }
        }

        private bool favorite;
        public bool Favorite
        {
            get
            {
                return favorite;
            }
            set
            {
                if (favorite != value)
                {
                    favorite = value;
                    OnPropertyChanged("Favorite");
                }
            }
        }

        private string publisher;
        public string Publisher
        {
            get
            {
                return publisher;
            }
            set
            {
                if (publisher != value)
                {
                    publisher = value;
                    OnPropertyChanged("Publisher");
                }
            }
        }

        private string developer;
        public string Developer
        {
            get
            {
                return developer;
            }
            set
            {
                if (developer != value)
                {
                    developer = value;
                    OnPropertyChanged("Developer");
                }
            }
        }

        private string year;
        public string Year
        {
            get
            {
                return year;
            }
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        private string players;
        public string Players
        {
            get
            {
                return players;
            }
            set
            {
                if (players != value)
                {
                    players = value;
                    OnPropertyChanged("Players");
                }
            }
        }

        private string coop;
        public string Coop
        {
            get
            {
                return coop;
            }
            set
            {
                if (coop != value)
                {
                    coop = value;
                    OnPropertyChanged("Coop");
                }
            }
        }

        private string esrb;
        public string ESRB
        {
            get
            {
                return esrb;
            }
            set
            {
                if (esrb != value)
                {
                    esrb = value;
                    OnPropertyChanged("ESRB");
                }
            }
        }


        protected void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }

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
}
