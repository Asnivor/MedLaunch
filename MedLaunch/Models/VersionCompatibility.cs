using MedLaunch.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class VersionCompatibility
    {
        public static List<MednafenChangeHistory> ChangeHistory { get; set; }

        public string CurrentMednafenVersion { get; set; }
        public string LaunchString { get; set; }

        public VersionCompatibility()
        {
            // set change history
            ChangeHistory = new List<MednafenChangeHistory>
            {
                // 0.9.43
                new MednafenChangeHistory
                {
                    Version = "0.9.43",
                    Changes = new List<VersionChange>
                    {

                    }
                },

                // 0.9.42
                new MednafenChangeHistory
                {
                    Version = "0.9.42",
                    Changes = new List<VersionChange>
                    {
                        new VersionChange { Description = "ss multitap 1", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.sport1.multitap" },
                        new VersionChange { Description = "ss multitap 2", ChangeMethod = ChangeType.ToRemove, Item = "ss.input.sport2.multitap" },
                    }
                },

                // 0.9.41
                new MednafenChangeHistory
                {
                    Version = "0.9.41",
                    Changes = new List<VersionChange>
                    {
                        new VersionChange { Description = "snes_faust multitap 1", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.sport1.multitap" },
                        new VersionChange { Description = "snes_faust multitap 2", ChangeMethod = ChangeType.ToRemove, Item = "snes_faust.input.sport2.multitap" },
                    }
                },

                // 0.9.39
                new MednafenChangeHistory
                {
                    Version = "0.9.39",
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

            

            // get mednafen version from disk
            string medVer = LogParser.GetMednafenVersion();
            // take only the first 3 parts of the version (x.x.x)
            if (medVer != null && medVer.Contains('.'))
            {
                string[] breakdown = medVer.Split('.');
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 3; i++)
                {
                    sb.Append(breakdown[i]);
                    if (i < 2)
                        sb.Append(".");
                }
                CurrentMednafenVersion = sb.ToString();
            }            
        }

        public static string GetCompatLaunchString(string launchParams)
        {
            VersionCompatibility VC = new VersionCompatibility();
            string working = launchParams;

            bool isVersionValid = Versions.MednafenVersionCheck(false);
            if (isVersionValid == false)
            {
                // skip processing
                return working;
            }

            // iterate through version changes
            foreach (MednafenChangeHistory c in ChangeHistory)
            {
                // process changes
                foreach (var change in c.Changes)
                {
                    StringBuilder sb = new StringBuilder();
                    switch (change.ChangeMethod)
                    {
                        case ChangeType.ToRemove:               // explicitly remove the entire command
                            string[] arr = working.Split('-');
                            foreach (string s in arr)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append("-" + s);
                            }
                            working = sb.ToString();
                            break;

                        case ChangeType.ToRemoveCompletely:
                            string[] arr2 = working.Split('-');
                            foreach (string s in arr2)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append("-" + s);
                            }
                            working = sb.ToString();                            
                            break;

                        case ChangeType.ToRename:
                            string[] arr3 = working.Split('-');
                            foreach (string s in arr3)
                            {
                                if (!s.Contains(change.Item))
                                    sb.Append("-" + s);
                                else
                                {
                                    sb.Append("-" + s.Replace(change.Item, change.ChangeItem));
                                }
                            }
                            working = sb.ToString();
                            break;

                        case ChangeType.ToAdd:
                            // currently not used
                            break;
                    }
                }

                working = working.TrimStart('-');

                if (VC.CurrentMednafenVersion == c.Version)
                {
                    // we have reached the targeted version and all transformations should have been applied
                    break;
                }
            }
            return working;
        }
    }

    public class MednafenChangeHistory
    {
        public string Version { get; set; }
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
}
