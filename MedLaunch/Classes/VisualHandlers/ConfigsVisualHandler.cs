using MahApps.Metro.Controls;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MedLaunch.Classes
{
    public class ConfigsVisualHandler
    {
        // Constructor
        public ConfigsVisualHandler()
        {
            // get an instance of the MainWindow
            MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get 'Base Configuration' button
            RadioButton btnConfigBase = (RadioButton)MWindow.FindName("btnConfigBase");

            // get settings panel
            WrapPanel wpConfigLeftPane = (WrapPanel)MWindow.FindName("wpConfigLeftPane");

            // get config wrappanel
            ConfigWrapPanel = (WrapPanel)MWindow.FindName("ConfigWrapPanel");

            // get all filter buttons from the Configs page
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpConfigLeftPane);//.Where(r => r.GroupName == "grpSettings").ToList();
            FilterButtons = _filterButtons;

            // setting grid containing right hand content
            Grid configGrid = (Grid)MWindow.FindName("configGrid");

            // get all right hand config panels that are dynamic
            var AllConfigPanels = UIHandler.GetLogicalChildCollection<Border>(configGrid).ToList();
            AllDynamicConfigPanels = (from a in AllConfigPanels
                                      where (a.Name.Length > 1 && !a.Name.Contains("NONDYNAMIC"))
                                      select a).ToList();

            GS = GlobalSettings.GetGlobals();
        }

        // Methods

        public void ActivateEnabledSystems()
        {
            foreach (RadioButton rb in FilterButtons)
            {
                // if button is enabled in the database then make sure it is enabled in the UI

                string name = rb.Name.Replace("btnConfig", "");
                if (name == "Base")
                    continue;

                int configId = ConfigBaseSettings.GetConfigIdFromButtonName(name);

                // check whether this system is enabled
                if (ConfigBaseSettings.GetConfig(configId).isEnabled == true)
                {
                    rb.IsEnabled = true;
                }
            }
        }

        private static string StripTrailingNumerals(string input)
        {
            //string input = b.Name;
            string pattern = @"\d+$";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            string output = rgx.Replace(input, replacement);
            return output;
        }

        public static void ButtonClick()
        {
            
            ConfigsVisualHandler cvh = new ConfigsVisualHandler();
            
            // Show/Hide system-specific panels
            cvh.SetFilter();
        }
        

        public void SetFilter()
        {
            // Activate default settings (system wide)


            // get active button
            RadioButton _activeRadio = FilterButtons.Where(a => a.IsChecked == true).Single();

            string name = _activeRadio.Name.Replace("btnConfig", "").ToLower();


            // set system specific config panels as visible
            foreach (Border border in AllDynamicConfigPanels)
            {
                string brdName = StripTrailingNumerals(border.Name.ToLower().Replace("brdspecific", ""));
                if (brdName == name || (name == "base" && GS.showAllBaseSettings == true))
                {
                    border.Visibility = Visibility.Visible;
                }
                else
                {
                    border.Visibility = Visibility.Collapsed;
                }
            }            
        }

        public static void HideControls(WrapPanel wp, int configId)
        {
            // get a class object with all child controls
            UIHandler ui = UIHandler.GetChildren(wp);

            ConfigsVisualHandler cv = new ConfigsVisualHandler();
            Border b = (Border)cv.MWindow.FindName("brdNONDYNAMICvfilters");

            // get a list of system codes
            List<string> codes = (from a in GSystem.GetSystems()
                                 select a.systemCode.ToString().ToLower()).ToList();

            // iterate through each ui element and collapse the ones that are not needed for system specific settings
            if (configId != 2000000000)
            {
                b.Visibility = Visibility.Collapsed;
                foreach (Button x in ui.Buttons)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (CheckBox x in ui.CheckBoxes)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (ComboBox x in ui.ComboBoxes)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (NumericUpDown x in ui.NumericUpDowns)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (RadioButton x in ui.RadioButtons)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (Slider x in ui.Sliders)
                {
                    if (x.Name.StartsWith("cfg___"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
                foreach (TextBox x in ui.TextBoxes)
                {
                    if (x.Name.StartsWith("cfg___") || x.Name.StartsWith("tb_Generic__"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }                
                foreach (Label x in ui.Labels)
                {
                    if (x.Name.StartsWith("cfg___") || x.Name.StartsWith("lbl_cfg___") || x.Name.StartsWith("lbl_Generic__"))
                    {
                        x.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                GlobalSettings gs = GlobalSettings.GetGlobals();
                if (gs.showAllBaseSettings == true)
                {
                    b.Visibility = Visibility.Collapsed;
                }
                else
                {
                    b.Visibility = Visibility.Visible;
                    foreach (Button x in ui.Buttons)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (CheckBox x in ui.CheckBoxes)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (ComboBox x in ui.ComboBoxes)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (NumericUpDown x in ui.NumericUpDowns)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (RadioButton x in ui.RadioButtons)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (Slider x in ui.Sliders)
                    {
                        if (x.Name.StartsWith("cfg___"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (TextBox x in ui.TextBoxes)
                    {
                        if (x.Name.StartsWith("cfg___") || x.Name.StartsWith("tb_Generic__"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                    foreach (Label x in ui.Labels)
                    {
                        if (x.Name.StartsWith("cfg___") || x.Name.StartsWith("lbl_cfg___") || x.Name.StartsWith("lbl_Generic__"))
                        {
                            x.Visibility = Visibility.Visible;
                        }
                    }
                }                
            }            
        }

       

        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllDynamicConfigPanels { get; set; }
        public WrapPanel ConfigWrapPanel { get; set; }
        public GlobalSettings GS { get; set; }
    }
}
