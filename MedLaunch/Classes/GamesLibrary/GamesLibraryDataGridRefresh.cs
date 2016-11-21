using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GamesLibraryDataGridRefresh
    {
        public static List<DataGridGamesView> Update(List<DataGridGamesView> AllGames)
        {
            using (var cnt = new MyDbContext())
            {
                List<LibraryDataGDBLink> lib = (from all in cnt.LibraryDataGDBLink
                                                select all).ToList();
                List<DataGridGamesView> ag = (from game in cnt.Game
                                              from link in cnt.GDBLink
                                              .Where(v => v.GameId == game.gameId)
                                              .DefaultIfEmpty()
                                              /*
                                              from s in cnt.LibraryDataGDBLink
                                              .Where(a => a.GDBId == link.GdbId)
                                              .DefaultIfEmpty()
                                              */
                                              select new DataGridGamesView
                                              {
                                                  ID = game.gameId,
                                                  Game = game.gameName,
                                                  System = GSystem.GetSystemName(game.systemId),
                                                  LastPlayed = DbEF.FormatDate(game.gameLastPlayed),
                                                  Favorite = game.isFavorite/*,

                                                  Publisher = GetScrapedData("Publisher", lib, link.GdbId.Value),
                                                  Developer = GetScrapedData("Developer", lib, link.GdbId.Value),
                                                  Year = GetScrapedData("Year", lib, link.GdbId.Value),
                                                  Players = GetScrapedData("Players", lib, link.GdbId.Value),
                                                  Coop = GetScrapedData("Coop", lib, link.GdbId.Value),
                                                  ESRB = GetScrapedData("ESRB", lib, link.GdbId.Value),*/

                                                  /*
                                                  Publisher = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Publisher,
                                                  Developer = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Developer,
                                                  Year = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Year,
                                                  Players = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Players,
                                                  Coop = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().Coop,
                                                  ESRB = lib.Where(a => a.GDBId == link.GdbId).FirstOrDefault().ESRB */
                                              }).ToList();
                List<DataGridGamesView> ng = new List<DataGridGamesView>();
                foreach (var x in ag)
                {
                    DataGridGamesView d = new DataGridGamesView();
                    d = x;

                    var link = cnt.GDBLink.Where(l => l.GameId == d.ID).ToList();
                    if (link.Count < 1)
                    {
                        ng.Add(d);
                        continue;
                    }
                        

                    int gdbId = link.First().GdbId.Value;

                    var s = cnt.LibraryDataGDBLink.Where(i => i.GDBId == gdbId).ToList();
                    if (s.Count < 1)
                    {
                        ng.Add(d);
                        continue;
                    }

                    var sc = s.First();

                    d.Coop = sc.Coop;
                    d.Developer = sc.Developer;
                    d.ESRB = sc.ESRB;
                    d.Players = sc.Players;
                    d.Publisher = sc.Publisher;
                    d.Year = sc.Year;

                    ng.Add(d);
                }

                return ng;
                
                //AllGames = ng;
            }
        }

        public static void Update()
        {
            App _App = ((App)Application.Current);
            List<DataGridGamesView> view = Update(_App.GamesList.AllGames);
            _App.GamesList.AllGames = view;     
        }

        public static string GetScrapedData(string type, List<LibraryDataGDBLink> libraryData, int gdbId)
        {
            string result = null;
            var lib = libraryData.Where(x => x.GDBId == gdbId).FirstOrDefault();
            if (lib != null)
            {
                switch (type)
                {
                    case "Publisher":
                        result = lib.Publisher;
                        break;
                    case "Developer":
                        result = lib.Developer;
                        break;
                    case "Year":
                        result = lib.Year;
                        break;
                    case "Players":
                        result = lib.Players;
                        break;
                    case "Coop":
                        result = lib.Coop;
                        break;
                    case "ESRB":
                        result = lib.ESRB;
                        break;
                    default:
                        return null;
                }
            }
            return result;           
        }
    }
}
