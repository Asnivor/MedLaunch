using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ucon64_wrapper
{
    public enum SystemType
    {
        None,
        Genesis,
        NeoGeoPocket,
        Playstation,
        GameboyAdvance,
        SNES,
        Gameboy,
        PCEngine,
        NES,
        SMS,
        GenericDisc,
        WonderSwan,
        VirtualBoy        
    }

    public enum RomType
    {
        Unknown,
        SMD,
        BIN
    }
}
