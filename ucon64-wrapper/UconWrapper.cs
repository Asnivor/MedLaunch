using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ucon64_wrapper
{
    public class UconWrapper
    {
        public string UconExePath { get; set; }
        public string GamePath { get; set; }
        public string OutputFolder { get; set; }

        public UconWrapper(string uconPath)
        {
            UconExePath = uconPath;
        }

        public UconResult ProcessSMD(string gamepath)
        {
            return ProcessSMD(gamepath, OutputFolder);
        }

        public UconResult ProcessSMD(string gamepath, string outputFolder)
        {
            if (outputFolder != null)
                OutputFolder = outputFolder;

            GamePath = gamepath;

            // do initial scan
            UconResult u = ScanGame(gamepath, SystemType.Genesis);

            if (u.Data.IsChecksumValid)
            {
                // we can proceed
                if (u.Data.IsInterleaved == true)
                {
                    // genesis ROM is interleaved - attempt to convert
                    ConvertSMDToBin(u);
                }
                else
                {
                    // genesis ROM is NOT interleaved - this should work fine with no modifications
                    u.ConvertedPath = u.Data.RomPath;                    
                }
            }
            else
            {
                // checksum not valid - do nothing
                return null;
            }

            return u;
        }

        public string DoTestScan(string gamepath, SystemType systemType)
        {
            string options = UconExePath + GetForceSystemString(systemType) + '"' + gamepath + '"';
            string result = RunCommand(options);
            return result;
        }

        public UconResult ConvertSMDToBin(UconResult input)
        {
            string options = UconExePath + GetForceSystemString(SystemType.Genesis) + "--bin " + "-o=\"" + OutputFolder + "\" " +  "\"" + input.Data.RomPath + "\"";
            string result = RunCommand(options);
            input.RawOutput = result;
            ParseOutput(input);

            return input;
        }

        public UconResult ScanGame(string gamepath, SystemType systemType)
        {
            UconResult u = new UconResult();
            string options = UconExePath + GetForceSystemString(systemType) + '"' + gamepath + '"';
            string result = RunCommand(options);
            u.RawOutput = result.Replace("Create: NTUSER.idx", "")
                .Replace("WARNING: \"NTUSER.DAT\" is meant for a console unknown to uCON64", "")
                .Replace("\r\n\r\n\r\n\r\n\r\n", "\r\n");
            u.Data.systemType = systemType;
            u.Data.RomPath = gamepath;

            ParseOutput(u);

            /*
            if (u.Data.IsChecksumValid == true)
                u.Status = "Detected Checksum is valid";
            else if (u.Data.IsChecksumValid == false)
                u.Status = "Detected Checksum is invalid";
            else
                u.Status = "Checksum could not be determined";

            */
            return u;
        }

        public UconResult ParseOutput(UconResult resultObj)
        {
            string raw = resultObj.RawOutput;
            string[] arr = raw.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // iterate through result string array
            bool dataStarted = false;
            int gameInfoCounter = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                // skip intro lines
                if (dataStarted == false)
                {
                    // check for rompath line
                    if (arr[i].StartsWith(resultObj.Data.RomPath))
                    {
                        dataStarted = true;
                    }
                    //continue;
                }

                /* now we should be into the actual data we want */
                // header detection
                if (arr[i].StartsWith("0000"))
                {
                    resultObj.Data.Header += arr[i] + "\n";
                }

                // rom format
                if (IsRomFormat(arr[i]) == true)
                {
                    resultObj.Data.DetectedRomType = arr[i];
                    resultObj.Data.romType = GetRomType(arr[i]);
                }

                // system type
                if (IsSystemType(arr[i]) == true)
                {
                    resultObj.Data.systemType = GetSystemType(arr[i]);
                    resultObj.Data.DetectedSystemType = arr[i];
                    // start game info counter (next 4 lines are information about the rom)
                    gameInfoCounter++;
                    if (arr[i].Trim() == "PC-Engine (CD Unit/Core Grafx(II)/Shuttle/GT/LT/Super CDROM/DUO(-R(X)))")
                    {
                        // pcengine system name takes up two lines
                        i++;
                    }
                    continue;
                }

                switch (gameInfoCounter)
                {
                    case 1:
                        // detected game name
                        resultObj.Data.DetectedGameName = arr[i];
                        gameInfoCounter++;
                        break;
                    case 2:
                        // detected publisher
                        resultObj.Data.DetectedPublisher = arr[i];
                        gameInfoCounter++;
                        break;
                    case 3:
                        // detected region data
                        resultObj.Data.DetectedRegion = arr[i];
                        gameInfoCounter++;
                        break;
                    case 4:
                        // detected size data
                        resultObj.Data.DetectedSize = arr[i];
                        gameInfoCounter++;
                        break;
                    default:
                        // do nothing
                        break;
                }

                // Interleaved
                if (arr[i].StartsWith("Interleaved/Swapped"))
                {
                    string[] inters = arr[i].Split(':');
                    if (inters[1].Trim() == "Yes")
                    {
                        resultObj.Data.IsInterleaved = true;
                    }
                    else
                    {
                        resultObj.Data.IsInterleaved = false;
                    }
                }

                // checksum comprison
                if (arr[i].StartsWith("Checksum:"))
                {
                    string[] chks = arr[i].Split(':');
                    string dChks = chks[1];
                    if (dChks.Contains(" OK"))
                    {
                        resultObj.Data.IsChecksumValid = true;
                    }
                    else
                    {
                        resultObj.Data.IsChecksumValid = false;
                    }

                    resultObj.Data.DetectedChecksumComparison = dChks;
                }
                if (arr[i].StartsWith("Checksum (CRC32):"))
                {
                    string crc = arr[i].Replace("Checksum (CRC32): ", "").Trim();
                    resultObj.Data.CRC32 = crc;
                }

                // output rom path
                if (arr[i].StartsWith("Wrote output to "))
                {
                    string path = arr[i].Replace("Wrote output to ", "").Trim();
                    resultObj.ConvertedPath = path;
                }

                // year
                if (arr[i].StartsWith("Date:"))
                {
                    string[] dates = arr[i].Split(':');
                    string date = dates[1];
                    resultObj.Data.DetectedYear = date.Trim();
                }

                // version
                if (arr[i].StartsWith("Version:"))
                {
                    string[] versions = arr[i].Split(':');
                    string version = versions[1];
                    resultObj.Data.DetectedVersion = version.Trim();
                }

                // padding
                if (arr[i].StartsWith("Padded:"))
                {
                    string[] pads = arr[i].Split(':');
                    string pad = pads[1];
                    resultObj.Data.DetectedPadding = pad.Trim();
                }
            }

            return resultObj;
        }

        

        public static bool IsSystemType(string input)
        {
            switch (input.Trim())
            {
                case "Genesis/Sega Mega Drive/Sega CD/32X/Nomad":                
                    return true;
                case "Nintendo Entertainment System/NES/Famicom/Game Axe (Redant)":
                    return true;
                case "Super Nintendo Entertainment System/SNES/Super Famicom":
                    return true;
                case "Neo Geo Pocket/Neo Geo Pocket Color":
                    return true;
                case "Game Boy/(Super GB)/GB Pocket/Color GB":
                    return true;
                case "Handy (prototype)/Lynx/Lynx II":
                    return true;
                case "Game Boy Advance (SP)":
                    return true;
                case "Nintendo Virtual Boy":
                    return true;
                case "PC-Engine (CD Unit/Core Grafx(II)/Shuttle/GT/LT/Super CDROM/DUO(-R(X)))":
                    return true;
                case "Sega Master System(II/III)/Game Gear (Handheld)":
                    return true;
                case "WonderSwan/WonderSwan Color/SwanCrystal":
                    return true;
                default:
                    return false;
            }
        }

        public static SystemType GetSystemType(string input)
        {
            switch (input.Trim())
            {
                case "Genesis/Sega Mega Drive/Sega CD/32X/Nomad":
                    return SystemType.Genesis;
                case "Nintendo Entertainment System/NES/Famicom/Game Axe (Redant)":
                    return SystemType.NES;
                case "Super Nintendo Entertainment System/SNES/Super Famicom":
                    return SystemType.SNES;
                default:
                    return SystemType.None;
            }
        }

        public static bool IsRomFormat(string input)
        {
            switch (input.Trim())
            {
                case "Super Com Pro/Super Magic Drive/SMD":
                    return true;
                case "Magicom/BIN/RAW":
                    return true;
                case "Famicom Disk System file (diskimage)":
                    return true;
                case "Multi Game Doctor (2)/Multi Game Hunter/MGH":
                    return true;
                case "Pocket Linker":
                    return true;
                case "Unknown backup unit/emulator":
                    return true;
                case "Flash Advance Linker":
                    return true;
                default:
                    return false;
            }
        }

        public static RomType GetRomType(string input)
        {
            switch (input.Trim())
            {
                case "Super Com Pro/Super Magic Drive/SMD":
                    return RomType.SMD;
                case "Magicom/BIN/RAW":
                    return RomType.BIN;
                default:
                    return RomType.Unknown;
            }
        }


        public UconResult InterrogateSMD()
        {
            UconResult u = new UconResult();

            string result = RunCommand("--gen " + GamePath);

            return u;
        }     

        private string RunCommand(string options)
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = UconExePath;
            p.StartInfo.Arguments = options;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

        private string GetForceSystemString(SystemType systemType)
        {
            switch (systemType)
            {
                case SystemType.Genesis:
                    return " --gen ";
                case SystemType.Gameboy:
                    return " --gb ";
                case SystemType.GameboyAdvance:
                    return " --gba ";
                case SystemType.GenericDisc:
                    return " --disc ";
                case SystemType.NES:
                    return " --nes ";
                case SystemType.SNES:
                    return " --snes ";
                case SystemType.NeoGeoPocket:
                    return " --ngp ";
                case SystemType.PCEngine:
                    return " --pce ";
                case SystemType.Playstation:
                    return " --psx ";
                case SystemType.SMS:
                    return " --sms ";
                case SystemType.VirtualBoy:
                    return " --vboy ";
                case SystemType.WonderSwan:
                    return " --swan ";
                case SystemType.Lynx:
                    return " --lynx ";

                default:
                    return "";
            }
        }

        
    }
}
