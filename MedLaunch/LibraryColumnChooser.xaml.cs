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

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for LibraryColumnChooser.xaml
    /// </summary>
    public partial class LibraryColumnChooser : ChildWindow
    {
        public LibraryColumnChooser()
        {
            InitializeComponent();
        }

        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
