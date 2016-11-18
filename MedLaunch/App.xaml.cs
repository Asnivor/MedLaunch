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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public GamesLibraryScrapedContent ScrapedData { get; set; }
        public GameListBuilder GamesList { get; set; }

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            var splashScreen = new SplashScreen(@"Data\Graphics\mediconsplash.png");
            splashScreen.Show(false);
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            // show the initialisation window and begin checks
            ShowInitWindow();
            Thread.Sleep(1000);
            // init should have completed - run MainWindow

            

            MainWindow mw = new MedLaunch.MainWindow();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = mw;
            mw.Show();

            splashScreen.Close(TimeSpan.FromSeconds(1));

            // instantiate ScrapedContent Object
            ScrapedData = new GamesLibraryScrapedContent();

            // instantiate GamesList object
            GamesList = new GameListBuilder();


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
    }
}
