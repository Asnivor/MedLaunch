using MedLaunch.Classes.Controls.VirtualDevices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.Controls;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for TestUC.xaml
    /// </summary>
    public partial class MdCtrl : UserControl
    {
        public MainWindow mw { get; set; }

        public MdCtrl()
        {
            InitializeComponent();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        private async void btnControlsConfigure_Click(object sender, RoutedEventArgs e)
        {
            // get button name
            Button button = (Button)sender;
            string name = button.Name;

            // remove beginning and end
            name = name.Replace("btn", "").Replace("Configure", "");
            
            // get the relevant combox
            ComboBox cb = (ComboBox)this.FindName("cmb" + name);

            // get the virtual port number
            string selectedString = cb.SelectionBoxItem.ToString();
            int portNum = Convert.ToInt32(selectedString.Replace("Virtual Port ", ""));

            // get mednafen config version
            bool isNewConfig = Classes.VersionChecker.Instance.IsNewConfig;

            IDeviceDefinition dev;

            if (isNewConfig)
            {
                dev = new DeviceDefinition();

                switch (name)
                {
                    case "Md3ButtonGamepad":
                        dev = Md.ThreeButton(portNum);
                        break;
                    case "Md6ButtonGamepad":
                        dev = Md.SixButton(portNum);
                        break;
                    case "Md2ButtonGamepad":
                        dev = Md.TwoButton(portNum);
                        break;
                    case "MdMegaMouse":
                        dev = Md.MegaMouse(portNum);
                        break;
                    default:
                        return;
                }
            }
            else
            {
                dev = new DeviceDefinitionLegacy();

                switch (name)
                {
                    case "Md3ButtonGamepad":
                        dev = Md_Legacy.ThreeButton(portNum);
                        break;
                    case "Md6ButtonGamepad":
                        dev = Md_Legacy.SixButton(portNum);
                        break;
                    case "Md2ButtonGamepad":
                        dev = Md_Legacy.TwoButton(portNum);
                        break;
                    case "MdMegaMouse":
                        dev = Md_Legacy.MegaMouse(portNum);
                        break;
                    default:
                        Classes.MessagePopper.PopControllerTargetingIssue();
                        return;
                }
            }
            
            mw.ControllerDefinition = dev;

            // launch controller configuration window
            if (isNewConfig)
            {
                Grid RootGrid = (Grid)mw.FindName("RootGrid");
                await mw.ShowChildWindowAsync(new ConfigureController()
                {
                    IsModal = true,
                    AllowMove = false,
                    Title = "Controller Configuration",
                    CloseOnOverlay = false,
                    ShowCloseButton = false,
                    CloseByEscape = false
                }, RootGrid);
            }
            else
            {
                Grid RootGrid = (Grid)mw.FindName("RootGrid");
                await mw.ShowChildWindowAsync(new ConfigureControllerLegacy()
                {
                    IsModal = true,
                    AllowMove = false,
                    Title = "Controller Configuration",
                    CloseOnOverlay = false,
                    ShowCloseButton = false,
                    CloseByEscape = false
                }, RootGrid);
            }
        }
    }
}
