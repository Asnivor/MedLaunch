using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.DirectInput;
using MedLaunch.Classes.Controls.InputManager;

namespace MedLaunch.Classes.Controls
{

    public struct di_axis_info
    {
        public int minimum;
        public int maximum;
        public int jd_logical_offset;
    }

    public struct ButtConfig
    {
        public byte ButtType;
        public byte DeviceNum;
        public UInt32 ButtonNum;
        public UInt64 DeviceId;
    }

    public class GamePad
    {
        // ********************************** Static interface **********************************

        static DirectInput dinput;
        public static List<GamePad> Devices;
        public static List<di_axis_info> DIAxisInfo;
        public static List<ButtConfig> BConfig;
        

        //public static Capabilities DevCaps;

        public static void Initialize()
        {
            if (dinput == null)
                dinput = new DirectInput();

            Devices = new List<GamePad>();

            DIAxisInfo = new List<di_axis_info>();

            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                Console.WriteLine("joydevice: {0} `{1}`", device.InstanceGuid, device.ProductName);

                if (device.ProductName.Contains("XBOX 360"))
                    continue; // Don't input XBOX 360 controllers into here; we'll process them via XInput (there are limitations in some trigger axes when xbox pads go over xinput)

                var joystick = new Joystick(dinput, device.InstanceGuid);
                //joystick.SetCooperativeLevel(GlobalWin.MainForm.Handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);

                
                int count = 0;
                foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
                {                                        
                    if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    {
                        joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-1000, 1000);                        
                    }

                    InputRange diprg = joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).LogicalRange;
                    int min = diprg.Minimum;
                    int max = diprg.Maximum;

                    if (min < max)
                    {
                        di_axis_info dai = new di_axis_info();
                        dai.jd_logical_offset = count;
                        dai.maximum = max;
                        dai.minimum = min;
                        DIAxisInfo.Add(dai);
                    }
                    count++;   
                }
                joystick.Acquire();
                

