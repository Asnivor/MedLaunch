using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.TheGamesDB;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MedLaunch.Classes.Scraper
{
    public class ScraperHandler
    {
        /* Properties */
        public ScraperSearch SSearch { get; set; }
        public ScraperMaster MasterRecord { get; set; }
        public GlobalSettings _GlobalSettings { get; set; }
        public MainWindow mw { get; set; }
        public int GameId { get; set; }


        /* Constructors */

        /// <summary>
        /// Instantiate class with a specific platform game entry based on gdbId
        /// </summary>
        /// <param name="gdbId"></param>
        public ScraperHandler(int gdbId, int gameId)
        {
            SSearch = new ScraperSearch();
            MasterRecord = ScraperMaster.GetMasterList().Where(a => a.gid == gdbId).FirstOrDefault();
            _GlobalSettings = SSearch._GlobalSettings;
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            GameId = gameId;
        }

        public ScraperHandler(int gdbId, int gameId, bool MainWindowRequired)
        {
            SSearch = new ScraperSearch();
            MasterRecord = ScraperMaster.GetMasterList().Where(a => a.gid == gdbId).FirstOrDefault();
            _GlobalSettings = SSearch._GlobalSettings;
            if (MainWindowRequired == true)
                mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            GameId = gameId;
        }

        /* Methods */

        public static void DoScrape(ProgressDialogController controller, Game game, string countString, ScraperSearch gs, bool rescrape)
        {
            string gameName = game.gameName;
            int systemId = game.systemId;
            int gameId = game.gameId;

            // attempt local match for game
            controller.SetMessage("Attempting local search match for:\n" + gameName + " (" + GSystem.GetSystemCode(systemId) + ")" + "\n(" + countString);
            List<ScraperMaster> results = gs.SearchGameLocal(gameName, systemId, gameId).ToList();

            if (results.Count == 0)
            {
                // no results returned
                return;
            }
            if (results.Count == 1)
            {
                // one result returned - add GdbId to the Game table
                Game.SetGdbId(gameId, results.Single().gid);
                // also add it to our locagames object (so it is not skipped in the next part)
                //game.gdbId = results.Single().gid;
            }
            if (results.Count > 1)
            {
                // more than one result returned - do nothing (because a definite match was not found)
                return;
            }

            /* Begin scraping */
            bool scrapingNeeded = false;
            // check game directory
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + results.Single().gid.ToString()))
            {
                // directory does not exist - scraping needed
                scrapingNeeded = true;
            }
            else
            {
                // directory does exist - check whether json file is present
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + results.Single().gid.ToString() + @"\" + results.Single().gid.ToString() + ".json"))
                {
                    // json file is not present - scraping needed
                    scrapingNeeded = true;
                }
            }

            // if rescrape bool has been set - always rescrape
            if (rescrape == true)
                scrapingNeeded = true;

            if (scrapingNeeded == false)
                return;

            // do scrape
            string message = "Scraping Started....\nGetting data for: " + gameName + " (" + GSystem.GetSystemCode(systemId) + ")" + "\n(" + countString;
            controller.SetMessage(message);
                   
            ScraperHandler sh = new ScraperHandler(results.Single().gid, gameId, false);
            sh.ScrapeGame(controller);
            //GameListBuilder.UpdateFlag();
            
        }

        /// <summary>
        /// Scrape multiple games that have been selected in the games library (either scrape unscraped, or re-scrape all)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="scrapeType"></param>
        public async static void ScrapeMultiple(List<GamesLibraryModel> list, ScrapeType scrapeType, int systemId)
        {
            // instantiate instance of this class
            ScraperSearch gs = new ScraperSearch();

            // get mainwindow 
            MainWindow MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // popup progress dialog
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await MWindow.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                // check whether list is null and generate it if it is (so scraping/rescraping whole systems or favorites)
                if (list == null)
                {
                    list = new List<GamesLibraryModel>();
                    List<Game> games = new List<Game>();
                    if (scrapeType == ScrapeType.Favorites || scrapeType == ScrapeType.RescrapeFavorites)
                    {
                        games = Game.GetGames().Where(a => a.isFavorite == true && a.hidden != true).ToList();
                    }
                    else
                    {
                        // get all games that have matching systemId and are not marked as hidden
                        games = Game.GetGames(systemId).Where(a => a.hidden != true).ToList();
                    }
                    
                    // populate list
                    foreach (var g in games)
                    {
                        GamesLibraryModel glm = new GamesLibraryModel();
                        glm.ID = g.gameId;
                        list.Add(glm);
                    }
                }


                // iterate through each game in list - match local then scrape
                int iter = 0;
                int maxCount = list.Count();
                int skip = 0;
                controller.Minimum = iter;
                controller.Maximum = maxCount;
                foreach (var game in list)
                {

                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }

                    iter++;
                    controller.SetProgress(iter);
                    // game game object from the database
                    Game g = Game.GetGame(game.ID);

                    string countString = iter + " of " + maxCount + " (" + skip + " skipped)";

                    switch (scrapeType)
                    {
                        // scrape selected games (that have no been scraped yet)
                        case ScrapeType.Selected:
                        case ScrapeType.Favorites:
                        case ScrapeType.ScrapeSystem:
                            if (g.gdbId == null || g.gdbId == 0 || Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString()) == false)
                            {
                                // scraping can happen
                                DoScrape(controller, g, countString, gs, false);
                            }
                            else
                            {
                                // the game already has a valid gdbid set AND has a game directory on disc.
                                skip++;
                            }
                            break;

                        // rescrape all selected games
                        case ScrapeType.SelectedRescrape:
                        case ScrapeType.RescrapeFavorites:
                        case ScrapeType.RescrapeSystem:
                            // scraping must always happen
                            DoScrape(controller, g, countString, gs, true);
                            break;
                    }
                }
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
                GamesLibraryVisualHandler.RefreshGamesLibrary();
            }
            else
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
                GamesLibraryVisualHandler.RefreshGamesLibrary();
            }
        }        

        /// <summary>
        /// Game scraping logic
        /// </summary>
        /// <param name="controller"></param>
        public void ScrapeGame(ProgressDialogController controller, string message)
        {
            // create data object for results that are returned
            //ScrapeDB glsc = new ScrapeDB();
            ScrapedGameData gameData = new ScrapedGameData();
            ScrapedGameObjectWeb gameObject = new ScrapedGameObjectWeb();
            gameObject.Data = gameData;
            gameObject.GdbId = MasterRecord.gid;

            // check for manuals
            if (_GlobalSettings.scrapeManuals == true)
            {
                if (gameObject.Manuals == null)
                    gameObject.Manuals = new List<string>();

                gameObject.Manuals = MasterRecord.Game_Docs;
            }
            

            // enumerate globalsettings
            switch (_GlobalSettings.primaryScraper)
            {
                case 1:
                    // gamesdb.net is primary scraper
                    GDBScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord, message);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        MobyScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord, message);
                    break;
                case 2:
                    // moby is primary scraper
                    MobyScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord, message);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        GDBScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord, message);
                    break;
            }

            if (controller.IsCanceled == true)
            {
                controller.CloseAsync();
                return;
            }

            // gameObject should now be populated - create folder structure on disk if it does not already exist
            controller.SetMessage(message + "Determining local folder structure");
            ScrapeDB.CreateFolderStructure(gameObject.GdbId);

            // save the object to json
            controller.SetMessage(message + "Saving game information");
            ScrapeDB.SaveJson(gameObject);

            // Download all the files
            if (_GlobalSettings.scrapeBanners == true || _GlobalSettings.scrapeBoxart == true || _GlobalSettings.scrapeFanart == true || _GlobalSettings.scrapeManuals == true ||
                _GlobalSettings.scrapeMedia == true || _GlobalSettings.scrapeScreenshots == true)
            {
                controller.SetMessage(message + "Downloading media");
                ContentDownloadManager(gameObject, controller, message + "Downloading media...\n");
            }


            // Populate library data
            PopulateLibraryData(GameId, gameObject.GdbId);
            //CreateDatabaseLink(GameId, gameObject.GdbId);
            
            
        }

        /// <summary>
        /// Game scraping logic
        /// </summary>
        /// <param name="controller"></param>
        public ScrapedGameObjectWeb ScrapeGameInspector(ProgressDialogController controller)
        {
            string message = "";

            // create data object for results that are returned
            //ScrapeDB glsc = new ScrapeDB();
            ScrapedGameData gameData = new ScrapedGameData();
            ScrapedGameObjectWeb gameObject = new ScrapedGameObjectWeb();
            gameObject.Data = gameData;
            gameObject.GdbId = MasterRecord.gid;

            // check for manuals
            if (_GlobalSettings.scrapeManuals == true)
            {
                if (gameObject.Manuals == null)
                    gameObject.Manuals = new List<string>();

                gameObject.Manuals = MasterRecord.Game_Docs;
            }


            // enumerate globalsettings
            switch (_GlobalSettings.primaryScraper)
            {
                case 1:
                    // gamesdb.net is primary scraper
                    GDBScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord, message);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        MobyScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord, message);
                    break;
                case 2:
                    // moby is primary scraper
                    MobyScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord, message);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        GDBScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord, message);
                    break;
            }

            if (controller.IsCanceled == true)
            {
                controller.CloseAsync();
                return null;
            }

            // gameObject should now be populated - create folder structure on disk if it does not already exist
            controller.SetMessage(message + "Determining local folder structure");
            ScrapeDB.CreateFolderStructure(gameObject.GdbId);

            // save the object to json
            controller.SetMessage(message + "Saving game information");
            ScrapeDB.SaveJson(gameObject);

            // Download all the files
            if (_GlobalSettings.scrapeBanners == true || _GlobalSettings.scrapeBoxart == true || _GlobalSettings.scrapeFanart == true || _GlobalSettings.scrapeManuals == true ||
                _GlobalSettings.scrapeMedia == true || _GlobalSettings.scrapeScreenshots == true)
            {
                controller.SetMessage(message + "Downloading media");
                ContentDownloadManager(gameObject, controller, message + "Downloading media...\n");
            }


            // Return data
            return gameObject;
            //PopulateLibraryData(GameId, gameObject.GdbId);
            //CreateDatabaseLink(GameId, gameObject.GdbId);


        }

        public void ScrapeGame(ProgressDialogController controller)
        {
            ScrapeGame(controller, "");
        }

        /// <summary>
        /// Takes a finished ScrapedGameObject and downloads all media present in it
        /// </summary>
        /// <param name="o"></param>
        /// <param name="controller"></param>
        public static void ContentDownloadManager(ScrapedGameObjectWeb o, ProgressDialogController controller, string message)
        {
            string baseDir = ScrapeDB.BaseContentDirectory + @"\" + o.GdbId.ToString() + @"\";
            int total;
            int count;
            
            // screenshots
            if (o.Screenshots != null && o.Screenshots.Count > 0)
            {
                total = o.Screenshots.Count;
                count = 1;
            controller.SetMessage(message + "Downloading Screenshots for: " + o.Data.Title);
            foreach (string s in o.Screenshots)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nScreenshot: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "Screenshots");
                    count++;
                }
            }

            // fanart
            if (o.FanArts != null && o.FanArts.Count > 0)
            {
                total = o.FanArts.Count;
                count = 1;
                controller.SetMessage(message + "Downloading FanArt for: " + o.Data.Title);
                foreach (string s in o.FanArts)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nFanart: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "FanArt");
                    count++;
                }
            }

            // medias
            if (o.Medias != null && o.Medias.Count > 0)
            {
                total = o.Medias.Count;
                count = 1;
                controller.SetMessage(message + "Downloading Media for: " + o.Data.Title);
                foreach (string s in o.Medias)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nMedia: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "Media");
                    count++;
                }
            }

            // front boxart
            if (o.FrontCovers != null && o.FrontCovers.Count > 0)
            {
                total = o.FrontCovers.Count;
                count = 1;
                controller.SetMessage(message + "Downloading Front Box Art for: " + o.Data.Title);
                foreach (string s in o.FrontCovers)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nFront Cover: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "FrontCover");
                    count++;
                }
            }

            // back boxart
            if (o.BackCovers != null && o.BackCovers.Count > 0)
            {
                controller.SetMessage(message + "Downloading Back Box Art for: " + o.Data.Title);
                total = o.BackCovers.Count;
                count = 1;
                foreach (string s in o.BackCovers)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nBack Cover: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "BackCover");
                    count++;
                }
            }

            // banners
            if (o.Banners != null && o.Banners.Count > 0)
            {
                total = o.Banners.Count;
                count = 1;
                controller.SetMessage(message + "Downloading Banners for: " + o.Data.Title);
                foreach (string s in o.Banners)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nBanner: " + count + " of " + total + "\n(" + s + ")");
                    DownloadFile(s, baseDir + "Banners");
                    count++;
                }
            }

            // manuals
            if (o.Manuals != null && o.Manuals.Count > 0)
            {
                total = o.Manuals.Count;
                count = 1;
                controller.SetMessage(message + "Downloading Manuals for: " + o.Data.Title);
                foreach (string s in o.Manuals)
                {
                    controller.SetMessage(message + "Downloading content for: " + o.Data.Title + "\nManual: " + count + " of " + total + "\n(" + s + ")");

                    DownloadFile(s, baseDir + "Manual");
                    count++;
                }
            }
        }

        /// <summary>
        /// Download an individual file
        /// </summary>
        public static void DownloadFile(string url, string localdir)
        {
            string local = localdir + @"\";
            string fileName = Path.GetFileName(new Uri(url).AbsolutePath);

            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 30000;              
                try
                {
                    // Try to extract the filename from the Content-Disposition header
                    var data = wc.DownloadData(url);
                    if (!String.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                    {
                        fileName = wc.ResponseHeaders["Content-Disposition"].Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 10).Replace("\"", "");
                    }
                    if (!File.Exists(local + fileName))
                    {
                        //wc.DownloadFile(url, local + fileName);
                        File.WriteAllBytes(local + fileName, data);
                    }
                    else
                    {
                        // file already exists - skip
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //wc.Dispose();
                    return;
                }
                finally { wc.Dispose(); }
            }
        }
        
       /*
       
        public async void StartGameScrape(ProgressDialogController controller)
        {
            if (controller == null)
            {
                // create UI dialog
                var mySettings = new MetroDialogSettings()
                {
                    NegativeButtonText = "Cancel Scraping",
                    AnimateShow = false,
                    AnimateHide = false
                };
                controller = await mw.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);
                controller.SetCancelable(true);
                await Task.Delay(100);
            }

            await Task.Run(() =>
            {
                ScrapeGame(controller);
            });
            */
            /*
            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
            }

            await Task.Delay(300);
            */
            //GamesLibraryVisualHandler.RefreshGamesLibrary();

       // }
        /// <summary>
        /// Extension method to handle no controller being passed
        /// </summary>
        /// 
        /*
        public void StartGameScrape()
        {
            StartGameScrape(null);
        }
        */
    


        /// <summary>
        /// Creates a new entry in the GDBLink table (linking MedLaunch gameId to scraped data (thegamesdb.net) ID)
        /// also deletes any rows that have the same GameId (so that duplicates are not possible)
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gamesDbId"></param>
        /// 
        /*
        public static void CreateDatabaseLink(int gameId, int gamesDbId)
        {
            GDBLink link = new GDBLink();
            link.GameId = gameId;
            link.GdbId = gamesDbId;
            // delete any existing links with the same GameId
            GDBLink l = GDBLink.GetRecord(gameId);
            if (l != null)
            {
                GDBLink.DeleteRecord(l);
            }
            GDBLink.SaveToDatabase(link);
            PopulateLibraryData(link);
            GameListBuilder.UpdateFlag();
        }
        */

        /// <summary>
        /// create data in the LibraryDataGDBLink table
        /// </summary>
        /// <param name="link"></param>
        public static void PopulateLibraryData(int gameId, int gdbId)
        {
            var data = LibraryDataGDBLink.GetLibraryData(gdbId);
            if (data == null)
                data = new LibraryDataGDBLink();

            //ScrapeDB gd = new ScrapeDB();
            ScrapedGameObject o = ScrapeDB.GetScrapedGameObject(gameId, gdbId);

            data.GDBId = o.GdbId;
            data.Coop = o.Data.Coop;
            data.Developer = o.Data.Developer;
            data.ESRB = o.Data.ESRB;
            data.Players = o.Data.Players;
            data.Publisher = o.Data.Publisher;
            data.Year = o.Data.Released;

            // save library data
            LibraryDataGDBLink.SaveToDataBase(data);

            // set isScraped flag in Game table
            Game ga = Game.GetGame(gameId);
            ga.isScraped = true;
            ga.gdbId = gdbId;
            Game.SetGame(ga);
        }

        public static void UnlinkGameData(DataGrid dgGameList)
        {
            // get selected row
            var row = (GamesLibraryModel)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return;
            }
            int GameId = row.ID;
            
            Game.SetGdbId(GameId, 0);

            //GameListBuilder.UpdateFlag();
            GamesLibraryVisualHandler.UpdateSidebar(GameId);
        }


        private class CustomWebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                ((HttpWebRequest)lWebRequest).KeepAlive = false;
                return lWebRequest;
            }
        }
    }

    public enum ScrapeType
    {
        RescrapeAll,
        ScrapeSystem,
        RescrapeSystem,
        Favorites,
        RescrapeFavorites,
        Selected,
        SelectedRescrape
    }
}
