using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.DirectX;
//using Microsoft.DirectX.DirectInput;
using System.Windows.Interop;
/*
namespace MedLaunch.Classes.Controls.Input
{
    public class di_axis_info
    {
        public int minimum { get; set; }
        public int maximum { get; set; }
        public UInt32 jd_logical_offset { get; set; }
    }

    public class Joystick
    {
        // ********************************** Static Interface **********************************
        public static List<Joystick> Devices;
        public static ControllerInfo[] ContInfoFromLog;
        

        public static void Initialize(MainWindow window)
        {
            // Find all the GameControl devices that are attached.
            DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly);

            if (gameControllerList.Count == 0)
                return;

            // iterate through each controller instance
            int count = 0;
            foreach (DeviceInstance deviceInstance in gameControllerList)
            {
                // create device
                Device joy = new Device(deviceInstance.InstanceGuid);

                if (deviceInstance.ProductName.Contains("XBOX"))
                    continue;

                IntPtr handle = new WindowInteropHelper(window).Handle;
                joy.SetCooperativeLevel(handle, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                joy.SetDataFormat(DeviceDataFormat.Joystick);

                joy.Acquire();

                ContInfoFromLog = LogParser.GetDirectInputControllerIds();

                Joystick j = new Joystick(deviceInstance.InstanceName, ContInfoFromLog[count].ID, joy);
                Devices.Add(j);
            }
        }

        

        public string ID { get; set; }
        public int NumAxes { get; set; }
        public int NumRelAxes { get; set; }
        public int NumButtons { get; set; }

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
        public DeviceCaps DevCaps;


        // ********************************** Instance Members **********************************
        Joystick(string name, string Id, Device device)
        {
            NumAxes = 0;
            NumButtons = 0;
            NumRelAxes = 0;

            for (uint rax = 0; rax < 8; rax++)
            {
            }
        }


        /*     

                public Int16 GetAxis(uint axis)
                {
                    return axis_state[Convert.ToInt32(axis)];
                }

                public bool GetButton(uint button)
                {
                    return button_state[Convert.ToInt32(button)];
                }

                public int GetRelAxis(uint rel_axis)
                {
                    return rel_axis_state[Convert.ToInt32(rel_axis)];
                }

                protected List<Int16> axis_state { get; set; }
                protected List<int> rel_axis_state { get; set; }
                protected List<bool> button_state { get; set; }

                public bool IsAxisButton(uint axis)
                {
                    return false;
                }

                public uint HatToAxisCompat(uint hat)
                {
                    return ~hat;
                }
                public uint HatToButtonCompat(uint hat)
                {
                    return ~hat;
                }
            }
            /*
            public class JoystickDriver
            {
                public JoystickDriver()
                {
                    NumJoysticks = 0;
                }

                public uint NumJoysticks { get; set; }

                public Joystick GetJoystick(uint index)
                {

                }       

                public void UpdateJoysticks()
                {

                } 
            }
            */

        /*

public enum AXIS_CONFIG_TYPE
{
    GENERIC = 0,
    ANABUTTON_POSPRESS,
    ANABUTTON_NEGPRESS
}

public class ButtConfig
{
    public ButtConfig()
    {

    }

    public byte ButtType { get; set; }
    public byte DeviceNum { get; set; }
    public UInt32 ButtonNum { get; set; }
    public string DeviceId { get; set; }
}

public class JoystickManager
{
    public void DetectAnalogButtonsForChangeCheck()
    {

    }

    public void Reset_BC_ChangeCheck()
    {

    }

    public bool Do_BC_ChangeCheck(ButtConfig bc)
    {

    }

    public void SetAnalogThreshold(double thresh)
    {

    }

    public void UpdateJoysticks()
    {

    }

    public bool TestButton(ButtConfig bc)
    {

    }

    public int TestAnalogButton(ButtConfig bc)
    {

    }

    public uint GetIndexByUniqueID(string unique_id)
    {

    }

    public string GetUniqueIDByIndex(uint index)
    {

    }

    private int AnalogThreshold;

    private List<JoystickDriver> JoystickDrivers;
    private List<JoystickManager_Cache> JoystickCache;
    private ButtConfig BCPending;
    private int BCPending_Prio;
    private UInt32 BCPending_Time;
    private UInt32 BCPending_CCCC;

    public JoystickManager()
    {
        JoystickDrivers = new List<JoystickDriver>();
        JoystickCache = new List<JoystickManager_Cache>();

        JoystickDriver main_driver = null;
        //JoystickDriver hicp_driver = null;

        // initialise joysticks
        DeviceList gameControllerList = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);

    }


}

public class JoystickManager_Cache
{
    Joystick joystick;
    string UniqueID;

    // Helpers for input configuration(may have semantics that differ from what the names would suggest)!
    int config_prio;    // Set to -1 to exclude this joystick instance from configuration, 0 is normal, 1 is SPECIALSAUCEWITHBACON.
    List<Int16> axis_config_type;
    bool prev_state_valid;
    List<Int16> prev_axis_state;
    List<int> axis_hysterical_ax_murderer;
    List<bool> prev_button_state;
    List<Int32> rel_axis_accum_state;

    public JoystickManager_Cache()
    {
        axis_config_type = new List<short>();
        prev_axis_state = new List<short>();
        axis_hysterical_ax_murderer = new List<int>();
        prev_button_state = new List<bool>();
        rel_axis_accum_state = new List<int>();
    }

}
*/
/*
    }
}
*/