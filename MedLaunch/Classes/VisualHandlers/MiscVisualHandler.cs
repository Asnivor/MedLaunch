using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Models;
using MedLaunch.Classes;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;

namespace MedLaunch.Classes.VisualHandlers
{
    public class MiscVisualHandler
    {

        public static void RefreshCoreVisibilities()
        {
            // get mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get the current settings
            GlobalSettings gs = GlobalSettings.GetGlobals();

            // create an array of values
            bool[] b = new bool[]
            {
                gs.coreVis1,
                gs.coreVis2,
                gs.coreVis3,
                gs.coreVis4,
                gs.coreVis5,
                gs.coreVis6,
                gs.coreVis7,
                gs.coreVis8,
                gs.coreVis9,
                gs.coreVis10,
                gs.coreVis11,
                gs.coreVis12,
                gs.coreVis13,
                gs.coreVis14,
                gs.coreVis15,
                gs.coreVis16,
                gs.coreVis17,
                gs.coreVis18
            };

            // iterate through each setting and set visibilities
            for (int i = 1; i <= b.Length; i++)
            {
                // games library filters
                
                string sysCode = GSystem.GetSystemCode(i);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                RadioButton rb = (RadioButton)mw.FindName("btn" + textInfo.ToTitleCase(sysCode));
                if (rb != null)
                {
                    if (b[i - 1] == true)
                        rb.Visibility = Visibility.Visible;
                    else
                        rb.Visibility = Visibility.Collapsed;
                }
                
                           

            }

        }

        
    }
}
