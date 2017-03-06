using MedLaunch.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Models
{
    public class Versions
    {
        public int versionId { get; set; }
        public string dbVersion { get; set; }

        // return compatible mednafen version branch
        public static string CompatibleMednafenBranch()
        {
            string branch = "0.9.43.x";
            return branch;
        }

        // compare mednafen versions
        public static bool IsMednafenVersionValid()
        {
            string localbranch = LogParser.GetMednafenVersion();
            string requiredbranch = Versions.CompatibleMednafenBranch();

            string[] lb = localbranch.Trim().Split('.');
            string[] rb = requiredbranch.Trim().Split('.');

            bool isValid = true;
            for (int i = 0; i < 3; i++)
            {
                if (lb[i] != rb[i])
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        // get application version
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

        // get defaults (for initial seed)
        public static Versions GetVersionDefaults()
        {
            Versions v = new Versions
            {
                versionId = 1,
                dbVersion = ReturnApplicationVersion()
            };
            return v;
        }

        // get the database version
        public static string GetVersionString()
        {
            string vStr = GetVersions().dbVersion;
            return vStr;
        }

        // return Versions entry from database
        public static Versions GetVersions()
        {
            Versions v = new Versions();
            using (var context = new MyDbContext())
            {
                var query = from s in context.Versions
                            where s.versionId == 1
                            select s;
                v = query.FirstOrDefault();
            }
            return v;
        }        

        public static bool MednafenVersionCheck(bool showDialog)
        {
            VersionCompatibility vc = new VersionCompatibility();

            // mednafen version check     
            Paths pa = Paths.GetPaths();
            string medFolderPath = pa.mednafenExe;
            string medPath = medFolderPath + @"\mednafen.exe";

            if (!File.Exists(medPath))
            {
                if (showDialog)
                    MessageBox.Show("Path to Mednafen is NOT valid\nPlease set this on the Settings tab", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string version = LogParser.GetMednafenVersion();

            if (version == null || version == "")
            {
                if (showDialog)
                    MessageBox.Show("There was a problem retreiving the Mednafen version.\nPlease check your paths", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string[] compArrMax = VersionCompatibility.ChangeHistory.First().Version.Split('.');
            string[] compArrMin = VersionCompatibility.ChangeHistory.Last().Version.Split('.');
            string[] versionArr = version.Split('.');

            // mednafen version we are targeting MUST be within the MIN and MAX mednafen versions supported
            for (int i = 0; i < 3; i++)
            {
                // convert to ints
                int compMin = Convert.ToInt32(compArrMin[i]);
                int compMax = Convert.ToInt32(compArrMax[i]);
                int medVer = Convert.ToInt32(versionArr[i]);

                if (medVer < compMin || medVer > compMax)
                {
                    if (showDialog)
                    {
                        // version doesnt match
                        StringBuilder sb = new StringBuilder();
                        sb.Append("The version of Mednafen you are trying to launch is potentially NOT compatible with this version of MedLaunch\n\nMednafen version installed: ");
                        sb.Append(version);
                        sb.Append("\nMednafen version required: ");
                        sb.Append(VersionCompatibility.ChangeHistory.Last().Version + " - " + VersionCompatibility.ChangeHistory.First().Version);
                        sb.Append("\n\nPlease ensure you are targeting a MedLaunch supported version of Mednafen.");
                        sb.Append("\n\nPress OK to return to the Games Library");
                        sb.Append("\nPress CANCEL to ignore these very important messages and try to launch the game (which probably will NOT work anyway)");

                        MessageBoxResult result = MessageBox.Show(sb.ToString(), "Mednafen Version Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        if (result == MessageBoxResult.OK)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                        
                }
            }

            // if we have gotten this far, the versions seem to match - return true
            return true;
            
        }
        
    }
}
