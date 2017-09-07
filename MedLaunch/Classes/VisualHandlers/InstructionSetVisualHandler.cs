using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes.VisualHandlers
{
    public class InstructionSetVisualHandler
    {
        public MainWindow mw { get; set; }
        public RadioButton btnSs { get; set; }
        public RadioButton btnConfigSs { get; set; }
        public RadioButton btnControlSs { get; set; }

        public InstructionSetVisualHandler()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            btnSs = (RadioButton)mw.FindName("btnSs");
            btnConfigSs = (RadioButton)mw.FindName("btnConfigSs");
            btnControlSs = (RadioButton)mw.FindName("btnControlSs");
        }

        // shows/hides saturn core options depending on x64/x86 detected
        public static void DoShowHides()
        {
            var IS = new InstructionSetVisualHandler();
            bool x86 = ((App)Application.Current).IsX86;

            if (x86 == true)
            {
                IS.btnSs.Visibility = Visibility.Collapsed;
                IS.btnConfigSs.Visibility = Visibility.Collapsed;
                IS.btnControlSs.Visibility = Visibility.Collapsed;
            }
            else
            {
                IS.btnSs.Visibility = Visibility.Visible;
                IS.btnConfigSs.Visibility = Visibility.Visible;
                IS.btnControlSs.Visibility = Visibility.Visible;
            }
        }
    }
}
