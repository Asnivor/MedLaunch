using FuzzyString;
using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes.MasterScraper
{
    // master scraper class - handles all scraping operations
    public class ScraperMainSearch
    {
        public GamesLibraryScrapedContent GLSC { get; set; }
        public GlobalSettings _GlobalSettings { get; set; }

        public List<ScraperMaster> PlatformGames { get; set; }
        public List<ScraperMaster> SystemCollection { get; set; }
        public List<ScraperMaster> WorkingSearchCollection { get; set; }
        public List<ScraperMaster> SearchCollection { get; set; }
        public List<MedLaunch.Models.Game> LocalGames { get; set; }

        public string SearchString { get; set; }
        public bool LocalGameFound { get; set; }
        public int LocalIterationCount { get; set; }
        public int ManualIterator { get; set; }
        public int CurrentLocalGameId { get; set; }

        // constructor
        public ScraperMainSearch()
        {            
            // get instance of this application
            App app = ((App)Application.Current);
            // populate GLSC
            GLSC = app.ScrapedData;
            // populate GlobalSettings
            _GlobalSettings = GlobalSettings.GetGlobals();

            PlatformGames = app.ScrapedData.MasterPlatformList;
            LocalGameFound = false;
            LocalIterationCount = 0;
            ManualIterator = 0;
            SearchCollection = new List<ScraperMaster>();
            WorkingSearchCollection = new List<ScraperMaster>();
        }

        // methods

        /// <summary>
        /// Choose a game from the local master list to link to an imported medlaunch game
        /// </summary>
        /// <param name="dgGameList"></param>
        public static string PickGame(DataGrid dgGameList)
        {
            // get selected row
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return null;
            }
            int GameId = row.ID;
            PickLocalGame(GameId);
            return "success";
        }

        public static string PickGames(DataGrid dgGameList)
        {
            // get number of selected rows
            int numRowsCount = dgGameList.SelectedItems.Count;

            if (numRowsCount == 0)
                return null;
            else if (numRowsCount == 1)
            {
                PickGame(dgGameList);
                return "single";
            }
            else
            {
                // multiples selected
                var rs = dgGameList.SelectedItems;
                List<DataGridGamesView> rows = new List<DataGridGamesView>();
                foreach (DataGridGamesView row in rs)
                {
                    rows.Add(row);
                }

                // process each row
                foreach (DataGridGamesView row in rows)
                {
                    int GameId = row.ID;
                    PickLocalGame(GameId);
                }
                return "multiple";
            }
        }

        /// <summary>
        /// Choose a game from the local master list to link to an imported medlaunch game
        /// based on medlaunch GameId
        /// </summary>
        /// <param name="GameId"></param>
        public static async void PickLocalGame(int GameId)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Grid RootGrid = (Grid)mw.FindName("RootGrid");

            await mw.ShowChildWindowAsync(new ListBoxChildWindow()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Pick a Game",
                CloseOnOverlay = false,
                ShowCloseButton = false
            }, RootGrid);

            GamesLibraryVisualHandler.UpdateSidebar(GameId);
            //GamesLibraryVisualHandler.RefreshGamesLibrary();
        }

        /// <summary>
        /// Return a search list
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public List<SearchOrdering> ShowPlatformGames(int systemId, string gameName)
        {
            // get a list with all games for this system
            SystemCollection = PlatformGames.Where(a => a.MedLaunchSystemId == systemId).ToList();
            // Match all words and return a list ordered by higest matches
            List<SearchOrdering> searchResult = OrderByMatchedWords(StripSymbols(gameName.ToLower()));
            return searchResult;
        }

        /// <summary>
        /// returns a list of search results ordered by number of hits
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public List<SearchOrdering> OrderByMatchedWords(string searchStr)
        {
            //Dictionary<GDBPlatformGame, int> totals = new Dictionary<GDBPlatformGame, int>();
            List<SearchOrdering> totals = new List<SearchOrdering>();

            foreach (ScraperMaster g in SystemCollection)
            {
                // sanitise
                string searchstring = StripSymbols(searchStr).ToLower();
                string resultstring = StripSymbols(g.TGDBData.GamesDBTitle).ToLower();

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

        /// <summary>
        /// strip symbols out of a string
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string StripSymbols(string i)
        {
            // remove all (xxx), [xxx] 
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            // add this to the class
            SearchString = s;
            // remove all - : _ '
            s = s.Replace(" - ", " ").Replace("_", " ").Replace(": ", " ").Replace(" : ", " ").Replace(":", "").Replace("'", "").Replace("-", " ").Trim();
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

        /// <summary>
        /// return a string array (whitespace delimited)
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public static string[] BuildArray(string searchStr)
        {
            string[] gArr = searchStr.ToLower().Trim().Split(' ');
            return gArr;
        }

        /// <summary>
        /// turns array back into string
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public string BuildSearchString(string[] arr, int position)
        {
            string searchStr = "";
            for (int i = 0; i <= position; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        /// <summary>
        /// turns array back into string
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string BuildSearchString(string[] arr)
        {
            string searchStr = "";
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        /// <summary>
        /// return a list of platform games that are matched to searchstring one word at a time
        /// </summary>
        /// <param name="games"></param>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public List<ScraperMaster> MatchOneWordAtATime(List<ScraperMaster> games, string searchStr)
        {
            string searchstring = StripSymbols(searchStr).ToLower();
            List<ScraperMaster> matched = new List<ScraperMaster>();

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

                List<ScraperMaster> matches = new List<ScraperMaster>();
                foreach (ScraperMaster g in games)
                {
                    string resultstring = StripSymbols(g.TGDBData.GamesDBTitle).ToLower();
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
                    matched = new List<ScraperMaster>();
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

        /// <summary>
        /// Fuzzy string matching (not currently used)
        /// </summary>
        /// <param name="searchStr"></param>
        /// <param name="manualIterator"></param>
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
            foreach (ScraperMaster g in SystemCollection)
            {
                bool result = searchStr.ApproximatelyEquals(g.TGDBData.GamesDBTitle, tolerance, fuzzOptions.ToArray());
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

            if (SearchCollection.Count == 1)
            {
                WorkingSearchCollection = SearchCollection;
                return;
            }

            // Check whether the actual game name contains the actual search 
            //GDBPlatformGame gp = SystemCollection.Where(a => StripSymbols(a.GameTitle.ToLower()).Contains(searchStr)).FirstOrDefault();
            List<ScraperMaster> gp = SystemCollection.Where(a => AddTrailingWhitespace(a.TGDBData.GamesDBTitle.ToLower()).Contains(AddTrailingWhitespace(SearchString))).ToList();
            if (gp == null)
            {
                // nothing found - proceed to other searches
            }
            else
            {
                if (gp.Count > 1)
                {
                    // multiples found - wipe out search collection and create a new one
                    SearchCollection = new List<ScraperMaster>();
                    SearchCollection.AddRange(gp);
                }
                else
                {
                    // only 1 entry found - return
                    SearchCollection = new List<ScraperMaster>();
                    SearchCollection.AddRange(gp);
                    WorkingSearchCollection = new List<ScraperMaster>();
                    WorkingSearchCollection.AddRange(gp);
                    return;
                }

            }


            // we should now have a pretty wide SearchCollection - count how many matched words
            Dictionary<ScraperMaster, int> totals = new Dictionary<ScraperMaster, int>();

            foreach (ScraperMaster g in SearchCollection)
            {
                int matchingWords = 0;
                // get total substrings in search string
                string[] arr = BuildArray(searchStr);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = BuildArray(g.TGDBData.GamesDBTitle);
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
            List<ScraperMaster> matches = (from a in totals
                                           where a.Value == maxValue
                                           select a.Key).ToList();
            if (matches.Count == 1)
            {
                // single match found
                WorkingSearchCollection = new List<ScraperMaster>();
                WorkingSearchCollection.AddRange(matches);
                return;
            }

            // run levenshetein fuzzy search on SearchCollection - 10 iterations
            int levCount = 0;
            while (levCount <= 10)
            {
                levCount++;
                double it = Convert.ToDouble(levCount) / 10;
                List<ScraperMaster> found = FuzzySearch.FSearch(searchStr, SearchCollection, it);
                //WorkingSearchCollection = new List<GDBPlatformGame>();

                if (found.Count == 1)
                {
                    // one entry returned
                    WorkingSearchCollection = new List<ScraperMaster>();
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
                WorkingSearchCollection = new List<ScraperMaster>();
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


                    var s = SystemCollection.Where(a => a.TGDBData.GamesDBTitle.ToLower().Contains(b)).ToList();
                    if (s.Count == 1)
                    {
                        // one entry returned - this is the one to keep
                        WorkingSearchCollection = new List<ScraperMaster>();
                        //SearchCollection = new List<GDBPlatformGame>();
                        WorkingSearchCollection.Add(s.Single());
                        return;

                    }
                    if (s.Count > 1)
                    {
                        // still multiple entries returned - single match not found - continue
                        WorkingSearchCollection = new List<ScraperMaster>();
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
                StartFuzzySearch(searchStr, 0);
            }


        }

        /// <summary>
        /// add whitespace to the end of a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string AddTrailingWhitespace(string s)
        {
            return s + " ";
        }

        public ICollection<ScraperMaster> SearchGameLocal(string gameName, int systemId, int gameId)
        {
            SearchString = gameName;
            LocalIterationCount = 0;
            WorkingSearchCollection = new List<ScraperMaster>();
            SearchCollection = new List<ScraperMaster>();

            if (SearchString.Contains("[PD]") || SearchString.Contains("(PD)") || SearchString.Contains("SC-3000") || SearchString.Contains("BIOS"))
            {
                // ignore public domain games
                return SearchCollection;
            }

            // convert pce-cd systemid
            if (systemId == 18)
                systemId = 7;

            // get a list with all games for this system
            SystemCollection = PlatformGames.Where(a => a.MedLaunchSystemId == systemId).ToList();

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
                    List<ScraperMaster> single = (from a in matches
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
            List<ScraperMaster> m = MatchOneWordAtATime(SearchCollection, StripSymbols(gameName.ToLower()));

            if (m.Count == 1)
                return m;
            if (m.Count > 1)
                SearchCollection = m;


            if (SearchCollection.Count == 2 && _GlobalSettings.preferGenesis == true)
            {
                // 2 records returned - check whether they match exactly
                string first = (from a in SearchCollection
                                select a.TGDBData.GamesDBTitle).First();
                string last = (from a in SearchCollection
                               select a.TGDBData.GamesDBTitle).Last();
                if (first == last)
                {
                    // looks like the same game - perhaps different systems on the games db (ie - Megadrive / Genesis) - return the first result
                    ScraperMaster pg = (from a in SearchCollection
                                        select a).First();
                    List<ScraperMaster> l = new List<ScraperMaster>();
                    l.Add(pg);
                    return l;
                }
            }

            // still no definiate match found
            // run levenshetein fuzzy search on SearchCollection - 10 iterations
            List<ScraperMaster> lg = LevenIteration(SearchCollection, StripSymbols(gameName.ToLower()));

            return lg;

            // remove [anything inbetween] or (anything inbetween) from in the incoming string and remove it

            // remove any symbols
            string gName = StripSymbols(gameName);

            // Pass to search method for fuzzy searching
            StartFuzzySearch(gName, 0);

            // if there is only one entry in searchcollection - match has been found - add it to the database for scraping later
            if (WorkingSearchCollection.Count == 1)
            {
                ScraperMaster g = WorkingSearchCollection.FirstOrDefault();
                //GDBGameData gd = new GDBGameData();
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

        public List<ScraperMaster> LevenIteration(List<ScraperMaster> games, string searchStr)
        {
            int levCount = 0;
            List<ScraperMaster> temp = new List<ScraperMaster>();
            while (levCount <= 10)
            {
                levCount++;
                double it = Convert.ToDouble(levCount) / 10;
                List<ScraperMaster> found = FuzzySearch.FSearch(StripSymbols(searchStr.ToLower()), SearchCollection, it);

                if (found.Count == 1)
                {
                    return found;
                }
                if (found.Count > 1)
                {
                    // multiple entries returned
                    temp = new List<ScraperMaster>();
                    temp.AddRange(found);
                }
                if (found.Count == 0)
                {
                    return temp;
                }
            }
            return temp;
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

                List<ScraperMaster> list = SystemCollection.Where(a => a.TGDBData.GamesDBTitle.ToLower().Contains(searchStr)).ToList();

                if (list.Count == 1)
                {
                    // One game found and it is likely the right one - destroy the current SearchCollection and create a new one - exit the method
                    SearchCollection = new List<ScraperMaster>();
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
                    SearchCollection = new List<ScraperMaster>();
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
                        string[] newArr = BuildArray(newSearch.Replace(":", "").Replace("_", "").Replace("-", "").Replace("'", ""));
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
    }
}
