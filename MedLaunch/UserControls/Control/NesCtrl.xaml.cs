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
    public partial class NesCtrl : UserControl
    {
        public MainWindow mw { get; set; }

        public NesCtrl()
        {
            InitializeComponent();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // hide controls from old config style

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
            //ComboBoxItem typeItem = (ComboBoxItem)cb.SelectedItem;
            string selectedString = cb.SelectionBoxItem.ToString();
            int portNum;

            // FXP we will call port 666
            if (selectedString == "Famicon Expansion Port")
                portNum = 666;
            else            
                portNum = Convert.ToInt32(selectedString.Replace("Virtual Port ", ""));

            // get mednafen config version
            bool isNewConfig = Classes.VersionChecker.Instance.IsNewConfig;

            IDeviceDefinition dev;

            if (isNewConfig)
            {
                dev = new DeviceDefinition();

                switch (name)
                {
                    case "NesGamepad":
                        dev = Nes.GamePad(portNum);
                        break;
                    case "NesZapper":
                        dev = Nes.Zapper(portNum);
                        break;
                    case "NesPowerPadA":
                        dev = Nes.PowerPadA(portNum);
                        break;
                    case "NesPowerPadB":
                        dev = Nes.PowerPadB(portNum);
                        break;
                    case "NesArkanoidPaddle":
                        dev = Nes.ArkanoidPaddle(portNum);
                        break;
                    case "NesFamilyKeyboard":
                        dev = Nes.FamilyKeyboard(portNum);
                        break;
                    case "NesFamilyTrainerA":
                        dev = Nes.FamilyTrainerA(portNum);
                        break;
                    case "NesFamilyTrainerB":
                        dev = Nes.FamilyTrainerB(portNum);
                        break;
                    case "NesHypershot":
                        dev = Nes.HyperShot(portNum);
                        break;
                    case "NesMahjong":
                        dev = Nes.Mahjong(portNum);
                        break;
                    case "NesOekakids":
                        dev = Nes.OekaKids(portNum);
                        break;
                    case "NesPartyTap":
                        dev = Nes.PartyTap(portNum);
                        break;
                    case "NesSpaceShadow":
                        dev = Nes.SpaceShadow(portNum);
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
                    case "NesGamepad":
                        dev = Nes_Legacy.GamePad(portNum);
                        break;
                    case "NesZapper":
                        dev = Nes_Legacy.Zapper(portNum);
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
