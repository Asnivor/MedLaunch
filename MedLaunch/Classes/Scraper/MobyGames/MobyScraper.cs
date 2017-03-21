using HtmlAgilityPack;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.Scraper;
using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    /// <summary>
    /// All scraping operations relating to mobygames
    /// </summary>
    public class MobyScraper
    {

        public static ScrapedGameObjectWeb ScrapeGame(ScrapedGameObjectWeb o, ScraperOrder order, ProgressDialogController controller, ScraperMaster masterrecord, string message)
        {
            bool priority;
            message = message + "Downloading information for: " + masterrecord.MobyData.MobyTitle + "\n(" + masterrecord.MobyData.MobyPlatformName + ")";
            if (order == ScraperOrder.Primary) { message = "Primary Scraping (mobygames)\n" + message; }
            else { message = "Secondary Scraping (mobygames)\n" + message; }
            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (order == ScraperOrder.Primary)
            {
                
                priority = true;    // primary
                if (masterrecord.MobyData.MobyTitle != null)
                {
                    // moby data has been matched
                    
                    controller.SetMessage(message);
                    o.Data.Title = masterrecord.MobyData.MobyTitle;
                    o.Data.Platform = masterrecord.MobyData.MobyPlatformName;
                }
                else
                {
                    // no moby data matched - use gamesdb title and platform and return
                    o.Data.Title = masterrecord.TGDBData.GamesDBTitle;
                    o.Data.Platform = masterrecord.TGDBData.GamesDBPlatformName;
                    return o;
                }                
            }
            else
            {
                // moby scraping is secondary
                if (masterrecord.MobyData.MobyTitle == null)
                {
                    o.Data.Title = masterrecord.TGDBData.GamesDBTitle;
                    o.Data.Platform = masterrecord.TGDBData.GamesDBPlatformName;
                    return o;
                }
                priority = false;    // primary
            }

            if (priority == true)
            {
                // primary scraping
                o = PullWebpageData(o, masterrecord, controller, ScraperOrder.Primary, message);
            }
            else
            {
                // secondary scraping
                o = PullWebpageData(o, masterrecord, controller, ScraperOrder.Secondary, message);
            }

            return o;
        }

        /// <summary>
        /// Master handler for parsing data from the mobygames website
        /// </summary>
        /// <param name="o"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static ScrapedGameObjectWeb PullWebpageData(ScrapedGameObjectWeb o, ScraperMaster masterrecord, ProgressDialogController controller, ScraperOrder order, string message)
        {
            // query the main game page
            string baseurl = "http://www.mobygames.com/game/";
            string param = masterrecord.MobyData.MobyPlatformName + "/" + masterrecord.MobyData.MobyURLName;
            string initialPage = ReturnWebpage(baseurl, param, 10000);

            GlobalSettings gs = GlobalSettings.GetGlobals();

            // convert page string to htmldoc
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(initialPage);            

            // get core information
            List<HtmlNode> divs = doc.DocumentNode.SelectNodes("//div").ToList();

            //List<HtmlNode> coreGenreList = coreGenre.ChildNodes.ToList();
            int divcount = divs.Count;
            for (int i = 0; i < divcount; i++)
            {
                // get just headings
                if (divs[i].InnerText == divs[i].InnerHtml)
                {
                    // this is just a heading - i+1 should give the value
                    if (divs[i].InnerHtml == "Published by")
                    {
                        if (order == ScraperOrder.Primary || o.Data.Publisher == null)
                        {
                            o.Data.Publisher = WebUtility.HtmlDecode(divs[i + 1].InnerText);
                        }                        
                    }
                    if (divs[i].InnerHtml == "Developed by")
                    {
                        if (order == ScraperOrder.Primary || o.Data.Developer == null)
                        {
                            o.Data.Developer = WebUtility.HtmlDecode(divs[i + 1].InnerText);
                        }                            
                    }
                    if (divs[i].InnerHtml == "Released")
                    {
                        if (order == ScraperOrder.Primary || o.Data.Released == null)
                        {
                            o.Data.Released = WebUtility.HtmlDecode(divs[i + 1].InnerText);
                        }                            
                    }
                    if (divs[i].InnerHtml == "ESRB Rating")
                    {
                        if (order == ScraperOrder.Primary || o.Data.ESRB == null)
                        {
                            o.Data.ESRB = WebUtility.HtmlDecode(divs[i + 1].InnerText);
                        }                            
                    }
                    if (divs[i].InnerHtml == "Genre")
                    {
                        string genres = WebUtility.HtmlDecode(divs[i + 1].InnerText);
                        string[] genreArr = genres.Split('/');
                        if (o.Data.Genres == null)
                            o.Data.Genres = new List<string>();
                        foreach (string s in genreArr)
                        {
                            if (order == ScraperOrder.Primary || o.Data.Genres.Count == 0)
                            {
                                if (s != null)
                                {
                                    o.Data.Genres.Add(s.Trim());
                                }
                            }    
                        }                        
                    }
                }
            }

            // get the game description
            if (initialPage.Contains("<h2>Description</h2>"))
            {
                if (order == ScraperOrder.Primary || o.Data.Overview == null)
                {
                    string[] arr1 = initialPage.Split(new string[] { "<h2>Description</h2>" }, StringSplitOptions.None);
                    string[] arr2 = arr1[1].Split(new string[] { "<div class=" }, StringSplitOptions.None);
                    string description = WebUtility.HtmlDecode(Regex.Replace(arr2[0], @"<[^>]*>", String.Empty));
                    o.Data.Overview = description;
                }                    
            }
            

            // get alternate titles
            if (initialPage.Contains("<h2>Alternate Titles</h2>"))
            {
                if (order == ScraperOrder.Primary || o.Data.AlternateTitles == null)
                {
                    string[] arr3 = initialPage.Split(new string[] { "<h2>Alternate Titles</h2>" }, StringSplitOptions.None);
                    string s3 = arr3[1].Replace("\n\r", "").Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("<ul>", "");
                    string[] arr4 = s3.Split(new string[] { "</ul>" }, StringSplitOptions.None);
                    string s4 = arr4[0].Trim();
                    string s5 = s4.Replace("<li>", "").Replace("</li>", "\n");
                    string[] s6 = s5.Split('\n');
                    if (o.Data.AlternateTitles == null)
                        o.Data.AlternateTitles = new List<string>();
                    foreach (string s in s6)
                    {
                        if (s != "")
                            o.Data.AlternateTitles.Add(WebUtility.HtmlDecode(s.Replace(" -- <em>Japanese spelling</em>", "")));
                    }
                }                   
            }

            if (gs.scrapeBoxart == true || gs.scrapeMedia == true)
            {
                // cover art
                // query the coverart page
                string baseurlcover = "http://www.mobygames.com/game/";
                string paramcover = masterrecord.MobyData.MobyPlatformName + "/" + masterrecord.MobyData.MobyURLName + "/cover-art";
                string coverPage = ReturnWebpage(baseurlcover, paramcover, 10000);

                // convert page string to htmldoc
                HtmlDocument cDoc = new HtmlDocument();
                cDoc.LoadHtml(coverPage);

                // get all divs of class "row"
                List<HtmlNode> coverDivs = cDoc.DocumentNode.SelectNodes("//div[@class ='row']").ToList();
                // take the second one
                HtmlNode cDiv = coverDivs[1];
                // now get the div classes that make up the 3 images we want
                if (coverPage.Contains("There are no covers for the selected platform."))
                {
                    // no cover images found - skip
                }
                else
                {
                    List<HtmlNode> imageDivs = cDiv.SelectNodes("//div[@class ='thumbnail']").ToList();

                    bool frontFound = false;
                    bool backFound = false;
                    bool mediaFound = false;

                    // iterate through every 'row' div
                    foreach (HtmlNode h in imageDivs)
                    {
                        // get media type
                        List<HtmlNode> type = h.SelectNodes("//div[@class ='thumbnail-cover-caption']").ToList();
                        List<HtmlNode> img = h.SelectNodes("//a[@class ='thumbnail-cover']").ToList();
                        int typeCount = type.Count;

                        for (int i = 0; i < typeCount; i++)
                        {
                            string t = type[i].InnerText.Trim().ToLower();
                            string MEDIA = "http://mobygames.com" + img[i].Attributes["style"].Value.Replace(");", "").Replace("background-image:url(", "").Replace("/s/", "/l/");

                            if (frontFound == false && t == "front cover")
                            {
                                if (o.FrontCovers == null || o.FrontCovers.Count == 0)
                                {
                                    if (gs.scrapeBoxart == true)
                                    {
                                        o.FrontCovers = new List<string>();
                                        o.FrontCovers.Add(MEDIA);
                                    }                                    
                                }
                                frontFound = true;
                            }
                            if (backFound == false && t == "back cover")
                            {
                                if (o.BackCovers == null || o.BackCovers.Count == 0)
                                {
                                    if (gs.scrapeBoxart == true)
                                    {
                                        o.BackCovers = new List<string>();
                                        o.BackCovers.Add(MEDIA);
                                    }                                        
                                }
                                backFound = true;
                            }
                            if (mediaFound == false && t == "media")
                            {
                                if (o.Medias == null || o.Medias.Count == 0)
                                {
                                    if (gs.scrapeMedia == true)
                                    {
                                        o.Medias = new List<string>();
                                        o.Medias.Add(MEDIA);
                                    }                                    
                                }
                                mediaFound = true;
                            }

                            if (mediaFound == true && backFound == true && frontFound == true)
                                break;
                        }
                    }
                }
            }

            if (gs.scrapeScreenshots == true)
            {
                // screenshots
                // query the screenshots page
                string baseurlscreen = "http://www.mobygames.com/game/";
                string paramscreen = masterrecord.MobyData.MobyPlatformName + "/" + masterrecord.MobyData.MobyURLName + "/screenshots";
                string screenPage = ReturnWebpage(baseurlscreen, paramscreen, 10000);

                // convert page string to htmldoc
                HtmlDocument sDoc = new HtmlDocument();
                sDoc.LoadHtml(screenPage);

                // get core information
                if (!screenPage.Contains("There are no user screenshots on file"))
                {
                    List<HtmlNode> screens = sDoc.DocumentNode.SelectNodes("//a[@class ='thumbnail-image']").ToList();
                    if (o.Screenshots == null)
                        o.Screenshots = new List<string>();
                    int co = 0;
                    foreach (var screen in screens)
                    {
                        if (co >= gs.maxScreenshots)
                            break;
                        var attrib = screen.Attributes["style"].Value;
                        string path = attrib.Replace(");", "").Replace("background-image:url(", "").Replace("/s/", "/l/");
                        o.Screenshots.Add("http://mobygames.com" + path);
                        co++;
                    }
                }
            }
            return o;
        }


        /// <summary>
        /// Scrape the full master list (basic) of games from mobygames
        /// and save to json file in VS project (not bin). 
        /// </summary>
        public static void ScrapeBasicGamesList()
        {
            ScrapeBasicGamesList(null);
        }
        public static void ScrapeBasicGamesList(ProgressDialogController controller)
        {
            List<MobyPlatformGame> games = GamesListScrape(controller);
            if (games == null || games.Count == 0)
            {
                // nothing returned
            }
            else
            {
                // save to json file
                if (controller != null)
                    controller.SetMessage("Saving to file...");
                // set file path
                string filePath = @"..\..\Data\System\MobyGames.json";
                //  dump file
                string json = JsonConvert.SerializeObject(games, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static List<MobyPlatformGame> GamesListScrape(ProgressDialogController controller)
        {
            string BaseUrl = "http://www.mobygames.com/browse/games/";

            // get all platforms
            List<GSystem> systems = GSystem.GetSystems();
            List<MobyPlatformGame> allGames = new List<MobyPlatformGame>();
            

            // iterate through each system
            foreach (GSystem s in systems)
            {
                if (s.systemId == 16 || s.systemId == 17)
                {
                    break;
                }

                foreach (string sys in s.MobyPlatformName)
                {
                    if (controller != null)
                    {
                        controller.SetMessage("Scraping basic list of all " + sys + " games");
                        if (controller.IsCanceled) { return null; }
                    }



                    // build initial query string to get the search page
                    string param = sys + "/list-games";
                    string initialPage = ReturnWebpage(BaseUrl, param, 10000);

                    /* Get the total number of games available for this system */
                    // split the html to list via line breaks
                    List<string> html = initialPage.Split('\n').ToList();
                    // get only the line that contains the number of games
                    string hLine = html.Where(a => a.Contains(" games)")).FirstOrDefault();
                    // get only the substring "xxx games"
                    string resultString = Regex.Match(hLine, @"(?<=\().+?(?=\))").Value;
                    // split by whitespace
                    string[] gArr = resultString.Split(' ');
                    // get int number of games
                    int totalGames = Convert.ToInt32(gArr[0]);

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(initialPage);

                    // build a list of page URLs
                    double numberofpages = Convert.ToDouble(totalGames) / 25;
                    int numberOfPages = Convert.ToInt32(Math.Ceiling(numberofpages));

                    // connect to every page and import all the game information
                    for (int i = 0; i < numberOfPages; i++)
                    {
                        if (controller != null)
                        {
                            if (controller.IsCanceled) { return null; }
                        }

                        int offset = i * 25;

                        string p = sys + "/offset," + offset + "/so,0a/list-games";
                        HtmlDocument hDoc = new HtmlDocument();
                        if (i == 0)
                            hDoc = doc;
                        else
                        {
                            string htmlRes = ReturnWebpage(BaseUrl, p, 10000);
                            hDoc.LoadHtml(htmlRes);
                        }

                        // get just the data table we are interested in
                        HtmlNode objectTable = hDoc.GetElementbyId("mof_object_list");

                        // iterate through each row and scrape the game information
                        int cGame = 1;
                        foreach (HtmlNode row in objectTable.SelectNodes("tbody/tr"))
                        {
                            int currentGameNumber = offset + cGame;
                            if (controller != null)
                            {
                                if (controller.IsCanceled) { return null; }
                                controller.SetMessage("Scraping basic list of all " + sys + " games\nGame: (" + currentGameNumber + " of " + totalGames + ")\nPage: (" + (i + 1) + " of " + numberOfPages + ")");
                                controller.Minimum = 1;
                                controller.Maximum = totalGames;
                                controller.SetProgress(Convert.ToDouble(currentGameNumber));
                            }


                            HtmlNode[] cells = (from a in row.SelectNodes("td")
                                                select a).ToArray();

                            string Title = cells[0].InnerText.Trim();
                            //var allLi = row.SelectSingleNode("//a[@href]");
                            string URLstring = cells[0].InnerHtml.Trim();
                            Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
                            Match match;
                            string URL = "";
                            for (match = regex.Match(URLstring); match.Success; match = match.NextMatch())
                            {
                                URL = match.Groups[1].ToString();
                            }

                            MobyPlatformGame game = new MobyPlatformGame();
                            game.SystemId = s.systemId;
                            game.PlatformName = sys;
                            game.Title = WebUtility.HtmlDecode(Title);
                            game.UrlName = WebUtility.HtmlDecode(URL.Split('/').LastOrDefault());

                            // add game to main list
                            allGames.Add(game);
                            cGame++;


                        }

                    }
                }
            }
            return allGames;
        }

        /// <summary>
        /// Pull Back Webpage as a string
        /// </summary>
        /// <param name="BaseUrl"></param>
        /// <param name="Params"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static string ReturnWebpage(string BaseUrl, string Params, int Timeout)
        {
            WebOps wo = new WebOps();
            if (Params == null)
                Params = "";
            if (Timeout == 0)
                Timeout = 10000;
            wo.BaseUrl = BaseUrl;
            wo.Timeout = Timeout;
            wo.Params = Params;
            string result = wo.ApiCall();
            wo = null;
            return result;
        }
    }
}
