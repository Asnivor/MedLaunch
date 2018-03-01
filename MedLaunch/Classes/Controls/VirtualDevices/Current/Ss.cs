﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class Ss : VirtualDeviceBase
    {
        public static DeviceDefinition GamePad(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Digital GamePad";
            device.ControllerName = "gamepad";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition ThreeD(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS 3D Control Pad";
            device.ControllerName = "3dpad";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };

            device.CustomOptions = new List<NonControlMapping>
            {
                new NonControlMapping
                {
                Description = "Default position for 'MODE' switch",
                MednafenCommand = device.CommandStart + ".3dpad.mode.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "digital", "analog"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },
            };
            
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Mission(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Mission Stick";
            device.ControllerName = "mission";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition DMission(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Dual Mission Stick";
            device.ControllerName = "dmission";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };

            device.CustomOptions = new List<NonControlMapping>
            {
                new NonControlMapping
                {
                Description = "Default position for 'A AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afa.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'B AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afb.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'C AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afc.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'X AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afx.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'Y AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afy.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'Z AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afz.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'L AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afl.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },

                new NonControlMapping
                {
                Description = "Default position for 'R AF' switch",
                MednafenCommand = device.CommandStart + ".dmission.afr.defpos",
                MinValue = 2,
                MaxValue = 6,
                Values = new List<string>
                    {
                        "off", "on"
                    },
                ContType = ContrType.COMBO,
                ConvType = ConvertionType.STRING
                },
            };

            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Wheel(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Steering Wheel";
            device.ControllerName = "wheel";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition Gun(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Light Gun";
            device.ControllerName = "gun";
            device.CommandStart = "ss.input.port" + VirtualPort;
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
            device.DeviceName = "SS Mouse";
            device.ControllerName = "mouse";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition KeyboardJP(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Keyboard (JP)";
            device.ControllerName = "jpkeyboard";
            device.CommandStart = "ss.input.port" + VirtualPort;
            device.VirtualPort = VirtualPort;
            device.MapList = new List<Mapping>
            {
                /* MapList is now autogenerated from mednafen.cfg */
            };
            DeviceDefinition.ParseOptionsFromConfig(device);
            DeviceDefinition.PopulateConfig(device);
            return device;
        }

        public static DeviceDefinition KeyboardUS(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "SS Keyboard (US)";
            device.ControllerName = "keyboard";
            device.CommandStart = "ss.input.port" + VirtualPort;
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
