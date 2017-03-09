using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GameListBuilder : INotifyPropertyChanged
    {
        // properties

        public string SortColumnName { get; set; }
        public ListSortDirection SortDirection { get; set; }

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

            // set initial sorting
            SortColumnName = "GAME";
            SortDirection = ListSortDirection.Ascending;
            
            UpdateRequired = false;
        }

        protected void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public static void ReLinkData()
        {
            /*
            var allLib = LibraryDataGDBLink.GetLibraryData();

            using (var db = new MyDbContext())
            {
                
                var allLinks = GDBLink.GetAllRecords().ToList();

                List<LibraryDataGDBLink> linksToAdd = new List<LibraryDataGDBLink>();

                // iterate through each entry in allLinks
                foreach (var l in allLinks)
                {
                    // try to match with the library link
                    var ll = allLib.Where(a => a.GDBId == l.GdbId).ToList();
                    if (ll.Count == 0)
                    {
                        // no entry was found - this needs linking
                        GamesLibraryScrapedContent gd = new GamesLibraryScrapedContent();
                        ScrapedGameObject o = gd.GetScrapedGameObject(l.GameId.Value);
                        if (o == null)
                            continue;

                        LibraryDataGDBLink d = new LibraryDataGDBLink();
                        d.GDBId = l.GdbId.Value;
                        d.Coop = o.Data.Coop;
                        d.Developer = o.Data.Developer;
                        d.ESRB = o.Data.ESRB;
                        d.Players = o.Data.Players;
                        d.Publisher = o.Data.Publisher;
                        d.Year = o.Data.Released;

                        linksToAdd.Add(d);
                        LibraryDataGDBLink.SaveToDataBase(d);
                        
                    }

                    // make sure GAME entry has scraped flag set
                    Game g = Game.GetGame(l.GameId.Value);
                    g.isScraped = true;
                    Game.SetGame(g, true);
                }
            }
            */
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

        public static List<DataGridGamesView> Filter(int systemId, string search)
        {
            List<DataGridGamesView> results = new List<DataGridGamesView>();
            App _App = ((App)Application.Current);

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

            // now we have results based on the system filter - process file search
            List<DataGridGamesView> srch = DoSearch(results, search);

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
                
            // trigger update in datagrid with sorting      
            _App.GamesList.FilteredSet = new ObservableCollection<DataGridGamesView>(sorted);

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
}
