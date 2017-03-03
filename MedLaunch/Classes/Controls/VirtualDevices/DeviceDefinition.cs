using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static bool WriteDefinitionToConfigFile(List<Mapping> maps)
        {
            try
            {
                // load the whole mednafen config into an array
                string[] lines = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen-09x.cfg").ToArray();

                // iterate through maps
                for (int i = 0; i < maps.Count; i++)
                {
                    // check whether this matches something in lines
                    for (int line = 0; line < lines.Length; line++)
                    {
                        if (lines[line].Contains(maps[i].MednafenCommand + " "))
                        {
                            // match is found - rewrite this line
                            lines[line] = maps[i].MednafenCommand + " " + maps[i].Config;
                        }
                    }                
                }

                // now write the updated config back to disk            
                File.WriteAllLines(Paths.GetPaths().mednafenExe + @"\mednafen-09x.cfg", lines);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// populate DeviceDefinition object with config strings from mednafen config
        /// </summary>
        /// <param name="deviceDef"></param>
        public static void PopulateConfig(DeviceDefinition deviceDef)
        {
            // get all config strings from the mednafen config file that match the 'CommandStart' property of deviceDef
            List<string> cfgs = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen-09x.cfg").Where(a => a.Contains(deviceDef.CommandStart)).ToList();

            // iterate through each mapping and set the config string
            for (int i = 0; i < deviceDef.MapList.Count; i++)
            {
                string cName = deviceDef.MapList[i].MednafenCommand;

                // look up the config command in the string list
                string lookup = (from a in cfgs
                                 where a.Contains(cName + " ")
                                 select a).FirstOrDefault();

                if (lookup == null || lookup == "" || lookup == " ")
                {
                    // command wasnt found
                    continue;
                }

                // if there is no config set for this command - continue
                if (!lookup.Contains(" "))
                    continue;

                // command was found - get just the config details
                string[] arr = lookup.Split(' ');

                // build the new string without the command
                StringBuilder sb = new StringBuilder();
                for (int c = 1; c < arr.Length; c++)
                {
                    sb.Append(arr[c]);
                    sb.Append(" ");
                }
                string cfg = sb.ToString().TrimEnd();

                // update deviceDef with the config
                deviceDef.MapList[i].Config = cfg;
            }
        }
    }

    public class Mapping
    {
        public string MednafenCommand { get; set; }
        public string Description { get; set; }
        public string Config { get; set; }

        
    }
}
