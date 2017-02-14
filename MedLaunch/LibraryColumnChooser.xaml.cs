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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for LibraryColumnChooser.xaml
    /// </summary>
    public partial class LibraryColumnChooser : ChildWindow
    {
        public LibraryColumnChooser()
        {
            InitializeComponent();

            // load all settings from database
            GlobalSettings gs = GlobalSettings.GetGlobals();
            string[] fArr = GlobalSettings.ReturnFilterArray();

            // get all checkboxes from the window
            List<CheckBox> cbs = UIHandler.GetLogicalChildCollection<CheckBox>(ugrid_columns);

            // Iterate through each checkbox row by row
            for (int i = 1; i <= 19; i++)
            {
                var list = cbs.Where(a => a.Name.StartsWith("c" + i.ToString() + "_")).ToArray();
                int count = list.Length;

                // get the hex string for this row
                string hex = fArr[i - 1];
                int value = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

                // iterate through each checkbox in this row working backwards and populate
                for (int b = count; b > 0; b--)
                {
                    CheckBox c = list.Where(a => a.Name.Contains("_" + b.ToString())).First();
                    int testValue = 1 * Convert.ToInt32(Math.Pow(2, Convert.ToDouble(b - 1)));
                    int val = value / testValue;
                    if (value / testValue == 1)
                    {
                        // checkbox should be checked
                        c.IsChecked = true;
                        // set remainder
                        value = value % testValue;
                    }
                    else
                    {
                        c.IsChecked = false;
                    }
                }                
            }            
        }

        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            // get all checkboxes from the window
            List<CheckBox> cbs = UIHandler.GetLogicalChildCollection<CheckBox>(ugrid_columns);

            List<string> results = new List<string>();

            // Iterate through each checkbox row by row
            for (int i = 1; i <= 19; i++)
            {
                var list = cbs.Where(a => a.Name.StartsWith("c" + i.ToString() + "_")).ToArray();
                int count = list.Length;

                int total = 0;

                // iterate through each checkbox in this row and add values together
                int calc = 0;
                for (int b = 0; b < count; b++)
                {
                    CheckBox c = list.Where(a => a.Name.Contains("_" + (b + 1).ToString())).First();

                    calc = Convert.ToInt32(Math.Pow(2.00, Convert.ToDouble(b)));

                    if (c.IsChecked == true)
                    {
                        total += calc;
                    }
                }

                // convert total to hex string
                string hexValue = total.ToString("X4");
                results.Add(hexValue);
            }

            // write results to database
            GlobalSettings.WriteFilterAray(results.ToArray());

            this.Close();
        }
    }
}
