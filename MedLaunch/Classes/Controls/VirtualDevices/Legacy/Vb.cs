using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    class Vb_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "VB GamePad";
            device.CommandStart = "vb.input.builtin";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑ (Left D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.up-l" },
                new Mapping { Description = "DOWN ↓ (Left D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.down-l" },
                new Mapping { Description = "LEFT ← (Left D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.left-l" },
                new Mapping { Description = "RIGHT → (Left D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.right-l" },
                new Mapping { Description = "UP ↑ (Right D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.up-r" },
                new Mapping { Description = "DOWN ↓ (Right D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.down-r" },
                new Mapping { Description = "LEFT ← (Right D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.left-r" },
                new Mapping { Description = "RIGHT → (Right D-Pad)", MednafenCommand = "vb.input.builtin.gamepad.right-r" },
                new Mapping { Description = "START", MednafenCommand = "vb.input.builtin.gamepad.start" },
                new Mapping { Description = "SELECT", MednafenCommand = "vb.input.builtin.gamepad.select" },
                new Mapping { Description = "A", MednafenCommand = "vb.input.builtin.gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = "vb.input.builtin.gamepad.b" },
                new Mapping { Description = "Left-Back", MednafenCommand = "vb.input.builtin.gamepad.lt" },
                new Mapping { Description = "Right-Back", MednafenCommand = "vb.input.builtin.gamepad.rt" },
                new Mapping { Description = "Rapid A", MednafenCommand = "vb.input.builtin.gamepad.rapid_a" },
                new Mapping { Description = "Rapid B", MednafenCommand = "vb.input.builtin.gamepad.rapid_b" }
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
