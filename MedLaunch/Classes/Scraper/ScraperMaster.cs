using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes.Scraper.DBModels;

namespace MedLaunch.Classes.Scraper
{
    public class ScraperMaster
    {
        public int gid { get; set; }
        public int pid { get; set; }
        public int? mid { get; set; }

        public string GDBTitle { get; set; }
        public string GDBPlatformName { get; set; }
        public string GDBPlatformAlias { get; set; }
        public string GDBYear { get; set; }

        public string MOBYTitle { get; set; }
        public string MOBYAlias { get; set; }
        public string MOBYPlatformName { get; set; } 
        public string MOBYPlatformAlias { get; set; }
        public string MOBYYear { get; set; }

        public List<string> Game_Docs { get; set; }

        public static List<ScraperMaster> MasterList { get; set; }

        public ScraperMaster()
        {
            Game_Docs = new List<string>();
            //if (MasterList == null)
                //MasterList = new List<ScraperMaster>();
        }

        public static List<ScraperMaster> GetMasterList()
        {
            List<ScraperMaster> mList = new List<ScraperMaster>();

            // get from masterview
            List<MasterView> mv = ScrapeDB.AllScrapeData; //MasterView.GetMasterView();

            // get gamedocs
            List<Game_Doc> docs = Game_Doc.GetDocs();

            // iterate through
            foreach (var entry in mv)
            {
                ScraperMaster sm = new ScraperMaster();
                sm.gid = entry.gid;
                sm.pid = entry.pid;
                sm.mid = entry.mid;
                sm.GDBTitle = entry.GDBTitle;
                sm.GDBPlatformName = entry.PlatformName;
                sm.GDBPlatformAlias = entry.PlatformAlias;
                sm.GDBYear = entry.GDBYear;
                sm.MOBYTitle = entry.MOBYTitle;
                sm.MOBYAlias = entry.MOBYAlias;
                sm.MOBYPlatformName = entry.MOBYPlatformName;
                sm.MOBYPlatformAlias = entry.MOBYPlatformAlias;
                sm.MOBYYear = entry.MOBYYear;

                List<string> ds = (from a in docs
                                   where a.gid == sm.gid
                                   select a.downloadUrl).ToList();
                if (ds.Count > 0)
                    sm.Game_Docs.AddRange(ds);

                mList.Add(sm);
            }
            return mList;
        }
    }
    /*
    public class ScraperMaster
    {
        public int GamesDbId { get; set; }
        public int MedLaunchSystemId { get; set; }
        public ScraperTGDB TGDBData { get; set; }
        public ScraperMoby MobyData { get; set; }
        public List<string> IDDBManual { get; set; }

        public ScraperMaster()
        {
            TGDBData = new ScraperTGDB();
            MobyData = new ScraperMoby();
        }
    }

    public class ScraperTGDB
    {
        public string GamesDBTitle { get; set; }
        public string GamesDBPlatformName { get; set; }
    }

    public class ScraperMoby
    {
        public string MobyTitle { get; set; }
        public string MobyURLName { get; set; }
        public string MobyPlatformName { get; set; }
    }

    */
}
