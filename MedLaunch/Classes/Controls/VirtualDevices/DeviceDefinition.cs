using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class DeviceDefinition
    {
        public string DeviceName { get; set; }
        public string CommandStart { get; set; }
        public int VirtualPort { get; set; }
        public List<Mapping> MapList { get; set; }

        public DeviceDefinition()
        {
            MapList = new List<Mapping>();
        }
    }

    public class Mapping
    {
        public string MednafenCommand { get; set; }
        public string Description { get; set; }
        public string Config { get; set; }
    }
}
