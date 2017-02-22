using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Ss
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Digital GamePad";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "Analog UP ↑", MednafenCommand = device.CommandStart +".gamepad.analog_up" },
                new Mapping { Description = "Analog DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.analog_down" },
                new Mapping { Description = "Analog LEFT ←", MednafenCommand = device.CommandStart +".gamepad.analog_left" },
                new Mapping { Description = "Analog RIGHT →", MednafenCommand = device.CommandStart +".gamepad.analog_right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad.start" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".gamepad.b" },
                new Mapping { Description = "C", MednafenCommand = device.CommandStart +".gamepad.c" },
                new Mapping { Description = "X", MednafenCommand = device.CommandStart +".gamepad.x" },
                new Mapping { Description = "Y", MednafenCommand = device.CommandStart +".gamepad.y" },
                new Mapping { Description = "Z", MednafenCommand = device.CommandStart +".gamepad.z" },
                new Mapping { Description = "Left Shoulder", MednafenCommand = device.CommandStart +".gamepad.ls" },
                new Mapping { Description = "Right Shoulder", MednafenCommand = device.CommandStart +".gamepad.rs" }
            };
            return device;
        }

        public static DeviceDefinition ThreeD(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS 3D Control Pad";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".3dpad.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".3dpad.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".3dpad.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".3dpad.right" },
                new Mapping { Description = "Analog UP ↑", MednafenCommand = device.CommandStart +".3dpad.analog_up" },
                new Mapping { Description = "Analog DOWN ↓", MednafenCommand = device.CommandStart +".3dpad.analog_down" },
                new Mapping { Description = "Analog LEFT ←", MednafenCommand = device.CommandStart +".3dpad.analog_left" },
                new Mapping { Description = "Analog RIGHT →", MednafenCommand = device.CommandStart +".3dpad.analog_right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".3dpad.start" },
                new Mapping { Description = "MODE", MednafenCommand = device.CommandStart +".3dpad.mode" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".3dpad.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".3dpad.b" },
                new Mapping { Description = "C", MednafenCommand = device.CommandStart +".3dpad.c" },
                new Mapping { Description = "X", MednafenCommand = device.CommandStart +".3dpad.x" },
                new Mapping { Description = "Y", MednafenCommand = device.CommandStart +".3dpad.y" },
                new Mapping { Description = "Z", MednafenCommand = device.CommandStart +".3dpad.z" },
                new Mapping { Description = "Left Shoulder (Analog)", MednafenCommand = device.CommandStart +".3dpad.ls" },
                new Mapping { Description = "Right Shoulder (Analog)", MednafenCommand = device.CommandStart +".3dpad.rs" }
            };
            return device;
        }
    }
}
