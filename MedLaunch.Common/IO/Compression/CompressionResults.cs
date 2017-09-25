using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common.Crypto;

namespace MedLaunch.Common.IO.Compression
{
    public class CompressionResults
    {
        public List<CompressionResult> Results { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveMD5 { get; set; }

        public CompressionResults(string archivePath)
        {
            Results = new List<CompressionResult>();
            ArchivePath = archivePath;
            ArchiveMD5 = Crypto.Converters.GetMD5Hash(ArchivePath);
        }
    }
}
