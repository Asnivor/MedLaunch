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
using Asnitech.SQLite;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Threading;
using System.Reflection;

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

        private static void Init()
        {
            // make sure class libraries are built
            Asnitech.Launch.Common.Startup.Start();

            // initialise directories if they do not exist
            SetupDirectories.Go();

            // Check whether database exists
            UpdateStatus("Checking whether there is an existing database present", true);
            string dbPath = @"Data\Settings\MedLaunch.db";
            if (Operations.CheckDbExists(dbPath) == true)
            {
                // database exists - check versions
                UpdateStatus("YES", false);

                UpdateStatus("Checking database version", true);
                string dbVersion = Operations.GetDbVersion();
                UpdateStatus(dbVersion, false);

                UpdateStatus("Checking application version", true);
                string appVersion = Versions.ReturnApplicationVersion();
                UpdateStatus(appVersion, false);

                // compare versions and determine whether an upgrade is needed
                UpdateStatus("Comparing versions...", true);
                string[] dbVersionArr = dbVersion.Split('.');
                string[] appVersionArr = appVersion.Split('.');
                int i = 0;
                bool upgradeNeeded = false;
                while (i < 3)
                {
                    // if anything but the 4th number (private build) is greater in the appVersion - database needs to be upgraded
                    if (Convert.ToInt32(appVersionArr[i]) > Convert.ToInt32(dbVersionArr[i]))
                    {
                        // database upgrade needed
                        upgradeNeeded = true;
                        break;
                    }
                    i++;
                }
                if (upgradeNeeded == true)
                {
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
                // db does not exist - proceed with creation and seeding
                UpdateStatus("NO", false);
                UpdateStatus("Creating database", true);
                CreateDatabase();
                UpdateStatus("Done", false);
                UpdateStatus("Seeding database with default values", true);
                DbEF.InitialSeed();
                UpdateStatus("Done", false);
                return;
            }
            return;
        }

        private static void DoSeed()
        {

        }

        private static void DoDbUpgrade(string dbVersion, string appVersion)
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
            catch {
                Exception e;
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
            UpdateStatus("Done", false);

            // database should be set up with all default settings. Now import all the old data
            UpdateStatus("Starting import of old data", true);

            using (var context = new MyDbContext())
            {
                // create lists for each table in the new database
                List<ConfigBaseSettings> _configBaseSettings = new List<ConfigBaseSettings>();
                List<ConfigNetplaySettings> _configNetplaySettings = new List<ConfigNetplaySettings>();
                List<ConfigServerSettings> _configServerSettings = new List<ConfigServerSettings>();
                List<GDBGameData> _gDBGameDatas = new List<GDBGameData>();
                List<GDBLink> _gDBLinks = new List<GDBLink>();
                List<GDBPlatformGame> _gDBPlatformGames = new List<GDBPlatformGame>();
                List<Game> _games = new List<Game>();
                List<GlobalSettings> _globalSettings = new List<GlobalSettings>();
                List<Paths> _paths = new List<Paths>();
                List<Versions> _versions = new List<Versions>();

                /* Process each one */

                foreach (Tab table in dbData.Tables)
                {
                    string tableName = table.TableName;
                    switch (tableName)
                    {
                        case "ConfigBaseSettings":
                            List<List<Data>> cfbsRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfbsRows)
                            {
                                ConfigBaseSettings settings = new ConfigBaseSettings();
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
                            break;
                        case "ConfigNetplaySettings":
                            List<List<Data>> cfnpsRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfnpsRows)
                            {
                                ConfigNetplaySettings settings = new ConfigNetplaySettings();
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
                            break;
                        case "ConfigServerSettings":
                            List<List<Data>> cfssRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in cfssRows)
                            {
                                ConfigServerSettings settings = new ConfigServerSettings();
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
                            break;
                        case "GDBGameData":
                            List<List<Data>> gdbgdRows = ReturnRows(dbData.Tables, tableName);
                            foreach (List<Data> row in gdbgdRows)
                            {
                                GDBGameData settings = new GDBGameData();
                                foreach (Data item in row)
                                {
                                    string name = item.ColName;
                                    string type = item.ColType;
                                    string value = item.Value;
                                    PropertyInfo p1 = settings.GetType().GetProperty(name);
                                    SetPropertyValue(settings, p1, type, value);
                                }
                                _gDBGameDatas.Add(settings);
                            }
                            break;
                        case "GDBLink":
                            break;
                        case "GDBPlatformGame":
                            break;
                        case "Game":
                            break;
                        case "GlobalSettings":
                            break;
                        case "Paths":
                            break;
                        case "Versions":
                            break;
                    }
                }
            }     
        }

        
        private static void SetPropertyValue(ConfigBaseSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(ConfigNetplaySettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(ConfigServerSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(GDBGameData settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(GDBLink settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(GDBPlatformGame settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(Game settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(GlobalSettings settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(Paths settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
            }
        }
        private static void SetPropertyValue(Versions settings, PropertyInfo p, string type, string value)
        {
            if (p.PropertyType == typeof(double))
            {
                var v = Convert.ToDouble(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(int))
            {
                var v = Convert.ToInt32(value);
                p.SetValue(settings, v, null);
            }
            if (p.PropertyType == typeof(bool))
            {
                var v = Convert.ToBoolean(value);
                p.SetValue(settings, v, null);
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
                          select a.PrimaryKeyValue).ToArray();

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

        private static void CreateDatabase()
        {
            UpdateStatus("Creating database", true);
            // initialise SQLite db if it does not already exist
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                // populate stock data 
                context.SaveChanges();
            }
        }

        private static void UpdateStatus(string s, bool newline)
        {
            // get window
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault();
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

            // scroll to end
            sv.ScrollToBottom();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Init();
            // if beginit returns false then terminate the whole application
            //if (b == false)
             //   Environment.Exit(0);
         
            
            // init has returned true - close this window and start mainwindow
            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault();
            win.Close();
        }
    }
}
