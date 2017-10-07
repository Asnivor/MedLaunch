using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Md
    {
        public static DeviceDefinition ThreeButton(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "MD GamePad (3-Button)";
            device.CommandStart = "md.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad.start" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".gamepad.b" },
                new Mapping { Description = "C", MednafenCommand = device.CommandStart +".gamepad.c" },
                new Mapping { Description = "Rapid A", MednafenCommand = device.CommandStart +".gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = device.CommandStart +".gamepad.rapid_b" },
                new Mapping { Description = "Rapid C", MednafenCommand = device.CommandStart +".gamepad.rapid_c" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition TwoButton(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "MD GamePad (2-Button)";
            device.CommandStart = "md.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad2.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad2.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad2.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad2.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad2.start" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".gamepad2.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".gamepad2.b" },
                new Mapping { Description = "Rapid A", MednafenCommand = device.CommandStart +".gamepad2.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = device.CommandStart +".gamepad2.rapid_b" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition SixButton(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "MD GamePad (6-Button)";
            device.CommandStart = "md.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad6.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad6.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad6.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad6.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad6.start" },
                new Mapping { Description = "Mode", MednafenCommand = device.CommandStart +".gamepad6.mode" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".gamepad6.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".gamepad6.b" },
                new Mapping { Description = "C", MednafenCommand = device.CommandStart +".gamepad6.c" },
                new Mapping { Description = "X", MednafenCommand = device.CommandStart +".gamepad6.x" },
                new Mapping { Description = "Y", MednafenCommand = device.CommandStart +".gamepad6.y" },
                new Mapping { Description = "Z", MednafenCommand = device.CommandStart +".gamepad6.z" },
                new Mapping { Description = "Rapid A", MednafenCommand = device.CommandStart +".gamepad6.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = device.CommandStart +".gamepad6.rapid_b" },
                new Mapping { Description = "Rapid C", MednafenCommand = device.CommandStart +".gamepad6.rapid_c" },
                new Mapping { Description = "Rapid X", MednafenCommand = device.CommandStart +".gamepad6.rapid_x" },
                new Mapping { Description = "Rapid Y", MednafenCommand = device.CommandStart +".gamepad6.rapid_y" },
                new Mapping { Description = "Rapid Z", MednafenCommand = device.CommandStart +".gamepad6.rapid_z" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition MegaMouse(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "MD Mega Mouse";
            device.CommandStart = "md.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Left Button", MednafenCommand = device.CommandStart +".megamouse.left" },
                new Mapping { Description = "Middle Button", MednafenCommand = device.CommandStart +".megamouse.middle" },
                new Mapping { Description = "Right Button", MednafenCommand = device.CommandStart +".megamouse.right" },
                new Mapping { Description = "Start Button", MednafenCommand = device.CommandStart +".gamepad6.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".megamouse.start" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
