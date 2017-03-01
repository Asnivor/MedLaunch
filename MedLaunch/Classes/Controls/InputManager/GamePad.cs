using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.DirectInput;
using MedLaunch.Classes.Controls.InputManager;
using MedLaunch.Extensions;
using System.Reflection;

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
        public static ControllerInfo[] ContInfoFromLog;
        

        public static void Initialize()
        {
            if (dinput == null)
                dinput = new DirectInput();

            Devices = new List<GamePad>();

            ContInfoFromLog = LogParser.GetDirectInputControllerIds();

            int count = 0;
            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                Console.WriteLine("joydevice: {0} `{1}`", device.InstanceGuid, device.ProductName);

                if (device.ProductName.Contains("XBOX 360"))
                    continue; // Don't input XBOX 360 controllers into here; we'll process them via XInput (there are limitations in some trigger axes when xbox pads go over xinput)

                var joystick = new Joystick(dinput, device.InstanceGuid);
                //joystick.SetCooperativeLevel(GlobalWin.MainForm.Handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);

                
                
                foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
                {                                        
                    if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    {
                        joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-1000, 1000);                        
                    }
                }
                joystick.Acquire();

                string nId = ContInfoFromLog[count].ID;
                

                GamePad p = new GamePad(device.InstanceName, nId, joystick);
                Devices.Add(p);
                count++;
            }
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

        public int HatToAxisCompat(int hat)
        {
            return (DIAxisInfo.Count() + (hat * 2));
        }


        // ********************************** Instance Members **********************************

        readonly string name;
        //readonly Guid guid;
        
        readonly string id;
        readonly Joystick joystick;

        public int num_rel_axes;
        public int num_axes;
        public int num_hats;
        public int num_buttons;

        public List<di_axis_info> DIAxisInfo;
        public List<ButtConfig> BConfig;

        public List<Int16> axis_state;
        public List<int> rel_axis_state;
        public List<bool> button_state;

        JoystickState state = new JoystickState();
        
        
        GamePad(string name, string Id, Joystick joystick)
        {
            this.name = name;
            this.id = Id;
            this.joystick = joystick;

            DIAxisInfo = new List<di_axis_info>();
            axis_state = new List<short>();
            rel_axis_state = new List<int>();
            button_state = new List<bool>();

            // get axis info
            int count = 0;
            foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
            {
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

            num_rel_axes = 0;
            num_axes = DIAxisInfo.Count() + joystick.Capabilities.PovCount * 2;
            num_buttons = joystick.Capabilities.ButtonCount;

            /* States */
            axis_state.Resize(num_axes);
            rel_axis_state.Resize(num_rel_axes);
            button_state.Resize(num_buttons);

            // buttons
            for (int button = 0; button < joystick.Capabilities.ButtonCount; button++)
            {
                if (state.GetButtons()[button] == true)
                    button_state[button] = true;
                else
                    button_state[button] = false;
            }

            // axis
            unsafe
            { 
                for (int axis = 0; axis < DIAxisInfo.Count; axis++)
                {
                    int X = state.X;
                    Int64 rv = (&X)[DIAxisInfo[axis].jd_logical_offset];

                    rv = (((rv - DIAxisInfo[axis].minimum) * 32767 * 2) / (DIAxisInfo[axis].maximum - DIAxisInfo[axis].minimum)) - 32767;
                    if (rv < -32767)
                        rv = -32767;

                    if (rv > 32767)
                        rv = 32767;
                    axis_state[axis] = Convert.ToInt16(rv);
                }
            }

            // hats
            for (int hat = 0; hat < joystick.Capabilities.PovCount; hat++)
            {
                uint hat_val = Convert.ToUInt32(state.GetPointOfViewControllers()[hat]);

                if (hat_val >= 36000)
                {
                    axis_state[(DIAxisInfo.Count + hat * 2) + 0] = 0;
                    axis_state[(DIAxisInfo.Count + hat * 2) + 1] = 0;
                }
                else
                {
                    int x = 0;
                    int y = 0;
                    uint octant = (hat_val / 4500) & 0x7;
                    int octant_doff = Convert.ToInt32(hat_val) % 4500;

                    switch(octant)
                    {
                        case 0: x = octant_doff * 32767 / 4500; y = -32767; break;
                        case 1: x = 32767; y = (-4500 + octant_doff) * 32767 / 4500; break;

                        case 2: x = 32767; y = octant_doff * 32767 / 4500; break;
                        case 3: x = (4500 - octant_doff) * 32767 / 4500; y = 32767; break;

                        case 4: x = (-octant_doff) * 32767 / 4500; y = 32767; break;
                        case 5: x = -32767; y = (4500 - octant_doff) * 32767 / 4500; break;

                        case 6: x = -32767; y = (-octant_doff) * 32767 / 4500; break;
                        case 7: x = (-4500 + octant_doff) * 32767 / 4500; y = -32767; break;
                    }

                    axis_state[(DIAxisInfo.Count + hat * 2) + 0] = Convert.ToInt16(x);
                    axis_state[(DIAxisInfo.Count + hat * 2) + 1] = Convert.ToInt16(y);
                }
                
            }

            // get mednafen id
            //id = IdGenerator.CalcOldStyleID(DIAxisInfo.Count, 0, joystick.Capabilities.PovCount, num_buttons).ToString("X16");


            


            Update();

            

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
        public string Id { get { return id; } }


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
            int le = num_buttons;
            int leax = num_axes;
            int lehat = num_hats;

            // buttons first
            for (int i = 0; i < num_buttons; i++)
            {
                int j = i;
                bool button_state = state.GetButtons()[i];
                UInt32 butnameint = Convert.ToUInt32(i);
                string na = butnameint.ToString("X8");
                //ulong iD = IdGenerator.CalcOldStyleID(DIAxisInfo.Count, 0, joystick.Capabilities.PovCount, joystick.Capabilities.ButtonCount);

                AddItem(id + " " + butnameint.ToString("X8").ToLower(), () => state.IsPressed(j));
            }

            // axis state names
            List<string> states = new List<string>();
            foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
            {
                
                if (deviceObject.Aspect == ObjectAspect.Position)
                {
                    if (!deviceObject.Name.ToLower().Contains("hat"))
                        states.Add(deviceObject.Name);
                }

            }
            
            // iterate through each axis and generate names and callbacks
            for (int axis = 0; axis < states.Count; axis++)
            {
                // dynamically create callback
                Func<bool> cbPos;
                Func<bool> cbNeg;

                // + states
                switch (states[axis])
                {
                    case "X Acceleration": cbPos = () => state.AccelerationX >= dzp; break;
                    case "Y Acceleration": cbPos = () => state.AccelerationY >= dzp; break;
                    case "Z Acceleration": cbPos = () => state.AccelerationZ >= dzp; break;
                    case "X AngularAcceleration": cbPos = () => state.AngularAccelerationX >= dzp; break;
                    case "Y AngularAcceleration": cbPos = () => state.AngularAccelerationY >= dzp; break;
                    case "Z AngularAcceleration": cbPos = () => state.AngularAccelerationZ >= dzp; break;
                    case "X AngularVelocity": cbPos = () => state.AngularVelocityX >= dzp; break;
                    case "Y AngularVelocity": cbPos = () => state.AngularVelocityY >= dzp; break;
                    case "Z AngularVelocity": cbPos = () => state.AngularVelocityZ >= dzp; break;
                    case "X Force": cbPos = () => state.ForceX >= dzp; break;
                    case "Y Force": cbPos = () => state.ForceY >= dzp; break;
                    case "Z Force": cbPos = () => state.ForceZ >= dzp; break;
                    case "X Rotation": cbPos = () => state.RotationX >= dzp; break;
                    case "Y Rotation": cbPos = () => state.RotationY >= dzp; break;
                    case "Z Rotation": cbPos = () => state.RotationZ >= dzp; break;
                    case "X Torque": cbPos = () => state.TorqueX >= dzp; break;
                    case "Y Torque": cbPos = () => state.TorqueY >= dzp; break;
                    case "Z Torque": cbPos = () => state.TorqueZ >= dzp; break;
                    case "X Velocity": cbPos = () => state.VelocityX >= dzp; break;
                    case "Y Velocity": cbPos = () => state.VelocityY >= dzp; break;
                    case "Z Velocity": cbPos = () => state.VelocityZ >= dzp; break;
                    case "X Axis": cbPos = () => state.X >= dzp; break;
                    case "Y Axis": cbPos = () => state.Y >= dzp; break;
                    case "Z Axis": cbPos = () => state.Z >= dzp; break;
                    default: cbPos = null; break;
                }
                if (cbPos == null)
                    continue;


                // - states
                switch (states[axis])
                {
                    case "X Acceleration": cbNeg = () => state.AccelerationX >= dzn; break;
                    case "Y Acceleration": cbNeg = () => state.AccelerationY >= dzn; break;
                    case "Z Acceleration": cbNeg = () => state.AccelerationZ >= dzn; break;
                    case "X AngularAcceleration": cbNeg = () => state.AngularAccelerationX >= dzn; break;
                    case "Y AngularAcceleration": cbNeg = () => state.AngularAccelerationY >= dzn; break;
                    case "Z AngularAcceleration": cbNeg = () => state.AngularAccelerationZ >= dzn; break;
                    case "X AngularVelocity": cbNeg = () => state.AngularVelocityX >= dzn; break;
                    case "Y AngularVelocity": cbNeg = () => state.AngularVelocityY >= dzn; break;
                    case "Z AngularVelocity": cbNeg = () => state.AngularVelocityZ >= dzn; break;
                    case "X Force": cbNeg = () => state.ForceX >= dzn; break;
                    case "Y Force": cbNeg = () => state.ForceY >= dzn; break;
                    case "Z Force": cbNeg = () => state.ForceZ >= dzn; break;
                    case "X Rotation": cbNeg = () => state.RotationX >= dzn; break;
                    case "Y Rotation": cbNeg = () => state.RotationY >= dzn; break;
                    case "Z Rotation": cbNeg = () => state.RotationZ >= dzn; break;
                    case "X Torque": cbNeg = () => state.TorqueX >= dzn; break;
                    case "Y Torque": cbNeg = () => state.TorqueY >= dzn; break;
                    case "Z Torque": cbNeg = () => state.TorqueZ >= dzn; break;
                    case "X Velocity": cbNeg = () => state.VelocityX >= dzn; break;
                    case "Y Velocity": cbNeg = () => state.VelocityY >= dzn; break;
                    case "Z Velocity": cbNeg = () => state.VelocityZ >= dzn; break;
                    case "X Axis": cbNeg = () => state.X >= dzn; break;
                    case "Y Axis": cbNeg = () => state.Y >= dzn; break;
                    case "Z Axis": cbNeg = () => state.Z >= dzn; break;
                    default: cbNeg = null; break;
                }
                if (cbNeg == null)
                    continue;

                // create hex format string
                string start = "0000";

                string pos = "8";
                string neg = "c";

                // apply name and callback based on positive and negative axis

                // (+)   
                StringBuilder sbPos = new StringBuilder();
                sbPos.Append(start);
                sbPos.Append(pos);
                sbPos.Append("00");
                sbPos.Append(axis);
                AddItem(id + " " + sbPos.ToString().ToLower(), cbPos);

                // (-)
                StringBuilder sbNeg = new StringBuilder();
                sbNeg.Append(start);
                sbNeg.Append(neg);
                sbNeg.Append("00");
                sbNeg.Append(axis);
                AddItem(id + " " + sbNeg.ToString().ToLower(), cbNeg);
             
            }

            // process hats as axis
            int ax = states.Count;
            for (int i = 0; i < state.GetPointOfViewControllers().Length; i++)
                {
               
                int j = i;

                // looks like mednafen does left-right before up-down
                string pos = "8";
                string neg = "c";
                string padding = "00";
                if (ax > 16)
                    padding = "0";

                StringBuilder sbLR = new StringBuilder();
                sbLR.Append(id + " 0000");                    

                // L - R
                string strL = sbLR.ToString() + neg + padding + ax;
                AddItem(strL, () => { int t = state.GetPointOfViewControllers()[j]; return t >= 22500 && t <= 31500; });

                string strR = sbLR.ToString() + pos + padding + ax;
                AddItem(strR, () => { int t = state.GetPointOfViewControllers()[j]; return t >= 4500 && t <= 13500; });

                ax++;

                StringBuilder sbUD = new StringBuilder();
                sbUD.Append(id + " 0000");

                // U - D
                string strU = sbUD.ToString() + neg + padding + ax;
                AddItem(strU, () => { int t = state.GetPointOfViewControllers()[j]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });

                string strD = sbUD.ToString() + pos + padding + ax;
                AddItem(strD, () => { int t = state.GetPointOfViewControllers()[j]; return t >= 13500 && t <= 22500; });
                    

                }
           

            

            /*
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
            */


            // i don't know what the "Slider"s do, so they're omitted for the moment

            for (int i = 0; i < state.GetButtons().Length; i++)
            {
                //int j = i;
                //AddItem(string.Format("B{0}", i + 1), () => state.IsPressed(j));
            }

            int povCount = state.GetPointOfViewControllers().Length;
            int nPovCount = joystick.Capabilities.PovCount;
            /*
            for (int i = 0; i < state.GetPointOfViewControllers().Length; i++)
            {
                int j = i;

                AddItem(string.Format(id + " " + "POV{0}U", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });
                AddItem(string.Format(id + " " + "POV{0}D", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 13500 && t <= 22500; });
                AddItem(string.Format(id + " " + "POV{0}L", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 22500 && t <= 31500; });
                AddItem(string.Format(id + " " + "POV{0}R", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 4500 && t <= 13500; });

            }
       */
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
