using SlimDX;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace MedLaunch.Classes.Controls.InputManager
{
    public struct KeyEvent
    {
        public Key Key;
        public bool Pressed;
    }

    public static class KeyInput
    {
        private static DirectInput dinput;
        private static Keyboard keyboard;
        private static KeyboardState state = new KeyboardState();

        public static void Dispose()
        {
            dinput.Dispose();
        }
        

        public static void Initialize(MainWindow window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;

            if (dinput == null)
                dinput = new DirectInput();

            if (keyboard == null || keyboard.Disposed)
            {
                keyboard = new Keyboard(dinput);
                keyboard.SetCooperativeLevel(handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);
                keyboard.Properties.BufferSize = 8;
            }

        }

        static List<KeyEvent> EmptyList = new List<KeyEvent>();
        static List<KeyEvent> EventList = new List<KeyEvent>();

        public static IEnumerable<KeyEvent> Update()
        {
            EventList.Clear();

            if (keyboard.Acquire().IsFailure)
                return EmptyList;
            if (keyboard.Poll().IsFailure)
                return EmptyList;
            

            for (;;)
            {
                var events = keyboard.GetBufferedData();
                if (Result.Last.IsFailure)
                    return EventList;
                if (events.Count == 0)
                    break;
                foreach (var e in events)
                {
                    foreach (var k in e.PressedKeys)
                        EventList.Add(new KeyEvent { Key = k, Pressed = true });
                    foreach (var k in e.ReleasedKeys)
                        EventList.Add(new KeyEvent { Key = k, Pressed = false });
                }
            }

            return EventList;
        }
    }
}
