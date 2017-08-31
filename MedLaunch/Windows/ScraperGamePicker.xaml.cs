using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MedLaunch.Classes;
using MedLaunch.Models;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Extensions;
using MedLaunch.Classes.MasterScraper;
using MedLaunch.Classes.Scraper;
using MedLaunch.Classes.GamesLibrary;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ListBoxChildWindow.xaml
    /// </summary>
    public partial class ScraperGamePicker : ChildWindow
    {
        public MainWindow mw { get; set; }

        public ScraperGamePicker()
        {
            this.InitializeComponent();

            this.ShowCloseButton = false;
            btnSelect.IsEnabled = false;

            // get the mainwindow
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            
           
            int GameId = mw.InspGame.gameId;
            int systemId = mw.InspGame.systemId;
            
            // check for pcecd games
            if (systemId == 18)
                systemId = 7;
               

            string sysName = GSystem.GetSystemName(systemId);
            this.Title = "Fuzzy Search Results for " + sysName;
            this.Refresh();

            ScraperSearch gs = new ScraperSearch();
            // get a list of all games for this platform - higest match first
            //List<SearchOrdering> games = gs.ShowPlatformGames(systemId, row.Game);

            GamesLibraryModel row = new GamesLibraryModel();
            row.ID = mw.InspGame.gameId;
            row.Game = mw.InspGame.gameName;

            List<SearchOrdering> games = gs.ShowPlatformGamesBySub(systemId, row);

            List< GameListItem > g = new List<GameListItem>();

            foreach (var gam in games)
            {
                if (gam.Matches == 0 || gam.Game.GDBTitle == "")
                {
                    continue;
                }
                GameListItem gli = new GameListItem();
                gli.GamesDBId = gam.Game.gid;
                gli.GameName = gam.Game.GDBTitle;
                //gli.Matches = gam.Matches;
                int wordcount = row.Game.Split(' ').Length;
                gli.Percentage = Convert.ToInt32((Convert.ToDouble(gam.Matches) / Convert.ToDouble(wordcount)) * 100);
                gli.Platform = gam.Game.GDBPlatformName;
                g.Add(gli);
            }
            // make sure list is ordered descending
            g.OrderByDescending(a => a.Matches);

            dgReturnedGames.ItemsSource = g;

        }
        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void dgReturnedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (GameListItem)dgReturnedGames.SelectedItem;
            if (row == null)
                btnSelect.IsEnabled = false;
            else
                btnSelect.IsEnabled = true;

            
        }

        private async void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };

            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");
            var r = (GamesLibraryModel)dgGameList.SelectedItem;
            var row = (GameListItem)dgReturnedGames.SelectedItem;

            var controller = await mw.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);

            ScraperHandler sh = new ScraperHandler(row.GamesDBId, r.ID);
            await Task.Delay(100);
            await Task.Run(() =>
            {
                if (controller.IsCanceled)
                {
                    controller.CloseAsync();
                    return;
                }
                // get the scraped datawebobject
                var obj = sh.ScrapeGameInspector(controller);

                mw.InspGameScrape = new Game();

                // populate the destination object
                mw.InspGameScrape.gameId = mw.InspGame.gameId;
                mw.InspGameScrape.systemId = mw.InspGame.systemId;

                StringBuilder sbAT = new StringBuilder();
                for (int i = 0; i < obj.Data.AlternateTitles.Count(); i++)
                {                    
                    sbAT.Append(obj.Data.AlternateTitles[i]);
                    if (i < (obj.Data.AlternateTitles.Count() - 1))
                        sbAT.Append(", ");
                }
                mw.InspGameScrape.AlternateTitles = sbAT.ToString();
                mw.InspGameScrape.Coop = obj.Data.Coop;
                mw.InspGameScrape.Developer = obj.Data.Developer;
                mw.InspGameScrape.ESRB = obj.Data.ESRB;
                mw.InspGameScrape.gameName = obj.Data.Title;
                mw.InspGameScrape.gdbId = obj.GdbId;
                StringBuilder sbGe = new StringBuilder();
                for (int i = 0; i < obj.Data.Genres.Count(); i++)
                {
                    sbGe.Append(obj.Data.Genres[i]);
                    if (i < (obj.Data.Genres.Count() - 1))
                        sbGe.Append(", ");
                }
                mw.InspGameScrape.Genres = sbGe.ToString();
                mw.InspGameScrape.Overview = obj.Data.Overview;
                mw.InspGameScrape.Players = obj.Data.Players;
                mw.InspGameScrape.Publisher = obj.Data.Publisher;
                mw.InspGameScrape.Year = obj.Data.Released;
                

            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
                //GamesLibraryVisualHandler.UpdateSidebar();
                //GamesLibraryView.RestoreSelectedRow();
            }
            else
            {
                //await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
                //GamesLibraryVisualHandler.UpdateSidebar();
                //GamesLibraryView.RestoreSelectedRow();
            }

            //var ro = (GamesLibraryModel)dgGameList.SelectedItem;
            //dgGameList.SelectedItem = null;
            //dgGameList.SelectedItem = ro;



            //GamesLibraryVisualHandler.RefreshGamesLibrary();
            
            this.Close();

        }
    }

    
}
