using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Models
{
    public class GlobalSettings
    {
        public int settingsId { get; set; }                         // always 1
        public bool? databaseGenerated { get; set; }                // set to true to avoid regeneration at runtime
        public bool? fullScreen { get; set; }                       // force fullscreen when running the emulator
        public bool? fullGuiScreen { get; set; }                    // force fullscreen on startup in GUI launcher
        public bool? bypassConfig { get; set; }                     // ignore GUI launcher configs and just launch with the mednafen text config files
        public bool? enableNetplay { get; set; }                    // enable netplay
        public bool? enableSnes_faust { get; set; }                 // use snes_faust emulation rather than SNES
        public bool? enablePce_fast { get; set; }                  // use pce_fast emulation rather than pce
        public int? serverSelected { get; set; }                    // stores the currently selected server ID
        public double guiZoom { get; set; }                         // the current GUI zoom factor as a decimal (1 = 100%)
        public bool? showAllBaseSettings { get; set; }              // if enabled, all settings are visible and configurable under the base configuration
        public bool? minToTaskBarOnGameLaunch { get; set; }         // whether or not MedLaunch is minimised to taskbar when game is launched
        public bool? hideSidebar { get; set; }                      // always hide the games library sidebar
        public DateTime? gdbLastUpdated { get; set; }               // last time platformgames were synced from thegamesdb.net
        public bool? backupMednafenConfig { get; set; }             // auto create a backup on the mednafen config when MedLaunch is first opened
        public bool? saveSystemConfigs { get; set; }                // auto save system.cfg in root of Mednafen directory before game is launched
        public bool? enableConfigToolTips { get; set; }             // enable tooltip popups on Mednafen config controls
        public bool? enableClearCacheOnExit { get; set; }           // clears the Data\\Cache folder on application exit

        // games library expander states
        public bool glGameStats { get; set; }
        public bool glGameInfo { get; set; }
        public bool glOverview { get; set; }
        public bool glScreenshots { get; set; }
        public bool glFanart { get; set; }
        public bool glScrapingOptions { get; set; }
        public bool glSystemInfo { get; set; }
        public bool glManuals { get; set; }

        // games library columns (no longer used)
        public bool showGLPublisher { get; set; }
        public bool showGLDeveloper { get; set; }
        public bool showGLYear { get; set; }
        public bool showGLPlayers { get; set; }
        public bool showGLCoop { get; set; }
        public bool showGLESRB { get; set; }

        // games library column visibility (based on filter)
        public string glFil01 { get; set; }             // Show All Games
        public string glFil02 { get; set; }             // Favorites
        public string glFil03 { get; set; }             // Unscraped Games
        public string glFil04 { get; set; }             // NES
        public string glFil05 { get; set; }             // SNES
        public string glFil06 { get; set; }             // SMS
        public string glFil07 { get; set; }             // MD
        public string glFil08 { get; set; }             // PCE
        public string glFil09 { get; set; }             // VB
        public string glFil10 { get; set; }             // NGP
        public string glFil11 { get; set; }             // WSWAN
        public string glFil12 { get; set; }             // GB
        public string glFil13 { get; set; }             // GBA
        public string glFil14 { get; set; }             // GG
        public string glFil15 { get; set; }             // LYNX
        public string glFil16 { get; set; }             // SS
        public string glFil17 { get; set; }             // PSX
        public string glFil18 { get; set; }             // PCECD
        public string glFil19 { get; set; }             // PCFX

        // games library search filters visibility
        public bool hideCountryFilters { get; set; }

        // Mednafen Cores Visability (uses medlaunch systemId)
        public bool coreVis1 { get; set; }
        public bool coreVis2 { get; set; }
        public bool coreVis3 { get; set; }
        public bool coreVis4 { get; set; }
        public bool coreVis5 { get; set; }
        public bool coreVis6 { get; set; }
        public bool coreVis7 { get; set; }
        public bool coreVis8 { get; set; }
        public bool coreVis9 { get; set; }
        public bool coreVis10 { get; set; }
        public bool coreVis11 { get; set; }
        public bool coreVis12 { get; set; }
        public bool coreVis13 { get; set; }
        public bool coreVis14 { get; set; }
        public bool coreVis15 { get; set; }
        public bool coreVis16 { get; set; }
        public bool coreVis17 { get; set; }
        public bool coreVis18 { get; set; }

        // Game scraping options
        public bool scrapeBanners { get; set; }
        public bool scrapeBoxart { get; set; }
        public bool scrapeScreenshots { get; set; }
        public bool scrapeFanart { get; set; }
        public bool scrapeMedia { get; set; }
        public bool scrapeManuals { get; set; }
        public bool preferGenesis { get; set; }
        public bool enabledSecondaryScraper { get; set; }
        public int primaryScraper { get; set; }

        public double maxScreenshots { get; set; }
        public double maxFanarts { get; set; }

        // GUI color scheme
        public string colorBackground { get; set; }
        public string colorAccent { get; set; }

        // application settings
        public bool? checkUpdatesOnStart { get; set; }
        public double imageToolTipPercentage { get; set; }           // what percentage of the current windows size can the image tooltips be
        public bool? importConfigsOnStart { get; set; }
        public bool? rememberSysWinPositions { get; set; }          // gets the saved windows position for each system at game launch and saves it at game end

        // system window positions
        public string sysWinPos1 { get; set; }                      // string format "x,y"
        public string sysWinPos2 { get; set; }
        public string sysWinPos3 { get; set; }
        public string sysWinPos4 { get; set; }
        public string sysWinPos5 { get; set; }
        public string sysWinPos6 { get; set; }
        public string sysWinPos7 { get; set; }
        public string sysWinPos8 { get; set; }
        public string sysWinPos9 { get; set; }
        public string sysWinPos10 { get; set; }
        public string sysWinPos11 { get; set; }
        public string sysWinPos12 { get; set; }
        public string sysWinPos13 { get; set; }
        public string sysWinPos14 { get; set; }
        public string sysWinPos15 { get; set; }
        public string sysWinPos16 { get; set; }
        public string sysWinPos17 { get; set; }
        public string sysWinPos18 { get; set; }

        public static GlobalSettings GetGlobalDefaults()
        {
            GlobalSettings gs = new GlobalSettings
            {
                databaseGenerated = false,
                fullGuiScreen = false,
                fullScreen = true,
                bypassConfig = false,
                settingsId = 1,
                enableNetplay = false,
                serverSelected = 1,
                enablePce_fast = false,
                enableSnes_faust = false,
                guiZoom = 1,
                minToTaskBarOnGameLaunch = true,
                hideSidebar = false,
                showAllBaseSettings = true,
                gdbLastUpdated = DateTime.Now,
                glGameStats = true,
                glGameInfo = true,
                glFanart = true,
                glOverview = true,
                glScrapingOptions = true,
                glScreenshots = true,
                glSystemInfo = true,
                glManuals = true,
                scrapeBanners = true,
                scrapeBoxart = true,
                scrapeFanart = true,
                scrapeScreenshots = true,
                scrapeManuals = true,
                scrapeMedia = true,
                enabledSecondaryScraper = true,
                preferGenesis = true,
                primaryScraper = 1,
                maxScreenshots = 4,
                maxFanarts = 4,
                colorBackground = "basedark",
                colorAccent = "Emerald",
                checkUpdatesOnStart = false,
                backupMednafenConfig = true,
                saveSystemConfigs = true,
                imageToolTipPercentage = 0.9,
                importConfigsOnStart = false,
                enableConfigToolTips = true,
                enableClearCacheOnExit = true,

                showGLCoop = true,
                showGLDeveloper = true,
                showGLESRB = true,
                showGLPlayers = true,
                showGLPublisher = true,
                showGLYear = true,

                glFil01 = "3FFF",
                glFil02 = "3FFF",
                glFil03 = "3FFF",
                glFil04 = "3FFF",
                glFil05 = "3FFF",
                glFil06 = "3FFF",
                glFil07 = "3FFF",
                glFil08 = "3FFF",
                glFil09 = "3FFF",
                glFil10 = "3FFF",
                glFil11 = "3FFF",
                glFil12 = "3FFF",
                glFil13 = "3FFF",
                glFil14 = "3FFF",
                glFil15 = "3FFF",
                glFil16 = "3FFF",
                glFil17 = "3FFF",
                glFil18 = "3FFF",
                glFil19 = "3FFF",

                hideCountryFilters = false,

                coreVis1 = true,
                coreVis2 = true,
                coreVis3 = true,
                coreVis4 = true,
                coreVis5 = true,
                coreVis6 = true,
                coreVis7 = true,
                coreVis8 = true,
                coreVis9 = true,
                coreVis10 = true,
                coreVis11 = true,
                coreVis12 = true,
                coreVis13 = true,
                coreVis14 = true,
                coreVis15 = true,
                coreVis16 = true,
                coreVis17 = true,
                coreVis18 = true,

                rememberSysWinPositions = false
            };
            return gs;
        }

        public static string[] GetGUIColors()
        {
            var gs = GlobalSettings.GetGlobals();
            string[] colors = { gs.colorBackground, gs.colorAccent };
            return colors;
        }

        public static string[] GetGUIColorsDefaults()
        {
            var gs = GlobalSettings.GetGlobalDefaults();
            string[] colors = { gs.colorBackground, gs.colorAccent };
            return colors;
        }

        public static void SaveCoreVisibilities(bool[] b)
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();

            gs.coreVis1 = b[0];
            gs.coreVis2 = b[1];
            gs.coreVis3 = b[2];
            gs.coreVis4 = b[3];
            gs.coreVis5 = b[4];
            gs.coreVis6 = b[5];
            gs.coreVis7 = b[6];
            gs.coreVis8 = b[7];
            gs.coreVis9 = b[8];
            gs.coreVis10 = b[9];
            gs.coreVis11 = b[10];
            gs.coreVis12 = b[11];
            gs.coreVis13 = b[12];
            gs.coreVis14 = b[13];
            gs.coreVis15 = b[14];
            gs.coreVis16 = b[15];
            gs.coreVis17 = b[16];
            gs.coreVis18 = b[17];

            GlobalSettings.SetGlobals(gs);
        }

        public static bool[] GetVisArray()
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();

            bool[] b = new bool[]
            {
                gs.coreVis1,
                gs.coreVis2,
                gs.coreVis3,
                gs.coreVis4,
                gs.coreVis5,
                gs.coreVis6,
                gs.coreVis7,
                gs.coreVis8,
                gs.coreVis9,
                gs.coreVis10,
                gs.coreVis11,
                gs.coreVis12,
                gs.coreVis13,
                gs.coreVis14,
                gs.coreVis15,
                gs.coreVis16,
                gs.coreVis17,
                gs.coreVis18
        };

            return b;
        }

        public static void SaveToDatabase(List<GlobalSettings> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.GlobalSettings.AsNoTracking().ToList();

                List<GlobalSettings> toAdd = new List<GlobalSettings>();
                List<GlobalSettings> toUpdate = new List<GlobalSettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    GlobalSettings t = (from a in current
                                               where a.settingsId == g.settingsId
                                               select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.GlobalSettings.UpdateRange(toUpdate);
                db.GlobalSettings.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        // get mintotaskbar value
        public static bool Min2TaskBar()
        {
            GlobalSettings g = GetGlobals();
            return g.minToTaskBarOnGameLaunch.Value;
        }

        // get hidesidebar settings
        public static bool GetHideSidebar()
        {
            GlobalSettings g = GetGlobals();
            return g.hideSidebar.Value;
        }

        // return Global Settings entry from DB
        public static GlobalSettings GetGlobals()
        {
            GlobalSettings gs = new GlobalSettings();
            using (var context = new MyDbContext())
            {
                var query = from s in context.GlobalSettings
                            where s.settingsId == 1
                            select s;
                gs = query.FirstOrDefault();
            }
            return gs;
        }

        // write Global Settings object to DB
        public static void SetGlobals(GlobalSettings gs)
        {
            using (var context = new MyDbContext())
            {
                context.GlobalSettings.Attach(gs);
                var entry = context.Entry(gs);                
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void LoadGlobalSettings(CheckBox EnableNetplay, CheckBox EnableSnes_Faust, CheckBox EnablePce_Fast, ComboBox GuiZoom, CheckBox MinToTaskBar, CheckBox HideSidebar,
            CheckBox chkAllowBanners, CheckBox chkAllowBoxart, CheckBox chkAllowScreenshots, CheckBox chkAllowFanart, CheckBox chkPreferGenesis, 
            CheckBox chkAllowManuals, CheckBox chkAllowMedia, CheckBox chkSecondaryScraperBackup, RadioButton rbGDB, RadioButton rbMoby, Slider slScreenshotsPerHost, Slider slFanrtsPerHost,
            CheckBox chkAllowUpdateCheck, CheckBox chkBackupMednafenConfig, CheckBox chkSaveSysConfigs, ComboBox comboImageTooltipSize, CheckBox chkLoadConfigsOnStart, CheckBox chkEnableConfigToolTips,
            CheckBox chkshowGLYear, CheckBox chkshowGLESRB, CheckBox chkshowGLCoop, CheckBox chkshowGLDeveloper, CheckBox chkshowGLPublisher, CheckBox chkshowGLPlayers, CheckBox chkEnableClearCacheOnExit, 
            CheckBox chkrememberSysWinPositions, CheckBox chkHideCountryFilter)
        {
            GlobalSettings gs = GetGlobals();
            // update all checkboxes
            EnableNetplay.IsChecked = gs.enableNetplay;
            EnablePce_Fast.IsChecked = gs.enablePce_fast;
            EnableSnes_Faust.IsChecked = gs.enableSnes_faust;

            chkrememberSysWinPositions.IsChecked = gs.rememberSysWinPositions;

            //FullScreen.IsChecked = gs.fullScreen;
            //BypassConfig.IsChecked = gs.bypassConfig;

            // update comboboxes
            GuiZoom.SelectedValue = gs.guiZoom.ToString();
            MinToTaskBar.IsChecked = gs.minToTaskBarOnGameLaunch;
            HideSidebar.IsChecked = gs.hideSidebar;

            chkSaveSysConfigs.IsChecked = gs.saveSystemConfigs;
            chkLoadConfigsOnStart.IsChecked = gs.importConfigsOnStart;
            chkEnableConfigToolTips.IsChecked = gs.enableConfigToolTips;

            chkHideCountryFilter.IsChecked = gs.hideCountryFilters;

            chkEnableClearCacheOnExit.IsChecked = gs.enableClearCacheOnExit;

            comboImageTooltipSize.SelectedValue = gs.imageToolTipPercentage;

            //chkAllBaseSettings.IsChecked = gs.showAllBaseSettings;
            //MessageBox.Show(gs.guiZoom.ToString());

            // game scraping options
            chkAllowBanners.IsChecked = gs.scrapeBanners;
            chkAllowBoxart.IsChecked = gs.scrapeBoxart;
            chkAllowFanart.IsChecked = gs.scrapeFanart;
            chkAllowScreenshots.IsChecked = gs.scrapeScreenshots;
            chkPreferGenesis.IsChecked = gs.preferGenesis;
            chkAllowMedia.IsChecked = gs.scrapeMedia;
            chkAllowManuals.IsChecked = gs.scrapeManuals;
            chkSecondaryScraperBackup.IsChecked = gs.enabledSecondaryScraper;

            slScreenshotsPerHost.Value = gs.maxScreenshots;
            slFanrtsPerHost.Value = gs.maxFanarts;

            chkAllowUpdateCheck.IsChecked = gs.checkUpdatesOnStart;

            chkBackupMednafenConfig.IsChecked = gs.backupMednafenConfig;

            chkshowGLYear.IsChecked = gs.showGLYear;
            chkshowGLCoop.IsChecked = gs.showGLCoop;
            chkshowGLDeveloper.IsChecked = gs.showGLDeveloper;
            chkshowGLESRB.IsChecked = gs.showGLESRB;
            chkshowGLPlayers.IsChecked = gs.showGLPlayers;
            chkshowGLPublisher.IsChecked = gs.showGLPublisher;

            chkshowGLYear.Visibility = Visibility.Collapsed;
            chkshowGLCoop.Visibility = Visibility.Collapsed;
            chkshowGLDeveloper.Visibility = Visibility.Collapsed;
            chkshowGLESRB.Visibility = Visibility.Collapsed;
            chkshowGLPlayers.Visibility = Visibility.Collapsed;
            chkshowGLPublisher.Visibility = Visibility.Collapsed;

            if (gs.primaryScraper == 1)
            {
                // thegamesdb
                rbGDB.IsChecked = true;
            }
            if (gs.primaryScraper == 2)
            {
                // moby
                rbMoby.IsChecked = true;
            }



        }

        public static void UpdateshowGLYear(CheckBox chkshowGLYear)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLYear = chkshowGLYear.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateshowGLCoop(CheckBox chkshowGLCoop)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLCoop = chkshowGLCoop.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateshowGLDeveloper(CheckBox chkshowGLDeveloper)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLDeveloper = chkshowGLDeveloper.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateshowGLESRB(CheckBox chkshowGLESRB)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLESRB = chkshowGLESRB.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateshowGLPlayers(CheckBox chkshowGLPlayers)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLPlayers = chkshowGLPlayers.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateshowGLPublisher(CheckBox chkshowGLPublisher)
        {
            GlobalSettings gs = GetGlobals();
            gs.showGLPublisher = chkshowGLPublisher.IsChecked.Value;
            SetGlobals(gs);
        }


        public static void UpdateMaxScreenshots(Slider slScreenshotsPerHost)
        {
            GlobalSettings gs = GetGlobals();
            gs.maxScreenshots = slScreenshotsPerHost.Value;
            SetGlobals(gs);
        }

        public static void UpdateMaxFanarts(Slider slFanrtsPerHost)
        {
            GlobalSettings gs = GetGlobals();
            gs.maxFanarts = slFanrtsPerHost.Value;
            SetGlobals(gs);
        }

        public static void UpdateEnableNetplay(CheckBox EnableNetplay)
        {
            GlobalSettings gs = GetGlobals();
            gs.enableNetplay = EnableNetplay.IsChecked;
            SetGlobals(gs);
        }
        public static void UpdateEnableSnes_faust(CheckBox EnableSnes_faust)
        {
            GlobalSettings gs = GetGlobals();
            gs.enableSnes_faust = EnableSnes_faust.IsChecked;
            SetGlobals(gs);
        }
        public static void UpdateEnablePce_fast(CheckBox EnablePce_fast)
        {
            GlobalSettings gs = GetGlobals();
            gs.enablePce_fast = EnablePce_fast.IsChecked;
            SetGlobals(gs);
        }
        public static void UpdateFullScreen(CheckBox FullScreen)
        {
            GlobalSettings gs = GetGlobals();
            gs.fullScreen = FullScreen.IsChecked;
            SetGlobals(gs);
        }
        public static void UpdateBypassConfig(CheckBox BypassConfig)
        {
            GlobalSettings gs = GetGlobals();
            gs.bypassConfig = BypassConfig.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateGuiZoom(double GuiZoom)
        {
            GlobalSettings gs = GetGlobals();
            gs.guiZoom = GuiZoom;
            SetGlobals(gs);
        }

        public static void UpdatechkAllBaseSettings(CheckBox chkAllBaseSettings)
        {
            GlobalSettings gs = GetGlobals();
            gs.showAllBaseSettings = chkAllBaseSettings.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateMinToTaskBar(CheckBox MinToTaskBar)
        {
            GlobalSettings gs = GetGlobals();
            gs.minToTaskBarOnGameLaunch = MinToTaskBar.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateHideSidebar(CheckBox HideSidebar)
        {
            GlobalSettings gs = GetGlobals();
            gs.hideSidebar = HideSidebar.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateCheckUpdatesOnStart(CheckBox chkAllowUpdateCheck)
        {
            GlobalSettings gs = GetGlobals();
            gs.checkUpdatesOnStart = chkAllowUpdateCheck.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateEnableConfigToolTips(CheckBox chkEnableConfigToolTips)
        {
            GlobalSettings gs = GetGlobals();
            gs.enableConfigToolTips = chkEnableConfigToolTips.IsChecked;
            SetGlobals(gs);
        }

        public static void UpdateEnableClearCacheOnExit(CheckBox chkEnableClearCacheOnExit)
        {
            GlobalSettings gs = GetGlobals();
            gs.enableClearCacheOnExit = chkEnableClearCacheOnExit.IsChecked;
            SetGlobals(gs);
        }

        // scraping options
        public static void UpdateAllowBanners(CheckBox chkAllowBanners)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeBanners = chkAllowBanners.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateAllowBoxart(CheckBox chkAllowBoxart)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeBoxart = chkAllowBoxart.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateAllowScreenshots(CheckBox chkAllowScreenshots)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeScreenshots = chkAllowScreenshots.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateAllowManuals(CheckBox chkAllowManuals)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeManuals = chkAllowManuals.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateAllowMedias(CheckBox chkAllowMedia)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeMedia = chkAllowMedia.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdateAllowFanart(CheckBox chkAllowFanart)
        {
            GlobalSettings gs = GetGlobals();
            gs.scrapeFanart = chkAllowFanart.IsChecked.Value;
            SetGlobals(gs);
        }
        public static void UpdatePreferGenesis(CheckBox chkPreferGenesis)
        {
            GlobalSettings gs = GetGlobals();
            gs.preferGenesis = chkPreferGenesis.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateScraperBackup(CheckBox chkSecondaryScraperBackup)
        {
            GlobalSettings gs = GetGlobals();
            gs.enabledSecondaryScraper = chkSecondaryScraperBackup.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateBackupMednafenConfig(CheckBox chkBackupMednafenConfig)
        {
            GlobalSettings gs = GetGlobals();
            gs.backupMednafenConfig = chkBackupMednafenConfig.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateSaveSysConfigs(CheckBox chkSaveSysConfigs)
        {
            GlobalSettings gs = GetGlobals();
            gs.saveSystemConfigs = chkSaveSysConfigs.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateHideCountryFilters(CheckBox chkHideCountryFilter)
        {
            GlobalSettings gs = GetGlobals();
            gs.hideCountryFilters = chkHideCountryFilter.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateImportConfigsOnStart(CheckBox chkLoadConfigsOnStart)
        {
            GlobalSettings gs = GetGlobals();
            gs.importConfigsOnStart = chkLoadConfigsOnStart.IsChecked.Value;
            SetGlobals(gs);
        }

        public static void UpdateRememberSysWinPositions(CheckBox chkrememberSysWinPositions)
        {
            GlobalSettings gs = GetGlobals();
            gs.rememberSysWinPositions = chkrememberSysWinPositions.IsChecked.Value;
            SetGlobals(gs);
        }

        public static string ConvertPointToString(System.Drawing.Point point)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(point.X);
            sb.Append(",");
            sb.Append(point.Y);

            return sb.ToString();
        }

        public static System.Drawing.Point ConvertStringToPoint(string coords)
        {
            System.Drawing.Point point = new System.Drawing.Point();
            if (coords == null || !coords.Contains(","))
            {
                point.X = 0;
                point.Y = 0;
                return point;
            }

            string[] pArr = coords.Split(',');
            point.X = Convert.ToInt32(pArr[0]);
            point.Y = Convert.ToInt32(pArr[1]);
            return point;
        }

        public static System.Drawing.Point GetWindowPosBySystem(int systemId)
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();

            string[] s = new string[]
            {
                gs.sysWinPos1,
                gs.sysWinPos2,
                gs.sysWinPos3,
                gs.sysWinPos4,
                gs.sysWinPos5,
                gs.sysWinPos6,
                gs.sysWinPos7,
                gs.sysWinPos8,
                gs.sysWinPos9,
                gs.sysWinPos10,
                gs.sysWinPos11,
                gs.sysWinPos12,
                gs.sysWinPos13,
                gs.sysWinPos14,
                gs.sysWinPos15,
                gs.sysWinPos16,
                gs.sysWinPos17,
                gs.sysWinPos18,
            };

            string result = s[systemId - 1];
            return ConvertStringToPoint(result);
        }

        public static void SaveWindowPosBySystem(int systemId, System.Drawing.Point point)
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();

            string coords = ConvertPointToString(point);

            switch (systemId)
            {
                case 1:
                    gs.sysWinPos1 = coords;
                    break;
                case 2:
                    gs.sysWinPos2 = coords;
                    break;
                case 3:
                    gs.sysWinPos3 = coords;
                    break;
                case 4:
                    gs.sysWinPos4 = coords;
                    break;
                case 5:
                    gs.sysWinPos5 = coords;
                    break;
                case 6:
                    gs.sysWinPos6 = coords;
                    break;
                case 7:
                    gs.sysWinPos7 = coords;
                    break;
                case 8:
                    gs.sysWinPos8 = coords;
                    break;
                case 9:
                    gs.sysWinPos9 = coords;
                    break;
                case 10:
                    gs.sysWinPos10 = coords;
                    break;
                case 11:
                    gs.sysWinPos11 = coords;
                    break;
                case 12:
                    gs.sysWinPos12 = coords;
                    break;
                case 13:
                    gs.sysWinPos13 = coords;
                    break;
                case 14:
                    gs.sysWinPos14 = coords;
                    break;
                case 15:
                    gs.sysWinPos15 = coords;
                    break;
                case 16:
                    gs.sysWinPos16 = coords;
                    break;
                case 17:
                    gs.sysWinPos17 = coords;
                    break;
                case 18:
                    gs.sysWinPos18 = coords;
                    break;
            }

            SetGlobals(gs);

        }

        public static void ResetAllSysWindowPositions()
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();

            string coords = "0,0";
            
            gs.sysWinPos1 = coords;
            gs.sysWinPos2 = coords;
            gs.sysWinPos3 = coords;
            gs.sysWinPos4 = coords;
            gs.sysWinPos5 = coords;
            gs.sysWinPos6 = coords;
            gs.sysWinPos7 = coords;
            gs.sysWinPos8 = coords;
            gs.sysWinPos9 = coords;
            gs.sysWinPos10 = coords;
            gs.sysWinPos11 = coords;
            gs.sysWinPos12 = coords;
            gs.sysWinPos13 = coords;
            gs.sysWinPos14 = coords;
            gs.sysWinPos15 = coords;
            gs.sysWinPos16 = coords;
            gs.sysWinPos17 = coords;
            gs.sysWinPos18 = coords;

            SetGlobals(gs);
        }

        public static string[] ReturnFilterArray()
        {
            GlobalSettings gs = GetGlobals();
            string[] arr = new string[]
            {
                gs.glFil01,
                gs.glFil02,
                gs.glFil03,
                gs.glFil04,
                gs.glFil05,
                gs.glFil06,
                gs.glFil07,
                gs.glFil08,
                gs.glFil09,
                gs.glFil10,
                gs.glFil11,
                gs.glFil12,
                gs.glFil13,
                gs.glFil14,
                gs.glFil15,
                gs.glFil16,
                gs.glFil17,
                gs.glFil18,
                gs.glFil19
            };

            return arr;
        }

        public static void WriteFilterAray(string[] filterResults)
        {
            GlobalSettings gs = GetGlobals();
            gs.glFil01 = filterResults[0];
            gs.glFil02 = filterResults[1];
            gs.glFil03 = filterResults[2];
            gs.glFil04 = filterResults[3];
            gs.glFil05 = filterResults[4];
            gs.glFil06 = filterResults[5];
            gs.glFil07 = filterResults[6];
            gs.glFil08 = filterResults[7];
            gs.glFil09 = filterResults[8];
            gs.glFil10 = filterResults[9];
            gs.glFil11 = filterResults[10];
            gs.glFil12 = filterResults[11];
            gs.glFil13 = filterResults[12];
            gs.glFil14 = filterResults[13];
            gs.glFil15 = filterResults[14];
            gs.glFil16 = filterResults[15];
            gs.glFil17 = filterResults[16];
            gs.glFil18 = filterResults[17];
            gs.glFil19 = filterResults[18];

            GlobalSettings.SetGlobals(gs);
        }

    }
}
