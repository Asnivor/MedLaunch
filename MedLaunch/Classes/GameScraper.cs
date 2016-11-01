using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using MedLaunch.Classes.TheGamesDB;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using FuzzyString;
using System.Windows;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Threading;
using MedLaunch.Classes.MasterScraper;

namespace MedLaunch.Classes
{
    public class GameScraper
    {
        // Properties
        public List<ScraperMaster> PlatformGames { get; set; }
        public List<ScraperMaster> SystemCollection { get; set; }
        public List<ScraperMaster> WorkingSearchCollection { get; set; }
        public List<ScraperMaster> SearchCollection { get; set; }
        public List<MedLaunch.Models.Game> LocalGames { get; set; }

        public string SearchString { get; set; }
        public bool LocalGameFound { get; set; }
        public int LocalIterationCount { get; set; }
        public int ManualIterator { get; set; }
        public int CurrentLocalGameId { get; set; }
        public GlobalSettings gs { get; set; }

        public App _applciation { get; set; }

        // Constructors
        public GameScraper()
        {
            _applciation = ((App)Application.Current);
            PlatformGames = _applciation.ScrapedData.MasterPlatformList;
            //RefreshPlatformGamesFromDb();
            LocalGameFound = false;
            LocalIterationCount = 0;
            ManualIterator = 0;
            SearchCollection = new List<ScraperMaster>();
            WorkingSearchCollection = new List<ScraperMaster>();
            gs = GlobalSettings.GetGlobals();
            
        }

        // Methods

        public async static void ScrapeGames(int systemId)
        {
            // instantiate instance of this class
            ScraperMainSearch gs = new ScraperMainSearch();

            // get mainwindow 
            MainWindow MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // popup progress dialog
            CancellationTokenSource cs = new CancellationTokenSource();
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = false,
                CancellationToken = cs.Token
            };
            var controller = await MWindow.ShowProgressAsync("Scraping Data", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                controller.SetMessage("Getting Local Games");
                

                // get all local games for this system already in the database
                gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).ToList();

                // get all games that have been matched but not yet scraped.
                List<GDBLink> link = new List<GDBLink>();
                using (var context = new MyDbContext())
                {
                    link = context.GDBLink.ToList();
                }               

                if (link.Count > 0)
                {
                    foreach (var g in link)
                    {
                        if (controller.IsCanceled)
                        {
                            controller.CloseAsync();
                            return;
                        }
                        var gam = (from a in gs.LocalGames
                                  where (a.gameId == g.GameId && a.systemId == systemId)
                                  select a).ToList();
                        // remove these games so they are not re-scanned
                        foreach (var gj in gam)
                        {
                            gs.LocalGames.Remove(gj);
                        }
                    }
                }



                // count number of games to scan for
                int numGames = gs.LocalGames.Count;
                controller.Minimum = 0;
                controller.Maximum = numGames;
                int i = 0;
                // iterate through each local game and attempt to match it with the gamesdb.net list
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
                    List<ScraperMaster> results = gs.SearchGameLocal(g.gameName, systemId, g.gameId).ToList();
                                       
                    if (results.Count == 0)
                    {
                        // no results returned
                    }
                    if (results.Count == 1)
                    {
                        // one result returned - create / update record in data table
                        ScraperMaster plat =  results.ToList().Single();
                        GDBGameData data = new GDBGameData();
                        data.GdbId = plat.GamesDbId;
                        data.Title = plat.TGDBData.GamesDBTitle;
                        GDBGameData.SaveToDatabase(data);

                        // create entry (or update existing) in the GDBLink table
                        GDBLink l = new GDBLink();
                        l.GdbId = data.GdbId;
                        l.GameId = g.gameId;
                        GDBLink.SaveToDatabase(l);
                    }
                    
                        

                }

                /* Begin actual scraping */

