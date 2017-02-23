using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes.VisualHandlers
{
    public class ControlsVisualHandler
    {
        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllDynamicControlPanels { get; set; }
        public WrapPanel ControlWrapPanel { get; set; }
        public GlobalSettings GS { get; set; }

        // constructor
        public ControlsVisualHandler()
        {
            // get an instance of the MainWindow
            MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get 'Base Configuration' button
            //RadioButton btnConfigBase = (RadioButton)MWindow.FindName("btnConfigBase");

            // get settings panel
            WrapPanel wpControlLeftPane = (WrapPanel)MWindow.FindName("wpControlLeftPane");

            // get config wrappanel
            ControlWrapPanel = (WrapPanel)MWindow.FindName("ControlWrapPanel");

            // get all filter buttons from the Configs page
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpControlLeftPane);//.Where(r => r.GroupName == "grpSettings").ToList();
            FilterButtons = _filterButtons;

            // setting grid containing right hand content
            Grid controlGrid = (Grid)MWindow.FindName("controlGrid");

            // get all right hand config panels that are dynamic
            var AllControlPanels = UIHandler.GetLogicalChildCollection<Border>(controlGrid).ToList();
            AllDynamicControlPanels = (from a in AllControlPanels
                                      where (a.Name.Length > 1 && !a.Name.Contains("NONDYNAMIC"))
                                      select a).ToList();

            GS = GlobalSettings.GetGlobals();
        }

        // methods
               
        public void SetFilter()
        {
            // Activate default settings (system wide)


            // get active button
            RadioButton _activeRadio = FilterButtons.Where(a => a.IsChecked == true).Single();

            string name = _activeRadio.Name.Replace("btnControl", "").ToLower();



            // set system specific config panels as visible
            foreach (Border border in AllDynamicControlPanels)
            {
                string brdName = StripTrailingNumerals(border.Name.ToLower().Replace("controlbrd", ""));
                if (brdName == name || (name == "base"))
                {
                    border.Visibility = Visibility.Visible;
                }
                else
                {
                    border.Visibility = Visibility.Collapsed;
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
            
            ControlsVisualHandler cvh = new ControlsVisualHandler();

            // Show/Hide system-specific panels
            cvh.SetFilter();
            



        }
    }
}
