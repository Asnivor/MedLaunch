using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using MedLaunch.Classes.TheGamesDB;
using MahApps.Metro.Controls.Dialogs;
using System.Text.RegularExpressions;
using FuzzyString;
using System.Windows;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Threading;

namespace MedLaunch.Classes
{
    // class for scraped sidebar data - instantiated once on main page then passed around (loads masterscraper data from json file once)
    public class GamesLibraryScrapedContent
    {
        // properties
        public string BaseContentDirectory { get; set; }
        public List<ScraperMaster> MasterPlatformList { get; set; }

        // constructor
        public GamesLibraryScrapedContent()
        {
            // set base content dir
            BaseContentDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games";

            // load the master json file
            string masterPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\MasterGames.json";
            string json = File.ReadAllText(masterPath);
            MasterPlatformList = JsonConvert.DeserializeObject<List<ScraperMaster>>(json);

            // ensure initial directory structure is created
            Directory.CreateDirectory(BaseContentDirectory);
        }

        /* METHODS */

        // looks up and returns scrapeddataobject based on Internal GameId (not gamesdb id)
        public ScrapedGameObject GetScrapedGameObject(int GameId)
        {
            // look up in link table to see if game has a link
            List<GDBLink> links = GDBLink.GetRecords(GameId);
            if (links.Count == 0)
                return null;
            GDBLink link = links.First();

            // we have a link record - proceed and generate object
            ScrapedGameObject sgo = new ScrapedGameObject();
            sgo.GdbId = link.GdbId.Value;

            // attempt to load game data json
            string gPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + sgo.GdbId.ToString() + @"\" + sgo.GdbId.ToString() + ".json";
            
            if (File.Exists(gPath))
            {
                ScrapedGameData sg = new ScrapedGameData();
                string jsonString = File.ReadAllText(gPath);
                try
                {
                    sg = JsonConvert.DeserializeObject<ScrapedGameData>(jsonString);
                }
                catch (Exception e)
                {
                    // there was a problem with the file - do nothing
                }
                finally
                {
                    sgo.Data = sg;
                }                
            }
            else { sgo.Data = new ScrapedGameData(); }

            // populate lists in object
            string baseGameDir = AppDomain.CurrentDomain.BaseDirectory + @"Data\Games\" + sgo.GdbId.ToString();

            sgo.BackCovers = GetAllFolderFiles(baseGameDir + @"\BackCovers");
            sgo.Banners = GetAllFolderFiles(baseGameDir + @"\Banners");
            sgo.FanArts = GetAllFolderFiles(baseGameDir + @"\FanArts");
            sgo.FrontCovers = GetAllFolderFiles(baseGameDir + @"\FrontCovers");
            sgo.Manuals = GetAllFolderFiles(baseGameDir + @"\Manuals");
            sgo.Medias = GetAllFolderFiles(baseGameDir + @"\Medias");
            sgo.PromoArts = GetAllFolderFiles(baseGameDir + @"\PromoArts");
            sgo.Screenshots = GetAllFolderFiles(baseGameDir + @"\Screenshots");

            // return object
            return sgo;
        }

        public static List<string> GetAllFolderFiles(string folderPath)
        {
            List<string> list = new List<string>();

            // check folder exists
            if (!Directory.Exists(folderPath))
                return list;

            // enumerate all files in folder (non-recursive)
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (string s in fileEntries) { list.Add(s); }
            return list;
        }
        

        public void CreateFolderStructure(int gdbId)
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
            // reload the master json file
            string masterPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\MasterGames.json";
            string json = File.ReadAllText(masterPath);
            MasterPlatformList = JsonConvert.DeserializeObject<List<ScraperMaster>>(json);
        }
    }
}
