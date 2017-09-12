using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.IO;

namespace MiscTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Compression.ProcessArchive(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.zip", new string[] { ".sms", ".7z" });


            Console.WriteLine("Asni's Archive Streaming Test (zip)");
            Console.WriteLine("1. Stream each file in an archive");
            Console.WriteLine("2. Calculate CRC32 hash of file");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.WriteLine("Input File: Sega - Master System - Mark III.7z (42.52MB)");
            Console.WriteLine("");

            Console.WriteLine("Starting PhysFS Test...");
            string phy = InitTest.testPhys();
            Console.Write(phy);
            Console.WriteLine();
            Console.WriteLine();
            /*
            Console.WriteLine("Starting System.IO.Compression Test...");
            string sysIO = InitTest.testDotNet();
            Console.Write(sysIO);
            Console.WriteLine();
            Console.WriteLine();
            */
            /*

            Console.WriteLine("Starting SharpCompress Test...");
            string sc = InitTest.testSharpComp();
            Console.Write(sc);
            */

            Console.ReadKey();
        }
    }
}
