using MedLaunch.Classes.Scraper.DAT.TOSEC.Models;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedLaunch.Classes.Scraper.DAT.TOSEC
{
    public class ImportToSecData
    {
        public static List<ToSecObject> Go()
        {
            // Import data for each system
            List<GSystem> systems = GSystem.GetSystems().ToList();

            List<ToSecObject> l = new List<ToSecObject>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.systemId);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<ToSecObject> list = Parse(s, sys.systemId);
                    l.AddRange(list);
                }
            }
            l.Distinct();
            return l;
        }



        public static List<ToSecObject> Parse(string dat, int systemId)
        {
            List<ToSecObject> list = new List<ToSecObject>();

            // replace illegal characters
            dat = dat.Replace(" & ", " &amp; ");

            // parse into an xml document
            XDocument xmlDoc = XDocument.Parse(dat);
            //var games = xmlDoc.Descendants("game");

            // iterate through each game
            foreach (XElement element in xmlDoc.Root.Elements("game"))
            {            
                string nameString = (string)element.Attribute("name");

                ToSecObject no = StringConverter.ParseString(nameString);
                no.SystemId = systemId;
                
                no.Description = (string)element.Element("description");
                XElement rom = element.Element("rom");

                no.RomName = (string)rom.Attribute("name");
                no.Size = (string)rom.Attribute("size");
                no.CRC = (string)rom.Attribute("crc");

                list.Add(no);
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
                case 1:
                    searchStr.Add("Nintendo Game Boy - ");
                    searchStr.Add("Nintendo Game Boy Color - ");
                    break;
                case 3:
                    searchStr.Add("Atari Lynx - ");
                    break;
                case 4:
                    searchStr.Add("Sega Mega Drive & Genesis - ");
                    break;
                case 5:
                    searchStr.Add("Sega Game Gear - ");
                    break;
                case 6:
                    searchStr.Add("SNK Neo-Geo Pocket ");
                    break;
                case 7:
                    searchStr.Add("NEC PC-Engine ");
                    searchStr.Add("NEC SuperGrafx ");
                    break;
                case 10:
                    searchStr.Add("Sega Mark III & Master System ");
                    break;
                case 11:
                    searchStr.Add("Nintendo Famicom & Entertainment System ");
                    break;
                case 12:
                    searchStr.Add("Nintendo Super Famicom & Super Entertainment System ");
                    break;
                case 14:
                    searchStr.Add("Nintendo Virtual Boy ");
                    break;
                case 15:
                    searchStr.Add("Bandai WonderSwan ");
                    break;
                case 8:
                    searchStr.Add("NEC PC-FX ");
                    break;
                case 9:
                    searchStr.Add("Sony PlayStation ");
                    break;
                case 13:
                    searchStr.Add("Sega Saturn ");
                    break;
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\TOSEC";

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
