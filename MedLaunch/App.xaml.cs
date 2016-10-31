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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public GamesLibraryScrapedContent ScrapedData { get; set; }

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
        }

        private void ShowInitWindow()
        {
            InitWindow init = new InitWindow();
            Application.Current.MainWindow = init;
            init.ShowDialog();
        }
    }
}
