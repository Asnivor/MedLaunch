using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGamesDBAPI {
    /// <summary>
    /// Represents a search result when listing games.
    /// </summary>
    public class PlatformSearchResult {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Name of the platform.
        /// </summary>
        public String Name;

        /// <summary>
        /// URL alias
        /// </summary>
        public String Alias;
    }
}
