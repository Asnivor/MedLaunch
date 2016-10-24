using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Collections;

namespace MedLaunch.Classes.TheGamesDB
{
    /// <summary>
    /// Fetches information from TheGamesDB.
    /// </summary>
    public static class GDBNETGamesDB
    {
        /// <summary>
        /// The base image path that should be prepended to all the relative image paths to get the full paths to the images.
        /// </summary>
        public const String BaseImgURL = @"http://thegamesdb.net/banners/";

        /// <summary>
        /// Gets a collection of games matched up with loose search terms.
        /// </summary>
        /// <param name="Name">The game title to search for</param>
        /// <param name="Platform">Filters results by platform</param>
        /// <param name="Genre">Filters results by genre</param>
        /// <returns>A collection of games that matched the search terms</returns>
        public static ICollection<GDBNETGameSearchResult> GetGames(String Name, String Platform = "", String Genre = "")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetGamesList.php?name=" + Name + @"&platform=" + Platform + @"&genre=" + Genre);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<GDBNETGameSearchResult> games = new List<GDBNETGameSearchResult>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                GDBNETGameSearchResult game = new GDBNETGameSearchResult();
                gameNode = (XmlNode)ienum.Current;

                IEnumerator ienumGame = gameNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumGame.MoveNext())
                {
                    attributeNode = (XmlNode)ienumGame.Current;

                    // Iterate through all game attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out game.ID);
                            break;
                        case "GameTitle":
                            game.Title = attributeNode.InnerText;
                            break;
                        case "ReleaseDate":
                            game.ReleaseDate = attributeNode.InnerText;
                            break;
                        case "Platform":
                            game.Platform = attributeNode.InnerText;
                            break;
                    }
                }

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets all games updated since the specified time.
        /// </summary>
        /// <param name="time">Last x seconds to get updated games for</param>
        /// <returns>A collection of game ID's for games that have been updated</returns>
        public static ICollection<int> GetUpdatedGames(int time)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/Updates.php?time=" + time);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();
            ienum.MoveNext();

            List<int> games = new List<int>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                gameNode = (XmlNode)ienum.Current;

                int game;
                int.TryParse(gameNode.InnerText, out game);

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets the data for a specific game.
        /// </summary>
        /// <param name="ID">The game ID to return data for</param>
        /// <returns>A Game-object containing all the data about the game, or null if no game was found</returns>
        public static GDBNETGame GetGame(int ID)
        {
            WebOps wo = new WebOps();
            wo.Params = "/GetGame.php?id=" + ID;
            string result = wo.ApiCall();
            /*
            wo.GDBApiCall();
            if (wo.BodyAsString == null)
            {
                // no body returned within the specified timeout periods
                return null;
            }
            */
            
            // string returned
            XmlDocument doc = new XmlDocument();            
            doc.LoadXml(result);
            
            /*
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(@"http://thegamesdb.net/api/GetGame.php?id=" + ID);
            }

            catch (Exception ex)
            {
                return new GDBNETGame();
            }
            finally { }
            */

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            XmlNode platformNode = root.FirstChild.NextSibling;
            GDBNETGame game = new GDBNETGame();

            IEnumerator ienumGame = platformNode.GetEnumerator();
            XmlNode attributeNode;
            while (ienumGame.MoveNext())
            {
                attributeNode = (XmlNode)ienumGame.Current;

                // Iterate through all platform attributes
                switch (attributeNode.Name)
                {
                    case "id":
                        int.TryParse(attributeNode.InnerText, out game.ID);
                        break;
                    case "Overview":
                        game.Overview = attributeNode.InnerText;
                        break;
                    case "GameTitle":
                        game.Title = attributeNode.InnerText;
                        break;
                    case "Platform":
                        game.Platform = attributeNode.InnerText;
                        break;
                    case "ReleaseDate":
                        game.ReleaseDate = attributeNode.InnerText;
                        break;
                    case "overview":
                        game.Overview = attributeNode.InnerText;
                        break;
                    case "ESRB":
                        game.ESRB = attributeNode.InnerText;
                        break;
                    case "Players":
                        game.Players = attributeNode.InnerText;
                        break;
                    case "Co-op":
                        game.Coop = attributeNode.InnerText;
                        break;
                    case "Publisher":
                        game.Publisher = attributeNode.InnerText;
                        break;
                    case "Developer":
                        game.Developer = attributeNode.InnerText;
                        break;
                    case "Rating":
                        //double.TryParse(attributeNode.InnerText, out game.Rating);
                        game.Rating = attributeNode.InnerText;
                        break;
                    case "AlternateTitles":
                        IEnumerator ienumAlternateTitles = attributeNode.GetEnumerator();
                        while (ienumAlternateTitles.MoveNext())
                        {
                            game.AlternateTitles.Add(((XmlNode)ienumAlternateTitles.Current).InnerText);
                        }
                        break;
                    case "Genres":
                        IEnumerator ienumGenres = attributeNode.GetEnumerator();
                        while (ienumGenres.MoveNext())
                        {
                            game.Genres.Add(((XmlNode)ienumGenres.Current).InnerText);
                        }
                        break;
                    case "Images":
                        game.Images.FromXmlNode(attributeNode);
                        break;
                }
            }

            return game;
        }

        /// <summary>
        /// Gets the data for a specific game.
        /// </summary>
        /// <param name="game">The game to return data for</param>
        /// <returns>A Game-object containing all the data about the game, or null if no game was found</returns>
        public static GDBNETGame GetGame(GDBNETGameSearchResult game)
        {
            return GetGame(game.ID);
        }

        /// <summary>
        /// Gets a collection of all the available platforms.
        /// </summary>
        /// <returns>A collection of all the available platforms</returns>
        public static ICollection<GDBNETPlatformSearchResult> GetPlatforms()
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(@"http://thegamesdb.net/api/GetPlatformsList.php");

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.FirstChild.NextSibling.GetEnumerator();

            List<GDBNETPlatformSearchResult> platforms = new List<GDBNETPlatformSearchResult>();

            // Iterate through all platforms
            XmlNode platformNode;
            while (ienum.MoveNext())
            {
                platformNode = (XmlNode)ienum.Current;

                GDBNETPlatformSearchResult platform = new GDBNETPlatformSearchResult();

                IEnumerator ienumPlatform = platformNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumPlatform.MoveNext())
                {
                    attributeNode = (XmlNode)ienumPlatform.Current;

                    // Iterate through all platform attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out platform.ID);
                            break;
                        case "name":
                            platform.Name = attributeNode.InnerText;
                            break;
                        case "alias":
                            platform.Alias = attributeNode.InnerText;
                            break;
                    }
                }

                platforms.Add(platform);
            }

            return platforms;
        }

        /// <summary>
        /// Gets all data for a specific platform.
        /// </summary>
        /// <param name="ID">The platform ID to return data for (can be found by using GetPlatformsList)</param>
        /// <returns>A Platform-object containing all the data about the platform, or null if no platform was found</returns>
        public static GDBNETPlatform GetPlatform(int ID)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/GetPlatform.php?id=" + ID);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            XmlNode platformNode = root.FirstChild.NextSibling;
            GDBNETPlatform platform = new GDBNETPlatform();

            IEnumerator ienumPlatform = platformNode.GetEnumerator();
            XmlNode attributeNode;
            while (ienumPlatform.MoveNext())
            {
                attributeNode = (XmlNode)ienumPlatform.Current;

                // Iterate through all platform attributes
                switch (attributeNode.Name)
                {
                    case "id":
                        int.TryParse(attributeNode.InnerText, out platform.ID);
                        break;
                    case "Platform":
                        platform.Name = attributeNode.InnerText;
                        break;
                    case "overview":
                        platform.Overview = attributeNode.InnerText;
                        break;
                    case "developer":
                        platform.Developer = attributeNode.InnerText;
                        break;
                    case "manufacturer":
                        platform.Manufacturer = attributeNode.InnerText;
                        break;
                    case "cpu":
                        platform.CPU = attributeNode.InnerText;
                        break;
                    case "memory":
                        platform.Memory = attributeNode.InnerText;
                        break;
                    case "graphics":
                        platform.Graphics = attributeNode.InnerText;
                        break;
                    case "sound":
                        platform.Sound = attributeNode.InnerText;
                        break;
                    case "display":
                        platform.Display = attributeNode.InnerText;
                        break;
                    case "media":
                        platform.Media = attributeNode.InnerText;
                        break;
                    case "maxcontrollers":
                        int.TryParse(attributeNode.InnerText, out platform.MaxControllers);
                        break;
                    case "Rating":
                        float.TryParse(attributeNode.InnerText, out platform.Rating);
                        break;
                    case "Images":
                        platform.Images.FromXmlNode(attributeNode);
                        break;
                }
            }

            return platform;
        }

        /// <summary>
        /// Gets all data for a specific platform.
        /// </summary>
        /// <param name="platform">The platform to return data for (can be found by using GetPlatformsList)</param>
        /// <returns>A Platform-object containing all the data about the platform, or null if no platform was found</returns>
        public static GDBNETPlatform GetPlatform(GDBNETPlatformSearchResult platform)
        {
            return GetPlatform(platform.ID);
        }

        /// <summary>
        /// Gets all the games for a platform. The Platform field will not be filled.
        /// </summary>
        /// <param name="ID">The platform ID to return games for (can be found by using GetPlatformsList)</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<GDBNETGameSearchResult> GetPlatformGames(int ID)
        {
            WebOps wo = new WebOps();
            wo.Params = "/GetPlatformGames.php?platform=" + ID;
            string result = wo.ApiCall();
            
            // string returned
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            /*
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(@"http://thegamesdb.net/api/GetPlatformGames.php?platform=" + ID);
            }
            catch (Exception ex)
            {
                return new List<GDBNETGameSearchResult>();
            }
            finally { }
            */

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<GDBNETGameSearchResult> games = new List<GDBNETGameSearchResult>();

            // Iterate through all games
            XmlNode gameNode;
            while (ienum.MoveNext())
            {
                GDBNETGameSearchResult game = new GDBNETGameSearchResult();
                gameNode = (XmlNode)ienum.Current;

                IEnumerator ienumGame = gameNode.GetEnumerator();
                XmlNode attributeNode;
                while (ienumGame.MoveNext())
                {
                    attributeNode = (XmlNode)ienumGame.Current;

                    // Iterate through all game attributes
                    switch (attributeNode.Name)
                    {
                        case "id":
                            int.TryParse(attributeNode.InnerText, out game.ID);
                            break;
                        case "GameTitle":
                            game.Title = attributeNode.InnerText;
                            break;
                        case "ReleaseDate":
                            game.ReleaseDate = attributeNode.InnerText;
                            break;
                    }
                }

                games.Add(game);
            }

            return games;
        }

        /// <summary>
        /// Gets all the games for a platform.
        /// </summary>
        /// <param name="platform">The platform to return games for</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<GDBNETGameSearchResult> GetPlatformGames(GDBNETPlatform platform)
        {
            ICollection<GDBNETGameSearchResult> games = GetPlatformGames(platform.ID);
            foreach (GDBNETGameSearchResult game in games)
            {
                game.Platform = platform.Name;
            }
            return games;
        }

        /// <summary>
        /// Gets all the games for a platform.
        /// </summary>
        /// <param name="platform">The platform to return games for</param>
        /// <returns>A collection of all the games on the platform</returns>
        public static ICollection<GDBNETGameSearchResult> GetPlatformGames(GDBNETPlatformSearchResult platform)
        {
            ICollection<GDBNETGameSearchResult> games = GetPlatformGames(platform.ID);
            foreach (GDBNETGameSearchResult game in games)
            {
                game.Platform = platform.Name;
            }
            return games;
        }

        /// <summary>
        /// Gets all of a user's favorites.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <returns>Collection of game ID:s</returns>
        public static ICollection<int> GetUserFavorites(String AccountIdentifier)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + AccountIdentifier);

            XmlNode root = doc.DocumentElement;
            IEnumerator ienum = root.GetEnumerator();

            List<int> favorites = new List<int>();

            IEnumerator ienumGame = root.GetEnumerator();
            XmlNode gameNode;
            while (ienumGame.MoveNext())
            {
                gameNode = (XmlNode)ienumGame.Current;

                int favorite = 0;
                int.TryParse(gameNode.InnerText, out favorite);
                favorites.Add(favorite);
            }

            return favorites;
        }

        /// <summary>
        /// Adds a game to the user's favorites.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="GameID">ID of the game to add</param>
        public static void AddUserFavorite(String AccountIdentifier, int GameID)
        {
            SendRequest(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + AccountIdentifier + @"&type=add&gameid=" + GameID);
        }

        /// <summary>
        /// Removes a game from the user's favorites.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="GameID">ID of the game to remove</param>
        public static void RemoveUserFavorite(String AccountIdentifier, int GameID)
        {
            SendRequest(@"http://thegamesdb.net/api/User_Favorites.php?accountid=" + AccountIdentifier + @"&type=remove&gameid=" + GameID);
        }

        /// <summary>
        /// Gets a user's rating of a specific game.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="GameID">ID of the game to get the rating of</param>
        /// <returns>A rating of 1 to 10 (or 0 if the user has not rated the game)</returns>
        public static int GetUserRating(String AccountIdentifier, int GameID)
        {
            // Create an XML document instance.
            XmlDocument doc = new XmlDocument();

            // Load the XML data from a URL.
            doc.Load(@"http://thegamesdb.net/api/User_Rating.php?accountid=58536D31278176DA&itemid=2");

            XmlNode root = doc.DocumentElement;

            int rating = 0;
            int.TryParse(root.FirstChild.FirstChild.InnerText, out rating);
            return rating;
        }

        /// <summary>
        /// Sets a user's rating of a specific game.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="GameID">ID of the game to rate</param>
        /// <param name="Rating">A rating of 1 to 10</param>
        public static void SetUserRating(String AccountIdentifier, int GameID, int Rating)
        {
            if (Rating < 1 || Rating > 10)
            {
                throw new ArgumentOutOfRangeException();
            }

            SendRequest(@"http://thegamesdb.net/api/User_Rating.php?accountid=" + AccountIdentifier + @"&itemid=" + GameID + @"&rating=" + Rating);
        }

        /// <summary>
        /// Removes a user's rating of a specific game.
        /// </summary>
        /// <param name="AccountIdentifier">The unique 'account identifier' of the user in question. It can be found on their 'My User Info' page.</param>
        /// <param name="GameID">ID of the game to remove the rating for</param>
        public static void RemoveUserRating(String AccountIdentifier, int GameID)
        {
            SendRequest(@"http://thegamesdb.net/api/User_Rating.php?accountid=" + AccountIdentifier + @"&itemid=" + GameID + @"&rating=0");
        }

        private static void SendRequest(String URL)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(URL);
        }
    }
}
