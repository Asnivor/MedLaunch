using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Ngp_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "NGP GamePad";
            device.CommandStart = "ngp.input.builtin";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = "ngp.input.builtin.gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = "ngp.input.builtin.gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = "ngp.input.builtin.gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = "ngp.input.builtin.gamepad.right" },
                new Mapping { Description = "OPTION", MednafenCommand = "ngp.input.builtin.gamepad.option" },
                new Mapping { Description = "A", MednafenCommand = "ngp.input.builtin.gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = "ngp.input.builtin.gamepad.b" },
                new Mapping { Description = "Rapid A", MednafenCommand = "ngp.input.builtin.gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = "ngp.input.builtin.gamepad.rapid_b" }
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
