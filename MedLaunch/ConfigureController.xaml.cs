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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ConfigureController.xaml
    /// </summary>
    public partial class ConfigureController : ChildWindow
    {
        public DeviceDefinition ControllerDefinition { get; set; }
        public MainWindow mw { get; set; }
        public IntPtr hWnd { get; set; }

        public ConfigureController()
        {
            InitializeComponent();

            // get the mainwindow
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

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
                configInfo.Name = TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                configInfo.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand); //ControllerDefinition.MapList[i].Config;
                configInfo.GotFocus += TextBox_GotFocus;
                configInfo.LostFocus += TextBox_LostFocus;
                configInfo.IsReadOnly = true;

                // configure button
                Button btn = new Button();
                btn.Content = "Configure";
                btn.Name = "btn" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                btn.Click += btnConfigureSingle_Click;

                // set rows and columns
                desc.SetValue(Grid.ColumnProperty, 0);
                desc.SetValue(Grid.RowProperty, i);

                configInfo.SetValue(Grid.ColumnProperty, 1);
                configInfo.SetValue(Grid.RowProperty, i);

                btn.SetValue(Grid.ColumnProperty, 2);
                btn.SetValue(Grid.RowProperty, i);

                // add controls to grid
                DynamicDataGrid.Children.Add(desc);
                DynamicDataGrid.Children.Add(configInfo);
                DynamicDataGrid.Children.Add(btn);
            }

            // populate the image
            BitmapImage b = GetImage(ControllerDefinition.DeviceName);
            if (b != null)
            {
                img.Source = b;
            }
        }

        private async void btnConfigureSingle_Click(object sender, RoutedEventArgs e)
        {            
            Button b = sender as Button;
            //MessageBox.Show("You clicked: " + TranslateControlName(b.Name).TrimStart('b').TrimStart('t').TrimStart('n'));
            string btnName = b.Name;
            string tbName = b.Name.Replace("btnControl", "Control");

            // get all the textboxes on the page
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);
            List<TextBox> tbs = ui.TextBoxes;

            TextBox tb = (from a in tbs
                          where a.Name == btnName.Replace("btnControl", "Control")
                          select a).FirstOrDefault();

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Configure Control", "Determining Paths and Counting Files...", settings: mySettings);

            controller.SetCancelable(false);
            controller.SetIndeterminate();

            await Task.Run(() =>
            {
                //Classes.Controls.Input input = new Classes.Controls.Input();
                Thread.Sleep(3000);
                
            });

            await controller.CloseAsync();

        }

        public static string TranslateConfigName(string configCommand)
        {
            string r1 = configCommand.Replace(".", "__");
            return "ControlCfg_" + r1;
        }

        public static string TranslateControlName(string controlName)
        {
            return controlName.Replace("__", ".").Replace("ControlCfg_", "");
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
                case "SNES GamePad":
                case "SNES (faust) GamePad":
                    imgName = "snes-controller.png";
                    break;
                case "SMS GamePad":
                    imgName = "sms-controller.png";
                    break;
                case "PCE GamePad":
                case "PCE (fast) GamePad":
                case "PCFX GamePad":
                    imgName = "pce-controller.png";
                    break;
                case "VB GamePad":
                    imgName = "vb-controller.png";
                    break;
                case "WSWAN GamePad":
                    imgName = "wswan-controller.png";
                    break;
                case "SS Digital GamePad":
                    imgName = "ss-controller.png";
                    break;
                case "SS 3D Control Pad":
                    imgName = "ss3d-controller.png";
                    break;
                case "PSX Digital GamePad":
                    imgName = "psx-controller.png";
                    break;
                case "PSX Dual Analog GamePad":
                case "PSX DualShock GamePad":
                    imgName = "psx-dualanalogcontroller.png";
                    break;
                case "PSX DancePad":
                    imgName = "psx-dancepad.png";
                    break;
                case "PSX NeGcon Controller":
                    imgName = "psx-negcon.png";
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
            // get all the textboxes on the page
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);
            List<TextBox> tbs = ui.TextBoxes;

            // iterate through each textbox
            foreach (TextBox tb in tbs)
            {
                string content = tb.Text;
                string name = tb.Name;
                string configName = TranslateControlName(name);
            }
            

            this.Close();
        }

        private void TextBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //MessageBox.Show("got focus");
        }

        private void TextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //MessageBox.Show("lost focus");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Background = Brushes.Red;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Background = Brushes.Transparent;
        }
    }

    
}
