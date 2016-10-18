using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asnitech.Launch.Common.Converters
{
    public static class RomanNumerals
    {
        private static Dictionary<char, int> _romanMap = new Dictionary<char, int>
{
   {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000}
};

        public static int RomanToNumber(string text)
        {
            int totalValue = 0, prevValue = 0;
            foreach (var c in text)
            {
                if (!_romanMap.ContainsKey(c))
                    return 0;
                var crtValue = _romanMap[c];
                totalValue += crtValue;
                if (prevValue != 0 && prevValue < crtValue)
                {
                    if (prevValue == 1 && (crtValue == 5 || crtValue == 10)
                        || prevValue == 10 && (crtValue == 50 || crtValue == 100)
                        || prevValue == 100 && (crtValue == 500 || crtValue == 1000))
                        totalValue -= 2 * prevValue;
                    else
                        return 0;
                }
                prevValue = crtValue;
            }
            return totalValue;
        }

        public static string NumberToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + NumberToRoman(number - 1000);
            if (number >= 900) return "CM" + NumberToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + NumberToRoman(number - 500);
            if (number >= 400) return "CD" + NumberToRoman(number - 400);
            if (number >= 100) return "C" + NumberToRoman(number - 100);
            if (number >= 90) return "XC" + NumberToRoman(number - 90);
            if (number >= 50) return "L" + NumberToRoman(number - 50);
            if (number >= 40) return "XL" + NumberToRoman(number - 40);
            if (number >= 10) return "X" + NumberToRoman(number - 10);
            if (number >= 9) return "IX" + NumberToRoman(number - 9);
            if (number >= 5) return "V" + NumberToRoman(number - 5);
            if (number >= 4) return "IV" + NumberToRoman(number - 4);
            if (number >= 1) return "I" + NumberToRoman(number - 1);
            return "0";

            //throw new ArgumentOutOfRangeException("something bad happened");
        }
    }
}
