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

                // hack for apple2 system
                if (brdName == "apple")
                {
                    brdName = "apple2";
                }

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
            //Border b = (Border)cv.MWindow.FindName("brdNONDYNAMICvfilters");

            // get a list of system codes
            List<string> codes = (from a in GSystem.GetSystems()
                                 select a.systemCode.ToString().ToLower()).ToList();

            // iterate through each ui element and collapse the ones that are not needed for system specific settings
            if (configId != 2000000000)
            {
                //b.Visibility = Visibility.Collapsed;
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
                    //b.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //b.Visibility = Visibility.Visible;
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

        /// <summary>
        /// Save settings based on settings group
        /// </summary>
        /// <param name="settingGroup"></param>
        public static void SaveSettings(SettingGroup settingGroup)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            switch (settingGroup)
            {
                case SettingGroup.BiosPaths:
                    ConfigBaseSettings.SaveBiosPaths();
                    break;

                case SettingGroup.GamePaths:
                    TextBox tbPathMednafen = (TextBox)mw.FindName("tbPathMednafen");
                    TextBox tbPathGb = (TextBox)mw.FindName("tbPathGb");
                    TextBox tbPathGba = (TextBox)mw.FindName("tbPathGba");
                    TextBox tbPathGg = (TextBox)mw.FindName("tbPathGg");
                    TextBox tbPathLynx = (TextBox)mw.FindName("tbPathLynx");
                    TextBox tbPathMd = (TextBox)mw.FindName("tbPathMd");
                    TextBox tbPathNes = (TextBox)mw.FindName("tbPathNes");
                    TextBox tbPathSnes = (TextBox)mw.FindName("tbPathSnes");
                    TextBox tbPathNgp = (TextBox)mw.FindName("tbPathNgp");
                    TextBox tbPathPce = (TextBox)mw.FindName("tbPathPce");
                    TextBox tbPathPcfx = (TextBox)mw.FindName("tbPathPcfx");
                    TextBox tbPathPsx = (TextBox)mw.FindName("tbPathPsx");
                    TextBox tbPathSs = (TextBox)mw.FindName("tbPathSs");
                    TextBox tbPathSms = (TextBox)mw.FindName("tbPathSms");
                    TextBox tbPathVb = (TextBox)mw.FindName("tbPathVb");
                    TextBox tbPathWswan = (TextBox)mw.FindName("tbPathWswan");
                    TextBox tbPathPceCd = (TextBox)mw.FindName("tbPathPceCd");
					TextBox tbPathApple2 = (TextBox)mw.FindName("tbPathApple2");

                    Paths.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathSms, tbPathVb, tbPathWswan, tbPathPsx, tbPathSs, tbPathPceCd, tbPathApple2);
                    break;

                case SettingGroup.GlobalSettings:
                    GlobalSettings gs = GlobalSettings.GetGlobals();

                    Slider slFanrtsPerHost = (Slider)mw.FindName("slFanrtsPerHost");
                    Slider slScreenshotsPerHost = (Slider)mw.FindName("slScreenshotsPerHost");
                    ComboBox comboImageTooltipSize = (ComboBox)mw.FindName("comboImageTooltipSize");
                    ComboBox cbFormatGameTitles = (ComboBox)mw.FindName("cbFormatGameTitles");

                    gs.maxFanarts = slFanrtsPerHost.Value;
                    gs.maxScreenshots = slScreenshotsPerHost.Value;
                    gs.imageToolTipPercentage = Convert.ToDouble(comboImageTooltipSize.SelectedValue, System.Globalization.CultureInfo.InvariantCulture);
                    gs.changeTitleCase = Convert.ToInt32(cbFormatGameTitles.SelectedValue);

                    GlobalSettings.SetGlobals(gs);
                    break;

                case SettingGroup.MednaNetSettings:
                    MednaNetSettings ms = MednaNetSettings.GetGlobals();

                    Slider slDiscordChatHistory = (Slider)mw.FindName("slDiscordChatHistory");
                    Slider slApiPollingFrequency = (Slider)mw.FindName("slApiPollingFrequency");

                    ms.ChatHistoryInMinutes = Convert.ToInt32(slDiscordChatHistory.Value, System.Globalization.CultureInfo.InvariantCulture);
                    ms.PollTimerIntervalInSeconds = Convert.ToInt32(slApiPollingFrequency.Value, System.Globalization.CultureInfo.InvariantCulture);

                    MednaNetSettings.SetGlobals(ms);
                    break;

                case SettingGroup.MednafenPaths:
                    ConfigBaseSettings.SaveMednafenPaths();
                    break;

                case SettingGroup.NetplaySettings:
                    TextBox tbNetplayNick = (TextBox)mw.FindName("tbNetplayNick");
                    Slider slLocalPlayersValue = (Slider)mw.FindName("slLocalPlayersValue");
                    Slider slConsoleLinesValue = (Slider)mw.FindName("slConsoleLinesValue");
                    Slider slConsoleScaleValue = (Slider)mw.FindName("slConsoleScaleValue");
                    RadioButton resOne = (RadioButton)mw.FindName("resOne");
                    RadioButton resTwo = (RadioButton)mw.FindName("resTwo");
                    RadioButton resThree = (RadioButton)mw.FindName("resThree");
                    RadioButton resFour = (RadioButton)mw.FindName("resFour");
                    RadioButton resFive = (RadioButton)mw.FindName("resFive");

                    ConfigNetplaySettings.SaveNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
                    break;

                case SettingGroup.ServerSettings:
                    TextBox tbServerDesc = (TextBox)mw.FindName("tbServerDesc");
                    TextBox tbHostname = (TextBox)mw.FindName("tbHostname");
                    Slider slServerPort = (Slider)mw.FindName("slServerPort");
                    TextBox tbPassword = (TextBox)mw.FindName("tbPassword");
                    TextBox tbGameKey = (TextBox)mw.FindName("tbGameKey");

                    ConfigServerSettings.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
                    break;

                default:
                    break;
            }
        }

       

        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllDynamicConfigPanels { get; set; }
        public WrapPanel ConfigWrapPanel { get; set; }
        public GlobalSettings GS { get; set; }
    }

    public enum SettingGroup
    {
        GamePaths,
        NetplaySettings,
        ServerSettings,
        MednafenPaths,
        BiosPaths,
        GlobalSettings,
        MednaNetSettings
    }
}
