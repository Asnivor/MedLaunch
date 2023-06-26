using MedLaunch._Debug.DATDB.Platforms.NOINTRO.Models;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedLaunch._Debug.DATDB.Platforms.NOINTRO
{
    public class ImportNoIntroData
    {
        /// <summary>
        /// process all nointro dat files for every system
        /// </summary>
        /// <returns></returns>
        public static List<DAT_Rom> Go()
        {
            // load each system
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
        /// process nointro dat files based on gdb platform ID
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

                DAT_Rom no = StringConverterNoIntro.ParseString(nameString);
                no.pid = systemId;

                no.cloneOf = (string)element.Attribute("cloneof");
                //no.Description = (string)element.Element("description");
                IEnumerable<XElement> roms = element.Elements("rom");

                foreach (XElement rom in roms)
                {
                    DAT_Rom n = new DAT_Rom
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
                        description = no.description,
                        year = no.year,
                        datProviderId = 1,                        
                    };

                    list.Add(n);
                }
            }
            return list;
        }

        public static List<string> LoadDATs(int systemId)
        {
            List<string> searchStr = new List<string>();
            switch (systemId)
            {
                case 4956:
                    searchStr.Add("Apple - II (Parent-Clone)");
                    break;
                case 4924:
                    searchStr.Add("Atari - Lynx (Parent-Clone) ");
                    break;
                case 4925:
                    searchStr.Add("Bandai - WonderSwan (Parent-Clone)");
                    break;
                case 4926:
                    searchStr.Add("Bandai - WonderSwan Color (Parent-Clone)");
                    break;
                case 34:
                    searchStr.Add("NEC - PC Engine - T");
                    searchStr.Add("NEC - PC Engine Su");
                    break;
                case 4936:
                    searchStr.Add("Nintendo - Family Computer Disk");
                    break;
                case 4:
                    searchStr.Add("Nintendo - Game Boy (Parent-Clone)");
                    break;
                case 5:
                    searchStr.Add("Nintendo - Game Boy Advance (Parent-Clone)");
                    break;
                case 41:
                    searchStr.Add("Nintendo - Game Boy Color (Parent-Clone)");
                    break;
                case 7:
                    searchStr.Add("Nintendo - Nintendo Entertainment System ");
                    break;
                case 6:
                    searchStr.Add("Nintendo - Super Nintendo Entertainment System ");
                    break;
                case 4918:
                    searchStr.Add("Nintendo - Virtual Boy ");
                    break;
                case 20:
                    searchStr.Add("Sega - Game Gear (Parent-Clone)");
                    break;
                case 35:
                    searchStr.Add("Sega - Master System -");
                    break;
                case 18:
                    searchStr.Add("Sega - Mega Drive - Genesis (Parent-Clone) ");
                    break;
                case 4922:
                    searchStr.Add("SNK - NeoGeo Pocket (P");
                    break;
                case 4923:
                    searchStr.Add("SNK - NeoGeo Pocket Color");
                    break;
                
                
                
                
                
                
                
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\_Debug\DATDB\DATFiles\NOINTRO";

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
