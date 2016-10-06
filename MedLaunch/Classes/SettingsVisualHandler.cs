using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public class SettingsVisualHandler
    {
        // Constructor
        public SettingsVisualHandler()
        {
            // get an instance of the MainWindow
            MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get 'show all settings' button and click it
            RadioButton btnAllSettings = (RadioButton)MWindow.FindName("btnAllSettings");
            //btnAllSettings.IsChecked = true;

            // get settings grid
            WrapPanel wpSettingsLeftPane = (WrapPanel)MWindow.FindName("wpSettingsLeftPane");

            // get all filter buttons from the settings page
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpSettingsLeftPane);//.Where(r => r.GroupName == "grpSettings").ToList();
            FilterButtons = _filterButtons;

            // setting grid containing right hand content
            Grid SettingGrid = (Grid)MWindow.FindName("SettingGrid");

            // get all settings panels
            //AllSettingPanels = UIHandler.GetLogicalChildCollection<Border>("SettingGrid").ToList();

            AllSettingPanels = UIHandler.GetLogicalChildCollection<Border>(SettingGrid).ToList();


            // iterate through each panel and match the border x:name to the class property name
            foreach (Border b in AllSettingPanels)
            {
                // remove any trailing numerals from the control name
                string name = StripTrailingNumerals(b.Name);                

                // if the control name matches a property name in this class, add it to that list
                PropertyInfo property = typeof(SettingsVisualHandler).GetProperty(name);
                if (property == null)
                {
                    // no property matched
                    continue;
                }

                // add the border control to the correct List
                switch (property.Name)
                {
                    case "MednafenPaths":
                        MednafenPaths.Add(b);
                        break;
                    case "GameFolders":
                        MednafenPaths.Add(b);
                        break;
                    case "SystemBios":
                        MednafenPaths.Add(b);
                        break;
                    case "Netplay":
                        MednafenPaths.Add(b);
                        break;
                    case "Emulator":
                        MednafenPaths.Add(b);
                        break;
                    case "MedLaunch":
                        MednafenPaths.Add(b);
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
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
            SettingsVisualHandler svh = new SettingsVisualHandler();
            svh.SetFilter();
        }

        public void SetFilter()
        {
            // get active button
            RadioButton _activeRadio = FilterButtons.Where(a => a.IsChecked == true).Single();

            string name = _activeRadio.Name.Replace("btn", "");

            // get all borders that have names that match the above string
            string brdName = "brd" + name;
            List<Border> _borders = (from b in AllSettingPanels
                                    where b.Name.Contains(brdName)
                                    select b).ToList();
            
            if (name == "brdAllSettings")
            {
                // all settings - show all
                foreach (Border b in AllSettingPanels)
                {
                    b.Visibility = Visibility.Visible;
                }
            }
            else
            {
                // active the border boxes relating to this filter button and deactivate the rest
                foreach (Border b in _borders)
                {
                    // set visibility visible
                    b.Visibility = Visibility.Visible;
                    // remove from AllSettingsPanels
                    AllSettingPanels.Remove(b);
                }
                foreach (Border b in AllSettingPanels)
                {
                    // set visibility collapsed
                    b.Visibility = Visibility.Collapsed;
                }
            }
        }


        // Properties
        private MainWindow MWindow { get; set; }
        private List<RadioButton> FilterButtons { get; set; }
        private List<Border> AllSettingPanels { get; set; }
        public List<Border> MednafenPaths { get; set; }
        public List<Border> GameFolders { get; set; }
        public List<Border> SystemBios { get; set; }
        public List<Border> Netplay { get; set; }
        public List<Border> Emulator { get; set; }
        public List<Border> MedLaunch { get; set; }
    }
}
