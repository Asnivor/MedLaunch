using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.ViewModels
{
    public class ServersViewModel
    {
        public ServersViewModel()
        { }

        public ServersViewModel(List<ConfigServerSettings> servers)
        {

        }

        public List<ConfigServerSettings> Servers { get; set; }



    }

    public class ServerViewModel
    {
        public ServerViewModel()
        { }

        
    }
}
