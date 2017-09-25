using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ucon64_wrapper;

namespace MedLaunch.Classes
{
    public class uConOps
    {
        public static SystemType GetSystemType(int systemId)
        {
            SystemType type = SystemType.None;

            switch (systemId)
            {
                case 1: type = SystemType.Gameboy; break;
                case 2: type = SystemType.GameboyAdvance; break;
                case 3: type = SystemType.Lynx; break;
                case 4: type = SystemType.Genesis; break;
                case 5: type = SystemType.SMS; break;
                case 6: type = SystemType.NeoGeoPocket; break;
                case 7: type = SystemType.PCEngine; break;
                case 8: type = SystemType.GenericDisc; break;
                case 9: type = SystemType.Playstation; break;
                case 10: type = SystemType.SMS; break;
                case 11: type = SystemType.NES; break;
                case 12: type = SystemType.SNES; break;
                case 13: type = SystemType.GenericDisc; break;
                case 14: type = SystemType.VirtualBoy; break;
                case 15: type = SystemType.WonderSwan; break;
                case 16: type = SystemType.SNES; break;
                case 17: type = SystemType.PCEngine; break;
                case 18: type = SystemType.GenericDisc; break;
            }

            return type;
        }
    }
}
