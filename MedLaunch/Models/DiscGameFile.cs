using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class DiscGameFile
    {
        // contructors
        public DiscGameFile() { }

        public DiscGameFile(string fullFilePath, int systemId)
        {
            // Set FullPath
            FullPath = fullFilePath;

            // Set FolderPath
            FolderPath = System.IO.Path.GetDirectoryName(FullPath);

            // Set FileName
            FileName = System.IO.Path.GetFileName(FullPath);

            // Set Extension
            Extension = System.IO.Path.GetExtension(FullPath).ToLower();

            // Set GameName 
            GameName = GetGameName(FileName);

            // Set SystemId
            SystemId = systemId;
        }

        public static string GetGameName(string filename)
        {
            string filenameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(filename);
            string tolow = filenameWithoutExt.ToLower();

            List<string> checks = new List<string>();

            string[] splitass = new string[]
            {
                "cd",
                "disc",
                "disk",
                "cd ",
                "disc ",
                "disk ",
                "d",
                "c",                                
                "cd",
                "disc",
                "disk",
                "cd ",
                "disc ",
                "disk ",

                "CD",
                "DISC",
                "DISK",
                "CD ",
                "DISC ",
                "DISK ",
                "D",
                "C",
            };

            for (int i = 1; i < 10; i++)
            {
                foreach (var s in splitass)
                {
                    checks.Add(s + i);
                }
            }

            foreach (var delim in checks)
            {
                if (filenameWithoutExt.Contains(delim))
                {
                    return StripBullshit(filename, delim).TrimEnd(']').TrimEnd(')').Trim();            
                }
            }

            return filenameWithoutExt;
        }

        public static string StripBullshit(string name, string splitString)
        {
            string[] chars = new string[] { splitString };
            return name.Split(chars, StringSplitOptions.None).First().Trim();
        }

        public DiscGameFile(string fullFilePath, int systemId, bool isSingleDisk)
        {
            // Set FullPath
            FullPath = fullFilePath;

            // Set FolderPath
            FolderPath = System.IO.Path.GetDirectoryName(FullPath);

            // Set FileName
            FileName = System.IO.Path.GetFileName(FullPath);

            // Set Extension
            Extension = System.IO.Path.GetExtension(FullPath).ToLower();

            // Set GameName from filename
            GameName = GameName = GetGameName(FileName);

            // Set SystemId
            SystemId = systemId;
        }
       

        

        // methods

        // properties
        public string FullPath { get; set; }
        public string FolderPath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string GameName { get; set; }
        public int GameId { get; set; }
        public int SystemId { get; set; }
        // unset props
        public string ExtraInfo { get; set; }       // PSX serial number etc

    }
}
