using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            var str = System.Text.Encoding.Default.GetString(by);

            // split the string
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

            return serial;            
        }
    }
}
