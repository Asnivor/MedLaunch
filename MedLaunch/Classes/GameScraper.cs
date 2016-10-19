using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using MedLaunch.Classes.TheGamesDB;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using FuzzyString;
using System.Windows;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using MahApps.Metro.SimpleChildWindow;


namespace MedLaunch.Classes
{
    public class GameScraper
    {
        // Properties
        public List<GDBPlatformGame> PlatformGames { get; set; }
        public List<GDBPlatformGame> SystemCollection { get; set; }
        public List<GDBPlatformGame> WorkingSearchCollection { get; set; }
        public List<GDBPlatformGame> SearchCollection { get; set; }
        public List<MedLaunch.Models.Game> LocalGames { get; set; }
        public string SearchString { get; set; }
        public bool LocalGameFound { get; set; }
        public int LocalIterationCount { get; set; }
        public int ManualIterator { get; set; }
        public int CurrentLocalGameId { get; set; }

        // Constructors
        public GameScraper()
        {
            RefreshPlatformGamesFromDb();
            LocalGameFound = false;
            LocalIterationCount = 0;
            ManualIterator = 0;
            SearchCollection = new List<GDBPlatformGame>();
            WorkingSearchCollection = new List<GDBPlatformGame>();
            
        }

        // Methods

        public async static void ScrapeGames(int systemId)
        {
            // instantiate instance of this class
            GameScraper gs = new GameScraper();

            // get mainwindow 
            MainWindow MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // popup progress dialog
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await MWindow.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                controller.SetMessage("Getting Local Games");

                // get all local games for this system already in the database
                gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).ToList();

                // get all games that have been matched but not yet scraped.
                List<GDBLink> link = new List<GDBLink>();
                using (var context = new MyDbContext())
                {
                    link = context.GDBLink.ToList();
                }               

                if (link.Count > 0)
                {
                    foreach (var g in link)
                    {
                        var gam = (from a in gs.LocalGames
                                  where (a.gameId == g.GameId && a.systemId == systemId)
                                  select a).ToList();
                        // remove these games so they are not re-scanned
                        foreach (var gj in gam)
                        {
                            gs.LocalGames.Remove(gj);
                        }
                    }
                }



                // count number of games to scan for
                int numGames = gs.LocalGames.Count;
                controller.Minimum = 0;
                controller.Maximum = numGames;
                int i = 0;
                // iterate through each local game and attempt to match it with the gamesdb.net list
                foreach (var g in gs.LocalGames)
                {
                    i++;
                    controller.SetProgress(i);
                    controller.SetMessage("Attempting gamesdb.net match for:\n" + g.gameName + "\n(" + i + " of " + numGames + ")");
                    List<GDBPlatformGame> results = gs.SearchGameLocal(g.gameName, systemId, g.gameId).ToList();
                                       
                    if (results.Count == 0)
                    {
                        // no results returned
                    }
                    if (results.Count == 1)
                    {
                        // one result returned - create / update record in data table
                        GDBPlatformGame plat =  results.ToList().Single();
                        GDBGameData data = new GDBGameData();
                        data.GdbId = plat.id;
                        data.Title = plat.GameTitle;
                        data.ReleaseDate = plat.ReleaseDate;
                        GDBGameData.SaveToDatabase(data);

                        // create entry (or update existing) in the GDBLink table
                        GDBLink l = new GDBLink();
                        l.GdbId = data.GdbId;
                        l.GameId = g.gameId;
                        GDBLink.SaveToDatabase(l);
                    }
                }

                /* Begin actual scraping */

                // Get all games that have been matched but still require scraping
                List<GDBGameData> ga = new List<GDBGameData>();
                controller.SetMessage("Calculating games that require scraping...");
                using (var db = new MyDbContext())
                {
                    ga = (from a in db.GDBGameData
                          where (a.Platform != null || a.Platform != "" || a.Overview != null || a.Overview != "")
                          select a).ToList();
                }
                if (ga.Count == 0)
                {
                    return;
                }

                // now limit to just the systemId that we have specified

