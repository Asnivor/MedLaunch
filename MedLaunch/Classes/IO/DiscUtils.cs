using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MedLaunch.Models;
using MedLaunch.Classes.Scanning;
using DiscSN;
using MedLaunch.Common.IO.Compression;

namespace MedLaunch.Classes.IO
{
    public class MedDiscUtils
    {
        /// <summary>
        /// returns the PSX serial - Bizhawk DiscSystem requires either cue, ccd or iso (not bin or img)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPSXSerial(string path)
        {
            string serial = "";
            try
            {
                serial = SerialNumber.GetPSXSerial(path);
            }
            catch
            {
                // exceptions
                return null;
            }

            if (serial == "")
                return null;

            return serial;
        }

        public static SaturnGame GetSSData(string path)
        {
            SaturnGame sg = new SaturnGame();

            if (!File.Exists(path))
                return null;

            // set start position
            int pos = 16;
            // set read length
            int required = 16;

            List<string> str = new List<string>();

            while (pos < required * 13)
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
                    pos += required;
                }
                // convert byte array to string
                str.Add(System.Text.Encoding.Default.GetString(by));
            }
            string[] spline = str[2].Split(' ');
            if (spline.Length > 1)
            {
                sg.SerialNumber = spline[0].Trim();
                sg.Version = spline.Last().Trim().Replace("V", "").Replace("v", "");
            }
            
            sg.Country = str[4].Trim();
            sg.PeriphCode = str[5].Trim();
            sg.Title = str[6].Trim();

            return sg;
        }
    }

    public class PsxSBI
    {
        public static List<string> SBINumbers { get; set; }
        public static string SBIArchivePath { get; set; }
        public static string PS1TitlesPath { get; set; }
        public static CompressionResults CompResults { get; set; }

        // constructor
        public PsxSBI()
        {
            SBIArchivePath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\SbiFiles.7z";
            PS1TitlesPath = AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ps1titles_us_eu_jp.txt";

            SBINumbers = new List<string>();

            // get all availble sbi numbers from the archive
            Archive arch = new Archive(SBIArchivePath);
            var results = arch.ProcessArchive(new string[] { ".7z" });
            CompResults = results;
            List<string> unprocessed = new List<string>(); //Archiving.GetSbiListFrom7z(SBIArchivePath);
            foreach (var thing in results.Results)
            {
                // strip extension and braces and add to list
                string tmp = thing.RomName.Replace(".7z", "")
                    .Replace("[", "")
                    .Replace("]", "");

                SBINumbers.Add(tmp);
            }

            /*
            // strip extension and braces and add to list
            foreach (string s in unprocessed)
            {
                string tmp = s.Replace(".7z", "")
                    .Replace("[", "")
                    .Replace("]", "");

                SBINumbers.Add(tmp);
            }
            */
        }

        public static void InstallSBIFile(DiscGameFile cueFile)
        {
            string sbiDestPath = cueFile.FolderPath + "\\" + cueFile.FileName.Replace(cueFile.Extension, "") + ".sbi";
            string serial = cueFile.ExtraInfo.Split('-')[1];

            if (cueFile.ExtraInfo == null || cueFile.ExtraInfo == "")
                return;

            string serialNo = cueFile.ExtraInfo;
            string fileName = "";

            // iterate through each detected sbi number
            foreach (string s in SBINumbers)
            {
                if (serialNo.Contains(s))
                {
                    // this is the SBI we want - extract it
                    fileName = "[" + s + "].7z";
                    Archive.ExtractFile(SBIArchivePath, fileName, cueFile.FolderPath);
                }
            }

            if (fileName == "")
                return;

            // now extract the inner 7z and rename to match cue file
            Archive a = new Archive(cueFile.FolderPath + "\\" + fileName);
            var res = a.ProcessArchive(new string[] { ".sbi" });
            var r = res.Results.FirstOrDefault();
            if (r == null)
                return;

            Archive.ExtractFile(cueFile.FolderPath + "\\" + fileName, r.FileName, cueFile.FolderPath);

            // rename the sbi file to match the cue
            if (File.Exists(cueFile.FolderPath + "\\" + r.FileName))
            {
                File.Move(cueFile.FolderPath + "\\" + r.FileName, sbiDestPath);
            }
                        
            // delete the 7z
            File.Delete(cueFile.FolderPath + "\\" + fileName);            
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

    /*
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

    /*
        }
       
    }
     */

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
