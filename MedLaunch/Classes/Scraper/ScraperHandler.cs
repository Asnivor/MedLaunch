using MahApps.Metro.Controls.Dialogs;
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

        /* Methods */

        /// <summary>
        /// Game scraping logic
        /// </summary>
        /// <param name="controller"></param>
        public void ScrapeGame(ProgressDialogController controller)
        {
            // create data object for results that are returned
            GamesLibraryScrapedContent glsc = new Classes.GamesLibraryScrapedContent();
            ScrapedGameData gameData = new ScrapedGameData();
            ScrapedGameObjectWeb gameObject = new ScrapedGameObjectWeb();
            gameObject.Data = gameData;
            gameObject.GdbId = MasterRecord.GamesDbId;

            // enumerate globalsettings
            switch (_GlobalSettings.primaryScraper)
            {
                case 1:
                    // gamesdb.net is primary scraper
                    GDBScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        MobyScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord);
                    break;
                case 2:
                    // moby is primary scraper
                    MobyScraper.ScrapeGame(gameObject, ScraperOrder.Primary, controller, MasterRecord);
                    if (_GlobalSettings.enabledSecondaryScraper == true)
                        GDBScraper.ScrapeGame(gameObject, ScraperOrder.Secondary, controller, MasterRecord);
                    break;
            }

            // gameObject should now be populated - create folder structure on disk if it does not already exist
            controller.SetMessage("Determining local folder structure");
            glsc.CreateFolderStructure(gameObject.GdbId);

            // save the object to json
            controller.SetMessage("Saving game information");
            glsc.SaveJson(gameObject);

            // Download all the files
            controller.SetMessage("Downloading media");
            ContentDownloadManager(gameObject, controller, glsc, "Downloading media...\n");

            // Create / Update GDBLink table
            CreateDatabaseLink(GameId, gameObject.GdbId);
            
            
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
            string filename = Path.GetFileName(new Uri(url).AbsolutePath);
            string localFile = local + filename;

            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 30000;
                try
                {
                    if (!File.Exists(localFile))
                    {
                        wc.DownloadFile(url, localFile);
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
}