                // reset localgames
                gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).ToList();
                List<GDBGameData> gb = new List<GDBGameData>();
                gb = ga.ToList();
                foreach (GDBGameData g in ga)
                {
                    int gdbId = g.GdbId;
                    // lookup in the link table
                    var li = link.Where(a => a.GdbId == gdbId).FirstOrDefault(); 
                    if (li != null)
                    {
                        var actualGame = gs.LocalGames.Where(a => a.gameId == li.GameId).FirstOrDefault();
                        if (actualGame == null || actualGame.systemId != systemId)
                        {
                            // we dont want this game
                            gb.Remove(g);
                        }
                    }                   
                    
                }
                ga = gb.ToList();

                int gamesCount = ga.Count;
                i = 0;
                controller.Minimum = 0;
                controller.Maximum = gamesCount;
                foreach (GDBGameData game in ga)
                {
                    // iterate through each game that requires scraping and attempt to download the data and import to database
                    i++;
                    controller.SetProgress(i);
                    string message = "Connecting to thegamesdb.net....\nGetting data for: " + game.Title + "\n(" + i + " of " + gamesCount + ")";
                    controller.SetMessage(message);
                    gs.ScrapeGame(game.GdbId, controller, message);
                }


            });


            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
            }
            else
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
            }

        }

        public void ScrapeGame(int GdbId, ProgressDialogController p, string currentMessage)
        {
            // does a json file already exist locally?
            if (DoesJsonFileExist(GdbId) ==  true)
            {
                // local json store of the class data already exists - use this instead
                GDBGameData newG = LoadJson(GdbId);
                GDBGameData.SaveToDatabase(newG);
                return;
            }
            // attempt connection
            try
            {
                GDBNETGame g = GDBNETGamesDB.GetGame(GdbId);
                if (g == null)
                {
                    // return
                    return;
                }
                // populate object
                GDBGameData gd = new GDBGameData();
                gd.GdbId = g.ID;
                gd.Overview = g.Overview;
                gd.Title = g.Title;
                gd.Platform = g.Platform;
                gd.ReleaseDate = g.ReleaseDate;
                gd.ESRB = g.ESRB;
                gd.Players = g.Players;
                gd.Publisher = g.Publisher;
                gd.Developer = g.Developer;
                gd.Rating = g.Rating;
                gd.Coop = g.Coop;
                gd.AlternateTitles = GDBGameData.JsonSerialize(g.AlternateTitles);
                gd.Genres = GDBGameData.JsonSerialize(g.Genres);

                // create folder structure for images (if it is needed)
                CreateFolderStructure(g.ID);               

                // download images
                var images = g.Images;
                if (images.BoxartFront == null) { }
                else
                {
                    gd = DownloadFile(images.BoxartFront.Path, g.ID, gd);
                    p.SetMessage(currentMessage + "\nDownloading BoxArt");
                }
                if (images.BoxartBack == null) { }
                else { gd = DownloadFile(images.BoxartBack.Path, g.ID, gd); p.SetMessage(currentMessage + "\nDownloading BoxArt"); }
                if (images.Screenshots.Count > 0)
                {
                    List<string> s1 = new List<string>();
                    foreach (var im in images.Screenshots)
                    {
                        string[] fArr = im.Path.Split('/');
                        string filename = fArr[fArr.Length - 1];
                        gd = DownloadFile(im.Path, g.ID, gd);
                        s1.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\Screenshots\\" + filename);
                        p.SetMessage(currentMessage + "\nDownloading Screenshots");
                    }
                    gd.ScreenshotLocalImages = GDBGameData.JsonSerialize(s1);
                }
                if (images.Fanart.Count > 0)
                {
                    List<string> s2 = new List<string>();
                    foreach (var im in images.Fanart)
                    {
                        string[] fArr = im.Path.Split('/');
                        string filename = fArr[fArr.Length - 1];
                        gd = DownloadFile(im.Path, g.ID, gd);
                        s2.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\FanArt\\" + filename);
                        p.SetMessage(currentMessage + "\nDownloading FanArt");
                    }
                    gd.FanartLocalImages = GDBGameData.JsonSerialize(s2);
                }
                if (images.Banners.Count > 0)
                {
                    List<string> s3 = new List<string>();
                    foreach (var im in images.Banners)
                    {
                        string[] fArr = im.Path.Split('/');
                        string filename = fArr[fArr.Length - 1];
                        gd = DownloadFile(im.Path, g.ID, gd);
                        s3.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\Banners\\" + filename);
                        p.SetMessage(currentMessage + "\nDownloading Banners");
                    }
                    gd.BannerLocalImages = GDBGameData.JsonSerialize(s3);
                }
                
                // now everything is downloaded, update the database and set the DateTime
                gd.LastScraped = DateTime.Now;
                

                GDBGameData.SaveToDatabase(gd);

                // export to Json file
                SaveJson(gd);


            }
            catch  (Exception ex)
            {

            } finally { }
        }

        public static bool DoesJsonFileExist(int gdbId)
        {
            string loadPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + gdbId + @"\" + gdbId + ".json";
            if (File.Exists(loadPath))
            {
                return true;
            }
            return false;
        }
        public static void SaveJson(GDBGameData j)
        {
            string savePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + j.GdbId + @"\" + j.GdbId + ".json";            
            string json = JsonConvert.SerializeObject(j);
            System.IO.File.WriteAllText(savePath, json);
        }
        public static GDBGameData LoadJson(int gdbId)
        {
            GDBGameData g = new GDBGameData();
            string loadPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + gdbId + @"\" + gdbId + ".json";
            if (File.Exists(loadPath))
            {
                string json = System.IO.File.ReadAllText(loadPath);
                g = JsonConvert.DeserializeObject<GDBGameData>(json);
            }
            return g;            
        }

        public static GDBGameData DownloadFile(string url, int gdbId, GDBGameData gd)
        {
            // decide where it should be saved locally
            string lPath = @"Data\Graphics\thegamesdb\" + gdbId.ToString() + @"\";
            string local = "";
            string[] fArr = url.Split('/');
            string filename = fArr[fArr.Length - 1];
            if (url.Contains("boxart/original/back")) { local = lPath + @"boxartback\" + filename; }
            if (url.Contains("boxart/original/front")) { local = lPath + @"boxartfront\" + filename; }
            if (url.Contains("fanart/")) { local = lPath + @"fanart\" + filename; }
            if (url.Contains("screenshots/")) { local = lPath + @"screenshots\" + filename; }
            if (url.Contains("banners/graphical")) { local = lPath + @"banners\" + filename; }

            // if localpath is "" return
            if (local == "") { return gd; }
            using (WebClient wc = new WebClient())
            {
                if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local))
                {
                    // file not exists
                    wc.DownloadFile(GDBNETGamesDB.BaseImgURL + url, System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local);                    
                }
               
                // add to object
                if (local.Contains("boxartfront"))
                    { gd.BoxartFrontLocalImage = lPath + @"boxartfront\" + filename; }

                if (local.Contains("boxartback"))
                    gd.BoxartBackLocalImage = lPath + @"boxartback\" + filename;
                /*
                if (local.Contains("screenshots"))
                {
                    List<string> s1 = GDBGameData.JsonDeSerialize(gd.ScreenshotLocalImages);
                    s1.Add(lPath + @"screenshots\" + filename);
                    gd.ScreenshotLocalImages = GDBGameData.JsonSerialize(s1);
                }
                if (local.Contains("fanart"))
                {
                    List<string> s2 = GDBGameData.JsonDeSerialize(gd.FanartLocalImages);
                    s2.Add(lPath + @"fanart\" + filename);
                    gd.FanartLocalImages = GDBGameData.JsonSerialize(s2);
                   // gd.FanartLocalImages += lPath + @"fanart\" + filename + ";";
                }
                if (local.Contains("banners"))
                {
                    List<string> s3 = GDBGameData.JsonDeSerialize(gd.BannerLocalImages);
                    s3.Add(lPath + @"banners\" + filename);
                    gd.BannerLocalImages = GDBGameData.JsonSerialize(s3);
                    //gd.BannerLocalImages += lPath + @"banners\" + filename + ";";
                }
                */
            }
            return gd;
        }

        public static void CreateFolderStructure(int gdbId)
        {
            string basePath = @"Data\Graphics\thegamesdb\" + gdbId.ToString() + @"\";
            // boxart
            System.IO.Directory.CreateDirectory(basePath + "boxartfront");
            System.IO.Directory.CreateDirectory(basePath + "boxartback");
            System.IO.Directory.CreateDirectory(basePath + "banners");
            System.IO.Directory.CreateDirectory(basePath + "screenshots");
            System.IO.Directory.CreateDirectory(basePath + "fanart");

        }

        public string AddTrailingWhitespace(string s)
        {
            return s + " ";
        }

        public string StripSymbols(string i)
        {
            // remove all (xxx), [xxx] 
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            // add this to the class
            SearchString = s;
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

        public List<SearchOrdering> ShowPlatformGames(int systemId, string gameName)
        {
            // get a list with all games for this system
            SystemCollection = PlatformGames.Where(a => a.SystemId == systemId).ToList();
            // Match all words and return a list ordered by higest matches
            List<SearchOrdering> searchResult = OrderByMatchedWords(StripSymbols(gameName.ToLower()));
            return searchResult;
        }

        public ICollection<GDBPlatformGame> SearchGameLocal(string gameName, int systemId, int gameId)
        {
            SearchString = gameName;
            LocalIterationCount = 0;
            WorkingSearchCollection = new List<GDBPlatformGame>();
            SearchCollection = new List<GDBPlatformGame>();

            if (SearchString.Contains("[PD]") || SearchString.Contains("(PD)") || SearchString.Contains("SC-3000") || SearchString.Contains("BIOS"))
            {
                // ignore public domain games
                return SearchCollection;
            }

            // get a list with all games for this system
            SystemCollection = PlatformGames.Where(a => a.SystemId == systemId).ToList();

            // Match all words and return a list ordered by higest matches
            List<SearchOrdering> searchResult = OrderByMatchedWords(StripSymbols(gameName.ToLower()));            

            // get max value in the list
            var maxValueRecord = searchResult.OrderByDescending(v => v.Matches).FirstOrDefault();
            if (maxValueRecord == null)
            {
                SearchCollection = (from a in searchResult
                                    select a.Game).ToList();
            }
            else
            {
                int maxValue = maxValueRecord.Matches;
                // select all records that have the max value

                List<SearchOrdering> matches = (from a in searchResult
                                                where (a.Matches == maxValue) // || a.Matches == maxValue - 1)
                                                select a).ToList();
                SearchCollection = (from a in matches
                                    select a.Game).ToList();
                if (matches.Count == 1)
                {
                    // single entry returned
                    List<GDBPlatformGame> single = (from a in matches
                                                    select a.Game).ToList();
                    return single;
                }
                if (matches.Count == 0)
                {
                    return null;
                }
            }
            

            // Multiple records returned - continue

            // match order of words starting with the first and incrementing
            List<GDBPlatformGame> m = MatchOneWordAtATime(SearchCollection, StripSymbols(gameName.ToLower()));

            if (m.Count == 1)
                return m;
            if (m.Count > 1)
                SearchCollection = m;

            // still no definiate match found
            // run levenshetein fuzzy search on SearchCollection - 10 iterations
            List<GDBPlatformGame> lg = LevenIteration(SearchCollection, StripSymbols(gameName.ToLower()));

            return lg;
            
            // remove [anything inbetween] or (anything inbetween) from in the incoming string and remove it

            // remove any symbols
            string gName = StripSymbols(gameName);

            // Pass to search method for fuzzy searching
            StartFuzzySearch(gName, 0);

            // if there is only one entry in searchcollection - match has been found - add it to the database for scraping later
            if (WorkingSearchCollection.Count == 1)
            {
                GDBPlatformGame g = WorkingSearchCollection.FirstOrDefault();
                GDBGameData gd = new GDBGameData();
                /*
                gd.Id = gameId;
                gd.GDBGameId = g.id;
                gd.Title = g.GameTitle;
                gd.ReleaseDate = g.ReleaseDate;  
                */              
            }
            //return SearchCollection;
            return WorkingSearchCollection;
        }

        public List<GDBPlatformGame> LevenIteration(List<GDBPlatformGame> games, string searchStr)
        {
            int levCount = 0;
            List<GDBPlatformGame> temp = new List<GDBPlatformGame>();
            while (levCount <= 10)
            {
                levCount++;
                double it = Convert.ToDouble(levCount) / 10;
                List<GDBPlatformGame> found = FuzzySearch.FSearch(StripSymbols(searchStr.ToLower()), SearchCollection, it);
                
                if (found.Count == 1)
                {
                    return found;
                }
                if (found.Count > 1)
                {
                    // multiple entries returned
                    temp = new List<GDBPlatformGame>();
                    temp.AddRange(found);
                }
                if (found.Count == 0)
                {
                    return temp;
                }
            }
            return temp;
        }

        public List<GDBPlatformGame> MatchOneWordAtATime(List<GDBPlatformGame> games, string searchStr)
        {
            string searchstring = StripSymbols(searchStr).ToLower();
            List<GDBPlatformGame> matched = new List<GDBPlatformGame>();

            // try the first word
            string[] arr = BuildArray(searchstring);
            int i = 0;
            string builder = "";
            while (i < arr.Length)
            {
                if (i == 0)
                {
                    builder += arr[i];
                }
                else
                {
                    builder += " " + arr[i];
                }
                string b = StripSymbols(builder).ToLower();

                List<GDBPlatformGame> matches = new List<GDBPlatformGame>();
                foreach (GDBPlatformGame g in games)
                {
                    string resultstring = StripSymbols(g.GameTitle).ToLower();
                    if (resultstring.Contains(searchstring))
                    {
                        matches.Add(g);
                    }
                }
                matches.Distinct();

                // one distinct match returned
                if (matches.Count == 1)
                    return matches;

                // multiple matches returned
                if (matches.Count > 1)
                {
                    matched = new List<GDBPlatformGame>();
                    matched.AddRange(matches);
                }     
                // increment while loop iterator
                i++;
            }

            // convert matched to distinct entries only
            matched.Distinct();

            // matched is empty - return original games list
            if (matched.Count == 0)
                return games;
            // matched has 1 result
            if (matched.Count == 1)
                return matched;
            // multiple results returned
            return matched;
        }

        public List<SearchOrdering> OrderByMatchedWords(string searchStr)
        {
            //Dictionary<GDBPlatformGame, int> totals = new Dictionary<GDBPlatformGame, int>();
            List<SearchOrdering> totals = new List<SearchOrdering>();

            foreach (GDBPlatformGame g in SystemCollection)
            {
                // sanitise
                string searchstring = StripSymbols(searchStr).ToLower();
                string resultstring = StripSymbols(g.GameTitle).ToLower();

                int matchingWords = 0;

                // get total substrings in search string
                string[] arr = BuildArray(searchstring);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = BuildArray(resultstring);
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
                SearchOrdering so = new SearchOrdering();
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

        private void StartFuzzySearch(string searchStr, int manualIterator)
        {
            // start iterator
            if (manualIterator > 0) { }
            else
            {
                LocalIterationCount++;
                manualIterator = LocalIterationCount;
            }
            
            // setup fuzzystring options based on iteration
            List<FuzzyStringComparisonOptions> fuzzOptions = new List<FuzzyStringComparisonOptions>();
            FuzzyStringComparisonTolerance tolerance;
            switch (manualIterator)
            {
                /* Iterations to widen the selection */
                // first auto iteration - strong matching using substring, subsequence and overlap coefficient
                case 1:
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroWinklerDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseSorensenDiceDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseTanimotoCoefficient);
                    tolerance = FuzzyStringComparisonTolerance.Normal;
                    break;
                // second iteration - same as the first but with normal matching
                case 2:
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroWinklerDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseSorensenDiceDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseTanimotoCoefficient);
                    tolerance = FuzzyStringComparisonTolerance.Normal;
                    break;
                // 3rd auto iteration - same as the first but with weak matching
                case 3:
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroWinklerDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseSorensenDiceDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseTanimotoCoefficient);
                    tolerance = FuzzyStringComparisonTolerance.Weak;
                    break;

                /* Iterations to narrow down selection */
                // first manual iteration
                case 100:
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseNormalizedLevenshteinDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaccardDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseHammingDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseJaroWinklerDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseSorensenDiceDistance);
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseTanimotoCoefficient);
                    tolerance = FuzzyStringComparisonTolerance.Strong;
                    break;
                default:
                    // end and return
                    return;
            }           

            // iterate through each gamesdb game in the list
            foreach (GDBPlatformGame g in SystemCollection)
            {
                bool result = searchStr.ApproximatelyEquals(g.GameTitle, tolerance, fuzzOptions.ToArray());
                if (result == true)
                {
                    // match found - add to searchcollection                    
                    SearchCollection.Add(g);
                    
                }
                else
                {
                    // match not found
                }
            }

            if (SearchCollection.Count == 1) {
                WorkingSearchCollection = SearchCollection;
                return;
            }

            // Check whether the actual game name contains the actual search 
            //GDBPlatformGame gp = SystemCollection.Where(a => StripSymbols(a.GameTitle.ToLower()).Contains(searchStr)).FirstOrDefault();
            List<GDBPlatformGame> gp = SystemCollection.Where(a => AddTrailingWhitespace(a.GameTitle.ToLower()).Contains(AddTrailingWhitespace(SearchString))).ToList();
            if (gp == null)
            {
                // nothing found - proceed to other searches
            }
            else
            {
                if (gp.Count > 1)
                {
                    // multiples found - wipe out search collection and create a new one
                    SearchCollection = new List<GDBPlatformGame>();
                    SearchCollection.AddRange(gp);
                }
                else
                {
                    // only 1 entry found - return
                    SearchCollection = new List<GDBPlatformGame>();
                    SearchCollection.AddRange(gp);
                    WorkingSearchCollection = new List<GDBPlatformGame>();
                    WorkingSearchCollection.AddRange(gp);
                    return;
                }

            }


            // we should now have a pretty wide SearchCollection - count how many matched words
            Dictionary<GDBPlatformGame, int> totals = new Dictionary<GDBPlatformGame, int>();

            foreach (GDBPlatformGame g in SearchCollection)
            {
                int matchingWords = 0;
                // get total substrings in search string
                string[] arr = BuildArray(searchStr);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = BuildArray(g.GameTitle);
                int resultLength = rArr.Length;

                // find matching words
                foreach (string s in arr)
                {
                    int i = 0;
                    while (i < resultLength)
                    {
                        if (StripSymbols(s) == StripSymbols(rArr[i]))
                        {
                            matchingWords++;
                            break;
                        }
                        i++;
                    }
                }
                // add to dictionary with count
                totals.Add(g, matchingWords);
            }

            // order dictionary
            totals.OrderByDescending(a => a.Value);
            // get max value
            var maxValueRecord = totals.OrderByDescending(v => v.Value).FirstOrDefault();
            int maxValue = maxValueRecord.Value;
            // select all records that have the max value
            List<GDBPlatformGame> matches = (from a in totals
                                             where a.Value == maxValue
                                             select a.Key).ToList();
            if (matches.Count == 1)
            {
                // single match found
                WorkingSearchCollection = new List<GDBPlatformGame>();
                WorkingSearchCollection.AddRange(matches);
                return;
            }

            // run levenshetein fuzzy search on SearchCollection - 10 iterations
            int levCount = 0;
            while (levCount <= 10)
            {
                levCount++;
                double it = Convert.ToDouble(levCount) / 10;
                List<GDBPlatformGame> found = FuzzySearch.FSearch(searchStr, SearchCollection, it);
                //WorkingSearchCollection = new List<GDBPlatformGame>();

                if (found.Count == 1)
                {
                    // one entry returned
                    WorkingSearchCollection = new List<GDBPlatformGame>();
                    WorkingSearchCollection.AddRange(found);
                    return;
                }
                if (found.Count > 1)
                {
                    // multiple entries returned

                }

                if (found.Count == 0)
                {

                }

                //WorkingSearchCollection.AddRange(found);
                //return;
            }
            
            //return;

            // check how many matches we have
            if (SearchCollection.Count == 1)
            {
                WorkingSearchCollection = new List<GDBPlatformGame>();
                WorkingSearchCollection.Add(SearchCollection.Single());
                return;
            }

            if (SearchCollection.Count > 1)
            {
                // add to working search collection
                WorkingSearchCollection.AddRange(SearchCollection.ToList());
                // clear SearchCollection
                //SearchCollection = new List<GDBPlatformGame>();

                // try the first word
                string[] arr = BuildArray(searchStr);
                int i = 0;
                string builder = "";
                while (i < arr.Length)
                {
                    if (i == 0)
                    {
                        builder += arr[i];
                    }
                    else
                    {
                        builder += " " + arr[i];
                    }
                    string b = StripSymbols(builder).ToLower();


                    var s = SystemCollection.Where(a => a.GameTitle.ToLower().Contains(b)).ToList();
                    if (s.Count == 1)
                    {
                        // one entry returned - this is the one to keep
                        WorkingSearchCollection = new List<GDBPlatformGame>();
                        //SearchCollection = new List<GDBPlatformGame>();
                        WorkingSearchCollection.Add(s.Single());
                        return;                        
                        
                    }
                    if (s.Count > 1)
                    {
                        // still multiple entries returned - single match not found - continue
                        WorkingSearchCollection = new List<GDBPlatformGame>();
                        WorkingSearchCollection.AddRange(s);
                        //SearchCollection = new List<GDBPlatformGame>();

                    }
                    if (s.Count == 0)
                    {
                        // no matches returned - this should never happen
                    }
                    i++;
                }

                // multiple matches found - run search again from the beginning but remove FIRST substring
                //StartFuzzySearch(searchStr, 100);
                return;
                /*
                string[] arr = BuildArray(searchStr);
                StartFuzzySearch(BuildSearchString(arr.Take(0).ToArray()), 1);
                // multiple matches found - run search again from the beginning but remove last substring              
                StartFuzzySearch(BuildSearchString(arr.Take(arr.Count() - 1).ToArray()), 1); 
                */              
                
            }
            if (SearchCollection.Count == 0)
            {
                // no matches found - run this method again with the next iterator (slightly weaker tolerance)
                StartFuzzySearch(searchStr , 0);
            }

                
        }


        private string[] BuildArray(string searchStr)
        {
            string[] gArr = searchStr.ToLower().Trim().Split(' ');
            return gArr;
        }

        private string BuildSearchString(string[] arr, int position)
        {
            string searchStr = "";
            for (int i = 0; i <= position; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        private string BuildSearchString(string[] arr)
        {
            string searchStr = "";
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        private void StartSearch(string[] gArr)
        {
            // start the iteraton counter
            LocalIterationCount++;
            // get the number of words in the array
            int wordCount = gArr.Length;
            int c = 0; // wordCount - 1;

            // starting with the first word, search against PlatformGames adding words until only 1 result is returned
            while (c <= wordCount - 1)
            {
                // build string from array
                string searchStr = BuildSearchString(gArr, c);

                List<GDBPlatformGame> list = SystemCollection.Where(a => a.GameTitle.ToLower().Contains(searchStr)).ToList();

                if (list.Count == 1)
                {
                    // One game found and it is likely the right one - destroy the current SearchCollection and create a new one - exit the method
                    SearchCollection = new List<GDBPlatformGame>();
                    SearchCollection.AddRange(list);
                    LocalGameFound = true;
                    return;
                }                  
                if (list.Count == 0)
                {
                    // no records found - keep the current SearchCollection
                    break;
                }
                if (list.Count > 1)
                {
                    // multiple records matched - add to searchcollection
                    SearchCollection = new List<GDBPlatformGame>();
                    SearchCollection.AddRange(list);
                }
                c++;
            }

            // first search routine has finished.
            if (SearchCollection.Count > 1)
            {
                string newSearch = "";
                switch (LocalIterationCount)
                {
                    case 1:
                        // proceed to second phase - try replacing some symbols
                        newSearch = BuildSearchString(gArr, wordCount - 1);
                        string[] newArr = BuildArray(newSearch.Replace(":", "").Replace("_","").Replace("-","").Replace("'", ""));
                        StartSearch(newArr);
                        break;

                    case 2:
                        // proceed to 3rd phase - convert numbers to numerals
                        newSearch = BuildSearchString(gArr, wordCount - 1);
                        string[] newArr2 = BuildArray(newSearch.Replace("1", "I").Replace("2", "II").Replace("3", "III").Replace("4", "IV").Replace("5", "V").Replace("6", "VI").Replace("7", "VII").Replace("8", "VIII"));
                        StartSearch(newArr2);
                        break;

                }
            }
        }

        
        

        // refresh platformgames from db
        public void RefreshPlatformGamesFromDb()
        {
            PlatformGames = GDBPlatformGame.GetGames();
        }

        // Update database with all platform games
        public static List<GDBPlatformGame> DatabasePlatformGamesImport(ProgressDialogController controller)
        {          

            // create a new object for database import
            List<GDBPlatformGame> gs = new List<GDBPlatformGame>();
            int count = 0;
            int sysCount = GSystem.GetSystems().Count - 3;

            if (controller != null)
            {
                controller.Minimum = 0;
                controller.Maximum = sysCount;
            }
            
            foreach (GSystem sys in GSystem.GetSystems())
            {

                // skip systems that are not needed
                if (sys.systemId == 16 || sys.systemId == 17 || sys.systemId == 18)
                    continue;
                count++;
                List<GDBNETGameSearchResult> merged = new List<GDBNETGameSearchResult>();

                if (controller != null)
                {
                    controller.SetProgress(Convert.ToDouble(count));
                    controller.SetMessage("Retrieving Game List for Platform: " + sys.systemName);
                    //controller.SetIndeterminate();
                }


                // perform lookups
                foreach (int gid in sys.theGamesDBPlatformId)
                {
                    List<GDBNETGameSearchResult> result = GDBNETGamesDB.GetPlatformGames(gid).ToList();
                    if (result.Count == 0)
                    {
                        // nothing returned
                        if (controller != null)
                        {
                            controller.SetMessage("No results returned.\n Maybe an issue connecting to thegamesdb.net...");
                            Task.Delay(2000);
                        }
                    }
                    merged.AddRange(result);
                }

                // remove duplicates
                List<GDBNETGameSearchResult> nodupe = merged.Distinct().ToList();

                // convert to GDBPlatformGame format and add to top list
                foreach (var n in nodupe)
                {
                    GDBPlatformGame gsingle = new GDBPlatformGame();
                    gsingle.id = n.ID;
                    gsingle.SystemId = sys.systemId;
                    gsingle.GameTitle = n.Title;
                    gsingle.ReleaseDate = n.ReleaseDate;

                    gs.Add(gsingle);
                }
            }

            // now we have a complete list of games for our platforms from thegamesdb.net - add or update the database
            if (controller != null)
            {
                controller.SetMessage("Saving to Database...");
            }

            return gs;
          
                

            // first get the current data
            //List<GDBPlatformGame> current = GDBPlatformGame.GetGames();
        }

        public static List<GDBPlatformGame> DatabasePlatformGamesImport()
        {
            return DatabasePlatformGamesImport(null);
        }

        // get all games from the API based on gamesdb system ID
        public static ICollection<GDBNETGameSearchResult> GetPlatformGames(int systemId)
        {

            ICollection<GDBNETGameSearchResult> result = GDBNETGamesDB.GetPlatformGames(systemId);
            return result;
        }

        public static void SavePlatformGamesToDisk()
        {
            var g = GDBPlatformGame.GetGames();
        }

        public static void UnlinkGameData(DataGrid dgGameList)
        {
            // get selected row
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return;
            }
            int GameId = row.ID;

            // see if the game is linked in the GDBLink table
            List<GDBLink> link = GDBLink.GetRecords(GameId);
            if (link.Count > 0)
            {
                foreach (GDBLink g in link)
                {
                    GDBLink.DeleteRecord(g);
                }
            }

            GamesLibraryVisualHandler.UpdateSidebar(GameId);

        }

        public static void PickGame(DataGrid dgGameList)
        {
            // get selected row
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return;
            }
            int GameId = row.ID;
            PickLocalGame(GameId);
        }

        public static async void PickLocalGame(int GameId)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");
            
            await mw.ShowChildWindowAsync(new ListBoxChildWindow() {
                IsModal = true,
                AllowMove = false,
                Title = "Pick a Game",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);

            GamesLibraryVisualHandler.UpdateSidebar(GameId);
        }
    }
}
