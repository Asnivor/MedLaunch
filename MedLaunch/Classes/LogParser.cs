using MedLaunch.Classes.IO;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public class ControllerInfo
    {
        public string Name { get; set; }
        public string ID { get; set; }
    }

    /// <summary>
    /// Parse the Mednafen stdout.txt log file
    /// </summary>
    public class LogParser
    {
        // properties
        public string LogPath { get; set; }
        public string MednafenEXE { get; set; }


        // constructor
        public LogParser()
        {
            Paths paths = Paths.GetPaths();
            if (paths != null)
            {
                LogPath = paths.mednafenExe + @"\stdout.txt";
                MednafenEXE = paths.mednafenExe + @"\mednafen.exe";
            }
            
        }

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
                medproc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                medproc.StartInfo.Arguments = "SDL.dll";
                medproc.Start();
                medproc.WaitForExit();
            }
            
        }
    }
}
