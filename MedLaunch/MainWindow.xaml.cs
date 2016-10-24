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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.IO;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using MedLaunch.Models;
using MedLaunch.Classes;
using Asnitech.Launch.Common;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
using System.Threading;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            // make sure class libraries are built
            //Asnitech.Launch.Common.Startup.Start();

            // initialise directories if they do not exist
            //SetupDirectories.Go();

            // check the database version if it exists - run the update/migration procedure if necessary
            //DbMigration.CheckVersions();

            // initialise SQLite db if it does not already exist
            /*
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                // populate stock data 
                DbEF.InitialSeed();
                context.SaveChanges();
            }
            */

            InitializeComponent();


            MainWindow mw = this;
            //this.WindowState = WindowState.Normal;

            // doubleclick handler for gui_zoom control
            //gui_zoom.MouseDoubleClick += new MouseButtonEventHandler(RestoreScalingFactor);
            dpZoomSlider.Visibility = Visibility.Collapsed;
            //gui_zoom.Value = Convert.ToDouble(gui_zoom_combo.SelectedValue);


            // set window size
            this.Height = 768;
            this.Width = 1366;

            // check workspace size, if mahapps resolution is too big - go full screen
            int wWidth = Convert.ToInt32(SystemParameters.WorkArea.Width);
            int wHeight = Convert.ToInt32(SystemParameters.WorkArea.Height);

            if (this.Height > wHeight || this.Width > wWidth)
            {
                // maximise window
                this.WindowState = WindowState.Maximized;
            }


            // get application version
            string appVersion = Versions.ReturnApplicationVersion();
            /*
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionMajor = fvi.ProductMajorPart.ToString();
            string versionMinor = fvi.ProductMinorPart.ToString();
            string versionBuild = fvi.ProductBuildPart.ToString();
            string versionPrivate = fvi.ProductPrivatePart.ToString();
            
            string fVersion = fvi.FileVersion;
            */

            // set title
            string linkTimeLocal = (Assembly.GetExecutingAssembly().GetLinkerTime()).ToString("yyyy-MM-dd HH:mm:ss");
            this.Title = "MedLaunch - Windows Front-End for Mednafen (v" + appVersion + ")";
            //this.Title = "MedLaunch - Windows Front-End for Mednafen (v" + versionMajor + "." + versionMinor + "." + versionBuild + "." + versionPrivate + ")"; // - Built: "+ linkTimeLocal;
            //this.Title = "MedLaunch - Windows Front-End for Mednafen (" + fVersion + ")";

            // Startup checks

            // is DB path to Mednafen set and working? If not force user to select it
            Paths.MedPathRoutine(btnPathMednafen, tbPathMednafen);
            
            // ensure 'show all' filter is checked on startup
            btnShowAll.IsChecked = true;

            // load globalsettings for front page
            GlobalSettings.LoadGlobalSettings(chkEnableNetplay, chkEnableSnes_faust, chkEnablePce_fast, gui_zoom_combo, chkMinToTaskbar, chkHideSidebar,
               chkAllowBanners, chkAllowBoxart, chkAllowScreenshots, chkAllowFanart, chkPreferGenesis);
            //gui_zoom.Value = Convert.ToDouble(gui_zoom_combo.SelectedValue);
            GlobalSettings gs = GlobalSettings.GetGlobals();
            mainScaleTransform.ScaleX = Convert.ToDouble(gs.guiZoom);
            mainScaleTransform.ScaleY = Convert.ToDouble(gs.guiZoom);




            // load netplay settings for netplay page
            ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);

            // load path settings for paths page
            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);

            // force location of Mednafen directory
            //var controller = this.ShowProgressAsync("Please wait...", "Progress Message");

            // initialise servers area

            // DbEF.PopulateServersCombo(cbServers);
            ConfigServerSettings.PopulateServersRadio(rbSrv01);
            ConfigServerSettings.PopulateServersRadio(rbSrv02);
            ConfigServerSettings.PopulateServersRadio(rbSrv03);
            ConfigServerSettings.PopulateServersRadio(rbSrv04);
            ConfigServerSettings.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
            ConfigServerSettings.GetSelectedServerCheckbox(rbSrv01, rbSrv02, rbSrv03, rbSrv04, rbSrvCustom);


            // Config Tab

            // Set radiobutton states
            foreach (UIElement element in ConfigSelectorWrapPanel.Children)
            {
                if (element is RadioButton)
                {
                    // Set the radiobutton state (enabled or disabled)
                    //MessageBoxResult result = MessageBox.Show((element as RadioButton).Name.ToLower());
                    ConfigBaseSettings.SetButtonState(element as RadioButton);
                }
            }

            // ensure base config is selected at startup
            btnConfigBase.IsChecked = true;

            // hide all system specific config options
            /*
            brdSpecificLynx.Visibility = Visibility.Collapsed;
            brdSpecificGb.Visibility = Visibility.Collapsed;
            brdSpecificNgp.Visibility = Visibility.Collapsed;
            brdSpecificNes1.Visibility = Visibility.Collapsed;
            brdSpecificNes2.Visibility = Visibility.Collapsed;
            brdSpecificNes3.Visibility = Visibility.Collapsed;
            brdSpecificPce_fast1.Visibility = Visibility.Collapsed;
            brdSpecificPce_fast2.Visibility = Visibility.Collapsed;
            brdSpecificPce1.Visibility = Visibility.Collapsed;
            brdSpecificPce2.Visibility = Visibility.Collapsed;
            brdSpecificPcfx1.Visibility = Visibility.Collapsed;
            brdSpecificPcfx2.Visibility = Visibility.Collapsed;
            brdSpecificSms.Visibility = Visibility.Collapsed;
            brdSpecificMd.Visibility = Visibility.Collapsed;
            brdSpecificSnes_faust.Visibility = Visibility.Collapsed;
            brdSpecificSnes.Visibility = Visibility.Collapsed;
            brdSpecificVb.Visibility = Visibility.Collapsed;
            brdSpecificSs1.Visibility = Visibility.Collapsed;
            brdSpecificSs2.Visibility = Visibility.Collapsed;
            brdSpecificPsx1.Visibility = Visibility.Collapsed;
            brdSpecificPsx2.Visibility = Visibility.Collapsed;
            brdSpecificPsxController1.Visibility = Visibility.Collapsed;
            brdSpecificPsxController2.Visibility = Visibility.Collapsed;
            brdSpecificPsxController3.Visibility = Visibility.Collapsed;
            brdSpecificPsxController4.Visibility = Visibility.Collapsed;
            brdSpecificPsxController5.Visibility = Visibility.Collapsed;
            brdSpecificPsxController6.Visibility = Visibility.Collapsed;
            brdSpecificPsxController7.Visibility = Visibility.Collapsed;
            brdSpecificPsxController8.Visibility = Visibility.Collapsed;
            brdSpecificWswan.Visibility = Visibility.Collapsed;
            */


            // settings tab
            btnAllSettings.IsChecked = true;
            SettingsVisualHandler svh = new SettingsVisualHandler();
            // get the button state and active/deativate required panels
            svh.SetFilter();
            // load all settings
            SettingsHandler sh = new SettingsHandler();
            sh.LoadAllSettings();

            // load all MednafenPath settings
            ConfigBaseSettings.LoadMednafenPathValues(spMedPathSettings);
            ConfigBaseSettings.LoadBiosPathValues(spSysBiosSettings);




            // load mednafen help page
            wb.Navigate("http://mednafen.fobby.net/");


            /* hide certain controls (whilst they are being developed) */

            // rescan all disk systems button
            btnRescanDisks.Visibility = Visibility.Collapsed;

            // Rescan specific disk system menu item
            ScanDisks13.Visibility = Visibility.Collapsed;
            ScanDisks9.Visibility = Visibility.Collapsed;
            ScanDisks18.Visibility = Visibility.Collapsed;
            ScanDisks8.Visibility = Visibility.Collapsed;

            // settings buttons and borders
            //btnMednafenPaths.Visibility = Visibility.Collapsed;
            //btnSystemBios.Visibility = Visibility.Collapsed;
            //btnEmulator.Visibility = Visibility.Collapsed;
            //brdMednafenPaths.Visibility = Visibility.Collapsed;
           /// brdSystemBios.Visibility = Visibility.Collapsed;
            //brdEmulator.Visibility = Visibility.Collapsed;

            wb.Navigated += new NavigatedEventHandler(wb_Navigated);

            // games library
            GamesLibraryVisualHandler.UpdateSidebar();

            // load expander states
            GamesLibraryVisualHandler.LoadExpanderStates();
            // enable events once expander states have been set
            /*
            List < Expander > exp = GamesLibraryVisualHandler.GetExpanderControls();
            foreach (Expander e in exp)
            {
                e.Collapsed += new RoutedEventHandler(GamesLibraryExpanderSaveLayout);
                e.Expanded += new RoutedEventHandler(GamesLibraryExpanderSaveLayout);
            }
            */

            // GameScraper.GetPlatformGames(4924);
            
        }

        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {

            ((Slider)sender).Value = 1.0;
        }
        private void gui_zoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double value = Convert.ToDouble((sender as ComboBox).SelectedValue);
            //MessageBox.Show(value.ToString());
            //gui_zoom.Value = value;
            if (Convert.ToDouble(value) > 0)
            {
                GlobalSettings.UpdateGuiZoom(value);
            }

            // now update zoom
            ScaleTransform st = (ScaleTransform)this.FindName("mainScaleTransform");
            st.ScaleX = value;
            st.ScaleY = value;
            
        }


        // web browser

        void wb_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(wb, true); // make it silent
        }

        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }


        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                wb.Navigate(txtUrl.Text);
        }

        private void wb_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            txtUrl.Text = e.Uri.OriginalString;
        }
        

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wb != null) && (wb.CanGoBack));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wb.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wb != null) && (wb.CanGoForward));
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wb.GoForward();
        }

        private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
      
        private void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wb.Navigate(txtUrl.Text);
        }

        private void btnWbMednafenDoc_Click(object sender, RoutedEventArgs e)
        {
            wb.Navigate(new Uri("http://mednafen.fobby.net/documentation/", UriKind.RelativeOrAbsolute));
        }

        private void btnWbMednafenForum_Click(object sender, RoutedEventArgs e)
        {
            wb.Navigate(new Uri("http://forum.fobby.net/index.php?t=i&", UriKind.RelativeOrAbsolute));
        }

        private void btnWbMedLaunch_Click(object sender, RoutedEventArgs e)
        {
            wb.Navigate(new Uri("http://medlaunch.asnitech.co.uk/", UriKind.RelativeOrAbsolute));
        }

        private void btnWbMednafenHome_Click(object sender, RoutedEventArgs e)
        {
            wb.Navigate(new Uri("http://mednafen.fobby.net/", UriKind.RelativeOrAbsolute));
        }

        private async void RescanSystemDisks(int sysId)
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,                
                AnimateHide = true
                
            };

            var controller = await this.ShowProgressAsync("Scanning Disk Directories", "Determining Paths and Counting Files...", settings: mySettings);
            controller.SetCancelable(false);
            controller.SetIndeterminate();

            await Task.Delay(100);

            btnFavorites.IsChecked = true;
            btnShowAll.IsChecked = true;

            await controller.CloseAsync();
        }

        private void RescanDisks(object sender, RoutedEventArgs e)
        {
            RescanSystemDisks(0);
        }

        private async void ScrapeGetAllPlatformGames_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource cs = new CancellationTokenSource();
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await this.ShowProgressAsync("MedLaunch - Getting Basic Games List From thegamesdb.net", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);
                await Task.Run(() =>
                {
                    Task.Delay(1);
                    List<GDBPlatformGame> gs = GameScraper.DatabasePlatformGamesImport(controller);
                    if (!controller.IsCanceled)
                     GDBPlatformGame.SaveToDatabase(gs);
                });
            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("MedLaunch - GameScraper", "Operation Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("MedLaunch - GameScraper", "Scanning Completed");
            }
        }


        private async void RescanSystemRoms(int sysId)
        {

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,
                AnimateHide = true
            };

            var controller = await this.ShowProgressAsync("Scanning ROM Directories", "Determining Paths and Counting Files...", settings: mySettings);
            controller.SetCancelable(true);
            //controller.SetIndeterminate();

            await Task.Delay(100);

            string output = "";
            int addedStats = 0;
            int updatedStats = 0;
            int untouchedStats = 0;
            int hiddenStats = 0;

            GameScanner rs = new GameScanner();

                       
            await Task.Delay(100);
           


            List<GSystem> scanRoms = new List<GSystem>();
            if (sysId == 0)
            {
                /* scan of all roms has been selected */

                // mark all roms in database as hidden where the system path is not set or if path no longer exists
                foreach (var hs in rs.Systems)
                {
                    string path = rs.GetPath(hs.systemId);
                    if (path == "" || path == null || !Directory.Exists(path))
                    {
                        
                        // No path returned or path is not valid - Mark existing games in Db as hidden
                        rs.MarkAllRomsAsHidden(hs.systemId);
                        hiddenStats += rs.HiddenStats;
                        rs.HiddenStats = 0;
                    }
                }

                scanRoms = rs.RomSystemsWithPaths;
            }
            else
            {
                /* only one system has been selected for scanning */

                // mark all roms in database as hidden for this system path is not set or if path no longer exists                
                string path = rs.GetPath(sysId);
                if (path == "" || path == null || !Directory.Exists(path))
                {
                    // No path returned or path is not valid - Mark existing games in Db as hidden
                    rs.MarkAllRomsAsHidden(sysId);
                    hiddenStats += rs.HiddenStats;
                    rs.HiddenStats = 0;
                }
             

                scanRoms = (from s in rs.RomSystemsWithPaths
                               where s.systemId == sysId
                               select s).ToList();
            }

            // check whether scanroms is null
            if (scanRoms.Count > 0)
            {
                // start the operations on a different thread
                await Task.Run(async () =>
                {
                    // data has been returned

                    // how many systems returned
                    int sysCount = scanRoms.Count;
                    controller.Minimum = 0;
                    controller.Maximum = sysCount;
                    int progress = 0;

                    // iterate through each system that has a system ROM path set
                    foreach (var s in scanRoms)
                    {
                        if (controller.IsCanceled)
                        {
                            break;
                        }
                        //MessageBoxResult result2 = MessageBox.Show(s.systemId.ToString());

                        // start scanning
                        controller.SetTitle("Starting " + s.systemName + " (" + s.systemCode + ") Scan");
                        await Task.Delay(100);
                        //output += "Scanning....";
                        controller.SetMessage(output);

                        progress++;
                        controller.SetProgress(progress);

                        // Start ROM scan for this system
                        rs.BeginRomImport(s.systemId, controller);

                        //output += ".....Completed\n\n";

                        // update totals
                        addedStats += rs.AddedStats;
                        updatedStats += rs.UpdatedStats;
                        untouchedStats += rs.UntouchedStats;
                        hiddenStats += rs.HiddenStats;

                        //output += rs.AddedStats + " ROMs Added\n" + rs.UpdatedStats + " ROMs Updated\n" + rs.UntouchedStats + " ROMs Skipped\n" + rs.HiddenStats + " ROMs Missing (marked as hidden)\n";
                        //controller.SetMessage(output);

                        // reset class totals
                        rs.AddedStats = 0;
                        rs.UpdatedStats = 0;
                        rs.UntouchedStats = 0;
                        rs.HiddenStats = 0;

                        await Task.Delay(200);
                    }
                });
                
            }
            else
            {
                // No systems returned
                controller.SetTitle("No ROM systems with valid paths found");
                controller.SetMessage("No GameSystem with valid path was found\n Please make sure there is a valid path set for this system");
                
            }

            await Task.Run(() =>
            {
                if (!controller.IsCanceled)
                {
                    controller.SetMessage(output + "\nUpdating Database");
                    rs.SaveToDatabase();
                }
                
            });
                
        

            await Task.Delay(100);



            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("RomScanner", "Operation Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("Scanning Completed", "Totals:\n\nROMs Added: " + addedStats + "\nROMs Updated: " + updatedStats + "\nROMs Skipped: " + untouchedStats + "\nROMs Marked as Hidden: " + hiddenStats);
            }

            //Update list
            // ensure 'show all' filter is checked on startup
            //btnFavorites.IsChecked = true;
            //btnShowAll.IsChecked = true;

            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();


            /*

            // get systems
            List<GameSystem> systems = RomScanner.GetSystems();
            List<Rom> romSystem = new List<Rom>();
            

            // iterate through each system and check if ROM path exists
            foreach (var s in systems)
            {
                string path = RomScanner.GetPath(s.systemId);
                if (path == "" || path == null)
                {
                    //do nothing
                }
                else
                {
                    //MessageBoxResult result = MessageBox.Show("System: " + s.systemName + " - Path: " + path, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Rom r = new Rom();
                    r.gameSystem = s;
                    r.path = path;
                    romSystem.Add(r);
                }
            }

            // we now have a List<Rom> that contains all systems that have a filesystem directory path set
            // iterate through each path and count the number of files

            int totalFiles = 0;
            int romsInserted = 0;
            int romsUpdated = 0;
            int romsSkipped = 0;
            foreach (var p in romSystem)
            {
                int systemFiles = RomScanner.CountFiles(p.path);
                totalFiles += systemFiles;
            }

            controller.SetMessage(totalFiles + " files found across all ROM directories");

            await Task.Delay(500);

            foreach (var item in romSystem)
            {
                // actually scan through ROMS
                string SystemName = item.gameSystem.systemName;
                controller.SetTitle("Importing " + SystemName + " Roms");

                // get a collection of files from each ROM directory
                var files = RomScanner.GetFiles(item.path);

                // iterate through each file
                foreach (var file in files)
                {
                    // get the relative path
                    string relPath = PathUtil.GetRelativePath(item.path, file);

                    // get just the filename
                    string fileName = System.IO.Path.GetFileName(file);

                    // get just the extension
                    string extension = System.IO.Path.GetExtension(file);

                    // get rom name wihout extension
                    string romName = fileName.Replace(extension, "");

                    //MessageBoxResult result = MessageBox.Show(romName, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // attempt to add Rom to database
                    int addRom = RomScanner.AddGame(item, file, relPath, fileName, extension, romName);

                    string romProcess = "";

                    switch (addRom)
                    {
                        case 1:
                            // Rom inserted
                            romsInserted++;
                            romProcess = "Added to Database";
                            break;
                        case 2:
                            // Rom updated
                            romsUpdated++;
                            romProcess = "Updated in Database";
                            break;
                        default:
                            // Rom skipped
                            romsSkipped++;
                            romProcess = "Has Been Skipped";
                            break;               
                    }

                    controller.SetMessage("ROM: " + romName + " " + romProcess);

                }
                await Task.Delay(100);
            }

            /*
            double i = 0.0;
            while (i < Convert.ToDouble(totalFiles + 1))
            {
                //double val = (i / 100.0) * totalFiles;
                double val = (100 / totalFiles) * i / 100;
                controller.SetProgress(val);
                
                controller.SetMessage("Scanning " +  + i + "...");

                if (controller.IsCanceled)
                    break; //canceled progressdialog auto closes.

                i += 1.0;

                await Task.Delay(2000);
            }
            

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("The operation was cancelled!", romsInserted +  " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
            }
            else
            {
                await this.ShowMessageAsync("Operation completed", romsInserted + " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
            }
            */
            //Update list
            // ensure 'show all' filter is checked on startup
            //btnFavorites.IsChecked = true;
            //btnShowAll.IsChecked = true;

            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void RescanRoms(object sender, RoutedEventArgs e)
        {
            RescanSystemRoms(0);
        }

        



        /*********** 
         * EVENTS
         * *********/






        // Misc
        private void System_Image_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Game filter buttons
        private void btnShowAll_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 0, tbFilterDatagrid.Text);
        }

        private void btnFavorites_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, -1, tbFilterDatagrid.Text);
        }

        private void btnUnscraped_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, -100, tbFilterDatagrid.Text);
        }

        private void btnNes_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 11, tbFilterDatagrid.Text);
        }

        private void btnSnes_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 12, tbFilterDatagrid.Text);
        }

        private void btnSms_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 10, tbFilterDatagrid.Text);
        }

        private void btnMd_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 4, tbFilterDatagrid.Text);
        }

        private void btnSs_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 13, tbFilterDatagrid.Text);
        }

        private void btnPsx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 9, tbFilterDatagrid.Text);
        }

        private void btnPce_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 7, tbFilterDatagrid.Text);
        }
        private void btnPcecd_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 18, tbFilterDatagrid.Text);
        }

        private void btnPcfx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 8, tbFilterDatagrid.Text);
        }

        private void btnVb_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 14, tbFilterDatagrid.Text);
        }

        private void btnNgp_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 6, tbFilterDatagrid.Text);
        }

        private void btnWswan_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 15, tbFilterDatagrid.Text);
        }

        private void btnGb_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 1, tbFilterDatagrid.Text);
        }

        private void btnGba_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 2, tbFilterDatagrid.Text);
        }

        private void btnGg_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 5, tbFilterDatagrid.Text);
        }

        private void btnLynx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 3, tbFilterDatagrid.Text);
        }

        // game filter context menu events
        private void ScanRoms_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ScanRoms", ""));
            RescanSystemRoms(sysId);
        }

        private void RemoveRoms_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("RemoveRoms", ""));
            GameScanner.RemoveRoms(sysId);
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void ScanDisks_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ScanDisks", ""));
        }

        private void RemoveDisks_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("RemoveDisks", ""));
            GameScanner.RemoveDisks(sysId);
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void ManualAddGame_Click(object sender, RoutedEventArgs e)
        {
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ManualAddGame", ""));
            GameScanner gs = new GameScanner();
            gs.BeginManualImport(sysId);
            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void RemoveAllGames_Click(object sender, RoutedEventArgs e)
        {
            GameScanner.RemoveAllGames();
        } 

        private void ScrapeGames_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ScrapeGames", ""));
            GameScraper.ScrapeGames(sysId);
        }


        // Games grid filter text box event
        private void tbFilterDatagrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            int system = 0;
            // determine which radiobutton is checked
            if (btnShowAll.IsChecked == true)
            {
                system = 0;
            }
            if (btnFavorites.IsChecked == true)
            {
                system = -1;
            }
            if (btnLynx.IsChecked == true)
            {
                system = 3;
            }
            if (btnGg.IsChecked == true)
            {
                system = 5;
            }
            if (btnGba.IsChecked == true)
            {
                system = 2;
            }
            if (btnGb.IsChecked == true)
            {
                system = 1;
            }
            if (btnWswan.IsChecked == true)
            {
                system = 15;
            }
            if (btnNgp.IsChecked == true)
            {
                system = 6;
            }
            if (btnVb.IsChecked == true)
            {
                system = 14;
            }
            if (btnPcfx.IsChecked == true)
            {
                system = 8;
            }
            if (btnPce.IsChecked == true)
            {
                system = 7;
            }
           
            if (btnPsx.IsChecked == true)
            {
                system = 9;
            }
            if (btnSs.IsChecked == true)
            {
                system = 13;
            }
          
            if (btnMd.IsChecked == true)
            {
                system = 4;
            }
            if (btnSms.IsChecked == true)
            {
                system = 10;
            }
            if (btnSnes.IsChecked == true)
            {
                system = 12;
            }
            if (btnNes.IsChecked == true)
            {
                system = 11;
            }
            if (btnPcecd.IsChecked == true)
            {
                system = 18;
            }

            DbEF.GetGames(dgGameList, system, textbox.Text);
        }

        // Clear all filters button click
        public void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Clear();
        }

        // Games Datagrid selection changed
        private void dgGameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // update right hand info
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem != null)
            {
                //DbEF.GetInfo(row.ID, lblSystemName, taSystemDescription, imgSystem);
                //GamesLibraryVisualHandler.UpdateSidebar();
                GamesLibraryVisualHandler.UpdateSidebar(row.ID);
            }
            else
            {
                GamesLibraryVisualHandler.UpdateSidebar();
            }
        }

        // generic TextBox_TextChanged handler
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
         
        }

        // generic Slider_ValueChanged handler
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var binding = ((Slider)sender).GetBindingExpression(Slider.ValueProperty);
            binding.UpdateSource();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Data.CollectionViewSource globalSettingsViewModelViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("globalSettingsViewModelViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // globalSettingsViewModelViewSource.Source = [generic data source]
        }

        // Netplay Settings - netplay page
        private void btnNetplaySaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ConfigNetplaySettings.SaveNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            //lblNpSettingsSave.Content = "***Netplay Settings Saved***";
        }

        private void btnNetplayCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            //lblNpSettingsSave.Content = "***Netplay Settings Reverted***";
        }

        // Global Settings - Game page

        private void chkEnableNetplay_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableNetplay(chkEnableNetplay);
        }
        private void chkEnableNetplay_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableNetplay(chkEnableNetplay);
        }
        private void chkEnablePce_fast_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnablePce_fast(chkEnablePce_fast);
        }

        private void chkEnablePce_fast_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnablePce_fast(chkEnablePce_fast);
        }

        private void chkEnableSnes_faust_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableSnes_faust(chkEnableSnes_faust);
            // enable faust config if its not already
        }

        private void chkEnableSnes_faust_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableSnes_faust(chkEnableSnes_faust);
        }

        private void chkMinToTaskbar_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateMinToTaskBar(chkMinToTaskbar);
        }

        private void chkMinToTaskbar_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateMinToTaskBar(chkMinToTaskbar);
        }

        private void chkHideSidebar_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateHideSidebar(chkHideSidebar);
        }

        private void chkHideSidebar_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateHideSidebar(chkHideSidebar);
        }

        private void chkPreferGenesis_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatePreferGenesis(chkPreferGenesis);
        }

        private void chkPreferGenesis_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatePreferGenesis(chkPreferGenesis);
        }

        private void chkAllowBanners_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowBanners(chkAllowBanners);
        }

        private void chkAllowBanners_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowBanners(chkAllowBanners);
        }

        private void chkAllowBoxart_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowBoxart(chkAllowBoxart);
        }

        private void chkAllowBoxart_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowBoxart(chkAllowBoxart);
        }

        private void chkAllowScreenshots_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowScreenshots(chkAllowScreenshots);
        }

        private void chkAllowScreenshots_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowScreenshots(chkAllowScreenshots);
        }

        private void chkAllowFanart_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowFanart(chkAllowFanart);
        }

        private void chkAllowFanart_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowFanart(chkAllowFanart);
        }

        // Mednafen BIOS Paths events
        private void btnMednafenBiosPaths_Click(object sender, RoutedEventArgs e)
        {
            // convert the button name
            string btnName = ((sender as Button).Name).Replace("tbBios", "");
            // textbox name
            string tbName = "";
            switch (btnName)
            {
                case "Gba":
                    tbName = "cfg_gba__bios";
                    break;
                case "PceGe":
                    tbName = "cfg_pce__gecdbioss";
                    break;
                case "PceCd":
                    tbName = "cfg_pce__cdbios";
                    break;
                case "PceFastCd":
                    tbName = "cfg_pce_fast__cdbios";
                    break;
                case "Pcfx":
                    tbName = "cfg_pcfx__bios";
                    break;
                case "MdCd":
                    tbName = "cfg_md__cdbios";
                    break;
                case "NesGg":
                    tbName = "cfg_nes__ggrom";
                    break;
                case "SsJp":
                    tbName = "cfg_ss__bios_jp";
                    break;
                case "SsNaEu":
                    tbName = "cfg_ss__bios_na_eu";
                    break;
                case "PsxEu":
                    tbName = "cfg_psx__bios_eu";
                    break;
                case "PsxJp":
                    tbName = "cfg_psx__bios_jp";
                    break;
                case "PsxNa":
                    tbName = "cfg_psx__bios_na";
                    break;

            }
            //string tbName = "cfg_filesys__path_" + btnName;
            // get textbox
            TextBox tb = (TextBox)(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()).FindName(tbName);

            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Multiselect = false;
            filePath.Title = "Select " + btnName + " BIOS/Rom";
            filePath.ShowDialog();

            if (filePath.FileName.Length > 0)
            {
                tb.Text = filePath.FileName;
            }

        }

        // Mednafen Paths (cheats, saves etc..) events
        private void btnMednafenPaths_Click(object sender, RoutedEventArgs e)
        {
            // convert the button name
            string btnName = ((sender as Button).Name).ToLower().Replace("btnpath", "");
            // textbox name
            string tbName = "cfg_filesys__path_" + btnName;
            // get textbox
            TextBox tb = (TextBox)(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()).FindName(tbName);

            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select " + btnName + " Directory";
            path.ShowDialog();

            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tb.Text = strPath;
            }

            

        }

        // Path Page button clicks
        private void btnPathMednafen_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Mednafen Directory";
            path.ShowDialog();

            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMednafen.Text = strPath;
            }

        }

        private void btnPathGb_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Gameboy ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGb.Text = strPath;
            }
        }

        private void btnPathGba_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Gameboy Advance ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGba.Text = strPath;
            }
        }

        private void btnPathGg_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select GameGear ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGg.Text = strPath;
            }
        }

        private void btnPathLynx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Atari Lynx ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathLynx.Text = strPath;
            }
        }

        private void btnPathMd_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Megadrive/Genesis ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMd.Text = strPath;
            }
        }

        private void btnPathNes_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select NES ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathNes.Text = strPath;
            }
        }

        private void btnPathSnes_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select SNES ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathSnes.Text = strPath;
            }
        }

        private void btnPathNgp_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select NeoGeo Pocket Color ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathNgp.Text = strPath;
            }
        }

        private void btnPathPce_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PC-Engine ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPce.Text = strPath;
            }
        }

        private void btnPathPcfx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PC-FX ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPcfx.Text = strPath;
            }
        }

        

        private void btnPathMs_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Sega Master System ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMs.Text = strPath;
            }
        }
       
        private void btnPathPsx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PSX ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPsx.Text = strPath;
            }
        }

        private void btnPathSs_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Sega Saturn ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathSs.Text = strPath;
            }
        }
        
        private void btnPathVb_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Virtual Boy ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathVb.Text = strPath;
            }
        }

        private void btnPathWswan_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select WonderSwan ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathWswan.Text = strPath;
            }
        }

        // Path page save & restore

        private void btnPathsSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Paths.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);
            //lblPathsSettingsSave.Content = "***Path Settings Saved***";
        }

        private void btnPathsCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);
            //lblPathsSettingsSave.Content = "***Path Settings Reverted***";
        }

        // games list context menu
        private void dgGameList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            dgGameList.Items.Refresh();
            dgGameList.InvalidateVisual();
            FrameworkElement fe = e.Source as FrameworkElement;
            ContextMenu cm = fe.ContextMenu;

            ContextMenu c = (ContextMenu)this.FindName("dgContext");

            // get selected row data
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            if (drv == null)
            {
                c = (ContextMenu)this.FindName("dgContext");
                //c.Visibility = Visibility.Collapsed;
                //c.IsOpen = false;
                return;
            }
           // c.Visibility = Visibility.Visible;


            //MessageBox.Show(drv.ID.ToString());
            string romName = drv.Game;
            int romId = drv.ID;

            // Replace Menu Items
            foreach (MenuItem mi in cm.Items)
            {
                // check whether selection is empty - if so dont display the menu item

                // play game menu item
                if ((String)mi.Header == "Play Game")
                {
                    mi.Header = "Play Game"; // + romName;
                }

                // Favorites toggle
                if ((String)mi.Header == "Favorites")
                {
                    // check the favorite status
                    if (GameScanner.GetFavoriteStatus(romId) == 1)
                        mi.Header = "Add/Remove From Favorites";
                    else
                        mi.Header = "Add/Remove From Favorites";
                }

                // remove game
                if ((String)mi.Header == "Delete From Games Library")
                {
                    mi.Header = "Delete From Games Library"; // + romName;
                }
            }
            

            

            //fe.ContextMenu = CMenu.BuildGamesMenu(dgGameList);
        }

        private void MenuItemFavorite_Click(object sender, RoutedEventArgs e)
        {
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            int romId = drv.ID;
            GameScanner.FavoriteToggle(romId);
            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();


        }

        private void DeleteRom_Click(object sender, RoutedEventArgs e)
        {
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            int romId = drv.ID;
            Game game = Game.GetGame(romId);
            // delete from library
            Game.DeleteGame(game);

            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void CopyLaunchStringToClipboard_Click(object sender, RoutedEventArgs e)
        {
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            int romId = drv.ID;
            GameLauncher.CopyLaunchStringToClipboard(romId);
        }

        private async void LaunchRom_Click(object sender, RoutedEventArgs e)
        {
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;

            // create new GameLauncher instance
            GameLauncher gl = new GameLauncher(romId);

            // popup launch dialog
            var mySettings = new MetroDialogSettings()
            {
                //NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = false
            };

            var controller = await this.ShowProgressAsync("Launching " + gl.SystemName + " Game", "Starting: " + gl.RomName, settings: mySettings);

           
                controller.SetIndeterminate();

                await Task.Delay(100);

                controller.SetCancelable(false);

                string[] returnStr = gl.PathChecks();

                if (returnStr != null && returnStr.Length > 0)
                {
                    string o = "";
                    foreach (string s in returnStr)
                    {
                        o += s + "\n";
                    }
                    controller.SetMessage(o + "\n...Cancelling Operation...");
                    await Task.Delay(100);
                    await controller.CloseAsync();
                }
                else
                {
                    string status = "...Building config...\n";
                    controller.SetMessage(status);
                    await Task.Delay(50);

                    string cfgName;
                    if (gl.ConfigId == 2000000000)
                        cfgName = "Base Configuration";
                    else
                        cfgName = gl.SystemName + " Configuration";

                    status += "Using " + cfgName + "\n";
                    controller.SetMessage(status);
                    await Task.Delay(50);

                    string netplayEnabled;
                    if (gl.Global.enableNetplay == true)
                        netplayEnabled = "Netplay Enabled: Yes\nHost: " + gl.Server.ConfigServerDesc;
                    else
                        netplayEnabled = "Netplay Enabled: No";
                    status += netplayEnabled + "\n";
                    controller.SetMessage(status);
                    await Task.Delay(50);

                    // get base config params
                    string configCmdString = gl.GetCommandLineArguments();

                    string launchGame = "...Launching Game...";
                    status += launchGame + "\n";
                    controller.SetMessage(status);
                    await Task.Delay(50);

                // check whether minimise to taskbar option is checked
                bool taskbar = this.ShowInTaskbar;
                    if (GlobalSettings.Min2TaskBar() == true)
                {
                    this.ShowInTaskbar = true;
                    this.WindowState = WindowState.Minimized;                    
                }


                // launch game
                await Task.Run(() =>
                {
                    // update lastplayed time
                    Game.SetStartedPlaying(gl.GameId);
                    
                    // launch game
                    gl.RunGame(configCmdString);
                });
                    

                if (GlobalSettings.Min2TaskBar() == true)
                {
                    this.ShowInTaskbar = taskbar;
                    this.WindowState = WindowState.Normal;
                }



                await this.Dispatcher.Invoke(async () =>
                {
                    controller.SetTitle("Cleaning Up");
                    controller.SetMessage("Please Wait....");
                    await Task.Delay(100);

                    // update lastfinished time
                    Game.SetFinishedPlaying(gl.GameId);

                    // update gameslibrary data as change has been made
                    GamesLibData.ForceUpdate();

                    // refresh library view
                    GamesLibraryVisualHandler.RefreshGamesLibrary();
                });

                await controller.CloseAsync();
            }

            GamesLibraryVisualHandler.UpdateSidebar(gl.GameId);


                //controller.SetMessage(totalFiles + " files found across all ROM directories");





                /*
                if (controller.IsCanceled)
                {
                    await this.ShowMessageAsync("The operation was cancelled!", romsInserted +  " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
                }
                else
                {
                    await this.ShowMessageAsync("Operation completed", romsInserted + " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
                }

                //Update list
                // ensure 'show all' filter is checked on startup
                btnFavorites.IsChecked = true;
                btnShowAll.IsChecked = true;

                */
                /*
                MessageBoxResult result = MessageBox.Show("RomId: " + gl.GameId.ToString());
                MessageBoxResult result1 = MessageBox.Show("SystemId: " + gl.SystemId.ToString());
                MessageBoxResult result2 = MessageBox.Show("RomName: " + gl.RomName.ToString());
                MessageBoxResult result3 = MessageBox.Show("RomFolder: " + gl.RomFolder.ToString());
                MessageBoxResult result4 = MessageBox.Show("RomPath: " + gl.RomPath.ToString());
                MessageBoxResult result5 = MessageBox.Show("MednafenPath: " + gl.MednafenFolder.ToString());
                */



                
        }


        // servers


        private void rbSrv01_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);
        }

        private void rbSrv02_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);
        }

        private void rbSrv03_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);
        }

        private void rbSrv04_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);
        }

        private void rbSrvCustom_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);

            // unhidden
            tbServerDesc.IsEnabled = true;
            tbServerDesc.Visibility = Visibility.Visible;
            tbHostname.IsEnabled = true;
            tbHostname.Visibility = Visibility.Visible;
            slServerPort.IsEnabled = true;
            slServerPort.Visibility = Visibility.Visible;
            tbPassword.IsEnabled = true;
            tbPassword.Visibility = Visibility.Visible;
            tbGameKey.IsEnabled = true;
            tbGameKey.Visibility = Visibility.Visible;

            lblServerDesc.Visibility = Visibility.Visible;
            lblHostname.Visibility = Visibility.Visible;
            lblServerPort.Visibility = Visibility.Visible;
            lblServerPortTxt.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            //lblGamekey.Visibility = Visibility.Visible;

            //btnServerSaveChanges.Visibility = Visibility.Visible;
            //btnServerCancelChanges.Visibility = Visibility.Visible;
            btnServerSaveChanges.Visibility = Visibility.Collapsed;
            btnServerCancelChanges.Visibility = Visibility.Collapsed;
        }
        private void rbSrvCustom_UnChecked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            ConfigServerSettings.SetSelectedServer(rb);

            // hidden
            tbServerDesc.IsEnabled = false;
            tbServerDesc.Visibility = Visibility.Collapsed;
            tbHostname.IsEnabled = false;
            tbHostname.Visibility = Visibility.Collapsed;
            slServerPort.IsEnabled = false;
            slServerPort.Visibility = Visibility.Collapsed;
            //tbPassword.IsEnabled = false;
            //tbPassword.Visibility = Visibility.Collapsed;
            //tbGameKey.IsEnabled = false;
            //tbGameKey.Visibility = Visibility.Collapsed;

            lblServerDesc.Visibility = Visibility.Collapsed;
            lblHostname.Visibility = Visibility.Collapsed;
            lblServerPort.Visibility = Visibility.Collapsed;
            lblServerPortTxt.Visibility = Visibility.Collapsed;
            //lblPassword.Visibility = Visibility.Collapsed;
            //lblGamekey.Visibility = Visibility.Collapsed;

            btnServerSaveChanges.Visibility = Visibility.Collapsed;
            btnServerCancelChanges.Visibility = Visibility.Collapsed;
        }

        private void btnServerSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ConfigServerSettings.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
        }

        private void btnServerCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            ConfigServerSettings.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
        }


        // Config Tab               

        // Generic Config Selection Buttons CHECKED
        private void btnConfig_Checked(object sender, RoutedEventArgs e)
        {
            // get selected ConfigId
            var rb = sender as RadioButton;
            int systemIdSelected = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
            // Save all control values to database
            ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, systemIdSelected);
            ConfigsVisualHandler.ButtonClick();
        }

        // Generic Config Selection Buttons UNCHECKED
        private void btnConfig_Unchecked(object sender, RoutedEventArgs e)
        {
            // get selected ConfigId
            var rb = sender as RadioButton;
            int systemIdSelected = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
            // Save all control values to database
            ConfigBaseSettings.SaveControlValues(ConfigWrapPanel, systemIdSelected);
        }
        
        // set config entry to defaults
        private void MenuItemDefault_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            RadioButton rb = null;

            if (mi != null)
            {
                rb = ((ContextMenu)mi.Parent).PlacementTarget as RadioButton;
                //MessageBoxResult result4 = MessageBox.Show("RadioButton Found...Name: " + rb.Name);

                // ensure button is selected
                rb.IsChecked = true;


                ConfigBaseSettings.ResetToDefault(rb.Name);
                ConfigBaseSettings.SetButtonState(rb);

                // refresh UI
                int systemIdSelected = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);                
                ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, systemIdSelected);


                lblConfigStatus.Content = "***Defaults Loaded***";
            }           
        }

        // Enable system specific config entry
        private void MenuItemConfigEnable_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            RadioButton rb = null;

            if (mi != null)
            {
                rb = ((ContextMenu)mi.Parent).PlacementTarget as RadioButton;

                // ensure button is selected
                rb.IsChecked = true;

                //MessageBoxResult result4 = MessageBox.Show("RadioButton Found...Name: " + rb.Name);
                ConfigBaseSettings.EnableConfigToggle(rb.Name);
                ConfigBaseSettings.SetButtonState(rb);

                // refresh UI
                int systemIdSelected = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
                ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, systemIdSelected);

                //lblConfigStatus.Content = "***Config Enabled***";
            }
        }


        private void btnConfigSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // get config ID
            int configId = 2000000000;
            foreach (UIElement element in ConfigSelectorWrapPanel.Children)
            {
                if (element is RadioButton)
                {
                    // Is Radiobutton selected?
                    if ((element as RadioButton).IsChecked == true)
                    {
                        string rbName = (element as RadioButton).Name;
                        configId = ConfigBaseSettings.GetConfigIdFromButtonName(rbName);
                    }
                }
            }
            // save config changes
            ConfigBaseSettings.SaveControlValues(ConfigWrapPanel, configId);
            lblConfigStatus.Content = "***Config Saved***";
        }

        private void btnConfigCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            // get config ID
            int configId = 2000000000;
            foreach (UIElement element in ConfigSelectorWrapPanel.Children)
            {
                if (element is RadioButton)
                {
                    // Is Radiobutton selected?
                    if ((element as RadioButton).IsChecked == true)
                    {
                        string rbName = (element as RadioButton).Name;
                        configId = ConfigBaseSettings.GetConfigIdFromButtonName(rbName);
                    }
                }
            }
            // re-load config changes
            ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, configId);
        }


        // settings tab events
        private void btnSettings_Checked(object sender, RoutedEventArgs e)
        {
            SettingsVisualHandler.ButtonClick();
        }

        private async void btnSettingsSaveAllChanges_Click(object sender, RoutedEventArgs e)
        {
            //SettingsHandler sh = new SettingsHandler();
            //sh.SaveAllSettings();

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = true
            };

            var controller = await this.ShowProgressAsync("Please wait...", "Saving Settings\n(This may take a few seconds depending on your system)", settings: mySettings);
            controller.SetCancelable(false);
            controller.SetIndeterminate();

            await Task.Delay(200);

           
                this.Dispatcher.Invoke(() =>
                {
                    Paths.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan);
                    ConfigNetplaySettings.SaveNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
                    ConfigServerSettings.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
                    ConfigBaseSettings.SaveMednafenPathValues(spMedPathSettings);
                    ConfigBaseSettings.SaveBiosPathValues(spSysBiosSettings);
                });

            await controller.CloseAsync();


        }

        private void btnSettingsCancelAllChanges_Click(object sender, RoutedEventArgs e)
        {
            //SettingsHandler sh = new SettingsHandler();
            //sh.LoadAllSettings();

            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan);
            ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            ConfigServerSettings.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
            ConfigBaseSettings.LoadMednafenPathValues(spMedPathSettings);
            ConfigBaseSettings.LoadBiosPathValues(spSysBiosSettings);

        }

        public class OneReturnsTrueConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (int)value == 1;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }
   

    private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* If any right-clicks happen where there is no datagrid row clicked on - dont open the context menu */

            DependencyObject DepObject = (DependencyObject)e.OriginalSource;
            /*
            while ((DepObject != null) && !(DepObject is DataGridColumnHeader))
            {
                DepObject = VisualTreeHelper.GetParent(DepObject);
            }

            if (DepObject == null)
            {                
                return;
            }


            if (DepObject is DataGridColumnHeader)
            {
                // prevent right clicks on header values
                e.Handled = true;
            }
            */

            DataGrid dg = sender as DataGrid;
            Point pt = e.GetPosition(dg);
            DataGridCell dgc = null;
            VisualTreeHelper.HitTest(dg, null, new HitTestResultCallback((result) =>
            {
                // Find the ancestor element form the hittested element
                // e.g., find the DataGridCell if we hittest on the inner TextBlock
                DataGridCell cell = FindVisualParent<DataGridCell>(result.VisualHit);
                if (cell != null)
                {
                    dgc = cell;
                    return HitTestResultBehavior.Stop;
                }
                else
                    return HitTestResultBehavior.Continue;
            }), new PointHitTestParameters(pt));

            if (dgc == null)
            {
                //MessageBox.Show("NULL!");
                e.Handled = true;
            }

        }

        public static Visibility IsDebug
        {
#if DEBUG
            get { return Visibility.Visible; }
#else
        get { return Visibility.Collapsed; }
#endif
        }

        private void btnSavePlatformGamesToDisk_Click(object sender, RoutedEventArgs e)
        {
            string linkTimeLocal = (Assembly.GetExecutingAssembly().GetLinkerTime()).ToString("yyyy-MM-dd HH:mm:ss");
            var platformgames = GDBPlatformGame.GetGames();
    
            string json = JsonConvert.SerializeObject(platformgames.ToArray());
            System.IO.File.WriteAllText(@"Data\Settings\thegamesdbplatformgames_" + linkTimeLocal.Replace(" ", "").Replace(":", "").Replace("-", "") + ".json", json);

        }

        // unlink selected game
        private void btnScrapingUnlinkGame_Click(object sender, RoutedEventArgs e)
        {
            GameScraper.UnlinkGameData(dgGameList);
        }

        private void btnTestGameSearch_Click(object sender, RoutedEventArgs e)
        {
            GameScraper gs = new GameScraper();
            Game game = Game.GetGame(20);

            List<Game> games = Game.GetGames(7);

            foreach (Game g in games)
            {
                List<GDBPlatformGame> result = gs.SearchGameLocal(g.gameName, g.systemId, g.gameId).ToList();
                string glist = g.gameName + "\n-------------------------\n\n";
                foreach (GDBPlatformGame bg in result)
                {
                    glist += bg.GameTitle + "\n";
                }
                MessageBox.Show(glist);
            }

            //List<GDBPlatformGame> result = gs.SearchGameLocal(game.gameName, game.systemId, game.gameId).ToList();
        }

        private void btnScrapingPickGame_Click(object sender, RoutedEventArgs e)
        {
            GameScraper.PickGame(dgGameList);
        }

        // save the layout of all the games library expander states
        private void GamesLibraryExpanderSaveLayout(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("expander triggered");
            GamesLibraryVisualHandler.SaveExpanderStates();
        }

        private void MW_Closing(object sender, CancelEventArgs e)
        {
            
                // save games library expander states
                GamesLibraryVisualHandler.SaveExpanderStates();
              
        }

        
    }

    
}
