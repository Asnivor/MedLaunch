using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Lynx_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "LYNX GamePad";
            device.CommandStart = "lynx.input.builtin";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = "lynx.input.builtin.gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = "lynx.input.builtin.gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = "lynx.input.builtin.gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = "lynx.input.builtin.gamepad.right" },
                new Mapping { Description = "PAUSE", MednafenCommand = "lynx.input.builtin.gamepad.pause" },
                new Mapping { Description = "A (outer)", MednafenCommand = "lynx.input.builtin.gamepad.a" },
                new Mapping { Description = "B (inner)", MednafenCommand = "lynx.input.builtin.gamepad.b" },
                new Mapping { Description = "Option 1 (upper)", MednafenCommand = "lynx.input.builtin.gamepad.option_1" },
                new Mapping { Description = "Option 2 (lower)", MednafenCommand = "lynx.input.builtin.gamepad.option_2" },
                new Mapping { Description = "Rapid A (outer)", MednafenCommand = "lynx.input.builtin.gamepad.rapid_a" },
                new Mapping { Description = "Rapid B (inner)", MednafenCommand = "lynx.input.builtin.gamepad.rapid_b" },
                new Mapping { Description = "Rapid Option 1 (upper)", MednafenCommand = "lynx.input.builtin.gamepad.rapid_option_1" },
                new Mapping { Description = "Rapid Option 2 (lower)", MednafenCommand = "lynx.input.builtin.gamepad.rapid_option_2" }
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
