using MedLaunch.Classes.Scraper.DAT.REDUMP.Models;
using MedLaunch.Classes.Scraper.DAT.TOSEC.Models;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedLaunch.Classes.Scraper.DAT.REDUMP
{
    public class ImportRedumpData
    {
        public static List<RedumpObject> Go()
        {
            // Import data for each system
            List<GSystem> systems = GSystem.GetSystems().ToList();

            List<RedumpObject> l = new List<RedumpObject>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.systemId);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<RedumpObject> list = Parse(s, sys.systemId);
                    l.AddRange(list);
                }
            }
            l.Distinct();
            return l;
        }

        public static List<RedumpObject> Parse(string dat, int systemId)
        {
            List<RedumpObject> list = new List<RedumpObject>();

            // replace illegal characters
            dat = dat.Replace(" & ", " &amp; ").Replace(" and ", " &amp; ");

            // parse into an xml document
            XDocument xmlDoc = XDocument.Parse(dat);
            //var games = xmlDoc.Descendants("game");

            // iterate through each game
            foreach (XElement element in xmlDoc.Root.Elements("game"))
            {            
                string nameString = (string)element.Attribute("name");

                RedumpObject no = StringConverterRedump.ParseString(nameString);
                no.SystemId = systemId;
                
                no.Description = (string)element.Element("description");
                IEnumerable<XElement> roms = element.Elements("rom");

                foreach (XElement rom in roms)
                {

                    RedumpObject t = new RedumpObject
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
                case 7:
                    searchStr.Add("NEC - PC-Engine ");
                    break;
                case 8:
                    searchStr.Add("NEC - PC-FX ");
                    break;
                case 9:
                    searchStr.Add("Sony - PlayStation ");
                    break;
                case 13:
                    searchStr.Add("Sega - Saturn ");
                    break;               
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\REDUMP";

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
