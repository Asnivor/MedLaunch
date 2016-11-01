using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
                                      where a.Name.Length > 1
                                      select a).ToList();
        }

        // Methods
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
                if (brdName == name)
                {
                    border.Visibility = Visibility.Visible;
                }
                else
                {
                    border.Visibility = Visibility.Collapsed;
                }
            }            
        }

        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllDynamicConfigPanels { get; set; }
        public WrapPanel ConfigWrapPanel { get; set; }
    }
}
