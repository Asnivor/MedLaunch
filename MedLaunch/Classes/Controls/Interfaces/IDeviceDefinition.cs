using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls
{
    public interface IDeviceDefinition
    {
        string DeviceName { get; set; }
        string CommandStart { get; set; }
        int VirtualPort { get; set; }
        List<Mapping> MapList { get; set; }
    }

    public class Mapping
    {
        // legacy
        public string MednafenCommand { get; set; }
        public string Description { get; set; }
        public string Config { get; set; }

        // current
        public DeviceType DeviceType { get; set; }
        public string DeviceID { get; set; }
        public string LogicString { get; set; }
        public string Scale { get; set; }

        public Mapping Primary { get; set; }
        public Mapping Secondary { get; set; }
        public Mapping Tertiary { get; set; }
    }

    public enum DeviceType
    {
        Keyboard,
        Mouse,
        Joystick
    }
}
