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
using System.Windows.Interop;

namespace MedLaunch.Classes.Controls
{
    

    public struct di_axis_info
    {
        public int minimum;
        public int maximum;
        public int jd_logical_offset;
        public string name;
        public int usage;
        public int usagepage;
        public int offset;
        public int offsetActual;
        public DeviceObjectInstance device;
    }

    public struct ButtConfig
    {
        public byte ButtType;
        public byte DeviceNum;
        public int ButtonNum;
    }

    public enum AXIS_CONFIG_TYPE
    {
        GENERIC = 0,
        ANABUTTON_POSPRESS,
        ANABUTTON_NEGPRESS
    }

    public class xinput_id
    {
        public string PID { get; set; }
        public string VID { get; set; }
        public string PIDVID { get; set; }
    }

    public class GamePad
    {
        // ********************************** Static interface **********************************

        static DirectInput dinput;
        public static List<GamePad> Devices;
        public static ControllerInfo[] ContInfoFromLog;
        public List<ButtConfig> ButtonList;
        public List<Int16> axis_config_type;
        


        public static void Initialize(MainWindow window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;

            if (dinput == null)
                dinput = new DirectInput();

            Devices = new List<GamePad>();

            ContInfoFromLog = LogParser.GetDirectInputControllerIds();


            List<xinput_id> xids = new List<xinput_id>();

            // get all directinput pnp devices from WMI
            List<USBDeviceInfo> pnpDevs = pnp.GetUSBDevices().Where(a => a.DeviceID.Contains("IG_")).ToList();

            foreach (USBDeviceInfo pD in pnpDevs)
            {
                xinput_id x = new xinput_id();
                string deviceId = pD.DeviceID;
                // get VID and PID
                string[] arr = deviceId.Split('&');
                x.VID = arr[0].Replace("HID\\VID_", "");
                x.PID = arr[1].Replace("PID_", "");
                x.PIDVID = (x.PID + x.VID).ToLower();
                xids.Add(x);
            }

            xids.Distinct();

            int count = 0;
            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                Console.WriteLine("joydevice: {0} `{1}`", device.InstanceGuid, device.ProductName);
                string prodName = device.ProductName;
                string devId = device.ProductGuid.ToString();
                string devPidVid = (devId.Split('-'))[0];

                // compare PIDVID from WMI from the first part of the ProductGuid. If they match, this is an xinput controller and can be skipped (and left for GamePad360)
                var check = (from a in xids
                            where a.PIDVID == devPidVid
                            select a).ToList();
                if (check.Count > 0)
                    continue;

                //if (device.ProductName.Contains("XBOX 360"))
                //  continue; // Don't input XBOX 360 controllers into here; we'll process them via XInput (there are limitations in some trigger axes when xbox pads go over xinput)

                var joystick = new Joystick(dinput, device.InstanceGuid);
                joystick.SetCooperativeLevel(handle, CooperativeLevel.Background | CooperativeLevel.Nonexclusive);
                                
                foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
                {              
                    // set range for each axis (not sure if this actually works)                        
                    if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    {
                        joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-1000, 1000);                        
                    }
                }
                joystick.Acquire();

                string nId = "";
                // get mednafen unique id for this controller from stdout.txt
                if (ContInfoFromLog.Count() > 0)
                    nId = ContInfoFromLog[count].ID;
                
