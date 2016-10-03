using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                enableSnes_faust = false
            };
            return gs;
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

        public static void LoadGlobalSettings(CheckBox EnableNetplay, CheckBox EnableSnes_Faust, CheckBox EnablePce_Fast)
        {
            GlobalSettings gs = GetGlobals();
            // update all checkboxes
            EnableNetplay.IsChecked = gs.enableNetplay;
            EnablePce_Fast.IsChecked = gs.enablePce_fast;
            EnableSnes_Faust.IsChecked = gs.enableSnes_faust;
            //FullScreen.IsChecked = gs.fullScreen;
            //BypassConfig.IsChecked = gs.bypassConfig;
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

    }
}
