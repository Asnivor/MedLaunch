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

namespace MedLaunch.Classes.GamesLibrary
{
    public class GameListBuilder : INotifyPropertyChanged
    {
        // properties

        public string SortColumnName { get; set; }
        public ListSortDirection SortDirection { get; set; }
        public string DGStatesPath { get; set; }

        public CountryFilter countryFilter { get; set; }

        public List<DataGridGamesView> FilteredSetCache;

        private ObservableCollection<DataGridGamesView> filteredSet;
        public ObservableCollection<DataGridGamesView> FilteredSet
        {
            get
            {
                return filteredSet;
            }
            set
            {
                if (filteredSet != value)
                {
                    filteredSet = value;
                    OnPropertyChanged("FilteredSet");
                }
            }
        }

        public Dictionary<int, List<ColumnInfo>> DataGridStates { get; set; }


        public List<DataGridGamesView> AllGames { get; set; }
        public int SystemId { get; set; }
        public string SearchString { get; set; }
        public bool UpdateRequired { get; set; }

        // constructors

        public GameListBuilder()
        {
            // instantiate new AllGames object for the first time
            AllGames = new List<DataGridGamesView>();

            // populate the object
            AllGames = GamesLibraryDataGridRefresh.Update(AllGames);

            // country filter initial population
            countryFilter = GamesLibraryVisualHandler.GetSelectedCountryFilter();

            DGStatesPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\GamesLibraryColumnStates.json";

            // set initial sorting
            SortColumnName = "GAME";
            SortDirection = ListSortDirection.Ascending;
            
            UpdateRequired = false;

            if (DataGridStates == null)
            {
                // load defaults from disk
                DataGridStates = new Dictionary<int, List<ColumnInfo>>();
                DataGridStates.Add(1, new List<ColumnInfo>());
                DataGridStates.Add(2, new List<ColumnInfo>());
                DataGridStates.Add(3, new List<ColumnInfo>());
                DataGridStates.Add(4, new List<ColumnInfo>());
                DataGridStates.Add(5, new List<ColumnInfo>());
                DataGridStates.Add(6, new List<ColumnInfo>());
                DataGridStates.Add(7, new List<ColumnInfo>());
                DataGridStates.Add(8, new List<ColumnInfo>());
                DataGridStates.Add(9, new List<ColumnInfo>());
                DataGridStates.Add(10, new List<ColumnInfo>());
                DataGridStates.Add(11, new List<ColumnInfo>());
                DataGridStates.Add(12, new List<ColumnInfo>());
                DataGridStates.Add(13, new List<ColumnInfo>());
                DataGridStates.Add(14, new List<ColumnInfo>());
                DataGridStates.Add(15, new List<ColumnInfo>());
                DataGridStates.Add(16, new List<ColumnInfo>());
                DataGridStates.Add(17, new List<ColumnInfo>());
                DataGridStates.Add(18, new List<ColumnInfo>());
                DataGridStates.Add(19, new List<ColumnInfo>());
                DataGridStates.Add(20, new List<ColumnInfo>());
                DataGridStates.Add(21, new List<ColumnInfo>());
                DataGridStates.Add(22, new List<ColumnInfo>());
                DataGridStates.Add(23, new List<ColumnInfo>());
                DataGridStates.Add(24, new List<ColumnInfo>());
                DataGridStates.Add(25, new List<ColumnInfo>());

                LoadColumnDefaults();
            }

            LoadDataGridStatesFromDisk();
        }

