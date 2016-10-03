using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public class ServersDataObject
    {
        public ServersDataObject()
        {
            ListOfServers = ConfigServerSettings.GetServers();
        }
        public IList<ConfigServerSettings> ListOfServers { get; set; }
    }

    
}
