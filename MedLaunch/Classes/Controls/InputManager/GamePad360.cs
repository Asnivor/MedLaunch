using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using SlimDX.XInput;
using System.Windows.Interop;
using Microsoft;

#pragma warning disable 169
#pragma warning disable 414

namespace MedLaunch.Classes.Controls
{
    public class GamePad360
    {
        // ********************************** Static interface **********************************

        public static List<GamePad360> Devices = new List<GamePad360>();

        static bool IsAvailable;

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddressOrdinal(IntPtr hModule, IntPtr procName);

        delegate uint XInputGetStateExProcDelegate(uint dwUserIndex, out XINPUT_STATE state);

        static bool HasGetInputStateEx;
        static IntPtr LibraryHandle;
        static XInputGetStateExProcDelegate XInputGetStateExProc;

        struct XINPUT_GAMEPAD
        {
            public ushort wButtons;
            public byte bLeftTrigger;
            public byte bRightTrigger;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
        }

        struct XINPUT_STATE
        {
            public uint dwPacketNumber;
            public XINPUT_GAMEPAD Gamepad;
        }

        public static ControllerInfo[] ContInfoFromLog;

        public static void Dispose()
        {

        }

        static class NativeMethods
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);
        }

        public static void Initialize(MainWindow window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            IsAvailable = false;

            ContInfoFromLog = LogParser.Instance.GetAttachedControllers(true).Where(a => a.Type == ControllerType.XInput).ToArray();

            try
            {
                //some users wont even have xinput installed. in order to avoid spurious exceptions and possible instability, check for the library first
                HasGetInputStateEx = true;
                LibraryHandle = NativeMethods.LoadLibrary("xinput1_3.dll"); // Win32.LoadLibrary("xinput1_3.dll");
                if (LibraryHandle == IntPtr.Zero)
                    LibraryHandle = NativeMethods.LoadLibrary("xinput1_4.dll");
                if (LibraryHandle == IntPtr.Zero)
                {
                    LibraryHandle = NativeMethods.LoadLibrary("xinput9_1_0.dll");
                    HasGetInputStateEx = false;
                }

                if (LibraryHandle != IntPtr.Zero)
                {
                    if (HasGetInputStateEx)
                    {
                        IntPtr proc = GetProcAddressOrdinal(LibraryHandle, new IntPtr(100));
                        XInputGetStateExProc = (XInputGetStateExProcDelegate)Marshal.GetDelegateForFunctionPointer(proc, typeof(XInputGetStateExProcDelegate));
                    }

                    //don't remove this code. it's important to catch errors on systems with broken xinput installs.
                    //(probably, checking for the library was adequate, but lets not get rid of this anyway)
                    var test = new SlimDX.XInput.Controller(UserIndex.One).IsConnected;
                    IsAvailable = true;
                }
                
            }
            catch { }

  

            if (!IsAvailable) return;

            //now, at this point, slimdx may be using one xinput, and we may be using another
            //i'm not sure how slimdx picks its dll to bind to.
            //i'm not sure how troublesome this will be
            //maybe we should get rid of slimdx for this altogether

            var c1 = new Controller(UserIndex.One);
            var c2 = new Controller(UserIndex.Two);
            var c3 = new Controller(UserIndex.Three);
            var c4 = new Controller(UserIndex.Four);



            if (c1.IsConnected)
            {
                Devices.Add(new GamePad360(0, c1, ContInfoFromLog[0]));
            }
            if (c2.IsConnected)
            {
                Devices.Add(new GamePad360(1, c2, ContInfoFromLog[1]));
            }
            if (c3.IsConnected)
            {
                Devices.Add(new GamePad360(2, c3, ContInfoFromLog[2]));
            }
            if (c4.IsConnected)
            {
                Devices.Add(new GamePad360(3, c4, ContInfoFromLog[3]));
            }
        }

        public static void UpdateAll()
        {
            if (IsAvailable)
                foreach (var device in Devices.ToList())
                    device.Update();
        }

        // ********************************** Instance Members **********************************

        readonly Controller controller;
        uint index0;
        XINPUT_STATE state;

        public int PlayerNumber { get { return (int)index0 + 1; } }
        public string ID { get; set; }

        GamePad360(uint index0, Controller c, ControllerInfo id)
        {
            this.index0 = index0;
            this.ID = id.ID;
            controller = c;
            InitializeButtons();
            Update();
        }

        public void Update()
        {
            if (controller.IsConnected == false)
                return;

            if (XInputGetStateExProc != null)
            {
                state = new XINPUT_STATE();
                XInputGetStateExProc(index0, out state);
            }
            else
            {
                var slimstate = controller.GetState();
                state.dwPacketNumber = slimstate.PacketNumber;
                state.Gamepad.wButtons = (ushort)slimstate.Gamepad.Buttons;
                state.Gamepad.sThumbLX = slimstate.Gamepad.LeftThumbX;
                state.Gamepad.sThumbLY = slimstate.Gamepad.LeftThumbY;
                state.Gamepad.sThumbRX = slimstate.Gamepad.RightThumbX;
                state.Gamepad.sThumbRY = slimstate.Gamepad.RightThumbY;
                state.Gamepad.bLeftTrigger = slimstate.Gamepad.LeftTrigger;
                state.Gamepad.bRightTrigger = slimstate.Gamepad.RightTrigger;
            }
        }

        public IEnumerable<Tuple<string, float>> GetFloats()
        {
            var g = state.Gamepad;
            const float f = 3.2768f;
            yield return new Tuple<string, float>("LeftThumbX", g.sThumbLX / f);
            yield return new Tuple<string, float>("LeftThumbY", g.sThumbLY / f);
            yield return new Tuple<string, float>("RightThumbX", g.sThumbRX / f);
            yield return new Tuple<string, float>("RightThumbY", g.sThumbRY / f);
            yield break;
        }

        public int NumButtons { get; private set; }

        private readonly List<string> names = new List<string>();
        private readonly List<Func<bool>> actions = new List<Func<bool>>();

        void InitializeButtons()
        {
            const int dzp = 9000;
            const int dzn = -9000;
            const int dzt = 40;

            AddItem("joystick " + ID + " " + "0000000c", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.A) != 0);                   // A
            AddItem("joystick " + ID + " " + "0000000d", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.B) != 0);                   // B
            AddItem("joystick " + ID + " " + "0000000e", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.X) != 0);                   // X
            AddItem("joystick " + ID + " " + "0000000f", () => (state.Gamepad.wButtons & unchecked((ushort)GamepadButtonFlags.Y)) != 0);        // Y
            AddItem("joystick " + ID + " " + "0000000a", () => (state.Gamepad.wButtons & 1024) != 0);

            AddItem("joystick " + ID + " " + "00000004", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.Start) != 0);                       // Start
            AddItem("joystick " + ID + " " + "00000005", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.Back) != 0);                         // Back
            AddItem("joystick " + ID + " " + "00000006", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.LeftThumb) != 0);
            AddItem("joystick " + ID + " " + "00000007", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.RightThumb) != 0);
            AddItem("joystick " + ID + " " + "00000008", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.LeftShoulder) != 0);         // LeftShoulder
            AddItem("joystick " + ID + " " + "00000009", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.RightShoulder) != 0);       // RightShoulder

            AddItem("joystick " + ID + " " + "00000000", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.DPadUp) != 0);                     // DpadUp
            AddItem("joystick " + ID + " " + "00000001", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.DPadDown) != 0);                 // DpadDown
            AddItem("joystick " + ID + " " + "00000002", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.DPadLeft) != 0);                 // DpadLeft
            AddItem("joystick " + ID + " " + "00000003", () => (state.Gamepad.wButtons & (ushort)GamepadButtonFlags.DPadRight) != 0);               // DpadRight

            AddItem("joystick " + ID + " " + "00008001", () => state.Gamepad.sThumbLY >= dzp);       // LStickUp
            AddItem("joystick " + ID + " " + "0000c001", () => state.Gamepad.sThumbLY <= dzn);       // LStickDown
            AddItem("joystick " + ID + " " + "0000c000", () => state.Gamepad.sThumbLX <= dzn);       // LStickLeft
            AddItem("joystick " + ID + " " + "00008000", () => state.Gamepad.sThumbLX >= dzp);       // LStickRight

            AddItem("joystick " + ID + " " + "00008003", () => state.Gamepad.sThumbRY >= dzp);       // RStickUp
            AddItem("joystick " + ID + " " + "0000c003", () => state.Gamepad.sThumbRY <= dzn);       // RStickDown
            AddItem("joystick " + ID + " " + "0000c002", () => state.Gamepad.sThumbRX <= dzn);       // RStickLeft
            AddItem("joystick " + ID + " " + "00008002", () => state.Gamepad.sThumbRX >= dzp);       // RStickRight

            AddItem("joystick " + ID + " " + "00008004", () => state.Gamepad.bLeftTrigger > dzt);    // LeftTrigger
            AddItem("joystick " + ID + " " + "00008005", () => state.Gamepad.bRightTrigger > dzt);   // RightTrigger
        }

        void AddItem(string name, Func<bool> pressed)
        {
            names.Add(name);
            actions.Add(pressed);
            NumButtons++;
        }

        public string ButtonName(int index)
        {
            return names[index];
        }

        public bool Pressed(int index)
        {
            return actions[index]();
        }
    }
}
