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

                // games library actual filtering
                //todo

                // config filters
                RadioButton configRb = (RadioButton)mw.FindName("btnConfig" + textInfo.ToTitleCase(sysCode));
                if (configRb != null)
                {
                    if (b[i - 1] == true)
                        configRb.Visibility = Visibility.Visible;
                    else
                        configRb.Visibility = Visibility.Collapsed;
                }
                
                // settings filters (game paths)
                Button pathBtn = (Button)mw.FindName("btnPath" + textInfo.ToTitleCase(sysCode));
                TextBox pathTb = (TextBox)mw.FindName("tbPath" + textInfo.ToTitleCase(sysCode));
                if (pathBtn != null)
                {
                    if (b[i - 1] == true)
                    {
                        pathBtn.Visibility = Visibility.Visible;
                        pathTb.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        pathBtn.Visibility = Visibility.Collapsed;
                        pathTb.Visibility = Visibility.Collapsed;
                    }                        
                }

                // controls filters
                // config filters
                RadioButton controlRb = (RadioButton)mw.FindName("btnControl" + textInfo.ToTitleCase(sysCode));
                if (controlRb != null)
                {
                    if (b[i - 1] == true)
                        controlRb.Visibility = Visibility.Visible;
                    else
                        controlRb.Visibility = Visibility.Collapsed;
                }
            }

            /* get selected filter buttons - if they are invisible then select a different one */

            // games library filters
            WrapPanel wpGamesList = (WrapPanel)mw.FindName("wpGamesList");
            List<RadioButton> _filterButtonsLib = UIHandler.GetLogicalChildCollection<RadioButton>(wpGamesList);
            RadioButton btnShowAll = (RadioButton)mw.FindName("btnShowAll");
            // get selected library filter button
            var gSel = _filterButtonsLib.Where(a => a.IsChecked == true);
            if (gSel.Count() > 0)
            {
                RadioButton gSelBut = gSel.FirstOrDefault();
                if (gSelBut.Visibility == Visibility.Collapsed)
                {
                    // selected button is invisible - select the showall filter button
                    btnShowAll.IsChecked = true;
                }
            }
            else
            {
                // no button is selected
                btnShowAll.IsChecked = true;
            }

            // configs
            // handle fast and faust
            RadioButton btnConfigSnes_Faust = (RadioButton)mw.FindName("btnConfigSnes_Faust");
            RadioButton btnConfigPce_Fast = (RadioButton)mw.FindName("btnConfigPce_Fast");

            RadioButton btnConfigSnes = (RadioButton)mw.FindName("btnConfigSnes");
            RadioButton btnConfigPce = (RadioButton)mw.FindName("btnConfigPce");

            

            if (btnConfigSnes.Visibility == Visibility.Collapsed)
            {
                btnConfigSnes_Faust.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnConfigSnes_Faust.Visibility = Visibility.Visible;
            }

            if (b[17] == false)
            {
                if (btnConfigPce.Visibility == Visibility.Collapsed)
                {
                    btnConfigPce_Fast.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnConfigPce_Fast.Visibility = Visibility.Visible;
                }
            }
            else
            {
                btnConfigPce_Fast.Visibility = Visibility.Visible;
                btnConfigPce.Visibility = Visibility.Visible;
            }


            WrapPanel wpConfigLeftPane = (WrapPanel)mw.FindName("wpConfigLeftPane");
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpConfigLeftPane);
            // get selected config button
            var cSel = _filterButtons.Where(a => a.IsChecked == true);
            if (cSel.Count() > 0)
            {
                RadioButton cSelBut = cSel.FirstOrDefault();
                if (cSelBut.Visibility == Visibility.Collapsed)
                {
                    // selected button is invisible - find the next one that IS visible and click it
                    SetRadioButton(_filterButtons);
                }
            }
            else
            {
                // no button is selected
                SetRadioButton(_filterButtons);
            }

            // controls

            // handle fast and faust
            RadioButton btnControlSnes_Faust = (RadioButton)mw.FindName("btnControlSnes_Faust");
            RadioButton btnControlPce_Fast = (RadioButton)mw.FindName("btnControlPce_Fast");

            RadioButton btnControlSnes = (RadioButton)mw.FindName("btnControlSnes");
            RadioButton btnControlPce = (RadioButton)mw.FindName("btnControlPce");

            if (btnControlSnes.Visibility == Visibility.Collapsed)
            {
                btnControlSnes_Faust.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnControlSnes_Faust.Visibility = Visibility.Visible;
            }

            if (b[17] == false)
            {
                if (btnConfigPce.Visibility == Visibility.Collapsed)
                {
                    btnControlPce_Fast.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnControlPce_Fast.Visibility = Visibility.Visible;
                }
            }
            else
            {
                btnControlPce_Fast.Visibility = Visibility.Visible;
                btnControlPce.Visibility = Visibility.Visible;
            }

            if (btnControlPce.Visibility == Visibility.Collapsed)
            {
                btnControlPce_Fast.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnControlPce_Fast.Visibility = Visibility.Visible;
            }

            WrapPanel wpControlLeftPane = (WrapPanel)mw.FindName("wpControlLeftPane");
            List<RadioButton> _filterButtonsControl = UIHandler.GetLogicalChildCollection<RadioButton>(wpControlLeftPane);
            // get selected config button
            var contSel = _filterButtonsControl.Where(a => a.IsChecked == true);
            if (contSel.Count() > 0)
            {
                RadioButton contSelBut = contSel.FirstOrDefault();
                if (contSelBut.Visibility == Visibility.Collapsed)
                {
                    // selected button is invisible - find the next one that IS visible and click it
                    SetRadioButton(_filterButtonsControl);
                }
            }
            else
            {
                // no button is selected
                SetRadioButton(_filterButtonsControl);
            }


            // Completely hide whole panels if NO system is visible
            var b2 = b.Where(a => a == true).ToList();

            DataGrid dgGameList = (DataGrid)mw.FindName("dgGameList");
            WrapPanel ConfigWrapPanel = (WrapPanel)mw.FindName("ConfigWrapPanel");
            Grid controlGrid = (Grid)mw.FindName("controlGrid");

            if (b2.Count == 0)
            {                
                dgGameList.Visibility = Visibility.Collapsed;
                ConfigWrapPanel.Visibility = Visibility.Collapsed;
                controlGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                dgGameList.Visibility = Visibility.Visible;
                ConfigWrapPanel.Visibility = Visibility.Visible;
                controlGrid.Visibility = Visibility.Visible;
            }
        }

        public static void SetRadioButton(List<RadioButton> list)
        {
            foreach (RadioButton rb in list)
            {
                if (rb.Visibility == Visibility.Visible)
                {
                    rb.IsChecked = true;
                    rb.IsChecked = false;
                    rb.IsChecked = true;
                }
            }
        }

        
    }
}
