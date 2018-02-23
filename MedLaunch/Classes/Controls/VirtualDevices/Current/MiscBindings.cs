using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public class MiscBindings : VirtualDeviceBase
    {
        public static DeviceDefinition Misc(int VirtualPort)
        {
            DeviceDefinition device = new DeviceDefinition();
            device.DeviceName = "Misc Bindings";
            device.CommandStart = "command";
            device.VirtualPort = 0;
            device.MapList = new List<Mapping>
            {
                new Mapping { Description = "Save state 0 select", MednafenCommand = "command.0" },
                new Mapping { Description = "Save state 1 select", MednafenCommand = "command.1" },
                new Mapping { Description = "Save state 2 select", MednafenCommand = "command.2" },
                new Mapping { Description = "Save state 3 select", MednafenCommand = "command.3" },
                new Mapping { Description = "Save state 4 select", MednafenCommand = "command.4" },
                new Mapping { Description = "Save state 5 select", MednafenCommand = "command.5" },
                new Mapping { Description = "Save state 6 select", MednafenCommand = "command.6" },
                new Mapping { Description = "Save state 7 select", MednafenCommand = "command.7" },
                new Mapping { Description = "Save state 8 select", MednafenCommand = "command.8" },
                new Mapping { Description = "Save state 9 select", MednafenCommand = "command.9" },
                new Mapping { Description = "Activate barcode(for Famicom)", MednafenCommand = "command.activate_barcode" },
                //new Mapping { Description = "Advance frame", MednafenCommand = "command.advance_frame" },
                /*
                new Mapping { Description = "Select virtual device on virtual input port 1", MednafenCommand = "command.device_select1" },
                new Mapping { Description = "Select virtual device on virtual input port 2", MednafenCommand = "command.device_select2" },
                new Mapping { Description = "Select virtual device on virtual input port 3", MednafenCommand = "command.device_select3" },
                new Mapping { Description = "Select virtual device on virtual input port 4", MednafenCommand = "command.device_select4" },
                new Mapping { Description = "Select virtual device on virtual input port 5", MednafenCommand = "command.device_select5" },
                new Mapping { Description = "Select virtual device on virtual input port 6", MednafenCommand = "command.device_select6" },
                new Mapping { Description = "Select virtual device on virtual input port 7", MednafenCommand = "command.device_select7" },
                new Mapping { Description = "Select virtual device on virtual input port 8", MednafenCommand = "command.device_select8" },
                new Mapping { Description = "Select virtual device on virtual input port 9", MednafenCommand = "command.device_select9" },
                new Mapping { Description = "Select virtual device on virtual input port 10", MednafenCommand = "command.device_select10" },
                new Mapping { Description = "Select virtual device on virtual input port 11", MednafenCommand = "command.device_select11" },
                new Mapping { Description = "Select virtual device on virtual input port 12", MednafenCommand = "command.device_select12" },
                */
                new Mapping { Description = "Exit", MednafenCommand = "command.exit" },
                new Mapping { Description = "Fast-forward", MednafenCommand = "command.fast_forward" },
                /*
                new Mapping { Description = "Configure buttons on virtual port 1", MednafenCommand = "command.input_config1" },
                new Mapping { Description = "Configure buttons on virtual port 2", MednafenCommand = "command.input_config2" },
                new Mapping { Description = "Configure buttons on virtual port 3", MednafenCommand = "command.input_config3" },
                new Mapping { Description = "Configure buttons on virtual port 4", MednafenCommand = "command.input_config4" },
                new Mapping { Description = "Configure buttons on virtual port 5", MednafenCommand = "command.input_config5" },
                new Mapping { Description = "Configure buttons on virtual port 6", MednafenCommand = "command.input_config6" },
                new Mapping { Description = "Configure buttons on virtual port 7", MednafenCommand = "command.input_config7" },
                new Mapping { Description = "Configure buttons on virtual port 8", MednafenCommand = "command.input_config8" },
                new Mapping { Description = "Configure buttons on virtual port 9", MednafenCommand = "command.input_config9" },
                new Mapping { Description = "Configure buttons on virtual port 10", MednafenCommand = "command.input_config10" },
                new Mapping { Description = "Configure buttons on virtual port 11", MednafenCommand = "command.input_config11" },
                new Mapping { Description = "Configure buttons on virtual port 11", MednafenCommand = "command.input_config12" },
                */
                new Mapping { Description = "Detect analog buttons on physical joysticks/gamepads(for use with the input configuration process)", MednafenCommand = "command.input_config_abd" },
                new Mapping { Description = "Configure command key", MednafenCommand = "command.input_configc" },
                //new Mapping { Description = "Configure command key, for all-pressed-to-trigger mode", MednafenCommand = "command.input_configc_am" },
                new Mapping { Description = "Insert coin", MednafenCommand = "command.insert_coin" },
                new Mapping { Description = "Insert/Eject disk/disc", MednafenCommand = "command.insert_eject_disk" },
                //new Mapping { Description = "Load movie", MednafenCommand = "command.load_movie" },
                new Mapping { Description = "Load state", MednafenCommand = "command.load_state" },
                /*
                new Mapping { Description = "Movie 0 select", MednafenCommand = "command.m0" },
                new Mapping { Description = "Movie 1 select", MednafenCommand = "command.m1" },
                new Mapping { Description = "Movie 2 select", MednafenCommand = "command.m2" },
                new Mapping { Description = "Movie 3 select", MednafenCommand = "command.m3" },
                new Mapping { Description = "Movie 4 select", MednafenCommand = "command.m4" },
                new Mapping { Description = "Movie 5 select", MednafenCommand = "command.m5" },
                new Mapping { Description = "Movie 6 select", MednafenCommand = "command.m6" },
                new Mapping { Description = "Movie 7 select", MednafenCommand = "command.m7" },
                new Mapping { Description = "Movie 8 select", MednafenCommand = "command.m8" },
                new Mapping { Description = "Movie 9 select", MednafenCommand = "command.m9" },
                */
                new Mapping { Description = "Power toggle", MednafenCommand = "command.power" },
                new Mapping { Description = "Reset", MednafenCommand = "command.reset" },
                //new Mapping { Description = "Rotate screen", MednafenCommand = "command.rotate_screen" },
                //new Mapping { Description = "Return to normal mode after advancing frames", MednafenCommand = "command.run_normal" },
                //new Mapping { Description = "Save movie", MednafenCommand = "command.save_movie" },
                new Mapping { Description = "Save state", MednafenCommand = "command.save_state" },
                new Mapping { Description = "Select disk/disc", MednafenCommand = "command.select_disk" },
                new Mapping { Description = "Slow-forward", MednafenCommand = "command.slow_forward" },
                new Mapping { Description = "Rewind", MednafenCommand = "command.state_rewind" },
                new Mapping { Description = "Decrease selected save state slot by 1", MednafenCommand = "command.state_slot_dec" },
                new Mapping { Description = "Increase selected save state slot by 1", MednafenCommand = "command.state_slot_inc" },
                //new Mapping { Description = "Take scaled(and filtered) screen snapshot", MednafenCommand = "command.take_scaled_snapshot" },
                new Mapping { Description = "Take screen snapshot", MednafenCommand = "command.take_snapshot" },
                /*
                new Mapping { Description = "Toggle graphics layer 1", MednafenCommand = "command.tl1" },
                new Mapping { Description = "Toggle graphics layer 2", MednafenCommand = "command.tl2" },
                new Mapping { Description = "Toggle graphics layer 3", MednafenCommand = "command.tl3" },
                new Mapping { Description = "Toggle graphics layer 4", MednafenCommand = "command.tl4" },
                new Mapping { Description = "Toggle graphics layer 5", MednafenCommand = "command.tl5" },
                new Mapping { Description = "Toggle graphics layer 6", MednafenCommand = "command.tl6" },
                new Mapping { Description = "Toggle graphics layer 7", MednafenCommand = "command.tl7" },
                new Mapping { Description = "Toggle graphics layer 8", MednafenCommand = "command.tl8" },
                new Mapping { Description = "Toggle graphics layer 9", MednafenCommand = "command.tl9" },
                */
                //new Mapping { Description = "Toggle debugger", MednafenCommand = "command.toggle_debugger" },
                new Mapping { Description = "Toggle DIP switch view", MednafenCommand = "command.toggle_dipview" },
                //new Mapping { Description = "Toggle frames-per-second display", MednafenCommand = "command.toggle_fps_view" },
                //new Mapping { Description = "Toggle fullscreen mode", MednafenCommand = "command.toggle_fs" },
                //new Mapping { Description = "Grab input", MednafenCommand = "command.toggle_grab" },
                new Mapping { Description = "Toggle help screen", MednafenCommand = "command.toggle_help" },
                //new Mapping { Description = "Toggle state rewind functionality", MednafenCommand = "command.toggle_state_rewind" },
                //new Mapping { Description = "Enable/Disable cheats", MednafenCommand = "command.togglecheatactive" },
                //new Mapping { Description = "Toggle cheat console", MednafenCommand = "command.togglecheatview" },
                new Mapping { Description = "Toggle netplay console", MednafenCommand = "command.togglenetview" },
            };

            DeviceDefinition.PopulateConfig(device);
            return device;
        }
    }
}
