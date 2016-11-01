using HtmlAgilityPack;
using MahApps.Metro.Controls.Dialogs;
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

        public static ScrapedGameObjectWeb ScrapeGame(ScrapedGameObjectWeb o, ScraperOrder order, ProgressDialogController controller, ScraperMaster masterrecord)
        {
            bool priority;
            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (order == ScraperOrder.Primary)
            {
                priority = true;    // primary
                if (masterrecord.MobyData.MobyTitle != null)
                {
                    // moby data has been matched
                    o.Data.Title = masterrecord.MobyData.MobyTitle;
                    o.Data.Platform = masterrecord.MobyData.MobyPlatformName;
                }
                else
                {
                    // no moby data matched - just return
                    return o;
                }                
            }
            else
            {
                // moby scraping is secondary
                priority = false;    // primary
            }

            if (priority == true)
            {
                // primary scraping
            }
            else
            {
                // secondary scraping
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

            int idCount = 1;

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
