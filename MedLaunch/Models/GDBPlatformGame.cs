using System;
using MedLaunch.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace MedLaunch.Models
{
    public class GDBPlatformGame
    {
        public int id { get; set; }
        public int SystemId { get; set; }
        //public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string ReleaseDate { get; set; }

        public static GDBPlatformGame GetGame(int id)
        {
            using (var db = new MyDbContext())
            {
                GDBPlatformGame g = (from a in db.GDBPlatformGame
                                     where a.id == id
                                     select a).FirstOrDefault();
                return g;
            }
        }

        public static void InitialSeed()
        {
            // to run after database is first created
            string dir = @"Data\Settings";

            if (GDBPlatformGame.GetGames().Count < 1)
            {
                // no results in the database - import from file
                // check whether json file exists
                if (Directory.Exists(dir))
                {
                    var files = (Directory.GetFiles(@"Data\Settings"));
                    var f = files.Where(a => a.EndsWith(".json")).OrderByDescending(b => b.ToString()).FirstOrDefault();

                    if(f == null)
                    {
                        //json file doesn't exist, so create it.
                        try
                        {
                            List<GDBPlatformGame> gs = GameScraper.DatabasePlatformGamesImport();
                            GDBPlatformGame.SaveToDatabase(gs);

                            var platformgames = GDBPlatformGame.GetGames();
                            string linkTimeLocal = (System.Reflection.Assembly.GetExecutingAssembly().GetLinkerTime()).ToString("yyyy-MM-dd HH:mm:ss");
                            string jsonGames = JsonConvert.SerializeObject(platformgames.ToArray());
                            System.IO.File.WriteAllText(@"Data\Settings\thegamesdbplatformgames_" + linkTimeLocal.Replace(" ", "").Replace(":", "").Replace("-", "") + ".json", jsonGames);
                            files = (Directory.GetFiles(@"Data\Settings"));
                            f = files.Where(a => a.EndsWith(".json")).OrderByDescending(b => b.ToString()).FirstOrDefault();
                        }
                        catch(Exception e)
                        {

                        }
                    }

                    string json = System.IO.File.ReadAllText(f);
                    List<GDBPlatformGame> g = JsonConvert.DeserializeObject<List<GDBPlatformGame>>(json);
                    SaveToDatabase(g);

                }
            }
            else
            {
                // results found - do nothing
            }
           
            
        }

        public static List<GDBPlatformGame> GetGames()
        {
            using (var db = new MyDbContext())
            {
                List<GDBPlatformGame> g = (from a in db.GDBPlatformGame
                                     select a).ToList();
                return g;
            }
        }
        
        public static int[] SaveToDatabase(List<GDBPlatformGame> games)
        {
            using (var db = new MyDbContext())
            {
                int added = 0;
                int updated = 0;

                // get current database context
                var current = db.GDBPlatformGame.AsNoTracking().ToList();

                List<GDBPlatformGame> toAdd = new List<GDBPlatformGame>();
                List<GDBPlatformGame> toUpdate = new List<GDBPlatformGame>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    GDBPlatformGame t = (from a in current
                                         where a.id == g.id
                                         select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); added++; }
                    else { toUpdate.Add(g); updated++; }
                }
                db.GDBPlatformGame.UpdateRange(toUpdate);
                db.GDBPlatformGame.AddRange(toAdd);
                db.SaveChangesAsync();

                MessageBox.Show(added + " added, " + updated + " updated.");

                return new int[]{added, updated};
            }
        }
     

        public static void AddToDatabase(List<GDBPlatformGame> gdbGamesToAdd)
        {
            using (var db = new MyDbContext())
            {
                db.AddRange(gdbGamesToAdd);                
                db.SaveChanges();
            }
        }
        public static void UpdateToDatabase(List<GDBPlatformGame> gdbGamesToUpdate)
        {
            using (var db = new MyDbContext())
            {
                db.UpdateRange(gdbGamesToUpdate);
                db.SaveChanges();
            }
        }
        public static void DeleteFromDatabase(GDBPlatformGame game)
        {
            using (var db = new MyDbContext())
            {
                db.Remove(game);
                db.SaveChanges();
            }
        }
    }
}
