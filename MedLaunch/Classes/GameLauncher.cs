using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedLaunch.Common;
using System.IO;
using MedLaunch.Classes.IO;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowScrape;
using WindowScrape.Constants;
using WindowScrape.Static;
using WindowScrape.Types;
using MedLaunch.Classes.Scanning;
using MedLaunch.Common.IO.Compression;

namespace MedLaunch.Classes
{
    public class GameLauncher
    {
        public GameLauncher()
        {}

        private MyDbContext db;

        public MainWindow mw { get; set; }

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

            // do PSX sbi check and check whether game file actually exists (as it might have been renamed)
            if (SystemId == 9 && File.Exists(game.gamePath))
            {
                // get all implied files from othe cue/m3u that is in the database
                string cuePath = game.gamePath;     // this is never relative with disc-based games
                DiscGameFile originalCue = new DiscGameFile(cuePath, 9);

                List<DiscGameFile> imageFiles = new List<DiscGameFile>(); // DiscScan.ParseTrackSheetForImageFiles(new DiscGameFile(cuePath, 9), 9);

                // check whether m3u
                if (originalCue.Extension.ToLower() == ".m3u")
                {
                    // get all cue files
                    var allc = DiscScan.ParseTrackSheet(originalCue, CueType.m3u, SystemId);
                    foreach (var g in allc)
                    {
                        imageFiles.Add(g);
                    }
                }
                else
                {
                    // standard cue file
                    imageFiles.Add(originalCue);
                }

                // iterate through each image and check for serial number
                for (int i = 0; i < imageFiles.Count; i++)
                {
                    string serial = MedDiscUtils.GetPSXSerial(imageFiles[i].FullPath);

                    if (serial == null || serial == "")
                        continue;

                    // add serial to imageFiles
                    imageFiles[i].ExtraInfo = serial;
                }

                // if imageFile has only one entry, then this matches originalCue
                if (imageFiles.Count == 1)
                { 
                    if (PsxSBI.IsSbiAvailable(imageFiles.First().ExtraInfo) == true)
                    {
                        // sbi is available - check whether sbi already exists
                        string sbipath = imageFiles.First().FullPath.Replace(imageFiles.First().Extension, ".sbi");
                        
                        //if (!File.Exists(imageFiles.First().FolderPath + "\\" + imageFiles.First().FileName.Replace(imageFiles.First().Extension, "") + ".sbi"))

                        if (!File.Exists(sbipath))
                        {
                            MessageBoxResult result = MessageBox.Show("MedLaunch has determined that you need an available SBI patch file to play this game properly.\n\nDo you wish to copy this file to your disc directory?\n",
                                "SBI Patch Needed - " + imageFiles.First().FileName, MessageBoxButton.YesNo, MessageBoxImage.Question);

                            if (result == MessageBoxResult.Yes)
                            {
                                // copy sbi file to folder (named the same as the cue file)
                                originalCue.ExtraInfo = imageFiles.First().ExtraInfo;

                                //PsxSBI.InstallSBIFile(originalCue);
                                PsxSBI.InstallSBIFile(imageFiles.First());
                            }
                        }                   
                    }
                }

                // if imageFiles has multiple entries - it will have come from an m3u file
                if (imageFiles.Count > 1)
                {
                    // create an array of m3u cue files
                    string[] cues = File.ReadAllLines(originalCue.FullPath);

                    // loop through
                    for (int image = 0; image < imageFiles.Count; image++)
                    {
                        if (PsxSBI.IsSbiAvailable(imageFiles[image].ExtraInfo) == true)
                        {
                            // sbi is available - prompt user
                            if (!File.Exists(imageFiles[image].FolderPath + "\\" + imageFiles[image].FileName.Replace(imageFiles[image].Extension, "") + ".sbi"))
                            {
                                MessageBoxResult result = MessageBox.Show("MedLaunch has determined that you need an available SBI patch file to play this game properly.\n\nDo you wish to copy this file to your disc directory?\n",
                                    "SBI Patch Needed - " + imageFiles[image].FileName + imageFiles[image].Extension, MessageBoxButton.YesNo, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    // copy sbi file to folder (named the same as the cue file)
                                    DiscGameFile d = new DiscGameFile(cues[image], 9);
                                    d.ExtraInfo = imageFiles[image].ExtraInfo;

                                    PsxSBI.InstallSBIFile(d);
                                }
                            }
                        }
                    }
                }
            }

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
                    SystemId = 7;
                }

            }

            gSystem = GSystem.GetSystems().Where(a => a.systemId == SystemId).Single();
            SystemCode = gSystem.systemCode;
           
            
            
            RomFolder = GetRomFolder(SystemId, db);

            MednafenFolder = (from m in db.Paths
                              select m.mednafenExe).SingleOrDefault();

            // set the config id
            int actualConfigId = SystemId + 2000000000;

            // take general settings from base config (2000000000) and system specific settings from actual config



            ConfigBaseSettings _config = (from c in db.ConfigBaseSettings
                                          where (c.ConfigId == actualConfigId)
                                          select c).SingleOrDefault();
            
            List<ConfigObject> sysConfigObject = ListFromType(_config).Where(a => !a.Key.StartsWith("__")).ToList();
            SysConfigObject = new List<ConfigObject>();

            foreach (var x in sysConfigObject)
            {
                var systems = GSystem.GetSystems().Where(a => a.systemCode != SystemCode);
                bool isValid = true;
                foreach (var sc in systems)
                {
                    if (x.Key.StartsWith(sc.systemCode + "__"))
                    {
                        isValid = false;
                        break;
                    }                    
                }
                if (isValid == true)
                {
                    SysConfigObject.Add(x);
                }
            }


            // if option is enabled save system specific config for this system
            if (Global.saveSystemConfigs == true)
            {
                if (SystemCode == "pcecd")
                    SystemCode = "pce";

                SaveSystemConfigToDisk(SystemCode, SysConfigObject);
            }


            // build actual config list
            //ConfObject = new List<ConfigObject>();
            //ConfObject.AddRange(GenConfigObject);
            //ConfObject.AddRange(SysConfigObject);

            /*
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
            */


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

        public static void SaveSystemConfigToDisk(string systemCode, List<ConfigObject> sysConfigObject)
        {
            var paths = Paths.GetPaths();
            string filePath = paths.mednafenExe + @"\" + systemCode + ".cfg";

            // build config file string
            StringBuilder sb = new StringBuilder();
            foreach (var t in sysConfigObject)
            {
                // Check for Null and Empty values
                if (t.Value == null || t.Value.ToString().Trim() == "")
                {
                    // null or empty value - continue
                    continue;
                }

                // convert all values to strings
                string v = t.Value.ToString();
                // convert SQL bool values to 0 and 1 strings
                if (t.Value.ToString() == "True")
                {
                    v = "1";
                }
                if (t.Value.ToString() == "False")
                {
                    v = "0";
                }


                if (t.Key == "ConfigId" || t.Key == "UpdatedTime" || t.Key == "isEnabled" || t.Key == "systemIdent" || t.Key.Contains("__enable"))
                {
                    // non-mednafen config settings or settings that must be added as a - param
                    continue;
                }

                string k = t.Key.Replace("__", ".");

                // is the parameter illegal (ie. in the list of illegal config parameters)
                if (IsParameterLegal(k) == false)
                {
                    // illegal - break out of the loop and continue to next parameter
                    continue;
                }
                sb.Append(k);
                sb.Append(" ");
                sb.Append(v);
                sb.Append("\r\n");
            }

            // save to disk
            File.WriteAllText(filePath, sb.ToString());
        }
        

        public void UpdateLastPlayed()
        {

        }

        public static void CopyLaunchStringToClipboard(int gId)
        {
            // create new GameLauncher instance
            GameLauncher gl = new GameLauncher(gId);
            string configCmdString = gl.GetCommandLineArguments();
            string fullString = "\"" + gl.BuildMednafenPath(gl.MednafenFolder) + "\"" + configCmdString;
            Clipboard.SetDataObject(fullString);
        }

        public void RunGame(string cmdArguments, int systemId)
        {
            /*
            // check mednafen.exe instruction set
            InstructionSet medInst = InstructionSetDetector.GetExeInstructionSet(BuildMednafenPath(MednafenFolder));
            // get operating system type            
            InstructionSet osInst = InstructionSetDetector.GetOperatingSystemInstructionSet();

            if (osInst == InstructionSet.x86)
            {
                if (medInst == InstructionSet.x64)
                {
                    MessageBox.Show("You are targetting a 64-bit version of Mednafen on a 32-bit operating system. This will not work.\n\nPlease target a 32-bit (x86) version of Mednafen", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (systemId == 13 && medInst == InstructionSet.x86)
                {
                    MessageBox.Show("You are trying to emulate a Sega Saturn game using a 32-bit Mednafen build on a 32-bit operating system. This will not work.\n\nYou are unable to emulate Saturn games on this machine", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (systemId == 13)
                {
                    MessageBox.Show("You are trying to emulate a Sega Saturn game using a 32-bit operating system. This will not work.\n\nYou are unable to emulate Saturn games on this machine", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            if (osInst == InstructionSet.x64)
            {
                if (systemId == 13 && medInst == InstructionSet.x86)
                {
                    MessageBox.Show("You are trying to emulate a Sega Saturn game using a 32-bit Mednafen build. This will not work.\n\nPlease target a 64-bit (x64) version of Mednafen", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            */

            int procId;

            bool rememberWinPos = GlobalSettings.GetGlobals().rememberSysWinPositions.Value;

            using (System.Diagnostics.Process gProcess = new System.Diagnostics.Process())
            {
                gProcess.StartInfo.UseShellExecute = true;
                gProcess.StartInfo.RedirectStandardOutput = false;
                gProcess.StartInfo.WorkingDirectory = "\"" + MednafenFolder + "\"";
                gProcess.StartInfo.FileName = "\"" + BuildMednafenPath(MednafenFolder) + "\"";
                gProcess.StartInfo.CreateNoWindow = false;
                // Build command line config arguments
                gProcess.StartInfo.Arguments = cmdArguments;
                gProcess.Start();
                
                gProcess.WaitForInputIdle();

                procId = gProcess.Id;
                IntPtr hwnd = new IntPtr();
                // set windows position
                System.Drawing.Point pnt = new System.Drawing.Point();

                // get process window handle
                try
                {
                    hwnd = gProcess.MainWindowHandle;
                    if (rememberWinPos == true)
                    {
                        // get windows position from database
                        pnt = GlobalSettings.GetWindowPosBySystem(systemId);
                        // set windows position
                        HwndInterface.SetHwndPos(hwnd, pnt.X, pnt.Y);
                    }

                    bool isClosed = false;
                    while (isClosed == false)
                    {
                        try
                        {
                            // get process id
                            Process p = Process.GetProcessById(procId);

                            if (rememberWinPos == true)
                            {
                                // get window top left x y coords
                                pnt = HwndInterface.GetHwndPos(hwnd);
                            }
                        }
                        catch
                        {
                            isClosed = true;
                        }

                        Thread.Sleep(1000);
                    }

                    if (rememberWinPos == true)
                    {
                        // save coords to database
                        GlobalSettings.SaveWindowPosBySystem(systemId, pnt);
                    }
                }
                catch
                {
                    // catch exception if mednafen doesnt launch correctly
                    return; 
                }
            }
        }

        public string GetCommandLineArguments()
        {
            string sep = " ";

            // space after mednafen.exe
            string baseStr = sep;
            
            // Get a Dictionary object with a list of config parameter names, along with the object itself
            //Dictionary<string, object> cmds = DictionaryFromType(Config);

            // Create a new List object to hold actual parameters that will be passed to Mednafen
            List<ConfigKeyValue> activeCmds = new List<ConfigKeyValue>();
            foreach (var thing in SysConfigObject)
            {
                // Check for Null and Empty values
                if (thing.Value == null || thing.Value.ToString().Trim() == "")
                {
                    // null or empty value - continue
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


                if (thing.Key == "ConfigId" || thing.Key == "UpdatedTime" || thing.Key == "isEnabled" || thing.Key == "systemIdent" || thing.Key.Contains("__enable"))
                {
                    // non-mednafen config settings or settings that must be added as a - param
                }
                else
                {                    
                    // convert key to correct mednafen config format
                    string k = thing.Key.Replace("__", ".");                                                         
                    
                    // is the parameter illegal (ie. in the list of illegal config parameters)
                    if (IsParameterLegal(k) == false)
                    {
                        // illegal - break out of the loop and continue to next parameter
                        continue;
                    }

                    // add command to activeCmds
                    ConfigKeyValue ckv = new ConfigKeyValue();
                    ckv.Key = k;
                    ckv.Value = v;
                    activeCmds.Add(ckv);      
                }                
            }

            // build parameter string
            foreach (var pair in activeCmds)
            {
                baseStr += "-" + pair.Key + sep + Validate(pair.Value) + sep;
            }      
            
            // add netplay settings
            if (Global.enableNetplay == true)
            {
                // add -connect switch
                baseStr += "-connect" + sep;
            

                // console font
                baseStr += "-netplay.console.font " + Validate(Netplay.netplay__console__font) + sep;
                // console lines
                baseStr += "-netplay.console.lines " + Validate(Netplay.netplay__console__lines.ToString()) + sep;
                // console scale
                baseStr += "-netplay.console.scale " + Validate(Netplay.netplay__console__scale.ToString()) + sep;
                // localplayers
                baseStr += "-netplay.localplayers " + Validate(Netplay.netplay__localplayers.ToString()) + sep;
                // nickname
                baseStr += "-netplay.nick " + Validate(Netplay.netplay__nick) + sep;

                // add server settings

                // host
                if (Server.netplay__host == null || Server.netplay__host.Trim() == "")
                {
                    // ignore
                }
                else
                {
                    // host has been set
                    baseStr += "-netplay.host " + Validate(Server.netplay__host) + sep;
                }

                // port
                if (Server.netplay__port == null || Server.netplay__port.ToString().Trim() == "")
                {
                    // no port set
                }
                else
                {
                    baseStr += "-netplay.port " + Validate(Server.netplay__port.ToString()) + sep;
                }
            
                // add password and gamekey fields from Server row

                // password
                if (Server.netplay__password == null || Server.netplay__password.Trim() == "")
                {
                    // no password set - apply blank
                    baseStr += "-netplay.password \"\""  + sep;
                }
                else
                {
                    baseStr += "-netplay.password " + Validate(Server.netplay__password) + sep;
                }

                // gamekey
                if (Server.netplay__gamekey == null || Server.netplay__gamekey.Trim() == "")
                {
                    // no gamekey set - set blank
                    baseStr += "-netplay.gamekey \"\"" + sep;
                }
                else
                {
                    baseStr += "-netplay.gamekey " + Validate(Server.netplay__gamekey) + sep;
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

            // perform mednafen version check and replace/remove config options that are not viable
            baseStr = Versions.GetCompatLaunchString(baseStr);

            // add gamepath to command line
            baseStr += "\"" + BuildFullGamePath(RomFolder, RomPath) + "\"";
            //MessageBox.Show(baseStr);


            

            return baseStr;
        }

        public static string Validate(string s)
        {
            // check whether there are spaces in the incoming string - if so surround the string in "" marks
            if (s.Contains(" "))
            {
                return "\"" + s + "\"";
            }
            else
                return s;
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

        public static bool IsParameterLegal(string param)
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

        public string BuildFullGamePath(string GamesFolder, string GamePath)
        {
            string path = string.Empty;
            string extension = string.Empty;

            // check whether relative or absolute path has been set in the database for this game
            if (RomPath.StartsWith("."))
            {
                // path is relative (rom autoimported from defined path) - build path
                path = GamesFolder + GamePath.TrimStart('.');
            }
            else
            {
                // rom or disk has been manually added with full path - return just full path
                path = GamePath;
            }

            // create cache directory if it does not exist
            string cacheDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\Cache";
            Directory.CreateDirectory(cacheDir);

            /* now do archive processing */
            if (path.Contains("*/"))
            {
                // this is an archive file with a direct reference to a ROM inside
                string[] arr = path.Split(new string[] { "*/" }, StringSplitOptions.None);
                string archivePath = arr[0];
                string archiveFile = arr[1];

                // copy specific rom from archive to cache folder and get cache rom path
                string cacheRom = Archive.ExtractFile(archivePath, archiveFile, cacheDir); //Archiving.SetupArchiveChild(archivePath, archiveFile, cacheDir);

                if (cacheRom != string.Empty)
                {
                    return cacheRom;
                }

            }
            else if (path.ToLower().Contains(".7z") || path.ToLower().Contains(".zip"))
            {
                // This is a standard archive with no defined link
                extension = System.IO.Path.GetExtension(path).ToLower();

                if (extension == ".zip") //zip
                {
                    // this should be a zip file with a single rom inside - pass directly to mednafen as is
                    return path;
                }

                else if (extension == ".7z")
                {
                    /* archive detected - extract contents to cache directory and modify path variable accordingly
                     * this is a legacy option really - in case people have a single rom in a .7z file in their library that 
                     * has not been directly referenced in the gamepath */

                    // inspect the archive
                    Archive arch = new Archive(path);
                    var res = arch.ProcessArchive(null);

                    // iterate through
                    List<CompressionResult> resList = new List<CompressionResult>();
                    foreach (var v in res.Results)
                    {
                        if (GSystem.IsFileAllowed(v.RomName, SystemId))
                        {
                            resList.Add(v);
                        }
                    }

                    if (resList.Count == 0)
                        return path;

                    // there should only be one result - take the first one
                    var resFinal = resList.FirstOrDefault();

                    // extract the rom to the cache
                    string cacheFile = Archive.ExtractFile(path, resFinal.InternalPath, cacheDir);

                    /*
                    // check whether file already exists in cache
                    string cacheFile = cacheDir + "\\" + resFinal.FileName;
                    if (File.Exists(cacheFile))
                    {
                        // file exists - check size
                        long length = new System.IO.FileInfo(cacheFile).Length;

                        if (length != filesize)
                        {
                            arch.ExtractArchive(cacheDir);
                        }
                    }
                    else
                    {
                        // extract contents of archive to cache folder
                        arch.ExtractArchive(cacheDir);
                    }
                    */

                    // return the new path to the rom
                    return cacheFile;
                }

                else if (extension == ".zip") //zip
                {
                    // this should be a zip file with a single rom inside - pass directly to mednafen as is
                    return path;
                }
            }
            

            return path;
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


        public static List<ConfigObject> ListFromType(object atype)
        {
            if (atype == null) return new List<ConfigObject>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<ConfigObject> dict = new List<ConfigObject>();
            foreach (PropertyInfo prp in props)
            {
                ConfigObject co = new ConfigObject();
                object value = prp.GetValue(atype, new object[] { });
                co.Key = prp.Name;
                co.Value = value;
                dict.Add(co);
            }
            return dict;
        }

        public static string SelectGameFile()
        {
            // get all game system allowed file extensions  
            var systems = GSystem.GetSystems();
            List<string> exts = new List<string>();
            foreach (var s in systems)
            {
                string sup = s.supportedFileExtensions;
                string[] spl = sup.Split(',');
                if (spl.Length > 0)
                {
                    foreach (string e in spl)
                    {
                        exts.Add(e);
                    }
                }
            }

            // remove duplicates
            exts.Distinct();
            // add ZIP extension
            exts.Add(".zip");

            // convert allowed types to filter string
            string filter = "";
            string fStart = "Allowed Types (";
            string fEnd = "";
            foreach (string i in exts)
            {
                if (i == "") { continue; }

                fStart += "*" + i + ",";
                fEnd += "*" + i + ";";
            }
            char comma = ',';
            char semi = ';';
            filter = (fStart.TrimEnd(comma)) + ")|" + (fEnd.TrimEnd(semi));
            //MessageBox.Show(filter);

            // open the file dialog showing only allowed file types - multi-select disabled
            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Multiselect = false;
            filePath.Filter = filter;
            filePath.Title = "Select a single ROM, Disc or playlist file to run with Mednafen";
            filePath.ShowDialog();

            if (filePath.FileName.Length > 0)
            {
                // file has been selected
                string file = filePath.FileName;
                return file;
            }
            else
            {
                // no files selected - return empty string
                return null;
            }
        }

        /// <summary>
        /// Rename all [system].cfg files in the mednafen directory
        /// </summary>
        public static void SystemConfigsOff()
        {
            // get a list of systems
            var systems = from s in GSystem.GetSystems()
                          select s.systemCode.ToLower();

            // check the mednafen directory exists before proceeding
            if (Paths.isMednafenPathValid() == false)
                return;

            string medpath = Paths.GetPaths().mednafenExe;

            // iterate through each system name
            foreach (string sys in systems)
            {
                // check for system.cfg
                if (File.Exists(medpath + "\\" + sys + ".cfg"))
                {
                    // does a backup already exist? if so delete it
                    if (File.Exists(medpath + "\\" + sys + ".cfgBackup"))
                    {
                        File.Delete(medpath + "\\" + sys + ".cfgBackup");
                    }

                    // rename system.cfg
                    File.Move(medpath + "\\" + sys + ".cfg", medpath + "\\" + sys + ".cfgBackup");
                }
            }

        }

        /// <summary>
        /// Name back all [system].cfg files in the mednafen directory
        /// </summary>
        public static void SystemConfigsOn()
        {
            // get a list of systems
            var systems = from s in GSystem.GetSystems()
                          select s.systemCode.ToLower();

            // check the mednafen directory exists before proceeding
            if (Paths.isMednafenPathValid() == false)
                return;

            string medpath = Paths.GetPaths().mednafenExe;

            // iterate through each system name
            foreach (string sys in systems)
            {
                // check for system.cfgBackup
                if (File.Exists(medpath + "\\" + sys + ".cfgBackup"))
                {
                    // does an original file already exist?
                    if (File.Exists(medpath + "\\" + sys + ".cfg"))
                    {
                        // do nothing
                        return;
                    }

                    // rename system.cfg
                    File.Move(medpath + "\\" + sys + ".cfgBackup", medpath + "\\" + sys + ".cfg");
                }
            }
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
        public ConfigBaseSettings SysConfig { get; set; }
        public List<ConfigObject> ConfObject { get; set; }
        public List<ConfigObject> SysConfigObject { get; set; }
        public List<ConfigObject> GenConfigObject { get; set; }
        public ConfigNetplaySettings Netplay { get; set; }
        public ConfigServerSettings Server { get; set; }
        public ConfigServerSettings ServerOveride { get; set; }
        public GlobalSettings Global { get; set; }
        public GSystem gSystem { get; set; }
    }

    public class ConfigKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ConfigObject
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
