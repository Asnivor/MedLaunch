using MedLaunch.Classes;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedLaunch.Models
{
    public class ConfigServerSettings
    {
        public int ConfigServerId { get; set; }
        public string ConfigServerDesc { get; set; }
        public String netplay__gamekey { get; set; }
        public String netplay__host { get; set; }
        public String netplay__password { get; set; }
        public int? netplay__port { get; set; }                      // 1 through 65535   

        public static List<ConfigServerSettings> GetServerDefaults()
        {
            List<ConfigServerSettings> servers = new List<ConfigServerSettings>
                {
                    new ConfigServerSettings { ConfigServerDesc = "LocalHost (local server required)", netplay__host = "127.0.0.1", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "" },
                    new ConfigServerSettings { ConfigServerDesc = "EmuParadise EU Server", netplay__host = "mednafen-nl.emuparadise.org", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "" },
                    new ConfigServerSettings { ConfigServerDesc = "EmuParadise US Server", netplay__host = "mednafen-us.emuparadise.org", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "" },
                    new ConfigServerSettings { ConfigServerDesc = "Official Mednafen Server", netplay__host = "netplay.fobby.net", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "" },
                    new ConfigServerSettings { ConfigServerDesc = "", netplay__host = "", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "", ConfigServerId = 100 }
                };
            return servers;
        }

        // return all servers from database
        public static List<ConfigServerSettings> GetServers()
        {
            List<ConfigServerSettings> servers = new List<ConfigServerSettings>();
            using (var srvContext = new MyDbContext())
            {
                var allSrvs = from s in srvContext.ConfigServerSettings
                              select s;
                servers.AddRange(allSrvs);
            }
            return servers;
        }

        // pouplate servers combobox
        public static void PopulateServersCombo(ComboBox cb)
        {
            // get all servers
            List<ConfigServerSettings> servers = GetServers();

            cb.ItemsSource = typeof(Colors).GetProperties();
        }

        // pouplate servers datagrid
        public static void PopulateServersDatagrid(DataGrid cb)
        {
            List<DataGridServersView> srvs = new List<DataGridServersView>();
            var servers = GetServers();

            // iterate through each server entry returned from the database
            foreach (var s in servers)
            {
                DataGridServersView server = new DataGridServersView();
                server.ID = s.ConfigServerId;
                server.Description = s.ConfigServerDesc;
                server.Gamekey = s.netplay__gamekey;
                server.Hostname = s.netplay__host;
                server.Password = s.netplay__password;
                server.Port = s.netplay__port.Value;

                srvs.Add(server);
            }

            cb.ItemsSource = srvs;

        }



        // server radio box - get id
        public static int GetServerRadioId(string rbName)
        {
            int sid = 1;
            switch (rbName)
            {
                case "rbSrv01":
                    // ID1
                    sid = 1;
                    break;
                case "rbSrv02":
                    // ID2
                    sid = 2;
                    break;
                case "rbSrv03":
                    // ID3
                    sid = 3;
                    break;
                case "rbSrv04":
                    // ID4
                    sid = 4;
                    break;
                case "rbSrvCustom":
                    // User Custom
                    sid = 100;
                    break;
                default:
                    sid = 1;
                    break;
            }
            return sid;
        }

        // populate servers radio
        public static void PopulateServersRadio(RadioButton rb)
        {
            var servers = GetServers();
            string rbName = rb.Name;
            //int sid = 1;
            int sid = GetServerRadioId(rbName);


            var server = (from s in servers
                          where s.ConfigServerId == sid
                          select s).SingleOrDefault();

            if (server.ConfigServerId == 100)
            {
                // this is the custom server
            }
            else
            {
                string strBuild = server.ConfigServerDesc + " \n(" + server.netplay__host + ":" + server.netplay__port + " )";
                rb.Content = strBuild;
            }
        }

        public static void PopulateCustomServer(TextBox tbServerDesc, TextBox tbHostname, Slider slServerPort, TextBox tbPassword, TextBox tbGamekey)
        {
            var servers = GetServers();
            var server = (from s in servers
                          where s.ConfigServerId == 100
                          select s).SingleOrDefault();

            tbServerDesc.Text = server.ConfigServerDesc;
            tbHostname.Text = server.netplay__host;
            slServerPort.Value = Convert.ToDouble(server.netplay__port);
            tbPassword.Text = server.netplay__password;
            tbGamekey.Text = server.netplay__gamekey;
        }

        public static void SetSelectedServer(RadioButton rb)
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();
            int sid = GetServerRadioId(rb.Name);

            gs.serverSelected = sid;
            GlobalSettings.SetGlobals(gs);
        }

        public static void GetSelectedServerCheckbox(RadioButton rbSrv01, RadioButton rbSrv02, RadioButton rbSrv03, RadioButton rbSrv04, RadioButton rbSrvCustom)
        {
            GlobalSettings gs = GlobalSettings.GetGlobals();
            int id = gs.serverSelected.Value;

            // wipe values
            rbSrv01.IsChecked = true;
            rbSrv02.IsChecked = true;
            rbSrv03.IsChecked = true;
            rbSrv04.IsChecked = true;
            rbSrvCustom.IsChecked = true;
            rbSrv01.IsChecked = true;

            switch (id)
            {
                case 1:
                    rbSrv01.IsChecked = true;
                    break;
                case 2:
                    rbSrv02.IsChecked = true;
                    break;
                case 3:
                    rbSrv03.IsChecked = true;
                    break;
                case 4:
                    rbSrv04.IsChecked = true;
                    break;
                case 100:
                    rbSrvCustom.IsChecked = true;
                    break;
                default:
                    rbSrv01.IsChecked = true;
                    break;
            }
        }

        public static void SaveCustomServerSettings(TextBox tbServerDesc, TextBox tbHostname, Slider slServerPort, TextBox tbPassword, TextBox tbGameKey)
        {
            ConfigServerSettings sSet = new ConfigServerSettings();
            var servers = GetServers();
            sSet = (from s in servers
                    where s.ConfigServerId == 100
                    select s).SingleOrDefault();

            sSet.ConfigServerId = 100;
            sSet.ConfigServerDesc = tbServerDesc.Text;
            sSet.netplay__host = tbHostname.Text;
            sSet.netplay__password = tbPassword.Text;
            sSet.netplay__gamekey = tbGameKey.Text;
            sSet.netplay__port = Convert.ToInt32(slServerPort.Value);

            SetCustomServer(sSet);
        }
        private static void SetCustomServer(ConfigServerSettings srv)
        {
            using (var context = new MyDbContext())
            {
                context.ConfigServerSettings.Attach(srv);
                var entry = context.Entry(srv);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }





    }
}
