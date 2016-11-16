using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{
    public class BackupConfig
    {
        public static void BackupMain()
        {
            // backup mednafen config if option is selected
            GlobalSettings gs = GlobalSettings.GetGlobals();
            if (gs.backupMednafenConfig == true)
            {
                // check whether file exists
                Paths p = Paths.GetPaths();
                string medPath = p.mednafenExe;
                string cfgPath = medPath + @"\mednafen-09x.cfg";
                if (File.Exists(cfgPath)) 
                {
                    // get timestamp
                    string timeStamp = DateTime.Now.ToString("yyyyMMdd_HH-mm-ss");
                    // create new filename
                    //string destPath = cfgPath.Replace("mednafen-09x.cfg", "mednafen-09x(MLBackup-" + timeStamp + ").cfg");
                    string backupDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\MednafenCFGBackups";
                    // create directory if it doesnt exist
                    Directory.CreateDirectory(backupDir);
                    string destPath = backupDir + "\\mednafen-09x(MLBackup-" + timeStamp + ").cfg";
                    // create a backup
                    File.Copy(cfgPath, destPath);
                }
            }
        }
    }
}
