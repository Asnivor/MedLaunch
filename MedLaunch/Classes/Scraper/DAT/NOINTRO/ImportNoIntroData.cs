using MedLaunch.Classes.Scraper.DAT.OFFLINENOINTRO.Models;
using MedLaunch.Classes.Scraper.DAT.TOSEC;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedLaunch.Classes.Scraper.DAT.OFFLINENOINTRO
{
    public class ImportNoIntroData
    {
        public static List<NoIntroObject> Go()
        {
            // Import data for each system
            List <GSystem> systems = GSystem.GetSystems().ToList();

            List<NoIntroObject> l = new List<NoIntroObject>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.systemId);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<NoIntroObject> list = Parse(s, sys.systemId);
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
                    no.RomName = (string)rom.Attribute("name");
                    no.Size = (string)rom.Attribute("size");
                    no.CRC = (string)rom.Attribute("crc");
                    no.MD5 = (string)rom.Attribute("md5");
                    no.SHA1 = (string)rom.Attribute("sha1");

                    list.Add(no);
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
                    searchStr.Add("Nintendo - Game Boy Advance Parent-Clone ");
                    break;
                case 1:
                    searchStr.Add("Nintendo - Game Boy Parent-Clone ");
                    searchStr.Add("Nintendo - Game Boy Color Parent-Clone ");
                    break;                
                case 3:
                    searchStr.Add("Atari - Lynx Parent-Clone ");
                    break;
                case 4:
                    searchStr.Add("Sega - Mega Drive - Genesis Parent-Clone ");
                    break;
                case 5:
                    searchStr.Add("Sega - Game Gear Parent-Clone ");
                    break;
                case 6:
                    searchStr.Add("SNK - Neo Geo Pocket ");
                    break;
                case 7:
                    searchStr.Add("NEC - PC Engine ");
                    searchStr.Add("NEC - PC Engine ");
                    break;
                case 10:
                    searchStr.Add("Sega - Master System ");
                    break;
                case 11:
                    searchStr.Add("Nintendo - Nintendo Entertainment System");
                    break;
                case 12:
                    searchStr.Add("Nintendo - Super Nintendo Entertainment System");
                    break;
                case 14:
                    searchStr.Add("Nintendo - Virtual Boy ");
                    break;
                case 15:
                    searchStr.Add("Bandai - WonderSwan ");
                    break;
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\NOINTRO";

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
