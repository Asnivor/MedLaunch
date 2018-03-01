﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Psx : VirtualDeviceBase
    {
        public static DeviceDefinition DigitalGamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Digital GamePad";
            device.ControllerName = "gamepad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition AnalogJoystick(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Analog Joystick";
            device.ControllerName = "analogjoy";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };

            device.CustomOptions = new List<NonControlMapping>
            {
                new NonControlMapping { Description = "Analog axis scale coefficient",
                    MednafenCommand = device.CommandStart + ".analogjoy.axis_scale",
                    MinValue = 1, MaxValue = 1.5,
                    ContType = ContrType.UPDOWN,
                    TickFrequency = 0.01,
                 ConvType = ConvertionType.DOUBLE},
            };

            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DualAnalog(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Dual Analog GamePad";
            device.ControllerName = "dualanalog";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };

            device.CustomOptions = new List<NonControlMapping>
            {
                new NonControlMapping { Description = "Analog axis scale coefficient",
                    MednafenCommand = device.CommandStart + ".dualanalog.axis_scale",
                    MinValue = 1, MaxValue = 1.5,
                    ContType = ContrType.UPDOWN,
                    TickFrequency = 0.01,
                 ConvType = ConvertionType.DOUBLE},
            };

            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DualShock(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX DualShock GamePad";
            device.ControllerName = "dualshock";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };

            device.CustomOptions = new List<NonControlMapping>
            {
                new NonControlMapping { Description = "Analog axis scale coefficient",
                    MednafenCommand = device.CommandStart + ".dualshock.axis_scale",
                    MinValue = 1, MaxValue = 1.5,
                    ContType = ContrType.UPDOWN,
                    TickFrequency = 0.01,
                 ConvType = ConvertionType.DOUBLE},
            };

            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DancePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX DancePad";
            device.ControllerName = "dancepad";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition NeGcon(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX NeGcon Controller";
            device.ControllerName = "negcon";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition GunCon(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX GunCon Controller";
            device.ControllerName = "guncon";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Justifier(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Konami Justifier";
            device.ControllerName = "justifier";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Mouse(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "PSX Mouse";
            device.ControllerName = "mouse";
            device.CommandStart = "psx.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
