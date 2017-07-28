using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordVisualHandler
    {
        public MainWindow mw { get; set; }
        public List<RadioButton> ChannelRadios { get; set; }
        public List<Button> UserButtons { get; set; }
        public TextBox tbDiscordName { get; set; }


        public DiscordVisualHandler()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get misc controls
            tbDiscordName = (TextBox)mw.FindName("tbDiscordName");

            // get channel radios

            // get user buttons
        }

        public static void Disconnected()
        {

        }
    }
}
