using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.TheGamesDB
{
    /// <summary>
    /// Represents a search result when searching for games.
    /// </summary>
    public class GDBNETGameSearchResult
    {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Title of the game.
        /// </summary>
        public String Title;

        /// <summary>
        /// Date on which the game was released.
        /// </summary>
        public String ReleaseDate;

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public String Platform;
    }
}