                // instantiate new gamepad instance and add it to the collection
                GamePad p = new GamePad(device.InstanceName, nId, joystick, device.InstanceGuid);
                Devices.Add(p);
                count++;
            }
        }        

        public static void UpdateAll()
        {
            if (Devices.Count > 0)
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
        readonly Guid guid;        
        readonly string id;
        readonly Joystick joystick;

        public int num_rel_axes;
        public int num_axes;
        public int num_hats;
        public int num_buttons;

        public List<di_axis_info> DIAxisInfo;
        public List<UInt32> BConfig;
        public Capabilities DevCaps;

        public DeviceObjectInstance[] devObList;



        public List<Int16> axis_state;
        public List<int> rel_axis_state;
        public List<bool> button_state;

        JoystickState state = new JoystickState();
        
        
        GamePad(string name, string Id, Joystick joystick, Guid guid)
        {
            this.name = name;
            this.guid = guid;
            this.id = Id;
            this.joystick = joystick;

            DIAxisInfo = new List<di_axis_info>();
            
            BConfig = new List<UInt32>();
            axis_state = new List<short>();
            rel_axis_state = new List<int>();
            button_state = new List<bool>();
            axis_config_type = new List<short>();

            DevCaps = joystick.Capabilities;
            
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

            // populate DIAXISINFO
            devObList = (joystick.GetObjects()
                .Where(a => a.UsagePage == 1 && a.Usage > 0 && a.Usage != 4)
                .GroupBy(x => x.Name).Select(x => x.First())
                .OrderBy(o => o.Usage)
                .ThenBy(o2 => o2.Offset)).ToArray();

            for (int axis = 0; axis < devObList.Count(); axis++)
            {
                DeviceObjectInstance deviceObject = joystick.GetObjects()[axis];
                InputRange diprg = joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).LogicalRange;
                int min = diprg.Minimum;
                int max = diprg.Maximum;

                if (min < max)
                {
                    di_axis_info dai = new di_axis_info();
                    dai.jd_logical_offset = axis;
                    dai.maximum = max;
                    dai.minimum = min;
                    dai.name = deviceObject.Name;
                    dai.usage = deviceObject.Usage;
                    dai.usagepage = deviceObject.UsagePage;
                    dai.offset = deviceObject.Offset / 4;
                    dai.offsetActual = deviceObject.Offset;
                    dai.device = deviceObject;
                    DIAxisInfo.Add(dai);
                }

                //axis_config_type[axis] = 0;
                

            }


            num_rel_axes = 0;
            num_axes = DIAxisInfo.Count(); // + joystick.Capabilities.PovCount * 2;
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
            /*
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

                    switch (octant)
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
            */

            //string iD = IdGenerator.CalcOldStyleID(DIAxisInfo.Count, 0, joystick.Capabilities.PovCount, joystick.Capabilities.ButtonCount).ToString();
            //string iD = IdGenerator.CalcOldStyleID(num_axes, 0, num_hats, num_buttons).ToString();

            /* populate objects */

            // buttons first
            for (int i = 0; i < num_buttons; i++)
            {
                int j = i;
                bool button_state = state.GetButtons()[i];
                UInt32 butnameint = Convert.ToUInt32(i);
                string buttonnum = butnameint.ToString("X8");

                BConfig.Add(butnameint);

                AddItem("joystick " + id + " " + butnameint.ToString("X8").ToLower(), () => state.IsPressed(j));
            }

           

            int states = 0;
            int povTmpCount = 0;
            Func<bool> cbPos;
            Func<bool> cbNeg;

            // names
            int buttNamePos;
            int buttNameNeg;
            // axis
            for (int axis = 0; axis < num_axes; axis++)
            {
                Int16 a_state = axis_state[axis];
                DeviceObjectInstance di = devObList[axis];
                
                // dynamically create callback               

                switch (di.Usage)
                {
                    case 0x30:      // X Axis
                        cbPos = () => state.X >= dzp;                        
                        cbNeg = () => state.X <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x31:      // y Axis
                        cbPos = () => state.Y >= dzp;
                        cbNeg = () => state.Y <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x32:      // Z Axis
                        cbPos = () => state.Z >= dzp;
                        cbNeg = () => state.Z <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x33:      // X Rotation
                        cbPos = () => state.RotationX >= dzp;
                        cbNeg = () => state.RotationX <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x34:      // Y Rotation
                        cbPos = () => state.RotationY >= dzp;
                        cbNeg = () => state.RotationY <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x35:      // Z Rotation
                        cbPos = () => state.RotationZ >= dzp;
                        cbNeg = () => state.RotationZ <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x40:      // X Velocity
                        cbPos = () => state.RotationZ >= dzp;
                        cbNeg = () => state.RotationZ <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x41:      // Y Velocity
                        cbPos = () => state.RotationZ >= dzp;
                        cbNeg = () => state.RotationZ <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x42:      // Z Velocity
                        cbPos = () => state.RotationZ >= dzp;
                        cbNeg = () => state.RotationZ <= dzn;
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), cbPos);
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), cbNeg);
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        break;
                    case 0x39:      // Hat Switch (contains 2 actual axis) 

                        // skip

                        /*
                        povTmpCount++;
                        // L - R
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 22500 && t <= 31500; });
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 4500 && t <= 13500; });
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));

                        // U - D
                        buttNamePos = 0x8000 + axis;
                        buttNameNeg = 0xc000 + axis;
                        AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });
                        AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 13500 && t <= 22500; });
                        BConfig.Add(Convert.ToUInt32(buttNamePos));
                        BConfig.Add(Convert.ToUInt32(buttNameNeg));
                        */
                        break;
                    default:
                        cbPos = null;
                        cbNeg = null;
                        break;
                }               
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


            // add sliders
            int povCount = state.GetPointOfViewControllers().Length;
            int nPovCount = joystick.Capabilities.PovCount;
            int axiscount = DevCaps.AxesCount;
            
            for (int i = 0; i < nPovCount; i++)
            {
                int j = i;
                int axis = axiscount - nPovCount + i;

                // L - R
                buttNamePos = 0x8000 + axis;
                buttNameNeg = 0xc000 + axis;
                //AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 22500 && t <= 31500; });
                AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(),  //string.Format("joystick " + id + " " + "POV{0}L", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 22500 && t <= 31500; });
                //AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 4500 && t <= 13500; });
                AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(),  //string.Format("joystick " + id + " " + "POV{0}R", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 4500 && t <= 13500; });
                BConfig.Add(Convert.ToUInt32(buttNamePos));
                BConfig.Add(Convert.ToUInt32(buttNameNeg));

                // U - D
                buttNamePos = 0x8000 + (axis + 1);
                buttNameNeg = 0xc000 + (axis + 1);
                //AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });
                AddItem("joystick " + id + " " + buttNamePos.ToString("X8").ToLower(),   //string.Format("joystick " + id + " " + "POV{0}U", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return (t >= 0 && t <= 4500) || (t >= 31500 && t < 36000); });
                //AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(), () => { int t = state.GetPointOfViewControllers()[povTmpCount]; return t >= 13500 && t <= 22500; });
                AddItem("joystick " + id + " " + buttNameNeg.ToString("X8").ToLower(),   //string.Format("joystick " + id + " " + "POV{0}D", i + 1),
                    () => { int t = state.GetPointOfViewControllers()[j]; return t >= 13500 && t <= 22500; });
                BConfig.Add(Convert.ToUInt32(buttNamePos));
                BConfig.Add(Convert.ToUInt32(buttNameNeg));

                // increment axis count (as there are effectively two axis in a POV Hat)
                axiscount++;

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
