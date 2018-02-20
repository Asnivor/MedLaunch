using MedLaunch.Classes.IO;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;

namespace MedLaunch.Classes
{
    public class ControllerInfo
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public ControllerType Type { get; set; }
    }

    public enum ControllerType
    {
        DirectInput,
        XInput
    }

    /// <summary>
    /// Parse the Mednafen stdout.txt log file
    /// </summary>
    public class LogParser
    {
        #region Static Instance

        /// <summary>
        /// Static instance of the logparser object
        /// </summary>
        public static LogParser Instance { get; set; }

        /// <summary>
        /// Initialises the single instance of logparser
        /// </summary>
        public static void Init()
        {
            Instance = new LogParser();
        }

        #endregion


        #region Properties

        /// <summary>
        /// The full path to stdout.txt
        /// </summary>
        public string LogPath { get; set; }

        /// <summary>
        /// The full path to mednafen.exe
        /// </summary>
        public string MednafenEXE { get; set; }

        /// <summary>
        /// If set to TRUE, data will be retrieved (either from console or stdout.txt) when a
        /// method call is made
        /// If FALSE then state data will be returned
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Set to true when medlaunch starts
        /// </summary>
        public bool IsInit = true;

        public bool IsNewFormat = true;

        /// <summary>
        /// Contains the full output returned when querying mednafen
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// The version string of the currently targeted mednafen
        /// </summary>
        public string VersionString { get; set; }

        /// <summary>
        /// Object that holds the current mednafen version info
        /// </summary>
        private MednafenVersionDescriptor medVersionDesc;
        public MednafenVersionDescriptor MedVersionDesc
        {
            get { return medVersionDesc; }
            set { medVersionDesc = value; }
        }


        List<ControllerInfo> Controllers = new List<ControllerInfo>();

        #endregion

        #region Construction

        public LogParser()
        {
            IsDirty = false;           

        }

        #endregion

        /// <summary>
        /// Attempts to parse data from either stdout or console
        /// </summary>
        public void ParseData()
        {
            // check whether this is the first parse or not (so it has to be forced)
            if (IsInit)
            {
                IsDirty = true;
                IsInit = false;
            }
                

            Paths paths = Paths.GetPaths();
            if (paths != null)
            {
                LogPath = paths.mednafenExe + @"\stdout.txt";
                MednafenEXE = paths.mednafenExe + @"\mednafen.exe";
            }

            if (!IsDirty)
                return;

            // With mednafen >= 1.21.0 we can now call mednafen from an existing console and get
            // the required output from the console itself - try this method first
            // if no data is returned check stdout.txt
            if (File.Exists(MednafenEXE))
            {
                // first try new method
                string args = "\"" + MednafenEXE + "\" EmptyTriggerConsole";

                var conProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        //Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
            }
                };

                // set the environment variable to hide popups
                conProcess.StartInfo.EnvironmentVariables["MEDNAFEN_NOPOPUPS"] = "1";

                conProcess.Start();
                int procId = conProcess.Id;

                conProcess.StandardInput.WriteLine(args);
                conProcess.StandardInput.Flush();
                conProcess.StandardInput.Close();
                
                Output = string.Empty;

                while (!conProcess.StandardOutput.EndOfStream)
                {
                    string line = conProcess.StandardOutput.ReadLine();
                    Output += line;
                    Output += "\n";
                }

                if (!Output.Contains("Starting Mednafen"))
                {
                    // no output detected - try old method
                    Output = string.Empty;

                    //Thread.Sleep(500);

                    var winProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = MednafenEXE,
                            Arguments = "EmptyTriggerWindow",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = false
                        }
                    };

                    // set the environment variable to hide popups
                    winProcess.StartInfo.EnvironmentVariables["MEDNAFEN_NOPOPUPS"] = "1";

                    winProcess.Start();
                    winProcess.WaitForExit();

                    // attempt to read from stdout.txt
                    // check whether stdout.txt doesnt exist or not
                    if (!File.Exists(LogPath))
                    {
                        Thread.Sleep(10);
                        ParseData();
                        Thread.Sleep(10);

                        if (!File.Exists(LogPath))
                        {
                            Output = string.Empty;
                        }
                        else
                        {
                            var ar = FileAndFolder.StreamAllLines(LogPath);
                            foreach (var a in ar)
                            {
                                Output += a + "\n";
                            }
                        }
                    }
                    else
                    {
                        var ar = FileAndFolder.StreamAllLines(LogPath);
                        foreach (var a in ar)
                        {
                            Output += a + "\n";
                        }
                    }
                }

                // attempt to parse the output
                List<string> list = Output.Replace("\r", "\n").Split('\n').ToList();

                if (list.Count() < 1 || list == null)
                {
                    // no data
                    return;
                }

                // get version info
                string versionLine = (from a in list
                                      where a.Contains(" Mednafen ")
                                      select a).FirstOrDefault();

                if (versionLine != null && versionLine.Trim() != "")
                {
                    // split line
                    string[] spl = versionLine.Split(new string[] { "Mednafen " }, StringSplitOptions.None);

                    // get the last item in the array (the version number)
                    VersionString = spl.Last().Trim();

                    // process version number
                    MedVersionDesc = MednafenVersionDescriptor.ReturnVersionDescriptor(VersionString);
                }

                // check whether this is a new type of mednafen or not
                if (!MedVersionDesc.IsNewFormat)
                {
                    // we need to run the detection again for the benefit of the joysticks
                    // (without the -MEDNAFEN_NOPOPUPS param)
                    IsNewFormat = false;
                    ParseData();
                }
                else
                {
                    // get joystick IDs
                    Controllers.Clear();
                    var lines = list.Where(a => a.Contains("ID: ")).ToList();

                    foreach (var l in lines)
                    {
                        ControllerInfo ci = new ControllerInfo();

                        if (l.ToLower().Contains("xinput") ||
                            l.ToLower().Contains("XBOX 360"))
                        {
                            // mednafen has probably detected this as an xinput controller
                            ci.Type = ControllerType.XInput;
                        }
                        else
                        {
                            // mednafen has probably detected this as a directinput controller
                            ci.Type = ControllerType.DirectInput;
                        }

                        // split the string up
                        string[] arr = l.TrimStart().Replace("ID: ", "").Split(new string[] { " - " }, StringSplitOptions.None);
                        string ID = arr[0].TrimStart('0').TrimStart('x');
                        string Name = arr[1].Trim();

                        ci.ID = ID;
                        ci.Name = Name;

                        Controllers.Add(ci);
                    }
                }

                IsDirty = false;
            }
        }

        /*
        /// <summary>
        /// Attempts to parse data from either stdout or console (ignores the dirty flag)
        /// </summary>
        public void ParseDataForce()
        {
            IsDirty = true;
            ParseData();
        }
        */

        /// <summary>
        /// Returns mednafen version info
        /// </summary>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
        public MednafenVersionDescriptor GetMednafenVersion(bool forceUpdate)
        {
            if (MedVersionDesc == null)
            {
                IsDirty = true;
                ParseData();
                return MedVersionDesc;
            }

            if (IsDirty)
            {
                IsDirty = true;
                ParseData();
            }
            else if (forceUpdate)
            {
                IsDirty = true;
                ParseData();
            }
            else
            {
                IsDirty = false;
                ParseData();
            }
                

            return MedVersionDesc;
        }

        /// <summary>
        /// Returns attached controllers
        /// </summary>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
        public List<ControllerInfo> GetAttachedControllers(bool forceUpdate)
        {
            if (IsDirty || forceUpdate)
                ParseData();

            return Controllers;
        }

        /*
        // methods
        public static ControllerInfo[] GetXInputControllerIds()
        {
            EmptyLoad();
            var lines = ReadLog().Where(a => a.TrimStart().StartsWith("Joystick "));
            List<string> onlyDi = new List<string>();

            List<ControllerInfo> list = new List<ControllerInfo>();

            foreach (string l in lines)
            {
                if (!l.Contains("XInput Unknown Controller"))
                    continue;

                string trimmed = l.Trim();

                // get unique ID
                ControllerInfo ci = new ControllerInfo();
                string[] arr = trimmed.Split(new string[] { " - " }, StringSplitOptions.None);
                ci.Name = arr[1];
                ci.ID = arr[2].Replace("Unique ID: ", "");

                list.Add(ci);
            }

            return list.ToArray();
        }

        public static ControllerInfo[] GetDirectInputControllerIds()
        {
            EmptyLoad();
            var lines = ReadLog().Where(a => a.TrimStart().StartsWith("Joystick "));
            List<string> onlyDi = new List<string>();

            List<ControllerInfo> list = new List<ControllerInfo>();

            foreach (string l in lines)
            {
                if (l.Contains("XInput Unknown Controller") || l.Contains("XBOX 360"))
                    continue;

                string trimmed = l.Trim();

                // get unique ID
                ControllerInfo ci = new ControllerInfo();
                string[] arr = trimmed.Split(new string[] { " - " }, StringSplitOptions.None);
                ci.Name = arr[1];
                ci.ID = arr[2].Replace("Unique ID: ", "");

                list.Add(ci);
            }

            return list.ToArray();
        }
        */
        /*
        /// <summary>
        /// Return the current Mednafen version
        /// </summary>
        /// <returns></returns>
        public static string GetMednafenVersion()
        {
            string versionLine = (from a in ReadLog()
                                  where a.Contains(" Mednafen ")
                                  select a).FirstOrDefault();
            if (versionLine == null || versionLine.Trim() == "")
                return null;

            // split line
            string[] spl = versionLine.Split(new string[] { "Mednafen " }, StringSplitOptions.None);
            // get the last item in the array (the version number)
            string vNum = spl.Last().Trim();

            return vNum;
        }

        /// <summary>
        /// Read the contents of the stdout.txt log file into a List<string>
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadLog()
        {
            LogParser lp = new LogParser();

            // check whether stdout.txt doesnt exist or not
            if (!File.Exists(lp.LogPath))
            {
                EmptyLoad();

                if (!File.Exists(lp.LogPath))
                {
                    return new List<string>();
                }
            }
            else
            {
                // it does exist - emptyload anyways (in case the version has changed)
                EmptyLoad();
            }
            

            string[] arr = FileAndFolder.StreamAllLines(lp.LogPath); // File.ReadAllLines(lp.LogPath);
            return arr.ToList();
        }

        /// <summary>
        /// Launches mednafen with an invalid rom path in order to generate stdout.txt
        /// if controllers are plugged in we should be able to get their unique IDs now
        /// </summary>
        public static void EmptyLoad()
        {
            LogParser lp = new LogParser();

            if (File.Exists(lp.MednafenEXE)) // File.Exists(lp.LogPath) && 
            {
                Process medproc = new Process();
                medproc.StartInfo.FileName = lp.MednafenEXE;
#if DEBUG
                medproc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
#else
                medproc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif
                medproc.StartInfo.Arguments = "EmptyTrigger";
                medproc.Start();
                medproc.WaitForExit();
            }
            
        }

    */
    }
}
