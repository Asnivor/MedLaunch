using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.GamesLibrary
{
    public class GameListBuilder
    {
        // properties
        public List<DataGridGamesView> FilteredSet { get; set; }
        public List<DataGridGamesView> AllGames { get; set; }
        public int SystemId { get; set; }
        public string SearchString { get; set; }

        // constructors
      
        public GameListBuilder()
        {
            using (var context = new MyDbContext())
            {
                
                List<DataGridGamesView> allGames = (from game in context.Game
                                                    join l in context.GDBLink on game.gameId equals l.GameId
                                                    join s in context.LibraryDataGDBLink on l.GdbId equals s.GDBId
                                                    select new
                                                    {
                                                        ID = game.gameId,
                                                        Game = game.gameName,
                                                        System = GSystem.GetSystemName(game.systemId),
                                                        LastPlayed = FormatDate(game.gameLastPlayed),
                                                        Favorite = game.isFavorite,
                                                        Publisher = s.Publisher,
                                                        Developer = s.Developer,
                                                        Year = s.Year,
                                                        Players = s.Players,
                                                        Coop = s.Coop,
                                                        ESRB = s.ESRB
                                                    }).ToList();
                AllGames = allGames;    
            }
        }
      

        public static string FormatDate(DateTime dt)
        {
            string lp;
            if (dt.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00")
            {
                lp = "NEVER";
            }
            else
            {
                lp = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return lp;
        }

        public GameListBuilder(int systemId)
        {
            List<Game> games = new List<Game>();
            SearchString = "";
            FilteredSet = new List<DataGridGamesView>();

            // system id selector
            switch (systemId)
            {
                case -1:        // favorites
                    games = (from g in Game.GetGames()
                                       where g.isFavorite == true
                                       select g).ToList();
                    break;
                case 0:         // all games
                    games = Game.GetGames();
                    break;
                case -100:      // unscraped games
                    games = (from g in Game.GetGames()
                             where g.isScraped != true
                             select g).ToList();
                    break;
                default:        // based on actual system id
                    games = (from g in Game.GetGames()
                             where g.systemId == systemId
                             select g).ToList();
                    break;
            }

            // build filtered set
            foreach (var g in games)
            {
                DataGridGamesView dgv = new DataGridGamesView();

                // standard entries from GAME table in database
                dgv.ID = g.gameId;
                dgv.Game = g.gameName;
                dgv.System = GSystem.GetSystemName(g.systemId);
                dgv.Favorite = g.isFavorite;
                string lp;
                if (g.gameLastPlayed.ToString("yyyy-MM-dd HH:mm:ss") == "0001-01-01 00:00:00")
                {
                    lp = "NEVER";
                }
                else
                {
                    lp = g.gameLastPlayed.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dgv.LastPlayed = lp;

                // add looked up entries from scraping link
                var l = GDBLink.GetRecord(dgv.ID);
                if (l != null)
                {
                    if (l.GdbId != null)
                    {
                        int gdbid = l.GdbId.Value;
                        LibraryDataGDBLink lnk = LibraryDataGDBLink.GetLibraryData(gdbid);
                        if (lnk != null)
                        {
                            dgv.Coop = lnk.Coop;
                            dgv.Developer = lnk.Developer;
                            dgv.ESRB = lnk.ESRB;
                            dgv.Players = lnk.Players;
                            dgv.Publisher = lnk.Publisher;
                            dgv.Year = lnk.Year;
                        }
                    }                   
                }                

                // add to filtered list
                FilteredSet.Add(dgv);
            }
        }
        /*
        public List<DataGridGamesView> Search (string SearchString)
        {

        }
        */
    }
}
