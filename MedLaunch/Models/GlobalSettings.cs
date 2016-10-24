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
        public bool? minToTaskBarOnGameLaunch { get; set; }         // whether or not MedLaunch is minimised to taskbar when game is launched
        public bool? hideSidebar { get; set; }                      // always hide the games library sidebar
        public DateTime? gdbLastUpdated { get; set; }               // last time platformgames were synced from thegamesdb.net

        // games library expander states
        public bool glGameStats { get; set; }
        public bool glGameInfo { get; set; }
        public bool glOverview { get; set; }
        public bool glScreenshots { get; set; }
        public bool glFanart { get; set; }
        public bool glScrapingOptions { get; set; }
        public bool glSystemInfo { get; set; }

        // Game scraping options
        public bool scrapeBanners { get; set; }
        public bool scrapeBoxart { get; set; }
        public bool scrapeScreenshots { get; set; }
        public bool scrapeFanart { get; set; }
        public bool preferGenesis { get; set; }

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
                gdbLastUpdated = DateTime.Now,
                glGameStats = true,
                glGameInfo = true,
                glFanart = true,
                glOverview = true,
                glScrapingOptions = true,
                glScreenshots = true,
                glSystemInfo = true,
                scrapeBanners = true,
                scrapeBoxart = true,
                scrapeFanart = true,
                scrapeScreenshots = true,
                preferGenesis = true
                
            };
            return gs;
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
            CheckBox chkAllowBanners, CheckBox chkAllowBoxart, CheckBox chkAllowScreenshots, CheckBox chkAllowFanart, CheckBox chkPreferGenesis)
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
            //MessageBox.Show(gs.guiZoom.ToString());

            // game scraping options
            chkAllowBanners.IsChecked = gs.scrapeBanners;
            chkAllowBoxart.IsChecked = gs.scrapeBoxart;
            chkAllowFanart.IsChecked = gs.scrapeFanart;
            chkAllowScreenshots.IsChecked = gs.scrapeScreenshots;
            chkPreferGenesis.IsChecked = gs.preferGenesis;



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

    }
}
