using MedLaunch.Models;
using System.Collections.Generic;

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
