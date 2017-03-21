using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.MasterScraper;
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
        public ScraperMainSearch ScraperSearch { get; set; }
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
            ScraperSearch = new ScraperMainSearch();
            MasterRecord = ScraperSearch.GLSC.MasterPlatformList.Where(a => a.GamesDbId == gdbId).FirstOrDefault();
            _GlobalSettings = ScraperSearch._GlobalSettings;
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            GameId = gameId;
        }

        public ScraperHandler(int gdbId, int gameId, bool MainWindowRequired)
        {
            ScraperSearch = new ScraperMainSearch();
            MasterRecord = ScraperSearch.GLSC.MasterPlatformList.Where(a => a.GamesDbId == gdbId).FirstOrDefault();
            _GlobalSettings = ScraperSearch._GlobalSettings;
            if (MainWindowRequired == true)
                mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            GameId = gameId;
        }

        /* Methods */

        public async static void ScrapeGamesMultiple(List<GamesLibraryModel> list, ScrapeType scrapeType)
        {
            // instantiate instance of this class
            ScraperMainSearch gs = new ScraperMainSearch();

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
                gs.LocalGames = new List<Game>();

                // iterate through each game, look it up and pass it to gs.LocalGames
                foreach (var game in list)
                {
                    Game g = Game.GetGame(game.ID);

                    if (scrapeType == ScrapeType.Selected)
                    {
                        if (g.gdbId == null || g.gdbId == 0 || Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString()) == false)
                        {
                            // scraping can happen
                            gs.LocalGames.Add(g);
                        }
                    }

                    if (scrapeType == ScrapeType.SelectedRescrape)
                    {
                        gs.LocalGames.Add(g);
                    }                    
                }

                // count number of games to scan for
                int numGames = gs.LocalGames.Count;
                controller.Minimum = 0;
                controller.Maximum = numGames;
                int i = 0;
                // iterate through each local game and attempt to match it with the master list
                foreach (var g in gs.LocalGames)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }
                    i++;
                    controller.SetProgress(i);
                    controller.SetMessage("Attempting local search match for:\n" + g.gameName + " (" + GSystem.GetSystemCode(g.systemId) + ")" + "\n(" + i + " of " + numGames + ")");
                    List<ScraperMaster> results = gs.SearchGameLocal(g.gameName, g.systemId, g.gameId).ToList();

                    if (results.Count == 0)
                    {
                        // no results returned
                    }
                    if (results.Count == 1)
                    {
                        // one result returned - add GdbId to the Game table
                        Game.SetGdbId(g.gameId, results.Single().GamesDbId);
                    }
                }

                /* Begin actual scraping */

                // Get all games that have a GdbId set and determine if they need scraping (is json file present for them)  
                var gamesTmp = gs.LocalGames;
                gs.LocalGames = new List<Game>();
                foreach (var g in gamesTmp)
                {
                    if (g.gdbId == null || g.gdbId == 0)
                        continue;

                    if (scrapeType == ScrapeType.SelectedRescrape)
                    {
                        gs.LocalGames.Add(g);
                        continue;
                    }

                    // check each game directory
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString()))
                    {
                        // directory does not exist - scraping needed
                        gs.LocalGames.Add(g);
                    }
                    else
                    {
                        // directory does exist - check whether json file is present
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString() + @"\" + g.gdbId.ToString() + ".json"))
                        {
                            // json file is not present - scraping needed
                            gs.LocalGames.Add(g);
                        }

                    }
                }

                int gamesCount = gs.LocalGames.Count;
                i = 0;
                controller.Minimum = 0;
                controller.Maximum = gamesCount;
                foreach (var g in gs.LocalGames)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }

                    // iterate through each game that requires scraping and attempt to download the data and import to database
                    i++;
                    controller.SetProgress(i);
                    string message = "Scraping Started....\nGetting data for: " + g.gameName + " (" + GSystem.GetSystemCode(g.systemId) + ")" + "\n(" + i + " of " + gamesCount + ")\n\n";
                    controller.SetMessage(message);

                    // do actual scraping                    
                    ScraperHandler sh = new ScraperHandler(g.gdbId.Value, g.gameId, false);
                    sh.ScrapeGame(controller);
                    //GameListBuilder.UpdateFlag();
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

        public async static void ScrapeGames(int systemId, ScrapeType scrapeType)
        {
            // instantiate instance of this class
            ScraperMainSearch gs = new ScraperMainSearch();

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
                if (scrapeType == ScrapeType.System)
                {
                    controller.SetMessage("Getting Local Games that require Scraping");
                    // get all local games for this system where GdbId is not set or is 0
                    gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).Where(a => a.gdbId == null || a.gdbId == 0).ToList();
                }
                if (scrapeType == ScrapeType.RescrapeAll)
                {
                    controller.SetMessage("Getting ALL Local Games for Scraping");
                    // get all local games for this system regardless of whether they have been scraped or not
                    gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).ToList();
                }
                if (scrapeType == ScrapeType.Favorites)
                {
                    controller.SetMessage("Getting Favorites That Require Scraping");
                    // get all local games that are favorites where gdbid is not set or is 0
                    gs.LocalGames = MedLaunch.Models.Game.GetGames().Where(a => (a.gdbId == null || a.gdbId == 0 || Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + a.gdbId.ToString()) == false) && a.isFavorite == true).ToList();
                }
                if (scrapeType == ScrapeType.RescrapeFavorites)
                {
                    controller.SetMessage("Getting ALL Favorites for Scraping");
                    // get all local games that are favorites where gdbid is not set or is 0
                    gs.LocalGames = MedLaunch.Models.Game.GetGames().Where(a => a.isFavorite == true).ToList();
                }

                // count number of games to scan for
                int numGames = gs.LocalGames.Count;
                controller.Minimum = 0;
                controller.Maximum = numGames;
                int i = 0;
                // iterate through each local game and attempt to match it with the master list
                foreach (var g in gs.LocalGames)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }
                    i++;
                    controller.SetProgress(i);
                    controller.SetMessage("Attempting local search match for:\n" + g.gameName + "\n(" + i + " of " + numGames + ")");
                    List<ScraperMaster> results = gs.SearchGameLocal(g.gameName, g.systemId, g.gameId).ToList();

                    if (results.Count == 0)
                    {
                        // no results returned
                    }
                    if (results.Count == 1)
                    {
                        // one result returned - add GdbId to the Game table
                        Game.SetGdbId(g.gameId, results.Single().GamesDbId);                         
                    }
                }

                /* Begin actual scraping */

                // Get all games that have a GdbId set and determine if they need scraping (is json file present for them)                
                var gamesTmp = MedLaunch.Models.Game.GetGames(systemId).Where(a => a.gdbId != null && a.gdbId > 0).ToList();
                if (systemId == 0)
                {
                    gamesTmp = gs.LocalGames.Where(a => a.gdbId != null && a.gdbId > 0).ToList();
                }

                gs.LocalGames = new List<Game>();
                foreach (var g in gamesTmp)
                {
                    if (scrapeType == ScrapeType.RescrapeAll || scrapeType == ScrapeType.RescrapeFavorites)
                    {
                        gs.LocalGames.Add(g);
                        continue;
                    }


                    // check each game directory
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString()))
                    {
                        // directory does not exist - scraping needed
                        gs.LocalGames.Add(g);
                    }
                    else
                    {
                        // directory does exist - check whether json file is present
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + g.gdbId.ToString() + @"\" + g.gdbId.ToString() + ".json"))
                        {
                            // json file is not present - scraping needed
                            gs.LocalGames.Add(g);
                        }

                    }
                }
                
                int gamesCount = gs.LocalGames.Count;
                i = 0;
                controller.Minimum = 0;
                controller.Maximum = gamesCount;
                foreach (var g in gs.LocalGames)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }
                    
                    // iterate through each game that requires scraping and attempt to download the data and import to database
                    i++;
                    controller.SetProgress(i);
                    string message = "Scraping Started....\nGetting data for: " + g.gameName + "\n(" + i + " of " + gamesCount + ")\n\n";
                    controller.SetMessage(message);
                    
                    // do actual scraping                    
                    ScraperHandler sh = new ScraperHandler(g.gdbId.Value, g.gameId, false);
                    sh.ScrapeGame(controller);
                    //GameListBuilder.UpdateFlag();
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
            GamesLibraryScrapedContent glsc = new Classes.GamesLibraryScrapedContent();
            ScrapedGameData gameData = new ScrapedGameData();
            ScrapedGameObjectWeb gameObject = new ScrapedGameObjectWeb();
            gameObject.Data = gameData;
            gameObject.GdbId = MasterRecord.GamesDbId;

            // check for manuals
            if (_GlobalSettings.scrapeManuals == true)
            {
                if (gameObject.Manuals == null)
                    gameObject.Manuals = new List<string>();

                gameObject.Manuals = MasterRecord.IDDBManual;
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

            // gameObject should now be populated - create folder structure on disk if it does not already exist
            controller.SetMessage(message + "Determining local folder structure");
            glsc.CreateFolderStructure(gameObject.GdbId);

            // save the object to json
            controller.SetMessage(message + "Saving game information");
            glsc.SaveJson(gameObject);

            // Download all the files
            if (_GlobalSettings.scrapeBanners == true || _GlobalSettings.scrapeBoxart == true || _GlobalSettings.scrapeFanart == true || _GlobalSettings.scrapeManuals == true ||
                _GlobalSettings.scrapeMedia == true || _GlobalSettings.scrapeScreenshots == true)
            {
                controller.SetMessage(message + "Downloading media");
                ContentDownloadManager(gameObject, controller, glsc, message + "Downloading media...\n");
            }


            // Populate library data
            PopulateLibraryData(GameId, gameObject.GdbId);
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
        public static void ContentDownloadManager(ScrapedGameObjectWeb o, ProgressDialogController controller, GamesLibraryScrapedContent glsc, string message)
        {
            string baseDir = glsc.BaseContentDirectory + @"\" + o.GdbId.ToString() + @"\";
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
                    wc.Dispose();
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

            GamesLibraryScrapedContent gd = new GamesLibraryScrapedContent();
            ScrapedGameObject o = gd.GetScrapedGameObject(gameId, gdbId);

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
        System,
        Favorites,
        RescrapeFavorites,
        Selected,
        SelectedRescrape
    }
}
