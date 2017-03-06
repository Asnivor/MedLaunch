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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ConfigureController.xaml
    /// </summary>
    public partial class ConfigureController : ChildWindow
    {
        public DeviceDefinition ControllerDefinition { get; set; }
        public DeviceDefinition ControllerDefinitionWorking { get; set; }
        public MainWindow mw { get; set; }
        public IntPtr hWnd { get; set; }

        private TextBox _activeTB { get; set; }

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly List<string> _bindings = new List<string>();

        private string _wasPressed = string.Empty;

        public ConfigureController()
        {
            InitializeComponent();

            // get the mainwindow
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            
            //Input.Initialize(mw);

            // clear all queued presses
            Input.Instance.ClearEvents();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);


            _timer.Tick += Timer_Tick;

            

            
            //Input.Initialize(mw);

            // set the controller definition from mainwindow
            if (mw.ControllerDefinition == null)
                this.Close();

            ControllerDefinition = mw.ControllerDefinition;

            string vPortStr = "";
            if (ControllerDefinition.VirtualPort > 0)
                vPortStr = " - Virtual Port: " + ControllerDefinition.VirtualPort;


            // set the title
            titleTextBlock.Text = "Configure " + ControllerDefinition.DeviceName + vPortStr;

            // headers
            Label headDesc = new Label();
            headDesc.Content = "Binding";
            headDesc.SetValue(Grid.ColumnProperty, 0);
            headDesc.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headDesc);

            Label headConfig1 = new Label();
            headConfig1.Content = "Primary";
            headConfig1.SetValue(Grid.ColumnProperty, 1);
            headConfig1.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headConfig1);

            Label headConfig2 = new Label();
            headConfig2.Content = "Secondary";
            headConfig2.SetValue(Grid.ColumnProperty, 2);
            headConfig2.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headConfig2);

            Label headConfig3 = new Label();
            headConfig3.Content = "Tertiary";
            headConfig3.SetValue(Grid.ColumnProperty, 3);
            headConfig3.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headConfig3);

            Label headCustom = new Label();
            headCustom.Content = "Custom Insert";
            headCustom.SetValue(Grid.ColumnProperty, 4);
            headCustom.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headCustom);

            // loop through maplist and populate the dynamic data grid row by row
            for (int i = 0; i < ControllerDefinition.MapList.Count; i++)
            {
                // description
                Label desc = new Label();
                desc.Content = ControllerDefinition.MapList[i].Description;
                desc.SetValue(Grid.ColumnProperty, 0);
                desc.SetValue(Grid.RowProperty, i + 1);
                ToolTip tt = new System.Windows.Controls.ToolTip();
                tt.Content = ControllerDefinition.MapList[i].MednafenCommand;
                desc.ToolTip = tt;
                KeyboardNavigation.SetIsTabStop(desc, false);
                DynamicDataGrid.Children.Add(desc);


                // Config Primary               
                TextBox configInfo = new TextBox();
                configInfo.Name = "Primary_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                //configInfo.Text = KeyboardTranslation.SDLCodetoDx(GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Primary), KeyboardType.UK);  //ControllerDefinition.MapList[i].Config;
                configInfo.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Primary);  //ControllerDefinition.MapList[i].Config;
                configInfo.GotFocus += TextBox_GotFocus;
                configInfo.LostFocus += TextBox_LostFocus;
                configInfo.KeyDown += TextBox_KeyDownHandler;
                configInfo.IsReadOnly = true;
                configInfo.MinWidth = 100;
                configInfo.SetValue(Grid.ColumnProperty, 1);
                configInfo.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo, i + 1);
                DynamicDataGrid.Children.Add(configInfo);

                // Config Secondary               
                TextBox configInfo2 = new TextBox();
                configInfo2.Name = "Secondary_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                //configInfo2.Text = /*KeyboardTranslation.SDLCodetoDx(GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Secondary), KeyboardType.UK); //*/ControllerDefinition.MapList[i].Config;
                configInfo2.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Secondary);
                configInfo2.GotFocus += TextBox_GotFocus;
                configInfo2.LostFocus += TextBox_LostFocus;
                configInfo2.KeyDown += TextBox_KeyDownHandler;
                configInfo2.IsReadOnly = true;
                configInfo2.MinWidth = 100;
                configInfo2.SetValue(Grid.ColumnProperty, 2);
                configInfo2.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo2, i + 1 + ControllerDefinition.MapList.Count);
                DynamicDataGrid.Children.Add(configInfo2);

                // Config Tertiary              
                TextBox configInfo3 = new TextBox();
                configInfo3.Name = "Tertiary_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                //configInfo3.Text = /*KeyboardTranslation.SDLCodetoDx(GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Tertiary), KeyboardType.UK); //*/ControllerDefinition.MapList[i].Config;
                configInfo3.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Tertiary);
                configInfo3.GotFocus += TextBox_GotFocus;
                configInfo3.LostFocus += TextBox_LostFocus;
                configInfo3.KeyDown += TextBox_KeyDownHandler;
                configInfo3.IsReadOnly = true;
                configInfo3.MinWidth = 100;
                configInfo3.SetValue(Grid.ColumnProperty, 3);
                configInfo3.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo3, i + 1 + (ControllerDefinition.MapList.Count * 2));
                DynamicDataGrid.Children.Add(configInfo3);

                // configure button
                Button btn = new Button();
                btn.Content = "Configure";
                btn.Name = "btn" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                btn.Click += btnConfigureSingle_Click;

                btn.SetValue(Grid.ColumnProperty, 4);
                btn.SetValue(Grid.RowProperty, i + 1);

                KeyboardNavigation.SetIsTabStop(btn, false);

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
            string r1 = configCommand.Replace(".", "__").Replace("-", "ddddd");
            return "ControlCfg_" + r1;
        }

        public static string TranslateControlName(string controlName)
        {
            return controlName.Replace("__", ".").Replace("ddddd", "-").Replace("ControlCfg_", "");
        }

        private static string GetConfigItem(string configItemName, ConfigOrder configOrder)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // load cfg string
            string cfg = (from a in mw.ControllerDefinition.MapList
                         where a.MednafenCommand == configItemName
                         select a.Config).FirstOrDefault();

            if (cfg == null || cfg == "" || cfg == " ")
                return "";

            // check whether ~ symbol exists (ie. multiple disperate command bindings for config command
            if (cfg.Contains("~"))
            {
                // split based on ~
                string[] ar = cfg.Split('~');

                int count = ar.Length;
                for (int i = 0; i < count; i++)
                {
                    if (configOrder == ConfigOrder.Primary && i == 0)
                        return ar[i];

                    if (configOrder == ConfigOrder.Secondary && i == 1)
                        return ar[i];

                    if (configOrder == ConfigOrder.Tertiary && i == 2)
                        return ar[i];
                }
            }

            else
            {
                if (configOrder == ConfigOrder.Primary)
                {
                    return cfg;
                }

            }

            return "";
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

                case "SS Steering Wheel":
                    imgName = "ss-wheel.png";
                    break;
                case "SS Dual Mission Stick":
                    imgName = "ss-mission.png";
                    break;
                case "SS Mission Stick":
                    imgName = "ss-dualmission.png";
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
            // reset all textboxes - first get all of the tbs on the page
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);
            foreach (TextBox tb in ui.TextBoxes)
            {
                if (tb.Name.Contains("Primary_"))
                {
                    string prim = TranslateControlName(tb.Name.Replace("Primary_", ""));
                    tb.Text = GetConfigItem(prim, ConfigOrder.Primary);
                }
                if (tb.Name.Contains("Secondary_"))
                {
                    string sec = TranslateControlName(tb.Name.Replace("Secondary_", ""));
                    tb.Text = GetConfigItem(sec, ConfigOrder.Secondary);
                }
                if (tb.Name.Contains("Tertiary_"))
                {
                    string ter = TranslateControlName(tb.Name.Replace("Tertiary_", ""));
                    tb.Text = GetConfigItem(ter, ConfigOrder.Tertiary);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            /*
            if (e.Key == Key.Escape)
                e.Handled = true;
                */
        }

        private void TextBox_KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter & (sender as TextBox).AcceptsReturn == false)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Return & (sender as TextBox).AcceptsReturn == false)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab & (sender as TextBox).AcceptsReturn == false)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Escape)
            {
                // clear textbox
                EraseMappings();
            }

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            // get all the textboxes on the page
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);

            // all textboxes
            List<TextBox> tbs = ui.TextBoxes;

            // just the primaries
            List<TextBox> prims = tbs.Where(a => a.Name.Contains("Primary_Control")).ToList();

            List<Mapping> maps = new List<Mapping>();

            // iterate through each primary textbox
            foreach (TextBox tb in prims)
            {
                string content = tb.Text;
                string name = tb.Name;
                string configName = name.Replace("Primary_Control", "");

                // check whether secondary and teritary also exist
                string sec = "";
                string ter = "";

                TextBox tSec = tbs.Where(a => a.Name.Contains("Secondary_Control" + configName)).First();
                TextBox tTer = tbs.Where(a => a.Name.Contains("Tertiary_Control" + configName)).First();

                sec = tSec.Text;
                ter = tTer.Text;

                // build config string
                string configData = ConfigLineBuilder(content, sec, ter);

                // populate controller definition with new value
                ControllerDefinitionWorking = ControllerDefinition;
                
                Mapping map = new Mapping();

                map = ControllerDefinition.MapList.Where(a => a.MednafenCommand == TranslateControlName(configName.Replace("Cfg_", "").Replace("cfg_", ""))).First();
                map.Config = configData;
                maps.Add(map);                
            }

            // now write all data to mednafen config
            bool write = DeviceDefinition.WriteDefinitionToConfigFile(maps);

            if (!write)
                MessageBox.Show("There was a problem reading from/writing to the mednafen config file", "Possible IO Error", MessageBoxButton.OK, MessageBoxImage.Error);

            this.Close();
        }

        public static string ConfigLineBuilder(string pri, string sec, string ter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(pri);
            sb.Append("~");
            sb.Append(sec);
            sb.Append("~");
            sb.Append(ter);

            return sb.ToString().ToLower()
                .Replace("~~~", "~")
                .Replace("~~", "")
                .TrimStart('~')
                .TrimEnd('~');
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            _activeTB = tb;
            tb.Background = Brushes.Red;

            _timer.Start();

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            _activeTB = null;
            tb.Background = Brushes.Transparent;

            _timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReadKeys();
        }

        private void Increment()
        {
            if (_activeTB == null)
                return;

            _activeTB.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void Decrement()
        {
            if (_activeTB == null)
                return;

            _activeTB.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
        }

        private void EraseMappings()
        {
            _activeTB.Text = "";
        }

        private void ReadKeys()
        {
            Input.Instance.Update();
            var bindingStr = Input.Instance.GetNextBindEvent();
            if (!string.IsNullOrEmpty(_wasPressed) && bindingStr == _wasPressed)
            {
                return;
            }

            if (bindingStr != null)
            {
                _wasPressed = bindingStr;
                
                if (bindingStr == "Escape")
                {
                    EraseMappings();
                    Increment();
                    return;
                }

                // ignore Alt+f4
                if (bindingStr == "Alt+F4")
                {
                    return;
                }

                /*
                //ignore special bindings
                foreach (var spec in SpecialBindings)
                    if (spec.BindingName == bindingStr)
                        return;

                if (!IsDuplicate(bindingStr))
                {
                    if (AutoTab)
                    {
                        ClearBindings();
                    }

                    _bindings.Add(bindingStr);
                }
                */


                //UpdateLabel();
                _activeTB.Text = KeyboardTranslation.DXtoSDLCode(bindingStr, KeyboardType.UK); // bindingStr;
                Increment();
               

            }

        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Input.Instance.Dispose();
        }

        void MoveToNextUIElement(KeyEventArgs e)
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }

    }

    public enum ConfigOrder
    {
        Primary,
        Secondary,
        Tertiary
    }
    
}
