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

using System.Windows.Input;
using Microsoft.Data.Entity;

namespace MedLaunch.ViewModels
{
    public class GlobalSettingsViewModel : ObservableObject
    {
        #region Construction
        // Constructs the default instance of a GlobalSettingsViewModel
        public GlobalSettingsViewModel()
        {
            /*
            _globalSettings = new GlobalSettings
            {
                settingsId = 1,
                bypassConfig = false,
                databaseGenerated = true,
                enableNetplay = false,
                fullGuiScreen = false,
                fullScreen = true
            };
            */
            
            _globalSettings = new GlobalSettings();
            using (var db = new MyDbContext())
            {
                GlobalSettings gs = db.GlobalSettings.Where(a => a.settingsId == 1).FirstOrDefault();
                _globalSettings = gs;
            }
            
        }
        #endregion

        #region Members
        GlobalSettings _globalSettings;
        #endregion

        #region Properties
        public GlobalSettings GlobalSettings
        {
            get
            {
                return _globalSettings;
            }
            set
            {
                _globalSettings = value;
            }
        }

        public int SettingsId
        {
            get { return _globalSettings.settingsId; }
            set { _globalSettings.settingsId = value; }
        }

        public bool FullScreen
        {
            get { return NullableBool2Bool.Convert(_globalSettings.fullScreen); }
            set
            {
                if (GlobalSettings.fullScreen != value)
                {
                    _globalSettings.fullScreen = value;
                    RaisePropertyChanged("FullScreen");
                    using (var db = new MyDbContext())
                    {
                        db.GlobalSettings.Attach(_globalSettings);
                        db.Entry(_globalSettings).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        public bool FullGuiScreen
        {
            get { return NullableBool2Bool.Convert(_globalSettings.fullGuiScreen); }
            set
            {
                if (GlobalSettings.fullGuiScreen != value)
                {
                    _globalSettings.fullGuiScreen = value;
                    RaisePropertyChanged("FullGuiScreen");
                }
            }
        }

        public bool BypassConfig
        {
            get { return NullableBool2Bool.Convert(_globalSettings.bypassConfig); }
            set
            {
                if (GlobalSettings.bypassConfig != value)
                {
                    _globalSettings.bypassConfig = value;
                    RaisePropertyChanged("BypassConfig");
                }
            }
        }

        public bool EnableNetplay
        {
            get { return NullableBool2Bool.Convert(_globalSettings.enableNetplay); }
            set
            {
                if (GlobalSettings.enableNetplay != value)
                {
                    _globalSettings.enableNetplay = value;
                    RaisePropertyChanged("EnableNetplay");
                }
            }
        }

        #endregion
        
        #region Commands
        void UpdateGlobalSettingsExecute()
        {
            GlobalSettings = _globalSettings;
        }
        bool CanUpdateGlobalSettingsExecute()
        {
            return true;
        }
        public ICommand UpdateGlobalSettings { get { return new RelayCommand(UpdateGlobalSettingsExecute, CanUpdateGlobalSettingsExecute); } }

        void UpdateFullScreenExecute()
        {
            GlobalSettings = _globalSettings;
        }
        bool CanUpdateFullScreenExecute()
        {
            return true;
        }
        public ICommand UpdateFullScreen { get { return new RelayCommand(UpdateGlobalSettingsExecute, CanUpdateFullScreenExecute); } }
        #endregion
    }
}
