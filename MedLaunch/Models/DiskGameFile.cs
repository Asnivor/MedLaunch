using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class DiskGameFile
    {
        // contructors
        public DiskGameFile() { }

        public DiskGameFile(string fullFilePath, int systemId)
        {
            // Set FullPath
            FullPath = fullFilePath;

            // Set FolderPath
            FolderPath = System.IO.Path.GetDirectoryName(FullPath);

            // Set FileName
            FileName = System.IO.Path.GetFileName(FullPath);

            // Set Extension
            Extension = System.IO.Path.GetExtension(FullPath).ToLower();

            // Set GameName from parent directory
            GameName = System.IO.Directory.GetParent(FullPath).Name;

            // Set SystemId
            SystemId = systemId;
        }

        public DiskGameFile(string fullFilePath, int systemId, bool isSingleDisk)
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
            GameName = System.IO.Path.GetFileNameWithoutExtension(FullPath);

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
    }
}
