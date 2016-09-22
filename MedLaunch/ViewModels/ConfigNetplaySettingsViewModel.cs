using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using MedLaunch.Models;
using System.ComponentModel;
using Asnitech.Launch.Common;
using Asnitech.Launch.Common.Converters;
using MedLaunch.Enums;
using System.Windows.Input;
using Microsoft.Data.Entity;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace MedLaunch.ViewModels
{
    /// <summary>
    /// This class is a view model of a Netplay Settings Entry.
    /// </summary>
    class ConfigNetplaySettingsViewModel : ObservableObject
    {
        #region Construction
        /// <summary>
        /// Constructs the default instance of a SongViewModel
        /// </summary>
        public ConfigNetplaySettingsViewModel()
        {
            using (var db = new MyDbContext())
            {
                _configNetplaySettings = (from a in db.ConfigNetplaySettings
                                          where a.ConfigNPId == 1
                                          select a).FirstOrDefault();
            }
            /*
            _configNetplaySettings = new ConfigNetplaySettings
            {
                ConfigNPId = 1,
                netplay__console__font = "9x18",
                netplay__console__lines = 5,
                netplay__console__scale = 1,
                netplay__localplayers = 1,
                netplay__nick = "Player One"
            };
            */
        }
        #endregion

        #region Members
        ConfigNetplaySettings _configNetplaySettings;
        //int _count = 0;
        #endregion

        #region Properties
        public ConfigNetplaySettings ConfigNetplaySettings
        {
            get
            {
                return _configNetplaySettings;
            }
            set
            {
                _configNetplaySettings = value;
                
            }
        }


        public string Nickname
        {
            get { return ConfigNetplaySettings.netplay__nick; }
            set
            {
                if (ConfigNetplaySettings.netplay__nick != value)
                {
                    ConfigNetplaySettings.netplay__nick = value;
                    RaisePropertyChanged("Nickname");
                }
            }
        }

        public string ConsoleFont
        {
            get { return ConfigNetplaySettings.netplay__console__font; }
            set
            {
                if (ConfigNetplaySettings.netplay__console__font != value)
                {
                    ConfigNetplaySettings.netplay__console__font = value;
                    RaisePropertyChanged("ConsoleFont");
                }
            }
        }

        public int ConsoleLines
        {
            get { return NullableInt2Int.Convert(ConfigNetplaySettings.netplay__console__lines); }
            set
            {
                if (ConfigNetplaySettings.netplay__console__lines != value)
                {
                    ConfigNetplaySettings.netplay__console__lines = value;
                    RaisePropertyChanged("ConsoleLines");
                }
            }
        }

        public int ConsoleScale
        {
            get { return NullableInt2Int.Convert(ConfigNetplaySettings.netplay__console__scale); }
            set
            {
                if (ConfigNetplaySettings.netplay__console__scale != value)
                {
                    ConfigNetplaySettings.netplay__console__scale = value;
                    RaisePropertyChanged("ConsoleScale");
                }
            }
        }

        public int LocalPlayers
        {
            get { return NullableInt2Int.Convert(ConfigNetplaySettings.netplay__localplayers); }
            set
            {
                if (ConfigNetplaySettings.netplay__localplayers != value)
                {
                    ConfigNetplaySettings.netplay__localplayers = value;
                    RaisePropertyChanged("LocalPlayers");
                }
            }
        }

        #endregion

        #region Commands
        void UpdateNicknameExecute()
        {
            //++_count;
            Nickname = this.Nickname;
        }
        bool CanUpdateNicknameExecute()
        {
            return true;
        }
        public ICommand UpdateNickname { get { return new RelayCommand(UpdateNicknameExecute, CanUpdateNicknameExecute); } }

        void UpdateConsoleFontExecute()
        {
            ConsoleFont = this.ConsoleFont;
        }
        bool CanUpdateConsoleFontExecute()
        {
            return true;
        }
        public ICommand UpdateConsoleFont { get { return new RelayCommand(UpdateConsoleFontExecute, CanUpdateConsoleFontExecute); } }
        #endregion

        void UpdateLocalPlayersExecute()
        {
            LocalPlayers = this.LocalPlayers;
        }
        bool CanUpdateLocalPlayersExecute()
        {
            return true;
        }
        public ICommand UpdateLocalPlayers { get { return new RelayCommand(UpdateLocalPlayersExecute, CanUpdateLocalPlayersExecute); } }
        public static void SetLocalPlayers(string text)
        {
            
        }

        public static void saveChanges(MetroWindow sp)
        {
            var nickname = (TextBox)sp.FindName("tbNetplayNick");
            var consoleLines = (Slider)sp.FindName("slConsoleLinesValue");
            var localPlayers = (Slider)sp.FindName("slLocalPlayersValue");
            var consoleScale = (Slider)sp.FindName("slConsoleScaleValue");
            //var consoleFont = (RadioButton)sp.FindName();

            ConfigNetplaySettings NetplaySettings = new ConfigNetplaySettings();
            NetplaySettings.ConfigNPId = 1;
            NetplaySettings.netplay__nick = "dfsdfs";//nickname.Text;
            NetplaySettings.netplay__console__lines = Convert.ToInt32(consoleLines.Value);
            NetplaySettings.netplay__console__scale = Convert.ToInt32(consoleScale.Value);
            NetplaySettings.netplay__localplayers = Convert.ToInt32(localPlayers);

            NetplaySettings.netplay__console__font = "9x18";

            MyDbContext newDbContext = new MyDbContext();
            /*
            newDbContext.ConfigNetplaySettings.Attach(NetplaySettings);
            newDbContext.Entry(NetplaySettings).State = EntityState.Modified;
            newDbContext.SaveChanges();
            newDbContext.Dispose();
            */

            /*
            var original = (from a in newDbContext.ConfigNetplaySettings
                            where a.ConfigNPId == 1
                            select a).FirstOrDefault();
            if (original != null)
            {
                newDbContext.Entry(original).CurrentValues.SetValues(NetplaySettings);
            }
            */

            newDbContext.ConfigNetplaySettings.Attach(NetplaySettings);
            var entry = newDbContext.Entry(NetplaySettings);
            entry.State = EntityState.Modified;

            entry.Property(e => e.ConfigNPId).IsModified = false;
            newDbContext.SaveChanges();
            

            
        }




    }
}