                // Get all games that have been matched but still require scraping
                List<GDBGameData> ga = new List<GDBGameData>();
                controller.SetMessage("Calculating games that require scraping...");
                using (var db = new MyDbContext())
                {
                    ga = (from a in db.GDBGameData
                          where (a.Platform != null || a.Platform != "" || a.Overview != null || a.Overview != "")
                          select a).ToList();
                }
                if (ga.Count == 0)
                {
                    return;
                }

                // now limit to just the systemId that we have specified

                // reset localgames
                gs.LocalGames = MedLaunch.Models.Game.GetGames(systemId).ToList();
                List<GDBGameData> gb = new List<GDBGameData>();
                gb = ga.ToList();
                foreach (GDBGameData g in ga)
                {
                    int gdbId = g.GdbId;
                    // lookup in the link table
                    var li = link.Where(a => a.GdbId == gdbId).FirstOrDefault(); 
                    if (li != null)
                    {
                        var actualGame = gs.LocalGames.Where(a => a.gameId == li.GameId).FirstOrDefault();
                        if (actualGame == null || actualGame.systemId != systemId)
                        {
                            // we dont want this game
                            gb.Remove(g);
                        }
                    }                   
                    
                }
                ga = gb.ToList();

                int gamesCount = ga.Count;
                i = 0;
                controller.Minimum = 0;
                controller.Maximum = gamesCount;
                foreach (GDBGameData game in ga)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return;
                    }
                    // iterate through each game that requires scraping and attempt to download the data and import to database
                    i++;
                    controller.SetProgress(i);
                    string message = "Connecting to thegamesdb.net....\nGetting data for: " + game.Title + "\n(" + i + " of " + gamesCount + ")";
                    controller.SetMessage(message);
                    //gs.ScrapeGame(game.GdbId, controller, message);
                }


            });


            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Cancelled");
            }
            else
            {
                await MWindow.ShowMessageAsync("MedLaunch Scraper", "Scraping Completed");
            }

        }

        public void ScrapeGame(int GdbId, ProgressDialogController p, string currentMessage)
        {

            /*
            // does a json file already exist locally?
            if (DoesJsonFileExist(GdbId) ==  true)
            {
                // local json store of the class data already exists - use this instead
                GDBGameData newG = LoadJson(GdbId);
                GDBGameData.SaveToDatabase(newG);
                return;
            }
            */

            

            // attempt connection
            try
            {
                GDBNETGame g = new GDBNETGame();
                // does a external.xml file exist locally (containing saved info from thegamesdb query)
                if (DoesXmlExternalFileExist(GdbId) == true)
                {
                    p.SetMessage(currentMessage + "\n(Using local URL cache)");
                    string localExtPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Graphics\\thegamesdb\\" + GdbId + "\\" + GdbId + "-external.xml";
                    string gString = File.ReadAllText(localExtPath);
                    g = GDBNETGamesDB.GetGame(gString);
                }
                else
                {
                    // go get the data from thegamesdb.net
                    g = GDBNETGamesDB.GetGame(GdbId);
                }
                
                if (g == null)
                {
                    // return
                    return;
                }                

                // populate object
                GDBGameData gd = new GDBGameData();
                gd.GdbId = g.ID;
                gd.Overview = g.Overview;
                gd.Title = g.Title;
                gd.Platform = g.Platform;
                gd.ReleaseDate = g.ReleaseDate;
                gd.ESRB = g.ESRB;
                gd.Players = g.Players;
                gd.Publisher = g.Publisher;
                gd.Developer = g.Developer;
                gd.Rating = g.Rating;
                gd.Coop = g.Coop;
                gd.AlternateTitles = GDBGameData.JsonSerialize(g.AlternateTitles);
                gd.Genres = GDBGameData.JsonSerialize(g.Genres);

                // create folder structure for images (if it is needed)
                CreateFolderStructure(g.ID);

                // save the returned gamesdb data to a json file in the game directory - so we can use this to download content rather than the initial connection to thegamesdb.net   
                //string gJson = JsonConvert.SerializeObject(g, Formatting.Indented);
                //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Data\\Graphics\\thegamesdb\\" + g.ID + "\\" + g.ID + "-external.json", gJson);    

                // download images
                var images = g.Images;
                if (images.BoxartFront == null) { }
                else
                {
                    if (gs.scrapeBoxart == true)
                    {
                        gd = DownloadFile(images.BoxartFront.Path, g.ID, gd);
                        p.SetMessage(currentMessage + "\nDownloading BoxArt (Front)");
                    }                    
                }
                if (images.BoxartBack == null) { }
                else
                {
                    if (gs.scrapeBoxart == true)
                    {
                        gd = DownloadFile(images.BoxartBack.Path, g.ID, gd);
                        p.SetMessage(currentMessage + "\nDownloading BoxArt (Back)");
                    }
                    
                }
                int numCount = 0;
                int ic = 0;

                if (images.Screenshots.Count > 0)
                {
                    if (gs.scrapeScreenshots == true)
                    {
                        List<string> s1 = new List<string>();
                        numCount = images.Screenshots.ToList().Count;
                        ic = 0;
                        foreach (var im in images.Screenshots)
                        {
                            ic++;
                            string[] fArr = im.Path.Split('/');
                            string filename = fArr[fArr.Length - 1];
                            gd = DownloadFile(im.Path, g.ID, gd);
                            s1.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\Screenshots\\" + filename);
                            p.SetMessage(currentMessage + "\nDownloading Screenshots\n(" + ic + " of " + numCount + ")");
                        }
                        gd.ScreenshotLocalImages = GDBGameData.JsonSerialize(s1);
                    }
                    
                }
                if (images.Fanart.Count > 0)
                {
                    if (gs.scrapeFanart == true)
                    {
                        List<string> s2 = new List<string>();
                        numCount = images.Fanart.ToList().Count;
                        ic = 0;
                        foreach (var im in images.Fanart)
                        {
                            ic++;
                            string[] fArr = im.Path.Split('/');
                            string filename = fArr[fArr.Length - 1];
                            gd = DownloadFile(im.Path, g.ID, gd);
                            s2.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\FanArt\\" + filename);
                            p.SetMessage(currentMessage + "\nDownloading FanArt\n(" + ic + " of " + numCount + ")");
                        }
                        gd.FanartLocalImages = GDBGameData.JsonSerialize(s2);
                    }
                    
                }
                if (images.Banners.Count > 0)
                {
                    if (gs.scrapeBanners == true)
                    {
                        List<string> s3 = new List<string>();
                        numCount = images.Banners.ToList().Count;
                        ic = 0;
                        foreach (var im in images.Banners)
                        {
                            ic++;
                            string[] fArr = im.Path.Split('/');
                            string filename = fArr[fArr.Length - 1];
                            gd = DownloadFile(im.Path, g.ID, gd);
                            s3.Add("Data\\Graphics\\thegamesdb\\" + g.ID + "\\Banners\\" + filename);
                            p.SetMessage(currentMessage + "\nDownloading Banners\n(" + ic + " of " + numCount + ")");
                        }
                        gd.BannerLocalImages = GDBGameData.JsonSerialize(s3);
                    }
                    
                }
                
                // now everything is downloaded, update the database and set the DateTime
                gd.LastScraped = DateTime.Now;
                

                GDBGameData.SaveToDatabase(gd);

                // export to Json file
                SaveJson(gd);


            }
            catch  (Exception ex)
            {

            } finally { }
        }

        public static bool DoesJsonFileExist(int gdbId)
        {
            string loadPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + gdbId + @"\" + gdbId + ".json";
            if (File.Exists(loadPath))
            {
                return true;
            }
            return false;
        }
        public static bool DoesXmlExternalFileExist(int gdbId)
        {
            string loadPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + gdbId + @"\" + gdbId + "-external.xml";
            if (File.Exists(loadPath))
            {
                return true;
            }
            return false;
        }
        public static void SaveJson(GDBGameData j)
        {
            string savePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + j.GdbId + @"\" + j.GdbId + ".json";            
            string json = JsonConvert.SerializeObject(j);
            System.IO.File.WriteAllText(savePath, json);
        }
        public static GDBGameData LoadJson(int gdbId)
        {
            GDBGameData g = new GDBGameData();
            string loadPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Graphics\thegamesdb\" + gdbId + @"\" + gdbId + ".json";
            if (File.Exists(loadPath))
            {
                string json = System.IO.File.ReadAllText(loadPath);
                g = JsonConvert.DeserializeObject<GDBGameData>(json);
            }
            return g;            
        }

        public static GDBGameData DownloadFile(string url, int gdbId, GDBGameData gd)
        {
            // decide where it should be saved locally
            string lPath = @"Data\Graphics\thegamesdb\" + gdbId.ToString() + @"\";
            string local = "";
            string[] fArr = url.Split('/');
            string filename = fArr[fArr.Length - 1];
            if (url.Contains("boxart/original/back")) { local = lPath + @"boxartback\" + filename; }
            if (url.Contains("boxart/original/front")) { local = lPath + @"boxartfront\" + filename; }
            if (url.Contains("fanart/")) { local = lPath + @"fanart\" + filename; }
            if (url.Contains("screenshots/")) { local = lPath + @"screenshots\" + filename; }
            if (url.Contains("graphical/")) { local = lPath + @"banners\" + filename; }

            // if localpath is "" return
            if (local == "") { return gd; }
            using (var wc = new CustomWebClient())
            {
                wc.Proxy = null;
                wc.Timeout = 30000;
                try
                {
                    if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local))
                    {
                        // file not exists 
                        wc.DownloadFile(GDBNETGamesDB.BaseImgURL + url, System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local);
                    }
                }
                catch (Exception ex)
                {
                    // error downloading the file
                    wc.Dispose();
                }
                finally
                {
                    wc.Dispose();
                }
                /*
                if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local))
                {
                    // file not exists  
                    wc.Proxy = null;                  
                    wc.DownloadFile(GDBNETGamesDB.BaseImgURL + url, System.AppDomain.CurrentDomain.BaseDirectory + "\\" + local);                    
                }
                */
               
                // add to object
                if (local.Contains("boxartfront"))
                    { gd.BoxartFrontLocalImage = lPath + @"boxartfront\" + filename; }

                if (local.Contains("boxartback"))
                    gd.BoxartBackLocalImage = lPath + @"boxartback\" + filename;
            }
            return gd;
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

        public static void CreateFolderStructure(int gdbId)
        {
            string basePath = @"Data\Graphics\thegamesdb\" + gdbId.ToString() + @"\";
            // boxart
            System.IO.Directory.CreateDirectory(basePath + "boxartfront");
            System.IO.Directory.CreateDirectory(basePath + "boxartback");
            System.IO.Directory.CreateDirectory(basePath + "banners");
            System.IO.Directory.CreateDirectory(basePath + "screenshots");
            System.IO.Directory.CreateDirectory(basePath + "fanart");

        }

        

        

        

        

        

        

        


        

        

        
        

        // refresh platformgames from db
        public void RefreshPlatformGamesFromDb()
        {
            //PlatformGames = GDBPlatformGame.GetGames();
        }

        // Update database with all platform games
        public static List<GDBPlatformGame> DatabasePlatformGamesImport(ProgressDialogController controller)
        {          

            // create a new object for database import
            List<GDBPlatformGame> gs = new List<GDBPlatformGame>();
            int count = 0;
            int sysCount = GSystem.GetSystems().Count - 3;

            if (controller != null)
            {
                controller.Minimum = 0;
                controller.Maximum = sysCount;
            }
            
            foreach (GSystem sys in GSystem.GetSystems())
            {
                if (controller.IsCanceled)
                {
                    controller.CloseAsync();
                    return new List<GDBPlatformGame>();
                }
                // skip systems that are not needed
                if (sys.systemId == 16 || sys.systemId == 17 || sys.systemId == 18)
                    continue;
                count++;
                List<GDBNETGameSearchResult> merged = new List<GDBNETGameSearchResult>();

                if (controller != null)
                {
                    controller.SetProgress(Convert.ToDouble(count));
                    controller.SetMessage("Retrieving Game List for Platform: " + sys.systemName);
                    //controller.SetIndeterminate();
                }


                // perform lookups
                foreach (int gid in sys.theGamesDBPlatformId)
                {
                    if (controller.IsCanceled)
                    {
                        controller.CloseAsync();
                        return new List<GDBPlatformGame>();
                    }
                    List<GDBNETGameSearchResult> result = GDBNETGamesDB.GetPlatformGames(gid).ToList();
                    if (result.Count == 0)
                    {
                        // nothing returned
                        if (controller != null)
                        {
                            controller.SetMessage("No results returned.\n Maybe an issue connecting to thegamesdb.net...");
                            Task.Delay(2000);
                        }
                    }
                    
                    foreach (var r in result)
                    {
                        if (controller.IsCanceled)
                        {
                            controller.CloseAsync();
                            return null;
                        }
                        GDBPlatformGame gsingle = new GDBPlatformGame();
                        gsingle.id = r.ID;
                        gsingle.SystemId = sys.systemId;
                        gsingle.GameTitle = r.Title;
                        gsingle.GDBPlatformName = GSystem.ReturnGamesDBPlatformName(gid);
                        gsingle.ReleaseDate = r.ReleaseDate;

                        gs.Add(gsingle);
                    }
                    //merged.AddRange(result);
                }

                // remove duplicates
                gs.Distinct();
                //List<GDBNETGameSearchResult> nodupe = merged.Distinct().ToList();

                /*
                // convert to GDBPlatformGame format and add to top list
                foreach (var n in nodupe)
                {
                    GDBPlatformGame gsingle = new GDBPlatformGame();
                    gsingle.id = n.ID;
                    gsingle.SystemId = sys.systemId;
                    gsingle.GameTitle = n.Title;
                    gsingle.ReleaseDate = n.ReleaseDate;
                    gsingle.GDBPlatformName = GSystem.ReturnGamesDBPlatformName(Convert.ToInt32(n.Platform));

                    gs.Add(gsingle);
                }
                */
            }

            // now we have a complete list of games for our platforms from thegamesdb.net - add or update the database
            if (controller != null)
            {
                controller.SetMessage("Saving to Database...");
            }
            if (controller.IsCanceled)
            {
                return new List<GDBPlatformGame>();
            }
            else
            {
                return gs;
            }


            
          
                

            // first get the current data
            //List<GDBPlatformGame> current = GDBPlatformGame.GetGames();
        }

        public static List<GDBPlatformGame> DatabasePlatformGamesImport()
        {
            return DatabasePlatformGamesImport(null);
        }

        // get all games from the API based on gamesdb system ID
        public static ICollection<GDBNETGameSearchResult> GetPlatformGames(int systemId)
        {

            ICollection<GDBNETGameSearchResult> result = GDBNETGamesDB.GetPlatformGames(systemId);
            return result;
        }

        public static void SavePlatformGamesToDisk()
        {
            //var g = GDBPlatformGame.GetGames();
        }

        public static void UnlinkGameData(DataGrid dgGameList)
        {
            // get selected row
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem == null)
            {
                // game is not selected
                return;
            }
            int GameId = row.ID;

            // see if the game is linked in the GDBLink table
            List<GDBLink> link = GDBLink.GetRecords(GameId);
            if (link.Count > 0)
            {
                foreach (GDBLink g in link)
                {
                    GDBLink.DeleteRecord(g);
                }
            }

            GamesLibraryVisualHandler.UpdateSidebar(GameId);

        }

        
    }
}
