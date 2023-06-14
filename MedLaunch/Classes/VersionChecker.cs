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
        /// Stores all the medlaunch changes per mednafen version (n-1)
        /// Other functions can iterate through this list backwards applying all transformations so that
        /// MedLaunch is compatible with the version the user is using
        /// </summary>
        /// <returns></returns>
        public static List<MednafenChangeHistory> GetMednafenCompatibilityMatrix()
        {
            return
                new List<MednafenChangeHistory>
                {
                    // 1.31.0-UNSTABLE
                    new MednafenChangeHistory
                    {
                        Version = "1.31.0-UNSTABLE",
                        DownloadURL = "https://mednafen.github.io/releases/files/mednafen-1.31.0-UNSTABLE-win64.zip",
                        Changes = new List<VersionChange>
                        {
                            /// NO BACKWARDS COMPATIBILITY BEFORE 1.31.0 NOW
                        }
                    },
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

                working = " -" + working.TrimStart('-').Replace("- -", "").Replace("-  -", "").Replace("--", "").Replace("--", "").TrimStart();

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
