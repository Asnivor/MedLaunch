using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Asnitech.Launch.Common;

namespace MedLaunch.Classes
{
    public class GameLauncher
    {
        public GameLauncher()
        {}

        private MyDbContext db;

        // Constructor
        public GameLauncher(int gameId)
        {
            db = new MyDbContext();

            GameId = gameId;
             
            // get Game object
            Game game = (from g in db.Game
                         where g.gameId == gameId
                         select g).SingleOrDefault();

            ConfigId = game.configId;
            RomPath = game.gamePath;
            RomName = game.gameName;

            // get globals
            Global = (from g in db.GlobalSettings
                      where g.settingsId == 1
                      select g).SingleOrDefault();

            SystemId = game.systemId;

            // logic for faust & fast
            if (game.systemId == 12)
            {
                if (Global.enableSnes_faust == true)
                {
                    SystemId = 16;
                    //MessageBoxResult result = MessageBox.Show("FAUST DETECTED");
                }
                else
                {
                    SystemId = game.systemId;
                }
            }
            if (game.systemId == 7 || game.systemId == 18)
            {
                if (Global.enablePce_fast == true)
                {
                    SystemId = 17;
                }
                else
                {
                    SystemId = game.systemId;
                }

            }
           
            
            
            RomFolder = GetRomFolder(SystemId, db);

            MednafenFolder = (from m in db.Paths
                              select m.mednafenExe).SingleOrDefault();

            // get GameSystems

            GSystem gSys = new GSystem(SystemId);
            SystemName = gSys.systemName;

            if (gSys.systemId == 18)
            {
                SystemCode = "pce";
            }
            else
            {
                SystemCode = gSys.systemCode;
            }



            // get config

            // check whether system specific config is enabled
            int actualConfigId = 2000000000;
            
            // pcecd check
            if (SystemId == 18)
            {
                actualConfigId = 2000000007;
            }
            else
            {
                actualConfigId = SystemId + 2000000000;
            }
            ConfigBaseSettings _config = (from c in db.ConfigBaseSettings
                      where (c.ConfigId == actualConfigId)
                      select c).SingleOrDefault();
            if (_config.isEnabled == true)
            {
                Config = _config;
            }
            else
            {
                Config = (from c in db.ConfigBaseSettings
                          where c.ConfigId == 2000000000
                          select c).SingleOrDefault();
            }



            // get netplay
            Netplay = (from n in db.ConfigNetplaySettings
                       where n.ConfigNPId == 1
                       select n).SingleOrDefault();

            

            // get server
            Server = (from s in db.ConfigServerSettings
                      where s.ConfigServerId == Global.serverSelected
                      select s).SingleOrDefault();

            // get overide server settings (password and gamekey from custom
            ServerOveride = (from s in db.ConfigServerSettings
                             where s.ConfigServerId == 100
                             select s).SingleOrDefault();   
            
                             
        }

        public void UpdateLastPlayed()
        {

        }

        public void RunGame(string cmdArguments)
        {
            System.Diagnostics.Process gProcess = new System.Diagnostics.Process();
            gProcess.StartInfo.UseShellExecute = true;
            gProcess.StartInfo.RedirectStandardOutput = false;
            gProcess.StartInfo.WorkingDirectory = "\"" + MednafenFolder + "\"";
            gProcess.StartInfo.FileName = "\"" + BuildMednafenPath(MednafenFolder) + "\"";
            gProcess.StartInfo.CreateNoWindow = false;

            // Build command line config arguments
            gProcess.StartInfo.Arguments = cmdArguments;
            //MessageBoxResult result = MessageBox.Show(BuildMednafenPath(MednafenFolder) + " " + BuildFullGamePath(RomFolder, RomPath));

            gProcess.Start();
            gProcess.WaitForExit();
        }

