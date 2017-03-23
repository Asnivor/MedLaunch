using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.GamesLibrary;
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
        public ScrapeDB _ScrapeDB { get; set; }
        public List<ScraperMaster> SystemCollection { get; set; }
        public List<ScraperMaster> WorkingSearchCollection { get; set; }
        public List<ScraperMaster> SearchCollection { get; set; }
        public List<MedLaunch.Models.Game> LocalGames { get; set; }

        public string SearchString { get; set; }

        public ScraperLookup()
        {
            _GlobalSettings = GlobalSettings.GetGlobals();
            _ScrapeDB = new ScrapeDB();

            SearchCollection = new List<ScraperMaster>();
            WorkingSearchCollection = new List<ScraperMaster>();
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
    }
}
