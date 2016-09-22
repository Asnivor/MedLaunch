using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public class DataGridServersView
    {
        public DataGridServersView()
        { }

        public DataGridServersView(int ConfigServerId, string ConfigServerDesc, string netplay__gamekey, string netplay__host, string netplay__password, int netplay__port)
        {
            this.ID = ConfigServerId;
            this.Description = ConfigServerDesc;
            this.Hostname = netplay__host;
            this.Port = netplay__port;
            this.Password = netplay__password;
            this.Gamekey = netplay__gamekey;
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Gamekey { get; set; }

    }
}
