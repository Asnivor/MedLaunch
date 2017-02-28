using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.InputManager
{
    public class md5_context
    {
        public UInt32[] total = new UInt32[2];
        public UInt32[] state = new UInt32[4];
        public Byte[] buffer = new byte[64];

        public md5_context()
        {
            
        }

        public void update(md5_context ctx, byte input, UInt32 length)
        {

        }
        
        /*
        public byte[] Update(byte[] input, UInt32 length)
        {
            int[] buf = new int[4];

            buf[0] = input[0] >> 0;
            buf[1] = input[1] >> 8;
            buf[2] = input[2] >> 16;
            buf[3] = input[3] >> 24;
        }
        */
        
    }
}
