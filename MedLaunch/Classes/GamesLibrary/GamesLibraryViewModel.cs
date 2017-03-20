using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GamesLibraryViewModel : GamesLibraryViewModelBase
    {
        ObservableCollection<GamesLibraryModel> _DataCollection;
        //private ICollectionView _LibraryView;
        private CollectionViewSource _LibraryView;
        ICommand _command;

        public string SearchText { get; set; }
        public CountryFilter CurrentCountryFilter { get; set; }
        public List<ColumnInfoObject> DataGridStates { get; set; }
        public string DGStatesPath { get; set; }

        CollectionViewSource cvs = new CollectionViewSource();

        public bool IsDirty;

        public MultipleFilterHandler MultipleFilter { get; set; }

        public ICollectionViewLiveShaping ShapingItems => LibraryView.View as ICollectionViewLiveShaping;

        public GamesLibraryViewModel()
        {
            this.DataCollection = new ObservableCollection<GamesLibraryModel>();
            this.LibraryView = new CollectionViewSource();

            

            DGStatesPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\GamesLibraryColumnStates.json";
            LoadDataGridStatesFromDisk();
            if (DataGridStates == null)
            {
                // load defaults from disk
                DataGridStates = new List<ColumnInfoObject>();
                for (int i = 1; i < 26; i++)
                {
                    ColumnInfoObject coo = new ColumnInfoObject();
                    coo.FilterNumber = i;
                    coo.ColumnInfoList = new List<ColumnInfo>();
                    coo.SortDescriptionList = new Dictionary<int, SortDescription>();
                    DataGridStates.Add(coo);
                }
                LoadColumnDefaults();
            }

            

            IsDirty = true;
            Update();

            // bind the CollectionViewSource to our GamesLibraryModel DataCollection
            _LibraryView.Source = DataCollection;

            ShapingItems.LiveSortingProperties.Add("Game");
            ShapingItems.LiveSortingProperties.Add("System");
            ShapingItems.IsLiveSorting = true;

            // multiplefilterhandlers
            MultipleFilter = new MultipleFilterHandler(LibraryView, MultipleFilterLogic.And);

            CurrentCountryFilter = GamesLibrary.CountryFilter.ALL;
            
        }   
        
        

        public ObservableCollection<GamesLibraryModel> DataCollection
        {
            get
            {
                return _DataCollection;
            }
            set
            {
                _DataCollection = value;
                OnPropertyChanged("DataCollection");
                //LibraryView.View.Refresh();
            }
        }

        public CollectionViewSource LibraryView
        {
            get
            {
                return _LibraryView;
            }
            
            set
            {
                _LibraryView = value;
                OnPropertyChanged("LibraryView");
                //LibraryView.View.Refresh();
            }

        }

        public ICommand RemoveCommand
        {
            get
            {
                if (_command == null)
                {
                    _command = new DelegateCommand(CanExecute, Execute);
                }
                return _command;
            }
        }

        private void Execute(object parameter)
        {
            int index = DataCollection.IndexOf(parameter as GamesLibraryModel);
            if (index > -1 && index < DataCollection.Count)
            {
                DataCollection.RemoveAt(index);
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        

        public void FilterByFilterButton(int fn)
        {
            if (fn == 0)
            {
                // get selected filter as none was provided
                fn = GamesLibraryVisualHandler.GetSelectedFilterNumber();
            }
            else
            {
                MultipleFilter.SetMainFilter(fn);
            }            
        }

        public void SearchFilter(string searchStr)
        {
            if (searchStr == "" && SearchText == "")
                return;

            if (searchStr == "")
            {
                MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.Search);
                LibraryView.View.Refresh();
                return;
            }

            SearchText = searchStr;   
            MultipleFilter.Filter += new FilterEventHandler(MultipleFilter.filters.Search);
        }

        public void CountryFilter(CountryFilter countryFilter)
        {
            CurrentCountryFilter = countryFilter;

            // remove all filters first
            MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.CountryEUR);
            MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.CountryJPN);
            MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.CountryUSA);

            // add the selected filter
            switch (countryFilter)
            {
                case GamesLibrary.CountryFilter.ALL:
                    // do nothing
                    break;
                case GamesLibrary.CountryFilter.EUR:
                    MultipleFilter.Filter += new FilterEventHandler(MultipleFilter.filters.CountryEUR); break;
                case GamesLibrary.CountryFilter.JPN:
                    MultipleFilter.Filter += new FilterEventHandler(MultipleFilter.filters.CountryJPN); break;
                case GamesLibrary.CountryFilter.USA:
                    MultipleFilter.Filter += new FilterEventHandler(MultipleFilter.filters.CountryUSA); break;
            }
        }

       
        public void Update()
        {

            if (IsDirty == false)
            {
                //return;
            }
                

            DataCollection = new ObservableCollection<GamesLibraryModel>();

            using (var cnt = new MyDbContext())
            {
                List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

                var games = (from g in cnt.Game
                             where g.hidden != true
                             select g).ToList();
                foreach (var game in games)
                {
                    GamesLibraryModel d = new GamesLibraryModel();
                    d.ID = game.gameId;

                    d.System = GSystem.GetSystemName(game.systemId);
                    d.LastPlayed = DbEF.FormatDate(game.gameLastPlayed);
                    d.Favorite = game.isFavorite;

                    d.Country = game.Country;

                    if (game.romNameFromDAT != null)
                    {
                        if (game.romNameFromDAT.Contains("(USA)"))
                            d.Country = "US";
                        if (game.romNameFromDAT.Contains("(Europe)"))
                            d.Country = "EU";
                    }


                    d.Flags = game.OtherFlags;
                    d.Language = game.Language;
                    d.Publisher = game.Publisher;
                    d.Developer = game.Developer;
                    d.Year = game.Year;

                    if (game.gameNameFromDAT != null && game.gameNameFromDAT != "")
                        d.Game = game.gameNameFromDAT;
                    else
                        d.Game = game.gameName;

                    //d.DatName = game.gameNameFromDAT;
                    d.DatRom = game.romNameFromDAT;

                    if (game.gdbId != null && game.gdbId > 0)
                    {
                        var link = links.Where(x => x.GDBId == game.gdbId).SingleOrDefault(); // LibraryDataGDBLink.GetLibraryData(game.gdbId.Value);
                        if (link != null)
                        {
                            if (link.Publisher != null && link.Publisher != "")
                                d.Publisher = link.Publisher;

                            d.Developer = link.Developer;

                            if (link.Year != null && link.Year != "")
                                d.Year = DbEF.ReturnYear(link.Year);
                            d.Players = link.Players;
                            d.Coop = link.Coop;
                            d.ESRB = link.ESRB;
                        }
                    }

                    DataCollection.Add(d);
                }

                IsDirty = false;
            }
        }

        public void LoadColumnDefaults()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
            {
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
                var dict = JsonConvert.DeserializeObject<List<ColumnInfoObject>>(json);
                for (int i = 0; i < DataGridStates.Count; i++)
                {
                    var temp = DataGridStates.Where(a => a.FilterNumber == (i + 1)).FirstOrDefault();
                    var d = dict.Where(a => a.FilterNumber == 1).FirstOrDefault();

                    temp.ColumnInfoList = d.ColumnInfoList;
                    temp.SortDescriptionList = d.SortDescriptionList;
                    //DataGridStates[i + 1] = dict[1];
                }
            }
        }

        public void LoadColumnDefaults(int filterNumber)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
            {
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
                var dict = JsonConvert.DeserializeObject<List<ColumnInfoObject>>(json);

                var temp = DataGridStates.Where(a => a.FilterNumber == filterNumber).FirstOrDefault();
                var d = dict.Where(a => a.FilterNumber == 1).FirstOrDefault();
                temp.ColumnInfoList = d.ColumnInfoList;
                temp.SortDescriptionList = d.SortDescriptionList;


                //DataGridStates[filterNumber] = dict[1];
            }
        }

        public void SaveDataGridStatesToDisk()
        {
            // convert to json
            string json = JsonConvert.SerializeObject(DataGridStates, Formatting.Indented);
            // save to disk (overwrite)
            try
            {
                File.WriteAllText(DGStatesPath, json);
            }
            catch
            {
                // IO error
                MessageBox.Show("There was an error writing to the config file: \n\n" + DGStatesPath + "\n\n Do you have the file open elsewhere??", "IO ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadDataGridStatesFromDisk()
        {
            if (File.Exists(DGStatesPath))
            {
                // load the file
                string json = File.ReadAllText(DGStatesPath);
                DataGridStates = JsonConvert.DeserializeObject<List<ColumnInfoObject>>(json);
            }
            else
            {
                // file does not exist - populate defaults
            }
        }
    }
}
