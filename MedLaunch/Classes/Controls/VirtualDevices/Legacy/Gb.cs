using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Gb_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "GB GamePad";
            device.CommandStart = "gb.input.builtin";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = "gb.input.builtin.gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = "gb.input.builtin.gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = "gb.input.builtin.gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = "gb.input.builtin.gamepad.right" },
                new Mapping { Description = "SELECT", MednafenCommand = "gb.input.builtin.gamepad.select" },
                new Mapping { Description = "START", MednafenCommand = "gb.input.builtin.gamepad.start" },                
                new Mapping { Description = "A", MednafenCommand = "gb.input.builtin.gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = "gb.input.builtin.gamepad.b" },
                new Mapping { Description = "Rapid A", MednafenCommand = "gb.input.builtin.gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = "gb.input.builtin.gamepad.rapid_b" },
                new Mapping { Description = "Tilt: UP ↑", MednafenCommand = "gb.input.tilt.tilt.up" },
                new Mapping { Description = "Tilt: DOWN ↓", MednafenCommand = "gb.input.tilt.tilt.down" },
                new Mapping { Description = "Tilt: LEFT ←", MednafenCommand = "gb.input.tilt.tilt.left" },
                new Mapping { Description = "Tilt: RIGHT →", MednafenCommand = "gb.input.tilt.tilt.right" },
            };

            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
