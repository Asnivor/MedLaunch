using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoytickInterop;
using System.Windows;

namespace MedLaunch.Classes.Input
{
    public class Controller
    {        
        public static void Start()
        {

            SDLJoystick.InitJoystickSystem();

            List<string> sticks = SDLJoystick.JoystickNames;
            foreach (var n in sticks)
            {
                MessageBox.Show(n.ToString());

                SDLJoystick j = new SDLJoystick(n);
                MessageBox.Show("Buttons:  " + j.Buttons.ToString());
                MessageBox.Show("Hats:  " + j.Hats.ToString());
                MessageBox.Show("Axis:  " + j.Axes.ToString());
                MessageBox.Show("Balls:  " + j.Balls.ToString());
            }

            
        }
        
    }
}
