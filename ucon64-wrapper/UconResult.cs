using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ucon64_wrapper
{
    public class UconResult
    {
        public string Status { get; set; }
        public string RawOutput { get; set; }
        public string MD5 { get; set; }
        public string ConvertedPath { get; set; }
        public UconData Data { get; set; }
        
        public UconResult()
        {
            Data = new UconData();
        }
    }

    public class UconData
    {
        public SystemType systemType { get; set; }
        public RomType romType { get; set; }
        public string RomPath { get; set; }
        public string Header { get; set; }
        public string DetectedSystemType { get; set; }
        public string DetectedRomType { get; set; }
        public bool? IsInterleaved { get; set; }
        public bool IsChecksumValid { get; set; }
        public string DetectedChecksumComparison { get; set; }
        public string CRC32 { get; set; }
        public string DetectedGameName { get; set; }
        public string DetectedPublisher { get; set; }
        public string DetectedRegion { get; set; }
        public string DetectedYear { get; set; }
        public string DetectedSize { get; set; }
        public string DetectedVersion { get; set; }
        public string DetectedPadding { get; set; }
    }
}
