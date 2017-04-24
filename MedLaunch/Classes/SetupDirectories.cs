using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MedLaunch.Classes
{
    class SetupDirectories
    {
        // create required directories in application folder if they do not exist

        public static void Go()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] directories = new string[] {
                "Data",
                "Data\\Graphics",
                "Data\\Graphics\\Icons",
                "Data\\Graphics\\Systems",
                "Data\\Settings",
                "Data\\System"
            };

        foreach (string d in directories)
        {
            try
            {
                string path = currentDirectory + "\\" + d;
                Directory.CreateDirectory(path);
            }
            finally
            {
                // completed
            }
        }

        // move the updater app to .\lib folder
        if (File.Exists(currentDirectory + "\\Updater.exe"))
        {
            // updater.exe is in the base directory - this happens directly after build
            // move it to the lib directory
            try
                {
                    if (File.Exists(currentDirectory + "\\lib\\Updater.exe"))
                        File.Delete(currentDirectory + "\\lib\\Updater.exe");
                    File.Move(currentDirectory + "\\Updater.exe", currentDirectory + "\\lib\\Updater.exe");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
        }

            // move the updater app config to .\lib folder
            if (File.Exists(currentDirectory + "\\Updater.exe.config"))
            {
                // updater.exe is in the base directory - this happens directly after build
                // move it to the lib directory
                try
                {
                    if (File.Exists(currentDirectory + "\\lib\\Updater.exe.config"))
                        File.Delete(currentDirectory + "\\lib\\Updater.exe.config");
                    File.Move(currentDirectory + "\\Updater.exe.config", currentDirectory + "\\lib\\Updater.exe.config");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }


        }
    }
}
