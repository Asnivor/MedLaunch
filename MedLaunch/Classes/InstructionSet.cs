using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public class InstructionSetDetector
    {
        public MainWindow mw { get; set; }
        public RadioButton btnSs { get; set; }
        public RadioButton btnConfigSs { get; set; }
        public RadioButton btnControlSs { get; set; }

        public InstructionSetDetector()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            btnSs = (RadioButton)mw.FindName("btnSs");
            btnConfigSs = (RadioButton)mw.FindName("btnConfigSs");
            btnControlSs = (RadioButton)mw.FindName("btnControlSs");
        }

        public static InstructionSet GetOperatingSystemInstructionSet()
        {
            var IS = new InstructionSetDetector();
            bool x86 = ((App)Application.Current).IsX86;
            if (x86)
                return InstructionSet.x86;
            else
                return InstructionSet.x64;
        }

        public static InstructionSet GetExeInstructionSet(string path)
        {
            if (!File.Exists(path))
                return InstructionSet.x64;

            try
            {
                byte[] exeData = File.ReadAllBytes(path).Skip(132).Take(10).ToArray();
                var i = exeData[0];
                if (i == 100)
                {
                    return InstructionSet.x64;
                }
                else if (i == 76)
                {
                    return InstructionSet.x86;
                }
            }
            catch
            {
                return InstructionSet.x64;
            }
            // unknown - assume x64
            return InstructionSet.x64;                
        }

        // shows/hides saturn core options depending on x64/x86 detected
        public static void DoShowHides()
        {
            var IS = new InstructionSetDetector();
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

    public enum InstructionSet
    {
        x64,
        x86
    }
}
