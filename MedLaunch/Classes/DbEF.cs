using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MedLaunch.Classes.GamesLibrary;
using System.Windows;
using System.Text.RegularExpressions;

namespace MedLaunch.Classes
{
    class DbEF
    {
        public static object GamesListView { get; private set; }

        public static void InitialSeed()
        {
            // check whether initial seed needs to continue
            bool doSeed = false; 
            using (var db = new MyDbContext())
            {
                var se = db.GlobalSettings.FirstOrDefault();
                if (se == null || se.databaseGenerated == false)
                {
                    doSeed = true;
                }
            }

            if (doSeed == true)
            {
                // populate Versions table
                Versions version = Versions.GetVersionDefaults();
                using (var context = new MyDbContext())
                {
                    context.Versions.Add(version);
                    context.SaveChanges();
                }


                    // default netplay settings
                    ConfigNetplaySettings npSettings = ConfigNetplaySettings.GetNetplayDefaults();
                using (var context = new MyDbContext())
                {
                    context.ConfigNetplaySettings.Add(npSettings);
                    context.SaveChanges();
                }

                // default ConfigBaseSettings population
                ConfigBaseSettings cfbs = ConfigBaseSettings.GetConfigDefaults();

                cfbs.ConfigId = 2000000000; // base configuration

                using (var context = new MyDbContext())
                {
                    context.ConfigBaseSettings.Add(cfbs);
                    context.SaveChanges();
                }

                // create system specific configs (set to disabled by default)
                List<GSystem> gamesystems = GSystem.GetSystems();
                using (var gsContext = new MyDbContext())
                {
                    // iterate through each system and create a default config for them - setting them to disabled, setting their ID to 2000000000 + SystemID
                    // and setting their systemident to systemid
                    foreach (GSystem System in gamesystems)
                    {
                        int def = 2000000000;
                        ConfigBaseSettings c = ConfigBaseSettings.GetConfigDefaults();
                        c.ConfigId = def + System.systemId;
                        c.systemIdent = System.systemId;
                        c.isEnabled = false;

                        // add to databsae
                        gsContext.ConfigBaseSettings.Add(c);
                        gsContext.SaveChanges();
                    }
                }

                // Populate Servers
                List<ConfigServerSettings> servers = ConfigServerSettings.GetServerDefaults();
                using (var context = new MyDbContext())
                {
                    context.ConfigServerSettings.AddRange(servers);
                    context.SaveChanges();
                }

                // Create General Settings Entry
                GlobalSettings gs = GlobalSettings.GetGlobalDefaults();
                using (var context = new MyDbContext())
                {
                    context.GlobalSettings.Add(gs);
                    context.SaveChanges();
                }

                // create mednanet general entry
                MednaNetSettings ms = MednaNetSettings.GetMednaNetDefaults();
                using (var context = new MyDbContext())
                {
                    context.MednaNetSettings.Add(ms);
                    context.SaveChanges();
                }

                // create Paths entry
                Paths paths = new Paths
                {
                    pathId = 1
                };
                using (var context = new MyDbContext())
                {
                    context.Paths.Add(paths);
                    context.SaveChanges();
                }

                // initial seeding complete. mark GeneralSettings table so that regeneration does not occur
                GlobalSettings set;
                using (var context = new MyDbContext())
                {
                    set = (from a in context.GlobalSettings
                           where a.settingsId == 1
                           select a).FirstOrDefault<GlobalSettings>();
                }

                if (set != null)
                {
                    set.databaseGenerated = true;
                }

                using (var dbCtx = new MyDbContext())
                {
                    dbCtx.Entry(set).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
            }
        }

        public static string FormatDate(DateTime dt)
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

        public static string ReturnYear(string str)
        {
            string pattern = @"\d{4}";
            Regex r = new Regex(pattern);
            if (str == null)
                return null;
            Match match = r.Match(str);
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return null;
            }
            //return Regex.Replace(str, pattern, "___").ToString();
        }


        


        public static void GetInfo(int gameID, Label sysLabel, TextBlock sysDesc, Image sysImage)//, Image gameImage)
        {
            // gets game and system info from the database and populates the right information panel

            // get system info first
            using (var context = new MyDbContext())
            {
                var gameInfo = (from g in context.Game
                               where g.gameId == gameID
                               select g).FirstOrDefault();
                List<GSystem> si = GSystem.GetSystems();
                var sysInfo = (from s in si
                               where s.systemId == gameInfo.systemId
                               select new { s.systemName, s.systemDescription, s.systemId }).FirstOrDefault();

                // image handling
                string image = @"Graphics\Icons\na.png";
                if (sysInfo != null)
                { 
                    if (sysInfo.systemId == 10)
                    {
                        // master system
                        image = @"Graphics\Systems\snes.jpg";
                    }
                    else
                    {
                        image = @"Graphics\Icons\na.png";
                    }

                }

                // set system image
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(image, UriKind.Relative);
                b.EndInit();

                // ... Get Image reference from sender.
                //var image = sender as Image;
                // ... Assign Source.
                sysImage.Source = b;
                //sysImage.Source = new BitmapImage(new Uri(image, UriKind.Relative));

                // set system label
                sysLabel.Content = sysInfo.systemName;

                // set system description
                sysDesc.Text = sysInfo.systemDescription;
                
            }
        }

        
    }
}
