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
    /*
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

        private string datName;
        public string DatName
        {
            get
            {
                return datName;
            }
            set
            {
                if (datName != value)
                {
                    datName = value;
                    OnPropertyChanged("DatName");
                }
            }
        }

        private string datRom;
        public string DatRom
        {
            get
            {
                return datRom;
            }
            set
            {
                if (datRom != value)
                {
                    datRom = value;
                    OnPropertyChanged("DatRom");
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

        private string country;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string language;
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                if (language != value)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        private string flags;
        public string Flags
        {
            get
            {
                return flags;
            }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    OnPropertyChanged("Flags");
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

  */
}
