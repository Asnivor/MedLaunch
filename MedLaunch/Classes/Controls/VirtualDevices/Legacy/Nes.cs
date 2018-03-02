using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Nes_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "NES GamePad";
            device.CommandStart = "nes.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".gamepad.select" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad.start" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".gamepad.b" },
                new Mapping { Description = "Rapid A", MednafenCommand = device.CommandStart +".gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = device.CommandStart +".gamepad.rapid_b" },
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }

        public static IDeviceDefinition Zapper(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "NES Zapper";
            device.CommandStart = "nes.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Away Trigger", MednafenCommand = device.CommandStart +".zapper.away_trigger" },
                new Mapping { Description = "Trigger", MednafenCommand = device.CommandStart +".zapper.trigger" },
                new Mapping { Description = "X Axis", MednafenCommand = device.CommandStart +".zapper.x_axis" },
                new Mapping { Description = "Y Axis", MednafenCommand = device.CommandStart +".zapper.y_axis" },
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }

    
}
