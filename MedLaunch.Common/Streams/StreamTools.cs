using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common.Streams
{
    public class StreamTools
    {
        public static void SaveStreamToDisk(Stream input, string FullLocalFilePath)
        {
            byte[] buffer = new byte[16345];
            using (FileStream fs = new FileStream(FullLocalFilePath,
                                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, read);
                }
            }
        }

        public static byte[] ReadAllBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
