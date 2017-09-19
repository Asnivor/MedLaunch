using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedLaunch._Debug.DATDB.Platforms.TOSEC
{
    public class ImportToSecData
    {
        /// <summary>
        /// process all tosec dat files for every system
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> Go()
        {
            // import data for all systems
            List<DAT_System> systems = DAT_System.GetSystems();

            List<DAT_Rom> l = new List<DAT_Rom>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.pid);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<DAT_Rom> list = Parse(s, sys.pid);
                    l.AddRange(list);
                }
            }
            l.Distinct();
            return l;
        }

        /// <summary>
        /// process tosec dat files based on gdb platform ID
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> Go(int platformId)
        {
            List<DAT_Rom> l = new List<DAT_Rom>();

            // get a list of strings containing all data info for this system
            List<string> dats = LoadDATs(platformId);

            // iterate through each data and parse the information into a new object
            foreach (string s in dats)
            {
                List<DAT_Rom> list = Parse(s, platformId);
                l.AddRange(list);
            }

            l.Distinct();
            return l;
        }

        public static List<DAT_Rom> Parse(string dat, int systemId)
        {
            List<DAT_Rom> list = new List<DAT_Rom>();

            // replace illegal characters
            dat = dat.Replace(" & ", " &amp; ").Replace(" and ", " &amp; ");

            // parse into an xml document
            XDocument xmlDoc = XDocument.Parse(dat);
            //var games = xmlDoc.Descendants("game");

            // iterate through each game
            foreach (XElement element in xmlDoc.Root.Elements("game"))
            {
                string nameString = (string)element.Attribute("name");

                DAT_Rom no = StringConverterToSec.ParseString(nameString);
                no.pid = systemId;

                no.description = (string)element.Element("description");
                IEnumerable<XElement> roms = element.Elements("rom");

                foreach (XElement rom in roms)
                {

                    DAT_Rom t = new DAT_Rom
                    {
                        name = no.name,
                        cloneOf = no.cloneOf,
                        copyright = no.copyright,
                        country = no.country,
                        crc = (string)rom.Attribute("crc"),
                        //size = (string)rom.Attribute("size"),
                        md5 = (string)rom.Attribute("md5"),
                        sha1 = (string)rom.Attribute("sha1"),
                        developmentStatus = no.developmentStatus,
                        language = no.language,
                        romName = (string)rom.Attribute("name"),
                        pid = no.pid,
                        otherFlags = no.otherFlags,
                        publisher = no.publisher,
                        //description = no.description,
                        year = no.year,
                        datProviderId = 2
                        
                    };

                    list.Add(t);
                }

            }
            return list;


        }

        public static List<string> LoadDATs(int systemId)
        {
            List<string> searchStr = new List<string>();
            switch (systemId)
            {
                case 2:
                    searchStr.Add("Nintendo Game Boy Advance - ");
                    break;
                case 4:
                    searchStr.Add("Nintendo Game Boy - ");
                    break;
                case 5:
                    searchStr.Add("Nintendo Game Boy Color - ");
                    break;
                case 4924:
                    searchStr.Add("Atari Lynx - ");
                    break;
                case 18:
                    searchStr.Add("Sega Mega Drive & Genesis - ");
                    break;
                case 20:
                    searchStr.Add("Sega Game Gear - ");
                    break;
                case 4922:
                    searchStr.Add("SNK Neo-Geo Pocket - ");
                    break;
                case 4923:
                    searchStr.Add("SNK Neo-Geo Pocket Color - ");
                    break;
                case 34:
                    searchStr.Add("NEC PC-Engine ");
                    searchStr.Add("NEC SuperGrafx ");
                    break;
                case 35:
                    searchStr.Add("Sega Mark III & Master System ");
                    break;
                case 7:
                    searchStr.Add("Nintendo Famicom & Entertainment System ");
                    break;
                case 6:
                    searchStr.Add("Nintendo Super Famicom & Super Entertainment System ");
                    break;
                case 4918:
                    searchStr.Add("Nintendo Virtual Boy ");
                    break;
                case 4925:
                    searchStr.Add("Bandai WonderSwan - ");
                    break;
                case 4926:
                    searchStr.Add("Bandai WonderSwan Color - ");
                    break;
                case 4930:
                    searchStr.Add("NEC PC-FX");
                    break;
                case 10:
                    searchStr.Add("Sony PlayStation ");
                    break;
                case 17:
                    searchStr.Add("Sega Saturn ");
                    break;
                case 4936:
                    searchStr.Add("Nintendo Famicom Disk System - ");
                    break;
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\_Debug\DATDB\DATFiles\TOSEC";

            // get all data files for this system
            if (!Directory.Exists(folder))
                return null;

            List<string> files = Directory.GetFiles(folder).ToList();
            List<string> f = new List<string>();
            foreach (string s in searchStr)
            {
                foreach (string b in files.Where(a => a.Contains(".dat")))
                {
                    if (b.ToLower().Contains(s.ToLower()))
                    {
                        f.Add(b);
                    }
                }
            }
            f.Distinct();

            foreach (var s in f)
            {
                // import the data to a string
                string d = File.ReadAllText(s);
                data.Add(d);
            }

            return data;
        }
    }
}
