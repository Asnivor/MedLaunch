using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes.TheGamesDB;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedLaunch.Models
{
    /*
    public class GDBGameData
    {
        public static string Serialize(List<string> strList)
        {
            string str = "";
            foreach (string s in strList)
            {
                str += s + ";";
            }
            return str.TrimEnd(';');
        }

        

        public static List<string> DeSerialize(string str)
        {
            List<string> ls = new List<string>();
            if (str == null || str == "")
            {
                return ls;
            }
            string[] arr = str.Split(';');
            foreach (string s in arr)
            {
                ls.Add(s);
            }
            return ls;
        }

        public static string JsonSerialize(List<string> strList)
        {
            string json = "";
            if (strList != null && strList.Count > 0)
            {
                json = JsonConvert.SerializeObject(strList);
            }            
            return json;
        }

        public static List<string> JsonDeSerialize(string json)
        {
            List<string> strList = new List<string>();
            if (json != null && json != "")
            {
                strList = JsonConvert.DeserializeObject<List<string>>(json);
            }           
            
            return strList;
        }

        public static GDBGameData GetGame(int gdbId)
        {
            using (var db = new MyDbContext())
            {
                var c = db.GDBGameData.Where(a => a.GdbId == gdbId).FirstOrDefault();
                return c;
            }
        }

        public int GdbId { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
        public string ESRB { get; set; }
        public string Players { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Rating { get; set; }
        public string Coop { get; set; }
        public string AlternateTitles { get; set; }
        public string Genres { get; set; }
        public string BoxartBackLocalImage { get; set; }
        public string BoxartFrontLocalImage { get; set; }
        public string FanartLocalImages { get; set; }
        public string BannerLocalImages { get; set; }
        public string ScreenshotLocalImages { get; set; }

        public DateTime? LastScraped { get; set; }


        public static void SaveToDatabase(List<GDBGameData> games)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.GDBGameData.AsNoTracking().ToList();

                List<GDBGameData> toAdd = new List<GDBGameData>();
                List<GDBGameData> toUpdate = new List<GDBGameData>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in games)
                {
                    GDBGameData t = (from a in current
                                         where a.GdbId == g.GdbId
                                         select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.GDBGameData.UpdateRange(toUpdate);
                db.GDBGameData.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        public static void SaveToDatabase(GDBGameData game)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.GDBGameData.AsNoTracking().ToList();

                List<GDBGameData> toAdd = new List<GDBGameData>();
                List<GDBGameData> toUpdate = new List<GDBGameData>();

                GDBGameData t = (from a in current
                                     where a.GdbId == game.GdbId
                                     select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(game); }
                    else { toUpdate.Add(game); }
                
                db.GDBGameData.UpdateRange(toUpdate);
                db.GDBGameData.AddRange(toAdd);
                db.SaveChanges();
            }
        }


    }

    */
}

   
