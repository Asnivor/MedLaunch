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

    public class SaturnLookup
    {
        public static List<SaturnGames> SaturnGamesList { get; set; }
        public static string ListPath { get; set; }

        public SaturnLookup()
        {
            if (SaturnGamesList == null)
                SaturnGamesList = new List<SaturnGames>();

            ListPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\SaturnList.txt";

            // parse the txt file
            string[] lines = File.ReadAllLines(ListPath);

            
            /*
            // iterate through each game block
            foreach (string block in games)
            {
                List<string> eachline = new List<string>();

                // iterate through each line
                using (StringReader sr = new StringReader(block))
                {
                    string li;
                    while ((li = sr.ReadLine()) != null)
                    {
                        if (li != "")
                         eachline.Add(li);
                        // split by :
                    }
                }

                // populate object
                SaturnGames sg = new SaturnGames();
                sg.Title = eachline[0].Split(':')[1].Trim();
                sg.Country = eachline[1].Split(':')[1].Trim();
                sg.JPNTitle = eachline[2].Split(':')[1].Trim();
                sg.Serial = eachline[3].Split(':')[1].Trim();
                sg.Version = eachline[4].Split(':')[1].Trim();
                sg.InternalDate = eachline[5].Split(':')[1].Trim();
                sg.TotalTracks = eachline[6].Split(':')[1].Trim();
                sg.DataTracks = eachline[7].Split(':')[1].Trim();
                sg.AudioTracks = eachline[8].Split(':')[1].Trim();
                sg.CountryCode = eachline[9].Split(':')[1].Trim(); ;
                sg.PeriphCode = eachline[10].Split(':')[1].Trim();
                sg.CreationDate = eachline[11].Split(':')[1].Trim();
                sg.CreationTime = eachline[12].Split(':')[1].Trim();
                sg.ModifiedDate = eachline[13].Split(':')[1].Trim();
                sg.ModifiedTime = eachline[14].Split(':')[1].Trim();
                sg.Comments = eachline[15].Split(':')[1].Trim();

                SaturnGamesList.Add(sg);
            }
            */


        }
    }

    public class SaturnGames
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public string JPNTitle { get; set; }
        public string Serial { get; set; }
        public string Version { get; set; }
        public string InternalDate { get; set; }
        public string TotalTracks { get; set; }
        public string DataTracks { get; set; }
        public string AudioTracks { get; set; }
        public string CountryCode { get; set; }
        public string PeriphCode { get; set; }
        public string CreationDate { get; set; }
        public string CreationTime { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedTime { get; set; }
        public string Comments { get; set; }
    }
}
