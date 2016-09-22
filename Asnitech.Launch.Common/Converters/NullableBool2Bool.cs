using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Asnitech.Launch.Common.Converters
{
    public class NullableBool2Bool
    {
        public static bool Convert(bool? value)
        {
            bool b = false;
            if (value == true) { b = true; }
            return b;
        }
    }
}