        public string GetCommandLineArguments()
        {
            string sep = " ";

            // space after mednafen.exe
            string baseStr = sep;
            
            // Get a Dictionary object with a list of config parameter names, along with the object itself
            Dictionary<string, object> cmds = DictionaryFromType(Config);
            // Create a new List object to hold actual parameters that will be passed to Mednafen
            Dictionary<String, String> activeCmds = new Dictionary<string, string>();
            foreach (var thing in cmds)
            {
                // Check for Null and Empty values
                if (thing.Value == null || thing.Value.ToString().Trim() == "")
                {
                    // null or empty value - continue out of look
                    continue;
                }

                // convert all values to strings
                string v = thing.Value.ToString();
                // convert SQL bool values to 0 and 1 strings
                if (thing.Value.ToString() == "True")
                {
                    v = "1";
                }
                if (thing.Value.ToString() == "False")
                {
                    v = "0";
                }


                if (thing.Key == "ConfigId" || thing.Key == "UpdatedTime" || thing.Key == "isEnabled" || thing.Key == "systemIdent")
                {
                    // non-mednafen config settings
                }
                else
                {                    
                    // convert key to correct mednafen config format
                    string k = thing.Key.Replace("__", ".");

                    // is this a generic system specific config entry (ie. it has a leading .)? If so prefix SystemCode
                    char[] array = k.ToCharArray();
                    if (array[0] ==  '.')
                    {
                        // append the system code at start
                        k = SystemCode.ToLower() + k;
                    }                    
                    
                    // is the parameter illegal (ie. in the list of illegal config parameters)
                    if (IsParameterLegal(k) == false)
                    {
                        // illegal - break out of the loop and continue to next parameter
                        continue;
                    }

                    // does the parameter NOT contain a system code
                    if(DoesParamContainSystemCode(k) == false)
                    {
                        // non system specific parameter. add to list and continue
                        activeCmds.Add(k, v);
                        continue;
                    }
                    else
                    {
                        // param contains a systemcode
                        // is the base config selected?
                        if (ConfigId == 2000000000)
                        {
                            // base config selected - all parameters must be added
                            activeCmds.Add(k, v);
                            continue;
                        }
                        else
                        {
                            // system specific config is selected - only add commands pertaining to this system
                            if (k.Contains(SystemCode))
                            {
                                // it does
                                activeCmds.Add(k, v);
                                continue;
                            }
                            else
                            {
                                // it does not - skip adding
                                continue;
                            }
                        }
                    }                    
                }                
            }

            // build parameter string
            foreach (var pair in activeCmds)
            {
                baseStr += "-" + pair.Key + sep + pair.Value + sep;
            }      
            
            // add netplay settings
            if (Global.enableNetplay == true)
            {
                // add -connect switch
                baseStr += "-connect" + sep;
            

                // console font
                baseStr += "-netplay.console.font " + Netplay.netplay__console__font + sep;
                // console lines
                baseStr += "-netplay.console.lines " + Netplay.netplay__console__lines.ToString() + sep;
                // console scale
                baseStr += "-netplay.console.scale " + Netplay.netplay__console__scale.ToString() + sep;
                // localplayers
                baseStr += "-netplay.localplayers " + Netplay.netplay__localplayers.ToString() + sep;
                // nickname
                baseStr += "-netplay.nick " + Netplay.netplay__nick + sep;

                // add server settings

                // host
                if (Server.netplay__host == null || Server.netplay__host.Trim() == "")
                {
                    // no host set - ignore
                }
                else
                {
                    // host has been set
                    baseStr += "-netplay.host " + Server.netplay__host + sep;
                }

                // port
                if (Server.netplay__port == null || Server.netplay__port.ToString().Trim() == "")
                {
                    // no port set
                }
                else
                {
                    baseStr += "-netplay.port " + Server.netplay__port + sep;
                }
            
                // add password and gamekey fields from id 100

                // password
                if (ServerOveride.netplay__password == null || ServerOveride.netplay__password.Trim() == "")
                {
                    // no password set
                }
                else
                {
                    baseStr += "-netplay.password " + ServerOveride.netplay__password + sep;
                }

                // gamekey
                if (ServerOveride.netplay__gamekey == null || ServerOveride.netplay__gamekey.Trim() == "")
                {
                    // no gamekey set
                }
                else
                {
                    baseStr += "-netplay.gamekey " + ServerOveride.netplay__gamekey + sep;
                }

            }

            /*
            // password
            baseStr += "-netplay.password " + Server.netplay__password + sep;
            // gamekey
            baseStr += "-netplay.gamekey " + Server.netplay__gamekey + sep;
            */

            // faust 
            if (SystemId == 16)
            {
                // force faust
                baseStr += "-force_module snes_faust" + sep;
            }
            // pce_fast
            if (SystemId == 17)
            {
                // force pce_fast
                baseStr += "-force_module pce_fast" + sep;
            }

            // add gamepath to command line
            baseStr += "\"" + BuildFullGamePath(RomFolder, RomPath) + "\"";
            MessageBox.Show(baseStr);
            return baseStr;
        }

