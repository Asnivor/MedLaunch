using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes.Scraper.DBModels;

namespace MedLaunch.Classes.Scraper
{
    public class ScrapeDB
    {
        // properties        
        public static List<MasterView> AllScrapeData { get; set; }
        public static string BaseContentDirectory { get; set; }

        // constructor
        public ScrapeDB()
        {
            // set base content dir
            BaseContentDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games";

            // load scrape data from sqlite db
            if (AllScrapeData == null)
            {
                //AllScrapeData = ScraperMaster.GetMasterList();
                ReloadMasterObject();
            }
                

            // ensure initial directory structure is created
            Directory.CreateDirectory(BaseContentDirectory);
        }

        /* METHODS */

        /// <summary>
        /// saves scraped data object to json in the gamedata directory
        /// </summary>
        /// <param name="o"></param>
        public static void SaveJson(ScrapedGameObjectWeb o)
        {
            string gPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + o.GdbId.ToString() + @"\" + o.GdbId.ToString() + ".json";
            string json = JsonConvert.SerializeObject(o, Formatting.Indented);
            File.WriteAllText(gPath, json);
        }

        /// <summary>
        /// looks up and returns scrapeddataobject based on Internal GameId (not gamesdb id)
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GdbId"></param>
        /// <returns></returns>
        public static ScrapedGameObject GetScrapedGameObject(int GameId, int GdbId)
        {
            //Game link = Game.GetGame(GameId);

            // we have a link record - proceed and generate object
            ScrapedGameObject sgo = new ScrapedGameObject();
            if (GdbId < 1)
                return null;
            sgo.GdbId = GdbId;

            // attempt to load game data json
            string gPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + sgo.GdbId.ToString() + @"\" + sgo.GdbId.ToString() + ".json";

            if (File.Exists(gPath))
            {
                ScrapedGameObjectWeb sgoweb = new ScrapedGameObjectWeb();
                ScrapedGameData sg = new ScrapedGameData();
                string jsonString = File.ReadAllText(gPath);
                try
                {
                    sgoweb = JsonConvert.DeserializeObject<ScrapedGameObjectWeb>(jsonString);
                }
                catch (Exception e)
                {
                    // there was a problem with the file - do nothing
                    Console.WriteLine(e);
                }
                finally
                {
                    sgo.Data = sgoweb.Data;
                }
            }
            else { sgo.Data = new ScrapedGameData(); }

            // populate lists in object
            string baseGameDir = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + sgo.GdbId.ToString();

            sgo.BackCovers = GetAllFolderFiles(baseGameDir + @"\BackCover");
            sgo.Banners = GetAllFolderFiles(baseGameDir + @"\Banners");
            sgo.FanArts = GetAllFolderFiles(baseGameDir + @"\FanArt");
            sgo.FrontCovers = GetAllFolderFiles(baseGameDir + @"\FrontCover");
            sgo.Manuals = GetAllFolderFiles(baseGameDir + @"\Manual");
            sgo.Medias = GetAllFolderFiles(baseGameDir + @"\Media");
            sgo.Screenshots = GetAllFolderFiles(baseGameDir + @"\Screenshots");

            // return object
            return sgo;
        }

        /// <summary>
        /// return all files in a folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static List<string> GetAllFolderFiles(string folderPath)
        {
            List<string> list = new List<string>();

            // check folder exists
            if (!Directory.Exists(folderPath))
                return list;

            // enumerate all files in folder (non-recursive)
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (string s in fileEntries.Where(a => !a.EndsWith(".php"))) { list.Add(s); }
            return list;
        }

        /// <summary>
        /// create gamedata game folder structure
        /// </summary>
        /// <param name="gdbId"></param>
        public static void CreateFolderStructure(int gdbId)
        {
            string basePath = BaseContentDirectory + @"\" + gdbId.ToString() + @"\";
            // boxart
            System.IO.Directory.CreateDirectory(basePath + "FrontCover");
            System.IO.Directory.CreateDirectory(basePath + "BackCover");
            System.IO.Directory.CreateDirectory(basePath + "Media");
            System.IO.Directory.CreateDirectory(basePath + "Banners");
            System.IO.Directory.CreateDirectory(basePath + "Screenshots");
            System.IO.Directory.CreateDirectory(basePath + "PromoArt");
            System.IO.Directory.CreateDirectory(basePath + "FanArt");
            System.IO.Directory.CreateDirectory(basePath + "Manual");
        }

        public void ReloadMasterObject()
        {
            AllScrapeData = MasterView.GetMasterView();
        }
    }
}
