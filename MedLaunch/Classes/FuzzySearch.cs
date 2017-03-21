using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using System.Text.RegularExpressions;
using MedLaunch.Classes.Scraper;

namespace MedLaunch.Classes
{   

    public static class FuzzySearch
    {
        public static List<ScraperMaster> FSearch(string word, List<ScraperMaster> platformGames, double fuzzyness)
        {
            List<ScraperMaster> g = new List<ScraperMaster>();

            g = (
                    from s in platformGames
                    let levenshteinDistance = LevenshteinDistance(StripSymbols(word), StripSymbols(s.TGDBData.GamesDBTitle))
                    let length = Math.Max(StripSymbols(s.TGDBData.GamesDBTitle).Length, StripSymbols(word).Length)
                    let score = 1.0 - (double)levenshteinDistance / length
                    where score > fuzzyness
                    select s
                ).ToList();
            return g;
        }

        public static string StripSymbols(string i)
        {
            // remove all (xxx), [xxx] 
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            // add this to the class
            //SearchString = s;
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

        public static List<string> Search(
        string word,
        List<string> wordList,
        double fuzzyness)
        {
            // Tests have prove that the !LINQ-variant is about 3 times
            // faster!
            List<string> foundWords =
                (
                    from s in wordList
                    let levenshteinDistance = LevenshteinDistance(word, s)
                    let length = Math.Max(s.Length, word.Length)
                    let score = 1.0 - (double)levenshteinDistance / length
                    where score > fuzzyness
                    select s
                ).ToList();

            return foundWords;
        }

        
        public static List<MobyPlatformGame> Search(
       string word,
       List<MobyPlatformGame> mobyPlatformGameList,
       double fuzzyness)
        {
            List<MobyPlatformGame> foundWords = new List<MobyPlatformGame>();
            if (fuzzyness == 0)
            {
                // do exact string check
                foundWords =
                    (
                    from s in mobyPlatformGameList
                    where s.Title.ToUpper().Trim() == word.ToUpper().Trim()
                    select s
                    ).ToList();
                return foundWords;
            }



            // use levistein
            foundWords =
                (
                from s in mobyPlatformGameList
                let levenshteinDistance = LevenshteinDistance(word.ToUpper().Trim(), s.Title.ToUpper().Trim())
                let length = Math.Max(s.Title.ToUpper().Trim().Length, word.ToUpper().Trim().Length)
                let score = 1.0 - (double)levenshteinDistance / length
                where score > fuzzyness
                select s                
                ).ToList();

            return foundWords;
        }


        public static int LevenshteinDistance(string src, string dest)
        {
            int[,] d = new int[src.Length + 1, dest.Length + 1];
            int i, j, cost;
            char[] str1 = src.ToCharArray();
            char[] str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
            {
                d[i, 0] = i;
            }
            for (j = 0; j <= str2.Length; j++)
            {
                d[0, j] = j;
            }
            for (i = 1; i <= str1.Length; i++)
            {
                for (j = 1; j <= str2.Length; j++)
                {

                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    d[i, j] =
                        Math.Min(
                            d[i - 1, j] + 1,              // Deletion
                            Math.Min(
                                d[i, j - 1] + 1,          // Insertion
                                d[i - 1, j - 1] + cost)); // Substitution

                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
                        str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[str1.Length, str2.Length];
        }


    }
}
