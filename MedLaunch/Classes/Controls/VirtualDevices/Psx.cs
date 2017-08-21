using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Psx
    {
        public static DeviceDefinition DigitalGamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Digital GamePad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "UP ↑", MednafenCommand = device.CommandStart +".gamepad.up" },
                new Mapping { Description = "DOWN ↓", MednafenCommand = device.CommandStart +".gamepad.down" },
                new Mapping { Description = "LEFT ←", MednafenCommand = device.CommandStart +".gamepad.left" },
                new Mapping { Description = "RIGHT →", MednafenCommand = device.CommandStart +".gamepad.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".gamepad.start" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".gamepad.select" },
                new Mapping { Description = "△ (upper)", MednafenCommand = device.CommandStart +".gamepad.triangle" },
                new Mapping { Description = "○ (right)", MednafenCommand = device.CommandStart +".gamepad.circle" },
                new Mapping { Description = "x (lower)", MednafenCommand = device.CommandStart +".gamepad.cross" },
                new Mapping { Description = "□ (left)", MednafenCommand = device.CommandStart +".gamepad.square" },
                new Mapping { Description = "L1 (front left shoulder)", MednafenCommand = device.CommandStart +".gamepad.l1" },
                new Mapping { Description = "R1 (front right shoulder)", MednafenCommand = device.CommandStart +".gamepad.r1" },
                new Mapping { Description = "L2 (rear left shoulder)", MednafenCommand = device.CommandStart +".gamepad.l2" },
                new Mapping { Description = "R2 (rear right shoulder)", MednafenCommand = device.CommandStart +".gamepad.r2" },
                new Mapping { Description = "Rapid △ (upper)", MednafenCommand = device.CommandStart +".gamepad.rapid_triangle" },
                new Mapping { Description = "Rapid ○ (right)", MednafenCommand = device.CommandStart +".gamepad.rapid_circle" },
                new Mapping { Description = "Rapid x (lower)", MednafenCommand = device.CommandStart +".gamepad.rapid_cross" },
                new Mapping { Description = "Rapid □ (left)", MednafenCommand = device.CommandStart +".gamepad.rapid_square" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DualAnalog(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Dual Analog GamePad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".dualanalog.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".dualanalog.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".dualanalog.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".dualanalog.right" },
                new Mapping { Description = "Left Stick UP ↑", MednafenCommand = device.CommandStart +".dualanalog.lstick_up" },
                new Mapping { Description = "Left Stick DOWN ↓", MednafenCommand = device.CommandStart +".dualanalog.lstick_down" },
                new Mapping { Description = "Left Stick LEFT ←", MednafenCommand = device.CommandStart +".dualanalog.lstick_left" },
                new Mapping { Description = "Left Stick RIGHT →", MednafenCommand = device.CommandStart +".dualanalog.lstick_right" },
                new Mapping { Description = "Right Stick UP ↑", MednafenCommand = device.CommandStart +".dualanalog.rstick_up" },
                new Mapping { Description = "Right Stick DOWN ↓", MednafenCommand = device.CommandStart +".dualanalog.rstick_down" },
                new Mapping { Description = "Right Stick LEFT ←", MednafenCommand = device.CommandStart +".dualanalog.rstick_left" },
                new Mapping { Description = "Right Stick RIGHT →", MednafenCommand = device.CommandStart +".dualanalog.rstick_right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".dualanalog.start" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".dualanalog.select" },
                new Mapping { Description = "△ (upper)", MednafenCommand = device.CommandStart +".dualanalog.triangle" },
                new Mapping { Description = "○ (right)", MednafenCommand = device.CommandStart +".dualanalog.circle" },
                new Mapping { Description = "x (lower)", MednafenCommand = device.CommandStart +".dualanalog.cross" },
                new Mapping { Description = "□ (left)", MednafenCommand = device.CommandStart +".dualanalog.square" },
                new Mapping { Description = "L1 (front left shoulder)", MednafenCommand = device.CommandStart +".dualanalog.l1" },
                new Mapping { Description = "R1 (front right shoulder)", MednafenCommand = device.CommandStart +".dualanalog.r1" },
                new Mapping { Description = "L2 (rear left shoulder)", MednafenCommand = device.CommandStart +".dualanalog.l2" },
                new Mapping { Description = "R2 (rear right shoulder)", MednafenCommand = device.CommandStart +".dualanalog.r2" },
                new Mapping { Description = "Left Stick, Button(L3)", MednafenCommand = device.CommandStart +".dualanalog.l3" },
                new Mapping { Description = "Right Stick, Button(L3)", MednafenCommand = device.CommandStart +".dualanalog.r3" },
                new Mapping { Description = "Rapid △ (upper)", MednafenCommand = device.CommandStart +".dualanalog.rapid_triangle" },
                new Mapping { Description = "Rapid ○ (right)", MednafenCommand = device.CommandStart +".dualanalog.rapid_circle" },
                new Mapping { Description = "Rapid x (lower)", MednafenCommand = device.CommandStart +".dualanalog.rapid_cross" },
                new Mapping { Description = "Rapid □ (left)", MednafenCommand = device.CommandStart +".dualanalog.rapid_square" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DualShock(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX DualShock GamePad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".dualshock.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".dualshock.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".dualshock.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".dualshock.right" },
                new Mapping { Description = "Left Stick UP ↑", MednafenCommand = device.CommandStart +".dualshock.lstick_up" },
                new Mapping { Description = "Left Stick DOWN ↓", MednafenCommand = device.CommandStart +".dualshock.lstick_down" },
                new Mapping { Description = "Left Stick LEFT ←", MednafenCommand = device.CommandStart +".dualshock.lstick_left" },
                new Mapping { Description = "Left Stick RIGHT →", MednafenCommand = device.CommandStart +".dualshock.lstick_right" },
                new Mapping { Description = "Right Stick UP ↑", MednafenCommand = device.CommandStart +".dualshock.rstick_up" },
                new Mapping { Description = "Right Stick DOWN ↓", MednafenCommand = device.CommandStart +".dualshock.rstick_down" },
                new Mapping { Description = "Right Stick LEFT ←", MednafenCommand = device.CommandStart +".dualshock.rstick_left" },
                new Mapping { Description = "Right Stick RIGHT →", MednafenCommand = device.CommandStart +".dualshock.rstick_right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".dualshock.start" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".dualshock.select" },
                new Mapping { Description = "△ (upper)", MednafenCommand = device.CommandStart +".dualshock.triangle" },
                new Mapping { Description = "○ (right)", MednafenCommand = device.CommandStart +".dualshock.circle" },
                new Mapping { Description = "x (lower)", MednafenCommand = device.CommandStart +".dualshock.cross" },
                new Mapping { Description = "□ (left)", MednafenCommand = device.CommandStart +".dualshock.square" },
                new Mapping { Description = "L1 (front left shoulder)", MednafenCommand = device.CommandStart +".dualshock.l1" },
                new Mapping { Description = "R1 (front right shoulder)", MednafenCommand = device.CommandStart +".dualshock.r1" },
                new Mapping { Description = "L2 (rear left shoulder)", MednafenCommand = device.CommandStart +".dualshock.l2" },
                new Mapping { Description = "R2 (rear right shoulder)", MednafenCommand = device.CommandStart +".dualshock.r2" },
                new Mapping { Description = "Left Stick, Button(L3)", MednafenCommand = device.CommandStart +".dualshock.l3" },
                new Mapping { Description = "Right Stick, Button(L3)", MednafenCommand = device.CommandStart +".dualshock.r3" },
                new Mapping { Description = "Rapid △ (upper)", MednafenCommand = device.CommandStart +".dualshock.rapid_triangle" },
                new Mapping { Description = "Rapid ○ (right)", MednafenCommand = device.CommandStart +".dualshock.rapid_circle" },
                new Mapping { Description = "Rapid x (lower)", MednafenCommand = device.CommandStart +".dualshock.rapid_cross" },
                new Mapping { Description = "Rapid □ (left)", MednafenCommand = device.CommandStart +".dualshock.rapid_square" },
                new Mapping { Description = "Analog(mode toggle)", MednafenCommand = device.CommandStart +".dualshock.analog" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DancePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX DancePad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".dancepad.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".dancepad.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".dancepad.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".dancepad.right" },                
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".dancepad.start" },
                new Mapping { Description = "SELECT", MednafenCommand = device.CommandStart +".dancepad.select" },
                new Mapping { Description = "△ (upper)", MednafenCommand = device.CommandStart +".dancepad.triangle" },
                new Mapping { Description = "○ (right)", MednafenCommand = device.CommandStart +".dancepad.circle" },
                new Mapping { Description = "x (lower)", MednafenCommand = device.CommandStart +".dancepad.cross" },
                new Mapping { Description = "□ (left)", MednafenCommand = device.CommandStart +".dancepad.square" }
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition NeGcon(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX NeGcon Controller";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "D-Pad UP ↑", MednafenCommand = device.CommandStart +".negcon.up" },
                new Mapping { Description = "D-Pad DOWN ↓", MednafenCommand = device.CommandStart +".negcon.down" },
                new Mapping { Description = "D-Pad LEFT ←", MednafenCommand = device.CommandStart +".negcon.left" },
                new Mapping { Description = "D-Pad RIGHT →", MednafenCommand = device.CommandStart +".negcon.right" },
                new Mapping { Description = "START", MednafenCommand = device.CommandStart +".negcon.start" },
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".negcon.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".negcon.b" },
                new Mapping { Description = "I (Analog)", MednafenCommand = device.CommandStart +".negcon.i" },
                new Mapping { Description = "II (Analog)", MednafenCommand = device.CommandStart +".negcon.ii" },
                new Mapping { Description = "Twist ↑|↓ (Analog, Turn Left)", MednafenCommand = device.CommandStart +".negcon.twist_ccwise" },
                new Mapping { Description = "Twist ↓|↑ (Analog, Turn Right)", MednafenCommand = device.CommandStart +".negcon.twist_cwise" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition GunCon(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX GunCon Controller";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "A", MednafenCommand = device.CommandStart +".guncon.a" },
                new Mapping { Description = "B", MednafenCommand = device.CommandStart +".guncon.b" },
                new Mapping { Description = "Offscreen Shot(Simulated)", MednafenCommand = device.CommandStart +".guncon.offscreen_shot" },
                new Mapping { Description = "Trigger", MednafenCommand = device.CommandStart +".guncon.trigger" },
                new Mapping { Description = "X Axis", MednafenCommand = device.CommandStart +".guncon.x_axis" },
                new Mapping { Description = "Y Axis", MednafenCommand = device.CommandStart +".guncon.y_axis" },
            };
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
