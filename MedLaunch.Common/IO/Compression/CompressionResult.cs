using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common.IO.Compression
{
    public class CompressionResult
    {
        public string FileName { get; set; }
        public string InternalPath { get; set; }
        public string InternalPathWithoutMount { get; set; }
        public string ArchivePath { get; set; }
        public string DBPathString { get; set; }
        public string MD5 { get; set; }
        public string CRC32 { get; set; }

        public CompressionResult CalculateDBPathString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ArchivePath);
            sb.Append("*/");
            sb.Append(InternalPath);
            DBPathString = sb.ToString();
            return this;
        }
    }
}
