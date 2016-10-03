using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Models
{
    public class GameFile
    {
        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string GameName { get; set; }
        public int GameId { get; set; }
        public string[] DiskArray { get; set; }
    }
}
