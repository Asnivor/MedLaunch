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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ListBoxChildWindow.xaml
    /// </summary>
    public partial class ListBoxChildWindow : ChildWindow
    {
        public ListBoxChildWindow()
        {
            this.InitializeComponent();

            this.ShowCloseButton = false;
            btnSelect.IsEnabled = false;

            // get the mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                this.Close();
                return;
            }
            int GameId = row.ID;
            //MessageBox.Show(GameId.ToString());
            int systemId = (Game.GetGame(GameId)).systemId;

            string sysName = GSystem.GetSystemName(systemId);
            this.Title = "Fuzzy Search Results for " + sysName;
            this.Refresh();

            GameScraper gs = new GameScraper();
            // get a list of all games for this platform - higest match first
            List<SearchOrdering> games = gs.ShowPlatformGames(systemId, row.Game);

            List < GameListItem > g = new List<GameListItem>();

            foreach (var gam in games)
            {
                if (gam.Matches == 0 || gam.Game.TGDBData.GamesDBTitle == "")
                {
                    continue;
                }
                GameListItem gli = new GameListItem();
                gli.GamesDBId = gam.Game.GamesDbId;
                gli.GameName = gam.Game.TGDBData.GamesDBTitle;
                //gli.Matches = gam.Matches;
                int wordcount = row.Game.Split(' ').Length;
                gli.Percentage = Convert.ToInt32((Convert.ToDouble(gam.Matches) / Convert.ToDouble(wordcount)) * 100);
                gli.Platform = gam.Game.TGDBData.GamesDBPlatformName;
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

            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");
            var r = (DataGridGamesView)dgGameList.SelectedItem;

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = false
            };
            var row = (GameListItem)dgReturnedGames.SelectedItem;
            int gdbId = row.GamesDBId;



            var controller = await mw.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            controller.SetMessage("Creating links...");
            GDBLink link = new GDBLink();
            link.GameId = r.ID;
            link.GdbId = gdbId;

            // delete any existing links with the same GameId
            GDBLink l = GDBLink.GetRecord(r.ID);
            if (l != null)
            {
                GDBLink.DeleteRecord(l);
            }

            GDBLink.SaveToDatabase(link);
            await Task.Delay(100);

            string message = "Connecting to thegamesdb.net....\nGetting data for: " + row.GameName;
            controller.SetMessage(message);
            GameScraper gs = new GameScraper();
            await Task.Delay(100);
            await Task.Run(() =>
            {
                if (controller.IsCanceled)
                {
                    controller.CloseAsync();
                    return;
                }
                gs.ScrapeGame(gdbId, controller, message);
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
            }

            var ro = (DataGridGamesView)dgGameList.SelectedItem;
            dgGameList.SelectedItem = null;
            dgGameList.SelectedItem = ro;

            this.Close();

        }
    }

    public class GameListItem
    {
        public int GamesDBId { get; set; }
        public string GameName { get; set; }
        public int Matches { get; set; }
        public int Percentage { get; set; }
        public string Platform { get; set; }
    }
}
