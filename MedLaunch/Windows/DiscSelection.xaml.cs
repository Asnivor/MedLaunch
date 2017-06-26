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
using MedLaunch.Classes.Scanning;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ListBoxChildWindow.xaml
    /// </summary>
    public partial class DiscSelection : ChildWindow
    {
        public string LaunchString { get; set; }
        public string[] DiscArray { get; set; }

        public DiscSelection()
        {
            this.InitializeComponent();

            this.ShowCloseButton = false;
            btnSelect.IsEnabled = false;

            // get the mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            int numRowsCount = mw.dgGameList.SelectedItems.Count;
            if (numRowsCount != 1)
                return;

            GamesLibraryModel drv = (GamesLibraryModel)mw.dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;

            // create new GameLauncher instance
            GameLauncher gl = new GameLauncher(romId);
            LaunchString = gl.GetCommandLineArguments();

            // choose disc
            string path = Game.GetGame(romId).gamePath;
            string[] sheets = DiscScan.ParseM3UFile(path);

            if (sheets == null || sheets.Length == 0)
            {
                MessageBox.Show("ERROR: Track Sheets Could Not be Parsed.");
                this.Close();
            }
            
            DiscArray = sheets;

            this.Title = "Choose Disc to Launch";
            this.Refresh();

            List <DSel> g = new List<DSel>();

            for(int i = 0; i < DiscArray.Length; i++)
            {
                DSel ds = new DSel();
                ds.DiscNumber = i;
                ds.DiscName = DiscArray[i];

                g.Add(ds);
            }
            // make sure list is ordered
            g.OrderBy(a => a.DiscNumber);
            dgReturnedGames.ItemsSource = g;

        }
        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgReturnedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (DSel)dgReturnedGames.SelectedItem;
            if (row == null)
                btnSelect.IsEnabled = false;
            else
                btnSelect.IsEnabled = true;

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            
            var row = (DSel)dgReturnedGames.SelectedItem;

            // add the which_medium command selected

            if (row != null)
            {
                LaunchString = LaunchString.Replace("-autosave ", "-which_medium " + row.DiscNumber.ToString() + " -autosave ");
            }

            else
            {
                MessageBox.Show("Invalid Disc. Press OK to Cancel");
                this.Close();
            }

            string compat = Versions.GetCompatLaunchString(LaunchString);

            mw.LaunchRomHandler(compat, false);

            this.Close();

        }
    }

    public class DSel
    {
        public int DiscNumber { get; set; }
        public string DiscName { get; set; }
    }
}
