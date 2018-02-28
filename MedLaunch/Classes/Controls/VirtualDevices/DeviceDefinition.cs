using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class DeviceDefinition : IDeviceDefinition
    {
        /// <summary>
        /// Friendly device name
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// The start of the mednafen command (eg, psx.input.port)
        /// </summary>        
        public string CommandStart { get; set; }
        /// <summary>
        /// The port number specified in the config
        /// </summary>
        public int VirtualPort { get; set; }
        /// <summary>
        /// The next part of the command string (eg, gamepad)
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// List of mapping objects (auto-generated from the mednafen config)
        /// </summary>
        public List<Mapping> MapList { get; set; }
        /// <summary>
        /// Custom mapping list - specifies explicit config options that are controller/virtual port related
        /// but are not button mappings
        /// </summary>
        public List<Mapping> CustomOptions { get; set; }
        /// <summary>
        /// An ordering list that is processed to order the mapping before display
        /// </summary>
        public List<string> CustomOrdering { get; set; }
        /// <summary>
        /// An array that contains each line of the mednafen config file
        /// </summary>
        public string[] ConfigList { get; set; }

        public DeviceDefinition()
        {
            MapList = new List<Mapping>();
            ConfigList = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen.cfg").ToArray();
        }        

        /// <summary>
        /// Automatically works out available config control options from mednafen.cfg
        /// </summary>
        /// <param name="maps"></param>
        public static void ParseOptionsFromConfig(DeviceDefinition dd)
        {
            dd.MapList = new List<Mapping>();

            // step through every line
            for (int i = 0; i < dd.ConfigList.Length; i++)
            {
                if (dd.ConfigList[i].TrimStart().StartsWith((dd.CommandStart + "." + dd.ControllerName + ".").Replace("..", ".")))
                {
                    // ignore these
                    if (dd.ConfigList[i].ToLower().Contains("default position for"))
                        continue;

                    if (dd.ConfigList[i].ToLower().Contains(" axis scale coefficient for "))
                        continue;

                    // this line is needed
                    Mapping map = new Mapping();

                    string[] arr = dd.ConfigList[i].Split(' ');
                    if (arr.Length < 1)
                        continue;

                    map.MednafenCommand = arr[0];

                    // now get description from previous line
                    string prev = dd.ConfigList[i - 1];

                    // ignore these
                    if (prev.ToLower().Contains("default position for"))
                        continue;

                    if (prev.ToLower().Contains(" axis scale coefficient for "))
                        continue;

                    string[] descSplit = prev.Split(new string[] { ", " }, StringSplitOptions.None);
                    if (descSplit.Length < 1)
                    {
                        map.Description = prev;
                    }
                    else if (descSplit.Length > 2)
                    {
                        string res = string.Empty;
                        for (int s = 1; s < descSplit.Length; s++)
                        {
                            res += descSplit[s];
                            if (s != descSplit.Length - 1)
                                res += ", ";
                        }
                        map.Description = res;
                    }
                    else
                    {
                        string last = descSplit.Last();
                        map.Description = last;
                    }

                    // attempt to remove the beginning nonsense
                    string[] n = map.Description.Split(new string[] { ": " }, StringSplitOptions.None);
                    if (n.Length < 2)
                        map.Description = map.Description;
                    else
                        map.Description = n.Last();

                    // add to maplist
                    dd.MapList.Add(map);
                }
            }

            dd.PerformOrdering();
            //dd.MapList = dd.MapList.OrderBy(a => a.Description).ToList();
        }

        public static bool WriteDefinitionToConfigFile(List<Mapping> maps)
        {
            try
            {
                // load the whole mednafen config into an array
                string[] lines = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen.cfg").ToArray();

                // iterate through maps
                for (int i = 0; i < maps.Count; i++)
                {
                    // check whether this matches something in lines
                    for (int line = 0; line < lines.Length; line++)
                    {
                        if (lines[line].Contains(maps[i].MednafenCommand + " "))
                        {
                            // match found

                            // add the command
                            StringBuilder sb = new StringBuilder();
                            sb.Append(maps[i].MednafenCommand + " ");

                            // add the blocks
                            if (maps[i].Primary != null)
                            {
                                sb.Append(maps[i].Primary.DeviceType.ToString().ToLower() + " ");
                                sb.Append(maps[i].Primary.DeviceID + " ");
                                sb.Append(maps[i].Primary.Config + " ");

                                if (maps[i].Primary.Scale != null && 
                                    maps[i].Primary.Scale.Trim() != "")
                                    sb.Append(maps[i].Primary.Scale + " ");

                                if (maps[i].Primary.LogicString != null && 
                                    maps[i].Primary.LogicString.Trim() != "" 
                                    && maps[i].Secondary != null)
                                    sb.Append(maps[i].Primary.LogicString + " ");

                                if (maps[i].Secondary != null)
                                {
                                    sb.Append(maps[i].Secondary.DeviceType.ToString().ToLower() + " ");
                                    sb.Append(maps[i].Secondary.DeviceID + " ");
                                    sb.Append(maps[i].Secondary.Config + " ");

                                    if (maps[i].Secondary.Scale != null && 
                                        maps[i].Secondary.Scale.Trim() != "")
                                        sb.Append(maps[i].Secondary.Scale + " ");

                                    if (maps[i].Secondary.LogicString != null && 
                                        maps[i].Secondary.LogicString.Trim() != "" && 
                                        maps[i].Tertiary != null)
                                        sb.Append(maps[i].Secondary.LogicString + " ");

                                    if (maps[i].Tertiary != null)
                                    {
                                        sb.Append(maps[i].Tertiary.DeviceType.ToString().ToLower() + " ");
                                        sb.Append(maps[i].Tertiary.DeviceID + " ");
                                        sb.Append(maps[i].Tertiary.Config + " ");

                                        if (maps[i].Tertiary.Scale != null && 
                                            maps[i].Tertiary.Scale.Trim() != "")
                                            sb.Append(maps[i].Tertiary.Scale + " ");

                                        /*
                                        if (maps[i].Tertiary.LogicString.Trim() != "")
                                            sb.Append(maps[i].Tertiary.LogicString + " ");
                                            */

                                    }
                                }
                            }

                            lines[line] = sb.ToString();                                      
                        }
                    }                
                }

                // now write the updated config back to disk            
                File.WriteAllLines(Paths.GetPaths().mednafenExe + @"\mednafen.cfg", lines);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// populate DeviceDefinition object with config strings from mednafen config
        /// </summary>
        /// <param name="deviceDef"></param>
        public static void PopulateConfig(IDeviceDefinition deviceDef)
        {
            // get all config strings from the mednafen config file that match the 'CommandStart' property of deviceDef
            List<string> cfgs = File.ReadAllLines(Paths.GetPaths().mednafenExe + @"\mednafen.cfg").Where(a => a.Contains(deviceDef.CommandStart)).ToList();

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

                if (arr.Length < 3)
                {
                    // command is not set
                    continue;
                }

                // get the string again without the command
                string justBindings = lookup.Replace(cName, "").TrimStart();
                arr = justBindings.Split(' ');

                // our array should now have the full config command - skip the first index (mednafen command)
                // and iterate through - each 'block'

                // denotes the position within the string
                // i.e. if logic functions are involved
                int location = 0;

                int lastLength = 0;

                // calculate the blocks lengths
                int blockPri = 0;
                int blockSec = 0;
                int blockTer = 0;

                int blockIndex = 0;
                bool isFin = false;

                for (int cou = 0; cou < arr.Length; cou++)
                {
                    if (isFin)
                        break;

                    switch (blockIndex)
                    {
                        case 0:
                            if (arr[cou] == "||" || arr[cou] == "&&" || arr[cou] == "&!")
                            {
                                // this is a logic operator
                                blockIndex++;
                            }
                            blockPri++;
                            break;
                        case 1:
                            if (arr[cou] == "||" || arr[cou] == "&&" || arr[cou] == "&!")
                            {
                                // this is a logic operator
                                blockIndex++;
                            }
                            blockSec++;
                            break;
                        case 2:
                            if (arr[cou] == "||" || arr[cou] == "&&" || arr[cou] == "&!")
                            {
                                // this is a logic operator
                                blockIndex++;
                            }
                            blockTer++;
                            break;
                        default:
                            // we dont support more than 3
                            isFin = true;
                            break;
                    }                    
                }

                // now process each block
                if (blockPri > 2)
                {
                    string[] blockPriArr = arr.Take(blockPri).ToArray();
                    deviceDef.MapList[i].Primary = ProcessBlock(blockPriArr);

                }
                if (blockSec > 2)
                {
                    string[] blockSecArr = arr.Skip(blockPri).Take(blockSec).ToArray();
                    deviceDef.MapList[i].Secondary = ProcessBlock(blockSecArr);
                }
                if (blockTer > 2)
                {
                    string[] blockTerArr = arr.Skip(blockPri + blockSec).Take(blockTer).ToArray();
                    deviceDef.MapList[i].Tertiary = ProcessBlock(blockTerArr);
                }

                // update deviceDef with the full config string
                deviceDef.MapList[i].Config = justBindings;
            }
        }

        /// <summary>
        /// processes a block string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Mapping ProcessBlock(string[] arr)
        {
            Mapping map = new Mapping();

            for (int i = 0; i < arr.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        // device type
                        switch (arr[i])
                        {
                            case "keyboard":
                                map.DeviceType = DeviceType.Keyboard;
                                break;
                            case "mouse":
                                map.DeviceType = DeviceType.Mouse;
                                break;
                            case "joystick":
                                map.DeviceType = DeviceType.Joystick;
                                break;
                        }
                        break;
                    case 1:
                        // device id
                        map.DeviceID = arr[i];
                        break;
                    case 2:
                        // mapping
                        map.Config = arr[i];
                        break;
                    case 3:
                        // either SCALE or LOGIC
                        if (arr[i] != "||" && arr[i] != "&&" && arr[i] != "&!")
                        {
                            // it is SCALE
                            map.Scale = arr[i];
                        }
                        else
                        {
                            // it is logic
                            map.LogicString = arr[i];
                        }
                        break;
                    case 4:
                        // probably LOGIC
                        map.LogicString = arr[i];
                        break;
                }
            }

            return map;
        }

        /// <summary>
        /// processes a block string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ProcessBlock(Mapping map)
        {
            string res = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (map != null)
            {
                sb.Append(map.DeviceType.ToString().ToLower() + " ");
                sb.Append(map.DeviceID + " ");
                sb.Append(map.Config + " ");

                //if (map.Scale != null && (map.Scale != "keyboard" && map.Scale != "joystick" && map.Scale != "mouse"))
                    //sb.Append(map.Scale + " ");

                /*
                if (map.LogicString != null)
                    sb.Append(map.LogicString);
                    */

                res = sb.ToString();
            }

            return res;            
        }

        /// <summary>
        /// Method attempts to order the maplist against a rough outline
        /// </summary>
        public void PerformOrdering()
        {
            // Rough idea of ordering
            // non-matches should be placed at the end
            CustomOrdering = new List<string>
            {
                "UP ↑",
                "DOWN ↓",
                "LEFT ←",
                "RIGHT →",

                "Left Stick UP ↑",
                "Left Stick DOWN ↓",
                "Left Stick LEFT ←",
                "Left Stick RIGHT →",
                "Left Stick, Button(L3)",

                "Right Stick UP ↑",
                "Right Stick DOWN ↓",
                "Right Stick LEFT ←",
                "Right Stick RIGHT →",
                "Right Stick, Button(R3)",

                "D-Pad UP ↑",
                "D-Pad DOWN ↓",
                "D-Pad LEFT ←",
                "D-Pad RIGHT →",

                "Analog UP ↑",
                "Analog DOWN ↓",
                "Analog LEFT ←",
                "Analog RIGHT →",

                "Stick FORE",
                "Stick BACK",
                "Stick LEFT",
                "Stick RIGHT",

                "L Stick FORE",
                "L Stick BACK",
                "L Stick LEFT",
                "L Stick RIGHT",
                "L Throttle Up",
                "L Throttle Down",

                "R Stick FORE",
                "R Stick BACK",
                "R Stick LEFT",
                "R Stick RIGHT",
                "R Throttle Up",
                "R Throttle Down",

                "X1(X UP ↑)",
                "X3(X DOWN ↓)",
                "X4(X LEFT ←)",
                "X2(X RIGHT →)",
                "Y1(Y UP ↑)",
                "Y3(Y DOWN ↓)",
                "Y4(Y LEFT ←)",
                "Y2(Y RIGHT →)",

                "Throttle Up",
                "Throttle Down",

                "L Gear Shift",
                "R Gear Shift",

                "Motion Up",
                "Motion Down",
                "Motion Left",
                "Motion Right",

                "A",
                "B",
                "C",

                "X",
                "Y",
                "Z",

                "Button 1",
                "Button 2",
                "Button 3",
                "Button 4",
                "Button 5",
                "Button 6",

                "I",
                "II",
                "III",
                "IV",
                "V",
                "VI",

                "I (Analog)",
                "II (Analog)",

                "△ (upper)",
                "○ (right)",
                "x (lower)",
                "□ (left)",

                "Fire 1/Start",
                "Fire 2",

                "Offscreen Shot",
                "Cursor",
                "Trigger",
                "Turbo",

                "SHOULDER L",
                "SHOULDER R",

                "Left Shoulder",
                "Right Shoulder",

                "Left-Back",
                "Right-Back",

                "L",
                "R",

                "L1 (front left shoulder)",
                "R1 (front right shoulder)",
                "L2 (rear left shoulder)",
                "R2 (rear right shoulder)",

                "Twist ↑|↓ (Analog, Turn Left)",
                "Twist ↓|↑ (Analog, Turn Right)",

                "Left Button",
                "Middle Button",
                "Right Button",

                "Away Trigger",
                "Trigger",
                "X Axis",
                "Y Axis",

                "SELECT",
                "START",
                "PAUSE",
                "MODE",
                "RUN",
                "Analog(mode toggle)",

                "Autofire Speed",

                "A AF",
                "B AF",
                "C AF",
                "X AF",
                "Y AF",
                "Z AF",
                "L AF",
                "R AF",

                "Rapid A",
                "Rapid B",
                "Rapid C",

                "Rapid X",
                "Rapid Y",
                "Rapid Z",

                "Rapid Button 1",
                "Rapid Button 2",
                "Rapid Button 3",
                "Rapid Button 4",
                "Rapid Button 5",
                "Rapid Button 6",

                "Rapid △",
                "Rapid ○",
                "Rapid x",
                "Rapid □",

                "Rapid Fire 1/Start",
                "Rapid Fire 2",

                "Rapid I",
                "Rapid II",

                "Tilt: UP ↑",
                "Tilt: DOWN ↓",
                "Tilt: LEFT ←",
                "Tilt: RIGHT →",

                "1",
                "2,",
                "3,",
                "4,",
                "5,",
                "6,",
                "7,",
                "8,",
                "9,",
                "10,",
                "11,",
                "12,",
                "13,",
                "14,",
                "15,",
                "16,",
                "17,",
                "18,",
                "19,",
                "20,",
            };


            // get a copy of the map list
            List<Mapping> working = MapList.ToList();

            // destructive copy
            List<Mapping> leftover = new List<Mapping>();

            List<Mapping> final = new List<Mapping>();

            // iterate through the sorting list
            foreach (var s in CustomOrdering)
            {
                // attempt to find matches - exact first
                var map = working.Where(a => a.Description.ToLower() == s.ToLower()).ToList();

                if (map.Count == 1)
                {
                    // exact match found
                    final.Add(map.First());
                    working.Remove(map.First());
                    continue;
                }

                // now try partial match
                var mapP = working.Where(a => a.Description.ToLower().StartsWith(s.ToLower() + " ")).ToList();

                if (mapP.Count == 1)
                {
                    // 1 entry
                    final.Add(mapP.First());
                    working.Remove(mapP.First());
                    continue;
                }
                if (mapP.Count > 1)
                {
                    // multiples found - for now add them all
                    final.AddRange(mapP);
                    foreach (var m in mapP)
                        working.Remove(m);

                    continue;
                }                
            }

            // at this point all that are remaining should be added
            final.AddRange(working);

            MapList.Clear();
            MapList = final;
        }

    }

    
}
