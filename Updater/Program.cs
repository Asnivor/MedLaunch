using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using MedLaunch.Models;
using MedLaunch.Classes;
using Asnitech.SQLite;
using System.Data;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            // define vars
            string title = @"
----------------------------------------------------
####################################################
################ MedLaunch Updater #################
####################################################
----------------------------------------------------

";

            Console.WriteLine(title);
            int milliseconds = 2000;
            //Thread.Sleep(milliseconds);

            // handle args
            if (args == null || args.Length == 0)
            {
                // no command line arguments specified - start online check for updates
                UpgradeDatabase("MedLaunch.db");
                CheckForUpdates();
            }
            foreach (string arg in args)
            {
                if (arg.Contains("/P:"))
                {
                    // process ID was specified
                    string[] arr = arg.Split(':');
                    Console.WriteLine("Waiting for MedLaunch to terminate (process ID: " + arr[1] +")\nPlease wait......");
                    Process.GetProcessById(int.Parse(arr[1])).WaitForExit();
                    Console.WriteLine("MedLaunch has exited - updater will now continue");
                    //Thread.Sleep(4000);
                }
                if (arg.Contains("/DBU"))
                {
                    string[] arr2 = arg.Split(':');
                    // Database upgrade was specifid
                    Console.WriteLine("Starting MedLaunch database upgrade..");
                    Thread.Sleep(10000);
                    UpgradeDatabase(arr2[1]);
                }

            }

        }

        static void UpgradeDatabase(string dbName)
        {
            // check whether upgrade needs to happen
            /*
            if(DbMigration.CheckVersions() == false)
            {
                // upgrade not needed
                StartMedLaunch();
            }
            */

            Database db = Operations.GetDatabaseObject(AppDomain.CurrentDomain.BaseDirectory + @"Data\Settings\" + dbName);


        }

        static void CheckForUpdates()
        {
            // still to be implemented
        }

        static void StartMedLaunch()
        {
            // still to be implemented
        }
    }
}
