using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Pce_fast
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PCE (fast) GamePad";
            device.CommandStart = "pce_fast.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".gamepad.select" },
                new Mapping { Description = "RUN", MednafenCommand = device.CommandStart +".gamepad.run" },
                new Mapping { Description = "MODE", MednafenCommand = device.CommandStart +".gamepad.mode_select" },
                new Mapping { Description = "I", MednafenCommand = device.CommandStart +".gamepad.i" },
                new Mapping { Description = "II", MednafenCommand = device.CommandStart +".gamepad.ii" },
                new Mapping { Description = "III", MednafenCommand = device.CommandStart +".gamepad.iii" },
                new Mapping { Description = "IV", MednafenCommand = device.CommandStart +".gamepad.iv" },
                new Mapping { Description = "V", MednafenCommand = device.CommandStart +".gamepad.v" },
                new Mapping { Description = "VI", MednafenCommand = device.CommandStart +".gamepad.vi" },
                new Mapping { Description = "Rapid I", MednafenCommand = device.CommandStart +".gamepad.rapid_i" },
                new Mapping { Description = "Rapid II", MednafenCommand = device.CommandStart +".gamepad.rapid_ii" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Mouse(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PCE (fast) Mouse";
            device.CommandStart = "pce_fast.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Left Button", MednafenCommand = device.CommandStart +".mouse.left" },
                new Mapping { Description = "Right Button", MednafenCommand = device.CommandStart +".mouse.right" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
