using MedLaunch._Debug.DATDB.Platforms.NOINTRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.DATDB.Platforms.NOINTRO
{
    public class StringConverterNoIntro
    {
        public static NoIntroObject ParseString(string nameString)
        {
            NoIntroObject no = new NoIntroObject();

            // get name without any options (integrating demo flag if available)
            //no.Name = nameString.Split(new string[] { " ) " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() + ")";

            // remove any unwanted options from string
            string a = RemoveUnneededOptions(nameString);

            // process data contained in ()
            string[] d = a.ToString().Split('(', ')');

            if (d.Length > 0)
                no.Name = d[0].Trim();


            if (d.Length > 1)
            {
                if (d[1].Length > 3)
                {
                    no.Country = d[1].Trim();
                }
            }

            if (d.Length > 2)
            {
                // iterate through remaining array of () data and determine values
                for (int i = 2; i < d.Length; i++)
                {
                    string f = d[i].Trim();

                    // check for language
                    if (IsLanguageFlag(f) == true)
                    {
                        no.Language = f;
                        continue;
                    }

                    // check version
                    if (IsVersion(f) == true)
                    {
                        no.OtherFlags = f;
                    }

                    // check development status
                    if (IsDevelopmenttStatus(f) == true)
                    {
                        no.DevelopmentStatus = f;
                        continue;
                    }

                    // check copyright status
                    if (IsCopyrightStatus(f) == true)
                    {
                        no.Copyright = f;
                        continue;
                    }



                    // Media Type - ignore for now

                    // Media Label - ignore for now
                }

                if (no.Copyright == null)
                    no.Copyright = "Commercial";
                if (no.Copyright == "Unl")
                    no.Copyright = "Unlicensed";
                if (no.DevelopmentStatus == null)
                    no.DevelopmentStatus = "Release";
                if (no.Language == null)
                    no.Language = "en";

            }

            // process other flags


            /*
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

                    no.OtherFlags = sb.ToString().Trim();
                }
            }
            */
            return no;

        }

        public static bool IsVersion(string s)
        {

            if (s.ToLower().Trim().StartsWith("Rev "))
            {
                return true;
            }

            string v = s.Trim();
            if (v.StartsWith("v") && v.Contains("."))
            {
                return true;
            }

            string[] a = v.Split('-');
            if (a.Length > 1)
            {
                if (a[0].Length == 2)
                    return true;
            }

            return false;
        }

        public static bool IsDevelopmenttStatus(string s)
        {
            List<string> DS = new List<string>
            {
                "Alpha", "Beta", "Proto", "Sample"
            };

            bool b = DS.Any(s.Contains);
            return b;
        }

        public static bool IsCopyrightStatus(string s)
        {
            List<string> CS = new List<string>
            {
                "Unl"
            };

            bool b = CS.Any(s.Contains);
            return b;
        }

        public static bool IsLanguageFlag(string s)
        {
            List<string> LC = new List<string>
            {
                "En", "Ja", "Fr", "De", "Es", "It", "Nl", "Pt",
                "Sv", "No", "Da", "Fi", "Zh", "Ko", "Pl"
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
