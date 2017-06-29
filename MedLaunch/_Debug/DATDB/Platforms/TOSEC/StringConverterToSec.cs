using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.TOSEC
{
    public class StringConverterToSec
    {
        /// <summary>
        /// Parses the name string from a tosec entry
        /// </summary>
        /// <param name="nameString"></param>
        /// <returns></returns>
        public static DAT_Rom ParseString(string nameString)
        {
            DAT_Rom no = new DAT_Rom();

            // remove any unwanted options from string
            string a = RemoveUnneededOptions(nameString);

            // process data contained in ()
            string[] d = a.ToString().Split('(', ')');

            if (d.Length > 0)
                no.name = d[0].Trim();


            if (d.Length > 1)
            {
                if (d[1].Length > 3)
                {
                    no.year = d[1].Substring(0, 4);     // take only the year (first 4 characters)
                }
            }


            if (d.Length > 3)
                no.publisher = d[3].Trim();


            if (d.Length > 4)
            {
                // iterate through remaining array of () data and determine values
                for (int i = 4; i < d.Length; i++)
                {
                    string f = d[i];

                    // Check for system field
                    if (f == "Aladdin Deck Enhancer" ||
                        f == "PlayChoice-10" ||
                        f == "VS DualSystem" ||
                        f == "VS UniSystem")
                    {
                        // ignore for now
                        continue;
                    }

                    // check for country/region flag
                    if (IsCountryFlag(f) == true)
                    {
                        no.country = f;
                        continue;
                    }

                    // check for language
                    if (IsLanguageFlag(f) == true)
                    {
                        no.language = f;
                        continue;
                    }

                    // check copyright status
                    if (IsCopyrightStatus(f) == true)
                    {
                        no.copyright = f;
                        continue;
                    }

                    // check development status
                    if (IsDevelopmenttStatus(f) == true)
                    {
                        no.developmentStatus = f;
                        continue;
                    }

                    // Media Type - ignore for now

                    // Media Label - ignore for now
                }

                if (no.copyright == null)
                    no.copyright = "Commercial";
                if (no.developmentStatus == null)
                    no.developmentStatus = "Release";
                if (no.language == null)
                    no.language = "en";

            }

            // process dump info flags and other info contained in []
            if (nameString.Contains("[") && nameString.Contains("]"))
            {
                List<string> e = nameString.ToString().Split('[', ']').ToList();
                if (e.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    int count = 0;
                    foreach (string s in e)
                    {
                        if (count == 0 || s == "")
                        {
                            count++;
                            continue;
                        }


                        sb.Append("[");
                        sb.Append(s);
                        sb.Append("]");
                        count++;
                    }

                    no.otherFlags = sb.ToString().Trim();
                }
            }
            return no;

        }

        public static bool IsDevelopmenttStatus(string s)
        {
            List<string> DS = new List<string>
            {
                "alpha", "beta", "preview", "pre-release", "proto"
            };

            bool b = DS.Any(s.Contains);
            return b;
        }

        public static bool IsCopyrightStatus(string s)
        {
            List<string> CS = new List<string>
            {
                "CW", "CW-R", "FW", "GW", "GW-R", "LW", "PD", "SW", "SW-R"
            };

            bool b = CS.Any(s.Contains);
            return b;
        }

        public static bool IsLanguageFlag(string s)
        {
            List<string> LC = new List<string>
            {
                "ar", "bg", "bs", "cs", "cy", "da", "de", "el", "en", "eo", "es", "et", "fa", "fi", "fr", "ga",
                "gu", "he", "hi", "hr", "hu", "is", "it", "ja", "ko", "lt", "lv", "ms", "nl", "no", "pl", "pt",
                "ro", "ru", "sk", "sl", "sq", "sr", "sv", "th", "tr", "ur", "vi", "yi", "zh", "M1", "M2", "M3",
                "M4", "M5", "M6", "M7", "M8", "M9"
            };

            bool b = false;

            if (!s.Contains("[") && !s.Contains("]"))
            {
                b = LC.Any(s.Contains);
            }

            return b;
        }

        public static bool IsCountryFlag(string s)
        {
            List<string> CC = new List<string>
            {
                "AE", "AL", "AS", "AT", "AU", "BA", "BE", "BG", "BR", "CA", "CH", "CL", "CN", "CS", "CY", "CZ",
                "DE", "DK", "EE", "EG", "EU", "ES", "FI", "FR", "GB", "GR", "HK", "HR", "HU", "ID", "IE", "IL",
                "IN", "IR", "IS", "IT", "JO", "JP", "KR", "LT", "LU", "LV", "MN", "MX", "MY", "NL", "NO", "NP",
                "NZ", "OM", "PE", "PH", "PL", "PT", "QA", "RO", "RU", "SE", "SG", "SI", "SK", "TH", "TR", "TW",
                "US", "VN", "YU", "ZA"
            };

            bool b = false;

            if (!s.Contains("[") && !s.Contains("]"))
            {
                b = CC.Any(s.Contains);
            }

            return b;
        }

        public static string RemoveUnneededOptions(string nameString)
        {
            // Remove unneeded entries
            string n = nameString
                .Replace(" (demo) ", " ")
                .Replace(" (demo-kiosk) ", " ")
                .Replace(" (demo-playable) ", " ")
                .Replace(" (demo-rolling) ", " ")
                .Replace(" (demo-slideshow) ", " ");

            return n;
        }
    }
}