        public void LoadColumnDefaults()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
            {
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
                var dict = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);
                for (int i = 0; i < DataGridStates.Count; i++)
                {
                    DataGridStates[i + 1] = dict[1];
                }
            }
        }

        public void LoadColumnDefaults(int filterNumber)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json"))
            {
                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ColumnDefaults.json");
                var dict = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);

                DataGridStates[filterNumber] = dict[1];                
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
                DataGridStates = JsonConvert.DeserializeObject<Dictionary<int, List<ColumnInfo>>>(json);
            }
            else
            {
                // file does not exist - populate defaults
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

        public void CountryFilterUpdate()
        {
            countryFilter = GamesLibraryVisualHandler.GetSelectedCountryFilter();

            List<DataGridGamesView> temp = new List<DataGridGamesView>();

            // update the collection based on country code filter
            switch (countryFilter)
            {
                case CountryFilter.EUR:                    
                    temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("EU")).ToList();
                    FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                    break;
                case CountryFilter.JPN:
                    temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("J")).ToList();
                    FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                    break;
                case CountryFilter.USA:
                    temp = FilteredSetCache.Where(a => a.Country != null && a.Country.ToUpper().Contains("US")).ToList();
                    FilteredSet = new ObservableCollection<DataGridGamesView>(temp);
                    break;
                default:
                    FilteredSet = new ObservableCollection<DataGridGamesView>(FilteredSetCache.ToList());
                    break;
            }            
        }

        // update game data for games datagrid including search
        public static void GetGames(DataGrid datagrid, int systemId, string search) //, CountryFilter countryFilter)
        {
            // get the full dataset from application
            App _App = ((App)Application.Current);
            //List<DataGridGamesView> allGames = _App.GamesList.AllGames;

            

            var result = GameListBuilder.Filter(systemId, search); //, countryFilter);
        }

        

        /// <summary>
        /// remove hidden systems from results set
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static List<DataGridGamesView> RemoveHidden(List<DataGridGamesView> results)
        {
            bool[] visibilities = GlobalSettings.GetVisArray();

            // check whether there are actually any hidden systems
            bool systemsHidden = false;
            for (int i = 1; i <= visibilities.Length; i++)
            {
                if (i == 16 || i == 17)
                    continue;

                if (visibilities[i - 1] == false)
                {
                    systemsHidden = true;
                    break;
                }
            }

            if (systemsHidden == false)
            {
                // there are no hidden systems - just return the results as-is
                return results;
            }                
            else
            {
                // there are hidden systems - remove them from results
                for (int i = 1; i <= visibilities.Length; i++)
                {
                    if (i == 16 || i == 17) // skip fast and faust
                        continue;

                    if (visibilities[i - 1] == false)
                    {
                        results = (from a in results
                                  where a.System != GSystem.GetSystemName(i)
                                  select a).ToList();
                    }
                }

                return results;
            }
        }

        public static List<DataGridGamesView> Filter(int systemId, string search) //, CountryFilter countryFilter)
        {
            List<DataGridGamesView> results = new List<DataGridGamesView>();
            App _App = ((App)Application.Current);

            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            List<ColumnInfo> cis = new List<ColumnInfo>();
            DataGrid dgGameList = new DataGrid();
            List<SortDescription> sdc = new List<SortDescription>();

            ICollectionView cView = CollectionViewSource.GetDefaultView(dgGameList.ItemsSource);
            if (cView != null)
                sdc = cView.SortDescriptions.ToList();
            

            if (mw != null)
            {
                dgGameList = (DataGrid)mw.FindName("dgGameList");
                // get the current datagrid column layout (will re-apply it at the end of this method)

                cis = ColumnInfo.GetColumnInfo(dgGameList);                
            }
            
            if (_App.GamesList.UpdateRequired == true)
                GamesLibraryDataGridRefresh.Update();

            // system id selector
            switch (systemId)
            {
                case -1:        // favorites
                    results = (from g in _App.GamesList.AllGames
                               where g.Favorite == true
                             select g).ToList();

                    results = RemoveHidden(results);

                    break;
                case 0:         // all games
                    results = _App.GamesList.AllGames;
                    results = RemoveHidden(results);


                    break;
                case -100:      // unscraped games

                    results = (from a in _App.GamesList.AllGames
                               where a.Coop == null &&
                               a.Developer == null &&
                               a.ESRB == null &&
                               a.Players == null
                               select a).ToList();

                    results = RemoveHidden(results);


                    /*
                    var re = _App.GamesList.AllGames.ToList();
                    List<Game> games = (from a in Game.GetGames()
                                        where a.isScraped != true
                                        select a).ToList();
                    foreach (var ga in re)
                    {
                        var i = (from b in games
                                 where b.gameId == ga.ID
                                 select b).ToList();
                        if (i.Count == 1)
                        {
                            results.Add(ga);
                        }
                    }
                    */

                    break;
                default:        // based on actual system id
                    results = (from g in _App.GamesList.AllGames
                               where GSystem.GetSystemId(g.System) == systemId
                             select g).ToList();
                    break;
            }

            // narrow search based on country/region filter
            /*
            switch (countryFilter)
            {
                case CountryFilter.EUR:
                    results = results.Where(a => a.Country != null && a.Country.ToUpper().Contains("EU")).ToList();
                    break;

                case CountryFilter.JPN:
                    results = results.Where(a => a.Country != null && a.Country.ToUpper().Contains("J")).ToList();
                    break;

                case CountryFilter.USA:
                    results = results.Where(a => a.Country != null && a.Country.ToUpper().Contains("US")).ToList();
                    break;

                default:
                    break;
            }
            */

            // now we have results based on the system filter - process file search
            List<DataGridGamesView> srch = DoSearch(results, search);

            /*
            List<DataGridGamesView> sorted = new List<DataGridGamesView>();
            // perform sorting
            if (_App.GamesList.SortDirection == ListSortDirection.Ascending)
            {
                switch (_App.GamesList.SortColumnName.ToUpper())
                {
                    case "ID":
                        sorted = srch.OrderBy(x => x.ID).ToList();
                        break;
                    case "GAME":
                        sorted = srch.OrderBy(x => x.Game).ToList();
                        break;
                    case "SYSTEM":
                        sorted = srch.OrderBy(x => x.System).ToList();
                        break;
                    case "LASTRUN":
                        sorted = srch.OrderBy(x => x.LastPlayed).ToList();
                        break;
                    case "FAV":
                        sorted = srch.OrderBy(x => x.Favorite).ToList();
                        break;
                    case "YEAR":
                        sorted = srch.OrderBy(x => x.Year).ToList();
                        break;
                    case "PLAYERS":
                        sorted = srch.OrderBy(x => x.Players).ToList();
                        break;
                    case "CO-OP":
                        sorted = srch.OrderBy(x => x.Coop).ToList();
                        break;
                    case "PUBLISHER":
                        sorted = srch.OrderBy(x => x.Publisher).ToList();
                        break;
                    case "DEVELOPER":
                        sorted = srch.OrderBy(x => x.Developer).ToList();
                        break;
                    case "RATING":
                        sorted = srch.OrderBy(x => x.ESRB).ToList();
                        break;
                    case "COUNTRY":
                        sorted = srch.OrderBy(x => x.Country).ToList();
                        break;
                    case "FLAGS":
                        sorted = srch.OrderBy(x => x.Flags).ToList();
                        break;
                    case "DETECTED ROM":
                        sorted = srch.OrderBy(x => x.DatRom).ToList();
                        break;
                    default:
                        sorted = srch.OrderBy(s => s.Game).ToList();
                        break;
                }
            }
            else
            {
                switch (_App.GamesList.SortColumnName.ToUpper())
                {
                    case "ID":
                        sorted = srch.OrderByDescending(x => x.ID).ToList();
                        break;
                    case "GAME":
                        sorted = srch.OrderByDescending(x => x.Game).ToList();
                        break;
                    case "SYSTEM":
                        sorted = srch.OrderByDescending(x => x.System).ToList();
                        break;
                    case "LASTRUN":
                        sorted = srch.OrderByDescending(x => x.LastPlayed).ToList();
                        break;
                    case "FAV":
                        sorted = srch.OrderByDescending(x => x.Favorite).ToList();
                        break;
                    case "YEAR":
                        sorted = srch.OrderByDescending(x => x.Year).ToList();
                        break;
                    case "PLAYERS":
                        sorted = srch.OrderByDescending(x => x.Players).ToList();
                        break;
                    case "CO-OP":
                        sorted = srch.OrderByDescending(x => x.Coop).ToList();
                        break;
                    case "PUBLISHER":
                        sorted = srch.OrderByDescending(x => x.Publisher).ToList();
                        break;
                    case "DEVELOPER":
                        sorted = srch.OrderByDescending(x => x.Developer).ToList();
                        break;
                    case "RATING":
                        sorted = srch.OrderByDescending(x => x.ESRB).ToList();
                        break;
                    case "COUNTRY":
                        sorted = srch.OrderByDescending(x => x.Country).ToList();
                        break;
                    case "FLAGS":
                        sorted = srch.OrderByDescending(x => x.Flags).ToList();
                        break;
                    case "DETECTED ROM":
                        sorted = srch.OrderByDescending(x => x.DatRom).ToList();
                        break;
                    default:
                        sorted = srch.OrderByDescending(s => s.Game).ToList();
                        break;
                }
            }

            */
                
            // trigger update in datagrid with sorting      
            _App.GamesList.FilteredSet = new ObservableCollection<DataGridGamesView>(srch);

            // setup cache
            _App.GamesList.FilteredSetCache = new List<DataGridGamesView>();
            _App.GamesList.FilteredSetCache = srch.ToList();

            // countryfilter
            _App.GamesList.CountryFilterUpdate();

            // re-apply datagrid column states            
            if (mw != null)
            {
                // Mainwindow has been found
                
                // re-apply column settings
                ColumnInfo.ApplyColumnInfo(dgGameList, cis);

                // refresh viewsource
                
                if (cView != null)
                {
                    //cView.SortDescriptions = sdc;
                    cView.Refresh();
                }
                    
            }
            

            return srch;
        }

        public static List<DataGridGamesView> DoSearch(List<DataGridGamesView> list, string sStr)
        {
            // check whether we need to search the gdb columns
            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (sStr == null || sStr == "")
                return list;

            List<DataGridGamesView> search = new List<DataGridGamesView>();

            search = (from g in list.Where(a => a.Game != null)
                     where g.Game.ToLower().Contains(sStr.ToLower())
                     select g).ToList();

            return search;
        }

        /// <summary>
        /// Set the update flag so that on the next local operation the data is refreshed from the database
        /// </summary>
        public static void UpdateFlag()
        {
            App _App = ((App)Application.Current);
            _App.GamesList.UpdateRequired = true;
        }
             
      
        

        /*
        public GameListBuilder(int systemId)
        {
            List<Game> games = new List<Game>();
            SearchString = "";
            FilteredSet = new List<DataGridGamesView>();

            // system id selector
            switch (systemId)
            {
                case -1:        // favorites
                    games = (from g in Game.GetGames()
                                       where g.isFavorite == true
                                       select g).ToList();
                    break;
                case 0:         // all games
                    games = Game.GetGames();
                    break;
                case -100:      // unscraped games
                    games = (from g in Game.GetGames()
                             where g.isScraped != true
                             select g).ToList();
                    break;
                default:        // based on actual system id
                    games = (from g in Game.GetGames()
                             where g.systemId == systemId
                             select g).ToList();
                    break;
            }

            // build filtered set
            foreach (var g in games)
            {
                DataGridGamesView dgv = new DataGridGamesView();

                // standard entries from GAME table in database
                dgv.ID = g.gameId;
                dgv.Game = g.gameName;
                dgv.System = GSystem.GetSystemName(g.systemId);
                dgv.Favorite = g.isFavorite;
                string lp;
                if (g.gameLastPlayed.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00")
                {
                    lp = "NEVER";
                }
                else
                {
                    lp = g.gameLastPlayed.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dgv.LastPlayed = lp;

                // add looked up entries from scraping link
                var l = GDBLink.GetRecord(dgv.ID);
                if (l != null)
                {
                    if (l.GdbId != null)
                    {
                        int gdbid = l.GdbId.Value;
                        LibraryDataGDBLink lnk = LibraryDataGDBLink.GetLibraryData(gdbid);
                        if (lnk != null)
                        {
                            dgv.Coop = lnk.Coop;
                            dgv.Developer = lnk.Developer;
                            dgv.ESRB = lnk.ESRB;
                            dgv.Players = lnk.Players;
                            dgv.Publisher = lnk.Publisher;
                            dgv.Year = lnk.Year;
                        }
                    }                   
                }                

                // add to filtered list
                FilteredSet.Add(dgv);
            }
        }

        */
        /*
        public List<DataGridGamesView> Search (string SearchString)
        {

        }
        */
    }

    public enum CountryFilter
    {
        ALL,
        USA,
        EUR,
        JPN
    }
}
