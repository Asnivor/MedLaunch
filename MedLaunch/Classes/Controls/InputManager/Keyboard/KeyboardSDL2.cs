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
    
    public class KeyboardTranslationSDL2 : IKeyboardTranslator
    {
        public List<DxKeys> dxKeys { get; set; }

        public KeyboardTranslationSDL2(KeyboardType keyboardType)
        {
            dxKeys = new List<DxKeys>();
            dxKeys.Add(new DxKeys { SDLK = 0, DxUK = "SDLK_UNKNOWN" });

            // letters
            dxKeys.Add(new DxKeys { SDLK = 4, DxUK = "A" });
            dxKeys.Add(new DxKeys { SDLK = 5, DxUK = "B" });
            dxKeys.Add(new DxKeys { SDLK = 6, DxUK = "C" });
            dxKeys.Add(new DxKeys { SDLK = 7, DxUK = "D" });
            dxKeys.Add(new DxKeys { SDLK = 8, DxUK = "E" });
            dxKeys.Add(new DxKeys { SDLK = 9, DxUK = "F" });
            dxKeys.Add(new DxKeys { SDLK = 10, DxUK = "G" });
            dxKeys.Add(new DxKeys { SDLK = 11, DxUK = "H" });
            dxKeys.Add(new DxKeys { SDLK = 12, DxUK = "I" });
            dxKeys.Add(new DxKeys { SDLK = 13, DxUK = "J" });
            dxKeys.Add(new DxKeys { SDLK = 14, DxUK = "K" });
            dxKeys.Add(new DxKeys { SDLK = 15, DxUK = "L" });
            dxKeys.Add(new DxKeys { SDLK = 16, DxUK = "M" });
            dxKeys.Add(new DxKeys { SDLK = 17, DxUK = "N" });
            dxKeys.Add(new DxKeys { SDLK = 18, DxUK = "O" });
            dxKeys.Add(new DxKeys { SDLK = 19, DxUK = "P" });
            dxKeys.Add(new DxKeys { SDLK = 20, DxUK = "Q" });
            dxKeys.Add(new DxKeys { SDLK = 21, DxUK = "R" });
            dxKeys.Add(new DxKeys { SDLK = 22, DxUK = "S" });
            dxKeys.Add(new DxKeys { SDLK = 23, DxUK = "T" });
            dxKeys.Add(new DxKeys { SDLK = 24, DxUK = "U" });
            dxKeys.Add(new DxKeys { SDLK = 25, DxUK = "V" });
            dxKeys.Add(new DxKeys { SDLK = 26, DxUK = "W" });
            dxKeys.Add(new DxKeys { SDLK = 27, DxUK = "X" });
            dxKeys.Add(new DxKeys { SDLK = 28, DxUK = "Y" });
            dxKeys.Add(new DxKeys { SDLK = 29, DxUK = "Z" });

            // numbers
            dxKeys.Add(new DxKeys { SDLK = 30, DxUK = "D1" });
            dxKeys.Add(new DxKeys { SDLK = 31, DxUK = "D2" });
            dxKeys.Add(new DxKeys { SDLK = 32, DxUK = "D3" });
            dxKeys.Add(new DxKeys { SDLK = 33, DxUK = "D4" });
            dxKeys.Add(new DxKeys { SDLK = 34, DxUK = "D5" });
            dxKeys.Add(new DxKeys { SDLK = 35, DxUK = "D6" });
            dxKeys.Add(new DxKeys { SDLK = 36, DxUK = "D7" });
            dxKeys.Add(new DxKeys { SDLK = 37, DxUK = "D8" });
            dxKeys.Add(new DxKeys { SDLK = 38, DxUK = "D9" });
            dxKeys.Add(new DxKeys { SDLK = 39, DxUK = "D0" });

            dxKeys.Add(new DxKeys { SDLK = 40, DxUK = "Return" });
            dxKeys.Add(new DxKeys { SDLK = 41, DxUK = "Escape" });
            dxKeys.Add(new DxKeys { SDLK = 42, DxUK = "Backspace" });
            dxKeys.Add(new DxKeys { SDLK = 43, DxUK = "Tab" });
            dxKeys.Add(new DxKeys { SDLK = 44, DxUK = "Space" });

            dxKeys.Add(new DxKeys { SDLK = 45, DxUK = "Minus" });
            dxKeys.Add(new DxKeys { SDLK = 46, DxUK = "Equals" });
            dxKeys.Add(new DxKeys { SDLK = 47, DxUK = "LeftBracket" });
            dxKeys.Add(new DxKeys { SDLK = 48, DxUK = "RightBracket" });
            dxKeys.Add(new DxKeys { SDLK = 49, DxUK = "Backslash" });     
            dxKeys.Add(new DxKeys { SDLK = 50, DxUK = "SDL_SCANCODE_NONUSHASH" });
            dxKeys.Add(new DxKeys { SDLK = 51, DxUK = "Semicolon" });
            dxKeys.Add(new DxKeys { SDLK = 52, DxUK = "Apostrophe" });
            dxKeys.Add(new DxKeys { SDLK = 53, DxUK = "Grave" });
            dxKeys.Add(new DxKeys { SDLK = 54, DxUK = "Comma" });
            dxKeys.Add(new DxKeys { SDLK = 55, DxUK = "Period" });
            dxKeys.Add(new DxKeys { SDLK = 56, DxUK = "Slash" });

            dxKeys.Add(new DxKeys { SDLK = 57, DxUK = "CapsLock" });

            dxKeys.Add(new DxKeys { SDLK = 58, DxUK = "F1" });
            dxKeys.Add(new DxKeys { SDLK = 59, DxUK = "F2" });
            dxKeys.Add(new DxKeys { SDLK = 60, DxUK = "F3" });
            dxKeys.Add(new DxKeys { SDLK = 61, DxUK = "F4" });
            dxKeys.Add(new DxKeys { SDLK = 62, DxUK = "F5" });
            dxKeys.Add(new DxKeys { SDLK = 63, DxUK = "F6" });
            dxKeys.Add(new DxKeys { SDLK = 64, DxUK = "F7" });
            dxKeys.Add(new DxKeys { SDLK = 65, DxUK = "F8" });
            dxKeys.Add(new DxKeys { SDLK = 66, DxUK = "F9" });
            dxKeys.Add(new DxKeys { SDLK = 67, DxUK = "F10" });
            dxKeys.Add(new DxKeys { SDLK = 68, DxUK = "F11" });
            dxKeys.Add(new DxKeys { SDLK = 69, DxUK = "F12" });

            dxKeys.Add(new DxKeys { SDLK = 70, DxUK = "PrintScreen" });
            dxKeys.Add(new DxKeys { SDLK = 71, DxUK = "ScrollLock" });
            dxKeys.Add(new DxKeys { SDLK = 72, DxUK = "Pause" });
            dxKeys.Add(new DxKeys { SDLK = 73, DxUK = "Insert" });
            dxKeys.Add(new DxKeys { SDLK = 74, DxUK = "Home" });
            dxKeys.Add(new DxKeys { SDLK = 75, DxUK = "PageUp" });
            dxKeys.Add(new DxKeys { SDLK = 76, DxUK = "Delete" });
            dxKeys.Add(new DxKeys { SDLK = 77, DxUK = "End" });            
            dxKeys.Add(new DxKeys { SDLK = 78, DxUK = "PageDown" });
            dxKeys.Add(new DxKeys { SDLK = 79, DxUK = "RightArrow" });
            dxKeys.Add(new DxKeys { SDLK = 80, DxUK = "LeftArrow" });
            dxKeys.Add(new DxKeys { SDLK = 81, DxUK = "DownArrow" });
            dxKeys.Add(new DxKeys { SDLK = 82, DxUK = "UpArrow" });

            dxKeys.Add(new DxKeys { SDLK = 83, DxUK = "NumberLock" });

            dxKeys.Add(new DxKeys { SDLK = 84, DxUK = "NumberPadSlash" });
            dxKeys.Add(new DxKeys { SDLK = 85, DxUK = "NumberPadStar" });
            dxKeys.Add(new DxKeys { SDLK = 86, DxUK = "NumberPadMinus" });
            dxKeys.Add(new DxKeys { SDLK = 87, DxUK = "NumberPadPlus" });
            dxKeys.Add(new DxKeys { SDLK = 88, DxUK = "NumberPadEnter" });
            dxKeys.Add(new DxKeys { SDLK = 89, DxUK = "NumberPad1" });
            dxKeys.Add(new DxKeys { SDLK = 90, DxUK = "NumberPad2" });
            dxKeys.Add(new DxKeys { SDLK = 91, DxUK = "NumberPad3" });
            dxKeys.Add(new DxKeys { SDLK = 92, DxUK = "NumberPad4" });
            dxKeys.Add(new DxKeys { SDLK = 93, DxUK = "NumberPad5" });
            dxKeys.Add(new DxKeys { SDLK = 94, DxUK = "NumberPad6" });
            dxKeys.Add(new DxKeys { SDLK = 95, DxUK = "NumberPad7" });
            dxKeys.Add(new DxKeys { SDLK = 96, DxUK = "NumberPad8" });
            dxKeys.Add(new DxKeys { SDLK = 97, DxUK = "NumberPad9" });
            dxKeys.Add(new DxKeys { SDLK = 98, DxUK = "NumberPad0" });            
            dxKeys.Add(new DxKeys { SDLK = 99, DxUK = "NumberPadPeriod" });

            dxKeys.Add(new DxKeys { SDLK = 100, DxUK = "SDL_SCANCODE_NONUSBACKSLASH" });
            dxKeys.Add(new DxKeys { SDLK = 101, DxUK = "Applications" });
            dxKeys.Add(new DxKeys { SDLK = 102, DxUK = "Power" });
            dxKeys.Add(new DxKeys { SDLK = 103, DxUK = "NumberPadEquals" });
            dxKeys.Add(new DxKeys { SDLK = 104, DxUK = "F13" });
            dxKeys.Add(new DxKeys { SDLK = 105, DxUK = "F14" });
            dxKeys.Add(new DxKeys { SDLK = 106, DxUK = "F15" });
            dxKeys.Add(new DxKeys { SDLK = 107, DxUK = "SDL_SCANCODE_F16" });
            dxKeys.Add(new DxKeys { SDLK = 108, DxUK = "SDL_SCANCODE_F17" });
            dxKeys.Add(new DxKeys { SDLK = 109, DxUK = "SDL_SCANCODE_F18" });
            dxKeys.Add(new DxKeys { SDLK = 110, DxUK = "SDL_SCANCODE_F19" });
            dxKeys.Add(new DxKeys { SDLK = 111, DxUK = "SDL_SCANCODE_F20" });
            dxKeys.Add(new DxKeys { SDLK = 112, DxUK = "SDL_SCANCODE_F21" });
            dxKeys.Add(new DxKeys { SDLK = 113, DxUK = "SDL_SCANCODE_F22" });
            dxKeys.Add(new DxKeys { SDLK = 114, DxUK = "SDL_SCANCODE_F23" });
            dxKeys.Add(new DxKeys { SDLK = 115, DxUK = "SDL_SCANCODE_F24" });
            dxKeys.Add(new DxKeys { SDLK = 116, DxUK = "SDL_SCANCODE_EXECUTE" });
            dxKeys.Add(new DxKeys { SDLK = 117, DxUK = "SDL_SCANCODE_HELP" });
            dxKeys.Add(new DxKeys { SDLK = 118, DxUK = "SDL_SCANCODE_MENU" });
            dxKeys.Add(new DxKeys { SDLK = 119, DxUK = "SDL_SCANCODE_SELECT" });
            dxKeys.Add(new DxKeys { SDLK = 120, DxUK = "SDL_SCANCODE_STOP" });
            dxKeys.Add(new DxKeys { SDLK = 121, DxUK = "SDL_SCANCODE_AGAIN" });
            dxKeys.Add(new DxKeys { SDLK = 122, DxUK = "SDL_SCANCODE_UNDO" });
            dxKeys.Add(new DxKeys { SDLK = 123, DxUK = "SDL_SCANCODE_CUT" });
            dxKeys.Add(new DxKeys { SDLK = 124, DxUK = "SDL_SCANCODE_COPY" });
            dxKeys.Add(new DxKeys { SDLK = 125, DxUK = "SDL_SCANCODE_PASTE" });
            dxKeys.Add(new DxKeys { SDLK = 126, DxUK = "WebSearch" });
            dxKeys.Add(new DxKeys { SDLK = 127, DxUK = "Mute" });
            dxKeys.Add(new DxKeys { SDLK = 128, DxUK = "VolumeUp" });
            dxKeys.Add(new DxKeys { SDLK = 129, DxUK = "VolumeDown" });

            dxKeys.Add(new DxKeys { SDLK = 133, DxUK = "NumberPadComma" });
            dxKeys.Add(new DxKeys { SDLK = 134, DxUK = "NumberPadEquals" });

            dxKeys.Add(new DxKeys { SDLK = 135, DxUK = "SDL_SCANCODE_INTERNATIONAL1" });
            dxKeys.Add(new DxKeys { SDLK = 136, DxUK = "SDL_SCANCODE_INTERNATIONAL2" });
            dxKeys.Add(new DxKeys { SDLK = 137, DxUK = "SDL_SCANCODE_INTERNATIONAL3" });
            dxKeys.Add(new DxKeys { SDLK = 138, DxUK = "SDL_SCANCODE_INTERNATIONAL4" });
            dxKeys.Add(new DxKeys { SDLK = 139, DxUK = "SDL_SCANCODE_INTERNATIONAL5" });
            dxKeys.Add(new DxKeys { SDLK = 140, DxUK = "SDL_SCANCODE_INTERNATIONAL6" });
            dxKeys.Add(new DxKeys { SDLK = 141, DxUK = "SDL_SCANCODE_INTERNATIONAL7" });
            dxKeys.Add(new DxKeys { SDLK = 142, DxUK = "SDL_SCANCODE_INTERNATIONAL8" });
            dxKeys.Add(new DxKeys { SDLK = 143, DxUK = "SDL_SCANCODE_INTERNATIONAL9" });
            dxKeys.Add(new DxKeys { SDLK = 144, DxUK = "SDL_SCANCODE_LANG1" });
            dxKeys.Add(new DxKeys { SDLK = 145, DxUK = "SDL_SCANCODE_LANG2" });
            dxKeys.Add(new DxKeys { SDLK = 146, DxUK = "SDL_SCANCODE_LANG3" });
            dxKeys.Add(new DxKeys { SDLK = 147, DxUK = "SDL_SCANCODE_LANG4" });
            dxKeys.Add(new DxKeys { SDLK = 148, DxUK = "SDL_SCANCODE_LANG5" });
            dxKeys.Add(new DxKeys { SDLK = 149, DxUK = "SDL_SCANCODE_LANG6" });
            dxKeys.Add(new DxKeys { SDLK = 150, DxUK = "SDL_SCANCODE_LANG7" });
            dxKeys.Add(new DxKeys { SDLK = 151, DxUK = "SDL_SCANCODE_LANG8" });
            dxKeys.Add(new DxKeys { SDLK = 152, DxUK = "SDL_SCANCODE_LANG9" });

            dxKeys.Add(new DxKeys { SDLK = 153, DxUK = "SDL_SCANCODE_ALTERASE" });
            dxKeys.Add(new DxKeys { SDLK = 154, DxUK = "SDL_SCANCODE_SYSREQ" });
            dxKeys.Add(new DxKeys { SDLK = 155, DxUK = "SDL_SCANCODE_CANCEL" });
            dxKeys.Add(new DxKeys { SDLK = 156, DxUK = "SDL_SCANCODE_CLEAR" });
            dxKeys.Add(new DxKeys { SDLK = 157, DxUK = "SDL_SCANCODE_PRIOR" });
            dxKeys.Add(new DxKeys { SDLK = 158, DxUK = "SDL_SCANCODE_RETURN2" });
            dxKeys.Add(new DxKeys { SDLK = 159, DxUK = "SDL_SCANCODE_SEPARATOR" });
            dxKeys.Add(new DxKeys { SDLK = 160, DxUK = "SDL_SCANCODE_OUT" });
            dxKeys.Add(new DxKeys { SDLK = 161, DxUK = "SDL_SCANCODE_OPER" });
            dxKeys.Add(new DxKeys { SDLK = 162, DxUK = "SDL_SCANCODE_CLEARAGAIN" });
            dxKeys.Add(new DxKeys { SDLK = 163, DxUK = "SDL_SCANCODE_CRSEL" });
            dxKeys.Add(new DxKeys { SDLK = 164, DxUK = "SDL_SCANCODE_EXSEL" });


            dxKeys.Add(new DxKeys { SDLK = 176, DxUK = "SDL_SCANCODE_KP_00" });
            dxKeys.Add(new DxKeys { SDLK = 177, DxUK = "SDL_SCANCODE_KP_000" });
            dxKeys.Add(new DxKeys { SDLK = 178, DxUK = "SDL_SCANCODE_THOUSANDSSEPARATOR" });
            dxKeys.Add(new DxKeys { SDLK = 179, DxUK = "SDL_SCANCODE_DECIMALSEPARATOR" });
            dxKeys.Add(new DxKeys { SDLK = 180, DxUK = "SDL_SCANCODE_CURRENCYUNIT" });
            dxKeys.Add(new DxKeys { SDLK = 181, DxUK = "SDL_SCANCODE_CURRENCYSUBUNIT" });
            dxKeys.Add(new DxKeys { SDLK = 182, DxUK = "SDL_SCANCODE_KP_LEFTPAREN" });
            dxKeys.Add(new DxKeys { SDLK = 183, DxUK = "SDL_SCANCODE_KP_RIGHTPAREN" });
            dxKeys.Add(new DxKeys { SDLK = 184, DxUK = "SDL_SCANCODE_KP_LEFTBRACE" });
            dxKeys.Add(new DxKeys { SDLK = 185, DxUK = "SDL_SCANCODE_KP_RIGHTBRACE" });
            dxKeys.Add(new DxKeys { SDLK = 186, DxUK = "SDL_SCANCODE_KP_TAB" });
            dxKeys.Add(new DxKeys { SDLK = 187, DxUK = "SDL_SCANCODE_KP_BACKSPACE" });
            dxKeys.Add(new DxKeys { SDLK = 188, DxUK = "SDL_SCANCODE_KP_A" });
            dxKeys.Add(new DxKeys { SDLK = 189, DxUK = "SDL_SCANCODE_KP_B" });
            dxKeys.Add(new DxKeys { SDLK = 190, DxUK = "SDL_SCANCODE_KP_C" });
            dxKeys.Add(new DxKeys { SDLK = 191, DxUK = "SDL_SCANCODE_KP_D" });
            dxKeys.Add(new DxKeys { SDLK = 192, DxUK = "SDL_SCANCODE_KP_E" });
            dxKeys.Add(new DxKeys { SDLK = 193, DxUK = "SDL_SCANCODE_KP_F" });
            dxKeys.Add(new DxKeys { SDLK = 194, DxUK = "SDL_SCANCODE_KP_XOR" });
            dxKeys.Add(new DxKeys { SDLK = 195, DxUK = "SDL_SCANCODE_KP_POWER" });
            dxKeys.Add(new DxKeys { SDLK = 196, DxUK = "SDL_SCANCODE_KP_PERCENT" });
            dxKeys.Add(new DxKeys { SDLK = 197, DxUK = "SDL_SCANCODE_KP_LESS" });
            dxKeys.Add(new DxKeys { SDLK = 198, DxUK = "SDL_SCANCODE_KP_GREATER" });
            dxKeys.Add(new DxKeys { SDLK = 199, DxUK = "SDL_SCANCODE_KP_AMPERSAND" });
            dxKeys.Add(new DxKeys { SDLK = 200, DxUK = "SDL_SCANCODE_KP_DBLAMPERSAND" });
            dxKeys.Add(new DxKeys { SDLK = 201, DxUK = "SDL_SCANCODE_KP_VERTICALBAR" });
            dxKeys.Add(new DxKeys { SDLK = 202, DxUK = "SDL_SCANCODE_KP_DBLVERTICALBAR" });
            dxKeys.Add(new DxKeys { SDLK = 203, DxUK = "Colon" });
            dxKeys.Add(new DxKeys { SDLK = 204, DxUK = "SDL_SCANCODE_KP_HASH" });
            dxKeys.Add(new DxKeys { SDLK = 205, DxUK = "SDL_SCANCODE_KP_SPACE" });
            dxKeys.Add(new DxKeys { SDLK = 206, DxUK = "SDL_SCANCODE_KP_AT" });
            dxKeys.Add(new DxKeys { SDLK = 207, DxUK = "SDL_SCANCODE_KP_EXCLAM" });
            dxKeys.Add(new DxKeys { SDLK = 208, DxUK = "SDL_SCANCODE_KP_MEMSTORE" });
            dxKeys.Add(new DxKeys { SDLK = 209, DxUK = "SDL_SCANCODE_KP_MEMRECALL" });
            dxKeys.Add(new DxKeys { SDLK = 210, DxUK = "SDL_SCANCODE_KP_MEMCLEAR" });
            dxKeys.Add(new DxKeys { SDLK = 211, DxUK = "SDL_SCANCODE_KP_MEMADD" });
            dxKeys.Add(new DxKeys { SDLK = 212, DxUK = "SDL_SCANCODE_KP_MEMSUBTRACT" });
            dxKeys.Add(new DxKeys { SDLK = 213, DxUK = "SDL_SCANCODE_KP_MEMMULTIPLY" });
            dxKeys.Add(new DxKeys { SDLK = 214, DxUK = "SDL_SCANCODE_KP_MEMDIVIDE" });
            dxKeys.Add(new DxKeys { SDLK = 215, DxUK = "SDL_SCANCODE_KP_PLUSMINUS" });
            dxKeys.Add(new DxKeys { SDLK = 216, DxUK = "SDL_SCANCODE_KP_CLEAR" });
            dxKeys.Add(new DxKeys { SDLK = 217, DxUK = "SDL_SCANCODE_KP_CLEARENTRY" });
            dxKeys.Add(new DxKeys { SDLK = 218, DxUK = "SDL_SCANCODE_KP_BINARY" });
            dxKeys.Add(new DxKeys { SDLK = 219, DxUK = "SDL_SCANCODE_KP_OCTAL" });
            dxKeys.Add(new DxKeys { SDLK = 220, DxUK = "SDL_SCANCODE_KP_DECIMAL" });
            dxKeys.Add(new DxKeys { SDLK = 221, DxUK = "SDL_SCANCODE_KP_HEXADECIMAL" });            


            dxKeys.Add(new DxKeys { SDLK = 224, DxUK = "LeftControl" });
            dxKeys.Add(new DxKeys { SDLK = 225, DxUK = "LeftShift" });
            dxKeys.Add(new DxKeys { SDLK = 226, DxUK = "LeftAlt" });
            dxKeys.Add(new DxKeys { SDLK = 227, DxUK = "LeftWindowsKey" });
            dxKeys.Add(new DxKeys { SDLK = 228, DxUK = "RightControl" });
            dxKeys.Add(new DxKeys { SDLK = 229, DxUK = "RightShift" });
            dxKeys.Add(new DxKeys { SDLK = 230, DxUK = "RightAlt" });
            dxKeys.Add(new DxKeys { SDLK = 231, DxUK = "RightWindowsKey" });

            dxKeys.Add(new DxKeys { SDLK = 257, DxUK = "SDL_SCANCODE_MODE" });

            dxKeys.Add(new DxKeys { SDLK = 258, DxUK = "NextTrack" });
            dxKeys.Add(new DxKeys { SDLK = 259, DxUK = "PreviousTrack" });
            dxKeys.Add(new DxKeys { SDLK = 260, DxUK = "MediaStop" });
            dxKeys.Add(new DxKeys { SDLK = 261, DxUK = "PlayPause" });
            dxKeys.Add(new DxKeys { SDLK = 262, DxUK = "SDL_SCANCODE_AUDIOMUTE" });
            dxKeys.Add(new DxKeys { SDLK = 263, DxUK = "MediaSelect" });
            dxKeys.Add(new DxKeys { SDLK = 264, DxUK = "SDL_SCANCODE_WWW" });
            dxKeys.Add(new DxKeys { SDLK = 265, DxUK = "Mail" });
            dxKeys.Add(new DxKeys { SDLK = 266, DxUK = "Calculator" });
            dxKeys.Add(new DxKeys { SDLK = 267, DxUK = "MyComputer" });
            dxKeys.Add(new DxKeys { SDLK = 268, DxUK = "WebSearch" });
            dxKeys.Add(new DxKeys { SDLK = 269, DxUK = "WebHome" });
            dxKeys.Add(new DxKeys { SDLK = 270, DxUK = "WebBack" });
            dxKeys.Add(new DxKeys { SDLK = 271, DxUK = "WebForward" });
            dxKeys.Add(new DxKeys { SDLK = 272, DxUK = "WebStop" });
            dxKeys.Add(new DxKeys { SDLK = 273, DxUK = "WebRefresh" });
            dxKeys.Add(new DxKeys { SDLK = 274, DxUK = "WebFavorites" });

            dxKeys.Add(new DxKeys { SDLK = 275, DxUK = "SDL_SCANCODE_BRIGHTNESSDOWN" });
            dxKeys.Add(new DxKeys { SDLK = 276, DxUK = "SDL_SCANCODE_BRIGHTNESSUP" });
            dxKeys.Add(new DxKeys { SDLK = 277, DxUK = "SDL_SCANCODE_DISPLAYSWITCH" });
            dxKeys.Add(new DxKeys { SDLK = 278, DxUK = "SDL_SCANCODE_KBDILLUMTOGGLE" });
            dxKeys.Add(new DxKeys { SDLK = 279, DxUK = "SDL_SCANCODE_KBDILLUMDOWN" });
            dxKeys.Add(new DxKeys { SDLK = 280, DxUK = "SDL_SCANCODE_KBDILLUMUP" });
            dxKeys.Add(new DxKeys { SDLK = 281, DxUK = "SDL_SCANCODE_EJECT = 281" });
            dxKeys.Add(new DxKeys { SDLK = 282, DxUK = "Sleep" });

            dxKeys.Add(new DxKeys { SDLK = 283, DxUK = "SDL_SCANCODE_APP1" });
            dxKeys.Add(new DxKeys { SDLK = 284, DxUK = "SDL_SCANCODE_APP2" });

        }



        public static string DXtoSDLCode(string dxString, KeyboardType keyboardType)
        {
            IKeyboardTranslator kbt = new KeyboardTranslationSDL2(keyboardType);

            if (keyboardType == KeyboardType.UK)
            {
                var uk = kbt.dxKeys.Where(a => a.DxUK == dxString).ToList();
                if (uk.Count == 0)
                {
                    return dxString;
                }

                return "keyboard 0x0 " + uk.First().SDLK.ToString();
            }
            return dxString;
        }

        public static string SDLCodetoDx(string sdlCode, KeyboardType keyboardType)
        {
            IKeyboardTranslator kbt = new KeyboardTranslationSDL2(keyboardType);

            if (sdlCode == null || sdlCode == "")
                return "";

            if (keyboardType == KeyboardType.UK)
            {
                var uk = kbt.dxKeys.Where(a => a.SDLK.ToString() == sdlCode.Replace("keyboard 0x0 ", "").TrimEnd()).ToList();
                if (uk.Count == 0)
                {
                    return sdlCode;
                }

                if (uk.First().DxUK.Contains("SDL_SCANCODE_"))
                {
                    // no dx lookup was found
                    //return "keyboard 0x0 " + uk.First().SDLK;
                }

                return uk.First().DxUK;
            }

            return sdlCode;
        }

    }

    

    
}
