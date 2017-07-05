using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MedLaunch._Debug.DATDB.Platforms.SATAKORE
{
    public class ImportSatakoreData
    {
        public static List<DAT_Rom> Go()
        {
            List<DAT_Rom> l = new List<DAT_Rom>();

            // load SaturnGames.json
            string jsonPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\SaturnGames.json";
            string txt = File.ReadAllText(jsonPath);
            List<SaturnGame> games = JsonConvert.DeserializeObject<List<SaturnGame>>(txt);

            // iterate through
            foreach (var g in games)
            {
                DAT_Rom dr = new DAT_Rom();
                dr.name = g.Title;
                dr.country = g.Country;
                dr.otherFlags = g.SerialNumber;
                dr.datProviderId = 6;
                dr.romName = g.Title + " - " + g.Version;
                dr.pid = 17;

                if (g.Country == "JPN")
                    dr.language = "J";
                if (g.Country == "USA")
                    dr.language = "En";
                if (g.Country == "EUR")
                    dr.language = "En";

                // get year
                string[] yArr = g.Date.Split('/');
                if (yArr.Length == 3)
                {
                    dr.year = yArr[2];
                }

                l.Add(dr);
            }
            l.Distinct();
            return l;
        }
    }
}
