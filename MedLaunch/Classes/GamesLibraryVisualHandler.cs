using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MedLaunch.Models;
using System.Windows.Media.Imaging;

namespace MedLaunch.Classes
{
    public static class GamesLibraryVisualHandler
    {
        public static void UpdateSidebar()
        {
            // no gameId specified - hide everything
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            ScrollViewer sv = (ScrollViewer)mw.FindName("srcSidebar");
            sv.Visibility = Visibility.Collapsed;
            ColumnDefinition cd = (ColumnDefinition)mw.FindName("sidebarColumn");
            cd.Width = new GridLength(0);
        }

        // update sidebar
        public static void UpdateSidebar(int gameId)
        {
                   
            // get an instance of the MainWindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // new instance of the LibrarySidebar class
            LibrarySidebar lsb = new LibrarySidebar(gameId);

            /* define all controls that we want to set */

            // scrollviewer
            ScrollViewer sv = (ScrollViewer)mw.FindName("srcSidebar");

            // return out if hidesiderbar db field is true
            if (GlobalSettings.GetHideSidebar() == true)
            {
                sv.Visibility = Visibility.Collapsed;
                return;
            }

            sv.Visibility = Visibility.Visible;
            ColumnDefinition cd = (ColumnDefinition)mw.FindName("sidebarColumn");
            cd.Width = new GridLength(0.3, GridUnitType.Star);

            // borders
            Border brdSysInfo = (Border)mw.FindName("brdSidebarSystem");
            Border brdGame = (Border)mw.FindName("brdSidebarGame");

            // expanders
            Expander expSysInfo = (Expander)mw.FindName("expSysInfo");
            Expander expGameInfo = (Expander)mw.FindName("expGameInfo");

            // labels
            Label lblGameName = (Label)mw.FindName("lblGameName");
            Label lblIsFavorite = (Label)mw.FindName("lblIsFavorite");
            Label lblLastLaunched = (Label)mw.FindName("lblLastLaunched");
            Label lblSessionLength = (Label)mw.FindName("lblSessionLength");
            Label lblTimesPlayed = (Label)mw.FindName("lblTimesPlayed");
            Label lblTotalTime = (Label)mw.FindName("lblTotalTime");

            // system info
            //FlowDocumentScrollViewer flowSysScroll = (FlowDocumentScrollViewer)mw.FindName("flowSystemInfo");
            Label lblSysName = (Label)mw.FindName("lblSystemName");
            Image sysImage = (Image)mw.FindName("imgSystemImage");
            TextBlock tblockSysImage = (TextBlock)mw.FindName("tblockSystemDesc");

            string imageFileName = @"Data\Graphics\Systems\" + lsb.SystemCode.ToLower() + ".jpg";
            //MessageBox.Show(imageFileName);
            // update controls

            // game stats
            lblGameName.Content = lsb.GameName;
            if (lsb.IsFavorite == true)
                lblIsFavorite.Content = "YES";
            else
                lblIsFavorite.Content = "NO";

            string lp = lsb.LastPlayed.ToString("yyyy-MM-dd HH:mm");
            if (lp == "0001-01-01 00:00")
                lblLastLaunched.Content = "Never";
            else
            {
                lblLastLaunched.Content = lp;
            }

            // times played
            lblTimesPlayed.Content = lsb.TimesPlayed;

            // last session time
            lblSessionLength.Content = GetDatetimeDifference(lsb.LastPlayed, lsb.LastFinished);
            // Total game time
            lblTotalTime.Content = FormatMinutesToString(lsb.TotalPlayTime);

            // system info
            
            //lblSysName.Content = lsb.SystemName;
            expSysInfo.Header = lsb.SystemName;
            tblockSysImage.Text = lsb.SystemDescription;
           // sysImage.Source = new BitmapImage(new Uri(@"/Data/Graphics/Systems/" + imageFileName, UriKind.Relative));
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.UriSource = new Uri(imageFileName, UriKind.Relative);
            s.EndInit();
            sysImage.Source = s;

        }

        public static string GetDatetimeDifference(DateTime older, DateTime newer)
        {
            TimeSpan t = newer - older;
            if (t.TotalMinutes > 0)
                return FormatMinutesToString(t.TotalMinutes);
            else
                return "Never";
        }

        public static string FormatMinutesToString(double minutes)
        {
            string tt = "";
            TimeSpan ts = TimeSpan.FromMinutes(minutes);
            int hh = ts.Hours;
            int mm = ts.Minutes;
            int ss = ts.Seconds;
            if (minutes <= 0)
                tt = "Never";
            else
            {
                if (hh > 0)
                    tt += hh + " Hours, ";
                if (mm > 0)
                    tt += mm + " Minutes, ";
                tt += ss + " Seconds";
            }
            return tt;
        }

        // Get the current setup of the games library (selected filters etc), do a refresh then return to previous configuration
        public static void RefreshGamesLibrary()
        {
            // get an instance of the MainWindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get btnClearFilters button
            Button btnClearFilters = (Button)mw.FindName("btnClearFilters");

            // get the wrappanel that contains all the GameLibrary toggleboxes
            Grid grdGameLibrary = (Grid)mw.FindName("grdGameLibrary");

            // get the dynamic filter textbox tbFilterDatagrid
            TextBox tbFilterDatagrid = (TextBox)mw.FindName("tbFilterDatagrid");
            string tbText = tbFilterDatagrid.Text;

            // get all grouped radio buttons
            List<RadioButton> buttons = UIHandler.GetLogicalChildCollection<RadioButton>(grdGameLibrary);

            foreach (RadioButton but in buttons)
            {
                //MessageBox.Show(but.Name);
            }

            // get the radio button that is checked
            RadioButton rtTarget = buttons
                .Where(r => r.IsChecked == true).Single();

            // get the showall button
            RadioButton btnShowAll = buttons
                .Where(r => r.Name == "btnShowAll").Single();


            // Clear all settings
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Text = "1337";

            // restore settings
            rtTarget.IsChecked = true;
            tbFilterDatagrid.Text = tbText;

        }

    }
}
