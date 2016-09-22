using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Asnitech.Launch.Common.Converters
{
    public class NullableInt2Int
    {
        public static int Convert(int? value)
        {
            int b = 0;
            if (value == 1) { b = 1; }
            return b;
        }
    }
}
