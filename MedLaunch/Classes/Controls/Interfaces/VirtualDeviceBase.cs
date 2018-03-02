using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Controls.VirtualDevices
{
    public abstract class VirtualDeviceBase
    {
        public static VirtualDeviceBase GetVirtualDevice(string devClassName, MednafenType mednafenType)
        {
            switch (mednafenType)
            {
                // current
                case MednafenType.gte_1_21_x:
                    // just load the current assembly
                    return InstantiateClass(devClassName);
                // legacy        
                case MednafenType.lt_1_21_x:
                    // need to check whether this version exists
                    if (CheckClassExists(devClassName + "_legacy"))
                    {
                        // return the legacy class
                        return InstantiateClass(devClassName + "_legacy");
                    }
                    else
                    {
                        // legacy class not found - return the current one (which should always exist)
                        return InstantiateClass(devClassName);
                    }

                default:
                    return InstantiateClass(devClassName);
            }
        }  
        
        public static bool CheckClassExists(string className)
        {
            var type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from t in assembly.GetTypes()
                        where t.Name == className
                        select t).FirstOrDefault();

            if (type == null)
                return false;
            else
                return true;
        }

        public static VirtualDeviceBase InstantiateClass(string className)
        {
            var type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from t in assembly.GetTypes()
                        where t.Name == className
                        select t).FirstOrDefault();

            if (type == null)
                return null;
            else
            {
                // instantiate and return the class
                VirtualDeviceBase inst = (VirtualDeviceBase)Activator.CreateInstance(type);
                return inst;
            }
        }
    }

    public enum MednafenType
    {
        gte_1_21_x,
        lt_1_21_x
    }

    public enum DeviceSystem
    {
        GameBoy,
        GameBoyAdvance,
        GameGear,
        Lynx,
        MegaDrive,
        Nes,
        NeoGeoPocket,
        PceEngine,
        PceEngineCD,
        PceEngineFast,
        Pcfx,
        Playstation,
        MasterSystem,
        Snes,
        SnesFaust,
        Saturn,
        VirtualBoy,
        WonderSwan,
        MiscBindings
    }
}
