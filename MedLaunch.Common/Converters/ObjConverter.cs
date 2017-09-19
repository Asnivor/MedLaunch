using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedLaunch.Common.Converters
{
    public class ObjConverter
    {
        public enum ConversationCase
        {
            ToLower,
            ToUpper,
            ToCamelCase,
            None
        }

        /// <summary>
        /// return a string array (whitespace delimited)
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public static string[] StringToArray(string str, ConversationCase conversionCase)
        {
            switch (conversionCase)
            {
                case ConversationCase.ToUpper:
                    string[] gArr1 = str.ToUpper().Trim().Split(' ');
                    return gArr1;
                case ConversationCase.ToLower:
                    string[] gArr2 = str.ToLower().Trim().Split(' ');
                    return gArr2;
                case ConversationCase.ToCamelCase:
                    break;
                default:
                    string[] gArr4 = str.Trim().Split(' ');
                    return gArr4;
            }
            return null;
        }

        /// <summary>
        /// turns array back into string
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string ArrayToString(string[] arr)
        {
            string searchStr = "";
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        /// <summary>
        /// turns array back into string
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public string ArrayToString(string[] arr, int position)
        {
            string searchStr = "";
            for (int i = 0; i <= position; i++)
            {
                searchStr += arr[i].ToLower() + " ";
            }
            searchStr.Trim();
            return searchStr;
        }

        public static string StripSymbols(string i)
        {
            // remove all (xxx), [xxx] 
            string regex = "(\\[.*\\])|(\\(.*\\))";
            string s = Regex.Replace(i, regex, "").Replace("()", "").Replace("[]", "").ToLower().Trim();
            // add this to the class

            // remove all - : _ '
            s = s.Replace(" - ", " ").Replace("-", "").Replace("_", "").Replace(": ", " ").Replace(" : ", " ").Replace(":", " ").Replace("'", "").Trim();
            // remove all roman numerals
            /*
            s.Replace(" I", " ");
            s.Replace(" II ", " ").Replace(" II", " ");
            s.Replace(" III ", " ").Replace(" III", " ");
            s.Replace(" IV ", " ").Replace(" IV", " ");
            s.Replace(" V ", " ");
            s.Replace(" VI ", " ").Replace(" VI", " ");
            s.Replace(" VII ", " ").Replace(" VII", " ");
            s.Replace(" VIII ", " ").Replace(" VIII", " ");
            s.Replace(" IX ", " ").Replace(" IX", " ");
            s.Replace(" X ", " ");
            s.Replace(" XI ", " ").Replace(" XI", " ");
            s.Replace(" XII ", " ").Replace(" XII", " ");

            // replace ending numbers
            string[] arr = BuildArray(s);
            string l = arr[arr.Length - 1];
            foreach (char c in l)
            {
                if (char.IsDigit(c))
                {
                    arr = arr.Take(arr.Count() - 1).ToArray();
                    break;
                }
            }
            s = BuildSearchString(arr);
            */
            return s;
        }
    }
}
