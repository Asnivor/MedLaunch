using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asnitech.Launch.Common.Search
{
    public class SearchObject
    {
        public int searchId { get; set; }
        public string searchString { get; set; }
        public List<SearchList> listToSearch { get; set; }
        public List<SearchResult> searchResults { get; set; }

        public SearchObject()
        {
            listToSearch = new List<SearchList>();
            searchResults = new List<SearchResult>();
        }
    }

    public class SearchList
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class SearchResult
    {
        public int resultId { get; set; }
        public string resultString { get; set; }
        public double score { get; set; }
    }
}
