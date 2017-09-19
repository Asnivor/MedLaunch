using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common.Converters;

namespace MedLaunch.Common.Search
{
    public class SearchFunctions
    {
        public static SearchObject WordCountMatch(SearchObject searchObj)
        {
            // iterate through each entry in the listtosearch
            for (int it = 0; it < searchObj.listToSearch.Count; it++)
            {
                // sanitse
                string searchOrig = ObjConverter.StripSymbols(searchObj.searchString).ToLower();
                string searchDest = ObjConverter.StripSymbols(searchObj.listToSearch[it].name).ToLower();

                double matchingWords = 0;

                // get total substrings in search string
                string[] arr = ObjConverter.StringToArray(searchOrig, ObjConverter.ConversationCase.None);
                int searchLength = arr.Length;

                // get total substrings in result string
                string[] rArr = ObjConverter.StringToArray(searchDest, ObjConverter.ConversationCase.None);
                int resultLength = rArr.Length;

                // find matching words
                foreach (string s in arr)
                {
                    double i = 0;
                    while (i < resultLength)
                    {
                        if (ObjConverter.StripSymbols(s) == ObjConverter.StripSymbols(rArr[Convert.ToInt32(i)].ToLower()))
                        {
                            // reduce score to 0.5 for common works like and, of, the, a
                            if (ObjConverter.StripSymbols(s).ToLower() == "a" ||
                                ObjConverter.StripSymbols(s).ToLower() == "of" ||
                                ObjConverter.StripSymbols(s).ToLower() == "the" ||
                                ObjConverter.StripSymbols(s).ToLower() == "and")
                            {
                                matchingWords = matchingWords + 0.3;
                            }
                            else
                            {
                                matchingWords = matchingWords + 1;
                            }
                            matchingWords++;
                            break;
                        }
                        i++;
                    }
                }

                // create new searchresult object and add to list
                SearchResult sr = new SearchResult();
                sr.resultId = searchObj.listToSearch[it].id;
                sr.resultString = searchObj.listToSearch[it].name;
                sr.score = (Convert.ToDouble(matchingWords) / Convert.ToDouble(arr.Length)) * 100;

                if (sr.score > 0)
                {
                    searchObj.searchResults.Add(sr);
                    continue;
                }
            }
            // order list
            searchObj.searchResults.OrderByDescending(a => a.score);

            return searchObj;
        }
    }
}
