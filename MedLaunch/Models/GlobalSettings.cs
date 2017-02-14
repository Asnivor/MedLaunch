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
                glFil02 = "3FFC",
                glFil03 = "3FFC",
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
                glFil19 = "3FFF"
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
            CheckBox chkshowGLYear, CheckBox chkshowGLESRB, CheckBox chkshowGLCoop, CheckBox chkshowGLDeveloper, CheckBox chkshowGLPublisher, CheckBox chkshowGLPlayers, CheckBox chkEnableClearCacheOnExit)
        {
            GlobalSettings gs = GetGlobals();
            // update all checkboxes
            EnableNetplay.IsChecked = gs.enableNetplay;
            EnablePce_Fast.IsChecked = gs.enablePce_fast;
            EnableSnes_Faust.IsChecked = gs.enableSnes_faust;
            //FullScreen.IsChecked = gs.fullScreen;
            //BypassConfig.IsChecked = gs.bypassConfig;

            // update comboboxes
            GuiZoom.SelectedValue = gs.guiZoom.ToString();
            MinToTaskBar.IsChecked = gs.minToTaskBarOnGameLaunch;
            HideSidebar.IsChecked = gs.hideSidebar;

            chkSaveSysConfigs.IsChecked = gs.saveSystemConfigs;
            chkLoadConfigsOnStart.IsChecked = gs.importConfigsOnStart;
            chkEnableConfigToolTips.IsChecked = gs.enableConfigToolTips;

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

        public static void UpdateImportConfigsOnStart(CheckBox chkLoadConfigsOnStart)
        {
            GlobalSettings gs = GetGlobals();
            gs.importConfigsOnStart = chkLoadConfigsOnStart.IsChecked.Value;
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
