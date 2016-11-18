using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GameListBuilder
    {
        // properties
        public List<DataGridGamesView> FilteredSet { get; set; }
        public List<DataGridGamesView> AllGames { get; set; }
        public int SystemId { get; set; }
        public string SearchString { get; set; }
        public bool UpdateRequired { get; set; }

        // constructors

        public GameListBuilder()
        {
            AllGames = new List<DataGridGamesView>();
            UpdateRequired = true;
            using (var cnt = new MyDbContext())
            {
                List<LibraryDataGDBLink> lib = (from all in cnt.LibraryDataGDBLink
                                                select all).ToList();
                var ag = (from game in cnt.Game
                          from link in cnt.GDBLink
                          .Where(v => v.GameId == game.gameId)
                          .DefaultIfEmpty()
                          select new DataGridGamesView
                          {
                              ID = game.gameId,
                              Game = game.gameName,
                              System = GSystem.GetSystemName(game.systemId),
                              LastPlayed = DbEF.FormatDate(game.gameLastPlayed),
                              Favorite = game.isFavorite,

                              Publisher = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Publisher, 
                              Developer = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Developer,
                              Year = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Year,
                              Players = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Players,
                              Coop = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Coop,
                              ESRB = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().ESRB                              
                          }).ToList();
                
                AllGames = ag;
            }
            UpdateRequired = false;
        }


        public static List<DataGridGamesView> Filter(int systemId, string search)
        {
            List<DataGridGamesView> results = new List<DataGridGamesView>();
            App _App = ((App)Application.Current);

            if (_App.GamesList.UpdateRequired == true)
                GameListBuilder.Update();

            // system id selector
            switch (systemId)
            {
                case -1:        // favorites
                    results = (from g in _App.GamesList.AllGames
                               where g.Favorite == true
                             select g).ToList();
                    break;
                case 0:         // all games
                    results = _App.GamesList.AllGames;
                    break;
                case -100:      // unscraped games
                    // ignore for now
                    results = _App.GamesList.AllGames;
                    break;
                default:        // based on actual system id
                    results = (from g in _App.GamesList.AllGames
                               where GSystem.GetSystemId(g.System) == systemId
                             select g).ToList();
                    break;
            }

            // now we have results based on the system filter - process file search
            List<DataGridGamesView> srch = DoSearch(results, search);
            return srch;
        }

        public static List<DataGridGamesView> DoSearch(List<DataGridGamesView> list, string sStr)
        {
            // check whether we need to search the gdb columns
            GlobalSettings gs = GlobalSettings.GetGlobals();

            List<DataGridGamesView> search = new List<DataGridGamesView>();

            search = (from g in list
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
      
        public static void Update()
        {
            using (var cnt = new MyDbContext())
            {
                App _App = ((App)Application.Current);

                List<LibraryDataGDBLink> lib = (from all in cnt.LibraryDataGDBLink
                                                select all).ToList();
                var ag = (from game in cnt.Game
                          from link in cnt.GDBLink
                          .Where(v => v.GameId == game.gameId)
                          .DefaultIfEmpty()
                          select new DataGridGamesView
                          {
                              ID = game.gameId,
                              Game = game.gameName,
                              System = GSystem.GetSystemName(game.systemId),
                              LastPlayed = game.gameLastPlayed.ToString(),
                              Favorite = game.isFavorite,
                              Publisher = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Publisher,
                              Developer = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Developer,
                              Year = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Year,
                              Players = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Players,
                              Coop = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Coop,
                              ESRB = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().ESRB
                          }).ToList();

               
                _App.GamesList.AllGames = ag;
            }                
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
