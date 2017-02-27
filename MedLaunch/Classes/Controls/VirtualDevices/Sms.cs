using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Sms
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SMS GamePad";
            device.CommandStart = "sms.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "Fire 1/Start", MednafenCommand = device.CommandStart +".gamepad.fire1" },
                new Mapping { Description = "Fire 2", MednafenCommand = device.CommandStart +".gamepad.fire2" },
                new Mapping { Description = "Rapid Fire 1/Start", MednafenCommand = device.CommandStart +".gamepad.rapid_fire1" },
                new Mapping { Description = "Rapid Fire 2", MednafenCommand = device.CommandStart +".gamepad.rapid_fire2" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
