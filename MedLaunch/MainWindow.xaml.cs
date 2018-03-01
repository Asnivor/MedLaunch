using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using System.IO;
using MedLaunch.Models;
using MedLaunch.Classes;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Threading;
using Medlaunch.Classes;
using System.Net;
using MedLaunch.Classes.Scraper;
using MedLaunch.Classes.GamesLibrary;
using System.Collections.ObjectModel;
using MedLaunch.Classes.VisualHandlers;
using Newtonsoft.Json.Linq;
using MedLaunch.Classes.IO;
using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.Controls.VirtualDevices;
using MedLaunch.Classes.Controls.InputManager;
using MedLaunch.Classes.Scanning;
using System.Windows.Threading;
using MedLaunch._Debug.ScrapeDB.ReplacementDocs;
using MedLaunch.Classes.MednaNet;
using ucon64_wrapper;
using System.Windows.Interactivity;
using MedLaunch.Common.Eventing.Listeners;
using MedLaunch.Common.IO.Compression;
using MedLaunch.Windows;
using MedLaunch.Classes.Controls;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Setting Properties

        /// <summary>
        /// Public reference to the main Application (App) object
        /// </summary>
        public App _App { get; set; }

        /// <summary>
        /// Public observable collection that references the main games library view model
        /// </summary>
        public ObservableCollection<GamesLibraryModel> dg { get; set; }

        /// <summary>
        /// If this is set to false then the GamesLibraryModel is not refreshed
        /// </summary>
        public bool SettingsDirtyFlag { get; set; }

        /// <summary>
        /// Instance of the Discord visual handler
        /// </summary>
        public DiscordVisualHandler DVH { get; set; }

        #endregion

        #region State vars

        /// <summary>
        /// The last generated LaunchString
        /// </summary>
        public string LaunchString { get; set; }

        /// <summary>
        /// Initial game data when opening the game inspection window
        /// </summary>
        public Game InspGame { get; set; }

        /// <summary>
        /// Game object returned after scraping within the inspection window
        /// </summary>
        public Game InspGameScrape { get; set; }

        /// <summary>
        /// Vars used when choosing a launch disc from a multi-disc game
        /// </summary>
        public string[] DiscArray { get; set; }
        public int DiscSelected { get; set; }

        /// <summary>
        /// The last selected ControllerDefinition
        /// </summary>
        public IDeviceDefinition ControllerDefinition { get; set; }

        /// <summary>
        /// Games library filtering
        /// </summary>
        public CountryFilter _countryFilter { get; set; }
        public int _filterId { get; set; }

        /// <summary>
        /// Last set width of the games library sidebar
        /// </summary>
        public Double GamesSidebarWidth { get; set; }

        /// <summary>
        /// State variables concerning medlaunch and mednafen update status
        /// </summary>
        public int UpdateStatus { get; set; }
        public bool UpdateStatusML { get; set; }
        public bool UpdateStatusMF { get; set; }

        /// <summary>
        /// Signs whether the 'CONFIGS' tab is currently selected
        /// </summary>
        public bool? ConfigsTabSelected { get; set; }

        /// <summary>
        /// The current case conversion mode
        /// </summary>
        public int CaseConvert { get; set; }

        #endregion

        #region MainWindow Construction

        public MainWindow()
        {
            InitializeComponent();

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            // init update vars
            DownloadComplete = false;
            UpdateStatus = 0;

            // setup main references
            _App = ((App)Application.Current);
            MainWindow mw = this;

            // init settings dirty flag
            SettingsDirtyFlag = false; // not dirty - do not save any settings

            // instantiate ScrapedContent Object
            ScrapeDB ScrapedData = new ScrapeDB();
            ScraperMaster.MasterList = ScraperMaster.GetMasterList();

            // hide the zoom slider on the settings page (should not be visible to the user)
            dpZoomSlider.Visibility = Visibility.Collapsed;

            // set initial window size
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
            string appVersion = VersionChecker.ReturnApplicationVersion();
            string devBuildNo = VersionChecker.GetDevBuild();
            
            // set window title
            string linkTimeLocal = (Assembly.GetExecutingAssembly().GetLinkerTime()).ToString("yyyy-MM-dd HH:mm:ss");

            if (devBuildNo == null)
                this.Title = "MedLaunch (v" + appVersion + ") - Windows Front-End for Mednafen";
            else
                this.Title = "MedLaunch (v" + appVersion + ") - DevBuild-" + devBuildNo + " - Windows Front-End for Mednafen";

            rightMenuLabel.Text = "(Compatible Mednafen v" + VersionChecker.GetMednafenCompatibilityMatrix().Last().Version + " - v" + VersionChecker.GetMednafenCompatibilityMatrix().First().Version + ")";

            /*
             *  Startup checks
             */

            // is DB path to Mednafen set and working? If not force user to select it
            Paths.MedPathRoutine(btnPathMednafen, tbPathMednafen);

            // ensure 'show all' filter is checked on startup
            btnShowAll.IsChecked = true;

            // ensure ALL country filter is checked
            srcFilterALL.IsChecked = true;

            // load globalsettings for front page
            GlobalSettings.LoadGlobalSettings(chkEnableNetplay, chkEnableSnes_faust, chkEnablePce_fast, gui_zoom_combo, chkMinToTaskbar, chkHideSidebar,
               chkAllowBanners, chkAllowBoxart, chkAllowScreenshots, chkAllowFanart, chkPreferGenesis, chkAllowManuals, chkAllowMedia, chkSecondaryScraperBackup,
               rbGDB, rbMoby, slScreenshotsPerHost, slFanrtsPerHost, chkAllowUpdateCheck, chkBackupMednafenConfig, chkSaveSysConfigs, comboImageTooltipSize, chkLoadConfigsOnStart, chkEnableConfigToolTips,
               chkshowGLYear, chkshowGLESRB, chkshowGLCoop, chkshowGLDeveloper, chkshowGLPublisher, chkshowGLPlayers, chkEnableClearCacheOnExit, chkrememberSysWinPositions, chkHideCountryFilter, cbFormatGameTitles);
            //gui_zoom.Value = Convert.ToDouble(gui_zoom_combo.SelectedValue);
            GlobalSettings gs = GlobalSettings.GetGlobals();
            mainScaleTransform.ScaleX = Convert.ToDouble(gs.guiZoom);
            mainScaleTransform.ScaleY = Convert.ToDouble(gs.guiZoom);

            // set the case conversion mode from the database
            CaseConvert = gs.changeTitleCase;

            // load netplay settings for netplay page
            ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            
            // load path settings for paths page
            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd);

            SettingsVisualHandler.PopulateServers(lvServers);
            SettingsVisualHandler.ServerSettingsInitialButtonHide();

            // load mednanet settings
            MednaNetSettings.PopulateUISettings(this);

            /*
             *  Config Tab
             */

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

            // controls tab
            btnAllControls.IsChecked = true;
            //btnControlNes.IsChecked = true;

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
            wb.Navigate("https://mednafen.github.io/");

            // create handler for browser control navigated event
            wb.Navigated += new NavigatedEventHandler(wb_Navigated);

            // games library update sidebar
            GamesLibraryVisualHandler.UpdateSidebar();

            // load expander states
            GamesLibraryVisualHandler.LoadExpanderStates();
            
            // about tab
            lblVersion.Visibility = Visibility.Collapsed;
            lblDate.Visibility = Visibility.Collapsed;
            tbNotes.Visibility = Visibility.Collapsed;
            btnUpdate.Visibility = Visibility.Collapsed;
            lbl1.Visibility = Visibility.Collapsed;
            lbl2.Visibility = Visibility.Collapsed;
            lbl3.Visibility = Visibility.Collapsed;
            lbl4.Visibility = Visibility.Collapsed;
            lblNoUpdate.Visibility = Visibility.Collapsed;
            btnReLink.Visibility = Visibility.Collapsed;

            // set library sidebar width from db if games library is not null
            GamesSidebarWidth = gs.sidebarwidth;
            if (Game.GetGames().Where(a => a.hidden != true).Count() > 0)
            {
                sidebarColumn.Width = new GridLength(gs.sidebarwidth);
            }
            else
            {
                sidebarColumn.Width = new GridLength(0);
            }

            // enable tooltips if neccesary
            if (gs.enableConfigToolTips == true)
            {
                ConfigToolTips.SetToolTips(1);
            }

            // set all tooltips to remain open until user moves the mouse away
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            // initialise joystick/keyboard input class            
            Input.Initialize(this);

            // initialise PSX SBI class
            PsxSBI psxsbi = new PsxSBI();

            // searchtimer - defines a period of time to wait whilst user is typing in the search box before actually searching
            // (makes the UI less laggy)
            _searchTimer = new DispatcherTimer();
            _searchTimer.Tick += new EventHandler(OnSearchTimerTick);
            _searchTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            // set background image
            SetBackgroundImage();

            // Run an emptyload of the log parser (running mednafen once if neccesary to generate log files)
            LogParser.Instance.ParseData();
        }

        #endregion

        #region Medlaunch visual properties and methods

        public void SetBackgroundImage()
        {
            var gs = GlobalSettings.GetGlobals();
            double opac = gs.bgImageOpacity;
            string path = gs.bgImagePath;
            int type = gs.bgImageDisplayType;

            RootGrid.Background = null;

            ImageBrush ib = new ImageBrush();

            if (!File.Exists(GlobalSettings.GetFullBGImagePath(path)))
            {
                ib.ImageSource = new BitmapImage(new Uri(GlobalSettings.GetFullBGImagePath(GlobalSettings.GetDefaultBeetlePath())));
            }
            else
            {
                ib.ImageSource = new BitmapImage(new Uri(GlobalSettings.GetFullBGImagePath(path)));
            }

            ib.Opacity = opac;

            // tile image
            if (type == 1)
            {
                ib.TileMode = TileMode.Tile;
                Rect r = new Rect(new Size(32, 32));
                ib.Viewport = r;
                ib.ViewportUnits = BrushMappingMode.Absolute;
            }

            // stretch but maintain aspect
            if (type == 2)
            {
                ib.Stretch = Stretch.Uniform;
            }

            // original size image
            if (type == 3)
            {
                ib.Stretch = Stretch.None;
            }

            RootGrid.Background = ib;
        }

        /// <summary>
        /// Instance of the color scheme and background image configuration window
        /// </summary>
        private MetroWindow colorSchemeChanger;

        /// <summary>
        /// Launches a new AccentStyleWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeColorScheme_Click(object sender, RoutedEventArgs e)
        {
            if (colorSchemeChanger != null)
            {
                colorSchemeChanger.Activate();
                return;
            }

            colorSchemeChanger = new AccentStyleWindow();
            colorSchemeChanger.Owner = this;
            colorSchemeChanger.Closed += (o, args) => colorSchemeChanger = null;
            colorSchemeChanger.Left = this.Left + this.ActualWidth / 2.0;
            colorSchemeChanger.Top = this.Top + this.ActualHeight / 2.0;
            colorSchemeChanger.Show();
        }

        /// <summary>
        /// Resets the current application scaling factor to default (1.0)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {
            ((Slider)sender).Value = 1.0;
        }

        /// <summary>
        /// Changes the current scaling factor based on combobox selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gui_zoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = (sender as ComboBox).SelectedValue;
            double value = 0;
            if (val != null)
            {
                value = double.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDouble((sender as ComboBox).SelectedValue);
            }

            //MessageBox.Show(value.ToString());
            //gui_zoom.Value = value;
            if (value > 0)
            {
                GlobalSettings.UpdateGuiZoom(value);
            }

            // now update zoom
            ScaleTransform st = (ScaleTransform)this.FindName("mainScaleTransform");
            st.ScaleX = value;
            st.ScaleY = value;
        }

        #endregion

        #region Web Browser

        /// <summary>
        /// Silent browser navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion

        #region Game Scraping

        private async void btnReLink_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Import",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await this.ShowProgressAsync("Data Re-Linker", "Re-importing orphaned scraped data into games library main datagrid view", true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            this.Dispatcher.Invoke(() =>
            {
                //GameListBuilder.ReLinkData();
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("Data Re-Linker", "Linking Cancelled");
                //GameListBuilder.UpdateFlag();
                GamesLibraryVisualHandler.RefreshGamesLibrary();
            }
            else
            {
                await this.ShowMessageAsync("Data Re-Linker", "Linking Completed");
                GamesLibraryVisualHandler.RefreshGamesLibrary();
                // GameListBuilder.UpdateFlag();
            }
        }

        private void btnScrapingPickGames_Click(object sender, RoutedEventArgs e)
        {
            ScraperLookup.PickGames(dgGameList);
        }

        private void btnScrapingPickGame_Click(object sender, RoutedEventArgs e)
        {
            ScraperLookup.PickGame(dgGameList);
        }

        private async void btnScrapingReScrape_Click(object sender, RoutedEventArgs e)
        {
            var r = (GamesLibraryModel)dgGameList.SelectedItem;
            // get the gamesdbid
            if (Game.GetGame(Convert.ToInt32(r.ID)).gdbId == null)
            {
                GamesLibraryView.RestoreSelectedRow();
                return;
            }


            int gdbId = Game.GetGame(Convert.ToInt32(r.ID)).gdbId.Value;
            // re-scrape the game
            if (gdbId > 0)
            {
                MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                var mySettings = new MetroDialogSettings()
                {
                    NegativeButtonText = "Cancel Scraping",
                    AnimateShow = false,
                    AnimateHide = false
                };
                var controller = await mw.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);

                ScraperHandler sh = new ScraperHandler(gdbId, r.ID);
                await Task.Delay(100);
                await Task.Run(() =>
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }
                    sh.ScrapeGame(controller);
                });

                await controller.CloseAsync();

                if (controller.IsCanceled)
                {
                    await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
                    GamesLibraryView.RestoreSelectedRow();
                    //GamesLibraryVisualHandler.RefreshGamesLibrary();
                }
                else
                {
                    await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
                    GamesLibraryView.RestoreSelectedRow();
                }

                /*
                var ro = (GamesLibraryModel)dgGameList.SelectedItem;
                dgGameList.SelectedItem = null;
                dgGameList.SelectedItem = ro;
                */
            }
        }

        private async void AuditScrapedData_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            await mw.ShowChildWindowAsync(new ScrapedDataAudit()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Scraped Data Folder Audit",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        #endregion

        #region ROM/Disc Scanning

        private void RescanDisks(object sender, RoutedEventArgs e)
        {
            RescanSystemDiscs(0);
        }

        /// <summary>
        /// Scan ROMs
        /// </summary>
        /// <param name="sysId"></param>
        private async void RescanSystemRoms(int sysId)
        {

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,
                AnimateHide = true
            };

            string initial = "";
            initial = "Scanning ROM Directories";

            var controller = await this.ShowProgressAsync(initial, "Determining Paths and Counting Files...", settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();

            await Task.Delay(100);

            string output = "";
            int addedStats = 0;
            int updatedStats = 0;
            int untouchedStats = 0;
            int hiddenStats = 0;

            RomScan rs = new RomScan();

            await Task.Delay(100);
            List<GSystem> scanList = new List<GSystem>();
            if (sysId == 0)
            {
                /* scan of all roms has been selected */

                // mark all roms in database as hidden where the system path is not set or if path no longer exists
                foreach (var hs in GameScanner.Systems)
                {

                    string path = rs.GetPath(hs.systemId);
                    if (path == "" || path == null || !Directory.Exists(path))
                    {
                        // No path returned or path is not valid - Mark existing games in Db as hidden
                        rs.MarkAllRomsAsHidden(hs.systemId);
                        hiddenStats += GameScanner.HiddenStats;
                        GameScanner.HiddenStats = 0;
                    }

                }
                scanList = GameScanner.RomSystemsWithPaths;

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
                    hiddenStats += GameScanner.HiddenStats;
                    GameScanner.HiddenStats = 0;
                }

                scanList = (from s in GameScanner.RomSystemsWithPaths
                            where s.systemId == sysId
                            select s).ToList();
            }

            if (scanList.Count > 0)
            {
                // start the operations on a different thread
                await Task.Run(() =>
                {
                    // data has been returned

                    // how many systems returned
                    int sysCount = scanList.Count;
                    controller.Minimum = 0;
                    controller.Maximum = sysCount;
                    int progress = 0;

                    // iterate through each system that has a system ROM path set
                    foreach (var s in scanList)
                    {
                        if (controller.IsCanceled)
                        {
                            break;
                        }

                        // start scanning
                        controller.SetTitle("Starting " + s.systemName + " (" + s.systemCode + ") Scan");
                        Task.Delay(100);
                        //output += "Scanning....";
                        controller.SetMessage(output);

                        progress++;
                        controller.SetProgress(progress);


                        // Start ROM scan for this system
                        rs.BeginRomImport(s.systemId, controller);


                        //output += ".....Completed\n\n";

                        // update totals
                        addedStats += GameScanner.AddedStats;
                        updatedStats += GameScanner.UpdatedStats;
                        untouchedStats += GameScanner.UntouchedStats;
                        hiddenStats += GameScanner.HiddenStats;

                        //output += rs.AddedStats + " ROMs Added\n" + rs.UpdatedStats + " ROMs Updated\n" + rs.UntouchedStats + " ROMs Skipped\n" + rs.HiddenStats + " ROMs Missing (marked as hidden)\n";
                        //controller.SetMessage(output);

                        // reset class totals
                        GameScanner.AddedStats = 0;
                        GameScanner.UpdatedStats = 0;
                        GameScanner.UntouchedStats = 0;
                        GameScanner.HiddenStats = 0;

                        Task.Delay(200);
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
                    GameScanner.SaveToDatabase();
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

            // update data is changes have been made
            if (addedStats > 0 || updatedStats > 0 || hiddenStats > 0)
                //GameListBuilder.UpdateFlag();

                // refresh library view
                //GamesLibraryVisualHandler.RefreshGamesLibrary();
                // refresh games library view
                GamesLibraryVisualHandler.DoFullUpdate();
        }

        /// <summary>
        /// Scan Discs
        /// </summary>
        /// <param name="sysId"></param>
        private async void RescanSystemDiscs(int sysId)
        {

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,
                AnimateHide = true
            };

            string initial = "";
            initial = "Scanning Disc Directories";


            var controller = await this.ShowProgressAsync(initial, "Determining Paths and Counting Files...", settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();

            await Task.Delay(100);

            string output = "";
            int addedStats = 0;
            int updatedStats = 0;
            int untouchedStats = 0;
            int hiddenStats = 0;

            DiscScan ds = new DiscScan();

            await Task.Delay(100);
            List<GSystem> scanList = new List<GSystem>();
            if (sysId == 0)
            {
                /* scan of all discs has been selected */

                // mark all roms in database as hidden where the system path is not set or if path no longer exists
                /*
                foreach (var hs in GameScanner.Systems)
                {
                    string path = rs.GetPath(hs.systemId);
                    if (path == "" || path == null || !Directory.Exists(path))
                    {
                        // No path returned or path is not valid - Mark existing games in Db as hidden
                        rs.MarkAllRomsAsHidden(hs.systemId);
                        hiddenStats += GameScanner.HiddenStats;
                        GameScanner.HiddenStats = 0;
                    }
                }
                */

                scanList = GameScanner.DiskSystemsWithPaths;
            }
            else
            {
                /* only one system has been selected for scanning */

                // mark all roms in database as hidden for this system path is not set or if path no longer exists                
                string path = ds.GetPath(sysId);
                /*
                if (path == "" || path == null || !Directory.Exists(path))
                {
                    // No path returned or path is not valid - Mark existing games in Db as hidden
                    rs.MarkAllRomsAsHidden(sysId);
                    hiddenStats += GameScanner.HiddenStats;
                    GameScanner.HiddenStats = 0;
                }
                */

                scanList = (from s in GameScanner.DiskSystemsWithPaths
                            where s.systemId == sysId
                            select s).ToList();
            }

            if (scanList.Count > 0)
            {
                // start the operations on a different thread
                await Task.Run(() =>
                {
                    // data has been returned

                    // how many systems returned
                    int sysCount = scanList.Count;
                    controller.Minimum = 0;
                    controller.Maximum = sysCount;
                    int progress = 0;

                    // iterate through each system that has a system ROM path set
                    foreach (var s in scanList)
                    {
                        if (controller.IsCanceled)
                        {
                            break;
                        }

                        // start scanning
                        controller.SetTitle("Starting " + s.systemName + " (" + s.systemCode + ") Scan");
                        Task.Delay(100);
                        //output += "Scanning....";
                        controller.SetMessage(output);

                        progress++;
                        controller.SetProgress(progress);


                        // Start ROM scan for this system
                        ds.BeginDiscImportImproved(s.systemId, controller);


                        //output += ".....Completed\n\n";

                        // update totals
                        addedStats += GameScanner.AddedStats;
                        updatedStats += GameScanner.UpdatedStats;
                        untouchedStats += GameScanner.UntouchedStats;
                        hiddenStats += GameScanner.HiddenStats;

                        //output += rs.AddedStats + " ROMs Added\n" + rs.UpdatedStats + " ROMs Updated\n" + rs.UntouchedStats + " ROMs Skipped\n" + rs.HiddenStats + " ROMs Missing (marked as hidden)\n";
                        //controller.SetMessage(output);

                        // reset class totals
                        GameScanner.AddedStats = 0;
                        GameScanner.UpdatedStats = 0;
                        GameScanner.UntouchedStats = 0;
                        GameScanner.HiddenStats = 0;

                        Task.Delay(200);
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

                    GameScanner.SaveToDatabase();
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

            // update data is changes have been made
            if (addedStats > 0 || updatedStats > 0 || hiddenStats > 0)
                // GameListBuilder.UpdateFlag();

                // refresh library view
                //GamesLibraryVisualHandler.RefreshGamesLibrary();

                // refresh games library view
                GamesLibraryVisualHandler.DoFullUpdate();
        }

        private void RescanRoms(object sender, RoutedEventArgs e)
        {
            RescanSystemRoms(0);
        }

        #endregion

        #region Administrator / Debug

        #region Not used anymore??

        private void btnmobyPlatformListDumpToFile_Click(object sender, RoutedEventArgs e)
        {
            //MobyGames.DumpPlatformGamesToDisk();
        }

        private void btnCombine_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.BeginMerge(false, false, false);
        }

        private void btnGetManuals_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.ScrapeManuals();
        }

        private void btnCombineManual_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.BeginMerge(true, false, false);
        }

        private void btnCombineManualnonleven_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.BeginMerge(true, true, false);
        }

        private void btnCombineManualEverything_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.BeginMerge(false, false, true);
        }

        private void btnCombineGamesDatabaseOrgManuals_Click(object sender, RoutedEventArgs e)
        {
            //CreateMasterList j = new CreateMasterList();
            //j.ParseGamesDatabaseOrgManuals();
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
                //List<GDBPlatformGame> gs = GameScraper.DatabasePlatformGamesImport(controller);
                //GDBScraper.ScrapeBasicGamesList(controller);
                /*
                if (!controller.IsCanceled)
                {
                    //GDBPlatformGame.SaveToDatabase(gs);       // disabled for the moment - working with flat json files
                    // save to json file
                    controller.SetMessage("Saving to file...");
                    // set file path
                    string filePath = @"..\..\Data\System\TheGamesDB.json";
                    //  dump file
                    string json = JsonConvert.SerializeObject(gs, Formatting.Indented);
                    File.WriteAllText(filePath, json);
                }
                */

            });
            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("GDB Master Games List Download", "Operation Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("GDB Master Games List Download", "Scanning Completed");
            }
        }

        private async void btnScrapeAllGameInfo_Click(object sender, RoutedEventArgs e)
        {
            // ensure directory is created
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Settings");

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await this.ShowProgressAsync("MedLaunch - Downloading all game info (will take a while)", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);
            await Task.Run(() =>
            {
                Task.Delay(1);
                // get list of all platformgames in the database
                controller.SetMessage("Unumerating PlatForm games from database...");
                var games = ScrapeDB.AllScrapeData;
                int gameCount = games.Count;
                int i = 0;

                foreach (var g in games)
                {
                    i++;
                    WebOps wo = new WebOps();
                    wo.Params = "/GetGame.php?id=" + g.gid;

                }



            });
            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("MedLaunch - GameScraper", "Operation Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("MedLaunch - GameScraper", "Downloading Completed");
            }
        }

        #endregion

        /// <summary>
        /// Imports skeleton key data into database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbgImportSkeleton_Click(object sender, RoutedEventArgs e)
        {
            _Debug.skeletonKey.AdminSkeletonKey sdb = new _Debug.skeletonKey.AdminSkeletonKey();
            sdb.EntryPoint();
        }


        private void dbgImportPlatGames_Click(object sender, RoutedEventArgs e)
        {
            _Debug.ScrapeDB.AdminScrapeDb adb = new _Debug.ScrapeDB.AdminScrapeDb();
            adb.ScrapePlatformGames();
        }

        private void dbgImportMobyPlatGames_Click(object sender, RoutedEventArgs e)
        {
            _Debug.ScrapeDB.AdminScrapeDb adb = new _Debug.ScrapeDB.AdminScrapeDb();
            adb.ScrapeMobyPlatformGames();
        }

        private void dbgtest1_Click(object sender, RoutedEventArgs e)
        {
            List<_Debug.ScrapeDB.MasterView> mv = _Debug.ScrapeDB.MasterView.GetMasterView();
        }

        private void dbgExactMatch_Click(object sender, RoutedEventArgs e)
        {
            _Debug.ScrapeDB.AdminScrapeDb adb = new _Debug.ScrapeDB.AdminScrapeDb();
            adb.MobyExactMatch();
        }

        private void dbgManualMatch_Click(object sender, RoutedEventArgs e)
        {
            _Debug.ScrapeDB.AdminScrapeDb adb = new _Debug.ScrapeDB.AdminScrapeDb();
            adb.MobyManualMatch(false);
        }

        private void dbgAutoWordMatch_Click(object sender, RoutedEventArgs e)
        {
            _Debug.ScrapeDB.AdminScrapeDb adb = new _Debug.ScrapeDB.AdminScrapeDb();
            adb.MobyManualMatch(true);
        }

        /// <summary>
        /// import all nointro DAT files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoIntroImportAll_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.ImportRoutine(_Debug.DATDB.ProviderType.NoIntro, 0);
        }

        /// <summary>
        /// import all tosec DAT files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToSecImportAll_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.ImportRoutine(_Debug.DATDB.ProviderType.ToSec, 0);
        }

        /// <summary>
        /// Scrapes all saturn json data into database
        /// and populates the intermediary debug DB (PSXDC.db)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportSaturn(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.ImportRoutine(_Debug.DATDB.ProviderType.Satakore, 0);
        }

        /// <summary>
        /// builds/updates the DAT_Game table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcessDATGameTable_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.ProcessTopLevelGames();
        }

        /// <summary>
        /// identify games with missing years and publishers and try to fix that from the rom sets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcYears_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.CalculateYearsAndPublishers();
        }



        /// <summary>
        /// identify games with missing years and publishers and try to fix that from the rom sets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcYearsManual_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.CalculateYearsAndPublishersManual();
        }

        /// <summary>
        /// iterate through each database rom entry and attempt to populate CRC32 entries from local DAT files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popCRCTOSEC_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();

            //tosec
            db.ImportCRC32Only(_Debug.DATDB.ProviderType.ToSec, 0);
        }

        /// <summary>
        /// iterate through each database rom entry and attempt to populate CRC32 entries from local DAT files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popCRCNOINTRO_Click(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();

            // nointro first
            db.ImportCRC32Only(_Debug.DATDB.ProviderType.NoIntro, 0);
        }

        private void btnParseRDList_Click(object sender, RoutedEventArgs e)
        {
            MedLaunch._Debug.ScrapeDB.Game_Doc.ParseManuals();
            // CreateMasterList j = new CreateMasterList();
            //j.ParseReplacementDocsManuals();
        }

        private async void btnmobyPlatformList_Click(object sender, RoutedEventArgs e)
        {
            // get the main window
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Scraping MobyGames (Basic) Games List", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                MobyScraper.ScrapeBasicGamesList(controller);
            });

            //MobyGames.ScrapeAllPlatformGames();
            // App app = ((App)Application.Current);

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("Moby Master Games List Download", "Scraping Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("Moby Master Games List Download", "Scraping Completed");
            }
        }

        private async void btnRDmanuallist_Click(object sender, RoutedEventArgs e)
        {

            // get the main window
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Scraping Manual List from replacementdocs.com", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                RdScraper.ScrapeBasicDocsList(controller);
            });

            //MobyGames.ScrapeAllPlatformGames();
            // App app = ((App)Application.Current);

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("replacementdocs.com manual link scraper", "Scraping Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("replacementdocs.com manual link scraper", "Scraping Completed");
            }
        }

        /// <summary>
        /// Scrapes all PSX game info from psxdatacenter
        /// and populates the intermediary debug DB (PSXDC.db)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrapePsxDataCenter(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.Platforms.PSXDATACENTER.PsxDc.ScrapeInitialList(true);
        }

        /// <summary>
        /// Scrapes all PSX game info from psxdatacenter
        /// and populates the intermediary debug DB (PSXDC.db)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrapePsxDataCenterNoDownload(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.Platforms.PSXDATACENTER.PsxDc.ScrapeInitialList(false);
        }



        /// <summary>
        /// Gets extra detail from online
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportPsxDataCenterExtraDetail(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.Platforms.PSXDATACENTER.PsxDc.GetExtraDetail();
        }

        /// <summary>
        /// Imports data from psxdc.db into AsniDAT.db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportPsxDataCenter(object sender, RoutedEventArgs e)
        {
            _Debug.DATDB.AdminDATDB db = new _Debug.DATDB.AdminDATDB();
            db.ImportRoutine(_Debug.DATDB.ProviderType.PsxDataCenter, 0);
        }

        private void btnGenerateToolTips_Click(object sender, RoutedEventArgs e)
        {
            var r = ConfigToolTips.GetDocumentationStrings();
        }

        private void btnPopToolTips_Click(object sender, RoutedEventArgs e)
        {
            ConfigToolTips.SetToolTips(1);
        }

        #endregion

        #region Generaic Handlers

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

        #endregion

        #region OnLoad / Opening / Closing

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // if check for updates on start is allowed
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\MedLaunch.db"))
            {
                bool updateCheck = GlobalSettings.GetGlobals().checkUpdatesOnStart.Value;
                if (updateCheck == true)
                {
                    UpdateCheck(true);
                }

                // if import configs on start is enabled then import configs
                bool importCheck = GlobalSettings.GetGlobals().importConfigsOnStart.Value;
                if (importCheck == true && Directory.Exists(Paths.GetPaths().mednafenExe))
                {
                    ConfigImport ci = new ConfigImport();
                    ci.ImportAll(null);
                }
            }

            //System.Windows.Data.CollectionViewSource globalSettingsViewModelViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("globalSettingsViewModelViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // globalSettingsViewModelViewSource.Source = [generic data source]

            SettingsDirtyFlag = true; // now safe to save settings

            MiscVisualHandler.RefreshCoreVisibilities();
        }

        private void MW_Closing(object sender, CancelEventArgs e)
        {

            // save games library expander states
            GamesLibraryVisualHandler.SaveExpanderStates();

            // clear the rom cache folder if this option has been enabled
            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (gs.enableClearCacheOnExit == true)
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Data\\Cache"))
                {
                    FileAndFolder.ClearFolder(AppDomain.CurrentDomain.BaseDirectory + "Data\\Cache");
                }

            }

            // save the games library column states to disk
            App _App = ((App)Application.Current);

            // first save the active state to memory (as this is only usually saved when the filter is unchecked)
            GamesLibraryVisualHandler.SaveSelectedColumnState();
            // now save collection to disk
            _App.GamesLibrary.SaveDataGridStatesToDisk();

        }

        #endregion

        #region Games Library Filters

        private void resetGLColStates_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults();
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void AllGamesResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(1);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void FavoriteResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(2);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void UnscrapedResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(3);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void NesResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(4);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void SnesResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(5);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void SmsResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(6);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void MdResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(7);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void PceResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(8);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void VbResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(9);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void NgpResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(10);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void WswanResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(11);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void GbResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(12);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void GbaResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(13);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void GgResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(14);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void LynxResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(15);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void SsResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(16);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void PsxResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(17);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void PcecdResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(18);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        private void PcfxResetToDefaultColumnState_Click(object sender, RoutedEventArgs e)
        {
            App _App = ((App)Application.Current);
            _App.GamesLibrary.LoadColumnDefaults(19);
            GamesLibraryVisualHandler.ReloadSelectedColumnState();
        }

        /// <summary>
        /// Country filter buttons to the right of the games library dynamic search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void srcFilter_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string name = rb.Name.Replace("srcFilter", "");

            switch (name)
            {
                case "ALL":
                    _countryFilter = CountryFilter.ALL;
                    break;
                case "USA":
                    _countryFilter = CountryFilter.USA;
                    break;
                case "EUR":
                    _countryFilter = CountryFilter.EUR;
                    break;
                case "JPN":
                    _countryFilter = CountryFilter.JPN;
                    break;
            }
            _App.GamesLibrary.CountryFilter(_countryFilter);

            //GameListBuilder.GetGames(dgGameList, _filterId, tbFilterDatagrid.Text, _countryFilter);

        }

        /// <summary>
        /// Called when a filter button is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilters_UnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            RadioButton[] btns = GamesLibraryVisualHandler.ReturnFilterButtons();
            for (int i = 0; i < btns.Length; i++)
            {
                if (rb.Name == btns[i].Name)
                {
                    GamesLibraryVisualHandler.SaveColumnInfo(i + 1);
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the games library ShowAll filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowAll_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(1);

            // load datagrid
            _filterId = 0;
            GamesLibraryVisualHandler.LoadColumnInfo(1);
            _App.GamesLibrary.FilterByFilterButton(1);
        }

        /// <summary>
        /// Called when the games library Favorites filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFavorites_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(2);

            // load datagrid
            _filterId = -1;
            GamesLibraryVisualHandler.LoadColumnInfo(2);
            _App.GamesLibrary.FilterByFilterButton(2);
        }

        /// <summary>
        /// Called when the games library Unscraped filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnscraped_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(3);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(3);
            _App.GamesLibrary.FilterByFilterButton(3);
        }

        /// <summary>
        /// Called when the games library Nes filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNes_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(4);

            // load datagrid
            _filterId = 11;
            GamesLibraryVisualHandler.LoadColumnInfo(4);
            _App.GamesLibrary.FilterByFilterButton(4);
        }

        /// <summary>
        /// Called when the games library Snes filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSnes_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(5);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(5);
            _App.GamesLibrary.FilterByFilterButton(5);
        }

        /// <summary>
        /// Called when the games library SMS filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSms_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(6);

            // load datagrid
            _filterId = 10;
            GamesLibraryVisualHandler.LoadColumnInfo(6);
            _App.GamesLibrary.FilterByFilterButton(6);
        }

        /// <summary>
        /// Called when the games library MD filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMd_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(7);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(7);
            _App.GamesLibrary.FilterByFilterButton(7);
        }

        /// <summary>
        /// Called when the games library SS filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSs_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(16);

            // load datagrid
            _filterId = 13;
            GamesLibraryVisualHandler.LoadColumnInfo(16);
            _App.GamesLibrary.FilterByFilterButton(16);
        }

        /// <summary>
        /// Called when the games library PSX filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPsx_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(17);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(17);
            _App.GamesLibrary.FilterByFilterButton(17);
        }

        /// <summary>
        /// Called when the games library PCE filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPce_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(8);

            // load datagrid
            _filterId = 7;
            GamesLibraryVisualHandler.LoadColumnInfo(8);
            _App.GamesLibrary.FilterByFilterButton(8);
        }

        /// <summary>
        /// Called when the games library PCECD filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPcecd_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(18);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(18);
            _App.GamesLibrary.FilterByFilterButton(18);
        }

        /// <summary>
        /// Called when the games library PC-FX filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPcfx_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(19);

            // load datagrid
            _filterId = 8;
            GamesLibraryVisualHandler.LoadColumnInfo(19);
            _App.GamesLibrary.FilterByFilterButton(19);
        }

        /// <summary>
        /// Called when the games library VB filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVb_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(9);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(9);
            _App.GamesLibrary.FilterByFilterButton(9);
        }

        /// <summary>
        /// Called when the games library NGP filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNgp_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(10);

            // load datagrid
            _filterId = 6;
            GamesLibraryVisualHandler.LoadColumnInfo(10);
            _App.GamesLibrary.FilterByFilterButton(10);
        }

        /// <summary>
        /// Called when the games library WSWAN filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWswan_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(11);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(11);
            _App.GamesLibrary.FilterByFilterButton(11);
        }

        /// <summary>
        /// Called when the games library GB filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGb_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(12);

            // load datagrid
            _filterId = 1;
            GamesLibraryVisualHandler.LoadColumnInfo(12);
            _App.GamesLibrary.FilterByFilterButton(12);
        }

        /// <summary>
        /// Called when the games library GBA filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGba_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(13);

            // load datagrid
            GamesLibraryVisualHandler.LoadColumnInfo(13);
            _App.GamesLibrary.FilterByFilterButton(13);
        }

        /// <summary>
        /// Called when the games library GG filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGg_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(14);

            // load datagrid
            _filterId = 5;
            GamesLibraryVisualHandler.LoadColumnInfo(14);
            _App.GamesLibrary.FilterByFilterButton(14);
        }

        /// <summary>
        /// Called when the games library LYNX filter is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLynx_Checked(object sender, RoutedEventArgs e)
        {
            // set column visibility
            GamesLibraryVisualHandler.SetColumnVisibility(15);

            // load datagrid
            _filterId = 3;
            GamesLibraryVisualHandler.LoadColumnInfo(15);
            _App.GamesLibrary.FilterByFilterButton(15);
        }


        #endregion

        #region Games Library Context Menus

        // games list context menu
        private void dgGameList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //dgGameList.Items.Refresh();
            //dgGameList.InvalidateVisual();
            FrameworkElement fe = e.Source as FrameworkElement;
            ContextMenu cm = fe.ContextMenu;

            ContextMenu c = (ContextMenu)this.FindName("dgContext");

            // get selected row data
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
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
                    if (Game.GetFavoriteStatus(romId) == 1)
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
        }

        private void MenuItemFavorite_Click(object sender, RoutedEventArgs e)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            int romId = drv.ID;
            Game.FavoriteToggle(romId);
            // refresh library view
            //GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void MenuItemFavorites_Click(object sender, RoutedEventArgs e)
        {
            int numRows = dgGameList.SelectedItems.Count;

            if (numRows == 0)
                return;
            else if (numRows == 1)
            {
                GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
                int romId = drv.ID;
                Game.FavoriteToggle(romId);
            }
            else
            {
                var rs = dgGameList.SelectedItems;
                List<GamesLibraryModel> games = new List<GamesLibraryModel>();
                foreach (GamesLibraryModel row in rs)
                {
                    games.Add(row);
                }

                foreach (var game in games)
                {
                    Game.FavoriteToggle(game.ID);
                }
            }

            // refresh library view
            //GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void DeleteRomFromDisk_Click(object sender, RoutedEventArgs e)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            int romId = drv.ID;
            Game game = Game.GetGame(romId);

            var g = Game.DeleteGamesFromDisk(new List<Game> { game });

            if (g.Count() > 0)
            {
                // delete from library
                Game.DeleteGame(g.First());
            }

            // refresh library view
            //GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void DeleteRomsFromDisk_Click(object sender, RoutedEventArgs e)
        {
            int numRows = dgGameList.SelectedItems.Count;

            if (numRows == 0)
                return;
            else if (numRows == 1)
            {
                GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
                int romId = drv.ID;
                Game game = Game.GetGame(romId);
                var g = Game.DeleteGamesFromDisk(new List<Game> { game });
                if (g.Count() > 0)
                {
                    // delete from library
                    Game.DeleteGame(g.First());
                }
            }
            else
            {
                var rs = dgGameList.SelectedItems;
                List<GamesLibraryModel> rows = new List<GamesLibraryModel>();
                foreach (GamesLibraryModel row in rs)
                {
                    rows.Add(row);
                }

                List<Game> games = new List<Game>();

                foreach (GamesLibraryModel row in rows)
                {
                    int id = row.ID;
                    Game game = Game.GetGame(id);
                    games.Add(game);
                }
                var r = Game.DeleteGamesFromDisk(games);
                if (r.Count > 0)
                {
                    Game.DeleteGames(r);
                }
            }

            // refresh library view
            // GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void DeleteRom_Click(object sender, RoutedEventArgs e)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            int romId = drv.ID;
            Game game = Game.GetGame(romId);
            // delete from library
            Game.DeleteGame(game);


            // refresh library view
            //GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void DeleteRoms_Click(object sender, RoutedEventArgs e)
        {
            int numRows = dgGameList.SelectedItems.Count;

            if (numRows == 0)
                return;
            else if (numRows == 1)
            {
                GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
                int romId = drv.ID;
                Game game = Game.GetGame(romId);
                // delete from library
                Game.DeleteGame(game);
            }
            else
            {
                var rs = dgGameList.SelectedItems;
                List<GamesLibraryModel> rows = new List<GamesLibraryModel>();
                foreach (GamesLibraryModel row in rs)
                {
                    rows.Add(row);
                }

                List<Game> games = new List<Game>();

                foreach (GamesLibraryModel row in rows)
                {
                    int id = row.ID;
                    Game game = Game.GetGame(id);
                    games.Add(game);
                }

                Game.DeleteGames(games);
            }

            // refresh library view
            // GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void CopyLaunchStringToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            int romId = drv.ID;
            GameLauncher.CopyLaunchStringToClipboard(romId);
        }

        private async void LaunchRomShowConfig_Click(object sender, RoutedEventArgs e)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;


            bool b = VersionChecker.MednafenVersionCheck(true);

            if (b == false)
            {
                return;
            }


            // create new GameLauncher instance
            GameLauncher gl = new GameLauncher(romId);

            // get base config params
            string configCmdString = gl.GetCommandLineArguments();

            // set to launchstring variable
            LaunchString = configCmdString;

            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            await mw.ShowChildWindowAsync(new LaunchStringWindow()
            {
                IsModal = true,
                AllowMove = false,
                Title = "View / Modify Launch String",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        private void LaunchRom_Click(object sender, RoutedEventArgs e)
        {
            int numRowsCount = dgGameList.SelectedItems.Count;
            if (numRowsCount != 1)
                return;

            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;

            bool b = VersionChecker.MednafenVersionCheck(true);

            if (b == false)
            {
                return;
            }



            // create new GameLauncher instance
            GameLauncher gl = new GameLauncher(romId);
            LaunchString = gl.GetCommandLineArguments();
            LaunchRomHandler(LaunchString, false);
        }

        private async void LaunchRomChooseDisc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");

            await mw.ShowChildWindowAsync(new DiscSelection()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Choose Disc",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);

        }

        public async void LaunchRomHandler(string cmdlineargs, bool bypassSystemConfigs)
        {
            GamesLibraryModel drv = (GamesLibraryModel)dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;
            int systemId = Game.GetGame(romId).systemId;

            /*
            bool b = Versions.MednafenVersionCheck(true);

            if (b == false)
            {
                return;
            }
            */

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
                await Task.Delay(3000);
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

                    // rename system configs if neccesary (this can be removed if/when Ryphecha implements a custom config cmdline option)
                    if (bypassSystemConfigs == true)
                    {
                        GameLauncher.SystemConfigsOff();
                    }

                    // launch game                        
                    gl.RunGame(cmdlineargs, systemId);

                    // name back system configs if neccesary (this can be removed if/when Ryphecha implements a custom config cmdline option)
                    if (bypassSystemConfigs == true)
                    {
                        GameLauncher.SystemConfigsOn();
                    }


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

                    // refresh library view
                    //GamesLibraryVisualHandler.RefreshGamesLibrary();
                });
            }
            if (controller.IsOpen == true)
                await controller.CloseAsync();

            //GameListBuilder.UpdateFlag();
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

        #endregion

        #region Games Library Filter Context Menus

        // game filter context menu events
        private void ScanRoms_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = 0;
            if (menuName.StartsWith("ScanRoms"))
            {
                sysId = Convert.ToInt32(menuName.Replace("ScanRoms", ""));
            }
            if (menuName.StartsWith("MenuScanRoms"))
            {
                sysId = Convert.ToInt32(menuName.Replace("MenuScanRoms", ""));
            }

            RescanSystemRoms(sysId);
        }

        private void RemoveRoms_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("RemoveRoms", ""));
            Game.RemoveRoms(sysId);
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void ScanDisks_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ScanDisks", ""));

            RescanSystemDiscs(sysId);

            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void RemoveDisks_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("RemoveDisks", ""));
            Game.RemoveDisks(sysId);
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void ManualAddGame_Click(object sender, RoutedEventArgs e)
        {
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ManualAddGame", ""));
            DiscScan gs = new DiscScan();
            gs.BeginManualImport(sysId);
            // refresh library view
            GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void RemoveAllGames_Click(object sender, RoutedEventArgs e)
        {
            Game.RemoveAllGames();
        }

        private void ScrapeFavorites_Click(object sender, RoutedEventArgs e)
        {
            //ScraperHandler.ScrapeGames(0, ScrapeType.Favorites);
            ScraperHandler.ScrapeMultiple(null, ScrapeType.Favorites, 0);
        }

        private void ReScrapeFavorites_Click(object sender, RoutedEventArgs e)
        {
            ScraperHandler.ScrapeMultiple(null, ScrapeType.RescrapeFavorites, 0);
        }

        private void ScrapeGamesMultiple_Click(object sender, RoutedEventArgs e)
        {
            // get number of selected rows
            int numOfRows = dgGameList.SelectedItems.Count;
            List<GamesLibraryModel> games = new List<GamesLibraryModel>();

            if (numOfRows == 0)
                return;
            else if (numOfRows == 1)
            {
                // single row selected
                games.Add((GamesLibraryModel)dgGameList.SelectedItem);
            }
            else
            {
                // multiple rows selected
                var rows = dgGameList.SelectedItems;
                foreach (GamesLibraryModel r in rows)
                {
                    games.Add(r);
                }
            }

            // parse list of games to the method to be auto-scraped
            //ScraperHandler.ScrapeGamesMultiple(games, ScrapeType.Selected);    
            ScraperHandler.ScrapeMultiple(games, ScrapeType.Selected, 0);
        }

        private void ReScrapeGamesMultiple_Click(object sender, RoutedEventArgs e)
        {
            // get number of selected rows
            int numOfRows = dgGameList.SelectedItems.Count;
            List<GamesLibraryModel> games = new List<GamesLibraryModel>();

            if (numOfRows == 0)
                return;
            else if (numOfRows == 1)
            {
                // single row selected
                games.Add((GamesLibraryModel)dgGameList.SelectedItem);
            }
            else
            {
                // multiple rows selected
                var rows = dgGameList.SelectedItems;
                foreach (GamesLibraryModel r in rows)
                {
                    games.Add(r);
                }
            }

            // parse list of games to the method to be auto-scraped
            ScraperHandler.ScrapeMultiple(games, ScrapeType.SelectedRescrape, 0);
        }

        private void ScrapeGames_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("ScrapeGames", ""));
            ScraperHandler.ScrapeMultiple(null, ScrapeType.ScrapeSystem, sysId);
        }

        private void RescrapeGames_Click(object sender, RoutedEventArgs e)
        {
            // get systemId from menu name
            string menuName = (sender as MenuItem).Name;
            int sysId = Convert.ToInt32(menuName.Replace("RescrapeGames", ""));

            ScraperHandler.ScrapeMultiple(null, ScrapeType.RescrapeSystem, sysId);
        }

        #endregion

        #region Games Library Search & Filter

        private DispatcherTimer _searchTimer;

        private void OnSearchTimerTick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            Search(tbFilterDatagrid.Text);
        }

        private void Search(string searchText)
        {
            Task.Run(() =>
            {
                // Execute search

                Dispatcher.Invoke(() =>
                {
                    if (searchText == tbFilterDatagrid.Text)
                    {
                        // Result is good
                        _App.GamesLibrary.SearchFilter(tbFilterDatagrid.Text);
                    }
                });
            });
        }

        // Games grid filter text box event
        private void tbFilterDatagrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        // Clear all filters button click
        public void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            srcFilterALL.IsChecked = true;
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Clear();
            tbFilterDatagrid.Text = "";
        }

        private void dgGameList_Sorting(object sender, DataGridSortingEventArgs e)
        {
            var dgSender = (DataGrid)sender;
            var cView = CollectionViewSource.GetDefaultView(dgSender.ItemsSource);

            // get datagrid sortdescriptions


            //var cView = _App.GamesLibrary.LibraryView.View;

            //Alternate between ascending/descending if the same column is clicked 

            ListSortDirection direction = ListSortDirection.Ascending;
            if (cView.SortDescriptions.FirstOrDefault().PropertyName == e.Column.SortMemberPath)
                direction = cView.SortDescriptions.FirstOrDefault().Direction == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            cView.SortDescriptions.Clear();
            GamesLibrarySorting.AddSortColumn((DataGrid)sender, e.Column.SortMemberPath, direction);
            //To this point the default sort functionality is implemented

            //Now check the wanted columns and add multiple sort 
            if (e.Column.SortMemberPath == "WantedColumn")
            {
                GamesLibrarySorting.AddSortColumn((DataGrid)sender, "SecondColumn", direction);
            }
            e.Handled = true;


            List<SortDescription> sds = _App.GamesLibrary.LibraryView.SortDescriptions.ToList();
        }



        private void dgGameList_Loaded(object sender, RoutedEventArgs e)
        {
            // SortDirectionHelper(sender as DataGrid);
        }

        public void SortDirectionHelper(DataGrid dg)
        {
            var tmp = dg.Items.SortDescriptions;

            foreach (SortDescription sd in tmp)
            {
                var col = dg.Columns.Where(x => (((Binding)x.ClipboardContentBinding).Path.Path == sd.PropertyName)).FirstOrDefault();
                if (col != null)
                {
                    col.SortDirection = sd.Direction;
                }
            }
        }

        #endregion

        #region Games Library DataGrid

        // Games Datagrid selection changed
        private void dgGameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // choose context menu to show based on single or multiple selection
            var dg = sender as DataGrid;

            if (dg.SelectedIndex > -1)
                GamesLibraryView.StoreSelectedRow(dg);

            ContextMenu menuToUse;
            int numRowsSelected = dgGameList.SelectedItems.Count;
            var rs = dgGameList.SelectedItems;
            List<GamesLibraryModel> rows = new List<GamesLibraryModel>();

            if (numRowsSelected != 0)
            {
                foreach (GamesLibraryModel r in rs)
                {
                    rows.Add(r);
                }
            }

            if (numRowsSelected == 0)
            {
                // nothing select - refresh sidebar
                GamesLibraryVisualHandler.UpdateSidebar();
            }
            else if (numRowsSelected == 1)
            {
                // update right hand pane with game details
                GamesLibraryVisualHandler.UpdateSidebar(rows.First().ID);
                // set single context menu

                // detect whether game uses m3u
                if (rows.Count == 0)
                    return;
                Game gm = Game.GetGame(rows.First().ID);
                if (gm == null)
                {
                    return;
                }
                string fileName = gm.gamePath;
                if (fileName.ToLower().EndsWith(".m3u"))
                {
                    // potentially a multi-disc game (uses an m3u file)
                    menuToUse = (ContextMenu)this.Resources["cmMultiDiscGamesListSingle"];
                    dg.ContextMenu = menuToUse;
                }
                else
                {
                    // not a multi-disc game
                    menuToUse = (ContextMenu)this.Resources["cmGamesListSingle"];
                    dg.ContextMenu = menuToUse;
                }
            }
            else
            {
                // multiple rows are selected - just take the first one to update sidebar
                GamesLibraryVisualHandler.UpdateSidebar(rows.First().ID);
                // set multiple context menu
                menuToUse = (ContextMenu)this.Resources["cmGamesListMultiple"];
                dg.ContextMenu = menuToUse;
            }

        }

        private void DataGrid_PreviewMouseRightButtonDownBlock(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("Preview MouseRightButtonDown");

            e.Handled = true;
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

        #endregion

        #region Games Library Sidebar

        /// <summary>
        /// fired when the games library sidebar gridsplitter has finished being dragged
        /// the width value should be then saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gamesSidebarGS_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            // set current column width
            double width = sidebarColumn.ActualWidth;
            GamesSidebarWidth = width;
            GlobalSettings.SetSidebarWidth(width);
        }

        private void btnBrowseDataFolder_Click(object sender, RoutedEventArgs e)
        {
            var r = (GamesLibraryModel)dgGameList.SelectedItem;
            // get the gamesdbid

            int gdbId = 0;

            Game ga = Game.GetGame(Convert.ToInt32(r.ID));
            if (ga.gdbId == null || ga.gdbId == 0)
                return;
            else
            {
                gdbId = ga.gdbId.Value;
            }

            if (gdbId > 0)
            {
                // open the folder in windows explorer
                string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Games\" + gdbId;
                // check folder exists
                if (Directory.Exists(dirPath))
                {
                    // open the folder
                    Process.Start(dirPath);
                }
            }
            GamesLibraryView.RestoreSelectedRow();
        }

        private void btnOpenManual_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            string path = btn.ToolTip.ToString();
            Process.Start(path);
        }

        // save the layout of all the games library expander states
        private void GamesLibraryExpanderSaveLayout(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("expander triggered");
            GamesLibraryVisualHandler.SaveExpanderStates();
        }

        #endregion

        #region Games Library ROM Examination

        private async void examineRom_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");

            var h = RootGrid.ActualHeight; // mw.ActualHeight; // RootGrid.ActualHeight;
            var w = RootGrid.ActualWidth; // mw.ActualWidth; // RootGrid.ActualWidth;

            mw.ShowMaxRestoreButton = false;

            await mw.ShowChildWindowAsync(new RomInspector()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Library Editor/Inspector",
                CloseOnOverlay = false,
                ShowCloseButton = true,
                ChildWindowHeight = h,
                ChildWindowWidth = w
            }, RootGrid);

            mw.ShowMaxRestoreButton = true;
        }

        #endregion

        #region Possibly Obsolete

        private void columnHeaderDGGAMES_Click(object sender, RoutedEventArgs e)
        {
            /*
            var columnHeader = sender as DataGridColumnHeader;
            
            if (columnHeader != null)
            {
                string columnName = ((DataGridColumnHeader)sender).Content.ToString();

                App _App = ((App)Application.Current);

                ListSortDirection sortDirection = (((DataGridColumnHeader)sender).SortDirection != ListSortDirection.Ascending) ?
                                ListSortDirection.Ascending : ListSortDirection.Descending;
                _App.GamesList.SortColumnName = columnName;
                _App.GamesList.SortDirection = sortDirection;
            }
            */
        }

        private void TabItem_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tiSettings_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        // Misc
        private void System_Image_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSavePlatformGamesToDisk_Click(object sender, RoutedEventArgs e)
        {
            /*
            string linkTimeLocal = (Assembly.GetExecutingAssembly().GetLinkerTime()).ToString("yyyy-MM-dd HH:mm:ss");
            App app = ((App)Application.Current);
            var platformgames = app.ScrapedData.AllScrapeData;

            string json = JsonConvert.SerializeObject(platformgames.ToArray());
            System.IO.File.WriteAllText(@"Data\Settings\thegamesdbplatformgames_" + linkTimeLocal.Replace(" ", "").Replace(":", "").Replace("-", "") + ".json", json);
            */
        }

        // unlink selected game
        private void btnScrapingUnlinkGame_Click(object sender, RoutedEventArgs e)
        {
            ScraperHandler.UnlinkGameData(dgGameList);
            //GamesLibraryView.RestoreCurrentItem();
            GamesLibraryView.RestoreSelectedRow();

            //GamesLibraryView.SelectRowByIndex(dgGameList, 10);
            //_App.GamesLibrary.DataGridFocused = true;
            //GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        private void btnTestGameSearch_Click(object sender, RoutedEventArgs e)
        {
            /*
            GameScraper gs = new GameScraper();
            Game game = Game.GetGame(20);

            List<Game> games = Game.GetGames(7);

            foreach (Game g in games)
            {
                List<ScraperMaster> result = gs.SearchGameLocal(g.gameName, g.systemId, g.gameId).ToList();
                string glist = g.gameName + "\n-------------------------\n\n";
                foreach (ScraperMaster bg in result)
                {
                    glist += bg.TGDBData.GamesDBTitle + "\n";
                }
                MessageBox.Show(glist);
            }
            */

            //List<GDBPlatformGame> result = gs.SearchGameLocal(game.gameName, game.systemId, game.gameId).ToList();
        }

        #endregion

        #region Settings - Global

        private void btnForgetSystemPosition_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ResetAllSysWindowPositions();
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

        private void chkrememberSysWinPositions_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateRememberSysWinPositions(chkrememberSysWinPositions);
        }

        private void chkrememberSysWinPositions_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateRememberSysWinPositions(chkrememberSysWinPositions);
        }

        /*
        private void chkAllBaseSettings_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatechkAllBaseSettings(chkAllBaseSettings);
            ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, 2000000000);
            btnConfigLynx.IsChecked = true;
            btnConfigBase.IsChecked = true;
            //btnConfigBase.IsChecked = false;
            //btnConfigBase.IsChecked = true;
        }

        private void chkAllBaseSettings_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatechkAllBaseSettings(chkAllBaseSettings);
            ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, 2000000000);
            btnConfigLynx.IsChecked = true;
            btnConfigBase.IsChecked = true;
            //btnConfigBase.IsChecked = false;
            //btnConfigBase.IsChecked = true;
        }
        */

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

        private void chkHideCountryFilter_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateHideCountryFilters(chkHideCountryFilter);
            CountryFilterBrd.Visibility = Visibility.Collapsed;
        }

        private void chkHideCountryFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateHideCountryFilters(chkHideCountryFilter);
            CountryFilterBrd.Visibility = Visibility.Visible;
        }

        private void chkshowGLCoop_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLCoop(chkshowGLCoop);
        }

        private void chkshowGLCoop_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLCoop(chkshowGLCoop);
        }

        private void chkshowGLDeveloper_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLDeveloper(chkshowGLDeveloper);
        }

        private void chkshowGLDeveloper_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLDeveloper(chkshowGLDeveloper);
        }

        private void chkshowGLESRB_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLESRB(chkshowGLESRB);
        }

        private void chkshowGLESRB_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLESRB(chkshowGLESRB);
        }

        private void chkshowGLPlayers_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLPlayers(chkshowGLPlayers);
        }

        private void chkshowGLPlayers_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLPlayers(chkshowGLPlayers);
        }

        private void chkshowGLPublisher_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLPublisher(chkshowGLPublisher);
        }

        private void chkshowGLPublisher_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLPublisher(chkshowGLPublisher);
        }

        private void chkshowGLYear_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLYear(chkshowGLYear);
        }

        private void chkshowGLYear_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateshowGLYear(chkshowGLYear);
        }


        private void chkPreferGenesis_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatePreferGenesis(chkPreferGenesis);
        }

        private void chkPreferGenesis_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdatePreferGenesis(chkPreferGenesis);
        }

        private void chkAllowUpdateCheck_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateCheckUpdatesOnStart(chkAllowUpdateCheck);
        }

        private void chkAllowUpdateCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateCheckUpdatesOnStart(chkAllowUpdateCheck);
        }

        private void chkBackupMednafenConfig_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateBackupMednafenConfig(chkBackupMednafenConfig);
        }

        private void chkBackupMednafenConfig_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateBackupMednafenConfig(chkBackupMednafenConfig);
        }

        private void chkSaveSysConfigs_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateSaveSysConfigs(chkSaveSysConfigs);
        }

        private void chkSaveSysConfigs_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateSaveSysConfigs(chkSaveSysConfigs);
        }

        private void chkLoadConfigsOnStart_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateImportConfigsOnStart(chkLoadConfigsOnStart);
        }

        private void chkLoadConfigsOnStart_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateImportConfigsOnStart(chkLoadConfigsOnStart);
        }

        private void chkEnableConfigToolTips_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableConfigToolTips(chkEnableConfigToolTips);
        }

        private void chkEnableConfigToolTips_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableConfigToolTips(chkEnableConfigToolTips);
        }

        private void chkEnableClearCacheOnExit_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableClearCacheOnExit(chkEnableClearCacheOnExit);
        }

        private void chkEnableClearCacheOnExit_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateEnableClearCacheOnExit(chkEnableClearCacheOnExit);
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

        private void chkAllowMedia_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowMedias(chkAllowMedia);
        }

        private void chkAllowMedia_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowMedias(chkAllowMedia);
        }

        private void chkAllowManuals_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowManuals(chkAllowManuals);
        }

        private void chkAllowManuals_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateAllowManuals(chkAllowManuals);
        }

        private void chkSecondaryScraperBackup_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateScraperBackup(chkSecondaryScraperBackup);
        }

        private void chkSecondaryScraperBackup_Unchecked(object sender, RoutedEventArgs e)
        {
            GlobalSettings.UpdateScraperBackup(chkSecondaryScraperBackup);
        }

        private void rbGDB_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings g = GlobalSettings.GetGlobals();
            g.primaryScraper = 1;
            GlobalSettings.SetGlobals(g);
        }

        private void rbMoby_Checked(object sender, RoutedEventArgs e)
        {
            GlobalSettings g = GlobalSettings.GetGlobals();
            g.primaryScraper = 2;
            GlobalSettings.SetGlobals(g);
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
                Paths.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd);
                ConfigNetplaySettings.SaveNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
                ConfigServerSettings.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
                //ConfigBaseSettings.SaveMednafenPathValues(spMedPathSettings);
                ConfigBaseSettings.SaveMednafenPaths();
                //ConfigBaseSettings.SaveBiosPathValues(spSysBiosSettings);
                ConfigBaseSettings.SaveBiosPaths();

                // global settings
                GlobalSettings gs = GlobalSettings.GetGlobals();
                gs.maxFanarts = slFanrtsPerHost.Value;
                gs.maxScreenshots = slScreenshotsPerHost.Value;
                gs.imageToolTipPercentage = Convert.ToDouble(comboImageTooltipSize.SelectedValue, System.Globalization.CultureInfo.InvariantCulture);
                GlobalSettings.SetGlobals(gs);
            });

            await controller.CloseAsync();


        }

        private void btnSettingsCancelAllChanges_Click(object sender, RoutedEventArgs e)
        {
            //SettingsHandler sh = new SettingsHandler();
            //sh.LoadAllSettings();

            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd);
            ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            //ConfigServerSettings.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
            ConfigBaseSettings.LoadMednafenPathValues(spMedPathSettings);
            ConfigBaseSettings.LoadBiosPathValues(spSysBiosSettings);

        }

        private void tbSettings_Paths_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.GamePaths);
        }

        private void tbSettings_Global_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.GlobalSettings);
        }

        private void tbSettings_MednaNet_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.MednaNetSettings);
        }

        private void tbSettings_BiosPaths_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.BiosPaths);
        }

        private void comboSettings_Global_DropDownClosed(object sender, EventArgs e)
        {
            if (SettingsDirtyFlag == true)
            {
                ConfigsVisualHandler.SaveSettings(SettingGroup.GlobalSettings);
                var cbs = sender as ComboBox;
                if (cbs.Name == "cbFormatGameTitles")
                {
                    CaseConvert = Convert.ToInt32(cbs.SelectedValue);
                }
            }

        }

        private void slSettings_Global_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.GlobalSettings);
        }

        private void tbSettings_MednafenPaths_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.MednafenPaths);
        }

        private void tbSettings_Netplay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.NetplaySettings);
        }

        private void tbSettings_Server_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.ServerSettings);
        }

        private void slSettings_Server_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.ServerSettings);
        }

        private void slSettings_Netplay_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.NetplaySettings);
        }

        private void rbSettings_Netplay_Checked(object sender, RoutedEventArgs e)
        {
            if (SettingsDirtyFlag == true)
                ConfigsVisualHandler.SaveSettings(SettingGroup.NetplaySettings);
        }

        #endregion

        #region Settings - Netplay

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

        #endregion

        #region Settings - Paths

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
                    tbName = "cfg_pce__gecdbios";
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
            //System.Windows.Forms.FolderBrowserDialog path = new System.Windows.Forms.FolderBrowserDialog();

            string origPath = tbPathMednafen.Text;

            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;

            path.Description = "Select Mednafen Directory";
            path.ShowDialog();

            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;

                if (strPath != origPath)
                {
                    // there has been a path change. Set the dirty flag on the log parser
                    LogParser.Instance.IsDirty = true;
                }

                tbPathMednafen.Text = strPath;
                Paths.SaveMednafenPath(strPath);

                // re-init logparser instance
                LogParser.Init();

                // re-init input
                Input.Initialize(this);

                UpdateCheckMednafen();
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
                tbPathSms.Text = strPath;
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

        private void btnPathPceCd_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PC Engine (CD) Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPceCd.Text = strPath;
            }
        }

        // Path page save & restore

        private void btnPathsSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Paths.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd);
            //lblPathsSettingsSave.Content = "***Path Settings Saved***";
        }

        private void btnPathsCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            Paths.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd);
            //lblPathsSettingsSave.Content = "***Path Settings Reverted***";
        }

        #endregion

        #region Settings = Servers

        private void btnServerSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ConfigServerSettings.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
        }

        private void lvServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (ServersListView)lvServers.SelectedItem;

            if (row == null)
            {
                btnServersSelect.Visibility = Visibility.Collapsed;
                btnServersDelete.Visibility = Visibility.Collapsed;
                btnServersSaveEdit.Visibility = Visibility.Collapsed;

                tbServerDesc.Text = null;
                tbHostname.Text = null;
                slServerPort.Value = 4046;
                tbPassword.Text = null;
                tbGameKey.Text = null;
            }
            else
            {
                btnServersSelect.Visibility = Visibility.Visible;
                btnServersDelete.Visibility = Visibility.Visible;
                btnServersSaveEdit.Visibility = Visibility.Visible;

                tbServerDesc.Text = row.Name;
                tbHostname.Text = row.Host;
                tbPassword.Text = row.Password;
                tbGameKey.Text = row.Gamekey;
                if (row.Port > 0)
                {
                    slServerPort.Value = Convert.ToDouble(row.Port, System.Globalization.CultureInfo.InvariantCulture);
                }

            }
        }

        private void btnServersSelect_Click(object sender, RoutedEventArgs e)
        {
            var row = (ServersListView)lvServers.SelectedItem;

            if (row == null)
            {
                MessagePopper.ShowMessageDialog("No Server Selected!", "ERROR");
                //MessageBox.Show("No Server Selected!");
            }
            else
            {
                GlobalSettings gs = GlobalSettings.GetGlobals();
                gs.serverSelected = row.ID;
                GlobalSettings.SetGlobals(gs);

                SettingsVisualHandler.PopulateServers(lvServers);
            }
        }

        private void btnServersAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbHostname.Text == null || tbHostname.Text == "" || tbHostname.Text.Trim() == "")
            {
                // hostname has not been entered
                MessagePopper.ShowMessageDialog("You must provide a Hostname or IP Address", "ERROR");
                //MessageBox.Show("You must provide a Hostname or IP Address");
                return;
            }

            // get the server list
            var dbServers = ConfigServerSettings.GetServers();

            // build a server object
            ConfigServerSettings s = new ConfigServerSettings();

            // check for full match
            var chk = from a in dbServers
                      where
                      a.ConfigServerDesc == tbServerDesc.Text &&
                      a.netplay__gamekey == tbGameKey.Text &&
                      a.netplay__host == tbHostname.Text &&
                      a.netplay__password == tbPassword.Text &&
                      a.netplay__port == Convert.ToInt32(slServerPort.Value)
                      select a;
            if (chk.Count() > 1)
            {
                MessagePopper.ShowMessageDialog("This server and associated settings already exists!", "ERROR");
                //MessageBox.Show("This server and associated settings already exists!");
                return;
            }

            // save the server
            s.ConfigServerDesc = tbServerDesc.Text;
            s.netplay__gamekey = tbGameKey.Text;
            s.netplay__host = tbHostname.Text;
            s.netplay__password = tbPassword.Text;
            s.netplay__port = Convert.ToInt32(slServerPort.Value);

            ConfigServerSettings.SaveToDatabase(s);

            SettingsVisualHandler.PopulateServers(lvServers);
        }

        private void btnServersDelete_Click(object sender, RoutedEventArgs e)
        {
            var row = (ServersListView)lvServers.SelectedItem;

            if (row == null)
            {
                MessagePopper.ShowMessageDialog("No Server Selected!", "ERROR");
                //MessageBox.Show("No Server Selected!");
                return;
            }

            if (row.Selected == true)
            {
                MessagePopper.ShowMessageDialog("Unable to delete because this is the current default server.\nSet another server to default (the 'use selected server button') and then try again", 
                    "ERROR");
                //MessageBox.Show("Unable to delete because this is the current default server.\nSet another server to default (the 'use selected server button') and then try again");
                return;
            }

            // obtain ID and delete selected server from the database
            ConfigServerSettings.DeleteServer(row.ID);
            SettingsVisualHandler.PopulateServers(lvServers);
        }

        private void btnServersSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            // get the selected ID
            var row = (ServersListView)lvServers.SelectedItem;

            if (row == null)
            {
                MessagePopper.ShowMessageDialog("No Server Selected!", "ERROR");
                //MessageBox.Show("No Server Selected!");
                return;
            }

            int id = row.ID;

            // create a new object
            ConfigServerSettings s = new ConfigServerSettings();

            s.ConfigServerId = id;
            s.ConfigServerDesc = tbServerDesc.Text;
            s.netplay__gamekey = tbGameKey.Text;
            s.netplay__host = tbHostname.Text;
            s.netplay__password = tbPassword.Text;
            s.netplay__port = Convert.ToInt32(slServerPort.Value);

            // update the record in the database
            ConfigServerSettings.SaveToDatabase(s);

            SettingsVisualHandler.PopulateServers(lvServers);
        }

        #endregion

        #region Controls

        private void btnControlRePoll_Click(object sender, RoutedEventArgs e)
        {
            Input.Instance.Dispose();
            Input.Initialize(this);
            //Input.Instance.Dispose();
        }

        /// <summary>
        /// Set non-system specific command bindings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnControlCommandBindings_Click(object sender, RoutedEventArgs e)
        {
            // launch controller configuration window

            if (VersionChecker.Instance.IsNewConfig)
            {
                await this.ShowChildWindowAsync(new ConfigureMiscBindings()
                {
                    IsModal = true,
                    AllowMove = false,
                    Title = "Misc Binding Configuration",
                    CloseOnOverlay = false,
                    CloseByEscape = false,
                    ShowCloseButton = false
                }, RootGrid);
            }
            else
            {
                await this.ShowChildWindowAsync(new ConfigureMiscBindingsLegacy()
                {
                    IsModal = true,
                    AllowMove = false,
                    Title = "Misc Binding Configuration",
                    CloseOnOverlay = false,
                    CloseByEscape = false,
                    ShowCloseButton = false
                }, RootGrid);
            }
            
        }

        // generic controls selections buttons
        private void btnControl_Checked(object sender, RoutedEventArgs e)
        {
            //ControlsVisualHandler.ButtonClick();

            RadioButton rb = sender as RadioButton;
            string rbName = rb.Name.Replace("btnControl", "").ToLower();

            // get all the filter buttons
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpControlLeftPane);
            RadioButton[] arr = (from a in _filterButtons
                                 where a.Name != "tccControls"
                                 select a).ToArray();

            // hide all
            for (int i = 0; i < arr.Length; i++)
            {
                TransitioningContentControl t = (TransitioningContentControl)this.FindName("tccControlsDyn" + (i + 1).ToString());
                t.Content = null;
            }


            switch (rbName)
            {
                case "nes":
                    tccControls.Content = new NesCtrl();
                    break;
                case "gb":
                    tccControls.Content = new GbCtrl();
                    break;
                case "gba":
                    tccControls.Content = new GbaCtrl();
                    break;
                case "gg":
                    tccControls.Content = new GgCtrl();
                    break;
                case "lynx":
                    tccControls.Content = new LynxCtrl();
                    break;
                case "ngp":
                    tccControls.Content = new NgpCtrl();
                    break;
                case "md":
                    tccControls.Content = new MdCtrl();
                    break;
                case "snes":
                    tccControls.Content = new SnesCtrl();
                    break;
                case "snes_faust":
                    tccControls.Content = new Snes_FaustCtrl();
                    break;
                case "sms":
                    tccControls.Content = new SmsCtrl();
                    break;
                case "pce":
                    tccControls.Content = new PceCtrl();
                    break;
                case "pce_fast":
                    tccControls.Content = new Pce_FastCtrl();
                    break;
                case "vb":
                    tccControls.Content = new VbCtrl();
                    break;
                case "wswan":
                    tccControls.Content = new WswanCtrl();
                    break;
                case "pcfx":
                    tccControls.Content = new PcfxCtrl();
                    break;
                case "ss":
                    tccControls.Content = new SsCtrl();
                    break;
                case "psx":
                    tccControls.Content = new PsxCtrl();
                    break;
                default:
                    // show all usercontrols

                    // null the static one
                    tccControls.Content = null;


                    // loop through and display each one
                    for (int i = 0; i < arr.Length; i++)
                    {
                        TransitioningContentControl t = (TransitioningContentControl)this.FindName("tccControlsDyn" + (i + 1).ToString());
                        switch (i + 1)
                        {
                            case 1:         // nes
                                t.Content = new NesCtrl();
                                break;
                            case 2:         // snes
                                t.Content = new SnesCtrl();
                                break;
                            case 3:         // snesfaust
                                t.Content = new Snes_FaustCtrl();
                                break;
                            case 4:         // sms
                                t.Content = new SmsCtrl();
                                break;
                            case 5:         // md
                                t.Content = new MdCtrl();
                                break;
                            case 6:         // pce
                                t.Content = new PceCtrl();
                                break;
                            case 7:         // pcefast
                                t.Content = new Pce_FastCtrl();
                                break;
                            case 8:         // vb
                                t.Content = new VbCtrl();
                                break;
                            case 9:         // npg
                                t.Content = new NgpCtrl();
                                break;
                            case 10:         // wswan
                                t.Content = new WswanCtrl();
                                break;
                            case 11:         // gb
                                t.Content = new GbCtrl();
                                break;
                            case 12:         // gba
                                t.Content = new GbaCtrl();
                                break;
                            case 13:         // gg
                                t.Content = new GgCtrl();
                                break;
                            case 14:         // lynx
                                t.Content = new LynxCtrl();
                                break;
                            case 15:         // ss
                                t.Content = new SsCtrl();
                                break;
                            case 16:         // psx
                                t.Content = new PsxCtrl();
                                break;
                            case 17:         // pcfx
                                t.Content = new PcfxCtrl();
                                break;

                        }
                    }

                    break;
            }
        }

        private void btnControl_UnChecked(object sender, RoutedEventArgs e)
        {

        }

        private void Controller_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the combobox
            ComboBox cb = sender as ComboBox;
            string cbName = cb.Name;
            string selected = (e.AddedItems[0] as ComboBoxItem).Content.ToString();

            // now parse based on system
            string sysStr = cbName.Replace("cmb", "");

            // nes standard gamepad
            if (sysStr == "Nes1")
            {

            }
        }

        private async void btnControlsConfigure_Click(object sender, RoutedEventArgs e)
        {
            // get button name
            Button button = (Button)sender;
            string name = button.Name;

            // remove beginning and end
            name = name.Replace("btn", "").Replace("Configure", "");

            // get the relevant combox
            ComboBox cb = (ComboBox)this.FindName("cmb" + name);

            // get the virtual port number
            //ComboBoxItem typeItem = (ComboBoxItem)cb.SelectedItem;
            string selectedString = cb.SelectionBoxItem.ToString();
            int portNum = Convert.ToInt32(selectedString.Replace("Virtual Port ", ""));

            // Get device definition for this controller
            DeviceDefinition dev = Nes.GamePad(portNum);
            ControllerDefinition = dev;

            // launch controller configuration window
            await this.ShowChildWindowAsync(new ConfigureController()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Controller Configuration",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        #endregion

        #region Config

        private void cfg_psx_shared_memcards_Checked(object sender, RoutedEventArgs e)
        {
            var res = MessagePopper.ShowMessageDialog("Only use this option if you understand the risks.\nMednafen writes and reads memcard data during Save/Load state operations. This can cause memory card save corruption in some cases",
                "WARNING", MessagePopper.DialogButtonOptions.YESNO);

            //MessageBoxResult res = MessageBox.Show("Only use this option if you understand the risks.\nMednafen writes and reads memcard data during Save/Load state operations. This can cause memory card save corruption in some cases", "WARNING!",
                //MessageBoxButton.OKCancel);
            if (res == MessagePopper.ReturnResult.Negative || res == MessagePopper.ReturnResult.FirstAux)
            {
                cfg_psx_shared_memcards.IsChecked = false;
            }
        }

        // Generic Config Selection Buttons CHECKED
        private void btnConfig_Checked(object sender, RoutedEventArgs e)
        {
            // get selected ConfigId
            var rb = sender as RadioButton;
            int systemIdSelected = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
            // Save all control values to database
            ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, systemIdSelected);
            ConfigsVisualHandler.HideControls(ConfigWrapPanel, systemIdSelected);
            ConfigsVisualHandler.ButtonClick();

            // populate the psx combo fields
            string hexCode = cfg_psx__input__analog_mode_ct__compare.Text;
            CalculateComboChecks(hexCode);

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

        /// <summary>
        /// Import all configs from disk into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConfigImportAll_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Import",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await this.ShowProgressAsync("Config Importer", "Importing all configs from disk...", true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            this.Dispatcher.Invoke(() =>
            {
                // get current active button
                ConfigsVisualHandler ch = new ConfigsVisualHandler();
                RadioButton rb = ch.FilterButtons.Where(a => a.IsChecked == true).Single();
                int ConfigId = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);

                ConfigImport ci = new ConfigImport();
                ci.ImportAll(controller);


                // update UI
                ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, ConfigId);
                ConfigNetplaySettings.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
                //ConfigServerSettings.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
                //ConfigServerSettings.SetCustomDefault();

                SettingsVisualHandler.PopulateServers(lvServers);

                Task.Delay(500);

                if (rb != btnConfigLynx)
                    btnConfigLynx.IsChecked = true;
                else
                    btnConfigMd.IsChecked = true;

                Task.Delay(500);
                rb.IsChecked = true;
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("Config Importer", "Config Import Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("Config Importer", "Config Import Completed");
            }

            lblConfigStatus.Content = "All Configs Imported";
        }


        private void LoadConfigFromDisk_Click(object sender, RoutedEventArgs e)
        {

            MenuItem mi = sender as MenuItem;
            RadioButton rb = null;

            if (mi != null)
            {
                rb = ((ContextMenu)mi.Parent).PlacementTarget as RadioButton;
                rb.IsChecked = true;
                int ConfigId = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
                string sysCode = rb.Name.Replace("btnConfig", "").ToLower();

                ConfigImport ci = new ConfigImport();
                ci._ConfigBaseSettings = ConfigBaseSettings.GetConfig(ConfigId);
                ci.ImportSystemConfigFromDisk(null, GSystem.GetSystems().Where(a => a.systemCode == sysCode).FirstOrDefault());

                ci.SaveToDatabase();

                // update UI
                ConfigBaseSettings.LoadControlValues(ConfigWrapPanel, ConfigId);

                Task.Delay(500);

                if (rb != btnConfigLynx)
                    btnConfigLynx.IsChecked = true;
                else
                    btnConfigMd.IsChecked = true;

                Task.Delay(500);
                rb.IsChecked = true;

                // activate enabled systems
                ConfigsVisualHandler cvh = new ConfigsVisualHandler();
                cvh.ActivateEnabledSystems();

                lblConfigStatus.Content = sysCode.ToUpper() + " Config Imported";

            }

        }

        private void SaveConfigToDisk_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            int ConfigId = ConfigBaseSettings.GetConfigIdFromButtonName(rb.Name);
            string sysCode = rb.Name.Replace("btnConfig", "");
        }

        private void comboPsxRecalculate_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> chks = new List<CheckBox>
            {
                comboPsx1,
                comboPsx2,
                comboPsx3,
                comboPsx4,
                comboPsx5,
                comboPsx6,
                comboPsx7,
                comboPsx8,
                comboPsx9,
                comboPsx10,
                comboPsx11,
                comboPsx12,
                comboPsx13,
                comboPsx14,
                comboPsx15,
                comboPsx16
            };

            // get all checked checkboxes
            var chked = (from a in chks
                         where a.IsChecked == true
                         select a).ToList();

            string hex = CalculateCombo(chked);

            // set text box 
            cfg_psx__input__analog_mode_ct__compare.Text = hex;
        }

        public void CalculateComboChecks(string hexValue)
        {
            // get list of checkboxes
            List<CheckBox> chks = new List<CheckBox>
            {
                comboPsx1,
                comboPsx2,
                comboPsx3,
                comboPsx4,
                comboPsx5,
                comboPsx6,
                comboPsx7,
                comboPsx8,
                comboPsx9,
                comboPsx10,
                comboPsx11,
                comboPsx12,
                comboPsx13,
                comboPsx14,
                comboPsx15,
                comboPsx16
            };

            int value = int.Parse(hexValue.TrimStart('0').TrimStart('x'), System.Globalization.NumberStyles.HexNumber);
            for (int i = 16; i > 0; i--)
            {
                int testValue = 1 * Convert.ToInt32(Math.Pow(2, Convert.ToDouble(i - 1)));
                if (value / testValue == 1)
                {
                    // checkbox that ends with 'i' needs to be ticked - 
                    CheckBox cb = chks.Where(a => a.Name.Contains(i.ToString())).First();
                    cb.IsChecked = true;

                    //now set remainder
                    value = value % testValue;
                }
            }
        }


        public static string CalculateCombo(List<CheckBox> chkboxes)
        {
            int total = 0;

            foreach (var c in chkboxes)
            {
                string name = c.Name.Replace("comboPsx", "");

                int value = Convert.ToInt32(name);

                int calc = 1;
                for (int i = 1; i <= value; i++)
                {
                    if (i == 1)
                    {
                        calc = 1;
                        continue;
                    }

                    calc = calc * 2;
                }

                total += calc;
            }

            // convert to hex string
            string hexValue = "0x" + total.ToString("X4");
            return hexValue;

        }

        #endregion

        #region DependencyObject and UI Related

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

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        

        #endregion

        #region Software Update

        /// <summary>
        /// Public instance of the mahapps progressdialogcontroller and related properties
        /// </summary>
        public ProgressDialogController PDC { get; set; }
        public string DialogMessage { get; set; }
        public bool DownloadComplete { get; set; }

        private async void UpdateCheck(bool isStartup)
        {
            Release newRelease = new Release();

            // get current medlaunch version
            string currVersion = VersionChecker.ReturnApplicationVersion();

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Update Check",
                AnimateShow = true,
                AnimateHide = true

            };

            var controller = await this.ShowProgressAsync("Checking for Updates", "Connecting to Github", settings: mySettings);
            controller.SetCancelable(false);
            controller.SetIndeterminate();

            await Task.Delay(400);

            string output;

            // attempt to download the LatestVersion text file from github
            string contents;

            

            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 2000;
                string userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)";
                wc.Headers.Add("user-agent", userAgent);
                try
                {
                    //contents = wc.DownloadString("https://raw.githubusercontent.com/Asnivor/MedLaunch/master/MedLaunch/LatestVersion.txt");
                    var c = wc.DownloadString("https://api.github.com/repos/Asnivor/MedLaunch/releases/latest");
                    contents = c;
                }
                catch
                {
                    controller.SetMessage("The request timed out - please try again");
                    await Task.Delay(2000);
                    await controller.CloseAsync();
                    wc.Dispose();
                    return;
                }
                finally
                {
                    wc.Dispose();
                }

            }

            controller.SetMessage("Determining latest version...");
            // dynamically parse the github json latest release info
            dynamic d = JObject.Parse(contents);

            // check whether the version is greater than the one we have installed
            string latestVersion = d.tag_name;
            // compare versions and determine whether an upgrade is needed
            string[] CurrVersionArr = currVersion.Split('.');
            string[] newVersionArr = latestVersion.Split('.');
            bool upgradeNeeded = false;

            for (int v = 0; v < 4; v++)
            {
                int currV = Convert.ToInt32(CurrVersionArr[v]);
                int currN = Convert.ToInt32(newVersionArr[v]);

                if (currV > currN)
                {
                    // current version is NEWER than new version - upgrade not needed - break
                    break;
                }
                if (currV == currN)
                {
                    // versions are the same - continue checking
                    continue;
                }
                if (currV < currN)
                {
                    // new version is greater than old for this octet - upgrade needed
                    upgradeNeeded = true;
                    break;
                }

            }

            if (upgradeNeeded == true)
            {
                output = "A New MedLaunch Release is Now Available";
                UpdateStatusML = true;
                ChangeUpdateStatus();
                //await Task.Delay(1000);
                controller.SetMessage("Downloading release information");
                await Task.Delay(500);
                string releaseInfo;

                // get release info from JSON
                releaseInfo = d.body;


            }
            else
            {
                output = "Your Version of MedLaunch is up to date";
                UpdateStatusML = false;
                ChangeUpdateStatus();
                //UpdatedHeader.Header = "Updates";
            }
            controller.SetMessage(output);
            if (isStartup == false)
                await Task.Delay(1000);

            await controller.CloseAsync();

            // update the UI if needed
            if (upgradeNeeded == true)
            {
                lbl1.Visibility = Visibility.Visible;
                lbl2.Visibility = Visibility.Visible;
                lbl3.Visibility = Visibility.Visible;
                lbl4.Visibility = Visibility.Visible;
                lbl5.Visibility = Visibility.Visible;

                // new version
                lblVersion.Visibility = Visibility.Visible;
                lblVersion.Content = d.tag_name;

                // release date
                lblDate.Visibility = Visibility.Visible;    // release date
                string sDate = d.published_at;
                string[] sD = sDate.Split('T');
                lblDate.Content = sD[0];

                // release notes
                tbNotes.Visibility = Visibility.Visible;
                tbNotes.Text = d.body;

                // download URL
                lblDownloadUrl.Visibility = Visibility.Visible;

                string ghUrl = d.assets[0].browser_download_url;
                // split the string up so only the version is left
                string[] stArr = ghUrl.Split(new string[] { "MedLaunch_v" }, StringSplitOptions.None);
                string verUnderscores = stArr.Last().Replace(".zip", "");
                // display new medlaunch.info download url
                string newUrl = "https://downloads.medlaunch.info/?download=" + verUnderscores;

                string te = newUrl.ToString();

                lblDownloadUrl.Text = newUrl.ToString();

                btnUpdate.Visibility = Visibility.Visible;
                lblNoUpdate.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblVersion.Visibility = Visibility.Collapsed;
                lblDate.Visibility = Visibility.Collapsed;
                tbNotes.Visibility = Visibility.Collapsed;
                lblDownloadUrl.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                lbl1.Visibility = Visibility.Collapsed;
                lbl2.Visibility = Visibility.Collapsed;
                lbl3.Visibility = Visibility.Collapsed;
                lbl4.Visibility = Visibility.Collapsed;
                lbl5.Visibility = Visibility.Collapsed;
                lblNoUpdate.Visibility = Visibility.Visible;
            }

            // mednafen versions
            UpdateCheckMednafen();
        }

        public void UpdateCheckMednafen()
        {
            //Versions ver = new Versions();
            // compatible mednafen version
            lblcompatmedversion.Content = VersionChecker.Instance.LatestCompatMedVerDesc.FullVersionString; // ver.LatestCompatMednafenVersion;

            // installed mednafen version
            if (VersionChecker.Instance.CurrentMedVerDesc == null || !VersionChecker.Instance.CurrentMedVerDesc.IsValid)
            {
                // version not detected. 
                lblinstalledmedversion.Content = "ERROR: Unable to detect.";
                return;
            }
            else
            {
                lblinstalledmedversion.Content = VersionChecker.Instance.CurrentMedVerDesc.FullVersionString;
            }

            // update header if there is a newer mednafen version available
            //string[] CurrVersionArr = ver.CurrentMednafenVersion.Split('.');
            //string[] newVersionArr = ver.LatestCompatMednafenVersion.Split('.');

            var CurMedVerDesc = VersionChecker.Instance.CurrentMedVerDesc; // LogParser.Instance.MedVersionDesc;
            var NewMedVerDesc = VersionChecker.Instance.LatestCompatMedVerDesc; // MednafenVersionDescriptor.ReturnVersionDescriptor(ver.LatestCompatMednafenVersion);
            
            bool upgradeNeeded = false;

            if (CurMedVerDesc.IsValid && NewMedVerDesc.IsValid)
            {
                while (!upgradeNeeded)
                {
                    if (NewMedVerDesc.MajorINT > CurMedVerDesc.MajorINT)
                    {
                        upgradeNeeded = true;
                    }
                    else if (NewMedVerDesc.MinorINT > CurMedVerDesc.MinorINT)
                    {
                        upgradeNeeded = true;
                    }
                    else if (NewMedVerDesc.BuildINT > CurMedVerDesc.BuildINT)
                    {
                        upgradeNeeded = true;
                    }
                    else
                    {
                        // test the 4th value if it actually exists
                        if (NewMedVerDesc.IsNewFormat)
                        {
                            break;
                        }
                        else
                        {
                            if (!CurMedVerDesc.IsNewFormat)
                            {
                                if (NewMedVerDesc.RevisionINT > CurMedVerDesc.RevisionINT)
                                {
                                    upgradeNeeded = true;
                                }
                            }
                        }
                    }
                }
            }

            /*
            for (int v = 0; v < 4; v++)
            {
                int currV = 0;
                int currN = 0;

                if (v < CurrVersionArr.Length)
                {
                    currV = Convert.ToInt32(CurrVersionArr[v]);
                }
                if (v < newVersionArr.Length)
                {
                    currN = Convert.ToInt32(newVersionArr[v]);
                }

                if (currV > currN)
                {
                    // current version is NEWER than new version - upgrade not needed - break
                    break;
                }
                if (currV == currN)
                {
                    // versions are the same - continue checking
                    continue;
                }
                if (currV < currN)
                {
                    // new version is greater than old for this octet - upgrade needed
                    upgradeNeeded = true;
                    break;
                }
            }
            */

            if (upgradeNeeded == true)
            {
                UpdateStatusMF = true;
            }
            else
            {
                UpdateStatusMF = false;
            }
            ChangeUpdateStatus();
        }

        public void ChangeUpdateStatus()
        {
            if (UpdateStatusMF == false && UpdateStatusML == false)
            {
                UpdatedHeader.Header = "Updates";
            }

            if (UpdateStatusMF == true && UpdateStatusML == false)
            {
                UpdatedHeader.Header = "**MEDNAFEN UPDATE AVAILABLE**";
            }

            if (UpdateStatusMF == false && UpdateStatusML == true)
            {
                UpdatedHeader.Header = "**MEDLAUNCH UPDATE AVAILABLE**";
            }

            if (UpdateStatusMF == true && UpdateStatusML == true)
            {
                UpdatedHeader.Header = "**2 UPDATES AVAILABLE**";
            }
        }

        private void btnCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(false);
        }

        private void btnCheckForMednafenUpdates_Click(object sender, RoutedEventArgs e)
        {
            LogParser.Instance.IsDirty = true;
            UpdateCheckMednafen();
        }

        public bool DownloadMednafenNoAsync()
        {
            /* start download and extraction of latest compatible mednafen version */
            //Versions ver = new Versions();

            ProgressBar pb = new ProgressBar();
            pb.IsIndeterminate = true;

            string downloadsFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\Updates";
            System.IO.Directory.CreateDirectory(downloadsFolder);

            // get the new version
            string url = VersionChecker.Instance.LatestCompatMednafenDownloadURL;
            string fName = url.Split('/').Last();

            // try the download

            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 2000;
                try
                {
                    wc.DownloadFile(url, downloadsFolder + "\\" + fName);
                }
                catch
                {
                    wc.Dispose();
                    return false;
                }
                finally
                {
                    wc.Dispose();
                }
            }

            // extract download to current mednafen folder
            string meddir = Paths.GetPaths().mednafenExe;

            //Archiving arch = new Archiving(downloadsFolder + "\\" + fName);

            try
            {
                //arch.ExtractArchiveZipOverwrite(meddir);
                Archive.ExtractEntireZip(downloadsFolder + "\\" + fName, meddir);
            }
            catch
            {
                return false;
            }
            finally
            {

            }
            return true;
        }

        public async void UpdateMednafenToLatest(bool isStartup)
        {
            /* start download and extraction of latest compatible mednafen version */
            Versions ver = new Versions();

            string downloadsFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\Updates";
            System.IO.Directory.CreateDirectory(downloadsFolder);

            // get the new version
            string url = VersionChecker.Instance.LatestCompatMednafenDownloadURL;
            string fName = url.Split('/').Last();

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Download",
                AnimateShow = true,
                AnimateHide = true

            };

            if (isStartup == false)
            {
                var controller = await this.ShowProgressAsync("Mednafen Update", "Downloading " + url, settings: mySettings);
                controller.SetCancelable(false);
                controller.SetIndeterminate();

                await Task.Delay(400);

                // try the download

                await Task.Run(() =>
                {
                    // delete the update locally if it already exists
                    if (File.Exists(downloadsFolder + "\\" + fName))
                    {
                        File.Delete(downloadsFolder + "\\" + fName);
                    }

                    // download mednafen
                    bool result = StartDownload(controller, url, downloadsFolder + "\\" + fName, "URL: " + url);
                    /*
                    using (var wc = new CustomWebClient())
                    {
                        wc.Proxy = null;
                        wc.Timeout = 2000;
                        try
                        {
                            wc.DownloadFile(url, downloadsFolder + "\\" + fName);
                        }
                        catch
                        {
                            controller.SetMessage("The request timed out - please try again");
                            Task.Delay(2000);
                            controller.CloseAsync();
                            wc.Dispose();
                            return;
                        }
                        finally
                        {
                            wc.Dispose();
                        }
                    }
                    */

                    if (result == true)
                    {
                        // extract download to current mednafen folder
                        string meddir = Paths.GetPaths().mednafenExe;

                        //Archiving arch = new Archiving(downloadsFolder + "\\" + fName);
                        try
                        {
                            //arch.ExtractArchiveZipOverwrite(meddir);
                            Archive.ExtractEntireZip(downloadsFolder + "\\" + fName, meddir);
                        }
                        catch { }
                    }
                });

                await controller.CloseAsync();
            }
            else
            {

                await Task.Delay(400);

                // try the download

                using (var wc = new CustomWebClient())
                {
                    wc.Proxy = null;
                    wc.Timeout = 2000;
                    try
                    {
                        wc.DownloadFile(url, downloadsFolder + "\\" + fName);
                    }
                    catch
                    {
                        await Task.Delay(2000);
                        wc.Dispose();
                        return;
                    }
                    finally
                    {
                        wc.Dispose();
                    }
                }

                // extract download to current mednafen folder
                string meddir = Paths.GetPaths().mednafenExe;

                //Archiving arch = new Archiving(downloadsFolder + "\\" + fName);
                try
                {
                    //arch.ExtractArchiveZipOverwrite(meddir);
                    Archive.ExtractEntireZip(downloadsFolder + "\\" + fName, meddir);
                }
                catch { }

            }



            // check updates again
            UpdateCheckMednafen();
        }

        private void btnUpdateMednafen_Click(object sender, RoutedEventArgs e)
        {
            UpdateMednafenToLatest(false);

        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            /* start download and autoupdate */


            string downloadsFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\Updates";
            System.IO.Directory.CreateDirectory(downloadsFolder);

            // get the new version
            string v = lblVersion.Content.ToString();
            string[] vArr = v.Split('.');

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Download",
                AnimateShow = true,
                AnimateHide = true
            };

            var controller = await this.ShowProgressAsync("Downloading MedLaunch v" + v, "", settings: mySettings);
            controller.SetCancelable(false);
            //controller.SetIndeterminate();

            // download url
            string url = lblDownloadUrl.Text.ToString();
            string ver = lblVersion.Content.ToString();
            await Task.Delay(400);

            await Task.Run(() =>
            {
                // get just the filename
                //string[] fArr = url.Split('/');
                //string fName = fArr[fArr.Length - 1];

                string fName = "MedLaunch_v" + ver.Replace(".", "_") + ".zip";

                // try the download
                bool result = StartDownload(controller, url, downloadsFolder + "\\" + fName, "URL: " + url);

                if (result == true)
                {
                    controller.SetIndeterminate();
                    controller.SetMessage("Download Complete. Starting Install.....");

                    // now run updater app to extract MedLaunch over the existing directory
                    // build command line args
                    string processArg = "/P:" + Process.GetCurrentProcess().Id.ToString();
                    string upgradeArg = "/U:" + fName; // "MedLaunch_v" + vArr[0] + "_" + vArr[1] + "_" + vArr[2] + "_" + vArr[3] + ".zip";
                    string args = processArg + " " + upgradeArg;
                    // call the external updater app and close this one
                    // call the updater app and close this one
                    Process.Start("lib\\Updater.exe", args);
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
            });
            await controller.CloseAsync();

            // mednafen versions
            UpdateCheckMednafen();
        }

        DispatcherTimer dlTimer = new DispatcherTimer();
        private void dlTimer_Tick(object sender, EventArgs e)
        {
            // if this gets called, we can assume the download has timed out
            //MessageBox.Show("download has timed out!");
            dlTimedout = true;
        }

        private bool StartDownload(ProgressDialogController controller, string uri, string destination, string dialogMessage)
        {
            DialogMessage = dialogMessage;

            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 2000;
                try
                {
                    DownloadComplete = false;
                    PDC = controller;
                    PDC.Maximum = 100;
                    PDC.Minimum = 0;
                    wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    wc.DownloadFileAsync(new Uri(uri), destination);

                    // start the download timer 
                    DownloadTimerGo(wc);



                    while (!DownloadComplete)
                    {
                        // tick
                    }

                    Task.Delay(2000);
                }
                catch
                {
                    controller.SetMessage("The request timed out - please try again");
                    Task.Delay(2000);
                    controller.CloseAsync();
                    wc.Dispose();
                    return false;
                }
                finally
                {
                    wc.Dispose();
                }

                return true;
            }
        }

        bool dlTimedout = false;

        private void DownloadTimerGo(CustomWebClient wc)
        {
            dlTimedout = false;
            dlTimer.Tick += new EventHandler(dlTimer_Tick);
            dlTimer.Interval = new TimeSpan(0, 0, 20);
            dlTimer.Start();

            while (dlTimer.IsEnabled)
            {
                // timer is running
                if (dlTimedout == true)
                {
                    // download has timed out - cancel it
                    wc.CancelAsync();
                }
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // update PDC with percentage
            PDC.SetProgress(e.ProgressPercentage);
            PDC.SetMessage(DialogMessage + "\n\nDownloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + " bytes");

            // reset the timer
            dlTimer.Stop();
            dlTimer.Start();
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // stop the timer
            dlTimer.Stop();

            //progressBar1.Value = 0;
            DownloadComplete = true;

            if (e.Cancelled)
            {
                MessagePopper.ShowMessageDialog("The download has been cancelled: \n\n" + e.Cancelled, "INFORMATION");
                //MessageBox.Show("The download has been cancelled: \n\n" + e.Cancelled);
                return;
            }



            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                MessagePopper.ShowMessageDialog("An error ocurred while trying to download file: \n\n" + e.Error, "ERROR");
                //MessageBox.Show("An error ocurred while trying to download file: \n\n" + e.Error);

                return;
            }

            //MessageBox.Show("File succesfully downloaded");
        }

        #endregion

        #region CustomWebClient Implementation

        private class CustomWebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                ((HttpWebRequest)lWebRequest).KeepAlive = false;
                return lWebRequest;
            }
        }

        #endregion

        #region Capture (not currently implemented)

        private void chkStreamDisk_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkStreamDisk_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkStreamTwitch_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkStreamTwitch_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnPathFfmpeg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPathOutputFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkForceOverwrite_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkForceOverwrite_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Logging Viewing

        private void btnViewMednafenLogFile_Click(object sender, RoutedEventArgs e)
        {
            // get path to mednafen folder
            Paths p = Paths.GetPaths();
            string folderPath = p.mednafenExe;
            string logPath = folderPath + "\\" + "stdout.txt";

            // test whether log exists
            if (!File.Exists(logPath))
                return;

            // open log in default editor
            FileAssociation.OpenTxtFileInDefaultViewer(logPath);
        }

        private void btnViewMednafenExFile_Click(object sender, RoutedEventArgs e)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Exceptions.log";

            // test whether log exists
            if (!File.Exists(logPath))
                return;

            // open log in default editor
            FileAssociation.OpenTxtFileInDefaultViewer(logPath);
        }

        #endregion

        #region Misc Menu Options

        private async void medCoreVisibility_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            await mw.ShowChildWindowAsync(new MednafenCoreVisibility()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Manage Emulated System Visibility",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        private void menuQuitMedLaunch_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TopMenu_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void manualLocateLaunchGame_Click(object sender, RoutedEventArgs e)
        {
            // popup select game file dialog
            string filePath = GameLauncher.SelectGameFile();

            // check whether path is null
            if (filePath == null)
                return;

            // check whether file exists
            if (!File.Exists(filePath))
                return;

            // Attempt to launch game using Mednafen config settings that are on disk (not parsing any MedLaunch command line parameters)
            string medExe = Paths.GetPaths().mednafenExe + "\\mednafen.exe";

            // check whether minimise to taskbar option is checked
            bool taskbar = this.ShowInTaskbar;
            if (GlobalSettings.Min2TaskBar() == true)
            {
                this.ShowInTaskbar = true;
                this.WindowState = WindowState.Minimized;
            }


            System.Diagnostics.Process gProcess = new System.Diagnostics.Process();
            gProcess.StartInfo.UseShellExecute = true;
            gProcess.StartInfo.RedirectStandardOutput = false;
            //gProcess.StartInfo.WorkingDirectory = "\"" + Paths.GetPaths().mednafenExe + "\"";
            gProcess.StartInfo.FileName = medExe;
            gProcess.StartInfo.CreateNoWindow = false;
            gProcess.StartInfo.Arguments = "\"" + filePath + "\"";
            gProcess.Start();
            gProcess.WaitForExit();


            if (GlobalSettings.Min2TaskBar() == true)
            {
                this.ShowInTaskbar = taskbar;
                this.WindowState = WindowState.Normal;
            }



        }

        private async void MenuItemColumn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            await mw.ShowChildWindowAsync(new LibraryColumnChooser()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Games Library Column Visibility Settings",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);
        }

        #endregion

        #region Tabs

        /// <summary>
        /// triggered when tab navigation happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tc = sender as TabControl;
            if (tc != null)
            {
                var item = (TabItem)tc.SelectedItem;
                if (item == null)
                    return;

                // instantiate the discord visual handler when the discord tab is first clicked
                if (DVH == null && item.Header.ToString() == "Discord")
                {
                    DVH = new DiscordVisualHandler();
                }

                if (item.Header.ToString() == "Configs")
                {
                    ConfigsTabSelected = true;
                }

                if (ConfigsTabSelected == null)
                {
                    // application is still starting and/or the configs tab has not been selected yet.
                    return;
                }

                if (item.Header.ToString() == "Configs")
                {
                    //MessageBox.Show("Tab Item Configs has been selected");
                    ConfigsTabSelected = true;
                }
                else
                {
                    //MessageBox.Show("Tab Item " + item.Header.ToString() +" has been selected");
                    ConfigsTabSelected = false;
                }

                // if something other than configs tab is selected (and the bool check is not null) then save the current config
                if (ConfigsTabSelected == false)
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
                    //MessageBox.Show("Config saved for configid: " + configId);
                    //lblConfigStatus.Content = "***Config Saved***";
                }



            }
        }

        #endregion

        #region Discord

        private void DiscordLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Launch discord invite link in default browser
            try
            {
                Process.Start("https://discord.gg/nsbanNa");
            }
            catch
            {
                // do nothing
            }

        }

        private void DiscordLogo_MouseEnter(object sender, MouseEventArgs e)
        {
            var im = (Image)sender as Image;
            im.Cursor = Cursors.Hand;
        }

        private void DiscordLogo_MouseLeave(object sender, MouseEventArgs e)
        {
            var im = (Image)sender as Image;
            im.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// entry point for discord connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiscordConnect_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender as Button;

            // temporary code for now until mednanet client is implemented
            if (tbDiscordName.Text == null || tbDiscordName.Text.Trim() == "")
            {
                MessagePopper.ShowMessageDialog("Please enter a valid username", "Username Missing!");
                //MessageBox.Show("Please enter a valid username", "Username Missing!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (btn.Content.ToString() == "CONNECT")
            {
                DVH.SetConnectedStatus(true);

                // init the api
                DVH.PostLocalOnlyMessage("Connecting to the MednaNet API...");

                DiscordHandler.Initialize();

                /*
                if (DiscordHandler.Instance.IsConnected == false)
                {
                    DiscordHandler.Instance.APIDisconnected(new Exception("Could not connect to the MednaNet API"));
                    return;
                } */

                // update username
                //DiscordHandler.Instance.Username = tbDiscordName.Text;
                //MednaNetSettings.SetUsername(MednaNetAPI.Instance.Username);

                // select the first channel
                UIHandler ui = UIHandler.GetChildren(DiscordSelectorWrapPanel);
                RadioButton rb = ui.RadioButtons.FirstOrDefault();

                if (rb != null)
                {
                    rb.IsChecked = true;
                    string name = rb.Name;
                    string idStr = name.Replace("rbDiscordCh", "");
                    int id = Convert.ToInt32(idStr);
                    DVH.ChangeChannel(id);

                    // update title
                    lblDiscordChannel.Content = "MedLaunch: Discord - #" + DVH.channels.Data.Where(a => a.ChannelId == id).FirstOrDefault().ChannelName;

                }
                else
                {
                    // rb is null
                }
            }
            else
            {
                DVH.SetConnectedStatus(false);
                DVH.PostLocalOnlyMessage("Disconnecting from the MednaNet API...");
                lblDiscordChannel.Content = "MedLaunch: Discord";
                expDiscordUsersOnline.Header = "USERS ONLINE (0)";

                //MednaNetAPI.Instance.Stop();
            }
        }

        private void btnDiscordChatSend_Click(object sender, RoutedEventArgs e)
        {
            string con = tbDiscordMessageBox.Text.Trim();
            if (con == "")
                return;

            //DVH.PostMessage(con, DVH.channels.ActiveChannel);
            //DVH.PostFromLocal(con);
            DiscordHandler.Instance.SendMessage(con);

            tbDiscordMessageBox.Text = "";
        }

        public void btnDiscordChannel_Checked(object sender, RoutedEventArgs e)
        {
            // get channel ID from name
            var btn = (RadioButton)sender as RadioButton;
            string name = btn.Name;
            string idStr = name.Replace("rbDiscordCh", "");
            int id = Convert.ToInt32(idStr);

            // notify discordvisualhandler that channel has changed
            DVH.ChangeChannel(id);

            // update title
            lblDiscordChannel.Content = "MedLaunch: Discord - #" + DVH.channels.Data.Where(a => a.ChannelId == id).FirstOrDefault().ChannelName;

        }

        public void Hyperlink_Click(object sender, EventArgs e)
        {
            Process.Start((sender as Hyperlink).NavigateUri.AbsoluteUri);
        }

        #endregion

        #region TV-View

        private void tvview_Click(object sender, RoutedEventArgs e)
        {
            TVView win = new TVView();
            win.Show();
        }

        #endregion


        #region Test Methods

        private void disctest_Click(object sender, RoutedEventArgs e)
        {
            string serial1 = MedDiscUtils.GetPSXSerial(@"G:\_Emulation\PSX\iso\Final Fantasy 9 [PSX][PAL]\Final Fantasy 9 CD1.img");
            string serial2 = MedDiscUtils.GetPSXSerial(@"G:\_Emulation\PSX\iso\Final Fantasy 9 [PSX][PAL]\Final Fantasy 9 CD2.img");
            string serial3 = MedDiscUtils.GetPSXSerial(@"G:\_Emulation\PSX\iso\Final Fantasy 9 [PSX][PAL]\Final Fantasy 9 CD3.img");
            string serial4 = MedDiscUtils.GetPSXSerial(@"G:\_Emulation\PSX\iso\Final Fantasy 1 (Origins)(E) [SLES-04034]\Final Fantasy Origins - Final Fantasy (E) [SLES-04034].bin");
            string serial5 = MedDiscUtils.GetPSXSerial(@"G:\_Emulation\PSX\iso\Alien Trilogy (E) [SLES-00101]\Alien Trilogy (E) [SLES-00101].iso");
        }

        private void UCONtest_Click(object sender, RoutedEventArgs e)
        {
            UconWrapper u = new UconWrapper(System.AppDomain.CurrentDomain.BaseDirectory + @"\ucon64-bin\ucon64.exe");
            string gamePath = @"G:\_Emulation\Sega Megadrive - 32x - SegaCD\roms\Megadrive\long folder test long folder test long folder testlong folder test long folder test long folder test\Animaniacs (E).zip";
            //string result = u.DoTestScan(gamePath, SystemType.Genesis);
            var result = u.ScanGame(gamePath, SystemType.Genesis);

            if (result.Data.IsChecksumValid == true && result.Data.IsInterleaved == true)
            {
                // checksum is valid and rom needs interleaving
                u.OutputFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Cache";
            }
        }

        private void UCONConvertSmd_Click(object sender, RoutedEventArgs e)
        {
            UconWrapper u = new UconWrapper(System.AppDomain.CurrentDomain.BaseDirectory + @"\ucon64-bin\ucon64.exe");
            //string gamePath = @"G:\_Emulation\Sega Megadrive - 32x - SegaCD\roms\Megadrive\long folder test long folder test long folder testlong folder test long folder test long folder test\Animaniacs (E).zip";
            string gamePath = @"G:\_Emulation\Sega Megadrive - 32x - SegaCD\roms\Megadrive\test\Alex Kidd in the Enchanted Castle (J).smd";
            u.OutputFolder = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Cache";
            var result = u.ProcessSMD(gamePath);
            if (result == null)
            {
                MessagePopper.ShowMessageDialog("checksum invalid - skipping rom", "INFORMATION");
                //MessageBox.Show("checksum invalid - skipping rom");
                return;
            }
            else
            {
                // either rom has been converted, or it is compatible
                MessagePopper.ShowMessageDialog("Final Rom Path: " + result.ConvertedPath, "INFORMATION");
                //MessageBox.Show("Final Rom Path: " + result.ConvertedPath);
            }
        }

        private async void testarchiving_Click(object sender, RoutedEventArgs e)
        {
            string arcPath = @"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.7z";

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await this.ShowProgressAsync("Scanning test archive", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);
            await Task.Run(() =>
            {
                Task.Delay(1);
                MedLaunch.Common.IO.Compression.Archive a = new MedLaunch.Common.IO.Compression.Archive(arcPath);
                ProgressDialogListener l = new ProgressDialogListener(controller, SignatureType.Archive);
                l.Subscribe(a);
                var results = a.ProcessArchive(new string[] { ".sms", ".7z" });

            });
            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("Archive test", "Operation Cancelled");
            }
            else
            {
                await this.ShowMessageAsync("Archive test", "Scanning Completed");
            }
        }

        #endregion

        public static Visibility IsDebug
        {
#if DEBUG
            get { return Visibility.Visible; }
#else
        get { return Visibility.Collapsed; }
#endif
        }

        
    }


    #region Converters

    /// <summary>
    /// Converts (int)1 to true
    /// </summary>
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

    public class CaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (value is String)
            {
                if (mw.CaseConvert == 0)
                    return value.ToString();
                if (mw.CaseConvert == 2)
                    return value.ToString().ToUpper();
                if (mw.CaseConvert == 1)
                {
                    // title case logic
                    string s = value.ToString().Replace("  ", " ").Trim();
                    string tc = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());

                    /* some more logic */
                    // numerals
                    tc = tc.Replace("Iii", "II")
                        .Replace("Ii", "II")
                        .Replace("Iiv", "IIV")
                        .Replace("Iv", "IV")
                        .Replace("Viii", "VIII")
                        .Replace("Vii", "VII")
                        .Replace("Vi", "VI")
                        .Replace("Iix", "IIX")
                        .Replace("Ix", "IX")
                        .Replace("Xiii", "XIII")
                        .Replace("Xii", "XII")
                        .Replace("Xi", "XII");

                    // known upper case phrases
                    tc = tc.Replace("Wwf", "WWF")
                        .Replace("Cg", "CG")
                        .Replace("VIr", "Vir")
                        .Replace("Nhl", "NHL")
                        .Replace("Nba", "NBA")
                        .Replace("Nfl", "NFL");

                    // playstation codes
                    tc = tc.Replace("Slus", "SLUS")
                        .Replace("Scus", "SCUS")
                        .Replace("Sles", "SLES")
                        .Replace("Sces", "SCES")
                        .Replace("Slps", "SLPS")
                        .Replace("Scps", "SCPS")
                        .Replace("Slpm", "SLPM")
                        .Replace("Sips", "SIPS");



                    return tc;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (value is String)
            {
                string input = value.ToString().Trim();

                // does string have 4 characters only (ie a possible year string, 1990, 200x etc)
                if (input.Length == 4)
                {
                    // is the first character a number
                    int n;
                    bool isNumeric = int.TryParse(input[0].ToString(), out n);
                    if (isNumeric)
                    {
                        return input;
                    }
                    else
                        return string.Empty;
                }

                // dates spit by delimeters
                char[] chrs = { '/', '_', '-', '.', ' ' };
                string final = "";
                foreach (var c in chrs)
                {
                    if (final != "")
                        break;

                    if (input.Contains(c))
                    {
                        string[] arr = input.Split(c);
                        foreach (var s in arr)
                        {
                            if (s.Length == 4)
                            {
                                // this is probably the date
                                // is the first character a number
                                int n;
                                bool isNumeric = int.TryParse(s[0].ToString(), out n);
                                if (isNumeric)
                                {
                                    final = s;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (final != "")
                    return final;

                // if we get this far its probably not a correct year.
                return string.Empty;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CountryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (value is String)
            {
                string input = value.ToString().Trim();

                return StringConv.ConvertCountryString(input);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AttachedProperties
    {
        #region HideExpanderArrow AttachedProperty

        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static bool GetHideExpanderArrow(DependencyObject obj)
        {
            return (bool)obj.GetValue(HideExpanderArrowProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static void SetHideExpanderArrow(DependencyObject obj, bool value)
        {
            obj.SetValue(HideExpanderArrowProperty, value);
        }

        // Using a DependencyProperty as the backing store for HideExpanderArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideExpanderArrowProperty =
            DependencyProperty.RegisterAttached("HideExpanderArrow", typeof(bool), typeof(AttachedProperties), new UIPropertyMetadata(false, OnHideExpanderArrowChanged));

        private static void OnHideExpanderArrowChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Expander expander = (Expander)o;

            if (expander.IsLoaded)
            {
                UpdateExpanderArrow(expander, (bool)e.NewValue);
            }
            else
            {
                expander.Loaded += new RoutedEventHandler((x, y) => UpdateExpanderArrow(expander, (bool)e.NewValue));
            }
        }

        private static void UpdateExpanderArrow(Expander expander, bool visible)
        {
            int c = VisualTreeHelper.GetChildrenCount(expander);
            if (c == 0)
                return;

            Grid headerGrid = new Grid();

            switch (c)
            {
                case 1:
                    headerGrid = VisualTreeHelper.GetChild(expander, 0) as Grid;
                    break;

                case 2:
                    headerGrid =
               VisualTreeHelper.GetChild(
                           VisualTreeHelper.GetChild(
                                       expander,
                                       0),
                       0) as Grid;
                    break;

                case 3:
                    headerGrid =
               VisualTreeHelper.GetChild(
                           VisualTreeHelper.GetChild(
                               VisualTreeHelper.GetChild(
                                       expander,
                                       0),
                                   0),
                       0) as Grid;
                    break;

                case 4:
                    headerGrid =
                VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(
                                        expander,
                                        0),
                                    0),
                            0),
                        0) as Grid;
                    break;

                case 5:
                    headerGrid =
                VisualTreeHelper.GetChild(
                    VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(
                                        expander,
                                        0),
                                    0),
                                0),
                            0),
                        0) as Grid;
                    break;
            }

            /*
            Grid headerGrid =
                VisualTreeHelper.GetChild(
                    VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(
                                        expander,
                                        0),
                                    0),
                                0),
                            0),
                        0) as Grid;
                        */

            headerGrid.Children[0].Visibility = visible ? Visibility.Collapsed : Visibility.Visible; // Hide or show the Ellipse
            headerGrid.Children[1].Visibility = visible ? Visibility.Collapsed : Visibility.Visible; // Hide or show the Arrow
            headerGrid.Children[2].SetValue(Grid.ColumnProperty, visible ? 0 : 1); // If the Arrow is not visible, then shift the Header Content to the first column.
            headerGrid.Children[2].SetValue(Grid.ColumnSpanProperty, visible ? 2 : 1); // If the Arrow is not visible, then set the Header Content to span both rows.
            headerGrid.Children[2].SetValue(ContentPresenter.MarginProperty, visible ? new Thickness(0) : new Thickness(4, 0, 0, 0)); // If the Arrow is not visible, then remove the margin from the Content.
        }

        #endregion
    }

    // Used on sub-controls of an expander to bubble the mouse wheel scroll event up 
    public sealed class BubbleScrollEvent : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            base.OnDetaching();
        }

        void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = UIElement.MouseWheelEvent;
            AssociatedObject.RaiseEvent(e2);
        }
    }

    #endregion

    public enum MediaType
    {
        ROM,
        DISC,
        ALL
    }
}
