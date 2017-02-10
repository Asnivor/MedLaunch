using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes
{  
    public enum ScraperOrder
    {
        Primary,
        Secondary,
        Tertiary
    }

    public enum Systems
    {
        cdplay,
        demo,
        gb,
        gba,
        gg,
        lynx,
        md,
        ngp,
        pce,
        pce_fast,
        pcfx,
        psx,
        sms,
        snes,
        snes_faust,
        ss,
        ssfplay,
        vb,
        wswan
    }

    public enum Qtrecord__vcodec
    {
        raw,
        cscd,
        png
    }

    public enum Sound__driver
    {
        def, // requires translating to "default"
        alsa,
        oss,
        wasapish,
        dsound,
        wasapi,
        sdl,
        jack
    }

    public enum Video__deinterlacer
    {
        weave,
        bob,
        bob_offset
    }

    public enum Video__driver
    {
        opengl,
        sdl,
        overlay
    }

    public enum RomAdded
    {
        added,
        skipped
    }
}
