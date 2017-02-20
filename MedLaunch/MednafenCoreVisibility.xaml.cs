using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MedLaunch.Classes;
using MedLaunch.Models;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Extensions;
using MedLaunch.Classes.VisualHandlers;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for LibraryColumnChooser.xaml
    /// </summary>
    public partial class MednafenCoreVisibility : ChildWindow
    {
        public MednafenCoreVisibility()
        {
            InitializeComponent();

            // load all settings from database
            GlobalSettings gs = GlobalSettings.GetGlobals();

            coreVis1.IsChecked = gs.coreVis1;
            coreVis2.IsChecked = gs.coreVis2;
            coreVis3.IsChecked = gs.coreVis3;
            coreVis4.IsChecked = gs.coreVis4;
            coreVis5.IsChecked = gs.coreVis5;
            coreVis6.IsChecked = gs.coreVis6;
            coreVis7.IsChecked = gs.coreVis7;
            coreVis8.IsChecked = gs.coreVis8;
            coreVis9.IsChecked = gs.coreVis9;
            coreVis10.IsChecked = gs.coreVis10;
            coreVis11.IsChecked = gs.coreVis11;
            coreVis12.IsChecked = gs.coreVis12;
            coreVis13.IsChecked = gs.coreVis13;
            coreVis14.IsChecked = gs.coreVis14;
            coreVis15.IsChecked = gs.coreVis15;
            coreVis16.IsChecked = false;// gs.coreVis16;
            coreVis17.IsChecked = false;// gs.coreVis17;
            coreVis18.IsChecked = gs.coreVis18;

            // hide faust and fast
            coreVis16.Visibility = Visibility.Collapsed;
            coreVis17.Visibility = Visibility.Collapsed;
        }

        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            List<bool> bs = new List<bool>()
            {
                coreVis1.IsChecked.Value,
                coreVis2.IsChecked.Value,
                coreVis3.IsChecked.Value,
                coreVis4.IsChecked.Value,
                coreVis5.IsChecked.Value,
                coreVis6.IsChecked.Value,
                coreVis7.IsChecked.Value,
                coreVis8.IsChecked.Value,
                coreVis9.IsChecked.Value,
                coreVis10.IsChecked.Value,
                coreVis11.IsChecked.Value,
                coreVis12.IsChecked.Value,
                coreVis13.IsChecked.Value,
                coreVis14.IsChecked.Value,
                coreVis15.IsChecked.Value,
                coreVis16.IsChecked.Value,
                coreVis17.IsChecked.Value,
                coreVis18.IsChecked.Value
            };

            bool[] bsa = bs.ToArray();

            // pass results to database
            GlobalSettings.SaveCoreVisibilities(bsa);

            // update
            MiscVisualHandler.RefreshCoreVisibilities();

            Classes.GamesLibrary.GamesLibraryDataGridRefresh.Update();

            this.Close();
        }
    }
}
