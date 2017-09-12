using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MedLaunch.Extensions;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GamesLibraryViewModel : GamesLibraryViewModelBase
    {
        ObservableCollectionEx<GamesLibraryModel> _DataCollection;
        //GamesLibraryCollection DataCollection;
        //private ICollectionView _LibraryView;
        private CollectionViewSource _LibraryView;
        ICommand _command;

        public string SearchText { get; set; }
        public CountryFilter CurrentCountryFilter { get; set; }
        public List<ColumnInfoObject> DataGridStates { get; set; }
        public string DGStatesPath { get; set; }

        CollectionViewSource cvs = new CollectionViewSource();

        public bool DataGridFocused { get; set; }

        public bool IsDirty;

        public MultipleFilterHandler MultipleFilter { get; set; }

        //public ICollectionViewLiveShaping ShapingItems => LibraryView.View as ICollectionViewLiveShaping;

        public GamesLibraryViewModel()
        {
            // this.DataCollection = new ObservableCollectionEx<GamesLibraryModel>();
            this._DataCollection = new ObservableCollectionEx<GamesLibraryModel>();
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

            //ShapingItems.LiveSortingProperties.Add("Game");
           // ShapingItems.LiveSortingProperties.Add("System");
           // ShapingItems.IsLiveSorting = true;

            // multiplefilterhandlers
            MultipleFilter = new MultipleFilterHandler(LibraryView, MultipleFilterLogic.And);

            CurrentCountryFilter = GamesLibrary.CountryFilter.ALL;

            DataGridFocused = true;

        }   
        
        

        
        public ObservableCollectionEx<GamesLibraryModel> DataCollection
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
            /*
            int index = DataCollection.IndexOf(parameter as GamesLibraryModel);
            if (index > -1 && index < DataCollection.Count)
            {
                DataCollection.RemoveAt(index);
            }
            */
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
               
            }

            MultipleFilter.SetMainFilter(fn);
            LibraryView.SortDescriptions.Add(new SortDescription("Game", ListSortDirection.Descending));
        }

        public void SearchFilter(string searchStr)
        {
           
            if (searchStr == "" && SearchText == "")
                return;

            if (searchStr == "")
            {
                MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.Search);
                //LibraryView.View.Refresh();
                return;
            }
           
            SearchText = searchStr;

            using (LibraryView.DeferRefresh())
            {
                MultipleFilter.Filter -= new FilterEventHandler(MultipleFilter.filters.Search);
                MultipleFilter.Filter += new FilterEventHandler(MultipleFilter.filters.Search);
            } 
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

        public void RemoveEntries(List<Game> games)
        {
            foreach (var game in games)
            {
                var g = DataCollection.Where(a => a.ID == game.gameId).FirstOrDefault();
                if (g == null)
                    continue;

                DataCollection.Remove(g);                    
            }
        }

        public void UpdateEntries(List<Game> games)
        {
            //List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

            foreach (var game in games)
            {
                var g = DataCollection.Where(a => a.ID == game.gameId).FirstOrDefault();
                if (g == null)
                    continue;

                App _App = (App)Application.Current;
                using (_App.GamesLibrary.LibraryView.DeferRefresh())
                {
                    DataCollection.Remove(g);
                    GamesLibraryModel glm = CreateModelFromGame(game);//, links);
                    DataCollection.Add(glm);
                }

                // because we have effectively removed a game from the collection and re-added it
                // the indecies will be off. go back one.
                _App.GamesLibrary.LibraryView.View.MoveCurrentToPrevious();                 
            }
        }

        public void AddEntries(List<Game> games)
        {
            //List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

            foreach (var game in games)
            {
                GamesLibraryModel glm = CreateModelFromGame(game);//, links);

                DataCollection.Add(glm);
            }
        }

        public void AddUpdateEntry(Game game)
        {
            //List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

            if (game != null)
            {
                // see if game is already in collection
                var search = (from a in _DataCollection
                              where a.ID == game.gameId
                              select a).FirstOrDefault();

                if (search == null)
                {
                    // game does not exist in the view - add it
                    GamesLibraryModel glm = CreateModelFromGame(game);//, links);
                    DataCollection.Add(glm);
                }
                else
                {
                    // game exists in the view - update it
                    UpdateEntries(new List<Game> { game });
                }
            }          
        }

        public static GamesLibraryModel CreateModelFromGame(Game game)//, List<LibraryDataGDBLink> links)
        {
            /*
            if (links == null)
                links = LibraryDataGDBLink.GetLibraryData().ToList();
                */

            GamesLibraryModel d = new GamesLibraryModel();
            d.ID = game.gameId;

            // check for subsystem
            if (game.subSystemId != null && game.subSystemId > 0)
            {
                string subName = GSystem.GetSubSystemName(game.subSystemId.Value);
                d.System = subName;
            }
            else
            {
                d.System = GSystem.GetSystemName(game.systemId);
            }

            

            d.LastPlayed = DbEF.FormatDate(game.gameLastPlayed);
            d.Favorite = game.isFavorite;

            d.Country = game.Country;

            if (game.romNameFromDAT != null)
            {
                /*
                if (game.romNameFromDAT.Contains("(USA)"))
                    d.Country = "USA";
                if (game.romNameFromDAT.Contains("(Europe)"))
                    d.Country = "EUR";
                if (game.romNameFromDAT.Contains("(Japan)"))
                    d.Country = "JPN";
                    */
            }

            d.Flags = game.OtherFlags;
            d.Language = game.Language;
            d.Publisher = game.Publisher;
            d.Developer = game.Developer;
            d.Year = game.Year;
            d.Coop = game.Coop;
            d.ESRB = game.ESRB;
            d.Players = game.Players;
            d.Year = game.Year;
            d.Game = game.gameName;

            /*
            if (game.gameNameFromDAT != null && game.gameNameFromDAT != "")
                d.Game = game.gameNameFromDAT;
            else
                d.Game = game.gameName;
                */

            //d.DatName = game.gameNameFromDAT;
            d.DatRom = game.romNameFromDAT;

            /*
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
            */
            //d.Year = "2914";

            // last minute region detection
            if ((d.Country == null || d.Country.Trim() == ""))
            {                
                if (d.Game.Contains("(Japan)"))
                    d.Country = "Japan";
                if (d.Game.Contains("(Europe)"))
                    d.Country = "Europe";
                if (d.Game.Contains("(USA)"))
                    d.Country = "USA";
                if (d.Game.Contains("(Usa, Europe)"))
                    d.Country = "USA, Europe";

                // goodtools
                if (d.Game.Contains("(W)"))
                    d.Country = "World";
                if (d.Game.Contains("(U)"))
                    d.Country = "USA";
                if (d.Game.Contains("(As)"))
                    d.Country = "Asia";
                if (d.Game.Contains("(E)"))
                    d.Country = "Europe";
            }

            return d;
        }

        /// <summary>
        /// big update - clears DataCollection and re-populates
        /// </summary>
        public void Update()
        {                     
            if (DataCollection == null)
                DataCollection = new ObservableCollectionEx<GamesLibraryModel>(); 

            using (var cnt = new MyDbContext())
            {
                DataCollection.Clear();

                //List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();

                var games = (from g in cnt.Game
                                where g.hidden != true
                                select g).ToList();
                foreach (var game in games)
                {
                    var g = CreateModelFromGame(game);//, links);
                    if (g != null)
                        DataCollection.Add(g);
                }

                DataCollection.OrderBy(a => a.Game);
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

    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e != null)  // There's been an addition or removal of items from the Collection
            {
                Unsubscribe(e.OldItems);
                Subscribe(e.NewItems);
                base.OnCollectionChanged(e);
            }
            else
            {
                // Just a property has changed, so reset the Collection.
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            }

        }

        protected override void ClearItems()
        {
            foreach (T element in this)
                element.PropertyChanged -= ContainedElementChanged;

            base.ClearItems();
        }

        private void Subscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged += ContainedElementChanged;
            }
        }

        private void Unsubscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
            }
        }

        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
            // Tell the Collection that the property has changed
            this.OnCollectionChanged(null);

        }
    }
}