                GamePad p = new GamePad(device.InstanceName, device.InstanceGuid, joystick, DIAxisInfo);
                Devices.Add(p);
            }
        }

        public static int HatToAxisCompat(int hat)
        {
            return (DIAxisInfo.Count() + (hat * 2));
        }

        public static void UpdateAll()
        {
            foreach (var device in Devices.ToList())
                device.Update();
        }

        public static void CloseAll()
        {
            if (Devices != null)
            {
                foreach (var device in Devices)
                    device.joystick.Dispose();
                Devices.Clear();
            }
        }

        

        // ********************************** Instance Members **********************************

        readonly string name;
        readonly Guid guid;
        readonly Joystick joystick;

        public int num_rel_axes;
        public int num_axes;
        public int num_buttons;
        
        JoystickState state = new JoystickState();

        GamePad(string name, Guid guid, Joystick joystick, List<di_axis_info> DIAxisInfo)
        {
            this.name = name;
            this.guid = guid;
            this.joystick = joystick;

            num_rel_axes = 0;
            num_axes = DIAxisInfo.Count() + joystick.Capabilities.PovCount * 2;
            num_buttons = joystick.Capabilities.ButtonCount;

            Update();

            

            // buttons
            /*
            for (int button = 0; button < joystick.Capabilities.ButtonCount; button++)
            {
                bool button_state = state.GetButtons()[button];
                UInt32 butnameint = Convert.ToUInt32(button);
                string na = butnameint.ToString("X8");
                names.Add(butnameint.ToString("X8"));
            }
            */

            // axis
            for (int axis = 0; axis < num_axes; axis++)
            {
                
            }

            // hats
            for (int hat = 0; hat < joystick.Capabilities.PovCount; hat++)
            {

            }

            InitializeCallbacks();
        }

        public void Update()
        {
            try
            {
                if (joystick.Acquire().IsFailure)
                    return;
            }
            catch
            {
                return;
            }
            if (joystick.Poll().IsFailure)
                return;

            state = joystick.GetCurrentState();
            
            if (Result.Last.IsFailure)
                // do something?
                return;
        }

        public IEnumerable<Tuple<string, float>> GetFloats()
        {
            var pis = typeof(JoystickState).GetProperties();
            foreach (var pi in pis)
                yield return new Tuple<string, float>(pi.Name, 10.0f * (float)(int)pi.GetValue(state, null));
        }

        /// <summary>FOR DEBUGGING ONLY</summary>
        public JoystickState GetInternalState()
        {
            return state;
        }

        public string Name { get { return name; } }
        public Guid Guid { get { return guid; } }


        public string ButtonName(int index)
        {
            return names[index];
        }
        public bool Pressed(int index)
        {
            return actions[index]();
        }
        public int NumButtons { get; private set; }

        private readonly List<string> names = new List<string>();
        private readonly List<Func<bool>> actions = new List<Func<bool>>();

        void AddItem(string _name, Func<bool> callback)
        {
            names.Add(_name);
            actions.Add(callback);
            NumButtons++;
        }

        void InitializeCallbacks()
        {
            const int dzp = 400;
            const int dzn = -400;

            names.Clear();
            actions.Clear();
            NumButtons = 0;

            // buttons first
            for (int i = 0; i < state.GetButtons().Length; i++)
            {
                int j = i;
                bool button_state = state.GetButtons()[i];
                UInt32 butnameint = Convert.ToUInt32(i);
                string na = butnameint.ToString("X8");
                ulong iD = IdGenerator.CalcOldStyleID(DIAxisInfo.Count, 0, joystick.Capabilities.PovCount, joystick.Capabilities.ButtonCount);

                AddItem(iD.ToString("X16") + " " + butnameint.ToString("X8"), () => state.IsPressed(j));
            }

            // axis
            int c = 0;
            foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
            {
                c++;
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                {

                }
                if (deviceObject.ObjectType == ObjectDeviceType.Axis)
                {

                }

                    string aName = deviceObject.Name;
                //joystick.GetObjectPropertiesById((int)deviceObject.ObjectType)
            }

            
            int numa = num_axes;
            int numa2 = DIAxisInfo.Count();
            for (int axis = 0; axis < num_axes; axis++)
            {
                
            }


            AddItem("AccelerationX+", () => state.AccelerationX >= dzp);
            AddItem("AccelerationX-", () => state.AccelerationX <= dzn);
            AddItem("AccelerationY+", () => state.AccelerationY >= dzp);
            AddItem("AccelerationY-", () => state.AccelerationY <= dzn);
            AddItem("AccelerationZ+", () => state.AccelerationZ >= dzp);
            AddItem("AccelerationZ-", () => state.AccelerationZ <= dzn);
            AddItem("AngularAccelerationX+", () => state.AngularAccelerationX >= dzp);
            AddItem("AngularAccelerationX-", () => state.AngularAccelerationX <= dzn);
            AddItem("AngularAccelerationY+", () => state.AngularAccelerationY >= dzp);
            AddItem("AngularAccelerationY-", () => state.AngularAccelerationY <= dzn);
            AddItem("AngularAccelerationZ+", () => state.AngularAccelerationZ >= dzp);
            AddItem("AngularAccelerationZ-", () => state.AngularAccelerationZ <= dzn);
            AddItem("AngularVelocityX+", () => state.AngularVelocityX >= dzp);
            AddItem("AngularVelocityX-", () => state.AngularVelocityX <= dzn);
            AddItem("AngularVelocityY+", () => state.AngularVelocityY >= dzp);
            AddItem("AngularVelocityY-", () => state.AngularVelocityY <= dzn);
            AddItem("AngularVelocityZ+", () => state.AngularVelocityZ >= dzp);
            AddItem("AngularVelocityZ-", () => state.AngularVelocityZ <= dzn);
            AddItem("ForceX+", () => state.ForceX >= dzp);
            AddItem("ForceX-", () => state.ForceX <= dzn);
            AddItem("ForceY+", () => state.ForceY >= dzp);
            AddItem("ForceY-", () => state.ForceY <= dzn);
            AddItem("ForceZ+", () => state.ForceZ >= dzp);
            AddItem("ForceZ-", () => state.ForceZ <= dzn);
            AddItem("RotationX+", () => state.RotationX >= dzp);
            AddItem("RotationX-", () => state.RotationX <= dzn);
            AddItem("RotationY+", () => state.RotationY >= dzp);
            AddItem("RotationY-", () => state.RotationY <= dzn);
            AddItem("00008003", () => state.RotationZ >= dzp); //AddItem("RotationZ+", () => state.RotationZ >= dzp);
            AddItem("0000c003", () => state.RotationZ <= dzn); //AddItem("RotationZ-", () => state.RotationZ <= dzn);
            AddItem("TorqueX+", () => state.TorqueX >= dzp);
            AddItem("TorqueX-", () => state.TorqueX <= dzn);
            AddItem("TorqueY+", () => state.TorqueY >= dzp);
            AddItem("TorqueY-", () => state.TorqueY <= dzn);
            AddItem("TorqueZ+", () => state.TorqueZ >= dzp);
            AddItem("TorqueZ-", () => state.TorqueZ <= dzn);
            AddItem("VelocityX+", () => state.VelocityX >= dzp);
            AddItem("VelocityX-", () => state.VelocityX <= dzn);
            AddItem("VelocityY+", () => state.VelocityY >= dzp);
            AddItem("VelocityY-", () => state.VelocityY <= dzn);
            AddItem("VelocityZ+", () => state.VelocityZ >= dzp);
            AddItem("VelocityZ-", () => state.VelocityZ <= dzn);
            AddItem("00008000", () => state.X >= dzp); //AddItem("X+", () => state.X >= dzp);
            AddItem("0000c000", () => state.X <= dzn); //AddItem("X-", () => state.X <= dzn);
            AddItem("00008001", () => state.Y >= dzp); //AddItem("Y+", () => state.Y >= dzp);
            AddItem("0000c001", () => state.Y <= dzn); //AddItem("Y-", () => state.Y <= dzn);
            AddItem("00008002", () => state.Z >= dzp); //AddItem("Z+", () => state.Z >= dzp);
            AddItem("0000c002", () => state.Z <= dzn); //AddItem("Z-", () => state.Z <= dzn);

            // i don't know what the "Slider"s do, so they're omitted for the moment

            for (int i = 0; i < state.GetButtons().Length; i++)
            {
                //int j = i;
                //AddItem(string.Format("B{0}", i + 1), () => state.IsPressed(j));
            } 

            for (int i = 0; i < state.GetPointOfViewControllers().Length; i++)
            {
                int j = i;
                AddItem(string.Format("POV{0}U", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });
                AddItem(string.Format("POV{0}D", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 13500 && t <= 22500; });
                AddItem(string.Format("POV{0}L", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 22500 && t <= 31500; });
                AddItem(string.Format("POV{0}R", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 4500 && t <= 13500; });
            }
        }

        // Note that this does not appear to work at this time. I probably need to have more infos.
        public void SetVibration(int left, int right)
        {
            int[] temp1, temp2;
            // my first clue that it doesnt work is that LEFT  and RIGHT _ARENT USED_
            // I should just look for C++ examples instead of trying to look for SlimDX examples

            var parameters = new EffectParameters
            {
                Duration = 0x2710,
                Gain = 0x2710,
                SamplePeriod = 0,
                TriggerButton = 0,
                TriggerRepeatInterval = 0x2710,
                Flags = EffectFlags.None
            };
            parameters.GetAxes(out temp1, out temp2);
            parameters.SetAxes(temp1, temp2);
            var effect = new Effect(joystick, EffectGuid.ConstantForce);
            effect.SetParameters(parameters);
            effect.Start(1);
        }
    }
}
