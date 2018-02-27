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
        public ChildWindow ParentWindow { get; set; }
        public MainWindow MainWindow { get; set; }

        public ConfigureModWindow()
        {
            InitializeComponent();

            // get the parent window
            var ParentWindow1 = Application.Current.Windows.OfType<ConfigureController>().ToList(); //.Where(a => a.GetType() == typeof(ConfigureModWindow)).FirstOrDefault();

            // get the mainwindow
            MainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // init defaults
            slScaleFactor.Value = 4096;
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
    }
}
