using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MedLaunch.Models;
using System.Diagnostics;
using System.Windows;

namespace MedLaunch.Classes
{
    public class VersionChecker
    {
        #region Static Instance

        /// <summary>
        /// Static instance of the versions object
        /// </summary>
        public static VersionChecker Instance { get; set; }

        /// <summary>
        /// Initialises the single instance of logparser
        /// </summary>
        public static void Init()
        {
            Instance = new VersionChecker();
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Returns the download URL for the latest compatible mednafen release
        /// </summary>
        public string LatestCompatMednafenDownloadURL
        {
            get
            {
                return GetMednafenCompatibilityMatrix().First().DownloadURL;
            }
        }

        /// <summary>
        /// Gets a MednafenVersionDescriptor object containing the latest compatible version info
        /// </summary>
        public MednafenVersionDescriptor LatestCompatMedVerDesc
        {
            get
            {
                var desc = GetMednafenCompatibilityMatrix();
                var latest = desc.First();
                return MednafenVersionDescriptor.ReturnVersionDescriptor(latest.Version);
            }
        }

        /// <summary>
        /// Gets a MednafenVersionDescriptor object containing the currently detected mednafen info
        /// </summary>
        public MednafenVersionDescriptor CurrentMedVerDesc
        {
            get
            {
                // for this to work, the database must be already generated
                // and a mednafen directory has been set
                Paths p = Paths.GetPaths();
                if (p != null && p.mednafenExe != null && Directory.Exists(p.mednafenExe))
                {
                    var curr = LogParser.Instance.GetMednafenVersion(false);
                    if (curr == null)
                        return null;
                    return curr;
                    /*
                    try
                    {
                        var curr = LogParser.Instance.GetMednafenVersion(false);
                        return curr;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    */
                }
                else
                    return null;
            }
        }

        public bool IsNewConfig
        {
            get
            {
                return CurrentMedVerDesc.IsNewConfig;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Compatibility Matrix
        /// Stores all the medlaunch changes per mednafen version
        /// Other functions can iterate through this list backwards applying all transformations so that
        /// MedLaunch is compatible with the version the user is using
        /// </summary>
        /// <returns></returns>
        public static List<MednafenChangeHistory> GetMednafenCompatibilityMatrix()
        {
            return
                new List<MednafenChangeHistory>
                {
                    // 1.21.1
                    new MednafenChangeHistory
                    {
                        Version = "1.21.1",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-1.21.1-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            ////////////////////////////////////////////////////////////////
                            // Top level changes - these MUST be moved the beginning of every new version entry
                            // Its a bit of a kludge, but for earlier versions they should get converted back again later on
                            new VersionChange { Description = "sdl depreciated", ChangeMethod = ChangeType.ToRename, Item = "video.driver sdl", ChangeItem = "video.driver softfb" },
                            new VersionChange { Description = "overlay depreciated", ChangeMethod = ChangeType.ToRename, Item = "video.driver overlay", ChangeItem = "video.driver default" },
                            ////////////////////////////////////////////////////////////////


                        }
                    },

                    // 1.21.0-UNSTABLE
                    new MednafenChangeHistory
                    {
                        Version = "1.21.0-UNSTABLE",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-1.21.0-UNSTABLE-win64.zip",
                        Changes = new List<VersionChange>
                        {

                        }
                    },

                    // 0.9.48
                    new MednafenChangeHistory
                    {
                        Version = "0.9.48",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.48-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "Display to use with fullscreen", ChangeMethod = ChangeType.ToRemove, Item = "video.fs.display" },
                            new VersionChange { Description = "sdl depreciated", ChangeMethod = ChangeType.ToRename, Item = "video.driver default", ChangeItem = "video.driver opengl" },
                            new VersionChange { Description = "overlay depreciated", ChangeMethod = ChangeType.ToRename, Item = "video.driver softfb", ChangeItem = "video.driver sdl" },

                            new VersionChange { Description = "fps.autoenable", ChangeMethod = ChangeType.ToRemove, Item = "fps.autoenable" },
                            new VersionChange { Description = "fps.bgcolor", ChangeMethod = ChangeType.ToRemove, Item = "fps.bgcolor" },
                            new VersionChange { Description = "fps.font", ChangeMethod = ChangeType.ToRemove, Item = "fps.font" },
                            new VersionChange { Description = "fps.position", ChangeMethod = ChangeType.ToRemove, Item = "fps.position" },
                            new VersionChange { Description = "fps.scale", ChangeMethod = ChangeType.ToRemove, Item = "fps.scale" },
                            new VersionChange { Description = "fps.textcolor", ChangeMethod = ChangeType.ToRemove, Item = "fps.textcolor" },

                            // for older mednafen versions just change the JP keyboard to US keyboard
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port1 jpkeyboard", ChangeItem = "ss.input.port1 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port2 jpkeyboard", ChangeItem = "ss.input.port2 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port3 jpkeyboard", ChangeItem = "ss.input.port3 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port4 jpkeyboard", ChangeItem = "ss.input.port4 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port5 jpkeyboard", ChangeItem = "ss.input.port5 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port6 jpkeyboard", ChangeItem = "ss.input.port6 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port7 jpkeyboard", ChangeItem = "ss.input.port7 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port8 jpkeyboard", ChangeItem = "ss.input.port8 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port9 jpkeyboard", ChangeItem = "ss.input.port9 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port10 jpkeyboard", ChangeItem = "ss.input.port10 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port11 jpkeyboard", ChangeItem = "ss.input.port11 keyboard" },
                            new VersionChange { Description = "SS JAP Keyboard", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port12 jpkeyboard", ChangeItem = "ss.input.port12 keyboard" },
                        }
                    },

                    // 0.9.47
                    new MednafenChangeHistory
                    {
                        Version = "0.9.47",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.47-win64.zip",
                        Changes = new List<VersionChange>
                        {

                        }
                    },

                    // 0.9.46
                    new MednafenChangeHistory
                    {
                        Version = "0.9.46",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.46-win64.zip",
                        Changes = new List<VersionChange>
                        {

                        }
                    },

                    // 0.9.45.1
                    new MednafenChangeHistory
                    {
                        Version = "0.9.45.1",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.45.1-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port1.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port2.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port3.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port4.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port5.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port6.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port7.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port8.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port9.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port10.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port11.3dpad.mode.defpos" },
                            new VersionChange { Description = "SS 3dpad default mode", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port12.3dpad.mode.defpos" },
                        }
                    },

                    // 0.9.44.1
                    new MednafenChangeHistory
                    {
                        Version = "0.9.44.1",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.44.1-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port1 gun", ChangeItem = "ss.input.port1 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port2 gun", ChangeItem = "ss.input.port2 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port3 gun", ChangeItem = "ss.input.port3 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port4 gun", ChangeItem = "ss.input.port4 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port5 gun", ChangeItem = "ss.input.port5 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port6 gun", ChangeItem = "ss.input.port6 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port7 gun", ChangeItem = "ss.input.port7 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port8 gun", ChangeItem = "ss.input.port8 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port9 gun", ChangeItem = "ss.input.port9 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port10 gun", ChangeItem = "ss.input.port10 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port11 gun", ChangeItem = "ss.input.port11 gamepad" },
                            new VersionChange { Description = "SS Gun", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port12 gun", ChangeItem = "ss.input.port12 gamepad" },

                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port1.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port2.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port3.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port4.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port5.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port6.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port7.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port8.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port9.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port10.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port11.gun_chairs" },
                            new VersionChange { Description = "SS xhair", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port12.gun_chairs" },
                        }
                    },

                    // 0.9.43
                    new MednafenChangeHistory
                    {
                        Version = "0.9.43",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.43-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "M3U Disc Chooser", ChangeMethod = ChangeType.ToRemove, Item = "which_medium" },
                            new VersionChange { Description = "snes_faust correct aspect", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.correct_aspect" },
                            new VersionChange { Description = "VB brightness", ChangeMethod = ChangeType.ToRemove, Item = "vb.ledonscale" },
                            //new VersionChange { Description = "WSWAN rotate input", ChangeMethod = ChangeType.ToRemove, Item = "wswan.rotateinput" },
                        }
                    },

                    // 0.9.42
                    new MednafenChangeHistory
                    {
                        Version = "0.9.42",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.42-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "ss multitap 1", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.sport1.multitap" },
                            new VersionChange { Description = "ss multitap 2", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.sport2.multitap" },
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port1 wheel", ChangeItem = "ss.input.port1 gamepad" },
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRename, Item = "ss.input.port2 wheel", ChangeItem = "ss.input.port1 gamepad" },
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port3"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port4"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port5"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port6"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port7"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port8"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port9"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port10"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port11"},
                            new VersionChange { Description = "ss wheel", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.port12"},
                        }
                    },

                    // 0.9.41
                    new MednafenChangeHistory
                    {
                        Version = "0.9.41",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-0.9.41-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "snes_faust multitap 1", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.sport1.multitap" },
                            new VersionChange { Description = "snes_faust multitap 2", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.sport2.multitap" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port3" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port4" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port5" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port6" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port7" },
                            new VersionChange { Description = "snes_faust extra ports", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.port8" },
                        }
                    },

                    // 0.9.39.2
                    new MednafenChangeHistory
                    {
                        Version = "0.9.39.2",
                        DownloadURL = "http://mednafen.fobby.net/releases/files/mednafen-0.9.39.2-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "rename pixshader", ChangeMethod = ChangeType.ToRename, Item = ".shader", ChangeItem = ".pixshader" },
                            new VersionChange { Description = "goat shader", ChangeMethod = ChangeType.ToRemoveCompletely, Item = "goat" },
                            new VersionChange { Description = "analogmode CT compare", ChangeMethod = ChangeType.ToRemove, Item = "psx.input.analog_mode_ct.compare" },
                            new VersionChange { Description = "analogmode CT", ChangeMethod = ChangeType.ToRemove, Item = "psx.input.analog_mode_ct" },
                            new VersionChange { Description = "snes.h_blend", ChangeMethod = ChangeType.ToRemove, Item = "snes.h_blend" },
                            new VersionChange { Description = "ss.h_blend", ChangeMethod = ChangeType.ToRemove, Item = "ss.h_blend" },
                            new VersionChange { Description = "ss.h_overscan", ChangeMethod = ChangeType.ToRemove, Item = "ss.h_overscan" },
                            new VersionChange { Description = "ss.correct_aspect", ChangeMethod = ChangeType.ToRemove, Item = "ss.correct_aspect" },
                            new VersionChange { Description = "sms.slstart", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstart" },
                            new VersionChange { Description = "sms.slend", ChangeMethod = ChangeType.ToRemove, Item = "sms.slend" },
                            new VersionChange { Description = "sms.slstartp", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstartp" },
                            new VersionChange { Description = "sms.slendp", ChangeMethod = ChangeType.ToRemove, Item = "sms.slendp" },
                            new VersionChange { Description = "sms.slstart", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstart" },
                        }
                    },

                    // 0.9.39.1
                    new MednafenChangeHistory
                    {
                        Version = "0.9.39.1",
                        DownloadURL = "http://mednafen.fobby.net/releases/files/mednafen-0.9.39.1-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            new VersionChange { Description = "rename pixshader", ChangeMethod = ChangeType.ToRename, Item = ".shader", ChangeItem = ".pixshader" },
                            new VersionChange { Description = "goat shader", ChangeMethod = ChangeType.ToRemoveCompletely, Item = "goat" },
                            new VersionChange { Description = "analogmode CT compare", ChangeMethod = ChangeType.ToRemove, Item = "psx.input.analog_mode_ct.compare" },
                            new VersionChange { Description = "analogmode CT", ChangeMethod = ChangeType.ToRemove, Item = "psx.input.analog_mode_ct" },
                            new VersionChange { Description = "snes.h_blend", ChangeMethod = ChangeType.ToRemove, Item = "snes.h_blend" },
                            new VersionChange { Description = "ss.h_blend", ChangeMethod = ChangeType.ToRemove, Item = "ss.h_blend" },
                            new VersionChange { Description = "ss.h_overscan", ChangeMethod = ChangeType.ToRemove, Item = "ss.h_overscan" },
                            new VersionChange { Description = "ss.correct_aspect", ChangeMethod = ChangeType.ToRemove, Item = "ss.correct_aspect" },
                            new VersionChange { Description = "sms.slstart", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstart" },
                            new VersionChange { Description = "sms.slend", ChangeMethod = ChangeType.ToRemove, Item = "sms.slend" },
                            new VersionChange { Description = "sms.slstartp", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstartp" },
                            new VersionChange { Description = "sms.slendp", ChangeMethod = ChangeType.ToRemove, Item = "sms.slendp" },
                            new VersionChange { Description = "sms.slstart", ChangeMethod = ChangeType.ToRemove, Item = "sms.slstart" },
                        }
                    }
                };

        }

        /// <summary>
        /// Gets the dev build version number from the DevStatus.txt file
        /// This is generated automatically by AppVeyor when a dev version is built
        /// MedLaunch uses the data in this file to show that it is a dev version in the title bar
        /// </summary>
        /// <returns></returns>
        public static string GetDevBuild()
        {
            string devStatusFile = AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\DevStatus.txt";
            if (File.Exists(devStatusFile))
            {
                string line = File.ReadAllLines(devStatusFile).FirstOrDefault();
                if (line != null && line != "0")
                {
                    return line;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parses a mednafen launch string using the compatibility matrix and removes/modifies anything
        /// that is not compatible with the user's current mednafen version
        /// </summary>
        /// <param name="launchParams"></param>
        /// <returns></returns>
        public static string GetCompatLaunchString(string launchParams)
        {
            //Versions VC = new Versions();
            string working = launchParams;

            bool isVersionValid = MednafenVersionCheck(false);
            if (isVersionValid == false)
            {
                // skip processing
                return working;
            }

            // iterate through version changes
            foreach (MednafenChangeHistory c in GetMednafenCompatibilityMatrix())
            {
                // process changes
                foreach (var change in c.Changes)
                {
                    StringBuilder sb = new StringBuilder();

                    switch (change.ChangeMethod)
                    {
                        case ChangeType.ToRemove:               // explicitly remove the entire command
                            string[] arr = working.Split(new string[] { " -" }, StringSplitOptions.None);
                            foreach (string s in arr)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append(" -" + s);
                            }
                            working = sb.ToString();
                            break;

                        case ChangeType.ToRemoveCompletely:
                            string[] arr2 = working.Split(new string[] { " -" }, StringSplitOptions.None);
                            foreach (string s in arr2)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append(" -" + s);
                            }
                            working = sb.ToString();
                            break;

                        case ChangeType.ToRename:
                            string[] arr3 = working.Split(new string[] { " -" }, StringSplitOptions.None);
                            foreach (string s in arr3)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append(" -" + s);
                                else
                                {
                                    sb.Append(" -" + s.Replace(change.Item, change.ChangeItem));
                                }
                            }
                            working = sb.ToString();
                            break;

                        case ChangeType.ToAdd:
                            // currently not used
                            break;
                    }
                }

                working = " -" + working.TrimStart('-').Replace("- -", "").Replace("-  -", "").TrimStart();

                string currIntOnly;
                string targetIntOnly;

                var targetDesc = MednafenVersionDescriptor.ReturnVersionDescriptor(c.Version);

                if (Instance.CurrentMedVerDesc.IsNewFormat)
                {
                    currIntOnly = Instance.CurrentMedVerDesc.MajorINT + "." +
                        Instance.CurrentMedVerDesc.MinorINT + "." +
                        Instance.CurrentMedVerDesc.BuildINT;

                    targetIntOnly = targetDesc.MajorINT + "." +
                        targetDesc.MinorINT + "." +
                        targetDesc.BuildINT;
                }
                else
                {
                    currIntOnly = Instance.CurrentMedVerDesc.MajorINT + "." +
                        Instance.CurrentMedVerDesc.MinorINT + "." +
                        Instance.CurrentMedVerDesc.BuildINT + "." +
                        Instance.CurrentMedVerDesc.RevisionINT;

                    targetIntOnly = targetDesc.MajorINT + "." +
                        targetDesc.MinorINT + "." +
                        targetDesc.BuildINT + "." +
                        targetDesc.RevisionINT;
                }

                if (currIntOnly == targetIntOnly)
                {
                    // we have reached the targeted version and all transformations should have been applied
                    break;
                }
            }
            return working;
        }

        /// <summary>
        /// Looks at the VS assembly info and gets the current version data
        /// </summary>
        /// <returns></returns>
        public static string ReturnApplicationVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionMajor = fvi.ProductMajorPart.ToString();
            string versionMinor = fvi.ProductMinorPart.ToString();
            string versionBuild = fvi.ProductBuildPart.ToString();
            string versionPrivate = fvi.ProductPrivatePart.ToString();

            string fVersion = fvi.FileVersion;
            return versionMajor + "." + versionMinor + "." + versionBuild + "." + versionPrivate;
        }

        /// <summary>
        /// Entry point for the application to get mednafen version and 
        /// display compatibility info if neccessary
        /// </summary>
        /// <param name="showDialog"></param>
        /// <returns></returns>
        public static bool MednafenVersionCheck(bool showDialog)
        {
            // mednafen version check     
            Paths pa = Paths.GetPaths();
            string medFolderPath = pa.mednafenExe;
            string medPath = medFolderPath + @"\mednafen.exe";

            if (!File.Exists(medPath))
            {
                if (showDialog)
                    MessagePopper.ShowMessageDialog("Path to Mednafen is NOT valid\nPlease set this on the Settings tab",
                        "ERROR");
                    //MessageBox.Show("Path to Mednafen is NOT valid\nPlease set this on the Settings tab", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            // detect current version
            var currDesc = Instance.CurrentMedVerDesc;
            if (currDesc == null)
            {
                if (showDialog)
                    MessagePopper.ShowMessageDialog("There was a problem retreiving the Mednafen version.\nPlease check your paths",
                        "ERROR");
                //MessageBox.Show("There was a problem retreiving the Mednafen version.\nPlease check your paths", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!currDesc.IsValid)
            {
                if (showDialog)
                    MessagePopper.ShowMessageDialog("There was a problem parsing the current Mednafen version.\nPlease check your paths",
                        "ERROR");
                //MessageBox.Show("There was a problem parsing the current Mednafen version.\nPlease check your paths", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // get the min and max support mednafen versions
            var latestDesc = Instance.LatestCompatMedVerDesc;
            var oldestDesc = MednafenVersionDescriptor.ReturnVersionDescriptor(GetMednafenCompatibilityMatrix().Last().Version);

            // check whether the current mednafen version is within the min and max supported constraints
            // just looking at the first 3 digits of the version
            string currStr = currDesc.MajorINT.ToString() + "." +
                currDesc.MinorINT.ToString() + "." +
                currDesc.BuildINT.ToString();

            var loookup = GetMednafenCompatibilityMatrix()
                .Where(a => a.Version.StartsWith(currStr)).ToList();

            bool isCompat;

            if (loookup.Count() > 0)
            {
                isCompat = true;
            }
            else
            {
                isCompat = false;
            }

            if (isCompat)
            {
                // is compatible
                return true;
            }
            else
            {
                // is not compatible
                if (showDialog)
                {
                    // version doesnt match
                    StringBuilder sb = new StringBuilder();
                    sb.Append("The version of Mednafen you are trying to launch is potentially NOT compatible with this version of MedLaunch.\n\n");
                    sb.Append("Mednafen version installed:\t");
                    sb.Append(currDesc.FullVersionString);
                    sb.Append("\nMednafen version required: \t");
                    sb.Append(oldestDesc.FullVersionString + " - " + latestDesc.FullVersionString);
                    sb.Append("\n\nPlease ensure you are targeting a MedLaunch supported version of Mednafen.\n");
                    sb.Append("\nPress LAUNCH ANYWAY to try your luck");
                    sb.Append("\nPress CANCEL to return to the Games Library");

                    var result = MessagePopper.ShowMessageDialog(sb.ToString(),
                        "POSSIBLE VERSION MISMATCH", MessagePopper.DialogButtonOptions.YESNO, new MahApps.Metro.Controls.Dialogs.MetroDialogSettings
                        {
                            AnimateHide = false,
                            AnimateShow = false,
                            //ColorScheme = MahApps.Metro.Controls.Dialogs.MetroDialogColorScheme.Accented,
                            AffirmativeButtonText = "LAUNCH ANYWAY",
                            NegativeButtonText = "CANCEL",
  
                        });

                    //MessageBoxResult result = MessageBox.Show(sb.ToString(), "Mednafen Version Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (result == MessagePopper.ReturnResult.Affirmative)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        #endregion
    }


    public class MednafenChangeHistory
    {
        public string Version { get; set; }
        public string DownloadURL { get; set; }
        public List<VersionChange> Changes { get; set; }
    }

    public class VersionChange
    {
        public string Description { get; set; }
        public ChangeType ChangeMethod { get; set; }
        public string Item { get; set; }
        public string ChangeItem { get; set; }
    }

    public enum ChangeType
    {
        ToRename,               // rename a specific string
        ToRemove,               // remove an explicit command line option
        ToRemoveCompletely,     // remove entire option where string is matched
        ToAdd                   // add in a command that was previous removed (not currently needed)
    }

    public class MednafenVersionDescriptor
    {
        /// <summary>
        /// The full version string passed in
        /// </summary>
        public string FullVersionString { get; set; }

        /// <summary>
        /// Major
        /// </summary>
        public int? MajorINT { get; set; }
        public string MajorSTR { get; set; }
        /// <summary>
        /// Minor
        /// </summary>
        public int? MinorINT { get; set; }
        public string MinorSTR { get; set; }
        /// <summary>
        /// Build
        /// </summary>
        public int? BuildINT { get; set; }
        public string BuildSTR { get; set; }
        /// <summary>
        /// Revision (as of mednafen >= 1.12.0 this no longer exists)
        /// </summary>
        public int? RevisionINT { get; set; }
        public string RevisionSTR { get; set; }

        /// <summary>
        /// Signs whether version number has 3 or 4 digits
        /// </summary>
        public bool IsNewFormat { get; set; }

        /// <summary>
        /// Signs whether mednafen version >= 1.21.x
        /// </summary>
        public bool IsNewConfig { get; set; }

        /// <summary>
        /// Signs that the version string was parsed successfully
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Creates a version descriptor object from a string
        /// </summary>
        /// <param name="versionString"></param>
        public static MednafenVersionDescriptor ReturnVersionDescriptor(string versionString)
        {
            MednafenVersionDescriptor vd = new MednafenVersionDescriptor();

            vd.FullVersionString = versionString.Trim();
            

            // attempt splitting the string by '.'
            string[] arr = versionString.Split('.');

            int count = arr.Length;

            vd.IsValid = true;

            switch (count)
            {
                // new version format
                case 3:
                    // get each version int, string
                    for (int i = 0; i < count; i++)
                    {
                        var result = ParseVersionInt(arr[i]);

                        if (result.Key == null)
                        {
                            vd.IsValid = false;
                        }

                        switch (i)
                        {
                            case 0:
                                vd.MajorINT = result.Key;
                                vd.MajorSTR = result.Value;
                                break;
                            case 1:
                                vd.MinorINT = result.Key;
                                vd.MinorSTR = result.Value;
                                break;
                            case 2:
                                vd.BuildINT = result.Key;
                                vd.BuildSTR = result.Value;
                                break;
                        }
                    }

                    vd.IsNewFormat = true;
                    break;

                // old version format
                case 4:
                    // get each version int, string
                    for (int i = 0; i < count; i++)
                    {
                        var result = ParseVersionInt(arr[i]);

                        if (result.Key == null)
                        {
                            vd.IsValid = false;
                        }

                        switch (i)
                        {
                            case 0:
                                vd.MajorINT = result.Key;
                                vd.MajorSTR = result.Value;
                                break;
                            case 1:
                                vd.MinorINT = result.Key;
                                vd.MinorSTR = result.Value;
                                break;
                            case 2:
                                vd.BuildINT = result.Key;
                                vd.BuildSTR = result.Value;
                                break;
                            case 3:
                                vd.RevisionINT = result.Key;
                                vd.RevisionSTR = result.Value;
                                break;
                        }
                    }

                    vd.IsNewFormat = false;
                    break;

                // not valid - no more processing should take place
                default:
                    vd.IsValid = false;
                    return vd;
            }

            if (vd.MajorINT > 0)
                vd.IsNewConfig = true;
            else
                vd.IsNewConfig = false;

            return vd;
        }

        private static KeyValuePair<int?, string> ParseVersionInt(string ver)
        {
            // attempt to parse the string
            int i;
            bool result = int.TryParse(ver, out i);

            if (!result)
            {
                // parsing was not successful - attempt to split string again
                // (often mednafen with have a 0-UNSTABLE)
                string[] arr = ver.Split('-');

                int count = arr.Length;

                if (count > 1)
                {
                    // test the first element
                    bool res = int.TryParse(arr[0], out i);

                    if (res)
                    {
                        // an int was parsed from the first element
                        // use this as the int, and use the second as the string
                        return new KeyValuePair<int?, string>(i, arr[1]);
                    }

                    // parsing was not successful - just return a null it value and the string
                    return new KeyValuePair<int?, string>(null, ver);
                }
                else
                {
                    // could not parse
                    return new KeyValuePair<int?, string>(null, ver);
                }
            }
            else
            {
                // parsing was successful
                return new KeyValuePair<int?, string>(i, ver);
            }
        }
    }
}