        public bool DoesParamContainSystemCode(string param)
        {
            // convert list of systemcodes to hashset
            var s = (from z in GSystem.GetSystems()
                     select z.systemCode);
            bool itDoes = false;
            foreach (string code in s)
            {
                if (param.Contains(code))
                {
                    itDoes = true;
                    break;
                }
            }
            return itDoes;
        }

        public bool IsParameterLegal(string param)
        {
            // define list of illegal config parameters
            HashSet<String> illegal = new HashSet<string>
            {
                "nes.forcemono",
                "npg.language"
            };
            if (illegal.Contains(param))
            {
                // illegal parameter found
                return false;
            }
            else
            {
                // parameter is potentially legal
                return true;
            }
        }

        public string[] PathChecks()
        {
            List<string> output = new List<string>();

            // build Mednafen EXE path
            string MedFullPath = BuildMednafenPath(MednafenFolder);
            // build full ROM path
            string GameFullPath = BuildFullGamePath(RomFolder, RomPath);

            // Test paths
            if (PathIsValid(MedFullPath) == false)
                output.Add("Mednafen Path is incorrect! - " + MedFullPath);
            if (PathIsValid(GameFullPath) == false)
                output.Add("Game Path is incorrect! - " + GameFullPath);

            if (output != null)
            {
                // there were errors - return
                return output.ToArray();
            }

            string[] nullString = null;
            return nullString;
        }

        private bool PathIsValid(string path)
        {
            if (System.IO.File.Exists(path))
                return true;
            else
                return false;
        }

        private string BuildMednafenPath(string MedFolder)
        {
            return MednafenFolder + ".\\mednafen.exe";
        }

        private string BuildFullGamePath(string GamesFolder, string GamePath)
        {
            // check whether relative or absolute path has been set in the database for this game
            if (RomPath.StartsWith("."))
            {
                // path is relative (rom autoimported from defined path) - build path
                return GamesFolder + GamePath;
            }
            else
            {
                // rom or disk has been manually added with full path - return just full path
                return GamePath;
            }                
        }

        private static string GetRomFolder(int systemId, MyDbContext db)
        {
            string romFolderPath;

            Paths ps = (from p in db.Paths
                            select p).SingleOrDefault();

            switch (systemId)
            {
                case 1:
                    romFolderPath = ps.systemGb;
                    break;
                case 2:
                    romFolderPath = ps.systemGba;
                    break;
                case 3:
                    romFolderPath = ps.systemLynx;
                    break;
                case 4:
                    romFolderPath = ps.systemMd;
                    break;
                case 5:
                    romFolderPath = ps.systemGg;
                    break;
                case 6:
                    romFolderPath = ps.systemNgp;
                    break;
                case 7:
                    romFolderPath = ps.systemPce;
                    break;
                case 8:
                    romFolderPath = ps.systemPcfx;
                    break;
                case 9:
                    romFolderPath = ps.systemPsx;
                    break;
                case 10:
                    romFolderPath = ps.systemSms;
                    break;
                case 11:
                    romFolderPath = ps.systemNes;
                    break;
                case 12:
                    romFolderPath = ps.systemSnes;
                    break;
                case 13:
                    romFolderPath = ps.systemSs;
                    break;
                case 14:
                    romFolderPath = ps.systemVb;
                    break;
                case 15:
                    romFolderPath = ps.systemWswan;
                    break;
                case 16:
                    romFolderPath = ps.systemSnes;
                    break;
                case 17:
                    romFolderPath = ps.systemPce;
                    break;
                default:
                    romFolderPath = ps.mednafenExe;
                    break;
            }
            return romFolderPath;
        }

        public static Dictionary<string, object> DictionaryFromType(object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }


        // Properties

        public int ConfigId { get; private set; }
        public int GameId { get; set; }
        public int SystemId { get; private set; }
        public string SystemCode { get; private set; }
        public string SystemName { get; private set; }
        public string MednafenFolder { get; private set; }
        public string RomFolder { get; private set; }
        public string RomPath { get; private set; }
        public string RomName { get; private set; }
        public ConfigBaseSettings Config { get; set; }
        public ConfigNetplaySettings Netplay { get; set; }
        public ConfigServerSettings Server { get; set; }
        public ConfigServerSettings ServerOveride { get; set; }
        public GlobalSettings Global { get; set; }
    }
}
