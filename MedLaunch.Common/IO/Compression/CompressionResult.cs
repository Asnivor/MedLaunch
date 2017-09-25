using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common.IO.Compression
{
    public class CompressionResult
    {
        public string FileName { get; set; }        // filename
        public string RomName { get; set; }         // filename without extension
        public string Extension { get; set; }       // just the extension
        public string RelativePath { get; set; }    
        public string InternalPath { get; set; }    
        public string ArchivePath { get; set; }     // full path to archive
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

            // work out extension
            string[] dots = FileName.Split('.');
            if (dots.Length > 0)
            {
                string ext = "." + dots.Last();
                Extension = ext;
            }

            // romname (without extension)
            RomName = FileName.Replace(Extension, "");

            return this;
        }
    }
}
