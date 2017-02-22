using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Gba
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "GBA GamePad";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = "gba.input.builtin.gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = "gba.input.builtin.gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = "gba.input.builtin.gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = "gba.input.builtin.gamepad.right" },
                new Mapping { Description = "START", MednafenCommand = "gba.input.builtin.gamepad.start" },
                new Mapping { Description = "SELECT", MednafenCommand = "gba.input.builtin.gamepad.select" },
                new Mapping { Description = "A", MednafenCommand = "gba.input.builtin.gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = "gba.input.builtin.gamepad.b" },
                new Mapping { Description = "SHOULDER L", MednafenCommand = "gba.input.builtin.gamepad.shoulder_l" },
                new Mapping { Description = "SHOULDER R", MednafenCommand = "gba.input.builtin.gamepad.shoulder_r" },
                new Mapping { Description = "Rapid A", MednafenCommand = "gba.input.builtin.gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = "gba.input.builtin.gamepad.rapid_b" }                
            };
            return device;
        }
    }
}
