using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using TheGamesDBAPI;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using FuzzyString;

namespace MedLaunch.Classes
{
    public class GameScraper
    {
        // Properties
        public List<GDBPlatformGame> PlatformGames { get; set; }
        public List<GDBPlatformGame> SystemCollection { get; set; }
        public List<GDBPlatformGame> WorkingSearchCollection { get; set; }
        public List<GDBPlatformGame> SearchCollection { get; set; }
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

        public static string StripSymbols(string i)
        {
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            s = s.Replace(" - ", " ").Replace("-", "").Replace("_", "").Replace(": ", " ").Replace(" : ", " ").Replace(":", "").Replace("'", "").Trim();
            return s;
        }

        public ICollection<GDBPlatformGame> SearchGameLocal(string gameName, int systemId, int gameId)
        {
            SearchString = gameName;
            LocalIterationCount = 0;
            WorkingSearchCollection = new List<GDBPlatformGame>();
            SearchCollection = new List<GDBPlatformGame>();

            // get a list with all games for this system
            SystemCollection = PlatformGames.Where(a => a.SystemId == systemId).ToList();
            
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

                gd.GameId = gameId;
                gd.GDBGameId = g.id;
                gd.Title = g.GameTitle;
                gd.ReleaseDate = g.ReleaseDate;                
            }
            //return SearchCollection;
            return WorkingSearchCollection;
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
                    fuzzOptions.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseSorensenDiceDistance);
                    //fuzzOptions.Add(FuzzyStringComparisonOptions.UseTanimotoCoefficient);
                    tolerance = FuzzyStringComparisonTolerance.Strong;
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

            // Check whether the actual game name matches the search - if so return
            GDBPlatformGame gp = SystemCollection.Where(a => StripSymbols(a.GameTitle.ToLower()).Contains(searchStr)).FirstOrDefault();
            if (gp == null) { }
            else
            {
                SearchCollection = new List<GDBPlatformGame>();
                SearchCollection.Add(gp);
                WorkingSearchCollection = new List<GDBPlatformGame>();
                WorkingSearchCollection.Add(gp);
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
            controller.Minimum = 0;
            controller.Maximum = sysCount;
            foreach (GSystem sys in GSystem.GetSystems())
            {

                // skip systems that are not needed
                if (sys.systemId == 16 || sys.systemId == 17 || sys.systemId == 18)
                    continue;
                count++;
                List<GameSearchResult> merged = new List<GameSearchResult>();
                controller.SetProgress(Convert.ToDouble(count));
                controller.SetMessage("Retrieving Game List for Platform: " + sys.systemName);
                //controller.SetIndeterminate();

                // perform lookups
                foreach (int gid in sys.theGamesDBPlatformId)
                {
                    List<GameSearchResult> result = TheGamesDBAPI.GamesDB.GetPlatformGames(gid).ToList();
                    if (result.Count == 0)
                    {
                        // nothing returned
                        controller.SetMessage("No results returned.\n Maybe an issue connecting to thegamesdb.net...");
                        Task.Delay(2000);
                    }
                    merged.AddRange(result);
                }

                // remove duplicates
                List<GameSearchResult> nodupe = merged.Distinct().ToList();

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
            controller.SetMessage("Saving to Database...");

            return gs;
          
                

            // first get the current data
            //List<GDBPlatformGame> current = GDBPlatformGame.GetGames();
        }

        // get all games from the API based on gamesdb system ID
        public static ICollection<GameSearchResult> GetPlatformGames(int systemId)
        {

            ICollection<GameSearchResult> result = TheGamesDBAPI.GamesDB.GetPlatformGames(systemId);
            return result;
        }

        public static void SavePlatformGamesToDisk()
        {
            var g = GDBPlatformGame.GetGames();
        }
    }
}
