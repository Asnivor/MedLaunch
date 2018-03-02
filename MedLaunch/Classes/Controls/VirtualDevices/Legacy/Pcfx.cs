using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Pcfx_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "PCFX GamePad";
            device.CommandStart = "pcfx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".gamepad.select" },
                new Mapping { Description = "RUN", MednafenCommand = device.CommandStart +".gamepad.run" },
                new Mapping { Description = "MODE 1", MednafenCommand = device.CommandStart +".gamepad.mode1" },
                new Mapping { Description = "MODE 2", MednafenCommand = device.CommandStart +".gamepad.mode2" },
                new Mapping { Description = "I", MednafenCommand = device.CommandStart +".gamepad.i" },
                new Mapping { Description = "II", MednafenCommand = device.CommandStart +".gamepad.ii" },
                new Mapping { Description = "III", MednafenCommand = device.CommandStart +".gamepad.iii" },
                new Mapping { Description = "IV", MednafenCommand = device.CommandStart +".gamepad.iv" },
                new Mapping { Description = "V", MednafenCommand = device.CommandStart +".gamepad.v" },
                new Mapping { Description = "VI", MednafenCommand = device.CommandStart +".gamepad.vi" },
                /*
                new Mapping { Description = "Rapid I", MednafenCommand = device.CommandStart +".gamepad.rapid_i" },
                new Mapping { Description = "Rapid II", MednafenCommand = device.CommandStart +".gamepad.rapid_ii" }
                */
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }

        public static IDeviceDefinition Mouse(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "PCFX Mouse";
            device.CommandStart = "pcfx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Left Button", MednafenCommand = device.CommandStart +".mouse.left" },
                new Mapping { Description = "Right Button", MednafenCommand = device.CommandStart +".mouse.right" },
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
