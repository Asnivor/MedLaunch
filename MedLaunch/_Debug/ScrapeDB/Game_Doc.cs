using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch._Debug.ScrapeDB
{
    public class Game_Doc
    {
        public int id { get; set; }
        public int? gid { get; set; }
        public string gameName { get; set; }
        public string downloadUrl { get; set; }
        public int? pid { get; set; }


        public static Game_Doc ReturnDocEntry(int id)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.Game_Doc
                             where g.id == id
                             select g).FirstOrDefault();
                return cData;
            }
        }

        public static List<Game_Doc> ReturnDocEntriesByGid(int gid)
        {
            using (var context = new AsniScrapeAdminDbContext())
            {
                var cData = (from g in context.Game_Doc
                             where g.gid == gid
                             select g).ToList();
                return cData;
            }
        }

        public static void AddDoc(Game_Doc doc)
        {
            using (var aG = new AsniScrapeAdminDbContext())
            {
                // check whether doc url already exists
                List<Game_Doc> docs = (from a in aG.Game_Doc
                                       where a.downloadUrl == doc.downloadUrl
                                       select a).ToList();

                if (docs.Count == 0)
                {
                    aG.Game_Doc.Add(doc);
                    aG.SaveChanges();
                    aG.Dispose();
                }
            }
        }

        public static void UpdateDoc(Game_Doc doc)
        {
            using (var aG = new AsniScrapeAdminDbContext())
            {
                // check whether doc url already exists
                Game_Doc doc1 = (from a in aG.Game_Doc.AsNoTracking()
                                       where a.id == doc.id
                                       select a).FirstOrDefault();

                if (doc1 != null)
                {
                    aG.Game_Doc.Update(doc);
                    aG.SaveChanges();
                    aG.Dispose();
                }
            }
        }

        public static async void ParseManuals()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Matching game manuals", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            await Task.Run(() =>
            {
                List<Game_Doc> docs = new List<Game_Doc>();
                List<Game_Doc> docsNoMatch = new List<Game_Doc>();

                // get all docs
                using (var context = new AsniScrapeAdminDbContext())
                {
                    var mData = from g in context.Game_Doc
                                select g;

                    docs = mData.ToList();
                }

                int Matched = 0;

                // iterate through each doc that does not have a game ID set
                foreach (Game_Doc m in docs.Where(a => a.gid == null))
                {
                    

                    controller.SetMessage("Matching Manuals for Game: " + m.gameName + "\n(" + MedLaunch.Models.GSystem.ReturnGamesDBPlatformName(m.pid.Value) + ")\n\nMatches Found: " + Matched.ToString());

                    var games = from a in GDB_Game.GetGames()
                                where a.pid == m.pid
                                select a;

                    // try exact match
                    var search = (from a in games
                                  where a.gameTitle.Trim().ToLower().Replace(":", "").Replace("-", "").Replace("'", "").Replace("  ", " ") 
                                  == m.gameName.Trim().ToLower().Replace(":", "").Replace("-", "").Replace("'", "").Replace("  ", " ")
                                  select a).FirstOrDefault();

                    if (search != null)
                    {
                        // exact match found - update entry
                        Game_Doc gd = new Game_Doc();
                        gd.id = m.id;
                        gd.gameName = m.gameName;
                        gd.pid = m.pid;
                        gd.downloadUrl = m.downloadUrl;
                        gd.gid = search.gid;

                        UpdateDoc(gd);

                        Matched++;                 
                    }
                    else
                    {
                        // no exact match found - do manual selection based on word matching
                        docsNoMatch.Add(m);
                        
                    }
                }

                // now do manual
                foreach (Game_Doc m in docsNoMatch.Where(a => a.gid == null))
                {
                    List<ManualCount> mcount = new List<ManualCount>();
                    var games = from a in GDB_Game.GetGames()
                                where a.pid == m.pid
                                select a;

                    string[] lArr = m.gameName.Trim().Split(' ');

                    List<GDB_Game> hitList = new List<GDB_Game>();

                    for (int i = 0; i < lArr.Length; i++)
                    {
                        var s = games.Where(a => a.gameTitle.ToLower().Trim().Contains(lArr[i].ToLower().Trim()));
                        hitList.AddRange(s);
                    }
                    // count entries in list
                    var q = from x in hitList
                            group x by x into g
                            let count = g.Count()
                            orderby count descending
                            select new { Value = g.Key, Count = count };
                    foreach (var x in q)
                    {
                        ManualCount mc = new ManualCount();
                        mc.Game = x.Value;
                        mc.Matches = x.Count;
                        mcount.Add(mc);
                    }

                    foreach (var g in mcount.OrderByDescending(a => a.Matches))
                    {
                        string message = "Manual: \n" + m.gameName + "\n";
                        message += "\nGame:\n " + g.Game.gameTitle + "\n(" + MedLaunch.Models.GSystem.ReturnGamesDBPlatformName(m.pid.Value) + ")\n";
                        message += "\nIs this a match??\n";
                        message += "YES - match / NO - nomatch / CANCEL - cancel";
                        MessageBoxResult result = MessageBox.Show(message, "Manual Matching", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Cancel)
                        {
                            break;
                        }
                        if (result == MessageBoxResult.Yes)
                        {
                            // this is a match - update the record
                            Game_Doc gd = new Game_Doc();
                            gd.id = m.id;
                            gd.gameName = m.gameName;
                            gd.pid = m.pid;
                            gd.downloadUrl = m.downloadUrl;
                            gd.gid = g.Game.gid;

                            UpdateDoc(gd);
                            Matched++;
                            break;
                        }
                        if (result == MessageBoxResult.No)
                        {
                            // not a match - continue
                            continue;
                        }
                    }
                }


            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("Parse Manuals", "Parsing Completed");
            }
        }
    }

    public class ManualCount
    {
        public GDB_Game Game { get; set; }
        public int Matches { get; set; }
    }


}
