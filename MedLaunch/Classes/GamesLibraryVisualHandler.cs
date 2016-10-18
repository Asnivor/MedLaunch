using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MedLaunch.Models;
using System.Windows.Media.Imaging;
using System.IO;

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

            Border brdSidebarScreenshots = (Border)mw.FindName("brdSidebarScreenshots");
            Border brdSidebarFanArt = (Border)mw.FindName("brdSidebarFanArt");

            // expanders
            Expander expSysInfo = (Expander)mw.FindName("expSysInfo");
            Expander expGameInfo = (Expander)mw.FindName("expGameInfo");
            Expander expGameInformation = (Expander)mw.FindName("expGameInformation");

            // labels
            Label lblGameName = (Label)mw.FindName("lblGameName");
            Label lblIsFavorite = (Label)mw.FindName("lblIsFavorite");
            Label lblLastLaunched = (Label)mw.FindName("lblLastLaunched");
            Label lblSessionLength = (Label)mw.FindName("lblSessionLength");
            Label lblTimesPlayed = (Label)mw.FindName("lblTimesPlayed");
            Label lblTotalTime = (Label)mw.FindName("lblTotalTime");

            Label lblAltNames = (Label)mw.FindName("lblAltNames");
            Label lblReleaseDate = (Label)mw.FindName("lblReleaseDate");
            Label lblPlayers = (Label)mw.FindName("lblPlayers");
            Label lblCoop = (Label)mw.FindName("lblCoop");
            Label lblGenres = (Label)mw.FindName("lblGenres");
            Label lblDeveloper = (Label)mw.FindName("lblDeveloper");
            Label lblPublisher = (Label)mw.FindName("lblPublisher");

            Label lblAltNamesTitle = (Label)mw.FindName("lblAltNamesTitle");
            Label lblReleaseDateTitle = (Label)mw.FindName("lblReleaseDateTitle");
            Label lblPlayersTitle = (Label)mw.FindName("lblPlayersTitle");
            Label lblCoopTitle = (Label)mw.FindName("lblCoopTitle");
            Label lblGenresTitle = (Label)mw.FindName("lblGenresTitle");
            Label lblDeveloperTitle = (Label)mw.FindName("lblDeveloperTitle");
            Label lblPublisherTitle = (Label)mw.FindName("lblPublisherTitle");

            // listview
            ListView Screenshots = (ListView)mw.FindName("Screenshots");

            // images
            Image imgBoxartFront = (Image)mw.FindName("imgBoxartFront");
            Image imgBoxartBack = (Image)mw.FindName("imgBoxartBack");
            Image imgBanner = (Image)mw.FindName("imgBanner");

            // lists of gamesdb related controls
            List<Image> gdbImages = new List<Image>
            {
                imgBoxartFront,
                imgBoxartBack,
                imgBanner
            };
            List<Label> gdbLabels = new List<Label>
            {
                lblAltNames,
                lblAltNamesTitle,
                lblReleaseDate,
                lblReleaseDateTitle,
                lblPlayers,
                lblPlayersTitle,
                lblCoop,
                lblCoopTitle,
                lblGenres,
                lblGenresTitle,
                lblDeveloper,
                lblDeveloperTitle,
                lblPublisher,
                lblPublisherTitle
            };

            // screenshots
            Image ss1 = (Image)mw.FindName("ss1");
            Image ss2 = (Image)mw.FindName("ss2");
            Image ss3 = (Image)mw.FindName("ss3");
            Image ss4 = (Image)mw.FindName("ss4");
            Image ss5 = (Image)mw.FindName("ss5");
            Image ss6 = (Image)mw.FindName("ss6");
            Image ss7 = (Image)mw.FindName("ss7");
            Image ss8 = (Image)mw.FindName("ss8");
            Image ss9 = (Image)mw.FindName("ss9");
            Image ss10 = (Image)mw.FindName("ss10");
            Image ss11 = (Image)mw.FindName("ss11");
            Image ss12 = (Image)mw.FindName("ss12");

            List<Image> ss = new List<Image>
            {
                ss1,ss2,ss3,ss4,ss5,ss6,ss7,ss8,ss9,ss10,ss11,ss12
            };

            foreach (Image i in ss)
            {
                i.Visibility = Visibility.Collapsed;
            }
            

            // fanart
            Image fa1 = (Image)mw.FindName("fa1");
            Image fa2 = (Image)mw.FindName("fa2");
            Image fa3 = (Image)mw.FindName("fa3");
            Image fa4 = (Image)mw.FindName("fa4");
            Image fa5 = (Image)mw.FindName("fa5");
            Image fa6 = (Image)mw.FindName("fa6");
            Image fa7 = (Image)mw.FindName("fa7");
            Image fa8 = (Image)mw.FindName("fa8");
            Image fa9 = (Image)mw.FindName("fa9");
            Image fa10 = (Image)mw.FindName("fa10");
            Image fa11 = (Image)mw.FindName("fa11");
            Image fa12 = (Image)mw.FindName("fa12");

            List<Image> fa = new List<Image>
            {
                fa1,fa2,fa3,fa4,fa5,fa6,fa7,fa8,fa9,fa10,fa11,fa12
            };
            foreach (Image i in fa)
            {
                i.Visibility = Visibility.Collapsed;
            }

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

            // Game info (from thegamesdb.net)
            // get the link table record
            GDBLink link = GDBLink.GetRecord(gameId);
            if (link == null)
            {
                // no gdb data has been scraped - hide controls
                foreach (Image i in gdbImages)
                {
                    i.Visibility = Visibility.Collapsed;
                }
                foreach (Label l in gdbLabels)
                {
                    l.Visibility = Visibility.Collapsed;
                }
                expGameInformation.Header = "thegamesdb.net Info";
                brdSidebarScreenshots.Visibility = Visibility.Collapsed;
                brdSidebarFanArt.Visibility = Visibility.Collapsed;

                
                
                return;
            }
                
            // get the GDBData record
            GDBGameData gd = GDBGameData.GetGame(link.GdbId.Value);
            if (gd == null)
                return;

            // Set the game info

            // Expander title
            expGameInformation.Header = gd.Title;

            
            // banner (just take one)
            List<string> banners = new List<string>();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + GDBGameData.JsonDeSerialize(gd.BannerLocalImages).FirstOrDefault()))
            {
                imgBanner.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + GDBGameData.JsonDeSerialize(gd.BannerLocalImages).FirstOrDefault(), UriKind.Absolute));
                //imgBanner.Height= 300;
                imgBanner.Visibility = Visibility.Visible;
            }        
            else { imgBanner.Visibility = Visibility.Collapsed; }       

            // boxart
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + gd.BoxartFrontLocalImage))
            {
                imgBoxartFront.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + gd.BoxartFrontLocalImage, UriKind.Absolute));
                //imgBoxartFront.Height = 300;
                imgBoxartFront.Visibility = Visibility.Visible;
            }
            else { imgBoxartFront.Visibility = Visibility.Collapsed; }
                
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + gd.BoxartBackLocalImage))
            {
                imgBoxartBack.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + gd.BoxartBackLocalImage, UriKind.Absolute));
                //imgBoxartBack.Height = 300;
                imgBoxartBack.Visibility = Visibility.Visible;
            }
            else { imgBoxartBack.Visibility = Visibility.Collapsed; }
                

            // labels
            lblAltNames.Content = string.Join(", ", (GDBGameData.JsonDeSerialize(gd.AlternateTitles)).ToArray());
            lblReleaseDate.Content = gd.ReleaseDate;
            lblPlayers.Content = gd.Players;
            lblCoop.Content = "";   // still to implement
            lblGenres.Content = string.Join(", ", (GDBGameData.JsonDeSerialize(gd.Genres)).ToArray());
            lblDeveloper.Content = gd.Developer;
            lblPublisher.Content = gd.Publisher;

            // set visibilities             
            foreach (Label l in gdbLabels)
            {
                l.Visibility = Visibility.Visible;
            }     
                   

            // hide things that have no data
            if ((string)lblAltNames.Content == "" || (string)lblAltNames.Content == " ")
            {
                lblAltNames.Visibility = Visibility.Collapsed;
                lblAltNamesTitle.Visibility = Visibility.Collapsed;
            }                
            if ((string)lblReleaseDate.Content == "" || (string)lblReleaseDate.Content == " ")
            {
                lblReleaseDate.Visibility = Visibility.Collapsed;
                lblReleaseDateTitle.Visibility = Visibility.Collapsed;
            }                
            if ((string)lblPlayers.Content == "" || (string)lblPlayers.Content == " ")
            {
                lblPlayers.Visibility = Visibility.Collapsed;
                lblPlayersTitle.Visibility = Visibility.Collapsed;
            }
            if ((string)lblCoop.Content == "" || (string)lblCoop.Content == "")
            {
                lblCoop.Visibility = Visibility.Collapsed;
                lblCoopTitle.Visibility = Visibility.Collapsed;
            }
            if ((string)lblGenres.Content == "" || (string)lblGenres.Content == "")
            {
                lblGenres.Visibility = Visibility.Collapsed;
                lblGenresTitle.Visibility = Visibility.Collapsed;
            }
            if ((string)lblDeveloper.Content == "" || (string)lblDeveloper.Content == " ")
            {
                lblDeveloper.Visibility = Visibility.Collapsed;
                lblDeveloperTitle.Visibility = Visibility.Collapsed;
            }
            if ((string)lblPublisher.Content == "" || (string)lblPublisher.Content == " ")
            {
                lblPublisher.Visibility = Visibility.Collapsed;
                lblPublisherTitle.Visibility = Visibility.Collapsed;
            }

            // screenshots
            if (gd.ScreenshotLocalImages != null && gd.ScreenshotLocalImages != "")
            {
                brdSidebarScreenshots.Visibility = Visibility.Visible;
                List<string> sshots = GDBGameData.JsonDeSerialize(gd.ScreenshotLocalImages);
                String[] arr = sshots.ToArray();
                int i = 0;
                while (i <= 12)
                {
                    i++;
                    // check whether we have run out of images
                    if (i > sshots.Count || sshots == null)
                    {
                        break;
                    }
                    // populate screenshot images                    
                    string path = arr[i - 1];
                    Image img = (Image)mw.FindName("ss" + i.ToString());
                    img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + path, UriKind.Absolute));
                    img.Visibility = Visibility.Visible;                   
                }               
            }
            else { brdSidebarScreenshots.Visibility = Visibility.Collapsed; }

            // fanart
            if (gd.FanartLocalImages != null && gd.FanartLocalImages != "")
            {
                brdSidebarFanArt.Visibility = Visibility.Visible;
                List<string> fshots = GDBGameData.JsonDeSerialize(gd.FanartLocalImages);
                String[] arr = fshots.ToArray();
                int i = 0;
                while (i <= 12)
                {
                    i++;
                    // check whether we have run out of images
                    if (i > fshots.Count || fshots == null)
                    {
                        break;
                    }
                    // populate screenshot images                    
                    string path = arr[i - 1];
                    Image img = (Image)mw.FindName("fa" + i.ToString());
                    img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + path, UriKind.Absolute));
                    img.Visibility = Visibility.Visible;
                }
            }
            else { brdSidebarFanArt.Visibility = Visibility.Collapsed; }




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
