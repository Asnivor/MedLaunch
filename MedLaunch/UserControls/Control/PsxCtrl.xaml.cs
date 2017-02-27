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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for TestUC.xaml
    /// </summary>
    public partial class PsxCtrl : UserControl
    {
        public MainWindow mw { get; set; }

        public PsxCtrl()
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

            DeviceDefinition dev = new DeviceDefinition();

            switch (name)
            {
                case "PsxGamePad":
                    dev = Psx.DigitalGamePad(portNum);
                    break;
                case "PsxDualAnalogGamepad":
                    dev = Psx.DualAnalog(portNum);
                    break;
                case "PsxDualShockGamepad":
                    dev = Psx.DualShock(portNum);
                    break;
                case "PsxNegconGamepad":
                    dev = Psx.NeGcon(portNum);
                    break;
                case "PsxDancepad":
                    dev = Psx.DancePad(portNum);
                    break;
                default:
                    return;
            }
            
            mw.ControllerDefinition = dev;

            // launch controller configuration window
            Grid RootGrid = (Grid)mw.FindName("RootGrid");
            await mw.ShowChildWindowAsync(new ConfigureController()
            {
                IsModal = true,
                AllowMove = false,
                Title = "Controller Configuration",
                CloseOnOverlay = false,
                CloseByEscape = false,
                ShowCloseButton = false
            }, RootGrid);
        }
    }
}
