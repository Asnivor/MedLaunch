using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.InputManager
{
    
    public class IdGenerator
    {
        public Int64 ID { get; set; }

        public static ulong CalcOldStyleID(int num_axes, int num_balls, int num_hats, int num_buttons)
        {
            byte[] digest = new byte[16];
            int[] tohash = new int[4];

            ulong ret = 0;

            tohash[0] = num_axes;
            tohash[1] = num_balls;
            tohash[2] = num_hats;
            tohash[3] = num_buttons;

            byte[] result = new byte[tohash.Length * sizeof(int)];
            Buffer.BlockCopy(tohash, 0, result, 0, result.Length);

            /*

            var str = System.Text.Encoding.Default.GetString(result);

            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, str);
                digest = StringToByteArray(hash);
            }
            */

            for (int i = 0; i < 16; i++)
            {
                ret ^= (ulong)result[i] << ((i & 7) * 8);
            }
            return ret;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
}
