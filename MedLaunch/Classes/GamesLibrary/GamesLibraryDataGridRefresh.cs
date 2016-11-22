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
                /*
                   var result = 
                       (from game in cnt.Game
                       join lib in cnt.LibraryDataGDBLink
                       on game.gdbId equals lib.GDBId
                       into gameGroups
                       from lib in gameGroups.DefaultIfEmpty()
                       select new DataGridGamesView
                       {
                           ID = game.gameId,
                           Game = game.gameName,
                           System = GSystem.GetSystemName(game.systemId),
                           LastPlayed = DbEF.FormatDate(game.gameLastPlayed),
                           Favorite = game.isFavorite,
                           Publisher = lib.Publisher,
                           Developer = lib.Developer,
                           Year = DbEF.ReturnYear(lib.Year),
                           Players = lib.Players,
                           Coop = lib.Coop,
                           ESRB = lib.ESRB
                       }).ToList();
                     */
                /*
                List<DataGridGamesView> ag = (from game in cnt.Game
                                             
                                              from link in cnt.LibraryDataGDBLink
                                              .Where(v => v.GDBId == game.gdbId)
                                              .DefaultIfEmpty()
                                            
                                              select new DataGridGamesView
                                              {
                                                  ID = game.gameId,
                                                  Game = game.gameName,
                                                  System = GSystem.GetSystemName(game.systemId),
                                                  LastPlayed = DbEF.FormatDate(game.gameLastPlayed),
                                                  Favorite = game.isFavorite,
                                                
                                                  Publisher = link.Publisher,
                                                  Developer = link.Developer,
                                                  Year = DbEF.ReturnYear(link.Year),
                                                  Players = link.Players,
                                                  Coop = link.Coop,
                                                  ESRB = link.ESRB
                                                
                                              }).ToList();
               
              */
                /*
                  var q = (from game in cnt.Game
                          join link in cnt.LibraryDataGDBLink on game.gdbId equals link.GDBId into ps
                          from link in ps.DefaultIfEmpty()
                          select new DataGridGamesView
                          {
                              ID = game.gameId,
                              Game = game.gameName,
                              System = GSystem.GetSystemName(game.systemId),
                              LastPlayed = DbEF.FormatDate(game.gameLastPlayed),
                              Favorite = game.isFavorite,

                              Publisher = link.Publisher,
                              Developer = link.Developer,
                              Year = DbEF.ReturnYear(link.Year),
                              Players = link.Players,
                              Coop = link.Coop,
                              ESRB = link.ESRB

                          }).ToList();
                          */
                List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();
                List<DataGridGamesView> q = new List<DataGridGamesView>();
                var games = (from g in cnt.Game
                             where g.hidden != true
                             select g).ToList();
                foreach (var game in games)
                {
                    DataGridGamesView d = new DataGridGamesView();
                    d.ID = game.gameId;
                    d.Game = game.gameName;
                    d.System = GSystem.GetSystemName(game.systemId);
                    d.LastPlayed = DbEF.FormatDate(game.gameLastPlayed);
                    d.Favorite = game.isFavorite;

                    if (game.gdbId != null && game.gdbId > 0)
                    {
                        var link = links.Where(x => x.GDBId == game.gdbId).SingleOrDefault(); // LibraryDataGDBLink.GetLibraryData(game.gdbId.Value);
                        if (link != null)
                        {
                            d.Publisher = link.Publisher;
                            d.Developer = link.Developer;
                            d.Year = DbEF.ReturnYear(link.Year);
                            d.Players = link.Players;
                            d.Coop = link.Coop;
                            d.ESRB = link.ESRB;
                        }
                    }

                    q.Add(d);
                }

                
                return q;
                
                //AllGames = ng;
            }
        }

        public static void Update()
        {
            App _App = ((App)Application.Current);
            List<DataGridGamesView> view = Update(_App.GamesList.AllGames);
            _App.GamesList.UpdateRequired = false;
            _App.GamesList.AllGames = view;     
        }

        /*
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
        */
    }
}
