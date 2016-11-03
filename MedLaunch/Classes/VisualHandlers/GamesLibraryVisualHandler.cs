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
using MedLaunch.Extensions;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

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

            // set column definitions
            ColumnDefinition cd = (ColumnDefinition)mw.FindName("sidebarColumn");
            ColumnDefinition colBoxartFront = (ColumnDefinition)mw.FindName("colBoxartFront");
            ColumnDefinition colBoxartBack = (ColumnDefinition)mw.FindName("colBoxartBack");
            ColumnDefinition colMedia = (ColumnDefinition)mw.FindName("colMedia");
            
            cd.Width = new GridLength(0.3, GridUnitType.Star);
            //colBoxartFront.Width = new GridLength(0.5, GridUnitType.Star);
            //colBoxartBack.Width = new GridLength(0.5, GridUnitType.Star);
            //colMedia.Width = new GridLength(0.5, GridUnitType.Star);

            UniformGrid gridManuals = (UniformGrid)mw.FindName("gridManuals");

            // borders
            Border brdSysInfo = (Border)mw.FindName("brdSidebarSystem");                            // game system info
            Border brdSidebarGameInformation = (Border)mw.FindName("brdSidebarGameInformation");    // main scraping info container
            brdSidebarGameInformation.Visibility = Visibility.Visible;

            Border brdGame = (Border)mw.FindName("brdSidebarGame");                                 
            Border brdSidebarScreenshots = (Border)mw.FindName("brdSidebarScreenshots");
            Border brdSidebarFanArt = (Border)mw.FindName("brdSidebarFanArt");
            Border brdSidebarOverview = (Border)mw.FindName("brdSidebarOverview");

            Border brdManuals = (Border)mw.FindName("brdManuals");

            // expanders
            Expander expSysInfo = (Expander)mw.FindName("expSysInfo");
            Expander expGameInfo = (Expander)mw.FindName("expGameInfo");
            Expander expGameInformation = (Expander)mw.FindName("expGameInformation");
            Expander expOverview = (Expander)mw.FindName("expOverview");
            Expander expManuals = (Expander)mw.FindName("expManuals");

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

            TextBlock tbOverview = (TextBlock)mw.FindName("tbOverview");


            // listview
            ListView Screenshots = (ListView)mw.FindName("Screenshots");

            // images
            Image imgBoxartFront = (Image)mw.FindName("imgBoxartFront");
            Image imgBoxartBack = (Image)mw.FindName("imgBoxartBack");
            Image imgBanner = (Image)mw.FindName("imgBanner");
            Image imgMedia = (Image)mw.FindName("imgMedia");

            // lists of gamesdb related controls
            List<Image> gdbImages = new List<Image>
            {
                imgBoxartFront,
                imgBoxartBack,
                imgBanner,
                imgMedia
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
            Label lblSysName = (Label)mw.FindName("lblSystemName");
            Image sysImage = (Image)mw.FindName("imgSystemImage");
            TextBlock tblockSysImage = (TextBlock)mw.FindName("tblockSystemDesc");

            string imageFileName = @"Data\Graphics\Systems\" + lsb.SystemCode.ToLower() + ".jpg";

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
            expSysInfo.Header = lsb.SystemName;
            tblockSysImage.Text = lsb.SystemDescription;
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
                expGameInformation.Header = "Scraped Information";
                brdSidebarGameInformation.Visibility = Visibility.Collapsed;
                brdSidebarScreenshots.Visibility = Visibility.Collapsed;
                brdSidebarFanArt.Visibility = Visibility.Collapsed;
                brdSidebarOverview.Visibility = Visibility.Collapsed;
                brdManuals.Visibility = Visibility.Collapsed;
                return;
            }

            // get the on-disk scraped information for this game
            //GDBGameData gd = GDBGameData.GetGame(link.GdbId.Value);
            GamesLibraryScrapedContent gd = new GamesLibraryScrapedContent();
            ScrapedGameObject o = gd.GetScrapedGameObject(gameId);
            if (gd == null)
                return;

            // Set the game info

            // Expander title
            expGameInformation.Header = o.Data.Title;

            
            // banner (just take one)
            List<string> banners = new List<string>();
            SetImage(imgBanner, o.Banners.FirstOrDefault(), UriKind.Absolute);
            imgBanner.SetVisibility();

            // boxart - front
            SetImage(imgBoxartFront, o.FrontCovers.FirstOrDefault(), UriKind.Absolute);
            imgBoxartFront.SetVisibility();
            
            // boxart - back
            SetImage(imgBoxartBack, o.BackCovers.FirstOrDefault(), UriKind.Absolute);   
            imgBoxartBack.SetVisibility();

            // media image
            SetImage(imgMedia, o.Medias.FirstOrDefault(), UriKind.Absolute);
            imgMedia.SetVisibility();
            
            // labels
            if (o.Data.AlternateTitles != null)
                lblAltNames.Content = string.Join(", ", (o.Data.AlternateTitles).ToArray());
            lblReleaseDate.Content = o.Data.Released;
            lblPlayers.Content = o.Data.Players;
            lblCoop.Content = o.Data.Coop;
            if (o.Data.Genres != null)
                lblGenres.Content = string.Join(", ", (o.Data.Genres).ToArray());
            lblDeveloper.Content = o.Data.Developer;
            lblPublisher.Content = o.Data.Publisher;
            tbOverview.Text = o.Data.Overview;

            // set visibilities             
            foreach (Label l in gdbLabels)
            {
                l.SetVisibility();
            }
            if (tbOverview.Text == "")
                brdSidebarOverview.Visibility = Visibility.Collapsed;
            else { brdSidebarOverview.Visibility = Visibility.Visible; }

            // hide things that have no data
            if (lblAltNames.Content == null)
            {
                lblAltNamesTitle.Visibility = Visibility.Collapsed;
                }
            if (lblReleaseDate.Content == null)
                 {
               lblReleaseDateTitle.Visibility = Visibility.Collapsed;
              }
            if (lblPlayers.Content == null)
                {
                lblPlayersTitle.Visibility = Visibility.Collapsed;
               }
            if (lblCoop.Content == null)
                {
                lblCoopTitle.Visibility = Visibility.Collapsed;
                }
           if (lblGenres.Content == null)
               {
               lblGenresTitle.Visibility = Visibility.Collapsed;
               }
           if (lblDeveloper.Content == null)
                {
               lblDeveloperTitle.Visibility = Visibility.Collapsed;
              }
           if (lblPublisher.Content == null)
             {
               lblPublisherTitle.Visibility = Visibility.Collapsed;
             }

            // screenshots
            if (o.Screenshots != null && o.Screenshots.Count > 0)
            {
                brdSidebarScreenshots.Visibility = Visibility.Visible;
                List<string> sshots = o.Screenshots;
                String[] arr = sshots.ToArray();
                int i = 0;
                while (i < 12)
                {                    
                    // check whether we have run out of images
                    if (i >= sshots.Count || sshots == null)
                    {
                        break;
                    }
                    // populate screenshot images                    
                    string path = arr[i];
                    Image img = (Image)mw.FindName("ss" + (i + 1).ToString());
                    SetImage(img, path, UriKind.Absolute);
                    img.SetVisibility();
                    i++;
                }               
            }
            else { brdSidebarScreenshots.Visibility = Visibility.Collapsed; }

            // fanart
            if (o.FanArts != null && o.FanArts.Count > 0)
            {
                brdSidebarFanArt.Visibility = Visibility.Visible;
                List<string> fshots = o.FanArts;
                String[] arr = fshots.ToArray();
                int i = 0;
                while (i < 12)
                {
                    
                    // check whether we have run out of images
                    if (i >= fshots.Count || fshots == null)
                    {
                        break;
                    }
                    // populate screenshot images                    
                    string path = arr[i];
                    Image img = (Image)mw.FindName("fa" + (i + 1).ToString());
                    SetImage(img, path, UriKind.Absolute);
                    img.SetVisibility();
                    i++;
                }
            }
            else { brdSidebarFanArt.Visibility = Visibility.Collapsed; }

            // manuals
            if (o.Manuals != null && o.Manuals.Count > 0)
            {
                brdManuals.Visibility = Visibility.Visible;
                // get number of manuals in the directory
                int manCount = o.Manuals.Count;
                // disable buttons that are not needed and setup the buttons that are required
                int c = 1;
                while (c <= 20)
                {
                    Button b = (Button)mw.FindName("btnMan" + c);
                    if ((c - 1) < manCount)
                    {
                        // activate button
                        b.Visibility = Visibility.Visible;
                        // get just filename
                        string path = Path.GetFileName(o.Manuals[c - 1]);
                        b.Content = path; // "Man " + c.ToString();
                        b.ToolTip = o.Manuals[c - 1];
                        b.FontSize = 8;
                    }
                    else
                    {
                        // hide button
                        b.Visibility = Visibility.Collapsed;
                    }
                    c++;
                }
            }
            else { brdManuals.Visibility = Visibility.Collapsed; }

        }

        public static void SetImage(Image img, string path, UriKind urikind)
        {            
            if (!File.Exists(path))
                return;
            try
            {
                // load content into the image
                BitmapImage b = new BitmapImage(new Uri(path, urikind));
                img.Source = b;

                // get actual pixel dimensions of image
                double pixelWidth = (img.Source as BitmapSource).PixelWidth;
                double pixelHeight = (img.Source as BitmapSource).PixelHeight;

                // get dimensions of main window
                MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                double windowWidth = mw.ActualWidth;
                double windowHeight = mw.ActualHeight;

                // set max dimensions on Image.ToolTip
                ToolTip tt = (ToolTip)img.ToolTip;
                tt.MaxHeight = windowHeight / 1.1;
                tt.MaxWidth = windowWidth / 1.1;
                img.ToolTip = tt;
            }

            catch (System.NotSupportedException ex)
            {
                img.Source = new BitmapImage();
            }
        }

        public static BitmapImage GetBitmapImageFromDisk(string path, UriKind urikind)
        {
            if (!File.Exists(path))
                return null;
            try
            {
                BitmapImage b = new BitmapImage(new Uri(path, urikind));                
                return b;
            }
            catch (System.NotSupportedException ex)
            {
                BitmapImage c = new BitmapImage();
                return c;
            }        
        }

        public static void ResizeImage(Image img, double maxWidth, double maxHeight)
        {
            if (img == null || img.Source == null)
                return;

            double srcWidth = img.Source.Width;
            double srcHeight = img.Source.Height;

            // Set your image tag to the sources DPI value for smart resizing if DPI != 96
            if (img.Tag != null && img.Tag.GetType() == typeof(double[]))
            {
                double[] DPI = (double[])img.Tag;
                srcWidth = srcWidth / (96 / DPI[0]);
                srcHeight = srcHeight / (96 / DPI[1]);
            }

            double resizedWidth = srcWidth;
            double resizedHeight = srcHeight;

            double aspect = srcWidth / srcHeight;

            if (resizedWidth > maxWidth)
            {
                resizedWidth = maxWidth;
                resizedHeight = resizedWidth / aspect;
            }
            if (resizedHeight > maxHeight)
            {
                aspect = resizedWidth / resizedHeight;
                resizedHeight = maxHeight;
                resizedWidth = resizedHeight * aspect;
            }

            img.Width = resizedWidth;
            img.Height = resizedHeight;
        }

        // convert image to 96DPI
        public static BitmapSource ConvertBitmapTo96DPI(BitmapImage bitmapImage)
        {
            double dpi = 96;
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            int stride = width * bitmapImage.Format.BitsPerPixel;
            byte[] pixelData = new byte[stride * height];
            bitmapImage.CopyPixels(pixelData, stride, 0);

            return BitmapSource.Create(width, height, dpi, dpi, bitmapImage.Format, null, pixelData, stride);
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

        public  static List<Expander> GetExpanderControls()
        {
            // get the mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MedLaunch.MainWindow>().FirstOrDefault();
            // get all the expander objects
            Expander expGameInfo = (Expander)mw.FindName("expGameInfo");                    // GameStats
            Expander expGameInformation = (Expander)mw.FindName("expGameInformation");      // GamesDB games info
            Expander expOverview = (Expander)mw.FindName("expOverview");                    // overview
            Expander expScreenshots = (Expander)mw.FindName("expScreenshots");              // screenshots
            Expander expFanArt = (Expander)mw.FindName("expFanArt");                        // fanart
            Expander expScrapingOptions = (Expander)mw.FindName("expScrapingOptions");      // scraping options
            Expander expSysInfo = (Expander)mw.FindName("expSysInfo");                      // system info
            Expander expManuals = (Expander)mw.FindName("expManuals");                      // game manuals and documents

            // add controls to the List
            List<Expander> exp = new List<Expander>
            {
                expGameInfo,
                expGameInformation,
                expOverview,
                expScreenshots,
                expFanArt,
                expScrapingOptions,
                expSysInfo,
                expManuals
            };
            return exp;
        }

        public static void LoadExpanderStates()
        {
            // get the expander controls
            List<Expander> expanders = GetExpanderControls();

            // get a globalsettings object
            GlobalSettings gs = GlobalSettings.GetGlobals();

            foreach (Expander e in expanders)
            {
                string name = e.Name;
                switch (name)
                {
                    case "expGameInfo":
                        e.IsExpanded = gs.glGameStats;
                        break;
                    case "expGameInformation":
                        e.IsExpanded = gs.glGameInfo;
                        break;
                    case "expOverview":
                        e.IsExpanded = gs.glOverview;
                        break;
                    case "expScreenshots":
                        e.IsExpanded = gs.glScreenshots;
                        break;
                    case "expFanArt":
                        e.IsExpanded = gs.glFanart;
                        break;
                    case "expScrapingOptions":
                        e.IsExpanded = gs.glScrapingOptions;
                        break;
                    case "expSysInfo":
                        e.IsExpanded = gs.glSystemInfo;
                        break;
                    case "expManuals":
                        e.IsExpanded = gs.glManuals;
                        break;
                }
            }
        }

        public static void SaveExpanderStates()
        {
            // get the expander controls
            List<Expander> expanders = GetExpanderControls();

            // get a globalsettings object
            GlobalSettings gs = GlobalSettings.GetGlobals();

            foreach (Expander e in expanders)
            {
                string name = e.Name;
                switch (name)
                {
                    case "expGameInfo":
                        gs.glGameStats = e.IsExpanded;
                        break;
                    case "expGameInformation":
                        gs.glGameInfo = e.IsExpanded;
                        break;
                    case "expOverview":
                        gs.glOverview = e.IsExpanded;
                        break;
                    case "expScreenshots":
                        gs.glScreenshots = e.IsExpanded;
                        break;
                    case "expFanArt":
                        gs.glFanart = e.IsExpanded;
                        break;
                    case "expScrapingOptions":
                        gs.glScrapingOptions = e.IsExpanded;
                        break;
                    case "expSysInfo":
                        gs.glSystemInfo = e.IsExpanded;
                        break;
                    case "expManuals":
                        gs.glManuals = e.IsExpanded;
                        break;
                }
            }
            // save to database
            GlobalSettings.SetGlobals(gs);
        }

        public static void RefreshSideBar(DataGrid dgGameList)
        {
            // get the selected item
            var r = (DataGridGamesView)dgGameList.SelectedItem;
            int gameId = r.ID;
            
        }

    }
}
