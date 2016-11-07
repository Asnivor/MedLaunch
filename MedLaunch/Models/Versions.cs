using MedLaunch.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class Versions
    {
        public int versionId { get; set; }
        public string dbVersion { get; set; }

        // return compatible mednafen version branch
        public static string CompatibleMednafenBranch()
        {
            string branch = "0.9.39.x";
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
    }
}
