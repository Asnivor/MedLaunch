using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public static class GamesLibraryVisualHandler
    {
        // Get the current setup of the games library (selected filters etc), do a refresh then return to previous configuration
        public static void RefreshGamesLibrary()
        {
            // get an instance of the MainWindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get btnClearFilters button
            Button btnClearFilters = (Button)mw.FindName("btnClearFilters");

            // get the wrappanel that contains all the GameLibrary toggleboxes
            Grid grdGameLibrary = (Grid)mw.FindName("grdGameLibrary");

            // get the dynamic filter textbox tbFilterDatagrid
            TextBox tbFilterDatagrid = (TextBox)mw.FindName("tbFilterDatagrid");
            string tbText = tbFilterDatagrid.Text;

            // get all grouped radio buttons
            List<RadioButton> buttons = UIHandler.GetLogicalChildCollection<RadioButton>(grdGameLibrary);

            foreach (RadioButton but in buttons)
            {
                //MessageBox.Show(but.Name);
            }

            // get the radio button that is checked
            RadioButton rtTarget = buttons
                .Where(r => r.IsChecked == true).Single();

            // get the showall button
            RadioButton btnShowAll = buttons
                .Where(r => r.Name == "btnShowAll").Single();


            // Clear all settings
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Text = "1337";

            // restore settings
            rtTarget.IsChecked = true;
            tbFilterDatagrid.Text = tbText;

        }

    }
}
