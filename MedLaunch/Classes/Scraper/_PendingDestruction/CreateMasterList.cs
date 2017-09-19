using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using MedLaunch.Models;
using MedLaunch.Common;
using Newtonsoft.Json;
using System.IO;
using FuzzyString;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;

namespace MedLaunch.Classes.MasterScraper
{
    /*
    // class to generate a master json file to be shipped with each release
    public class CreateMasterList
    {
        // properties
        public List<ScraperMaster> MasterGames { get; set; }
        public List<GDBPlatformGame> GDBGames { get; set; }
        public List<MobyPlatformGame> MobyGames { get; set; }

        public List<ScraperMaster> WorkingCollection { get; set; }
        public List<ScraperMaster> MatchFound { get; set; }
        public List<ScraperMaster> NoneFound { get; set; }
        public List<DuplicateSearchResult> Duplicates { get; set; }

        public int NoMatches { get; set; }
        public int Matched { get; set; }

        public string AppBaseDirectory { get; set; }
        public string MasterJsonPath { get; set; }

        public string MobyJson { get; set; }

        public string MasterGamesJsonPath { get; set; }
        public string WorkingCollectionJsonPath { get; set; }
        public string DuplicatesJsonPath { get; set; }
        public string NoneFoundJsonPath { get; set; }

        public MainWindow mw { get; set; }

        // constructor
        public CreateMasterList()
        {
            AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Data\System\";
            string GDBJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\TheGamesDB.json");
            MobyJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\MobyGames.json");

            //MasterGamesJsonPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + @"\Data\System\MasterGames.json";
            MasterGamesJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\MasterGames.json";
            DuplicatesJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\_duplicates.json";
            NoneFoundJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\_nonefound.json";

            MasterGames = new List<ScraperMaster>();
            WorkingCollection = new List<ScraperMaster>();
            MatchFound = new List<ScraperMaster>();
            NoneFound = new List<ScraperMaster>();
            Duplicates = new List<DuplicateSearchResult>();

            Matched = 0;

            GDBGames = JsonConvert.DeserializeObject<List<GDBPlatformGame>>(GDBJson);
            MobyGames = JsonConvert.DeserializeObject<List<MobyPlatformGame>>(MobyJson);

            // if MasterGames json file doesnt exist create it - otherwise load data from it
            PopulateInitial(null);
            SaveMasterJson();

            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        // methods

        // parse replacementdocs manuals into master list
        public async void ParseReplacementDocsManuals()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Matching replacementdocs.com manuals", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                // load all manual data into object
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\replacementdocs-manuals.json"))
                {
                    return;
                }

                string json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\replacementdocs-manuals.json");
                List<ReplacementDocs> manuals = JsonConvert.DeserializeObject<List<ReplacementDocs>>(json);

                // iterate through each manual
                foreach (var m in manuals)
                {
                    // check whether manual link already exists
                    bool updateneeded = true;
                    foreach (string u in m.Urls)
                    {
                        var master = (from a in MasterGames
                                      where a.IDDBManual != null && a.TGDBData.GamesDBPlatformName == m.TGBSystemName && a.IDDBManual.Contains(u)
                                      select a).ToList();
                        if (master.Count > 0)
                            updateneeded = false;
                        else
                            updateneeded = true;
                    }

                    if (updateneeded == false)
                        continue;

                    // get list just for this system
                    var sysList = MasterGames.Where(a => a.TGDBData.GamesDBPlatformName == m.TGBSystemName).ToList();
                    controller.SetMessage("Matching Manuals for Game: " + m.GameName + "\n(" + m.TGBSystemName + ")\n\nMatches Found: " + Matched.ToString());
                    MatchManualRD(sysList, m, controller);
                }

            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Completed");
            }
        }

        // Parse static list of manual links from gamesdatabase.org
        public async void ParseGamesDatabaseOrgManuals()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Matching gamesdatabase.org manuals", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                // parse all the links in the file
                List<string> allLinks = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\gamesdatabase.org-manuals.txt").ToList();
                allLinks.Distinct();

                // iterate through each manual link
                foreach (string s in allLinks)
                {
                    // if entry already exists in master json - skip
                    string test = File.ReadAllText(MasterGamesJsonPath);
                    if (test.Contains(s))
                    {
                        continue;
                    }

                    if (s.Contains("/Nintendo_Game_Boy/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Nintendo Game Boy"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Sega_Master_System/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Sega Master System"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Nintendo_Game_Boy_Color/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Nintendo Game Boy Color"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Atari_Lynx/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Atari Lynx"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Sega_Genesis/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Sega Genesis"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Sega_Game_Gear/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Sega Game Gear"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/NEC_TurboGrafx_16/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "TurboGrafx 16"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/NEC_PC_Engine/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "TurboGrafx 16"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Sony_Playstation/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Sony Playstation"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Nintendo_NES/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Nintendo Entertainment System (NES)"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Nintendo_SNES/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Super Nintendo (SNES)"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Sega_Saturn/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Sega Saturn"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                    if (s.Contains("/Nintendo_Virtual_Boy/"))
                    {
                        List<ScraperMaster> games = (from a in MasterGames
                                                     where a.TGDBData.GamesDBPlatformName == "Nintendo Virtual Boy"
                                                     select a).ToList();
                        MatchManual(games, s);
                    }
                }
            });



            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Completed");
            }
        }

        public void MatchManualRD(List<ScraperMaster> games, ReplacementDocs rd, ProgressDialogController controller)
        {         
            List<ScraperMaster> search = new List<ScraperMaster>();
            List<ManualCount> mcount = new List<ManualCount>();

            // first do exact match
            var exact = (from a in games
                         where a.TGDBData.GamesDBTitle.ToLower().Trim() == rd.GameName.ToLower().Trim()
                         select a).ToList();
            if (exact.Count == 1)
            {
                // exact entry found
                var record = exact.FirstOrDefault();
                if (record.IDDBManual == null)
                    record.IDDBManual = new List<string>();
                record.IDDBManual.AddRange(rd.Urls);
                record.IDDBManual.Distinct();
                AddOrUpdate(record);
                SaveMasterJson();
                return;
            }

            // manual matching based on word count
            string[] lArr = rd.GameName.Trim().Split(' ');
            for (int i = 0; i < lArr.Length; i++)
            {
                var s = games.Where(a => a.TGDBData.GamesDBTitle.ToLower().Trim().Contains(lArr[i].ToLower().Trim()));
                search.AddRange(s);
            }
            // count entries in list
            var q = from x in search
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                ManualCount mc = new ManualCount();
                mc.Game = x.Value;
                mc.Matches = x.Count;
                mcount.Add(mc);
            }

            foreach (var g in mcount.OrderByDescending(a => a.Matches))
            {
                string message = "Manual: \n" + rd.GameName + "\n";
                message += "\nGame:\n " + g.Game.TGDBData.GamesDBTitle + "\n(" + g.Game.TGDBData.GamesDBPlatformName + ")\n";
                message += "\nIs this a match??\n";
                message += "YES - match / NO - nomatch / CANCEL - cancel";
                MessageBoxResult result = MessageBox.Show(message, "Manual Matching", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (result == MessageBoxResult.Yes)
                {
                    // this is a match - update the master json
                    var record = MasterGames.Where(a => a.GamesDbId == g.Game.GamesDbId).FirstOrDefault();
                    if (record.IDDBManual == null)
                        record.IDDBManual = new List<string>();
                    record.IDDBManual.AddRange(rd.Urls);
                    record.IDDBManual.Distinct();
                    AddOrUpdate(record);
                    SaveMasterJson();
                    break;
                }
                if (result == MessageBoxResult.No)
                {
                    // not a match - continue
                    continue;
                }
            }

        }

        public void MatchManual(List<ScraperMaster> games, string url)
        {
            // split the url based on /
            string[] arr = url.Split('/');
            // get the latest item and strip any extra /
            string last = arr.Last().Replace("/", "").Trim();
            // remove .pdf
            last = last.Replace(".pdf", "").Replace("..pdf", "");
            // spacing
            last = last.Replace("_-_", " ").Replace("_", " ").Replace(",", "").Replace("'", "");
            string[] lArr = last.Split(' ');
            
            List<ScraperMaster> search = new List<ScraperMaster>();
            List<ManualCount> mcount = new List<ManualCount>();
            
            for (int i = 0; i < lArr.Length; i++)
            {                
                var s = games.Where(a => a.TGDBData.GamesDBTitle.ToLower().Contains(lArr[i].ToLower()));
                search.AddRange(s);
            }
            // count entries in list
            var q = from x in search
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                ManualCount mc = new ManualCount();
                mc.Game = x.Value;
                mc.Matches = x.Count;
                mcount.Add(mc);
            }   

            foreach (var g in mcount.OrderByDescending(a => a.Matches))
            {
                string message = "Manual: \n" + url + "\n";
                message += "\nGame:\n " + g.Game.TGDBData.GamesDBTitle + "\n(" + g.Game.TGDBData.GamesDBPlatformName + ")\n";
                message += "\nIs this a match??\n";
                message += "YES - match / NO - nomatch / CANCEL - cancel";
                MessageBoxResult result = MessageBox.Show(message, "Manual Matching", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (result == MessageBoxResult.Yes)
                {
                    // this is a match - update the master json
                    var record = MasterGames.Where(a => a.GamesDbId == g.Game.GamesDbId).FirstOrDefault();
                    record.IDDBManual.Add(url);
                    record.IDDBManual.Distinct();
                    AddOrUpdate(record);
                    SaveMasterJson();
                    break;
                }
                if (result == MessageBoxResult.No)
                {
                    // not a match - continue
                    continue;
                }
            }

        }

        // Main handler for all processes
        public async void BeginMerge(bool ManualResolve, bool usewordcountmatching, bool manualEverything)
        {
            // get the main window
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

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
                int masterTotalGames = MasterGames.Count;
                int gdbTotalGames = GDBGames.Count;
                string me = "Adding new thegamesdb.net games to master list...\n" + (gdbTotalGames - masterTotalGames).ToString() + " new games detected..Parsing - Please wait....\n";
                controller.SetMessage(me);
                Task.Delay(1000);
                foreach (var g in GDBGames)
                {
                    var mG = MasterGames.Where(a => a.GamesDbId == g.id).ToList();
                    if (mG.Count > 0)
                        continue;

                    // game not found - add it to master list
                    controller.SetMessage(me + "Adding: " + g.GameTitle + " (" + g.GDBPlatformName + ")");
                    ScraperMaster sm = new ScraperMaster();
                    sm.GamesDbId = g.id;
                    sm.MedLaunchSystemId = g.SystemId;
                    sm.TGDBData.GamesDBTitle = g.GameTitle;
                    sm.TGDBData.GamesDBPlatformName = g.GDBPlatformName;

                    // either add or update this game to the master list (or do nothing if record is unchanged)
                    AddOrUpdate(sm);
                }
            });

                await Task.Run(() =>
            {
                controller.SetMessage("Removing potential results from search context to speed up processing...");
                var matchedR = (from a in MasterGames
                               where a.MobyData.MobyTitle != null
                               select a).ToList();
                int matchedCount = matchedR.Count;
                controller.Maximum = matchedCount;
                controller.Minimum = 0;
                int c = 0;
                foreach (var g in matchedR)
                {
                    c++;
                    controller.SetMessage("Removing potential results from search context to speed up processing...\n(" + c + " of " + matchedCount + ")");
                    controller.SetProgress(Convert.ToDouble(c - 1));
                    MobyPlatformGame mg = (from a in MobyGames
                              where a.UrlName == g.MobyData.MobyURLName
                              select a).FirstOrDefault();
                    if (mg != null)
                    {
                        MobyGames.Remove(mg);
                    }
                }
            });

            await Task.Delay(1000);

                await Task.Run(() =>
            {
                // now go through each game in MasterGames and try and match up with games in the Moby list
                NoMatches = 0;

                if (manualEverything == true)
                {
                    // one exact pass - then manually prompt for everything
                    MatchMobyToGDB(controller, null, 1000);
                }
                else
                {
                    if (usewordcountmatching == true)
                    {
                        // manual interaction selected - no leven
                        // third iteration - lev 0.7
                        MatchMobyToGDB(controller, null, 100, true, true);

                    }

                    else
                    {
                        if (ManualResolve == true)
                        {
                            // manual interaction selected
                            // third iteration - lev 0.7
                            MatchMobyToGDB(controller, null, 0, true, false);
                            MatchMobyToGDB(controller, null, 0.99, true, false);
                            MatchMobyToGDB(controller, null, 0.95, true, false);
                            MatchMobyToGDB(controller, null, 0.93, true, false);
                        }
                        else
                        {
                            // first iteration - straight string match
                            MatchMobyToGDB(controller, null, 0);

                            // second iteration - lev 0.8
                            MatchMobyToGDB(controller, null, 0.99);

                            // second iteration - lev 0.8
                            MatchMobyToGDB(controller, null, 0.95);

                            // second iteration - lev 0.8
                            MatchMobyToGDB(controller, null, 0.93);
                        }
                    }
                }

                // dump unmatched entries to disk
                var masterUnmatched = (from a in MasterGames
                                      where a.MobyData.MobyTitle == null
                                      select a).ToList();
                string masterUnmatchedJson = JsonConvert.SerializeObject(masterUnmatched, Formatting.Indented);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\_masterUnMatched.json", masterUnmatchedJson);

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

        public void LoadMasterJson()
        {
            string json = File.ReadAllText(MasterGamesJsonPath);
            MasterGames = JsonConvert.DeserializeObject<List<ScraperMaster>>(json);
        }
        public void SaveMasterJson()
        {
            string json = JsonConvert.SerializeObject(MasterGames, Formatting.Indented);
            File.WriteAllText(MasterGamesJsonPath, json);
        }

        public void PopulateInitial(ProgressDialogController controller)
        {
            if (File.Exists(MasterGamesJsonPath))
            {
                // file exists - populate object from file
                LoadMasterJson();
            }

            foreach (var g in GDBGames)
            {
                ScraperMaster sm = new ScraperMaster();
                sm.GamesDbId = g.id;
                sm.MedLaunchSystemId = g.SystemId;
                sm.TGDBData.GamesDBTitle = g.GameTitle;
                sm.TGDBData.GamesDBPlatformName = g.GDBPlatformName;

                // either add or update this game to the master list (or do nothing if record is unchanged)
                AddOrUpdate(sm);
            }
        }

        public void UpdateMasterWithFound()
        {
            foreach (ScraperMaster sm in MatchFound)
            {
                AddOrUpdate(sm);
            }
        }

        public void MatchMobyToGDB(ProgressDialogController controller, List<ScraperMaster> masterlist, double fuzzyness)
        {
            MatchMobyToGDB(controller, masterlist, fuzzyness, false, false);
        }

        public void MatchMobyToGDB(ProgressDialogController controller, List<ScraperMaster> masterlist, double fuzzyness, bool manualMultipleMatch, bool manualNonLeven)
        {
            controller.SetTitle("Matching pass - fuzzyness: " + fuzzyness);
            MatchFound = new List<ScraperMaster>();
            Duplicates = new List<DuplicateSearchResult>();
            NoneFound = new List<ScraperMaster>();

            if (masterlist == null)
                masterlist = MasterGames.ToList();
            foreach (var g in masterlist.Where(a => (a.MobyData.MobyTitle == null)))
            {               
                controller.SetMessage("Scanning for matches: " + g.TGDBData.GamesDBTitle + "\nMatched: " + MatchFound.Count.ToString() + "\nMultiples: " + Duplicates.Count.ToString() + "\nNo Matches: " + NoneFound.Count.ToString());
                // get all games for just this system
                var mg = (from a in MobyGames
                                      where a.PlatformName == CG2M(g.TGDBData.GamesDBPlatformName)
                                      select a).ToList();

                

                // non leven manual matching
                if (manualNonLeven == true)
                {
                    // get full mobygames list (in case of error)
                    MobyGames = (JsonConvert.DeserializeObject<List<MobyPlatformGame>>(MobyJson)).ToList();
                    var mgf = (from a in MobyGames
                             where a.PlatformName == CG2M(g.TGDBData.GamesDBPlatformName)
                             select a).ToList();
                    List<MobySearchOrdering> se = OrderByMatchedWordsManual(g.TGDBData.GamesDBTitle, mgf);
                    foreach (MobySearchOrdering m in se)
                    {
                        string message = g.TGDBData.GamesDBTitle + "\nPlatform: " + g.TGDBData.GamesDBPlatformName + "\n\n\nIs the following search result a match?:\n\n";
                        message += m.Game.Title + "\n(" + m.Game.PlatformName + ")\n\n";

                        MessageBoxResult result = MessageBox.Show(message, "MobyGames - Manual Matching (based on word count)", MessageBoxButton.YesNoCancel);

                        if (result == MessageBoxResult.Cancel)
                        {
                            // cancel selected
                            break;
                        }
                        if (result == MessageBoxResult.No)
                        {
                            // not the result we need - continue
                            continue;
                        }
                        if (result == MessageBoxResult.Yes)
                        {
                            // save this entry - update to file as we go
                            AddMobyDataToMatchFound(m.Game, g);
                            UpdateMasterWithFound();
                            SaveMasterJson();
                            MatchFound = new List<ScraperMaster>();
                            break;
                        }
                    }
                    continue;
                }

                List<MobyPlatformGame> searchResults = FuzzySearch.Search(g.TGDBData.GamesDBTitle, mg, fuzzyness);

                if (searchResults.Count == 1)
                {
                    // single result returned - add to working collection (to be updated later)
                    AddMobyDataToMatchFound(searchResults, g);
                    continue;
                }
                if (searchResults.Count == 0)
                {
                    // no results returned
                    NoMatches++;
                    NoneFound.Add(g);
                    continue;
                }
                if (searchResults.Count > 1)
                {
                    // multiple results - proceed to try and narrow down
                    DuplicateSearchResult d = new DuplicateSearchResult();
                    d.scraperMaster = g;
                    d.mobyPlatformGames.AddRange(searchResults);
                    Duplicates.Add(d);
                    if (manualMultipleMatch == false)
                    {
                        // count matched words to attempt to match
                        //MatchWords(g, d);
                        continue;
                    }
                    else
                    {

                        // manual match based on levenshtein
                        foreach (var search in searchResults)
                        {
                            string message = g.TGDBData.GamesDBTitle + "\nPlatform: " + g.TGDBData.GamesDBPlatformName + "\n\n\nIs the following search result a match?:\n\n";
                            message += search.Title + "\n(" + search.PlatformName + ")\n\n";

                            MessageBoxResult result = MessageBox.Show(message, "MobyGames - Manual Matching", MessageBoxButton.YesNoCancel);

                            if (result == MessageBoxResult.Cancel)
                            {
                                // cancel selected
                                return;
                            }
                            if (result == MessageBoxResult.No)
                            {
                                // not the result we need - continue
                                continue;
                            }
                            if (result == MessageBoxResult.Yes)
                            {
                                // save this entry - update to file as we go
                                AddMobyDataToMatchFound(search, g);
                                UpdateMasterWithFound();
                                SaveMasterJson();
                                MatchFound = new List<ScraperMaster>();
                                break;
                            }
                        }

                    }
                    
                }
                //controller.SetMessage("Scanning for matches: " + g.TGDBData.GamesDBTitle + "\nMatched: " + MatchFound.Count.ToString() + "\nMultiples: " + WorkingCollection.Count.ToString() + "\nNo Matches: " + NoMatches);
            }

            // save various to json
            UpdateMasterWithFound();
            SaveMasterJson();

            string nonejson = JsonConvert.SerializeObject(NoneFound, Formatting.Indented);
            File.WriteAllText(NoneFoundJsonPath, nonejson);
            string duplicatejson = JsonConvert.SerializeObject(Duplicates, Formatting.Indented);
            File.WriteAllText(DuplicatesJsonPath, duplicatejson);
            
        }

        public async Task ManualSelection(ProgressDialogController controller, List<MobyPlatformGame> searchResults, ScraperMaster game)
        {
            var myDialogSettings = new MetroDialogSettings()
            {
                NegativeButtonText = "NO - Not a Match",
                AnimateShow = false,
                AnimateHide = false,
                AffirmativeButtonText = "YES - Match",
                FirstAuxiliaryButtonText = "CANCEL"
            };

            foreach (var search in searchResults)
            {
                string message = "Game: " + game.TGDBData.GamesDBTitle + "\nPlatform: " + game.TGDBData.GamesDBPlatformName + "\n\n\nIs the following search result a match?:\n\n";
                message += search.Title + "\n(" + search.PlatformName + ")\n\n";
                
                var result = await mw.ShowMessageAsync("MobyGames - Manual Matching", message, MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, myDialogSettings);

                if (result == MessageDialogResult.FirstAuxiliary)
                {
                    // cancel selected
                    return;
                }
                if (result == MessageDialogResult.Negative)
                {
                    // not the result we need - continue
                    continue;
                }
                if (result == MessageDialogResult.Affirmative)
                {
                    // save this entry - update to file as we go
                    AddMobyDataToMatchFound(search, game);
                    UpdateMasterWithFound();
                    SaveMasterJson();
                    MatchFound = new List<ScraperMaster>();
                    return;
                }
            }

            
        }

        public void MatchWordsManual(ScraperMaster sm, DuplicateSearchResult d)
        {
            List<MobySearchOrdering> list = OrderByMatchedWords(sm.TGDBData.GamesDBTitle);
            if (list.Count == 0)
                return;
            // get the higest value for matches
            int maxValue = list.Max(t => t.Matches);
            // only accept this value if number of matched words is >= 4
            if (maxValue < 3)
                return;
            // get only results that have this value
            var filtList = (from a in list
                            where a.Matches == maxValue
                            select a).ToList();
            // check how many results returned
            if (filtList.Count == 0)
            {
                // 0 returned
                return;
            }


            if (filtList.Count > 1)
            {

            }

            /*
            if (filtList.Count == 1)
            {
                // only one result - add this
                sm.MobyData.MobyPlatformName = filtList.Single().Game.PlatformName;
                sm.MobyData.MobyTitle = filtList.Single().Game.Title;
                sm.MobyData.MobyURLName = filtList.Single().Game.UrlName;

                MatchFound.Add(sm);
                Duplicates.Remove(d);

            }
            */
            /*
        }

        public void MatchWords(ScraperMaster sm, DuplicateSearchResult d)
        {
            List<MobySearchOrdering> list = OrderByMatchedWords(sm.TGDBData.GamesDBTitle);
            if (list.Count == 0)
                return;
            // get the higest value for matches
            int maxValue = list.Max(t => t.Matches);
            // only accept this value if number of matched words is >= 4
            if (maxValue < 3)
                return;
            // get only results that have this value
            var filtList = (from a in list
                           where a.Matches == maxValue
                           select a).ToList();
            // check how many results returned
            if (filtList.Count == 0)
            {
                // 0 returned
                return;
            }
            if (filtList.Count == 1)
            {
                // only one result - add this
                sm.MobyData.MobyPlatformName = filtList.Single().Game.PlatformName;
                sm.MobyData.MobyTitle = filtList.Single().Game.Title;
                sm.MobyData.MobyURLName = filtList.Single().Game.UrlName;

                MatchFound.Add(sm);
                Duplicates.Remove(d);
                
            }
        }

        public void AddMobyDataToMatchFound(List<MobyPlatformGame> mobygame, ScraperMaster masterrecord)
        {
            ScraperMaster sm = masterrecord;
            sm.MobyData.MobyPlatformName = mobygame.Single().PlatformName;
            sm.MobyData.MobyTitle = mobygame.Single().Title;
            sm.MobyData.MobyURLName = mobygame.Single().UrlName;
            MatchFound.Add(sm);

            // remove from MobyGames list to speed up subsequent searches
            //MobyGames.Remove(mobygame.Single());
        }

        public void AddMobyDataToMatchFound(MobyPlatformGame mobygame, ScraperMaster masterrecord)
        {
            ScraperMaster sm = masterrecord;
            sm.MobyData.MobyPlatformName = mobygame.PlatformName;
            sm.MobyData.MobyTitle = mobygame.Title;
            sm.MobyData.MobyURLName = mobygame.UrlName;
            MatchFound.Add(sm);

            // remove from MobyGames list to speed up subsequent searches
            //MobyGames.Remove(mobygame);
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

        public string StripSymbols(string i)
        {
            // remove all (xxx), [xxx] 
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            // add this to the class
            
            // remove all - : _ '
            s = s.Replace(" - ", " ").Replace("-", " ").Replace("_", "").Replace(": ", " ").Replace(" : ", " ").Replace(":", " ").Replace("'", "").Trim();
            // remove all roman numerals
            /*
            s.Replace(" I", " ");
            s.Replace(" II ", " ").Replace(" II", " ");
            s.Replace(" III ", " ").Replace(" III", " ");
            s.Replace(" IV ", " ").Replace(" IV", " ");
            s.Replace(" V ", " ");
            s.Replace(" VI ", " ").Replace(" VI", " ");
            s.Replace(" VII ", " ").Replace(" VII", " ");
            s.Replace(" VIII ", " ").Replace(" VIII", " ");
            s.Replace(" IX ", " ").Replace(" IX", " ");
            s.Replace(" X ", " ");
            s.Replace(" XI ", " ").Replace(" XI", " ");
            s.Replace(" XII ", " ").Replace(" XII", " ");

            // replace ending numbers
            string[] arr = BuildArray(s);
            string l = arr[arr.Length - 1];
            foreach (char c in l)
            {
                if (char.IsDigit(c))
                {
                    arr = arr.Take(arr.Count() - 1).ToArray();
                    break;
                }
            }
            s = BuildSearchString(arr);
            */
            /*
            return s;
        }

        public List<MobySearchOrdering> OrderByMatchedWords(string searchStr)
        {
            //Dictionary<GDBPlatformGame, int> totals = new Dictionary<GDBPlatformGame, int>();
            List<MobySearchOrdering> totals = new List<MobySearchOrdering>();

            foreach (MobyPlatformGame g in MobyGames)
            {
                // sanitise
                string searchstring = StripSymbols(searchStr).ToLower();
                string resultstring = StripSymbols(g.Title).ToLower();

                int matchingWords = 0;

                // get total substrings in search string
                string[] arr = ScraperMainSearch.BuildArray(searchstring);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = ScraperMainSearch.BuildArray(resultstring);
                int resultLength = rArr.Length;

                // find matching words
                foreach (string s in arr)
                {
                    int i = 0;
                    while (i < resultLength)
                    {
                        if (StripSymbols(s) == StripSymbols(rArr[i]).ToLower())
                        {
                            matchingWords++;
                            break;
                        }
                        i++;
                    }
                }
                // add to dictionary with count
                MobySearchOrdering so = new MobySearchOrdering();
                so.Game = g;
                so.Matches = matchingWords;
                totals.Add(so);
            }

            // remove all entries with a count of 0
            totals = totals.Where(a => a.Matches != 0).ToList();

            // order list
            totals = totals.OrderByDescending(a => a.Matches).ToList();



            return totals;
        }

        public List<MobySearchOrdering> OrderByMatchedWordsManual(string searchStr, List<MobyPlatformGame> gamelist)
        {
            //Dictionary<GDBPlatformGame, int> totals = new Dictionary<GDBPlatformGame, int>();
            List<MobySearchOrdering> totals = new List<MobySearchOrdering>();

            foreach (MobyPlatformGame g in gamelist)
            {
                // sanitise
                string searchstring = StripSymbols(searchStr).ToLower();
                string resultstring = StripSymbols(g.Title).ToLower();

                int matchingWords = 0;

                // get total substrings in search string
                string[] arr = ScraperMainSearch.BuildArray(searchstring);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = ScraperMainSearch.BuildArray(resultstring);
                int resultLength = rArr.Length;

                // find matching words
                foreach (string s in arr)
                {
                    int i = 0;
                    while (i < resultLength)
                    {
                        if (StripSymbols(s) == StripSymbols(rArr[i]).ToLower())
                        {
                            matchingWords++;
                            break;
                        }
                        i++;
                    }
                }
                // add to dictionary with count
                MobySearchOrdering so = new MobySearchOrdering();
                so.Game = g;
                so.Matches = (matchingWords / arr.Length) * 100;
                //so.Matches = matchingWords;
                
                 totals.Add(so);
            }

            // remove all entries with a count of 0
            totals = totals.Where(a => a.Matches != 0).ToList();

            // order list
            totals = totals.OrderByDescending(a => a.Matches).ToList();



            return totals;
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
            if (record.Count == 1)
            {
                // 1 entries returned - update
                ScraperMaster r = record.Single();
                if (sm.Equals(r))
                {
                    // records match - do nothing
                }
                else
                {
                    if (r.IDDBManual != null && sm.IDDBManual == null)
                        sm.IDDBManual = r.IDDBManual;
                    if (r.TGDBData.GamesDBTitle != null && sm.TGDBData.GamesDBTitle == null)
                        sm.TGDBData.GamesDBTitle = r.TGDBData.GamesDBTitle;
                    if (r.MobyData.MobyTitle != null && sm.MobyData.MobyTitle == null)
                        sm.MobyData.MobyTitle = r.MobyData.MobyTitle;
                    if (r.MobyData.MobyURLName != null && sm.MobyData.MobyURLName == null)
                        sm.MobyData.MobyURLName = r.MobyData.MobyURLName;
                    if (r.MobyData.MobyPlatformName != null && sm.MobyData.MobyPlatformName == null)
                        sm.MobyData.MobyPlatformName = r.MobyData.MobyPlatformName;
                    /*
                    // update changed fields where neccessary
                    if (sm.IDDBManual != null)
                        sm.IDDBManual = r.IDDBManual;
                    if (!sm.TGDBData.GamesDBTitle.Equals(r.TGDBData.GamesDBTitle) && (sm.TGDBData.GamesDBTitle == "" || sm.TGDBData.GamesDBTitle == null))
                        sm.TGDBData.GamesDBTitle = r.TGDBData.GamesDBTitle;
                    if (!sm.MobyData.MobyTitle.Equals(r.MobyData.MobyTitle) && (sm.MobyData.MobyTitle == "" || sm.MobyData.MobyTitle == null))
                        sm.MobyData.MobyTitle = r.MobyData.MobyTitle;
                    if (!sm.MobyData.MobyURLName.Equals(r.MobyData.MobyURLName) && (sm.MobyData.MobyURLName == "" || sm.MobyData.MobyURLName == null))
                        sm.MobyData.MobyURLName = r.MobyData.MobyURLName;
                        */
                        /*
                }
                MasterGames.Remove(r);
                MasterGames.Add(sm);                
            }
        }

        public async void ScrapeManuals()
        {
            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Scraping Manual URLs", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            controller.SetMessage("Removing records from search context to speed up processing...");
            var matchedR = (from a in MasterGames
                            where a.IDDBManual == null
                            select a).ToList();
            int found = 0;
            int notfound = 0;
            int multiples = 0;
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                int count = matchedR.Count;
                int c = 0;
               
                // iterate through each record and search google for the PDF manual URL
                foreach (ScraperMaster sm in matchedR)
                {
                    c++;
                    string message = "Searching for a PDF link for: " + sm.TGDBData.GamesDBTitle + " (" + sm.TGDBData.GamesDBPlatformName + ")";
                    controller.SetMessage(message);
                    controller.SetMessage(message + "\nFound: " + found + "\nNot Found: " + notfound + "\nMultiples: " + multiples);
                    controller.Minimum = 0;
                    controller.Maximum = count;
                    controller.SetProgress(Convert.ToDouble(c));
                    string url = BuildGoogleSearchQuery(sm.TGDBData.GamesDBPlatformName, sm.TGDBData.GamesDBTitle);
                    WebOps wo = new WebOps();
                    wo.BaseUrl = url;
                    wo.Timeout = 10000;
                    string response = wo.ApiCall();

                    if (response.Contains("Your search - ") && response.Contains(" - did not match any documents"))
                    {
                        //MessageBox.Show("No results returned");
                        notfound++;
                        continue;
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(response);
                    // get all links on the page
                    List<string> allLinks = new List<string>();
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        allLinks.Add(att.Value);
                    }
                    List<string> pdfLinks =  allLinks.Where(a => (a.ToLower().Contains(".pdf") && a.ToLower().Contains(@"url?q=http://www.gamesdatabase.org"))).ToList();
                    List<string> cleanedLinks = new List<string>();
                    foreach (string s in pdfLinks)
                    {
                        string t = s.Replace(@"/url?q=", "");
                        string[] arr = t.Split(new string[] { ".pdf" }, StringSplitOptions.None);
                        cleanedLinks.Add(arr[0] + ".pdf");
                    }

                    if (cleanedLinks.Count == 1)
                    {
                        // single entry found - add game
                        sm.IDDBManual.Add(cleanedLinks.First());
                        sm.IDDBManual.Distinct();
                        MatchFound.Add(sm);
                        UpdateMasterWithFound();
                        found++;
                    }
                    if (cleanedLinks.Count > 1)
                    {
                        // single entry found - add game
                        //sm.IDDBManual = cleanedLinks.First();
                        //MatchFound.Add(sm);
                        multiples++;
                        sm.IDDBManual.AddRange(cleanedLinks);
                        sm.IDDBManual.Distinct();
                        MatchFound.Add(sm);
                        UpdateMasterWithFound();
                    }
                    if (cleanedLinks.Count == 0)
                    {
                        // single entry found - add game
                        //sm.IDDBManual = cleanedLinks.First();
                        //MatchFound.Add(sm);
                    }

                    

                }

                controller.SetMessage("Updating....");
                //UpdateMasterWithFound();
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MobyGames Scraper", "Matching Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MobyGames Scraper", "Matching Completed\nFound: " + found + "\nNot Found: " + notfound + "\nMultiples: " + multiples);
            }
        }

        public string BuildGoogleSearchQuery(string platform, string gametitle)
        {
            string b = @"https://www.google.co.uk/search?q=filetype:pdf site:www.gamesdatabase.org manual """ + platform + @""" """ + gametitle + @"""";

            string rep = b.Replace(" ", "+").Replace(@"""", "%22");
            return rep;
            
        }
    }

    public class DuplicateSearchResult
    {
        public ScraperMaster scraperMaster { get; set; }
        public List<MobyPlatformGame> mobyPlatformGames { get; set; }

        public DuplicateSearchResult()
        {
            mobyPlatformGames = new List<MobyPlatformGame>();
        }
    }

    public class ManualCount
    {
        public ScraperMaster Game { get; set; }
        public int Matches { get; set; }
    }
    */
}
