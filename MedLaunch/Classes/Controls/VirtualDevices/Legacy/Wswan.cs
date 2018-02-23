using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Wswan_Legacy : VirtualDeviceBase
    {
        public static IDeviceDefinition GamePad(int VirtualPort)
        {
            IDeviceDefinition device = new DeviceDefinitionLegacy();
            device.DeviceName = "WSWAN GamePad";
            device.CommandStart = "wswan.input.builtin";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "X1(X UP ↑)", MednafenCommand = "wswan.input.builtin.gamepad.up-x" },
                new Mapping { Description = "X3(X DOWN ↓)", MednafenCommand = "wswan.input.builtin.gamepad.down-x" },
                new Mapping { Description = "X4(X LEFT ←)", MednafenCommand = "wswan.input.builtin.gamepad.left-x" },
                new Mapping { Description = "X2(X RIGHT →)", MednafenCommand = "wswan.input.builtin.gamepad.right-x" },
                new Mapping { Description = "Y1(Y UP ↑)", MednafenCommand = "wswan.input.builtin.gamepad.up-y" },
                new Mapping { Description = "Y3(Y DOWN ↓)", MednafenCommand = "wswan.input.builtin.gamepad.down-y" },
                new Mapping { Description = "Y4(Y LEFT ←)", MednafenCommand = "wswan.input.builtin.gamepad.left-y" },
                new Mapping { Description = "Y2(Y RIGHT →)", MednafenCommand = "wswan.input.builtin.gamepad.right-y" },
                new Mapping { Description = "START", MednafenCommand = "wswan.input.builtin.gamepad.start" },
                new Mapping { Description = "A", MednafenCommand = "wswan.input.builtin.gamepad.a" },
                new Mapping { Description = "B", MednafenCommand = "wswan.input.builtin.gamepad.b" }
            };
            DeviceDefinitionLegacy.PopulateConfig(device);
            return device;
        }
    }
}
