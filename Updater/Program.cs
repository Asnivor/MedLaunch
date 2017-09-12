using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Data;
using System.IO.Compression;
using System.IO;
using Microsoft.Win32;

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
            //int milliseconds = 2000;
            //Thread.Sleep(milliseconds);

            // handle args
            if (args == null || args.Length == 0)
            {
                // no command line arguments specified
                Console.WriteLine("You cannot run this application directly. Press any key to exit.");
                Console.ReadLine();
                Environment.Exit(0);
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
                    Console.WriteLine();
                    Console.WriteLine();
                    //Thread.Sleep(4000);
                }
                if (arg.Contains("/DBU"))
                {
                    string[] arr2 = arg.Split(':');
                    // Database upgrade was specifid
                    Console.WriteLine("Starting MedLaunch database upgrade..");
                    Thread.Sleep(10000);
                    //UpgradeDatabase(arr2[1]);
                }
                if (arg.Contains("/U:"))
                {
                    string[] arr3 = arg.Split(':');
                    if (arr3[1] == "" || arr3[1] == " " || arr3[1] == null)
                    {
                        Console.WriteLine("No upgrade file was specified");
                        StartMedLaunch();
                    }
                    string updaterBase = System.AppDomain.CurrentDomain.BaseDirectory;
                    string medlaunchBase = System.IO.Directory.GetParent(System.IO.Directory.GetParent(updaterBase).FullName).FullName;
                    string upgradeFile = arr3[1];

                    //Console.WriteLine(updaterBase);
                    //Console.WriteLine(medlaunchBase);
                    //Console.WriteLine(upgradeFile);
                   // Console.ReadLine();

                    // extract the update file over the current medlaunch folder

                    try
                    {
                        using (ZipArchive zip = ZipFile.OpenRead(medlaunchBase + "\\Data\\Updates\\" + upgradeFile))
                        {
                           
                            //extract the file
                            Console.WriteLine("Unpacking the release...");
                            zip.ExtractToDirectory(medlaunchBase, true);
                            Console.WriteLine("....Done");
                            Thread.Sleep(1000);
                            StartMedLaunch();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR with zip file: " + upgradeFile);
                        Console.WriteLine(ex.InnerException);
                        //Console.ReadLine();
                        StartMedLaunch();
                    }
                }

            }

        }

        static void StartMedLaunch()
        {
            Console.WriteLine("Retarting MedLaunch in:");
            for (int i = 5; i >= 0; i--)
            {
                Console.WriteLine("...." + i);
                Thread.Sleep(1000);
            }
            string updaterBase = System.AppDomain.CurrentDomain.BaseDirectory;
            string medPath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(updaterBase).FullName).FullName + "\\MedLaunch.exe";
            Process.Start(medPath);
            Environment.Exit(0);
        }
        

        static void CheckForUpdates()
        {
            // still to be implemented
        }

  
    }
}
