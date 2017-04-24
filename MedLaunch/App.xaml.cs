using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;
using MedLaunch.Classes;
using MahApps.Metro;
using MedLaunch.Models;
using System.IO;
using MedLaunch.Classes.GamesLibrary;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using MedLaunch.Classes.Scraper;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        //public ScrapeDB ScrapedData { get; set; }

        /*
        private GameListBuilder gamesList;
        public GameListBuilder GamesList
        {
            get
            {
                return gamesList;
            }
            set
            {
                if (gamesList != value)
                {
                    gamesList = value;
                    OnPropertyChanged("GamesList");

                }
            }
        }
        */

        private GamesLibraryViewModel gamesLibrary;
        public GamesLibraryViewModel GamesLibrary
        {
            get
            {
                return gamesLibrary;
            }
            set
            {
                if (gamesLibrary != value)
                {
                    gamesLibrary = value;
                    OnPropertyChanged("GamesLibrary");

                }
            }
        }

        /// <summary>
        /// unhandled exception logging
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="s"></param>
        private static void LogUnhandledException(Exception exception, string s)
        {
            string DirectoryName = AppDomain.CurrentDomain.BaseDirectory;

            if (!Directory.Exists(DirectoryName))
            {
                Directory.CreateDirectory(DirectoryName);
            }

            var contents =
                string.Format(
                    "HResult:    {1}{0}" + "HelpLink:   {2}{0}" + "Message:    {3}{0}" + "Source:     {4}{0}"
                    + "StackTrace: {5}{0}" + "{0}",
                    Environment.NewLine,
                    exception.HResult,
                    exception.HelpLink,
                    exception.Message,
                    exception.Source,
                    exception.StackTrace);
            File.AppendAllText(DirectoryName + "\\Exceptions.log", "******************** Exception detail - " + DateTime.Now.ToString() + " - ********************\n\n");
            File.AppendAllText(string.Format("{0}Exceptions.log", DirectoryName), contents);
            File.AppendAllText(DirectoryName + "\\Exceptions.log", "\n\n****************************************\n\n");
        }


        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            // unhandled exception events
            AppDomain.CurrentDomain.UnhandledException +=
                (s, exception) =>
                LogUnhandledException((Exception)exception.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException +=
                (s, exception) =>
                LogUnhandledException(exception.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException +=
                (s, exception) =>
                LogUnhandledException(exception.Exception, "TaskScheduler.UnobservedException");



            var splashScreen = new SplashScreen(@"Data\Graphics\mediconsplash-new.png");
            splashScreen.Show(false);
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            // show the initialisation window and begin checks
            ShowInitWindow();
            Thread.Sleep(1000);
            // init should have completed - run MainWindow

            // instantiate GamesList object  
            //GamesList = new GameListBuilder();
            GamesLibrary = new GamesLibraryViewModel();

            MainWindow mw = new MedLaunch.MainWindow();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = mw;

            
            //mw.DataContext = _GamesLibraryViewModel;
            mw.Show();

            // instantiate GamesList object  
            //GamesList = new GameListBuilder();



            splashScreen.Close(TimeSpan.FromSeconds(1));

            // instantiate ScrapeDB
            //ScrapedData = new ScrapeDB();
            ScrapeDB sdb = new ScrapeDB();

            


            // set color scheme from database
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\MedLaunch.db"))
            {
                // database already exists
                var gs = GlobalSettings.GetGlobals();
                ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent(gs.colorAccent),
                                    ThemeManager.GetAppTheme(gs.colorBackground));
            }
            else
            {
                // database hasnt been generated yet - set default
                ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent("Emerald"),
                                    ThemeManager.GetAppTheme("BaseDark"));
            }
            

        }

        private void ShowInitWindow()
        {
            InitWindow init = new InitWindow();
            Application.Current.MainWindow = init;
            init.ShowDialog();
        }

        protected void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
