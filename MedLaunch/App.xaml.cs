using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            // show the initialisation window and begin checks
            ShowInitWindow();
            Thread.Sleep(1000);
            // init should have completed - run MainWindow
            MainWindow mw = new MedLaunch.MainWindow();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow = mw;
            mw.Show();
        }

        private void ShowInitWindow()
        {
            InitWindow init = new InitWindow();
            Application.Current.MainWindow = init;
            init.ShowDialog();
        }
    }
}
