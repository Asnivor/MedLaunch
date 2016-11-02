using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System.IO;

namespace MedLaunch.Classes.TheGamesDB
{
    /// <summary>
    /// All scraping operations relating to thegamesdb.net
    /// </summary>
    public class GDBScraper
    {

        public static ScrapedGameObjectWeb ScrapeGame(ScrapedGameObjectWeb o, ScraperOrder order, ProgressDialogController controller, ScraperMaster masterrecord)
        {
            bool priority;
            string message;
            string BaseImgUrl = "http://thegamesdb.net/banners/";

            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (order == ScraperOrder.Primary)
            {
                controller.SetMessage("Primary Scraping (thegamesdb.net)\nDownloading information for: " + masterrecord.TGDBData.GamesDBTitle + "\n(" + masterrecord.TGDBData.GamesDBPlatformName + ")");
                priority = true;    // primary
                message = 
                o.Data.Title = masterrecord.TGDBData.GamesDBTitle;
                o.Data.Platform = masterrecord.TGDBData.GamesDBPlatformName;
            }
            else
            {
                // GDB is secondary scraper
                priority = false;    // primary
                if (o.Data.Title == null)
                    o.Data.Title = masterrecord.TGDBData.GamesDBTitle;
                if (o.Data.Platform == null)
                    o.Data.Platform = masterrecord.TGDBData.GamesDBPlatformName;
            }
            
            if (priority == true)
            {
                /* Primary Scraping */

                // get the text data from thegamesdb.net
                GDBNETGame g = new GDBNETGame();
                g = GDBNETGamesDB.GetGame(o.GdbId);

                if (g == null)
                {
                    // Nothing was returned
                    return o;
                }

                o.Data.AlternateTitles = g.AlternateTitles;
                o.Data.Coop = g.Coop;
                o.Data.Developer = g.Developer;
                o.Data.ESRB = g.ESRB;
                o.Data.Genres = g.Genres;
                o.Data.Overview = g.Overview;
                o.Data.Players = g.Players;
                o.Data.Publisher = g.Publisher;
                o.Data.Released = g.ReleaseDate;
                if (gs.scrapeBoxart == true)
                {
                    o.BackCovers.Add(BaseImgUrl + g.Images.BoxartBack.Path);
                    o.FrontCovers.Add(BaseImgUrl + g.Images.BoxartFront.Path);
                }
                if (gs.scrapeBanners == true)
                {
                    foreach (var s in g.Images.Banners)
                    {
                        o.Banners.Add(BaseImgUrl + s.Path);
                    }
                }
                if (gs.scrapeFanart == true)
                {
                    foreach (var s in g.Images.Fanart)
                    {
                        o.FanArts.Add(BaseImgUrl + s.Path);
                    }
                }
                if (gs.scrapeScreenshots == true)
                {
                    foreach (var s in g.Images.Screenshots)
                    {
                        o.Screenshots.Add(BaseImgUrl + s.Path);
                    }
                }         
            }
            else
            {
                /* secondary scraping */

                // get the text data from thegamesdb.net
                GDBNETGame g = new GDBNETGame();
                g = GDBNETGamesDB.GetGame(o.GdbId);

                if (g == null)
                {
                    // Nothing was returned
                    return o;
                }
                
                if (o.Data.AlternateTitles == null && g.AlternateTitles.Count > 0)
                    o.Data.AlternateTitles.AddRange(g.AlternateTitles);
                if (o.Data.Coop == null && g.Coop != null)
                    o.Data.Coop = g.Coop;
                if (o.Data.Developer == null && g.Developer != null)
                    o.Data.Developer = g.Developer;
                if (o.Data.ESRB == null && g.ESRB != null)
                    o.Data.ESRB = g.ESRB;
                if (o.Data.Genres.Count == 0 && g.Genres != null)
                    o.Data.Genres.AddRange(g.Genres);
                if (o.Data.Overview == null)
                    o.Data.Overview = g.Overview;
                if (o.Data.Players == null)
                    o.Data.Players = g.Players;
                if (o.Data.Publisher == null)
                    o.Data.Publisher = g.Publisher;
                if (o.Data.Released == null)
                    o.Data.Released = g.ReleaseDate;
                if (gs.scrapeBoxart == true)
                {
                    if (o.BackCovers.Count == 0 && g.Images.BoxartBack != null)
                        o.BackCovers.Add(BaseImgUrl + g.Images.BoxartBack.Path);
                    if (o.FrontCovers.Count == 0 && g.Images.BoxartFront != null)
                        o.FrontCovers.Add(BaseImgUrl + g.Images.BoxartFront.Path);
                }
                if (gs.scrapeBanners == true)
                {
                    if (o.Banners.Count == 0 && g.Images.Banners != null)
                    {
                        foreach (var s in g.Images.Banners)
                        {
                            o.Banners.Add(BaseImgUrl + s.Path);
                        }
                    }                        
                }
                if (gs.scrapeFanart == true)
                {
                    if (o.FanArts.Count == 0 && g.Images.Fanart != null)
                    {
                        foreach (var s in g.Images.Fanart)
                        {
                            o.FanArts.Add(BaseImgUrl + s.Path);
                        }
                    }                    
                }
                if (gs.scrapeScreenshots == true && g.Images.Screenshots != null)
                {
                    foreach (var s in g.Images.Screenshots)
                    {
                        o.Screenshots.Add(BaseImgUrl + s.Path);
                    }
                }

                // remove duplicates
                //o.Data.AlternateTitles.Distinct();
                o.Data.Genres.Distinct();
                o.Screenshots.Distinct();
                o.FanArts.Distinct();
                o.Banners.Distinct();
                o.BackCovers.Distinct();
                o.FrontCovers.Distinct();
            }

            return o;
        }

        /// <summary>
        /// Scrape the full master list (basic) of games from thegamesdb.net
        /// and save to json file in VS project (not bin).
        /// </summary>
        public static void ScrapeBasicGamesList()
        {
            ScrapeBasicGamesList(null);
        }
        public static void ScrapeBasicGamesList(ProgressDialogController controller)
        {
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
                    return;
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
                        return;
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
                            return;
                        }
                        GDBPlatformGame gsingle = new GDBPlatformGame();
                        gsingle.id = r.ID;
                        gsingle.SystemId = sys.systemId;
                        gsingle.GameTitle = r.Title;
                        gsingle.GDBPlatformName = GSystem.ReturnGamesDBPlatformName(gid);
                        gsingle.ReleaseDate = r.ReleaseDate;

                        gs.Add(gsingle);
                    }
                }
                // remove duplicates
                gs.Distinct();

                // now we have a complete list of games for our platforms from thegamesdb.net - update the local json file
                if (controller != null)
                {
                    controller.SetMessage("Saving to file...");
                }
                if (controller.IsCanceled)
                {
                    return;
                }

                string filePath = @"..\..\Data\System\TheGamesDB.json";
                string json = JsonConvert.SerializeObject(gs, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

    }
}
