using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.IO
{
    public class Crypto
    {
        /// <summary>
        /// Calculates MD5 hash from a STREAM
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5Hash(System.IO.Stream stream)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                return h;
            }
        }

        /// <summary>
        /// Calculates MD5 hash from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string filePath)
        {
            if (!File.Exists(filePath))
                return "";

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    return h;
                }
            }
        }

        /// <summary>
        /// Returns a pseudo-random phrase (with no spaces) of desired length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetNotStrongRandomPhrase(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
