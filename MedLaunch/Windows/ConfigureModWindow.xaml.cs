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
using MedLaunch.Classes.Controls.VirtualDevices;
using System.IO;
using System.Windows.Interop;
using MedLaunch.Classes.Controls.InputManager;
using System.Threading;
using System.Windows.Threading;
using MedLaunch.Classes.Controls;
using MahApps.Metro.Controls;
using MedLaunch.Common;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ConfigureController.xaml
    /// </summary>
    public partial class ConfigureModWindow : ChildWindow
    {
        public ConfigureController ParentWindow { get; set; }
        public MainWindow MW { get; set; }
        public bool GFlagHidden { get; set; }
        public bool ApplyGFlag { get; set; }

        public ConfigureModWindow()
        {
            InitializeComponent();

            ApplyGFlag = false;

            // get the mainwindow
            MW = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get the parent window
            var childs = MW.RootGrid.Children; //Application.Current.Windows; //.OfType<ConfigureController>().ToList(); //.Where(a => a.GetType() == typeof(ConfigureModWindow)).FirstOrDefault();
            foreach (var c in childs)
            {
                if (c.GetType() == typeof(ConfigureController))
                {
                    ParentWindow = (ConfigureController)c;
                }
            }           

            double scale = 4096;
            string currVal = string.Empty;

            

            // set the slider value from the parent control
            switch (ParentWindow.tmpOrder)
            {
                case ConfigOrder.Primary:
                    currVal = ParentWindow.tmpMap.Primary.Scale;
                    if (ParentWindow.tmpMap.Primary.DeviceType == DeviceType.Joystick && ParentWindow.tmpMap.Primary.Config.Contains("abs_"))
                    {
                        ShowGFlag();
                    }
                        
                    else
                        HideGFlag();
                    break;
                case ConfigOrder.Secondary:
                    currVal = ParentWindow.tmpMap.Secondary.Scale;
                    if (ParentWindow.tmpMap.Secondary.DeviceType == DeviceType.Joystick && ParentWindow.tmpMap.Secondary.Config.Contains("abs_"))
                    {
                        ShowGFlag();
                    }                        
                    else
                        HideGFlag();
                    break;
                case ConfigOrder.Tertiary:
                    currVal = ParentWindow.tmpMap.Tertiary.Scale;
                    if (ParentWindow.tmpMap.Tertiary.DeviceType == DeviceType.Joystick && ParentWindow.tmpMap.Tertiary.Config.Contains("abs_"))
                    {
                        ShowGFlag();
                    }                        
                    else
                        HideGFlag();
                    break;
            }

            // attempt to convert the string value
            int conv;
            bool isParsed = int.TryParse(currVal, out conv);

            if (isParsed)
                scale = (double)conv;

            // init default
            slScaleFactor.Value = scale;
        }

        private void HideGFlag()
        {
            chkGFlag.Visibility = Visibility.Collapsed;
            gTextBlock.Visibility = Visibility.Collapsed;
            GFlagHidden = true;
        }

        private void ShowGFlag()
        {
            chkGFlag.Visibility = Visibility.Visible;
            gTextBlock.Visibility = Visibility.Visible;
            GFlagHidden = false;
        }

        /// <summary>
        /// Closes child window without saving changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Closes window saving changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            switch (ParentWindow.tmpOrder)
            {
                case ConfigOrder.Primary:
                    if (slScaleFactor.Value == 4096)
                    {
                        // default - remove the scale binding
                        ParentWindow.tmpMap.Primary.Scale = null;
                    }
                    else
                    {
                        ParentWindow.tmpMap.Primary.Scale = slScaleFactor.Value.ToString();
                    }

                    if (ApplyGFlag)
                    {
                        ParentWindow.tmpMap.Primary.Config = ApplyFlag(ParentWindow.tmpMap.Primary.Config);
                        ParentWindow.tmpMap.Primary.GFlag = "g";
                    }

                    break;
                case ConfigOrder.Secondary:
                    if (slScaleFactor.Value == 4096)
                    {
                        // default - remove the scale binding
                        ParentWindow.tmpMap.Secondary.Scale = null;
                    }
                    else
                    {
                        ParentWindow.tmpMap.Secondary.Scale = slScaleFactor.Value.ToString();
                    }

                    if (ApplyGFlag)
                    {
                        ParentWindow.tmpMap.Secondary.Config = ApplyFlag(ParentWindow.tmpMap.Secondary.Config);
                        ParentWindow.tmpMap.Secondary.GFlag = "g";
                    }

                    break;
                case ConfigOrder.Tertiary:
                    if (slScaleFactor.Value == 4096)
                    {
                        // default - remove the scale binding
                        ParentWindow.tmpMap.Tertiary.Scale = null;
                    }
                    else
                    {
                        ParentWindow.tmpMap.Tertiary.Scale = slScaleFactor.Value.ToString();
                    }

                    if (ApplyGFlag)
                    {
                        ParentWindow.tmpMap.Tertiary.Config = ApplyFlag(ParentWindow.tmpMap.Tertiary.Config);
                        ParentWindow.tmpMap.Tertiary.GFlag = "g";
                    }

                    break;
            }

            this.Close();
        }

        private string ApplyFlag(string config)
        {
            // create new instance
            string work = config.ToString();

            // existing layout will end with abs_n+, abs_n- or abs_n-+g
            // split based on abs_
            string[] arr = work.Split(new string[] { "abs_" }, StringSplitOptions.None);
            int len = arr.Length;

            string last = arr.Last();

            string firstpart = string.Empty;

            for (int i = 0; i < len - 1; i++)
            {
                firstpart += arr[i];
            }

            firstpart += "abs_";

            if (last.Contains("-+g"))
            {
                // flag is already applied
                return config;
            }

            if (last.EndsWith("-+"))
            {
                // axis is correct, just no modifier
                return firstpart + "g";
            }

            if (last.EndsWith("-"))
            {
                return firstpart + last.Replace("-", "-+g");
            }

            if (last.EndsWith("+"))
            {
                return firstpart + last.Replace("+", "-+g");
            }

            return config;
        }

        /// <summary>
        /// Sets the scale slider to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScaleDefault_Click(object sender, RoutedEventArgs e)
        {
            slScaleFactor.Value = 4096;
        }

        /// <summary>
        /// Signs that the G-Flag should be applied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGFlag_Click(object sender, RoutedEventArgs e)
        {
            ApplyGFlag = true;
        }
    }
}
