using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            // get all filter buttons from the Configs page
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpConfigLeftPane);//.Where(r => r.GroupName == "grpSettings").ToList();
            FilterButtons = _filterButtons;

            // setting grid containing right hand content
            Grid ConfigGrid = (Grid)MWindow.FindName("ConfigGrid");

            // get all right hand config panels
            AllConfigPanels = UIHandler.GetLogicalChildCollection<Border>(ConfigGrid).ToList();
        }

        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllConfigPanels { get; set; }
    }
}
