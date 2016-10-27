using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using MedLaunch.Models;
using Asnitech.Launch.Common;
using Newtonsoft.Json;
using System.IO;
using FuzzyString;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace MedLaunch.Classes.MasterScraper
{
    // class to generate a master json file to be shipped with each release
    public class CreateMasterJson
    {
        // properties
        public List<ScraperMaster> MasterGames { get; set; }
        public List<GDBPlatformGame> GDBGames { get; set; }
        public List<MobyPlatformGame> MobyGames { get; set; }

        public List<ScraperMaster> WorkingCollection { get; set; }
        public List<ScraperMaster> MatchFound { get; set; }

        public int NoMatches { get; set; }

        public string AppBaseDirectory { get; set; }
        public string MasterJsonPath { get; set; }

        // constructor
        public CreateMasterJson()
        {
            AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Data\System\";
            string GDBJson = File.ReadAllText(AppBaseDirectory + "TheGamesDB.json");
            string MobyJson = File.ReadAllText(AppBaseDirectory + "MobyGames.json");
            MasterGames = new List<ScraperMaster>();
            WorkingCollection = new List<ScraperMaster>();
            MatchFound = new List<ScraperMaster>();

            GDBGames = JsonConvert.DeserializeObject<List<GDBPlatformGame>>(GDBJson);
            MobyGames = JsonConvert.DeserializeObject<List<MobyPlatformGame>>(MobyJson);

            //MasterJsonPath = AppBaseDirectory + ".json"
        }

        // methods

        // Main handler for all processes
        public async void BeginMerge()
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
            var controller = await mw.ShowProgressAsync("Matching Games", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                // first populate the MasterGames object with GDB Games
                PopulateInitial(controller);
            });


            await Task.Run(() =>
            {
                // now go through each game in MasterGames and try and match up with games in the Moby list
                NoMatches = 0;
                MatchMobyToGDB(controller, 0.98);
                // update master
                foreach (var g in MatchFound)
                {
                    AddOrUpdate(g);
                }
                // save json
                string json = JsonConvert.SerializeObject(MasterGames);


                if (WorkingCollection.Count > 100)
                {
                    NoMatches = 0;
                    // do the same again with tighter matching
                    MatchMobyToGDB(controller, 0.98);
                }
                if (WorkingCollection.Count > 50)
                {
                    NoMatches = 0;
                    // do the same again with tighter matching
                    MatchMobyToGDB(controller, 0.95);
                }
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MobyGames Scraper", "Matching Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MobyGames Scraper", "Matching Completed");
            }
        }

        public void PopulateInitial(ProgressDialogController controller)
        {
            foreach (var g in GDBGames)
            {
                ScraperMaster sm = new ScraperMaster();
                sm.GamesDbId = g.id;
                sm.MedLaunchSystemId = g.SystemId;
                sm.TGDBData.GamesDBTitle = g.GameTitle;
                sm.TGDBData.GamesDBPlatformName = g.GDBPlatformName;

                // either add or update this game to the master list
                AddOrUpdate(sm);
            }
        }

        public void MatchMobyToGDB(ProgressDialogController controller, double fuzzyness)
        {
            foreach (var g in MasterGames.Where(a => a.MobyData.MobyTitle != ""))
            {
                controller.SetMessage("Scanning for matches: " + g.TGDBData.GamesDBTitle + "\nMatched: " + MatchFound.Count.ToString() + "\nMultiples: " + WorkingCollection.Count.ToString() + "\nNo Matches: " + NoMatches);
                // get all games for just this system
                var mg = (from a in MobyGames
                                      where a.PlatformName == CG2M(g.TGDBData.GamesDBPlatformName)
                                      select a).ToList();

                List<string> l = new List<string>();
                foreach (var c in mg) { l.Add(c.Title); }
                // try first fuzzy match
                List<string> searchResults = FuzzySearch.Search(g.TGDBData.GamesDBTitle, l, fuzzyness);
                if (searchResults.Count == 1)
                {
                    // single result returned - add to working collection (to be updated later)
                    UpdateScraperMasterRecord(searchResults.Single(), g.GamesDbId);
                    continue;
                }
                if (searchResults.Count == 0)
                {
                    // no results returned
                    NoMatches++;
                    continue;
                }
                if (searchResults.Count > 1)
                {
                    // multiple results - proceed to try and narrow down
                    WorkingCollection.Add(g);
                }
                controller.SetMessage("Scanning for matches: " + g.TGDBData.GamesDBTitle + "\nMatched: " + MatchFound.Count.ToString() + "\nMultiples: " + WorkingCollection.Count.ToString() + "\nNo Matches: " + NoMatches);
            }
        }

        public void UpdateScraperMasterRecord (string gameTitle, int GDBID)
        {
            // get moby games record
            MobyPlatformGame m = (from a in MobyGames
                                 where a.Title == gameTitle
                                 select a).First();

            ScraperMaster s = (from a in MasterGames
                               where a.GamesDbId == GDBID
                               select a).FirstOrDefault();

            s.MobyData.MobyTitle = m.Title;
            s.MobyData.MobyPlatformName = m.PlatformName;
            s.MobyData.MobyURLName = m.UrlName;

            MatchFound.Add(s);

            //WorkingCollection.Add(s);

        }

        public string CG2M(string GDBPlatformName) { return ConvertPlatformNameG2M(GDBPlatformName); }
        public string ConvertPlatformNameG2M(string GDBPlatformName)
        {
            string r = "";
            switch (GDBPlatformName)
            {
                case "Nintendo Game Boy":
                    r = "gameboy";
                    break;
                case "Nintendo Game Boy Color":
                    r = "gameboy-color";
                    break;
                case "Nintendo Game Boy Advance":
                    r = "gameboy-advance";
                    break;
                case "Atari Lynx":
                    r = "lynx";
                    break;
                case "Sega Genesis":
                    r = "genesis";
                    break;
                case "Sega Mega Drive":
                    r = "genesis";
                    break;
                case "Sega Game Gear":
                    r = "game-gear";
                    break;
                case "Neo Geo Pocket":
                    r = "neo-geo-pocket";
                    break;
                case "Neo Geo Pocket Color":
                    r = "neo-geo-pocket-color";
                    break;
                case "TurboGrafx 16":
                    r = "turbo-grafx";
                    break;
                case "TurboGrafx CD":
                    r = "turbografx-cd";
                    break;
                case "PC-FX":
                    r = "pc-fx";
                    break;
                case "Sony Playstation":
                    r = "playstation";
                    break;
                case "Sega Master System":
                    r = "sega-master-system";
                    break;
                case "Nintendo Entertainment System (NES)":
                    r = "nes";
                    break;
                case "Super Nintendo (SNES)":
                    r = "snes";
                    break;
                case "Sega Saturn":
                    r = "sega-saturn";
                    break;
                case "Nintendo Virtual Boy":
                    r = "virtual-boy";
                    break;
                case "WonderSwan":
                    r = "wonderswan";
                    break;
                case "WonderSwan Color":
                    r = "wonderswan-color";
                    break;               
            }

            return r;
        }

        public void AddOrUpdate(ScraperMaster sm)
        {
            // check whether record already exists
            List<ScraperMaster> record = (from a in MasterGames
                         where a.GamesDbId == sm.GamesDbId
                         select a).ToList();
            if (record.Count < 1)
            {
                // no entries returned - add
                MasterGames.Add(sm);
            }
            else
            {
                // 1 or more entries returned - update
                MasterGames.Remove(record.First());
                MasterGames.Add(sm);
            }
        }
    }
}
