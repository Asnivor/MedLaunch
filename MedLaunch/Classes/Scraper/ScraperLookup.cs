using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.Scraper.DBModels;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes.Scraper
{
    public class ScraperLookup
    {
        public GlobalSettings _GlobalSettings { get; set; }
        //public ScrapeDB _ScrapeDB { get; set; }
        public List<MasterView> SystemCollection { get; set; }
        public List<MasterView> WorkingSearchCollection { get; set; }
        public List<MasterView> SearchCollection { get; set; }
        public List<MedLaunch.Models.Game> LocalGames { get; set; }

        public string SearchString { get; set; }

        public ScraperLookup()
        {
            _GlobalSettings = GlobalSettings.GetGlobals();
            //_ScrapeDB = new ScrapeDB();

            SearchCollection = new List<MasterView>();
            WorkingSearchCollection = new List<MasterView>();
        }


        // STATIC METHODS

        /// <summary>
        /// Choose a game from the local master list to link to an imported medlaunch game
        /// </summary>
        /// <param name="dgGameList"></param>
        public static string PickGame(DataGrid dgGameList)
        {
            // get selected row
            var row = (GamesLibraryModel)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return null;
            }
            int GameId = row.ID;
            PickLocalGame(GameId);
            return "success";
        }

        public static string PickGames(DataGrid dgGameList)
        {
            // get number of selected rows
            int numRowsCount = dgGameList.SelectedItems.Count;

            if (numRowsCount == 0)
                return null;
            else if (numRowsCount == 1)
            {
                PickGame(dgGameList);
                return "single";
            }
            else
            {
                // multiples selected
                var rs = dgGameList.SelectedItems;
                List<GamesLibraryModel> rows = new List<GamesLibraryModel>();
                foreach (GamesLibraryModel row in rs)
                {
                    rows.Add(row);
                }

                // process each row
                foreach (GamesLibraryModel row in rows)
                {
                    int GameId = row.ID;
                    PickLocalGame(GameId);
                }

                return "multiple";
            }
        }

        /// <summary>
        /// Choose a game from the local master list to link to an imported medlaunch game
        /// based on medlaunch GameId
        /// </summary>
        /// <param name="GameId"></param>
        public static async void PickLocalGame(int GameId)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");

            await mw.ShowChildWindowAsync(new ListBoxChildWindow()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Pick a Game",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        /// <summary>
        /// Choose a game from the local master list to link to an imported medlaunch game
        /// based on medlaunch GameId - SHOW IN INSPECTOR BUT DONT UPDATE THE GAME DB TABLE
        /// </summary>
        /// <param name="GameId"></param>
        public static async void PickLocalGameInspector(int GameId, MainWindow mw, RomInspector cw)
        {            
            //mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");

            await mw.ShowChildWindowAsync(new ScraperGamePicker()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Pick a Game",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);

            // populate textboxes
            cw.tbScrapeData_AlternateTitles.Text = mw.InspGameScrape.AlternateTitles;
            cw.tbScrapeData_Coop.Text = mw.InspGameScrape.Coop;
            cw.tbScrapeData_Copyright.Text = mw.InspGameScrape.Copyright;
            cw.tbScrapeData_Country.Text = mw.InspGameScrape.Country;
            cw.tbScrapeData_Developer.Text = mw.InspGameScrape.Developer;
            cw.tbScrapeData_ESRB.Text = mw.InspGameScrape.ESRB;
            cw.tbScrapeData_gameName.Text = mw.InspGameScrape.gameName;
            cw.tbScrapeData_gdbId.Text = mw.InspGameScrape.gdbId.ToString();
            cw.tbScrapeData_Genres.Text = mw.InspGameScrape.Genres;
            cw.tbScrapeData_Language.Text = mw.InspGameScrape.Language;
            cw.tbScrapeData_Overview.Text = mw.InspGameScrape.Overview;
            cw.tbScrapeData_Players.Text = mw.InspGameScrape.Players;
            cw.tbScrapeData_Publisher.Text = mw.InspGameScrape.Publisher;
            cw.tbScrapeData_Year.Text = mw.InspGameScrape.Year;
        }


    }
}
