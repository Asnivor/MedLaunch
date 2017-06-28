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
        public static List<NoIntroObject> Go()
        {
            // load each system
            List<DAT_System> systems = DAT_System.GetSystems();

            List<NoIntroObject> l = new List<NoIntroObject>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.pid);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<NoIntroObject> list = Parse(s, sys.pid);
                    l.AddRange(list);
                }
            }
            l.Distinct();
            return l;
        }



        public static List<NoIntroObject> Parse(string dat, int systemId)
        {
            List<NoIntroObject> list = new List<NoIntroObject>();

            // replace illegal characters
            dat = dat.Replace(" & ", " &amp; ").Replace(" and ", " &amp; ");

            // parse into an xml document
            XDocument xmlDoc = XDocument.Parse(dat);
            //var games = xmlDoc.Descendants("game");

            // iterate through each game
            foreach (XElement element in xmlDoc.Root.Elements("game"))
            {

                string nameString = (string)element.Attribute("name");

                NoIntroObject no = StringConverterNoIntro.ParseString(nameString);
                no.SystemId = systemId;

                no.CloneOf = (string)element.Attribute("cloneof");
                //no.Description = (string)element.Element("description");
                IEnumerable<XElement> roms = element.Elements("rom");

                foreach (XElement rom in roms)
                {
                    NoIntroObject n = new NoIntroObject
                    {
                        Name = no.Name,
                        CloneOf = no.CloneOf,
                        Copyright = no.Copyright,
                        Country = no.Country,
                        CRC = (string)rom.Attribute("crc"),
                        Size = (string)rom.Attribute("size"),
                        MD5 = (string)rom.Attribute("md5"),
                        SHA1 = (string)rom.Attribute("sha1"),
                        DevelopmentStatus = no.DevelopmentStatus,
                        Language = no.Language,
                        RomName = (string)rom.Attribute("name"),
                        SystemId = no.SystemId,
                        OtherFlags = no.OtherFlags,
                        Publisher = no.Publisher,
                        Description = no.Description,
                        Year = no.Year
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
                case 5:
                    searchStr.Add("Nintendo - Game Boy Advance Parent-Clone ");
                    break;
                case 4:
                    searchStr.Add("Nintendo - Game Boy Parent-Clone ");
                    break;
                case 41:
                    searchStr.Add("Nintendo - Game Boy Color Parent-Clone ");
                    break;
                case 4924:
                    searchStr.Add("Atari - Lynx Parent-Clone ");
                    break;
                case 18:
                    searchStr.Add("Sega - Mega Drive - Genesis Parent-Clone ");
                    break;
                case 20:
                    searchStr.Add("Sega - Game Gear Parent-Clone ");
                    break;
                case 4922:
                    searchStr.Add("SNK - Neo Geo Pocket P");
                    break;
                case 4923:
                    searchStr.Add("SNK - Neo Geo Pocket Color");
                    break;
                case 34:
                    searchStr.Add("NEC - PC Engine - T");
                    searchStr.Add("NEC - PC Engine Su");
                    break;
                case 35:
                    searchStr.Add("Sega - Master System ");
                    break;
                case 7:
                    searchStr.Add("Nintendo - Nintendo Entertainment System");
                    break;
                case 6:
                    searchStr.Add("Nintendo - Super Nintendo Entertainment System");
                    break;
                case 4918:
                    searchStr.Add("Nintendo - Virtual Boy ");
                    break;
                case 4925:
                    searchStr.Add("Bandai - WonderSwan P");
                    break;
                case 4926:
                    searchStr.Add("Bandai - WonderSwan Color");
                    break;
                case 4936:
                    searchStr.Add("Nintendo - Family Computer Disk");
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
