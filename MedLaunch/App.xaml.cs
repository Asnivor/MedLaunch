using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            // show the initialisation window and begin checks
            ShowInitWindow();

            // init should have completed - run MainWindow
            MainWindow mw = new MedLaunch.MainWindow();
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
