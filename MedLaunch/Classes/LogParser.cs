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
            LogPath = paths.mednafenExe + @"\stdout.txt";
            MednafenEXE = paths.mednafenExe + @"\mednafen.exe";
        }

        // methods

        /// <summary>
        /// Return the current Mednafen version
        /// </summary>
        /// <returns></returns>
        public static string GetMednafenVersion()
        {
            string versionLine = (from a in ReadLog()
                                  where a.Contains("Starting Mednafen ")
                                  select a).FirstOrDefault();
            if (versionLine == null || versionLine.Trim() == "")
                return null;

            return versionLine.Replace("Starting Mednafen ", "").Trim();
        }

        /// <summary>
        /// Read the contents of the stdout.txt log file into a List<string>
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadLog()
        {
            LogParser lp = new LogParser();
            if (!File.Exists(lp.LogPath))
            {
                EmptyLoad();
            }
            if (!File.Exists(lp.LogPath))
            {
                return new List<string>();
            }

            string[] arr = File.ReadAllLines(lp.LogPath);
            return arr.ToList();
        }

        /// <summary>
        /// Launches mednafen with an invalid rom path in order to generate stdout.txt
        /// if controllers are plugged in we should be able to get their unique IDs now
        /// </summary>
        public static void EmptyLoad()
        {
            LogParser lp = new LogParser();
            Process medproc = new Process();
            medproc.StartInfo.FileName = lp.MednafenEXE;
            medproc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            medproc.StartInfo.Arguments = "SDL.dll";
            medproc.Start();
            medproc.WaitForExit();
        }
    }
}
