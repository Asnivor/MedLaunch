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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ConfigureController.xaml
    /// </summary>
    public partial class ConfigureController : ChildWindow
    {
        public DeviceDefinition ControllerDefinition { get; set; }

        public ConfigureController()
        {
            InitializeComponent();

            // get the mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // set the controller definition from mainwindow
            if (mw.ControllerDefinition == null)
                this.Close();

            ControllerDefinition = mw.ControllerDefinition;

            string vPortStr = "";
            if (ControllerDefinition.VirtualPort > 0)
                vPortStr = " - Virtual Port: " + ControllerDefinition.VirtualPort;


            // set the title
            titleTextBlock.Text = "Configure " + ControllerDefinition.DeviceName + vPortStr;

            // loop through maplist and populate the dynamic data grid row by row
            for (int i = 0; i < ControllerDefinition.MapList.Count; i++)
            {
                // description
                Label desc = new Label();
                desc.Content = ControllerDefinition.MapList[i].Description;

                ToolTip tt = new System.Windows.Controls.ToolTip();
                tt.Content = ControllerDefinition.MapList[i].MednafenCommand;
                desc.ToolTip = tt;

                // saved config info
                TextBox configInfo = new TextBox();
                configInfo.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand); //ControllerDefinition.MapList[i].Config;

                // set rows and columns
                desc.SetValue(Grid.ColumnProperty, 0);
                desc.SetValue(Grid.RowProperty, i);

                configInfo.SetValue(Grid.ColumnProperty, 2);
                configInfo.SetValue(Grid.RowProperty, i);

                // add controls to grid
                DynamicDataGrid.Children.Add(desc);
                DynamicDataGrid.Children.Add(configInfo);
            }

            // populate the image
            BitmapImage b = GetImage(ControllerDefinition.DeviceName);
            if (b != null)
            {
                img.Source = b;
            }
        }

        private static string GetConfigItem(string configItemName)
        {
            List<string> cfgs = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen-09x.cfg").ToList();

            string line = (from a in cfgs
                           where a.StartsWith(configItemName)
                           select a).Last();

            if (line == null)
                return "";

            // split the line by spaces
            string[] arr = line.Split(' ');

            // build the new string without the command
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < arr.Length; i++)
            {
                sb.Append(arr[i]);
                sb.Append(" ");
            }

            return sb.ToString().TrimEnd();
        }

        private static BitmapImage GetImage(string controllerName)
        {
            string imgName = "";

            switch (controllerName)
            {
                case "NES GamePad":
                    imgName = "nes-controller.png";
                    break;
                case "GB GamePad":
                    imgName = "gb_controller.png";
                    break;
                case "GBA GamePad":
                    imgName = "gba-controller.png";
                    break;
                case "LYNX GamePad":
                    imgName = "lynx_controller.png";
                    break;
                case "NGP GamePad":
                    imgName = "ngp-controller.png";
                    break;
                case "GG GamePad":
                    imgName = "gg-controller.png";
                    break;
                case "MD GamePad (3-Button)":
                    imgName = "md-controller-3button.png";
                    break;
                case "MD GamePad (6-Button)":
                    imgName = "md-controller-6button.png";
                    break;

                default:
                    return null;
            }

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"/Data/Graphics/Controllers/" + imgName, UriKind.RelativeOrAbsolute);
            logo.EndInit();

            return logo;
        }


        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
