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
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;

namespace MedLaunch.Classes.MasterScraper
{
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

        public string AppBaseDirectory { get; set; }
        public string MasterJsonPath { get; set; }

        public string MasterGamesJsonPath { get; set; }
        public string WorkingCollectionJsonPath { get; set; }
        public string DuplicatesJsonPath { get; set; }
        public string NoneFoundJsonPath { get; set; }

        public MainWindow mw { get; set; }

        // constructor
        public CreateMasterList()
        {
            AppBaseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Data\System\";
            string GDBJson = File.ReadAllText(AppBaseDirectory + "TheGamesDB.json");
            string MobyJson = File.ReadAllText(AppBaseDirectory + "MobyGames.json");

            //MasterGamesJsonPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + @"\Data\System\MasterGames.json";
            MasterGamesJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\MasterGames.json";
            DuplicatesJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\_duplicates.json";
            NoneFoundJsonPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\_nonefound.json";

            MasterGames = new List<ScraperMaster>();
            WorkingCollection = new List<ScraperMaster>();
            MatchFound = new List<ScraperMaster>();
            NoneFound = new List<ScraperMaster>();
            Duplicates = new List<DuplicateSearchResult>();

            GDBGames = JsonConvert.DeserializeObject<List<GDBPlatformGame>>(GDBJson);
            MobyGames = JsonConvert.DeserializeObject<List<MobyPlatformGame>>(MobyJson);

            // if MasterGames json file doesnt exist create it - otherwise load data from it
            PopulateInitial(null);
            SaveMasterJson();

            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        // methods

        // Main handler for all processes
        public async void BeginMerge()
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

                // first iteration - straight string match
                MatchMobyToGDB(controller, null, 0);

                // second iteration - lev 0.8
                MatchMobyToGDB(controller, null, 0.8);

                // third iteration - lev 0.7
                MatchMobyToGDB(controller, null, 0.7);

                // third iteration - lev 0.6
                MatchMobyToGDB(controller, null, 0.6);

                // third iteration - lev 0.5
                MatchMobyToGDB(controller, null, 0.5);

                // third iteration - lev 0.4
                MatchMobyToGDB(controller, null, 0.4);

                // third iteration - lev 0.3
                MatchMobyToGDB(controller, null, 0.3);

                // third iteration - lev 0.2
                MatchMobyToGDB(controller, null, 0.2);

                // third iteration - lev 0.1
                MatchMobyToGDB(controller, null, 0.1);

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
            controller.SetTitle("Matching pass - fuzzyness: " + fuzzyness);
            MatchFound = new List<ScraperMaster>();
            Duplicates = new List<DuplicateSearchResult>();
            NoneFound = new List<ScraperMaster>();

            if (masterlist == null)
                masterlist = MasterGames;
            foreach (var g in masterlist.Where(a => (a.MobyData.MobyTitle == null)))
            {               
                controller.SetMessage("Scanning for matches: " + g.TGDBData.GamesDBTitle + "\nMatched: " + MatchFound.Count.ToString() + "\nMultiples: " + Duplicates.Count.ToString() + "\nNo Matches: " + NoneFound.Count.ToString());
                // get all games for just this system
                var mg = (from a in MobyGames
                                      where a.PlatformName == CG2M(g.TGDBData.GamesDBPlatformName)
                                      select a).ToList();

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
                    // count matched words to attempt to match
                    MatchWords(g, d);
                    continue;
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

        public void MatchWords(ScraperMaster sm, DuplicateSearchResult d)
        {
            List<MobySearchOrdering> list = OrderByMatchedWords(sm.TGDBData.GamesDBTitle);
            if (list.Count == 0)
                return;
            // get the higest value for matches
            int maxValue = list.Max(t => t.Matches);
            // only accept this value if number of matched words is >= 4
            if (maxValue < 4)
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
            MobyGames.Remove(mobygame.Single());
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
            s = s.Replace(" - ", " ").Replace("_", "").Replace(": ", " ").Replace(" : ", " ").Replace(":", "").Replace("'", "").Trim();
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
                string[] arr = GameScraper.BuildArray(searchstring);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = GameScraper.BuildArray(resultstring);
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
                        sm.IDDBManual = cleanedLinks.First();
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
                        sm.IDDBManual = cleanedLinks.First();
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
}
