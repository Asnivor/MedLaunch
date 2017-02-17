using MedLaunch.Classes;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                    new ConfigServerSettings { ConfigServerDesc = "Speedvicio's Server (EU)", netplay__host = "speedvicio.dtdns.net", netplay__port = 4046,
                        netplay__password = "", netplay__gamekey = "", ConfigServerId = 100 }
                    // new custom user-defined servers should now automatically start at ID 101
                };
            return servers;
        }

        public static void SaveToDatabase(List<ConfigServerSettings> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.ConfigServerSettings.AsNoTracking().ToList();

                List<ConfigServerSettings> toAdd = new List<ConfigServerSettings>();
                List<ConfigServerSettings> toUpdate = new List<ConfigServerSettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    ConfigServerSettings t = (from a in current
                                               where a.ConfigServerId == g.ConfigServerId
                                               select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.ConfigServerSettings.UpdateRange(toUpdate);
                db.ConfigServerSettings.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        public static void SaveToDatabase(ConfigServerSettings Config)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.ConfigServerSettings.AsNoTracking().ToList();

                List<ConfigServerSettings> toAdd = new List<ConfigServerSettings>();
                List<ConfigServerSettings> toUpdate = new List<ConfigServerSettings>();

                // iterate through the games list and separete out games to be added and games to be updated
                
                ConfigServerSettings t = (from a in current
                                            where a.ConfigServerId == Config.ConfigServerId
                                            select a).SingleOrDefault();
                if (t == null) { toAdd.Add(Config); }
                else { toUpdate.Add(Config); }
              
                db.ConfigServerSettings.UpdateRange(toUpdate);
                db.ConfigServerSettings.AddRange(toAdd);
                db.SaveChanges();
            }
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

        // get server entry from database based on id
        public static ConfigServerSettings GetServer(int serverId)
        {
            using (var srvContext = new MyDbContext())
            {
                var server = (from s in srvContext.ConfigServerSettings
                              where s.ConfigServerId == serverId
                              select s).FirstOrDefault();
                return server;
            }
        }

                        
        public static void PopulateCustomServer()
        {
            var servers = GetServers();
            var server = (from s in servers
                          where s.ConfigServerId == 100
                          select s).SingleOrDefault();

            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            TextBox tbServerDesc = (TextBox)mw.FindName("tbServerDesc");
            TextBox tbHostname = (TextBox)mw.FindName("tbHostname");
            TextBox tbPassword = (TextBox)mw.FindName("tbPassword");
            TextBox tbGamekey = (TextBox)mw.FindName("tbGamekey");
            Slider slServerPort = (Slider)mw.FindName("slServerPort");
            

            tbServerDesc.Text = server.ConfigServerDesc;
            tbHostname.Text = server.netplay__host;
            slServerPort.Value = Convert.ToDouble(server.netplay__port);
            tbPassword.Text = server.netplay__password;
            tbGamekey.Text = server.netplay__gamekey;
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

        public static void DeleteServer(int id)
        {
            using (var cont = new MyDbContext())
            {
                // get the record
                ConfigServerSettings c = (from a in cont.ConfigServerSettings
                                          where a.ConfigServerId == id
                                          select a).FirstOrDefault();

                if (c == null)
                    return;

                cont.ConfigServerSettings.Remove(c);
                cont.SaveChanges();
            }
        }





    }
}
