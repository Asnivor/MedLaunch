using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MedLaunch.Classes.Controls.InputManager
{
    
    public class IdGenerator
    {
        public Int64 ID { get; set; }

        public static ulong CalcOldStyleID(int num_axes, int num_balls, int num_hats, int num_buttons)
        {
            byte[] digest = new byte[16];
            byte[] tohash = new byte[4];

            ulong ret = 0;

            tohash[0] = Convert.ToByte(num_axes);
            tohash[1] = Convert.ToByte(num_balls);
            tohash[2] = Convert.ToByte(num_hats);
            tohash[3] = Convert.ToByte(num_buttons);

            //byte[] result = new byte[tohash.Length * sizeof(int)];
            //Buffer.BlockCopy(tohash, 0, result, 0, result.Length);            

            var str = System.Text.Encoding.Default.GetString(tohash);
            
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, str);
                digest = StringToByteArray(hash);
            }
           

            for (int i = 0; i < 16; i++)
            {
                ret ^= (ulong)digest[i] << ((i & 7) * 8);
            }
            return ret;
        }





        public class md5_context
        {
            private UInt32[] total = new UInt32[2];
            private UInt32[] state = new UInt32[4];
            private byte[] buffer = new byte[64];

            //constructor
            public md5_context()
            {
                total[0] = 0;
                total[1] = 0;
                state[0] = 0x67452301;
                state[1] = 0xEFCDAB89;
                state[2] = 0x98BADCFE;
                state[3] = 0x10325476;
            }

            public void update(byte[] input, UInt32 length)
            {
                UInt32 left;
                UInt32 fill;

                if (length == 0) return;

                left = (total[0] >> 3) & 0x3F;
                fill = 64 - left;

                total[0] += length << 3;
                total[1] += length >> 29;

                
            }
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
