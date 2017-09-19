using HtmlAgilityPack;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes;
using MedLaunch.Classes.TheGamesDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MedLaunch.Common.Search;

namespace MedLaunch._Debug.ScrapeDB
{
    public class AdminScrapeDb
    {
        public MainWindow mw { get; set; }
        public List<GDB_Platform> platforms { get; set; }
        public List<MOBY_Platform> mobyplatforms { get; set; }

        public AdminScrapeDb()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            platforms = GDB_Platform.GetPlatforms();
            mobyplatforms = MOBY_Platform.GetPlatforms();
        }

        /// <summary>
        /// scrape all moby platform games
        /// </summary>
        public async void ScrapeMobyPlatformGames()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await mw.ShowProgressAsync("MedLaunch - Getting Basic Games List From mobygames.net", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                Task.Delay(1);
                
                int count = 1;
                int sysCount = mobyplatforms.Count();

                controller.Minimum = 0;
                controller.Maximum = sysCount;

                foreach (var platform in mobyplatforms)
                {
                    List<MOBY_Game> gs = new List<MOBY_Game>();
                    controller.SetProgress(Convert.ToDouble(count));
                    controller.SetMessage("Retrieving Game List for Platform: " + platform.name);

                    // get initial page
                    string url = platform.listURL;
                    string initialPage = ReturnWebpage(url, "", 10000);

                    bool isAttrib = false;
                    if (url.Contains("attribute/sheet"))
                        isAttrib = true;

                    int totalGames = 0;

                    if (!isAttrib)
                    {
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
                        totalGames = Convert.ToInt32(gArr[0]);
                    }
                    else
                    {
                        List<string> html = initialPage.Split('\n').ToList();
                        string hLine = html.Where(a => a.Contains("(items")).FirstOrDefault();
                        string resultString = Regex.Match(hLine, @"(?<=\().+?(?=\))").Value;
                        string[] gArr = resultString.Split(' ');
                        totalGames = Convert.ToInt32(gArr.Last());
                    }

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(initialPage);

                    // build a list of page URLs
                    double numberofpages = Convert.ToDouble(totalGames) / 25;
                    int numberOfPages = Convert.ToInt32(Math.Ceiling(numberofpages));

                    // connect to every page and import all the game information
                    for (int i = 0; i < numberOfPages; i++)
                    {
                        int offset = i * 25;
                        string newUrl = url.Replace("offset,0", "offset," + offset);

                        HtmlDocument hDoc = new HtmlDocument();
                        if (i == 0)
                            hDoc = doc;
                        else
                        {
                            string htmlRes = ReturnWebpage(newUrl, "", 10000);
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
                                controller.SetMessage("Scraping basic list of all " + platform.name + " games\nGame: (" + currentGameNumber + " of " + totalGames + ")\nPage: (" + (i + 1) + " of " + numberOfPages + ")");
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

                            MOBY_Game game = new MOBY_Game();
                            game.pid = platform.pid;
                            game.gameTitle = WebUtility.HtmlDecode(Title);
                            game.alias = WebUtility.HtmlDecode(URL.Split('/').LastOrDefault());
                            game.releaseYear = cells[1].InnerText.Trim();
                            
                            // add game to main list
                            gs.Add(game);
                        }
                    }
                    MOBY_Game.SaveToDatabase(gs);
                }
                               
            });

            await controller.CloseAsync();
            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MOBY Master Games List Download", "Operation Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MOBY Master Games List Download", "Scanning and Import Completed");
            }
        }

        /// <summary>
        /// update the scrape database with a list of gdb platform games (from the web)
        /// </summary>
        public async void ScrapePlatformGames()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await mw.ShowProgressAsync("MedLaunch - Getting Basic Games List From thegamesdb.net", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                Task.Delay(1);
                List<GDB_Game> gs = new List<GDB_Game>();
                int count = 0;
                int sysCount = platforms.Count();

                controller.Minimum = 0;
                controller.Maximum = sysCount;

                foreach (var platform in platforms)
                {
                    controller.SetProgress(Convert.ToDouble(count));
                    controller.SetMessage("Retrieving Game List for Platform: " + platform.name);

                    List<GDBNETGameSearchResult> result = GDBNETGamesDB.GetPlatformGames(platform.pid).ToList();
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
                        GDB_Game g = new GDB_Game();
                        g.gid = r.ID;
                        g.gameTitle = r.Title.Trim();                        
                        g.pid = platform.pid;

                        if (r.ReleaseDate != null) 
                        {
                            if (r.ReleaseDate.Contains("/"))
                                g.releaseYear = r.ReleaseDate.Split('/').Last().Trim();
                            else
                                g.releaseYear = r.ReleaseDate.Trim();
                        }                       

                        gs.Add(g);
                    }

                    // remove duplicates
                    gs.Distinct();

                    controller.SetMessage("Saving to database");                    
                }

                GDB_Game.SaveToDatabase(gs);

            });

            await controller.CloseAsync();
            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("GDB Master Games List Download", "Operation Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("GDB Master Games List Download", "Scanning and Import Completed");
            }
        }

        public async void MobyExactMatch()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await mw.ShowProgressAsync("Attempting exact match - mobygames to thegamesdb", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            

            await Task.Run(() =>
            {
                Task.Delay(1);
                int progress = 0;               

                List<MasterView> unmatched = (from a in MasterView.GetMasterView()
                                              where a.mid == null
                                              select a).ToList();

                controller.Minimum = 0;
                controller.Maximum = unmatched.Count();
                int co = unmatched.Count();

                int matched = 0;
                int notmatched = 0;

                controller.SetProgress(progress);
                controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);

                List<int[]> list = new List<int[]>();

                List<MOBY_Game> mgames = MOBY_Game.GetGames().ToList();

                foreach (MasterView m in unmatched)
                {
                    controller.SetProgress(progress);
                    controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);
                    progress++;
                    int gid = m.gid;
                    string gdbTitle = m.GDBTitle.Trim();

                    // lookup in mobygames
                    MOBY_Game mg = (from a in mgames
                                    where a.pid == m.pid &&
                                    a.gameTitle.Trim().ToLower() == gdbTitle.ToLower()
                                    select a).FirstOrDefault();
                    if (mg == null)
                    {
                        notmatched++;
                        continue;
                    }
                    matched++;

                    int[] i = new int[2];
                    i[0] = m.gid;
                    i[1] = mg.mid;

                    list.Add(i);                    

                    controller.SetProgress(progress);
                    controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);    
                }

                controller.SetMessage("Saving to Database...");

                List<Junction> jList = new List<Junction>();
                foreach (var a in list)
                {
                    Junction j = new Junction();
                    j.gid = a[0];
                    j.mid = a[1];
                    jList.Add(j);
                }

                Junction.SaveToDatabase(jList);
                
            });

            await controller.CloseAsync();
            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("ExactMatch Matching", "Operation Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("ExactMatch Matching", "Scanning and Import Completed");
            }
        }

        public async void MobyManualMatch(bool AutoMatchOnSingle100Score)
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false,
            };

            var controller = await mw.ShowProgressAsync("Attempting manual match based on word count - mobygames to thegamesdb", "", settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            

            await Task.Run(() =>
            {
                Task.Delay(1);
                int progress = 0;

                List<Junction> jList = new List<Junction>();

                List<MasterView> unmatched = (from a in MasterView.GetMasterView()
                                              where a.mid == null
                                              select a).ToList();

                controller.Minimum = 0;
                controller.Maximum = unmatched.Count();
                int co = unmatched.Count();

                int matched = 0;
                int notmatched = 0;

                controller.SetProgress(progress);
                controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);

                List<int[]> list = new List<int[]>();

                List<MOBY_Game> mgames = MOBY_Game.GetGames().ToList();

                bool cancel = false;

                foreach (MasterView m in unmatched)
                {
                    if (cancel == true)
                        break;

                    controller.SetProgress(progress);
                    controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);
                    progress++;
                    int gid = m.gid;
                    int pid = m.pid;

                    // build SearchObject
                    SearchObject so = new SearchObject();
                    so.searchId = gid;
                    so.searchString = m.GDBTitle.Trim();
                    so.listToSearch = (from a in mgames
                                       where a.pid == pid
                                       select new SearchList { id = a.mid, name = a.gameTitle }).ToList();

                    // get search results
                    so = SearchFunctions.WordCountMatch(so);

                    // get results list separately
                    List<SearchResult> results = so.searchResults.OrderByDescending(a => a.score).ToList();

                    // if automatch on unique 100 score is selected
                    if (AutoMatchOnSingle100Score)
                    {
                        var hundreds = results.Where(a => a.score == 100).ToList();
                        if (hundreds.Count == 1)
                        {
                            Junction j = new Junction();
                            j.gid = gid;
                            j.mid = hundreds.First().resultId;
                            jList.Add(j);
                            matched++;
                            //controller.SetMessage("Saving to Database...");
                            //Junction.SaveToDatabase(jList);
                            //jList = new List<Junction>();
                            continue;
                        }
                        else
                        {
                            notmatched++;
                            continue;
                        }
                    }
                    else
                    {
                        // iterate through each result and prompt user to accept / as for next result / or skip completely
                        foreach (var res in results)
                        {
                            string title = "Matching " + m.PlatformAlias + " - " + m.GDBTitle;
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Potential match found for " + m.GDBTitle + " \n(" + m.PlatformAlias + "): ");
                            sb.Append(res.resultString);
                            sb.Append("\n\n");
                            sb.Append("Word score: " + res.score);
                            sb.Append("\n\n\n");
                            sb.Append("Press YES to accept this match and save to the database\n");
                            sb.Append("Press NO to show the next match for this entry\n");
                            sb.Append("Press CANCEL to skip this entirely and move to the next search object");
                            MessageBoxResult mbr = MessageBox.Show(sb.ToString(), title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);

                            if (mbr == MessageBoxResult.Yes)
                            {
                                // match has been confirmed. create a junction record
                                Junction j = new Junction();
                                j.gid = gid;
                                j.mid = res.resultId;
                                jList.Add(j);
                                matched++;
                                controller.SetMessage("Saving to Database...");
                                Junction.SaveToDatabase(jList);
                                jList = new List<Junction>();
                                break;
                            }
                            if (mbr == MessageBoxResult.No)
                            {
                                continue;
                            }
                            if (mbr == MessageBoxResult.Cancel)
                            {
                                break;
                            }
                            if (mbr == MessageBoxResult.None)
                            {
                                cancel = true;
                                break;
                            }
                        }
                    }

                    controller.SetProgress(progress);
                    controller.SetMessage("TOTAL: " + co + "\n\nMatched: " + matched + "\nUnmatched: " + notmatched);
                }

                if (AutoMatchOnSingle100Score)
                {
                    controller.SetMessage("Saving to Database...");
                    Junction.SaveToDatabase(jList);
                }

            });

            await controller.CloseAsync();
            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("ExactMatch Matching", "Operation Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("ExactMatch Matching", "Scanning and Import Completed");
            }
        }

        public void ScrapeManualsFromOnline()
        {

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
