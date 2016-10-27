using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Asnitech.Launch.Common
{
    public static class RegexOps
    {
        public static string DumpHRefs(string inputString)
        {
            Match m;
            string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            m = Regex.Match(inputString, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
            string result = "";
            while (m.Success)
            {
                result = m.Groups[1].ToString();
            }

            return result;


        }
    }
}
