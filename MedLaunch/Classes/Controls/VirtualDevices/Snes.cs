using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Snes
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SNES GamePad";
            device.CommandStart = "snes.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad.start" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".gamepad.select" },
                new Mapping { Description = "B (center, lower)", MednafenCommand = device.CommandStart +".gamepad.b" },
                new Mapping { Description = "A (right)", MednafenCommand = device.CommandStart +".gamepad.a" },
                new Mapping { Description = "Y (left)", MednafenCommand = device.CommandStart +".gamepad.y" },
                new Mapping { Description = "X (center, upper)", MednafenCommand = device.CommandStart +".gamepad.x" },
                new Mapping { Description = "Left Shoulder", MednafenCommand = device.CommandStart +".gamepad.l" },
                new Mapping { Description = "Right Shoulder", MednafenCommand = device.CommandStart +".gamepad.r" },
                new Mapping { Description = "Rapid B (center, lower)", MednafenCommand = device.CommandStart +".gamepad.rapid_b" },
                new Mapping { Description = "Rapid A (right)", MednafenCommand = device.CommandStart +".gamepad.rapid_a" },
                new Mapping { Description = "Rapid Y (left)", MednafenCommand = device.CommandStart +".gamepad.rapid_y" },
                new Mapping { Description = "Rapid X (center, upper)", MednafenCommand = device.CommandStart +".gamepad.rapid_x" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
