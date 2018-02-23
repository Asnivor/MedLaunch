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
    public partial class ConfigureController : ChildWindow
    {
        public IDeviceDefinition ControllerDefinition { get; set; }
        public IDeviceDefinition ControllerDefinitionWorking { get; set; }
        public MainWindow mw { get; set; }
        public IntPtr hWnd { get; set; }
        public ContextMenu TBCM { get; set; }

        public List<CustomInsert> CustomInsertList { get; set; }

        private TextBox _activeTB { get; set; }

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly List<string> _bindings = new List<string>();

        private string _wasPressed = string.Empty;

        public ConfigureController()
        {
            InitializeComponent();

            // textbox context menu
            TBCM = new ContextMenu();

            // context headings
            Label heading = new Label();
            heading.Content = "Pre-defined Macros";
            TBCM.Items.Add(heading);
            TBCM.Items.Add(new Separator());

            MenuItem HeaderMouse = new MenuItem { Header = "Mouse" };
            TBCM.Items.Add(HeaderMouse);

            // Mouse Buttons
            MenuItem LMB = new MenuItem { Header = "Insert LeftMouseButton (button_left)", Name = "menuLMB" };
            LMB.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(LMB);
            MenuItem RMB = new MenuItem { Header = "Insert RightMouseButton (button_right)", Name = "menuRMB" };
            RMB.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(RMB);
            MenuItem MMB = new MenuItem { Header = "Insert MiddleMouseButton (button_middle)", Name = "menuMMB" };
            MMB.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MMB);
            
            MenuItem MSU = new MenuItem { Header = "Insert MouseScrollUp", Name = "menuMSU" };
            MSU.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSU);
            MenuItem MSD = new MenuItem { Header = "Insert MouseScrollDown", Name = "menuMSD" };
            MSD.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSD);

            
            MenuItem MSB3 = new MenuItem { Header = "Insert MouseButton4 (button_3)", Name = "menuMSB3" };
            MSB3.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSB3);
            MenuItem MSB4 = new MenuItem { Header = "Insert MouseButton5 (button_4)", Name = "menuMSB4" };
            MSB4.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSB4);
            MenuItem MSB5 = new MenuItem { Header = "Insert MouseButton6 (button_5)", Name = "menuMSB5" };
            MSB5.Click += new RoutedEventHandler(Macro_Click);            
            HeaderMouse.Items.Add(MSB5);
            
            HeaderMouse.Items.Add(new Separator());

            // Mouse Axis
            MenuItem MSXAXIS = new MenuItem { Header = "Insert Mouse X-Axis (cursor_x-+)", Name = "menuMSXAXIS" };
            MSXAXIS.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSXAXIS);
            MenuItem MSYAXIS = new MenuItem { Header = "Insert Mouse Y-Axis (cursor_y-+)", Name = "menuMSYAXIS" };
            MSYAXIS.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSYAXIS);

            MenuItem MSYAXISMOUP = new MenuItem { Header = "Insert Mouse Motion UP (rel_y-)", Name = "menuMSYAXISMOUP" };
            MSYAXISMOUP.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSYAXISMOUP);

            MenuItem MSYAXISMODOWN = new MenuItem { Header = "Insert Mouse Motion DOWN (rel_y+)", Name = "menuMSYAXISMODOWN" };
            MSYAXISMODOWN.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSYAXISMODOWN);

            MenuItem MSYAXISMOLEFT = new MenuItem { Header = "Insert Mouse Motion LEFT (rel_x-)", Name = "menuMSYAXISMOLEFT" };
            MSYAXISMOLEFT.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSYAXISMOLEFT);

            MenuItem MSYAXISMORIGHT = new MenuItem { Header = "Insert Mouse Motion RIGHT (rel_x+)", Name = "menuMSYAXISMORIGHT" };
            MSYAXISMORIGHT.Click += new RoutedEventHandler(Macro_Click);
            HeaderMouse.Items.Add(MSYAXISMORIGHT);

            // get the mainwindow
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            
            //Input.Initialize(mw);

            // clear all queued presses
            Input.Instance.ClearEvents();

            // event to handle the escape key
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);


            _timer.Tick += Timer_Tick;

            // set the controller definition from mainwindow
            if (mw.ControllerDefinition == null)
                this.Close();

            ControllerDefinition = mw.ControllerDefinition;

            // build the port string
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
            headConfig2.SetValue(Grid.ColumnProperty, 3);
            headConfig2.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headConfig2);

            Label headConfig3 = new Label();
            headConfig3.Content = "Tertiary";
            headConfig3.SetValue(Grid.ColumnProperty, 5);
            headConfig3.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headConfig3);

            /*
            Label headCustom = new Label();
            headCustom.Content = "Custom Insert";
            headCustom.SetValue(Grid.ColumnProperty, 4);
            headCustom.SetValue(Grid.RowProperty, 0);
            DynamicDataGrid.Children.Add(headCustom);
            */

            BuildDynamicGrid();

            // populate the image
            BitmapImage b = GetImage(ControllerDefinition.DeviceName);
            if (b != null)
            {
                img.Source = b;
            }

            // set childwindow size - this should be a little less than the actual window size
            this.ChildWindowHeight = mw.ActualHeight - 100;

            // now set the dynamic data scrollbar max height
            scrData.MaxHeight = this.ChildWindowHeight - 230;

            // setup the working definition
            ControllerDefinitionWorking = ((DeviceDefinition)ControllerDefinition as DeviceDefinition).CloneJson<DeviceDefinition>();
            /*
            ControllerDefinitionWorking.CommandStart = ControllerDefinition.CommandStart;
            ControllerDefinitionWorking.DeviceName = ControllerDefinition.DeviceName;
            ControllerDefinitionWorking.MapList = ControllerDefinition.MapList;
            ControllerDefinitionWorking.VirtualPort = ControllerDefinition.VirtualPort;
            */

        }

        /// <summary>
        /// Builds (or refreshes) the dynamic grid from the original Constroller Definition
        /// </summary>
        public void BuildDynamicGrid()
        {
            // remove current items
            DynamicDataGrid.Children.Clear();

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
                //configInfo.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Primary);  //ControllerDefinition.MapList[i].Config;

                string primTxt = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Primary);  //ControllerDefinition.MapList[i].Config;
                configInfo.Text = ConvertText(primTxt, ConversionOrder.Load);

                configInfo.GotFocus += TextBox_GotFocus;
                configInfo.LostFocus += TextBox_LostFocus;
                configInfo.KeyDown += TextBox_KeyDownHandler;
                configInfo.IsReadOnly = true;
                configInfo.MinWidth = 100;
                configInfo.SetValue(Grid.ColumnProperty, 1);
                configInfo.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo, i + 1);
                configInfo.ContextMenu = TBCM;
                configInfo.MouseEnter += tb_MouseEnter;
                configInfo.TextChanged += Data_TextChanged;

                DynamicDataGrid.Children.Add(configInfo);


                // Config Secondary               
                TextBox configInfo2 = new TextBox();
                configInfo2.Name = "Secondary_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                //configInfo2.Text = /*KeyboardTranslation.SDLCodetoDx(GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Secondary), KeyboardType.UK); //*/ControllerDefinition.MapList[i].Config;
                //configInfo2.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Secondary);

                string secTxt = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Secondary);  //ControllerDefinition.MapList[i].Config;
                configInfo2.Text = ConvertText(secTxt, ConversionOrder.Load);

                configInfo2.GotFocus += TextBox_GotFocus;
                configInfo2.LostFocus += TextBox_LostFocus;
                configInfo2.KeyDown += TextBox_KeyDownHandler;
                configInfo2.IsReadOnly = true;
                configInfo2.MinWidth = 100;
                configInfo2.SetValue(Grid.ColumnProperty, 3);
                configInfo2.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo2, i + 1 + ControllerDefinition.MapList.Count);
                configInfo2.ContextMenu = TBCM;
                configInfo2.MouseEnter += tb_MouseEnter;
                configInfo2.TextChanged += Data_TextChanged;
                DynamicDataGrid.Children.Add(configInfo2);


                // Config Tertiary              
                TextBox configInfo3 = new TextBox();
                configInfo3.Name = "Tertiary_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                //configInfo3.Text = /*KeyboardTranslation.SDLCodetoDx(GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Tertiary), KeyboardType.UK); //*/ControllerDefinition.MapList[i].Config;
                //configInfo3.Text = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Tertiary);

                string terTxt = GetConfigItem(ControllerDefinition.MapList[i].MednafenCommand, ConfigOrder.Tertiary);  //ControllerDefinition.MapList[i].Config;
                configInfo3.Text = ConvertText(terTxt, ConversionOrder.Load);

                configInfo3.GotFocus += TextBox_GotFocus;
                configInfo3.LostFocus += TextBox_LostFocus;
                configInfo3.KeyDown += TextBox_KeyDownHandler;
                configInfo3.IsReadOnly = true;
                configInfo3.MinWidth = 100;
                configInfo3.SetValue(Grid.ColumnProperty, 5);
                configInfo3.SetValue(Grid.RowProperty, i + 1);
                KeyboardNavigation.SetTabIndex(configInfo3, i + 1 + (ControllerDefinition.MapList.Count * 2));
                configInfo3.ContextMenu = TBCM;
                configInfo3.MouseEnter += tb_MouseEnter;
                configInfo3.TextChanged += Data_TextChanged;
                DynamicDataGrid.Children.Add(configInfo3);


                // primary logic button
                Button btnLogic1 = new Button();
                btnLogic1.Content = "OR";
                btnLogic1.Name = "btnLogic1_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                btnLogic1.Click += SetLogicButton_Click;

                btnLogic1.SetValue(Grid.ColumnProperty, 2);
                btnLogic1.SetValue(Grid.RowProperty, i + 1);

                try
                {
                    switch (ControllerDefinition.MapList[i].Primary.LogicString)
                    {
                        case "||":
                        default: btnLogic1.Content = "OR"; break;
                        case "&&": btnLogic1.Content = "AND"; break;
                        case "&!": btnLogic1.Content = "ANOT"; break;
                    }
                }
                catch (Exception)
                {
                    btnLogic1.Content = "OR";
                }

                DynamicDataGrid.Children.Add(btnLogic1);

                // secondary logic button
                Button btnLogic2 = new Button();
                btnLogic2.Content = "OR";
                btnLogic2.Name = "btnLogic2_" + TranslateConfigName(ControllerDefinition.MapList[i].MednafenCommand);
                btnLogic2.Click += SetLogicButton_Click;

                btnLogic2.SetValue(Grid.ColumnProperty, 4);
                btnLogic2.SetValue(Grid.RowProperty, i + 1);

                try
                {
                    switch (ControllerDefinition.MapList[i].Secondary.LogicString)
                    {
                        case "||":
                        default: btnLogic2.Content = "OR"; break;
                        case "&&": btnLogic2.Content = "AND"; break;
                        case "&!": btnLogic2.Content = "ANOT"; break;
                    }
                }
                catch (Exception)
                {
                    btnLogic1.Content = "OR";
                }

                DynamicDataGrid.Children.Add(btnLogic2);

            }
        }

        /// <summary>
        /// Called every time a textbox content changes
        /// This should update the working copy of the controller definition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string tbName = tb.Name;

            // get the command string
            string cmd = tbName.Split(new string[] { "ControlCfg_" }, StringSplitOptions.None)[1].Replace("__", ".");

            string text = tb.Text;
            if (text.Contains("box cannot be empty"))
                return; 

            // update that command
            Mapping map = ControllerDefinitionWorking.MapList.Where(a => a.MednafenCommand == cmd).FirstOrDefault();

            if (map == null)
            {
                MessageBox.Show("command was not found");
                return;
            }

            string converted = string.Empty;

            if (tbName.StartsWith("Primary_"))
            {
                converted = ConvertText(tb.Text, ConversionOrder.Save);
                if (map.Primary == null)
                    map.Primary = new Mapping();

                if (tb.Text.Trim() == "")
                {
                    map.Primary = null;
                }
                else
                {
                    map.Primary = CreateMapFromString(map.Primary, converted);
                }                
            }

            if (tbName.StartsWith("Secondary_"))
            {
                converted = ConvertText(tb.Text, ConversionOrder.Save);
                if (map.Secondary == null)
                    map.Secondary = new Mapping();

                if (tb.Text.Trim() == "")
                {
                    map.Secondary = null;
                }
                else
                {
                    map.Secondary = CreateMapFromString(map.Secondary, converted);
                }
            }

            if (tbName.StartsWith("Tertiary_"))
            {
                converted = ConvertText(tb.Text, ConversionOrder.Save);
                if (map.Tertiary == null)
                    map.Tertiary = new Mapping();

                if (tb.Text.Trim() == "")
                {
                    map.Tertiary = null;
                }
                else
                {
                    map.Tertiary = CreateMapFromString(map.Tertiary, converted);
                }
            }

            string test = "0";
        }

        /// <summary>
        /// Processes an input string that comes from a textbox
        /// This expects a mednafen formated config string
        /// </summary>
        /// <param name="map"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private Mapping CreateMapFromString(Mapping map, string input)
        {
            if (input.Trim() == "")
                return map;

            string[] arr = input.Split(' ');

            switch (arr[0])
            {
                case "keyboard":
                    map.DeviceType = DeviceType.Keyboard;
                    map.DeviceID = "0x0";
                    map.Scale = null;
                    map.Config = arr[2];
                    break;
                case "mouse":
                    map.DeviceType = DeviceType.Mouse;
                    map.DeviceID = "0x0";
                    map.Config = arr[2];
                    if (arr.Length == 4)
                        map.Scale = arr[3];
                    break;
                case "joystick":
                    map.DeviceType = DeviceType.Joystick;
                    map.DeviceID = arr[1];
                    map.Config = arr[2];
                    if (arr.Length == 4)
                        map.Scale = arr[3];
                    break;
            }

            return map;
        }

        /// <summary>
        /// When a logic button is clicked cycle through the available boolean operators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLogicButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> ops = new List<string>
            {
                "OR",
                "AND",
                "ANOT"
            };

            Button b = sender as Button;
            //MessageBox.Show("You clicked: " + TranslateControlName(b.Name).TrimStart('b').TrimStart('t').TrimStart('n'));
            string btnName = b.Name;

            for (int i = 0; i < ops.Count(); i++)
            {
                if (ops[i] == b.Content.ToString())
                {
                    if (i == ops.Count() - 1)
                    {
                        // this is the last entry
                        b.Content = ops[0];
                        break;
                    }
                    else
                    {
                        b.Content = ops[i + 1];
                        break;
                    }
                }
            }

            // now update the working definitions

            // get the command string
            string cmd = btnName.Split(new string[] { "ControlCfg_" }, StringSplitOptions.None)[1].Replace("__", ".");

            // update that command
            Mapping map = ControllerDefinitionWorking.MapList.Where(a => a.MednafenCommand == cmd).FirstOrDefault();

            if (map == null)
            {
                MessageBox.Show("command was not found");
                return;
            }

            // get order identifier
            string order = btnName.Split(new string[] { "_ControlCfg_" }, StringSplitOptions.None)[0];

            switch (order)
            {
                case "btnLogic1":
                    if (b.Content.ToString() == "OR")
                        map.Primary.LogicString = "||";
                    if (b.Content.ToString() == "AND")
                        map.Primary.LogicString = "&&";
                    if (b.Content.ToString() == "ANOT")
                        map.Primary.LogicString = "&!";
                    break;
                case "btnLogic2":
                    if (b.Content.ToString() == "OR")
                        map.Secondary.LogicString = "||";
                    if (b.Content.ToString() == "AND")
                        map.Secondary.LogicString = "&&";
                    if (b.Content.ToString() == "ANOT")
                        map.Secondary.LogicString = "&!";
                    break;
            }
        }
        /*
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

        }*/

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

            // get the map
            var map = (from a in mw.ControllerDefinition.MapList
                       where a.MednafenCommand == configItemName
                       select a).FirstOrDefault();

            if (map == null || map.Primary == null)
                return "";

            switch (configOrder)
            {
                case ConfigOrder.Primary:
                    if (map.Primary != null)
                        return DeviceDefinition.ProcessBlock(map.Primary);
                    break;
                case ConfigOrder.Secondary:
                    if (map.Secondary != null)
                        return DeviceDefinition.ProcessBlock(map.Secondary);
                    break;
                case ConfigOrder.Tertiary:
                    if (map.Tertiary != null)
                        return DeviceDefinition.ProcessBlock(map.Tertiary);
                    break;
            }

            return "";

            /*

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

            */
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
                case "SS Light Gun":
                    imgName = "ss_gun.png";
                    break;
                case "SNES Super Scope":
                    imgName = "snes-superscope.png";
                    break;
                case "PSX GunCon":
                    imgName = "psx-gcon.png";
                    break;
                case "PSX Konami Justifier":
                    imgName = "psx-justifier.png";
                    break;
                case "MD Mega Mouse":
                    imgName = "md-controller-megamouse.png";
                    break;
                case "PCE Mouse":
                case "PCE (fast) Mouse":
                    imgName = "pce-mouse.png";
                    break;
                case "PCFX Mouse":
                    imgName = "pcfx-mouse.png";
                    break;
                case "PSX Mouse":
                    imgName = "psx-mouse.png";
                    break;
                case "SNES Mouse":
                    imgName = "snes-mouse.png";
                    break;
                case "SS Mouse":
                    imgName = "ss-mouse.png";
                    break;
                case "NES Zapper":
                    imgName = "nes-zapper.png";
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

        /// <summary>
        /// Resets the form state back to how it was (pre-any changes)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ControllerDefinitionWorking = ((DeviceDefinition)ControllerDefinition as DeviceDefinition).CloneJson<DeviceDefinition>();
            BuildDynamicGrid();   
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

        /// <summary>
        /// Parses all the form data into a List<Mapping> object and saves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            /*
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
                string content = ConvertText(tb.Text, ConversionOrder.Save);
                string name = tb.Name;
                string configName = name.Replace("Primary_Control", "");

                // check whether secondary and teritary also exist
                string sec = "";
                string ter = "";

                TextBox tSec = tbs.Where(a => a.Name.Contains("Secondary_Control" + configName)).First();
                TextBox tTer = tbs.Where(a => a.Name.Contains("Tertiary_Control" + configName)).First();

                sec = ConvertText(tSec.Text, ConversionOrder.Save);
                ter = ConvertText(tTer.Text, ConversionOrder.Save); 

                /*
                // convert non-mednafen things
                if (!content.StartsWith("joystick ") && !content.StartsWith("mouse "))
                {
                    // this is a translated keyboard entry
                    content = KeyboardTranslation.DXtoSDLCode(content, KeyboardType.UK);
                }
                if (!sec.StartsWith("joystick ") && !sec.StartsWith("mouse "))
                {
                    // this is a translated keyboard entry
                    sec = KeyboardTranslation.DXtoSDLCode(sec, KeyboardType.UK);
                }
                if (!ter.StartsWith("joystick ") && !ter.StartsWith("mouse "))
                {
                    // this is a translated keyboard entry
                    ter = KeyboardTranslation.DXtoSDLCode(ter, KeyboardType.UK);
                }
                */
            /*
            // build config string
            string configData = ConfigLineBuilder(content, sec, ter);

            // populate controller definition with new value
            ControllerDefinitionWorking = ControllerDefinition;

            Mapping map = new Mapping();

            map = ControllerDefinition.MapList.Where(a => a.MednafenCommand == TranslateControlName(configName.Replace("Cfg_", "").Replace("cfg_", ""))).First();
            map.Config = configData;
            maps.Add(map);                
        }

    */

            // process all of the logic buttons
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);

            // get all logic buttons
            List<Button> tbs = ui.Buttons.Where(a => a.Name.Contains("btnLogic")).ToList();

            // iterate through each item in the maplist
            for (int i = 0; i < ControllerDefinitionWorking.MapList.Count(); i++)
            {
                string ctrlStr = ControllerDefinitionWorking.MapList[i].MednafenCommand.Replace(".", "__");

                var line = tbs.Where(a => a.Name.Contains(ctrlStr)).ToList();

                Button prim = line.Where(a => a.Name.Contains("ogic1")).FirstOrDefault();
                Button sec = line.Where(a => a.Name.Contains("ogic2")).FirstOrDefault();

                try
                {
                    switch (prim.Content.ToString())
                    {
                        case "OR": ControllerDefinitionWorking.MapList[i].Primary.LogicString = "||"; break;
                        case "AND": ControllerDefinitionWorking.MapList[i].Primary.LogicString = "&&"; break;
                        case "ANOT": ControllerDefinitionWorking.MapList[i].Primary.LogicString = "&!"; break;
                    }
                    switch (sec.Content.ToString())
                    {
                        case "OR": ControllerDefinitionWorking.MapList[i].Secondary.LogicString = "||"; break;
                        case "AND": ControllerDefinitionWorking.MapList[i].Secondary.LogicString = "&&"; break;
                        case "ANOT": ControllerDefinitionWorking.MapList[i].Secondary.LogicString = "&!"; break;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            // now write all data to mednafen config
            bool write = DeviceDefinition.WriteDefinitionToConfigFile(ControllerDefinitionWorking.MapList);

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
            string name = tb.Name;
            _activeTB = tb;

            // get all the textboxes on the page
            UIHandler ui = UIHandler.GetChildren(DynamicDataGrid);

            // all textboxes
            List<TextBox> tbs = ui.TextBoxes;

            // check whether previous tbs have been populated - if not then accept no input
            if (tb.Name.Contains("Primary_Control"))
            {
                tb.Background = Brushes.Red;
                _timer.Start();
            }
            if (tb.Name.Contains("Secondary_Control"))
            {
                // check whether primary is populated
                string primName = name.Replace("Secondary_", "Primary_");
                TextBox tbPrim = tbs.Where(a => a.Name == name.Replace("Secondary_", "Primary_")).FirstOrDefault();
                if (tbPrim.Text == "")
                {
                    tb.Text = "<-- Primary box cannot be empty";
                    tb.Background = Brushes.Orange;
                    return;
                }
                tb.Background = Brushes.Red;
                _timer.Start();
            }
            if (tb.Name.Contains("Tertiary_Control"))
            {
                // check whether secondary is populated
                TextBox tbSec = tbs.Where(a => a.Name == name.Replace("Tertiary_", "Secondary_")).FirstOrDefault(); 
                if (tbSec.Text == "")
                {
                    tb.Text = "<-- Secondary box cannot be empty";
                    tb.Background = Brushes.Orange;
                    return;
                }
                tb.Background = Brushes.Red;
                _timer.Start();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            _activeTB = null;
            tb.Background = Brushes.Transparent;

            if (tb.Text == "<-- Primary box cannot be empty" || tb.Text == "<-- Secondary box cannot be empty")
            {
                tb.Text = "";
                return;
            }

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
                _activeTB.Text = bindingStr; // KeyboardTranslation.DXtoSDLCode(bindingStr, KeyboardType.UK); // bindingStr;
                Increment();
               

            }

        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Input.Instance.Dispose();
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

        public void Macro_Click(Object sender, RoutedEventArgs e)
        {
            // get menuitem
            MenuItem mnu = sender as MenuItem;
            string menuName = mnu.Name;

            // get parent textbox
            TextBox tb = null;

            if (mnu == null)
                return;

            MenuItem par = ((MenuItem)mnu.Parent);
            tb = ((ContextMenu)par.Parent).PlacementTarget as TextBox;
            string tbName = tb.Name;

            // set the textbox .text
            switch (menuName)
            {
                case "menuLMB":
                    tb.Text = ConvertText("mouse 0x0 button_left", ConversionOrder.Load);
                    break;

                case "menuMMB":
                    tb.Text = ConvertText("mouse 0x0 button_middle", ConversionOrder.Load);
                    break;

                case "menuRMB":
                    tb.Text = ConvertText("mouse 0x0 button_right", ConversionOrder.Load);
                    break;                

                case "menuMSU":
                    tb.Text = ConvertText("mouse 0000000000000000 00000003", ConversionOrder.Load);
                    break;

                case "menuMSD":
                    tb.Text = ConvertText("mouse 0000000000000000 00000004", ConversionOrder.Load);
                    break;

                case "menuMSB3":
                    tb.Text = ConvertText("mouse 0x0 button_3", ConversionOrder.Load);
                    break;

                case "menuMSB4":
                    tb.Text = ConvertText("mouse 0x0 button_4", ConversionOrder.Load);
                    break;

                case "menuMSB5":
                    tb.Text = ConvertText("mouse 0x0 button_5", ConversionOrder.Load);
                    break;

                case "menuMSXAXIS":
                    tb.Text = ConvertText("mouse 0x0 cursor_x-+", ConversionOrder.Load);
                    break;

                case "menuMSYAXIS":
                    tb.Text = ConvertText("mouse 0x0 cursor_y-+", ConversionOrder.Load);
                    break;

                case "menuMSYAXISMOUP":
                    tb.Text = ConvertText("mouse 0x0 rel_y-", ConversionOrder.Load);
                    break;

                case "menuMSYAXISMODOWN":
                    tb.Text = ConvertText("mouse 0x0 rel_y+", ConversionOrder.Load);
                    break;

                case "menuMSYAXISMOLEFT":
                    tb.Text = ConvertText("mouse 0x0 rel_x-", ConversionOrder.Load);
                    break;

                case "menuMSYAXISMORIGHT":
                    tb.Text = ConvertText("mouse 0x0 rel_x+", ConversionOrder.Load);
                    break;
            }
        }

        /// <summary>
        /// handles conversions between how control bindings are displayed versus how mednafen stores them in its config file
        /// </summary>
        /// <param name="input"></param>
        /// <param name="conversionOrder"></param>
        /// <returns></returns>
        public static string ConvertText(string input, ConversionOrder conversionOrder)
        {
            // create the output string
            string output = input;

            switch (conversionOrder)
            {
                // Text is being loaded FROM mednafen config
                case ConversionOrder.Load:
                    if (input.StartsWith("keyboard 0x0 "))
                    {
                        // keyboard binding
                        output = KeyboardTranslationSDL2.SDLCodetoDx(input, KeyboardType.UK);
                    }
                    if (input.StartsWith("mouse 0x0 "))
                    {
                        // mouse binding
                        if (input.Contains("mouse 0x0 button_left")) { output = "LeftMouseButton"; }
                        if (input.Contains("mouse 0x0 button_middle")) { output = "MiddleMouseButton"; }
                        if (input.Contains("mouse 0x0 button_right")) { output = "RightMouseButton"; }

                        if (input.Contains("mouse 0000000000000000 00000003")) { output = "MouseScrollWheelUp"; }
                        if (input.Contains("mouse 0000000000000000 00000004")) { output = "MouseScrollWheelDown"; }
                        if (input.Contains("mouse 0x0 button_3")) { output = "MouseButton4"; }
                        if (input.Contains("mouse 0x0 button_4")) { output = "MouseButton5"; }
                        if (input.Contains("mouse 0x0 button_5")) { output = "MouseButton6"; }

                        if (input.Contains("mouse 0x0 cursor_x-+")) { output = "MouseX-Axis (Cursor)"; }
                        if (input.Contains("mouse 0x0 cursor_y-+")) { output = "MouseY-Axis (Cursor)"; }
                        if (input.Contains("mouse 0x0 rel_y-")) { output = "Mouse RelativeY-"; }
                        if (input.Contains("mouse 0x0 rel_y+")) { output = "Mouse RelativeY+"; }
                        if (input.Contains("mouse 0x0 rel_x-")) { output = "Mouse RelativeX-"; }
                        if (input.Contains("mouse 0x0 rel_x+")) { output = "Mouse RelativeX+"; }
                    }

                    if (input.StartsWith("joystick "))
                    {
                        // joystick binding - not currently used
                    }
                    break;

                // Text is being saved TO mednafen config
                case ConversionOrder.Save:

                    if (input == "LeftMouseButton") { output = "mouse 0x0 button_left"; return output; }
                    if (input == "MiddleMouseButton") { output = "mouse 0x0 button_middle"; return output; }
                    if (input == "RightMouseButton") { output = "mouse 0x0 button_right"; return output; }

                    if (input == "MouseScrollWheelUp") { output = "mouse 0000000000000000 00000003"; return output; }
                    if (input == "MouseScrollWheelDown") { output = "mouse 0000000000000000 00000004"; return output; }
                    if (input == "MouseButton3") { output = "mouse 0000000000000000 00000005"; return output; }
                    if (input == "MouseButton4") { output = "mouse 0000000000000000 00000006"; return output; }
                    if (input == "MouseButton5") { output = "mouse 0000000000000000 00000007"; return output; }
                    if (input == "MouseX-Axis") { output = "mouse 0000000000000000 00008000"; return output; }
                    if (input == "MouseY-Axis") { output = "mouse 0000000000000000 00008001"; return output; }

                    if (input == "MouseX-Axis (Cursor)") { output = "mouse 0x0 cursor_x-+"; return output; }
                    if (input == "MouseY-Axis (Cursor)") { output = "mouse 0x0 cursor_y-+"; return output; }
                    if (input == "Mouse RelativeY-") { output = "mouse 0x0 rel_y-"; return output; }
                    if (input == "Mouse RelativeY+") { output = "mouse 0x0 rel_y+"; return output; }
                    if (input == "Mouse RelativeX-") { output = "mouse 0x0 rel_x-"; return output; }
                    if (input == "Mouse RelativeX+") { output = "mouse 0x0 rel_x+"; return output; }

                    if (!input.StartsWith("mouse ") && !input.StartsWith("joystick "))
                    {
                        // assume keyboard
                        output = KeyboardTranslationSDL2.DXtoSDLCode(input, KeyboardType.UK);
                    }
                    break;
            }

            return output;
        }

        /// <summary>
        /// gets fired everytime mouse enters a textbox
        /// used to update tooltip with mednafen converted string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string text = tb.Text;

            if (text == null || text.Trim() == "")
                return;

            string ttText = ConvertText(text, ConversionOrder.Save);

            // create tooltip based on the conversion
            ToolTip tt = new System.Windows.Controls.ToolTip();
            tt.Content = ttText;
            tb.ToolTip = tt;
        }
    }

    public enum ConfigOrder
    {
        Primary,
        Secondary,
        Tertiary
    }

    public enum ConversionOrder
    {
        Load,
        Save
    }

    public class CustomInsert
    {
        public string Title { get; set; }
        public string Command { get; set; }
    }
    
}
