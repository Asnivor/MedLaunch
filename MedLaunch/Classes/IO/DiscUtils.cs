using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MedLaunch.Models;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Readers;

namespace MedLaunch.Classes.IO
{
    public class DiscUtils
    {
        public static string GetPSXSerial(string path)
        {
            // set start position
            int pos = 54112;
            // set read length
            int required = 2000;

            bool found = false;

            string str = null;

            while (!found)
            {
                byte[] by = new byte[required];

                using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    try
                    {
                        // seek to required position
                        b.BaseStream.Seek(pos, SeekOrigin.Begin);

                        // Read the required bytes into a bytearray
                        by = b.ReadBytes(required);
                    }
                    catch
                    {

                    }
                }
                // convert byte array to string
                str = System.Text.Encoding.Default.GetString(by);

                if (str.Contains("cdrom:"))
                    found = true;
                else
                    pos += (required + 1);
            }            

            // split
            string[] arr = str.Split(new string[] { "cdrom:" }, StringSplitOptions.None);
            string[] arr2 = arr[1].Split(new string[] { ";1" }, StringSplitOptions.None);
            string serial = arr2[0].Replace("_", "-").Replace(".", "").TrimStart('\\');

            // split the string
            /*
            string[] arr = str.Split(new string[] { "BOOT = cdrom:" }, StringSplitOptions.None);
            if (arr.Length == 1)
            {
                // string wasnt found
                arr = str.Split(new string[] { "BOOT=cdrom:" }, StringSplitOptions.None);
                if (arr.Length == 1)
                {
                    // still not found - return empty
                    return "";
                }
            }
            string[] arr2 = arr[1].Split(new string[] { ";1" }, StringSplitOptions.None);
            string serial = arr2[0].Replace("_", "-").Replace(".", "").Replace("\\", "");
            */

            return serial;            
        }
    }

    public class PsxSBI
    {
        public static List<string> SBINumbers { get; set; }
        public static string SBIArchivePath { get; set; }
        public static string PS1TitlesPath { get; set; }

        // constructor
        public PsxSBI()
        {
            SBIArchivePath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\SbiFiles.7z";
            PS1TitlesPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ps1titles_us_eu_jp.txt";

            SBINumbers = new List<string>();            

            // get all availble sbi numbers from the archive
            List<string> unprocessed = Archiving.GetSbiListFrom7z(SBIArchivePath);

            // strip extension and braces
            foreach (string s in unprocessed)
            {
                string tmp = s.Replace(".7z", "")
                    .Replace("[", "")
                    .Replace("]", "");

                SBINumbers.Add(tmp);
            }
        }

        public static void InstallSBIFile(DiscGameFile cueFile)
        {
            string sbiDestPath = cueFile.FolderPath + "\\" + cueFile.FileName.Replace(cueFile.Extension, "") + ".sbi";
            string serial = cueFile.ExtraInfo.Split('-')[1];

            if (cueFile.ExtraInfo == null || cueFile.ExtraInfo == "")
                return;

            // open master 7z
            var archive = ArchiveFactory.Open(SBIArchivePath);
            string origname = null;

            ExtractionOptions exo = new ExtractionOptions
            {
                Overwrite = true
            };

            foreach (SevenZipArchiveEntry entry in archive.Entries.Where(a => a.Key.Contains(serial)))
            {
                if (entry.IsDirectory)
                    continue;

                origname = entry.Key;

                //extract to temp dir
                entry.WriteToFile(cueFile.FolderPath + "\\" + origname, exo);
                break;         
            }

            archive.Dispose();

            // now extract the inner 7z and rename to match cue file
            var archiveInner = ArchiveFactory.Open(cueFile.FolderPath + "\\" + origname);
            var e = archiveInner.Entries.FirstOrDefault();

            // extract the sbi file to the game folder naming it correctly
            string sbiname = e.Key;
            e.WriteToFile(sbiDestPath, exo);

            // cleanup
            archiveInner.Dispose();
            File.Delete(cueFile.FolderPath + "\\" + origname);
        }

        public static bool IsSbiAvailable(string psxSerial)
        {
            foreach (string num in SBINumbers)
            {
                if (psxSerial != null && psxSerial.Contains(num.Trim()))
                    return true;
            }
            return false;
        }
    }
}
