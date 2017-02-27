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
            DeviceDefinition.PopulateConfig(device);
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
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Mission(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Mission Stick";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {          
                new Mapping { Description = "Stick FORE ↑ (Analog)", MednafenCommand = device.CommandStart +".mission.stick_fore" },
                new Mapping { Description = "Stick BACK ↓ (Analog)", MednafenCommand = device.CommandStart +".mission.stick_back" },
                new Mapping { Description = "Stick LEFT ← (Analog)", MednafenCommand = device.CommandStart +".mission.stick_left" },
                new Mapping { Description = "Stick RIGHT → (Analog)", MednafenCommand = device.CommandStart +".mission.stick_right" },
                new Mapping { Description = "Throttle Up (Analog)", MednafenCommand = device.CommandStart +".mission.throttle_up" },
                new Mapping { Description = "Throttle Down (Analog)", MednafenCommand = device.CommandStart +".mission.throttle_down" },
                
                new Mapping { Description = "A (Stick Trigger)", MednafenCommand = device.CommandStart +".mission.a" },
                new Mapping { Description = "B (Stick Left Button)", MednafenCommand = device.CommandStart +".mission.b" },
                new Mapping { Description = "C (Stick Right Button)", MednafenCommand = device.CommandStart +".mission.c" },
                new Mapping { Description = "X", MednafenCommand = device.CommandStart +".mission.x" },
                new Mapping { Description = "Y", MednafenCommand = device.CommandStart +".mission.y" },
                new Mapping { Description = "Z", MednafenCommand = device.CommandStart +".mission.z" },
                new Mapping { Description = "L", MednafenCommand = device.CommandStart +".mission.l" },
                new Mapping { Description = "R", MednafenCommand = device.CommandStart +".mission.r" },

                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".mission.start" },
                new Mapping { Description = "Autofire Speed", MednafenCommand = device.CommandStart +".mission.afspeed" },

                new Mapping { Description = "A AF", MednafenCommand = device.CommandStart +".mission.afa" },
                new Mapping { Description = "B AF", MednafenCommand = device.CommandStart +".mission.afb" },
                new Mapping { Description = "C AF", MednafenCommand = device.CommandStart +".mission.afc" },
                new Mapping { Description = "X AF", MednafenCommand = device.CommandStart +".mission.afx" },
                new Mapping { Description = "Y AF", MednafenCommand = device.CommandStart +".mission.afy" },
                new Mapping { Description = "Z AF", MednafenCommand = device.CommandStart +".mission.afz" },
                new Mapping { Description = "L AF", MednafenCommand = device.CommandStart +".mission.afl" },
                new Mapping { Description = "R AF", MednafenCommand = device.CommandStart +".mission.afr" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DMission(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Dual Mission Stick";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".dmission.start" },
                new Mapping { Description = "Autofire Speed", MednafenCommand = device.CommandStart +".dmission.afspeed" },

                new Mapping { Description = "L Stick FORE ↑ (Analog)", MednafenCommand = device.CommandStart +".dmission.lstick_fore" },
                new Mapping { Description = "L Stick BACK ↓ (Analog)", MednafenCommand = device.CommandStart +".dmission.lstick_back" },
                new Mapping { Description = "L Stick LEFT ← (Analog)", MednafenCommand = device.CommandStart +".dmission.lstick_left" },
                new Mapping { Description = "L Stick RIGHT → (Analog)", MednafenCommand = device.CommandStart +".dmission.lstick_right" },
                new Mapping { Description = "L Throttle Up (Analog)", MednafenCommand = device.CommandStart +".dmission.lthrottle_up" },
                new Mapping { Description = "L Throttle Down (Analog)", MednafenCommand = device.CommandStart +".dmission.lthrottle_down" },
                new Mapping { Description = "L", MednafenCommand = device.CommandStart +".dmission.l" },
                new Mapping { Description = "X (L Stick Trigger)", MednafenCommand = device.CommandStart +".dmission.x" },
                new Mapping { Description = "Y (L Stick Left Button)", MednafenCommand = device.CommandStart +".dmission.y" },
                new Mapping { Description = "Z (L Stick Right Button)", MednafenCommand = device.CommandStart +".dmission.z" },

                new Mapping { Description = "L AF", MednafenCommand = device.CommandStart +".dmission.afl" },
                new Mapping { Description = "X AF", MednafenCommand = device.CommandStart +".dmission.afx" },
                new Mapping { Description = "Y AF", MednafenCommand = device.CommandStart +".dmission.afy" },
                new Mapping { Description = "Z AF", MednafenCommand = device.CommandStart +".dmission.afz" },

                new Mapping { Description = "R Stick FORE ↑ (Analog)", MednafenCommand = device.CommandStart +".dmission.rstick_fore" },
                new Mapping { Description = "R Stick BACK ↓ (Analog)", MednafenCommand = device.CommandStart +".dmission.rstick_back" },
                new Mapping { Description = "R Stick LEFT ← (Analog)", MednafenCommand = device.CommandStart +".dmission.rstick_left" },
                new Mapping { Description = "R Stick RIGHT → (Analog)", MednafenCommand = device.CommandStart +".dmission.rstick_right" },
                new Mapping { Description = "R Throttle Up (Analog)", MednafenCommand = device.CommandStart +".dmission.rthrottle_up" },
                new Mapping { Description = "R Throttle Down (Analog)", MednafenCommand = device.CommandStart +".dmission.rthrottle_down" },
                new Mapping { Description = "R", MednafenCommand = device.CommandStart +".dmission.r" },
                new Mapping { Description = "A (L Stick Trigger)", MednafenCommand = device.CommandStart +".dmission.a" },
                new Mapping { Description = "B (L Stick Left Button)", MednafenCommand = device.CommandStart +".dmission.b" },
                new Mapping { Description = "C (L Stick Right Button)", MednafenCommand = device.CommandStart +".dmission.c" },

                new Mapping { Description = "R AF", MednafenCommand = device.CommandStart +".dmission.afr" },
                new Mapping { Description = "A AF", MednafenCommand = device.CommandStart +".dmission.afa" },
                new Mapping { Description = "B AF", MednafenCommand = device.CommandStart +".dmission.afb" },
                new Mapping { Description = "C AF", MednafenCommand = device.CommandStart +".dmission.afc" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Wheel(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Steering Wheel";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Analog LEFT ←", MednafenCommand = device.CommandStart +".wheel.analog_left" },
                new Mapping { Description = "Analog RIGHT →", MednafenCommand = device.CommandStart +".wheel.analog_right" },
                new Mapping { Description = "L Gear Shift(Equiv. UP ↑)", MednafenCommand = device.CommandStart +".wheel.up" },
                new Mapping { Description = "R Gear Shift(Equiv. DOWN ↓)", MednafenCommand = device.CommandStart +".wheel.down" },
                new Mapping { Description = "Throttle Up (Analog)", MednafenCommand = device.CommandStart +".mission.throttle_up" },
                new Mapping { Description = "Throttle Down (Analog)", MednafenCommand = device.CommandStart +".mission.throttle_down" },

                new Mapping { Description = "X (L Group)", MednafenCommand = device.CommandStart +".wheel.x" },
                new Mapping { Description = "Y (L Group)", MednafenCommand = device.CommandStart +".wheel.y" },
                new Mapping { Description = "Z (L Group)", MednafenCommand = device.CommandStart +".wheel.z" },
                new Mapping { Description = "A (R Group)", MednafenCommand = device.CommandStart +".wheel.a" },
                new Mapping { Description = "B (R Group)", MednafenCommand = device.CommandStart +".wheel.b" },
                new Mapping { Description = "C (R Group)", MednafenCommand = device.CommandStart +".wheel.c" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
