using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using MedLaunch.Classes;
using MedLaunch.Models;
using MedLaunch.Common.SQLite;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Threading;
using System.Reflection;
using MedLaunch.Extensions;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.Scraper;
using Microsoft.Win32;
using System.IO;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Window
    {
        public InitWindow()
        {
            InitializeComponent();
        }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Init()
        {
            // make sure class libraries are built            
            MedLaunch.Common.Startup.Start();

            // initialise directories if they do not exist
            SetupDirectories.Go();

            /* check that pre-reqs are installed */

           
            UpdateStatus("Scanning for pre-requisites...", true);

            UpdateStatus("Visual C++ 2010 x86 redist... ", true);
            // Visual c++ 2010 x86 (for SlimDX.dll)
            string keyName = "ProductName";
            string keyPath = @"SOFTWARE\Classes\Installer\Products";//\1D5E3C0FEDA1E123187686FED06E995A";

            var rootKey = MedLaunch.Common.RegistryHelpers.GetRegistryKey(keyPath);

            // get an array of all installerproduct subkey names
            string[] subKeys = rootKey.GetSubKeyNames();

            bool isFound = false;

            // iterate through each subkey and see if vcredist x86 is installed
            foreach (string k in subKeys)
            {
                string match = "Microsoft Visual C++ 2010  x86 Redistributable";
                string newKeyPath = keyPath + @"\" + k;
                var conString = MedLaunch.Common.RegistryHelpers.GetRegistryValue(newKeyPath, keyName);
                if (conString != null && conString.ToString().Contains(match))
                {
                    isFound = true;
                    break;
                }                    
            }

            if (isFound == true)
            {
                // it appears vcredist is installed
                UpdateStatus("...OK", true);
            }
            else
            {
                // doesnt appear to be installed
                UpdateStatus("...NOT FOUND", true);
                MessageBoxResult r = MessageBox.Show("MedLaunch requires the Microsoft Visual C++ 2010 (x86) Redistributable to be installed and this was NOT detected on your system.\n\nClicking YES will install this before proceeding.\nClicking NO will terminate MedLaunch.", "vcredist_x86.exe not installed", MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    // install
                    UpdateStatus("Installing vcredist_x86.exe...", true);
                    string currDir = Directory.GetCurrentDirectory();
                    string installerPath = currDir + @"\Data\Installers\vcredist_x86.exe";

                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = installerPath;
                    p.StartInfo.Arguments = "/passive /norestart";
                    p.Start();
                    p.WaitForExit();
                }
                else
                {
                    // exit medlaunch
                    Environment.Exit(0);
                }
            }

            

            // Check whether database exists
            UpdateStatus("Checking whether there is an existing database present", true);
            Task.Delay(1);
            string dbPath = @"Data\Settings\MedLaunch.db";
            if (Operations.CheckDbExists(dbPath) == true)
            {
                // database exists - check versions
                UpdateStatus("YES", false);

                UpdateStatus("Checking database version", true);
                string dbVersion = Operations.GetDbVersion();
                if (dbVersion == "")
                {
                    dbVersion = "NULL";
                    UpdateStatus(dbVersion + " - Database is probably too old to have a version number", false);
                }
                else
                {
                    UpdateStatus(dbVersion, false);
                }                

                UpdateStatus("Checking application version", true);
                string appVersion = Versions.ReturnApplicationVersion();
                UpdateStatus(appVersion, false);

                // compare versions and determine whether an upgrade is needed
                UpdateStatus("Comparing versions...", true);
                if (dbVersion != "NULL")
                {
                    string[] dbVersionArr = dbVersion.Split('.');
                    string[] appVersionArr = appVersion.Split('.');
                    bool upgradeNeeded = false;
                    for (int v = 0; v < 3; v++)
                    {
                        int appV = Convert.ToInt32(appVersionArr[v]);
                        int dbV = Convert.ToInt32(dbVersionArr[v]);

                        if (dbV > appV)
                        {
                            // database is NEWER than the current application version
                            Console.WriteLine("WARNING: Your MedLaunch database version is NEWER than your installed MedLaunch application!");
                            Console.WriteLine("It looks like you have installed an older version of MedLaunch over an existing MedLaunch directory.");
                            Console.WriteLine("This can only be bad and will need to be resolved by you before you continue.");
                            Console.WriteLine("Manual solution 1: Press any key to terminate this application. Then download the latest version of MedLaunch from GitHub and extract it over your current MedLaunch folder");
                            Console.WriteLine("Manual solution 2: Press any key to terminate this application. Then delete your MedLaunch database (.Data/Settings/MedLaunch.db). This will wipe out all of your settings/games/etc.");
                            Console.WriteLine("");
                            Console.WriteLine("Hint: Manual solution 1 is recommended.");
                            Console.WriteLine("");
                            Console.WriteLine("Press any key to abort Medlaunch initialisation......");

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("WARNING: Your MedLaunch database version is NEWER than your installed MedLaunch application!");
                            sb.AppendLine("It looks like you have installed an older version of MedLaunch over an existing MedLaunch directory.");
                            sb.AppendLine("This can only be bad and will need to be resolved by you before you continue.");
                            sb.AppendLine("Manual solution 1: Press any key to terminate this application. Then download the latest version of MedLaunch from GitHub and extract it over your current MedLaunch folder");
                            sb.AppendLine("Manual solution 2: Press any key to terminate this application. Then delete your MedLaunch database (.Data/Settings/MedLaunch.db). This will wipe out all of your settings/games/etc.");
                            sb.AppendLine("Hint: Manual solution 1 is recommended.");
                            sb.AppendLine("Press OK to quit MedLaunch");

                            //Console.ReadKey();
                            //Thread.Sleep(10000);
                            MessageBox.Show(sb.ToString());
                            Environment.Exit(0);
                            break;
                        }
                        if (dbV == appV)
                        {
                            continue;
                        }
                        if (dbV < appV)
                        {
                            // database is OLDER than application - upgrade needed
                            upgradeNeeded = true;
                            break;
                        }

                    }
                    
                    
                    if (upgradeNeeded == true)
                    {
                        // unhide the init window
                        this.Visibility = Visibility.Visible;
                        this.Refresh();
                        // start db upgrade routine
                        UpdateStatus("Database upgrade is needed", true);
                        DoDbUpgrade(dbVersion, appVersion);
                    }
                    else
                    {
                        // upgrade not needed - proceed with main application launch
                        UpdateStatus("Database upgrade is not needed", true);
                        return;
                    }
                }
                else
                {
                    // upgrade definiately needed as old database didnt contain a dbversion entry (ie - it is very old)
                    this.Visibility = Visibility.Visible;
                    this.Refresh();
                    UpdateStatus("Database upgrade is needed", true);
                    DoDbUpgrade(dbVersion, appVersion);
                }

                
            }
            else
            {
                this.Visibility = Visibility.Visible;
                this.Refresh();
                // db does not exist - proceed with creation and seeding
                UpdateStatus("NO", false);
                UpdateStatus("Creating database", true);
                CreateDatabase();
                UpdateStatus("Done", false);
                UpdateStatus("Seeding database with default values", true);
                DbEF.InitialSeed();
                UpdateStatus("Done", false);

                // ask whether to enable auto-update checking
                GlobalSettings gs = GlobalSettings.GetGlobals();
                MessageBoxResult re = MessageBox.Show("MedLaunch can automatically check for new updates when it starts.\n Do you wish to enable this feature?\n(An internet connection is required)", "Update Checking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (re == MessageBoxResult.Yes)
                {
                    gs.checkUpdatesOnStart = true;
                }
                else
                {
                    gs.checkUpdatesOnStart = false;
                }
                GlobalSettings.SetGlobals(gs);

                UpdateStatus("Done", false);

                return;
            }
            return;
        }

        private void DoDbUpgrade(string dbVersion, string appVersion)
        {
            UpdateStatus("Starting database upgrade", true);
            MessageBoxResult result = MessageBox.Show("The MedLaunch settings database needs to be upgraded before the application can run (" + dbVersion + " > " + appVersion + "). Do you wish to proceed?\n\nYES will backup the database content and proceed.\nNO will terminate MedLaunch", "MedLaunch DB Upgrade", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                Environment.Exit(0);

            /* proceed with upgrade */

            // get a class with all the database information and values
            Database dbData = Operations.GetDatabaseObject(AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\MedLaunch.db");

            // save data to json
            string json = JsonConvert.SerializeObject(dbData, Formatting.Indented);

            // rename existing database
            string currtime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newfileName = @"Data\Settings\MedLaunch_" + dbVersion + "_" + currtime + ".db";

            SQLiteConnection.ClearAllPools();
            // attempt database rename
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            Thread.Sleep(10);
            try
            {
                System.IO.File.Move(AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\MedLaunch.db", AppDomain.CurrentDomain.BaseDirectory + newfileName);
            }
            catch (Exception e) {
                
                Console.WriteLine(e);
                MessageBox.Show("ERROR: The database file is locked (perhaps open in another application.\nMedLaunch will now shut down.");
                Environment.Exit(0);
            }

            // database has now been renamed - call the creation and seeding code
            UpdateStatus("Old database (version:" + dbVersion + ") has been loaded into memory and renamed ", true);
            UpdateStatus("Creating a new database file (version: " + appVersion + ")", true);
            CreateDatabase();
            UpdateStatus("Done", false);
            UpdateStatus("Seeding database with default values", true);
            DbEF.InitialSeed();
            //GDBPlatformGame.InitialSeed();
            UpdateStatus("Done", false);

            // database should be set up with all default settings. Now import all the old data
            UpdateStatus("Starting import of old data", true);

            using (var context = new MyDbContext())
            {
                // create lists for each table in the new database
                List<ConfigBaseSettings> _configBaseSettings = new List<ConfigBaseSettings>();
                List<ConfigNetplaySettings> _configNetplaySettings = new List<ConfigNetplaySettings>();
                List<ConfigServerSettings> _configServerSettings = new List<ConfigServerSettings>();
                /*
                List<GDBGameData> _gDBGameDatas = new List<GDBGameData>();
                List<GDBLink> _gDBLinks = new List<GDBLink>();
                */
                List<GDBPlatformGame> _gDBPlatformGames = new List<GDBPlatformGame>();
                List<Game> _games = new List<Game>();
                List<GlobalSettings> _globalSettings = new List<GlobalSettings>();
                List<Paths> _paths = new List<Paths>();
                List<Versions> _versions = new List<Versions>();

                List<MednaNetSettings> _mednaNet = new List<MednaNetSettings>();

                /* Process each one */

                foreach (Tab table in dbData.Tables)
                {
                    string tableName = table.TableName;
                    UpdateStatus("Processing Data for: " + tableName, true);
                    switch (tableName)
                    {
                        case "ConfigBaseSettings":                            
                            List<List<Data>> cfbsRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfbsRows)
                            {
                                
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //ConfigBaseSettings settings = new ConfigBaseSettings();
                                // get the whole record from the database
                                ConfigBaseSettings settings = ConfigBaseSettings.GetConfig(primaryKey);
                                if (settings == null) { settings = new ConfigBaseSettings(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _configBaseSettings.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Configs", true);
                            ConfigBaseSettings.SaveToDatabase(_configBaseSettings);
                            
                            break;

                        case "ConfigNetplaySettings":
                            List<List<Data>> cfnpsRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfnpsRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //ConfigNetplaySettings settings = new ConfigNetplaySettings();
                                // get the whole record from the database
                                ConfigNetplaySettings settings = ConfigNetplaySettings.GetNetplay();
                                if (settings == null) { settings = new ConfigNetplaySettings(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _configNetplaySettings.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Netplay", true);
                            ConfigNetplaySettings.SaveToDatabase(_configNetplaySettings);
                            
                            break;

                        case "ConfigServerSettings":
                            List<List<Data>> cfssRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfssRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //ConfigServerSettings settings = new ConfigServerSettings();
                                // get the whole record from the database
                                ConfigServerSettings settings = ConfigServerSettings.GetServer(primaryKey);
                                if (settings == null) { settings = new ConfigServerSettings(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _configServerSettings.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Servers", true);
                            ConfigServerSettings.SaveToDatabase(_configServerSettings);
                           
                            break;
                         
                        case "Game":
                            List<List<Data>> gameRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in gameRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //Game settings = new Game();
                                // get the whole record from the database
                                Game settings = Game.GetGame(primaryKey);
                                if (settings == null) { settings = new Game(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _games.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Games", true);
                            Game.SaveToDatabase(_games, true);
                           
                            break;

                        case "GlobalSettings":
                            List<List<Data>> gsRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in gsRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //GlobalSettings settings = new GlobalSettings();
                                // get the whole record from the database
                                GlobalSettings settings = GlobalSettings.GetGlobals();
                                if (settings == null) { settings = new GlobalSettings(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _globalSettings.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Global Settings", true);
                            GlobalSettings.SaveToDatabase(_globalSettings);
                            
                            break;

                        case "MednaNetSettings":
                            List<List<Data>> msRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in msRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                // get the whole record from the database
                                MednaNetSettings settingsM = MednaNetSettings.GetGlobals();
                                if (settingsM == null) { settingsM = new MednaNetSettings(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settingsM.GetType().GetProperty(name);
                                    SetPropertyValue(settingsM, p1, type, value);
                                }
                                _mednaNet.Add(settingsM);
                            }
                            UpdateStatus("Init Database Update: Global Settings", true);
                            MednaNetSettings.SetGlobals(_mednaNet.FirstOrDefault());

                            break;

                        case "Paths":
                            List<List<Data>> pathRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in pathRows)
                            {
                                // get the primary key value for this row
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                //Paths settings = new Paths();
                                // get the whole record from the database
                                Paths settings = Paths.GetPaths();
                                if (settings == null) { settings = new Paths(); }
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _paths.Add(settings);
                            }
                            UpdateStatus("Init Database Update: Paths", true);
                            Paths.SaveToDatabase(_paths);
                            break;

                        case "Versions":
                            List<List<Data>> verRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in verRows)
                            {
                                // get the primary key value for this row
                                /*
                                var rArr = row.ToArray();
                                Data data = rArr[0];
                                int primaryKey = Convert.ToInt32(data.PrimaryKeyValue);
                                Versions settings = new Versions();
                                // get the whole record from the database
                                settings = Versions.GetVersionDefaults();
                                // we dont want to import the old data for this - just set it to new
                                _versions.Add(settings);
                                */
                            }
                            break;
                    }
                }



                // skip versions

                // Now we should be done - proceed with main application

                // start re-linking routine (copy flatfile scraped text into database if game manual edit is not enabled)
                UpdateStatus("Re-Linking orphaned scraped data...", true);

                List<Game> gToUpdate = new List<Game>();

                // get all games from database where manual editing is not enabled and it has a gdbid set
                var games = Game.GetGames().Where(a => a.ManualEditSet != true && (a.gdbId != null && a.gdbId > 0)).ToList();

                UpdateStatus("Processing " + games.Count().ToString() + " detected games", true);
                UpdateStatus("Please wait....", true);

                // iterate through each game
                foreach (Game g in games)
                {
                    // attempt to load xml data from disk
                    ScrapedGameObject o = ScrapeDB.GetScrapedGameObject(g.gameId, g.gdbId.Value);

                    // populate fields
                    StringBuilder sbAT = new StringBuilder();
                    if (o.Data.AlternateTitles == null)
                        o.Data.AlternateTitles = new List<string>();

                    for (int i = 0; i < o.Data.AlternateTitles.Count(); i++)
                    {
                        sbAT.Append(o.Data.AlternateTitles[i]);
                        if (i > 0 && i < (o.Data.AlternateTitles.Count() - 1))
                            sbAT.Append(", ");
                    }
                    g.AlternateTitles = sbAT.ToString();

                    StringBuilder sbGN = new StringBuilder();
                    if (o.Data.Genres == null)
                        o.Data.Genres = new List<string>();

                    for (int i = 0; i < o.Data.Genres.Count(); i++)
                    {
                        sbGN.Append(o.Data.Genres[i]);
                        if (i > 0 && i < (o.Data.Genres.Count() - 1))
                            sbGN.Append(", ");
                    }
                    g.Genres = sbGN.ToString();

                    g.Coop = o.Data.Coop;
                    g.Developer = o.Data.Developer;
                    g.Publisher = o.Data.Publisher;
                    g.Overview = o.Data.Overview;
                    g.Players = o.Data.Players;
                    g.Year = o.Data.Released;
                    g.ESRB = o.Data.ESRB;
                    g.gameName = o.Data.Title;

                    // add game to be updated
                    gToUpdate.Add(g);
                }

                // update all games
                Game.SaveToDatabase(gToUpdate);

            }     
        }

        
        public static void SetPropertyValue(ConfigBaseSettings settings, PropertyInfo p, string type, string value)
        {
            if (p == null)
                return;

            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        public static void SetPropertyValue(ConfigNetplaySettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        public static void SetPropertyValue(ConfigServerSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        /*
        private static void SetPropertyValue(GDBGameData settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
                
            }
        }
        */
        /*
        private static void SetPropertyValue(GDBLink settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        */
        private static void SetPropertyValue(GDBPlatformGame settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        private static void SetPropertyValue(Game settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?) && value != "")
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }

        private static void SetPropertyValue(GlobalSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }

        private static void SetPropertyValue(MednaNetSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }

        private static void SetPropertyValue(Paths settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }
        private static void SetPropertyValue(Versions settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(string))
            {
                var v = Convert.ToString(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(double?) && value != "")
            {
                var v = Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int?) && value != "")
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool?) && value != "")
            {
                var v = Convert.ToBoolean(Convert.ToInt32(value));
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime))
            {
                var v = Convert.ToDateTime(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(DateTime?))
            {
                if (value != "")
                {
                    var v = Convert.ToDateTime(value);
                    p.SetValue(settings, v, null);
                }
            }
        }

        private static List<Col> ReturnColumns(List<Tab> TableList, string TableName)
        {
            Tab t = TableList.Where(a => a.TableName == TableName).FirstOrDefault();
            return t.Columns;
        }

        // take our list of Tab objects and convert this into a list of rows
        private static List<List<Data>> ReturnRows(List<Tab> TableList, string TableName)
        {
            // new rows object
            List<List<Data>> rows = new List<List<Data>>();

            // get just the table that we are interested in
            Tab sT = (from a in TableList
                               where a.TableName == TableName
                               select a).FirstOrDefault();
            if (sT == null)
            {
                // no result returned
                return null;
            }

            string tableName = sT.TableName;
            string primKeyName = sT.PrimaryKeyColumn;

            // get all primary key values into an array (this will form the rows)
            int[] keys = (from a in sT.Data
                          select a.PrimaryKeyValue).Distinct().ToArray();

            // build a list of rows
            int i = 0;
            while (i < keys.Length)
            {
                List<Data> d = new List<Data>();
                // get all data objects that have a primary key of value keys[i]
                d = (from a in sT.Data
                     where a.PrimaryKeyValue == keys[i]
                     select a).ToList();   
                rows.Add(d);
                i++;
            }

            return rows;
        }

        private void CreateDatabase()
        {
            //UpdateStatus("Creating database", true);
            // initialise SQLite db if it does not already exist
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                // populate stock data 
                context.SaveChanges();
            }
        }

        private void UpdateStatus(string s, bool newline)
        {
            // get window
            Window win = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            // get textbox
            TextBlock t = (TextBlock)win.FindName("tbStatus");
            // get scrollviewer
            ScrollViewer sv = (ScrollViewer)win.FindName("scr");

            // get the current status
            string current = t.Text;
            string output = current;
            if (newline == true)
                output += "\n";
            else { output += " ... "; }
            
            // append s to the textbox
            t.Text = output + s;
            t.Refresh();
            Thread.Sleep(50);

            // scroll to end
            sv.ScrollToBottom();
            sv.Refresh();
            Thread.Sleep(50);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // hide the init window
            this.Visibility = Visibility.Collapsed;
            Init();
            // if beginit returns false then terminate the whole application
            //if (b == false)
            //   Environment.Exit(0);


            UpdateStatus("Preparing to start the main MedLaunch application....", true);
            Thread.Sleep(100);


            // init has returned true - close this window and start mainwindow
            Window win = Application.Current.Windows.OfType<Window>().FirstOrDefault();
            win.Close();
        }
    }
}
