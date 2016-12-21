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
                List<LibraryDataGDBLink> links = LibraryDataGDBLink.GetLibraryData().ToList();
                List<DataGridGamesView> q = new List<DataGridGamesView>();
                var games = (from g in cnt.Game
                             where g.hidden != true
                             select g).ToList();
                foreach (var game in games)
                {
                    DataGridGamesView d = new DataGridGamesView();
                    d.ID = game.gameId;
                    
                    d.System = GSystem.GetSystemName(game.systemId);
                    d.LastPlayed = DbEF.FormatDate(game.gameLastPlayed);
                    d.Favorite = game.isFavorite;

                    d.Country = game.Country;

                    if (game.romNameFromDAT != null)
                    {
                        if (game.romNameFromDAT.Contains("(USA)"))
                            d.Country = "US";
                        if (game.romNameFromDAT.Contains("(Europe)"))
                            d.Country = "EU";
                    }
                    

                    d.Flags = game.OtherFlags;
                    d.Language = game.Language;
                    d.Publisher = game.Publisher;
                    d.Year = game.Year;

                    if (game.gameNameFromDAT != null && game.gameNameFromDAT != "")
                        d.Game = game.gameNameFromDAT;
                    else
                        d.Game = game.gameName;

                    //d.DatName = game.gameNameFromDAT;
                    d.DatRom = game.romNameFromDAT;

                    if (game.gdbId != null && game.gdbId > 0)
                    {
                        var link = links.Where(x => x.GDBId == game.gdbId).SingleOrDefault(); // LibraryDataGDBLink.GetLibraryData(game.gdbId.Value);
                        if (link != null)
                        {
                            if (link.Publisher != null && link.Publisher != "")
                                d.Publisher = link.Publisher;
                                                        
                            d.Developer = link.Developer;

                            if (link.Year != null && link.Year != "")
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
