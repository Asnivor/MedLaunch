using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common;
using MedLaunch.Common.Eventing;
using MedLaunch.Common.IO.Compression;
using MedLaunch.Common.Eventing.CustomEventArgs;
using System.IO;

namespace MiscTesting
{

    public class Listener
    {
        public void Subscribe(Archive a)
        {
            a.Message += new Archive.MessageHandler(MessageRevieved);
        }
        private void MessageRevieved(Archive a, ProgressDialogEventArgs e)
        {
            Console.WriteLine(e.DialogText);
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            /*
            Archive aa = new Archive(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\test3\test3.7z");
            var ree = aa.ProcessArchive(null);


            Console.ReadKey();
            */

            // file md5
            string fileHash = "";
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\ActRaiser (USA).sfc"))
                {
                    fileHash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            Console.WriteLine("File Hash: " + fileHash);

            // stream md5 - zip
            Archive arc = new Archive(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\ActRaiser (USA).zip");
            var re = arc.ProcessArchive(new string[] { ".sfc" });
            string streamHash = re.Results.FirstOrDefault().MD5;
            Console.WriteLine("Hash (zp): " + streamHash);

            // stream md5 - 7zip
            Archive arc7 = new Archive(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\ActRaiser (USA).7z");
            var re7 = arc7.ProcessArchive(new string[] { ".sfc" });
            string streamHash7 = re7.Results.FirstOrDefault().MD5;
            Console.WriteLine("Hash (7z): " + streamHash7);
            

            // extract zip then calculate
            Archive.ExtractFile(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\ActRaiser (USA).7z", "ActRaiser (USA).sfc", @"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\extractedzip");
            string fileHashextz = "";
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\extractedzip\ActRaiser (USA).sfc"))
                {
                    fileHashextz = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            Console.WriteLine("Zpex Hash: " + fileHashextz);

            // extract 7z then calculate
            Archive.ExtractFile(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\ActRaiser (USA).7z", "ActRaiser (USA).sfc", @"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\extracted");
            string fileHashext7 = "";
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\_md5test\extracted\ActRaiser (USA).sfc"))
                {
                    fileHashext7 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            Console.WriteLine("7zex Hash: " + fileHashext7);

            Console.ReadKey();

            string arcPath = @"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.zip";
            Archive a = new Archive(arcPath);

            Listener l = new Listener();
            l.Subscribe(a);

            var r = a.ProcessArchive(new string[] { ".sms", ".7z" });

            //var result = Archive.ProcessArchive(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.zip", new string[] { ".sms", ".7z" });


            Console.WriteLine("Asni's Archive Streaming Test (zip)");
            Console.WriteLine("1. Stream each file in an archive");
            Console.WriteLine("2. Calculate CRC32 hash of file");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.WriteLine("Input File: Sega - Master System - Mark III.7z (42.52MB)");
            Console.WriteLine("");

            Console.WriteLine("Starting PhysFS Test...");
            //string phy = InitTest.testPhys();
            //Console.Write(phy);
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
