using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpPhysFS;
using System.IO;
using MedLaunch.IO;
using System.Security.Cryptography;

namespace MedLaunch.IO
{
    public class InitTest
    {
        public static string testDotNet()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Dictionary<string, string> hashes = new Dictionary<string, string>();

            using (System.IO.Compression.ZipArchive zip = System.IO.Compression.ZipFile.OpenRead(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.zip"))
            {
                
                // iterate through each entry
                foreach (System.IO.Compression.ZipArchiveEntry entry in zip.Entries)
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = entry.Open())
                        {
                            string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                            hashes.Add(entry.Name, h);
                        }
                    }
                      
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return "Number of files scanned: " + hashes.Count() + "\nElapsed MS: " + elapsedMs.ToString() + "\nElapsed Seconds: " + (Convert.ToDouble(elapsedMs) / 1000);
        }
        /*
        public static string testSharpComp()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Dictionary<string, string> hashes = new Dictionary<string, string>();

            var archive = ArchiveFactory.Open(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.zip");

            foreach (SharpCompress.Archives.Zip.ZipArchive entry in archive.Entries)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = entry.op())
                    {
                        string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                        hashes.Add(entry.Key ,h);
                    }
                }
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return "Number of files scanned: " + hashes.Count() + "\nElapsed MS: " + elapsedMs.ToString() + "\nElapsed Seconds: " + (Convert.ToDouble(elapsedMs) / 1000);
        }
        */

        public static string testPhys()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Dictionary<string, string> hashes = new Dictionary<string, string>();

            using (var pfs = new PhysFS(""))
            {
                var supported = pfs.SupportedArchiveTypes();
                pfs.SetWriteDir(@"c:\data\_test");

                pfs.Mount(@"D:\Dropbox\Dropbox\_Games\Emulation\_Roms\Sega - Master System - Mark III\Sega - Master System - Mark III.7z", "test", false);

                var searchPath = pfs.GetSearchPath();
                
                var files = pfs.EnumerateFiles("test/");

                foreach (var file in files)
                {
                    bool isDir = pfs.IsDirectory("test/" + file);

                    if (!isDir)
                    {
                        using (var reader = new BinaryReader(pfs.OpenRead("test/" + file)))
                        {
                            //Streams.SaveStreamToDisk(reader.BaseStream, pfs.GetWriteDir() + @"\" + file);

                            // calculate MD5 hash

                            string hash = Crypto.GetMD5Hash(reader.BaseStream);
                            hashes.Add(file, hash);
                           
                            /*
                            using (var md5 = MD5.Create())
                            {
                                    string h = BitConverter.ToString(md5.ComputeHash(reader.BaseStream)).Replace("-", string.Empty);
                                hashes.Add(file, h);                             
                            }
                            */
                        }
                    }
                }

            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return "Number of files scanned: " + hashes.Count() + "\nElapsed MS: " + elapsedMs.ToString() + "\nElapsed Seconds: " + (Convert.ToDouble(elapsedMs) / 1000);
        }

    }
}
