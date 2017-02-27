using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.DirectInput;
using MahApps.Metro.SimpleChildWindow;
using System.Windows.Interop;

namespace MedLaunch.Classes.Controls
{
    public class DxKeys
    {
        public int SDLK { get; set; }
        public string DxUK { get; set; }
    }

    public class KeyboardTranslation
    {
        public List<DxKeys> dxKeys { get; set; }

        public KeyboardTranslation()
        {
            dxKeys = new List<DxKeys>();
            dxKeys.Add(new DxKeys { SDLK = 0, DxUK = "SDLK_UNKNOWN" });
            dxKeys.Add(new DxKeys { SDLK = 8, DxUK = "Backspace" });
            dxKeys.Add(new DxKeys { SDLK = 9, DxUK = "Tab" });
            dxKeys.Add(new DxKeys { SDLK = 12, DxUK = "SDLK_CLEAR" });
            dxKeys.Add(new DxKeys { SDLK = 13, DxUK = "Return" });
            dxKeys.Add(new DxKeys { SDLK = 19, DxUK = "Pause" });
            dxKeys.Add(new DxKeys { SDLK = 27, DxUK = "Escape" });
            dxKeys.Add(new DxKeys { SDLK = 32, DxUK = "Space" });
            dxKeys.Add(new DxKeys { SDLK = 33, DxUK = "SDLK_EXCLAIM" });
            dxKeys.Add(new DxKeys { SDLK = 34, DxUK = "SDLK_QUOTEDBL" });
            dxKeys.Add(new DxKeys { SDLK = 35, DxUK = "SDLK_HASH" });
            dxKeys.Add(new DxKeys { SDLK = 36, DxUK = "SDLK_DOLLAR" });
            dxKeys.Add(new DxKeys { SDLK = 38, DxUK = "SDLK_AMPERSAND" });
            dxKeys.Add(new DxKeys { SDLK = 39, DxUK = "Apostrophe" });
            dxKeys.Add(new DxKeys { SDLK = 40, DxUK = "SDLK_LEFTPAREN" });
            dxKeys.Add(new DxKeys { SDLK = 41, DxUK = "SDLK_RIGHTPAREN" });
            dxKeys.Add(new DxKeys { SDLK = 42, DxUK = "SDLK_ASTERISK" });
            dxKeys.Add(new DxKeys { SDLK = 43, DxUK = "SDLK_PLUS" });
            dxKeys.Add(new DxKeys { SDLK = 44, DxUK = "Comma" });
            dxKeys.Add(new DxKeys { SDLK = 45, DxUK = "Minus" });
            dxKeys.Add(new DxKeys { SDLK = 46, DxUK = "Period" });
            dxKeys.Add(new DxKeys { SDLK = 47, DxUK = "Slash" });
            dxKeys.Add(new DxKeys { SDLK = 48, DxUK = "D0" });
            dxKeys.Add(new DxKeys { SDLK = 49, DxUK = "D1" });
            dxKeys.Add(new DxKeys { SDLK = 50, DxUK = "D2" });
            dxKeys.Add(new DxKeys { SDLK = 51, DxUK = "D3" });
            dxKeys.Add(new DxKeys { SDLK = 52, DxUK = "D4" });
            dxKeys.Add(new DxKeys { SDLK = 53, DxUK = "D5" });
            dxKeys.Add(new DxKeys { SDLK = 54, DxUK = "D6" });
            dxKeys.Add(new DxKeys { SDLK = 55, DxUK = "D7" });
            dxKeys.Add(new DxKeys { SDLK = 56, DxUK = "D8" });
            dxKeys.Add(new DxKeys { SDLK = 57, DxUK = "D9" });
            dxKeys.Add(new DxKeys { SDLK = 58, DxUK = "Colon" });
            dxKeys.Add(new DxKeys { SDLK = 59, DxUK = "Semicolon" });
            dxKeys.Add(new DxKeys { SDLK = 60, DxUK = "Oem102" });
            dxKeys.Add(new DxKeys { SDLK = 61, DxUK = "Equals" });
            dxKeys.Add(new DxKeys { SDLK = 62, DxUK = "SDLK_GREATER" });
            dxKeys.Add(new DxKeys { SDLK = 63, DxUK = "SDLK_QUESTION" });
            dxKeys.Add(new DxKeys { SDLK = 64, DxUK = "SDLK_AT" });

            // skip uppercase

            dxKeys.Add(new DxKeys { SDLK = 91, DxUK = "LeftBracket" });
            dxKeys.Add(new DxKeys { SDLK = 92, DxUK = "Backslash" });
            dxKeys.Add(new DxKeys { SDLK = 93, DxUK = "RightBracket" });
            dxKeys.Add(new DxKeys { SDLK = 94, DxUK = "SDLK_CARET" });
            dxKeys.Add(new DxKeys { SDLK = 95, DxUK = "SDLK_UNDERSCORE" });
            dxKeys.Add(new DxKeys { SDLK = 96, DxUK = "Grave" });
            dxKeys.Add(new DxKeys { SDLK = 97, DxUK = "A" });
            dxKeys.Add(new DxKeys { SDLK = 98, DxUK = "B" });
            dxKeys.Add(new DxKeys { SDLK = 99, DxUK = "C" });
            dxKeys.Add(new DxKeys { SDLK = 100, DxUK = "D" });
            dxKeys.Add(new DxKeys { SDLK = 101, DxUK = "E" });
            dxKeys.Add(new DxKeys { SDLK = 102, DxUK = "F" });
            dxKeys.Add(new DxKeys { SDLK = 103, DxUK = "G" });
            dxKeys.Add(new DxKeys { SDLK = 104, DxUK = "H" });
            dxKeys.Add(new DxKeys { SDLK = 105, DxUK = "I" });
            dxKeys.Add(new DxKeys { SDLK = 106, DxUK = "J" });
            dxKeys.Add(new DxKeys { SDLK = 107, DxUK = "K" });
            dxKeys.Add(new DxKeys { SDLK = 108, DxUK = "L" });
            dxKeys.Add(new DxKeys { SDLK = 109, DxUK = "M" });
            dxKeys.Add(new DxKeys { SDLK = 110, DxUK = "N" });
            dxKeys.Add(new DxKeys { SDLK = 111, DxUK = "O" });
            dxKeys.Add(new DxKeys { SDLK = 112, DxUK = "P" });
            dxKeys.Add(new DxKeys { SDLK = 113, DxUK = "Q" });
            dxKeys.Add(new DxKeys { SDLK = 114, DxUK = "R" });
            dxKeys.Add(new DxKeys { SDLK = 115, DxUK = "S" });
            dxKeys.Add(new DxKeys { SDLK = 116, DxUK = "T" });
            dxKeys.Add(new DxKeys { SDLK = 117, DxUK = "U" });
            dxKeys.Add(new DxKeys { SDLK = 118, DxUK = "V" });
            dxKeys.Add(new DxKeys { SDLK = 119, DxUK = "W" });
            dxKeys.Add(new DxKeys { SDLK = 120, DxUK = "X" });
            dxKeys.Add(new DxKeys { SDLK = 121, DxUK = "Y" });
            dxKeys.Add(new DxKeys { SDLK = 122, DxUK = "Z" });
            dxKeys.Add(new DxKeys { SDLK = 127, DxUK = "Delete" });
            // end ascii mapped keysims

            // SDLK_WORLD
            dxKeys.Add(new DxKeys { SDLK = 160, DxUK = "SDLK_WORLD_0" });
            dxKeys.Add(new DxKeys { SDLK = 161, DxUK = "SDLK_WORLD_1" });
            dxKeys.Add(new DxKeys { SDLK = 162, DxUK = "SDLK_WORLD_2" });
            dxKeys.Add(new DxKeys { SDLK = 163, DxUK = "SDLK_WORLD_3" });
            dxKeys.Add(new DxKeys { SDLK = 164, DxUK = "SDLK_WORLD_4" });
            dxKeys.Add(new DxKeys { SDLK = 165, DxUK = "SDLK_WORLD_5" });
            dxKeys.Add(new DxKeys { SDLK = 166, DxUK = "SDLK_WORLD_6" });
            dxKeys.Add(new DxKeys { SDLK = 167, DxUK = "SDLK_WORLD_7" });
            dxKeys.Add(new DxKeys { SDLK = 168, DxUK = "SDLK_WORLD_8" });
            dxKeys.Add(new DxKeys { SDLK = 169, DxUK = "SDLK_WORLD_9" });
            dxKeys.Add(new DxKeys { SDLK = 170, DxUK = "SDLK_WORLD_10" });
            dxKeys.Add(new DxKeys { SDLK = 171, DxUK = "SDLK_WORLD_11" });
            dxKeys.Add(new DxKeys { SDLK = 172, DxUK = "SDLK_WORLD_12" });
            dxKeys.Add(new DxKeys { SDLK = 173, DxUK = "SDLK_WORLD_13" });
            dxKeys.Add(new DxKeys { SDLK = 174, DxUK = "SDLK_WORLD_14" });
            dxKeys.Add(new DxKeys { SDLK = 175, DxUK = "SDLK_WORLD_15" });
            dxKeys.Add(new DxKeys { SDLK = 176, DxUK = "SDLK_WORLD_16" });
            dxKeys.Add(new DxKeys { SDLK = 177, DxUK = "SDLK_WORLD_17" });
            dxKeys.Add(new DxKeys { SDLK = 178, DxUK = "SDLK_WORLD_18" });
            dxKeys.Add(new DxKeys { SDLK = 179, DxUK = "SDLK_WORLD_19" });
            dxKeys.Add(new DxKeys { SDLK = 180, DxUK = "SDLK_WORLD_20" });
            dxKeys.Add(new DxKeys { SDLK = 181, DxUK = "SDLK_WORLD_21" });
            dxKeys.Add(new DxKeys { SDLK = 182, DxUK = "SDLK_WORLD_22" });
            dxKeys.Add(new DxKeys { SDLK = 183, DxUK = "SDLK_WORLD_23" });
            dxKeys.Add(new DxKeys { SDLK = 184, DxUK = "SDLK_WORLD_24" });
            dxKeys.Add(new DxKeys { SDLK = 185, DxUK = "SDLK_WORLD_25" });
            dxKeys.Add(new DxKeys { SDLK = 186, DxUK = "SDLK_WORLD_26" });
            dxKeys.Add(new DxKeys { SDLK = 187, DxUK = "SDLK_WORLD_27" });
            dxKeys.Add(new DxKeys { SDLK = 188, DxUK = "SDLK_WORLD_28" });
            dxKeys.Add(new DxKeys { SDLK = 189, DxUK = "SDLK_WORLD_29" });
            dxKeys.Add(new DxKeys { SDLK = 190, DxUK = "SDLK_WORLD_30" });
            dxKeys.Add(new DxKeys { SDLK = 191, DxUK = "SDLK_WORLD_31" });
            dxKeys.Add(new DxKeys { SDLK = 192, DxUK = "SDLK_WORLD_32" });
            dxKeys.Add(new DxKeys { SDLK = 193, DxUK = "SDLK_WORLD_33" });
            dxKeys.Add(new DxKeys { SDLK = 194, DxUK = "SDLK_WORLD_34" });
            dxKeys.Add(new DxKeys { SDLK = 195, DxUK = "SDLK_WORLD_35" });
            dxKeys.Add(new DxKeys { SDLK = 196, DxUK = "SDLK_WORLD_36" });
            dxKeys.Add(new DxKeys { SDLK = 197, DxUK = "SDLK_WORLD_37" });
            dxKeys.Add(new DxKeys { SDLK = 198, DxUK = "SDLK_WORLD_38" });
            dxKeys.Add(new DxKeys { SDLK = 199, DxUK = "SDLK_WORLD_39" });
            dxKeys.Add(new DxKeys { SDLK = 200, DxUK = "SDLK_WORLD_40" });
            dxKeys.Add(new DxKeys { SDLK = 201, DxUK = "SDLK_WORLD_41" });
            dxKeys.Add(new DxKeys { SDLK = 202, DxUK = "SDLK_WORLD_42" });
            dxKeys.Add(new DxKeys { SDLK = 203, DxUK = "SDLK_WORLD_43" });
            dxKeys.Add(new DxKeys { SDLK = 204, DxUK = "SDLK_WORLD_44" });
            dxKeys.Add(new DxKeys { SDLK = 205, DxUK = "SDLK_WORLD_45" });
            dxKeys.Add(new DxKeys { SDLK = 206, DxUK = "SDLK_WORLD_46" });
            dxKeys.Add(new DxKeys { SDLK = 207, DxUK = "SDLK_WORLD_47" });
            dxKeys.Add(new DxKeys { SDLK = 208, DxUK = "SDLK_WORLD_48" });
            dxKeys.Add(new DxKeys { SDLK = 209, DxUK = "SDLK_WORLD_49" });
            dxKeys.Add(new DxKeys { SDLK = 210, DxUK = "SDLK_WORLD_50" });
            dxKeys.Add(new DxKeys { SDLK = 211, DxUK = "SDLK_WORLD_51" });
            dxKeys.Add(new DxKeys { SDLK = 212, DxUK = "SDLK_WORLD_52" });
            dxKeys.Add(new DxKeys { SDLK = 213, DxUK = "SDLK_WORLD_53" });
            dxKeys.Add(new DxKeys { SDLK = 214, DxUK = "SDLK_WORLD_54" });
            dxKeys.Add(new DxKeys { SDLK = 215, DxUK = "SDLK_WORLD_55" });
            dxKeys.Add(new DxKeys { SDLK = 216, DxUK = "SDLK_WORLD_56" });
            dxKeys.Add(new DxKeys { SDLK = 217, DxUK = "SDLK_WORLD_57" });
            dxKeys.Add(new DxKeys { SDLK = 218, DxUK = "SDLK_WORLD_58" });
            dxKeys.Add(new DxKeys { SDLK = 219, DxUK = "SDLK_WORLD_59" });
            dxKeys.Add(new DxKeys { SDLK = 220, DxUK = "SDLK_WORLD_60" });
            dxKeys.Add(new DxKeys { SDLK = 221, DxUK = "SDLK_WORLD_61" });
            dxKeys.Add(new DxKeys { SDLK = 222, DxUK = "SDLK_WORLD_62" });
            dxKeys.Add(new DxKeys { SDLK = 223, DxUK = "SDLK_WORLD_63" });
            dxKeys.Add(new DxKeys { SDLK = 224, DxUK = "SDLK_WORLD_64" });
            dxKeys.Add(new DxKeys { SDLK = 225, DxUK = "SDLK_WORLD_65" });
            dxKeys.Add(new DxKeys { SDLK = 226, DxUK = "SDLK_WORLD_66" });
            dxKeys.Add(new DxKeys { SDLK = 227, DxUK = "SDLK_WORLD_67" });
            dxKeys.Add(new DxKeys { SDLK = 228, DxUK = "SDLK_WORLD_68" });
            dxKeys.Add(new DxKeys { SDLK = 229, DxUK = "SDLK_WORLD_69" });
            dxKeys.Add(new DxKeys { SDLK = 230, DxUK = "SDLK_WORLD_70" });
            dxKeys.Add(new DxKeys { SDLK = 231, DxUK = "SDLK_WORLD_71" });
            dxKeys.Add(new DxKeys { SDLK = 232, DxUK = "SDLK_WORLD_72" });
            dxKeys.Add(new DxKeys { SDLK = 233, DxUK = "SDLK_WORLD_73" });
            dxKeys.Add(new DxKeys { SDLK = 234, DxUK = "SDLK_WORLD_74" });
            dxKeys.Add(new DxKeys { SDLK = 235, DxUK = "SDLK_WORLD_75" });
            dxKeys.Add(new DxKeys { SDLK = 236, DxUK = "SDLK_WORLD_76" });
            dxKeys.Add(new DxKeys { SDLK = 237, DxUK = "SDLK_WORLD_77" });
            dxKeys.Add(new DxKeys { SDLK = 238, DxUK = "SDLK_WORLD_78" });
            dxKeys.Add(new DxKeys { SDLK = 239, DxUK = "SDLK_WORLD_79" });
            dxKeys.Add(new DxKeys { SDLK = 240, DxUK = "SDLK_WORLD_80" });
            dxKeys.Add(new DxKeys { SDLK = 241, DxUK = "SDLK_WORLD_81" });
            dxKeys.Add(new DxKeys { SDLK = 242, DxUK = "SDLK_WORLD_82" });
            dxKeys.Add(new DxKeys { SDLK = 243, DxUK = "SDLK_WORLD_83" });
            dxKeys.Add(new DxKeys { SDLK = 244, DxUK = "SDLK_WORLD_84" });
            dxKeys.Add(new DxKeys { SDLK = 245, DxUK = "SDLK_WORLD_85" });
            dxKeys.Add(new DxKeys { SDLK = 246, DxUK = "SDLK_WORLD_86" });
            dxKeys.Add(new DxKeys { SDLK = 247, DxUK = "SDLK_WORLD_87" });
            dxKeys.Add(new DxKeys { SDLK = 248, DxUK = "SDLK_WORLD_88" });
            dxKeys.Add(new DxKeys { SDLK = 249, DxUK = "SDLK_WORLD_89" });
            dxKeys.Add(new DxKeys { SDLK = 250, DxUK = "SDLK_WORLD_90" });
            dxKeys.Add(new DxKeys { SDLK = 251, DxUK = "SDLK_WORLD_91" });
            dxKeys.Add(new DxKeys { SDLK = 252, DxUK = "SDLK_WORLD_92" });
            dxKeys.Add(new DxKeys { SDLK = 253, DxUK = "SDLK_WORLD_93" });
            dxKeys.Add(new DxKeys { SDLK = 254, DxUK = "SDLK_WORLD_94" });
            dxKeys.Add(new DxKeys { SDLK = 255, DxUK = "SDLK_WORLD_95" });

            // numeric keypad
            dxKeys.Add(new DxKeys { SDLK = 256, DxUK = "NumberPad0" });
            dxKeys.Add(new DxKeys { SDLK = 257, DxUK = "NumberPad1" });
            dxKeys.Add(new DxKeys { SDLK = 258, DxUK = "NumberPad2" });
            dxKeys.Add(new DxKeys { SDLK = 259, DxUK = "NumberPad3" });
            dxKeys.Add(new DxKeys { SDLK = 260, DxUK = "NumberPad4" });
            dxKeys.Add(new DxKeys { SDLK = 261, DxUK = "NumberPad5" });
            dxKeys.Add(new DxKeys { SDLK = 262, DxUK = "NumberPad6" });
            dxKeys.Add(new DxKeys { SDLK = 263, DxUK = "NumberPad7" });
            dxKeys.Add(new DxKeys { SDLK = 264, DxUK = "NumberPad8" });
            dxKeys.Add(new DxKeys { SDLK = 265, DxUK = "NumberPad9" });
            dxKeys.Add(new DxKeys { SDLK = 266, DxUK = "NumberPadPeriod" });
            dxKeys.Add(new DxKeys { SDLK = 267, DxUK = "NumberPadSlash" });
            dxKeys.Add(new DxKeys { SDLK = 268, DxUK = "NumberPadStar" });
            dxKeys.Add(new DxKeys { SDLK = 269, DxUK = "NumberPadMinus" });
            dxKeys.Add(new DxKeys { SDLK = 270, DxUK = "NumberPadPlus" });
            dxKeys.Add(new DxKeys { SDLK = 271, DxUK = "NumberPadEnter" });
            dxKeys.Add(new DxKeys { SDLK = 272, DxUK = "NumberPadEquals" });

            // Arrows + Home/End pad
            dxKeys.Add(new DxKeys { SDLK = 273, DxUK = "UpArrow" });
            dxKeys.Add(new DxKeys { SDLK = 274, DxUK = "DownArrow" });
            dxKeys.Add(new DxKeys { SDLK = 275, DxUK = "RightArrow" });
            dxKeys.Add(new DxKeys { SDLK = 276, DxUK = "LeftArrow" });
            dxKeys.Add(new DxKeys { SDLK = 277, DxUK = "Insert" });
            dxKeys.Add(new DxKeys { SDLK = 278, DxUK = "Home" });
            dxKeys.Add(new DxKeys { SDLK = 279, DxUK = "End" });
            dxKeys.Add(new DxKeys { SDLK = 280, DxUK = "PageUp" });
            dxKeys.Add(new DxKeys { SDLK = 281, DxUK = "PageDown" });

            // Function keys
            dxKeys.Add(new DxKeys { SDLK = 282, DxUK = "F1" });
            dxKeys.Add(new DxKeys { SDLK = 283, DxUK = "F2" });
            dxKeys.Add(new DxKeys { SDLK = 284, DxUK = "F3" });
            dxKeys.Add(new DxKeys { SDLK = 285, DxUK = "F4" });
            dxKeys.Add(new DxKeys { SDLK = 286, DxUK = "F5" });
            dxKeys.Add(new DxKeys { SDLK = 287, DxUK = "F6" });
            dxKeys.Add(new DxKeys { SDLK = 288, DxUK = "F7" });
            dxKeys.Add(new DxKeys { SDLK = 289, DxUK = "F8" });
            dxKeys.Add(new DxKeys { SDLK = 290, DxUK = "F9" });
            dxKeys.Add(new DxKeys { SDLK = 291, DxUK = "F10" });
            dxKeys.Add(new DxKeys { SDLK = 292, DxUK = "F11" });
            dxKeys.Add(new DxKeys { SDLK = 293, DxUK = "F12" });
            dxKeys.Add(new DxKeys { SDLK = 294, DxUK = "F13" });
            dxKeys.Add(new DxKeys { SDLK = 295, DxUK = "F14" });
            dxKeys.Add(new DxKeys { SDLK = 296, DxUK = "F15" });

            // Key state modifier keys
            dxKeys.Add(new DxKeys { SDLK = 300, DxUK = "NumberLock" });
            dxKeys.Add(new DxKeys { SDLK = 301, DxUK = "CapsLock" });
            dxKeys.Add(new DxKeys { SDLK = 302, DxUK = "ScrollLock" });
            dxKeys.Add(new DxKeys { SDLK = 303, DxUK = "RightShift" });
            dxKeys.Add(new DxKeys { SDLK = 304, DxUK = "LeftShift" });
            dxKeys.Add(new DxKeys { SDLK = 305, DxUK = "RightControl" });
            dxKeys.Add(new DxKeys { SDLK = 306, DxUK = "RightAlt" });       //alt-gr
            dxKeys.Add(new DxKeys { SDLK = 307, DxUK = "SDLK_RALT" });
            dxKeys.Add(new DxKeys { SDLK = 308, DxUK = "LeftAlt" });
            dxKeys.Add(new DxKeys { SDLK = 309, DxUK = "SDLK_RMETA" });
            dxKeys.Add(new DxKeys { SDLK = 310, DxUK = "SDLK_LMETA" });
            dxKeys.Add(new DxKeys { SDLK = 311, DxUK = "LeftWindowsKey" });
            dxKeys.Add(new DxKeys { SDLK = 312, DxUK = "RightWindowsKey" });
            dxKeys.Add(new DxKeys { SDLK = 313, DxUK = "SDLK_RALT" });
            dxKeys.Add(new DxKeys { SDLK = 314, DxUK = "SDLK_COMPOSE" });

            // Misc function keys
            dxKeys.Add(new DxKeys { SDLK = 315, DxUK = "SDLK_HELP" });
            dxKeys.Add(new DxKeys { SDLK = 316, DxUK = "PrintScreen" });
            dxKeys.Add(new DxKeys { SDLK = 317, DxUK = "SDLK_SYSREQ" });
            dxKeys.Add(new DxKeys { SDLK = 318, DxUK = "Pause" });
            dxKeys.Add(new DxKeys { SDLK = 319, DxUK = "Applications" });
            dxKeys.Add(new DxKeys { SDLK = 320, DxUK = "Power" });
            dxKeys.Add(new DxKeys { SDLK = 321, DxUK = "SDLK_EURO" });
            dxKeys.Add(new DxKeys { SDLK = 322, DxUK = "SDLK_UNDO" });
        }

        public static string DXtoSDLCode(string dxString, KeyboardType keyboardType)
        {
            KeyboardTranslation kbt = new KeyboardTranslation();

            if (keyboardType == KeyboardType.UK)
            {
                var uk = kbt.dxKeys.Where(a => a.DxUK == dxString).ToList();
                if (uk.Count == 0)
                {
                    return dxString;
                }

                return "keyboard " + uk.First().SDLK.ToString();
            }
            return dxString;
        }

        public static string SDLCodetoDx(string sdlCode, KeyboardType keyboardType)
        {
            KeyboardTranslation kbt = new KeyboardTranslation();

            if (sdlCode == null || sdlCode == "")
                return "";

            if (keyboardType == KeyboardType.UK)
            {
                var uk = kbt.dxKeys.Where(a => a.SDLK.ToString() == sdlCode.Replace("keyboard ", "")).ToList();
                if (uk.Count == 0)
                {
                    return sdlCode;
                }

                if (uk.First().DxUK.Contains("SDLK_"))
                {
                    // no dx lookup was found
                    return "keyboard " + uk.First().SDLK;
                }

                return uk.First().DxUK;
            }

            return sdlCode;
        }

    }

    

    public enum KeyboardType
    {
        UK,
        US
    }
}
