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
using System.Windows.Input;
using MedLaunch.Classes.GamesLibrary;
using System.Windows.Data;
using MedLaunch.Classes.Scraper;
using System.Windows.Threading;

namespace MedLaunch.Classes
{
    public static class GamesLibraryVisualHandler
    {

        public static string ConvertDateString(DateTime dt)
        {
            string lp;
            if (dt.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00")
            {
                lp = "NEVER";
            }
            else
            {
                lp = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return lp;
        }
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
        public static async void UpdateSidebar(int gameId)
        {
            // if gameid does not exist (ie it has been deleted) then return
            var ga = Game.GetGame(gameId);
            if (ga == null)
                return;
                   
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
            UniformGrid ugridScreenshots = (UniformGrid)mw.FindName("ugridScreenshots");
            UniformGrid ugridFanarts = (UniformGrid)mw.FindName("ugridFanarts");

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
            Label lblESRB = (Label)mw.FindName("lblESRB");

            Label lblAltNamesTitle = (Label)mw.FindName("lblAltNamesTitle");
            Label lblReleaseDateTitle = (Label)mw.FindName("lblReleaseDateTitle");
            Label lblPlayersTitle = (Label)mw.FindName("lblPlayersTitle");
            Label lblCoopTitle = (Label)mw.FindName("lblCoopTitle");
            Label lblGenresTitle = (Label)mw.FindName("lblGenresTitle");
            Label lblDeveloperTitle = (Label)mw.FindName("lblDeveloperTitle");
            Label lblPublisherTitle = (Label)mw.FindName("lblPublisherTitle");
            Label lblESRBTitle = (Label)mw.FindName("lblESRBTitle");

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
            Image ss13 = (Image)mw.FindName("ss13");
            Image ss14 = (Image)mw.FindName("ss14");
            Image ss15 = (Image)mw.FindName("ss15");
            Image ss16 = (Image)mw.FindName("ss16");
            Image ss17 = (Image)mw.FindName("ss17");
            Image ss18 = (Image)mw.FindName("ss18");
            Image ss19 = (Image)mw.FindName("ss19");
            Image ss20 = (Image)mw.FindName("ss20");
            Image ss21 = (Image)mw.FindName("ss21");
            Image ss22 = (Image)mw.FindName("ss22");
            Image ss23 = (Image)mw.FindName("ss23");
            Image ss24 = (Image)mw.FindName("ss24");
            Image ss25 = (Image)mw.FindName("ss25");
            Image ss26 = (Image)mw.FindName("ss26");
            Image ss27 = (Image)mw.FindName("ss27");
            Image ss28 = (Image)mw.FindName("ss28");
            Image ss29 = (Image)mw.FindName("ss29");
            Image ss30 = (Image)mw.FindName("ss30");
            Image ss31 = (Image)mw.FindName("ss31");
            Image ss32 = (Image)mw.FindName("ss32");
            Image ss33 = (Image)mw.FindName("ss33");
            Image ss34 = (Image)mw.FindName("ss34");
            Image ss35 = (Image)mw.FindName("ss35");
            Image ss36 = (Image)mw.FindName("ss36");

            List<Image> ss = new List<Image>
            {
                ss1,ss2,ss3,ss4,ss5,ss6,ss7,ss8,ss9,ss10,ss11,ss12,ss13,ss14,ss15,ss16,ss17,ss18,ss19,ss20,ss21,ss22,ss23,ss24,ss25,ss26,ss27,ss28,ss29,ss30,ss31,ss32,ss33,ss34,ss35,ss36
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
            Image fa13 = (Image)mw.FindName("fa13");
            Image fa14 = (Image)mw.FindName("fa14");
            Image fa15 = (Image)mw.FindName("fa15");
            Image fa16 = (Image)mw.FindName("fa16");
            Image fa17 = (Image)mw.FindName("fa17");
            Image fa18 = (Image)mw.FindName("fa18");
            Image fa19 = (Image)mw.FindName("fa19");
            Image fa20 = (Image)mw.FindName("fa20");
            Image fa21 = (Image)mw.FindName("fa21");
            Image fa22 = (Image)mw.FindName("fa22");
            Image fa23 = (Image)mw.FindName("fa23");
            Image fa24 = (Image)mw.FindName("fa24");
            Image fa25 = (Image)mw.FindName("fa25");
            Image fa26 = (Image)mw.FindName("fa26");
            Image fa27 = (Image)mw.FindName("fa27");
            Image fa28 = (Image)mw.FindName("fa28");
            Image fa29 = (Image)mw.FindName("fa29");
            Image fa30 = (Image)mw.FindName("fa30");
            Image fa31 = (Image)mw.FindName("fa31");
            Image fa32 = (Image)mw.FindName("fa32");
            Image fa33 = (Image)mw.FindName("fa33");
            Image fa34 = (Image)mw.FindName("fa34");
            Image fa35 = (Image)mw.FindName("fa35");
            Image fa36 = (Image)mw.FindName("fa36");

            List<Image> fa = new List<Image>
            {
                fa1,fa2,fa3,fa4,fa5,fa6,fa7,fa8,fa9,fa10,fa11,fa12,fa13,fa14,fa15,fa16,fa17,fa18,fa19,fa20,fa21,fa22,fa23,fa24,fa25,fa26,fa27,fa28,fa29,fa30,fa31,fa32,fa33,fa34,fa35,fa36
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

            // Game info (scraped)
            // get the link table record

            ScrapeDB gd = new ScrapeDB();
            
            Game game = Game.GetGame(gameId);
            

            if (game.gdbId == null || game.gdbId == 0)
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
            else
            {
                ScrapedGameObject o = ScrapeDB.GetScrapedGameObject(gameId, game.gdbId.Value);

                // link is there - SET DATA and control states
                bool isGameInfoData = false;                            //GameInfo Expander
                bool isScreenshotsData = false;                         //Screenshots Expander
                bool isFanrtsData = false;                              //Fanrt expander
                bool isManualsData = false;                             //manuals expander

                // Game Title
                if (o.Data.Title == null || o.Data.Title == "")
                {
                    expGameInformation.Header = "Scraped Information";
                }
                else
                {
                    // Expander title
                    expGameInformation.Header = o.Data.Title;
                }

                // Back Cover Image
                if (o.BackCovers == null || o.BackCovers.Count == 0)
                {
                    imgBoxartBack.Source = null;
                    imgBoxartBack.SetVisibility();
                }        
                else
                {
                    isGameInfoData = true;
                    // boxart - back
                    SetImage(imgBoxartBack, o.BackCovers.FirstOrDefault(), UriKind.Absolute);
                    imgBoxartBack.SetVisibility();
                }

                // Banner Image
                if (o.Banners == null || o.Banners.Count == 0)
                {
                    imgBanner.Source = null;
                    imgBanner.SetVisibility();
                }                    
                else
                {
                    isGameInfoData = true;
                    // banner (just take one)
                    List<string> banners = new List<string>();
                    SetImage(imgBanner, o.Banners.FirstOrDefault(), UriKind.Absolute);
                    imgBanner.SetVisibility();
                }

                // Front Cover Image
                if (o.FrontCovers == null || o.FrontCovers.Count == 0)
                {
                    imgBoxartFront.Source = null;
                    imgBoxartFront.SetVisibility();
                }                    
                else
                {
                    isGameInfoData = true;
                    // boxart - front
                    SetImage(imgBoxartFront, o.FrontCovers.FirstOrDefault(), UriKind.Absolute);
                    imgBoxartFront.SetVisibility();
                }

                // Media Image
                if (o.Medias == null || o.Medias.Count == 0)
                {
                    imgMedia.Source = null;
                    imgMedia.SetVisibility();
                } 
                else
                {
                    isGameInfoData = true;
                    // media image
                    SetImage(imgMedia, o.Medias.FirstOrDefault(), UriKind.Absolute);
                    imgMedia.SetVisibility();
                }

                // Alternate Game Titles
                if (o.Data.AlternateTitles == null || o.Data.AlternateTitles.Count == 0)
                {
                    lblAltNames.Content = null;
                    lblAltNamesTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblAltNames.Content = string.Join(", ", (o.Data.AlternateTitles).ToArray());
                    lblAltNames.Visibility = Visibility.Visible;
                    lblAltNamesTitle.Visibility = Visibility.Visible;
                }

                // Release Date
                if (o.Data.Released == null || o.Data.Released == "")
                {
                    lblReleaseDate.Content = null;
                    lblReleaseDateTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblReleaseDate.Content = o.Data.Released;
                    lblReleaseDate.Visibility = Visibility.Visible;
                    lblReleaseDateTitle.Visibility = Visibility.Visible;
                }

                // Coop               
                if (o.Data.Coop == null || o.Data.Coop == "")
                {
                    lblCoop.Content = null;
                    lblCoopTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblCoop.Content = o.Data.Coop;
                    lblCoop.Visibility = Visibility.Visible;
                    lblCoopTitle.Visibility = Visibility.Visible;
                }

                // Developer
                if (o.Data.Developer == null || o.Data.Developer == "")
                {
                    lblDeveloper.Content = null;
                    lblDeveloperTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblDeveloper.Content = o.Data.Developer;
                    lblDeveloper.Visibility = Visibility.Visible;
                    lblDeveloperTitle.Visibility = Visibility.Visible;
                }

                // Publisher
                if (o.Data.Publisher == null || o.Data.Publisher == "")
                {
                    lblPublisher.Content = null;
                    lblPublisherTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblPublisher.Content = o.Data.Publisher;
                    lblPublisher.Visibility = Visibility.Visible;
                    lblPublisherTitle.Visibility = Visibility.Visible;
                }

                // Genres
                if (o.Data.Genres == null || o.Data.Genres.Count == 0)
                {
                    lblGenres.Content = null;
                    lblGenresTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblGenres.Content = string.Join(", ", (o.Data.Genres).ToArray());
                    lblGenres.Visibility = Visibility.Visible;
                    lblGenresTitle.Visibility = Visibility.Visible;
                }

                // ESRB Rating
                if (o.Data.ESRB == null || o.Data.ESRB == "")
                {
                    lblESRB.Content = null;
                    lblESRBTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblESRB.Content = o.Data.ESRB;
                    lblESRB.Visibility = Visibility.Visible;
                    lblESRBTitle.Visibility = Visibility.Visible;
                }

                // Number of players
                if (o.Data.Players == null || o.Data.Players == "")
                {
                    lblPlayers.Content = null;
                    lblPlayersTitle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    lblPlayers.Content = o.Data.Players;
                    lblPlayers.Visibility = Visibility.Visible;
                    lblPlayersTitle.Visibility = Visibility.Visible;
                }

                // game overview (description)
                if (o.Data.Overview == null || o.Data.Overview == "")
                {
                    tbOverview.Text = null;
                    brdSidebarOverview.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isGameInfoData = true;
                    tbOverview.Text = o.Data.Overview;
                    brdSidebarOverview.Visibility = Visibility.Visible;
                }

                // Fanart Images
                if (o.FanArts == null || o.FanArts.Count == 0)
                {
                    brdSidebarFanArt.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isFanrtsData = true;                    
                    int fanCount = o.FanArts.Count;
                    ugridFanarts.Columns = DetermineUniformGridColumns(fanCount);
                    brdSidebarFanArt.Visibility = Visibility.Visible;
                    List<string> fshots = o.FanArts;
                    String[] arr = fshots.ToArray();
                    int i = 0;
                    while (i < fa.Count)
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

                // Screenshot Images
                if (o.Screenshots == null || o.Screenshots.Count == 0)
                {
                    brdSidebarScreenshots.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isScreenshotsData = true;
                    int screenCount = o.Screenshots.Count;
                    ugridScreenshots.Columns = DetermineUniformGridColumns(screenCount);                    
                    brdSidebarScreenshots.Visibility = Visibility.Visible;
                    List<string> sshots = o.Screenshots;
                    String[] arr = sshots.ToArray();
                    int i = 0;
                    while (i < ss.Count)
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

                // Manuals (external documents)
                if (o.Manuals == null || o.Manuals.Count == 0)
                {
                    brdManuals.Visibility = Visibility.Collapsed;
                }
                else
                {
                    isManualsData = true;
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

                // set border visibilities
                if (isGameInfoData == false)
                    brdSidebarGameInformation.Visibility = Visibility.Collapsed;
                else
                    brdSidebarGameInformation.Visibility = Visibility.Visible;
                if (isFanrtsData == false)
                    brdSidebarFanArt.Visibility = Visibility.Collapsed;
                else
                    brdSidebarFanArt.Visibility = Visibility.Visible;
                if (isScreenshotsData == false)
                    brdSidebarScreenshots.Visibility = Visibility.Collapsed;
                else
                    brdSidebarScreenshots.Visibility = Visibility.Visible;
                if (isManualsData == false)
                    brdManuals.Visibility = Visibility.Collapsed;
                else
                    brdManuals.Visibility = Visibility.Visible;

            }   
        }

        public static int DetermineUniformGridColumns(int count)
        {
            int counter = 4;
            if (count <= 4) { return count; }
            if (count > 16)
            {
                counter = 5;
            }
            if (count > 20)
            {
                counter = 6; 
            }
            return counter;
        }

        
        public static  void SetImage(Image img, string path, UriKind urikind)
        {            
            if (!File.Exists(path))
                return;
            try
            {
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

                double ttPercentage = GlobalSettings.GetGlobals().imageToolTipPercentage;

                ToolTip tt = (ToolTip)img.ToolTip;
                tt.MaxHeight = windowHeight * ttPercentage;
                tt.MaxWidth = windowWidth * ttPercentage;
                img.ToolTip = tt;
            }

            catch (System.NotSupportedException ex)
            {
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
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
            /*
            // get an instance of the MainWindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            App _App = ((App)Application.Current);
            //var data = _App.GamesList.FilteredSet;
            //_App.GamesList.FilteredSet = new System.Collections.ObjectModel.ObservableCollection<DataGridGamesView>(data);
            DataGrid dg = (DataGrid)mw.FindName("dgGameList");
            
            //var items = dg.ItemsSource;
            
            // get btnClearFilters button
            Button btnClearFilters = (Button)mw.FindName("btnClearFilters");

            // get the wrappanel that contains all the GameLibrary toggleboxes
            Grid grdGameLibrary = (Grid)mw.FindName("grdGameLibrary");

            // get the dynamic filter textbox tbFilterDatagrid
            TextBox tbFilterDatagrid = (TextBox)mw.FindName("tbFilterDatagrid");
            string tbText = tbFilterDatagrid.Text;

            // get all grouped radio buttons
            List<RadioButton> buttons = UIHandler.GetLogicalChildCollection<RadioButton>(grdGameLibrary).Where(a => !a.Name.StartsWith("srcFilter")).ToList();

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

            // get selected item in datagrid
            GamesLibraryModel row = (GamesLibraryModel)dg.SelectedItem;
            
            // Clear all settings
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Text = "1337";

            // restore settings
            rtTarget.IsChecked = true;
            tbFilterDatagrid.Text = tbText;

            */

            /*
            // set column visibuilities
            GlobalSettings gs = GlobalSettings.GetGlobals();

            DataGridTextColumn colYear = (DataGridTextColumn)mw.FindName("colYear");
            DataGridTextColumn colPlayers = (DataGridTextColumn)mw.FindName("colPlayers");
            DataGridTextColumn colCoop = (DataGridTextColumn)mw.FindName("colCoop");
            DataGridTextColumn colPublisher = (DataGridTextColumn)mw.FindName("colPublisher");
            DataGridTextColumn colDeveloper = (DataGridTextColumn)mw.FindName("colDeveloper");
            DataGridTextColumn colRating = (DataGridTextColumn)mw.FindName("colRating");

            if (gs.showGLCoop == false)
                colCoop.Visibility = Visibility.Collapsed;
            else
                colCoop.Visibility = Visibility.Visible;

            if (gs.showGLPlayers == false)
                colPlayers.Visibility = Visibility.Collapsed;
            else
                colPlayers.Visibility = Visibility.Visible;

            if (gs.showGLYear == false)
                colYear.Visibility = Visibility.Collapsed;
            else
                colYear.Visibility = Visibility.Visible;

            if (gs.showGLPublisher== false)
                colPublisher.Visibility = Visibility.Collapsed;
            else
                colPublisher.Visibility = Visibility.Visible;

            if (gs.showGLDeveloper == false)
                colDeveloper.Visibility = Visibility.Collapsed;
            else
                colDeveloper.Visibility = Visibility.Visible;

            if (gs.showGLESRB == false)
                colRating.Visibility = Visibility.Collapsed;
            else
                colRating.Visibility = Visibility.Visible;
                */
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
            var r = (GamesLibraryModel)dgGameList.SelectedItem;
            int gameId = r.ID;            
        }

        public static int GetSelectedFilterNumber()
        {
            App _App = ((App)Application.Current);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");

            // get active filter
            RadioButton[] rbs = ReturnFilterButtons();
            for (int i = 0; i < rbs.Length; i++)
            {
                if (rbs[i].IsChecked == true)
                {
                    return (i + 1);
                }
            }

            return 0;
        }

        public static void SaveColumnInfo(int FilterNumber)
        {
            App _App = ((App)Application.Current);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");

            // get the list of columninfo from the datagrid
            ColumnInfoObject colInfo = ColumnInfo.GetColumnInfo(dgGameList);
            colInfo.FilterNumber = FilterNumber;

            // add to the global object
            var temp = _App.GamesLibrary.DataGridStates.Where(a => a.FilterNumber == colInfo.FilterNumber).FirstOrDefault();
            if (temp != null)
            {
                temp.FilterNumber = colInfo.FilterNumber;
                temp.ColumnInfoList = colInfo.ColumnInfoList;
                temp.SortDescriptionList = colInfo.SortDescriptionList;
            }
               
            // _App.GamesLibrary.DataGridStates[FilterNumber] = colInfo;
        }

        public static void LoadColumnInfo(int FilterNumber)
        {
            App _App = ((App)Application.Current);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");

            // clear existing sortdescriptions
            
            var cView = CollectionViewSource.GetDefaultView(dgGameList.ItemsSource);
            if (cView != null)
                cView.SortDescriptions.Clear();
             

            ColumnInfoObject colInfo = _App.GamesLibrary.DataGridStates.Where(a => a.FilterNumber == FilterNumber).FirstOrDefault();
            if (colInfo != null)
                if (colInfo.ColumnInfoList.Count == 0)
                {
                    return;
                }
                

            ColumnInfo.ApplyColumnInfo(dgGameList, colInfo);
        }

        public static void ReloadSelectedColumnState()
        {
            App _App = ((App)Application.Current);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");

            // get active filter
            RadioButton[] rbs = ReturnFilterButtons();
            for (int i = 0; i < rbs.Length; i++)
            {
                if (rbs[i].IsChecked == true)
                {
                    LoadColumnInfo(i + 1);
                    break;
                }
            }
        }
                
        public static void SaveSelectedColumnState()
        {
            App _App = ((App)Application.Current);
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");

            // get active filter
            RadioButton[] rbs = ReturnFilterButtons();
            for (int i = 0; i < rbs.Length; i++)
            {
                if (rbs[i].IsChecked == true)
                {
                    SaveColumnInfo(i + 1);
                    break;
                }
            }
        }

        public static void SetColumnVisibility(int FilterNumber)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // return filter array
            string[] fArr = GlobalSettings.ReturnFilterArray();
            // get the hex string for the filter we are interested in
            string hex = fArr[FilterNumber - 1];

            // Translate hex and set column visibility
            int value = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            for (int i = 14; i > 0; i--)
            {
                int testValue = 1 * Convert.ToInt32(Math.Pow(2, Convert.ToDouble(i - 1)));
                if (i != 5)
                {
                    int val = value / testValue;
                    DataGridTextColumn tc = (DataGridTextColumn)mw.FindName("glCol" + i.ToString());
                    if (value / testValue == 1)
                    {
                        // column should be visible
                        tc.Visibility = Visibility.Visible;
                        // set remainder
                        value = value % testValue;
                    }
                    else
                    {
                        // column should not be visible
                        tc.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    DataGridCheckBoxColumn cc = (DataGridCheckBoxColumn)mw.FindName("glCol" + i.ToString());
                    if (value / testValue == 1)
                    {
                        // column should be visible
                        cc.Visibility = Visibility.Visible;
                        // set remainder
                        value = value % testValue;
                    }
                    else
                    {
                        // column should not be visible
                        cc.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public static RadioButton[] ReturnFilterButtons()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            RadioButton[] buttons = new RadioButton[]
            {
                (RadioButton)mw.FindName("btnShowAll"),         // 1
                (RadioButton)mw.FindName("btnFavorites"),       // 2
                (RadioButton)mw.FindName("btnUnscraped"),       // 3
                (RadioButton)mw.FindName("btnNes"),             // 4
                (RadioButton)mw.FindName("btnSnes"),            // 5
                (RadioButton)mw.FindName("btnSms"),             // 6
                (RadioButton)mw.FindName("btnMd"),              // 7
                (RadioButton)mw.FindName("btnPce"),             // 8
                (RadioButton)mw.FindName("btnVb"),              // 9
                (RadioButton)mw.FindName("btnNgp"),             // 10
                (RadioButton)mw.FindName("btnWswan"),           // 11
                (RadioButton)mw.FindName("btnGb"),              // 12
                (RadioButton)mw.FindName("btnGba"),             // 13
                (RadioButton)mw.FindName("btnGg"),              // 14
                (RadioButton)mw.FindName("btnLynx"),            // 15
                (RadioButton)mw.FindName("btnSs"),              // 16
                (RadioButton)mw.FindName("btnPsx"),             // 17
                (RadioButton)mw.FindName("btnPcecd"),           // 18
                (RadioButton)mw.FindName("btnPcfx"),            // 19
            };

            return buttons;
        }

        public static CountryFilter GetSelectedCountryFilter()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mw == null)
            {
                // maybe mainwindow has not loaded yet
                return CountryFilter.ALL;
            }

            Grid CountryFilterGrid = (Grid)mw.FindName("CountryFilterGrid");

            // get all country filter radiobuttons
            UIHandler ui = UIHandler.GetChildren(CountryFilterGrid);
            RadioButton[] rbs = ui.RadioButtons.Where(a => a.GroupName == "filters").ToArray();

            // get the checked radio button
            RadioButton rb = rbs.Where(a => a.IsChecked == true).FirstOrDefault();

            if (rb == null)
            {
                return CountryFilter.ALL;
            }                

            for (int i = 0; i < rbs.Length; i++)
            {
                string name = rbs[i].Name.Replace("srcFilterAll", "");

                switch (name)
                {
                    case "USA":
                        return CountryFilter.USA;
                    case "EUR":
                        return CountryFilter.EUR;
                    case "JPN":
                        return CountryFilter.JPN;
                    default:
                        return CountryFilter.ALL;
                }
            }
            return CountryFilter.ALL;
        }

        public static void DoFullUpdate()
        {
            App _App = (App)Application.Current;            

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                var currentItem = _App.GamesLibrary.LibraryView.View.CurrentItem;
                int currentPos = _App.GamesLibrary.LibraryView.View.CurrentPosition;
                _App.GamesLibrary.Update();
                // reselect current item
                _App.GamesLibrary.LibraryView.View.MoveCurrentTo(currentItem);
            }));
        }

        public static void DoGameDelete(List<Game> games)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                App _App = (App)Application.Current;
                _App.GamesLibrary.RemoveEntries(games);
            }));
        }

        public static void DoGameAdd(List<Game> games)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                App _App = (App)Application.Current;
                _App.GamesLibrary.AddEntries(games);
            }));
        }

        public static void DoGameUpdate(List<Game> games)
        {
            //App _App = (App)Application.Current;
            //_App.GamesLibrary.UpdateEntries(games);
            
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                App _App = (App)Application.Current;
                _App.GamesLibrary.UpdateEntries(games);
            }));
           
        }
    }
}
