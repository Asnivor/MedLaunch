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

        public static bool MednafenVersionCheck()
        {
            // define current compatible version branch
            string compVersion = CompatibleMednafenBranch();

            // mednafen version check     
            Paths pa = Paths.GetPaths();
            string medFolderPath = pa.mednafenExe;
            string medPath = medFolderPath + @"\mednafen.exe";

            if (!File.Exists(medPath))
            {
                MessageBox.Show("Path to Mednafen is NOT valid\nPlease set this on the Settings tab", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string version = LogParser.GetMednafenVersion();

            if (version == null || version == "")
            {
                MessageBox.Show("There was a problem retreiving the Mednafen version.\nPlease check your paths", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string[] compArr = compVersion.Split('.');
            string[] versionArr = version.Split('.');

            for (int i = 0; i < 4; i++)
            {
                if (i == 3 && compArr[3] == "x")
                {
                    // the 4th digital is 'x' - therefore we skip this check
                    break;
                }

                if (compArr[i] != versionArr[i])
                {
                    // versions do not match - run mednafen once to update the stdout file incase we are looking at the most up to date log file
                    LogParser.EmptyLoad();
                    version = LogParser.GetMednafenVersion();
                    if (version == null || version == "")
                    {
                        MessageBox.Show("There was a problem retreiving the Mednafen version.\nPlease check your paths", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    versionArr = version.Split('.');
                    if (compArr[i] == versionArr[i])
                    {
                        i--;
                        continue;
                    }                      
                    


                    // version doesnt match
                    MessageBoxResult result = MessageBox.Show("The version of Mednafen you are trying to launch is potentially NOT compatible with this version of MedLaunch\n\nMednafen version installed: " + version + "\nMednafen version required: " + compVersion + "\n\nPlease ensure you have the correct version of MedLaunch installed for the version of Mednafen you are trying to run.\n\nThe latest MedLaunch releases will always try to stay compatible with the newest Mednafen release. There is NO backwards compatibility in place.\n\nPress OK to return to the Games Library\nPress CANCEL to ignore these very important messages and try to launch the game (which probably will NOT work anyway)", "Mednafen Version Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (result == MessageBoxResult.OK)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            // if we have gotten this far, the versions seem to match - return true
            return true;
            
        }
        
    }
}
