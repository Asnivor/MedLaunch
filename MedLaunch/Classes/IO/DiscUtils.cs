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
using MedLaunch.Classes.Scanning;
using BizHawk.Emulation.DiscSystem;


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
            //path = @"G:\_Emulation\PSX\iso\Metal Gear Solid - Integral (J) [SLPM-86247]\Metal Gear Solid - Integral (J) (Disc 1) [SLPM-86247].cue";

            int lba = 23;
            Disc disc = Disc.LoadAutomagic(path);

            if (disc == null)
            {
                // unable to mount disc - return null
                return null;
            }

            var discView = EDiscStreamView.DiscStreamView_Mode1_2048;
            if (disc.TOC.Session1Format == SessionFormat.Type20_CDXA)
                discView = EDiscStreamView.DiscStreamView_Mode2_Form1_2048;

            var iso = new ISOFile();
            bool isIso = iso.Parse(new DiscStream(disc, discView, 0));

            if (isIso)
            {
                var appId = System.Text.Encoding.ASCII.GetString(iso.VolumeDescriptors[0].ApplicationIdentifier).TrimEnd('\0', ' ');

                var desc = iso.Root.Children;

                long ir = 0;
                ISONode ifn = null;

                foreach (var i in desc)
                {
                    if (i.Key.Contains("SYSTEM.CNF"))
                        ifn = i.Value;                        
                }

                lba = Convert.ToInt32(ifn.Offset);
            }
            
              
            DiscIdentifier di = new DiscIdentifier(disc);

            // start by checking sector 23 (as most discs seem to have system.cfg there
            byte[] data =  di.GetPSXSerialNumber(lba);
            // take first 32 bytes
            byte[] data32 = data.ToList().Take(32).ToArray();

            string sS = System.Text.Encoding.Default.GetString(data32);

            if (!sS.Contains("cdrom:"))
            {
                return null;
            }
            
            // get the actual serial number from the returned string
            string[] arr = sS.Split(new string[] { "cdrom:" }, StringSplitOptions.None);
            string[] arr2 = arr[1].Split(new string[] { ";1" }, StringSplitOptions.None);
            string serial = arr2[0].Replace("_", "-").Replace(".", "");
            if (serial.Contains("\\"))
                serial = serial.Split('\\').Last();
            else
                serial = serial.TrimStart('\\').TrimStart('\\');

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
