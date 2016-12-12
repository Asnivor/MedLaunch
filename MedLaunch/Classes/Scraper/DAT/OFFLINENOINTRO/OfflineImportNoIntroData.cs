using MedLaunch.Classes.Scraper.DAT.OFFLINENOINTRO.Models;
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
    public class OfflineImportNoIntroData
    {
        public static List<OfflineNoIntroObject> Go()
        {
            // Import data for each system
            List <GSystem> systems = GSystem.GetSystems().ToList();

            List<OfflineNoIntroObject> l = new List<OfflineNoIntroObject>();

            foreach (var sys in systems)
            {
                // get a list of strings containing all data info for this system
                List<string> dats = LoadDATs(sys.systemId);

                // iterate through each data and parse the information into a new object
                foreach (string s in dats)
                {
                    List<OfflineNoIntroObject> list = Parse(s, sys.systemId);
                    l.AddRange(list);
                }
            }
            l.Distinct();
            return l;
        }

       

        public static List<OfflineNoIntroObject> Parse(string dat, int systemId)
        {
            List<OfflineNoIntroObject> list = new List<OfflineNoIntroObject>();

            // replace illegal characters
            dat = dat.Replace(" & ", " &amp; ");

            // parse into an xml document
            XDocument xmlDoc = XDocument.Parse(dat);
            //var games = xmlDoc.Descendants("game");

            IEnumerable<XElement> els =
                from el in xmlDoc.Descendants("game")
                select el;

            foreach (XElement x in els)
            {
                OfflineNoIntroObject no = new OfflineNoIntroObject();
                no.SystemId = systemId;
                no.Name = (string)x.Element("title");
                no.Publisher = (string)x.Element("publisher");
                no.Size = (string)x.Element("romSize");
                no.OtherFlags = (string)x.Element("comment");

                IEnumerable<XElement> roms = x.Descendants("files");
                var rom = roms.FirstOrDefault();

                if (rom != null)
                {
                    no.CRC = (string)rom.Element("romCRC");
                    string ext = (string)rom.Attribute("extension");
                    no.RomName = no.Name + ext;
                }

            }
            

            // iterate through each game
            foreach (XElement element in xmlDoc.Elements("game"))
            {
                OfflineNoIntroObject no = new OfflineNoIntroObject();
                no.SystemId = systemId;
                //no.Name = (string)element.Attribute("name");
                no.Name = (string)element.Element("title");
                //no.CloneOf = (string)element.Attribute("cloneof");
                //no.Description = (string)element.Element("description");
                no.Publisher = (string)element.Element("publisher");
                no.Size = (string)element.Element("romSize");
                


                XElement rom = element.Element("files");

                no.RomName = (string)rom.Attribute("name");
                no.Size = (string)rom.Attribute("size");
                no.CRC = (string)rom.Attribute("crc");
                no.MD5 = (string)rom.Attribute("md5");
                no.SHA1 = (string)rom.Attribute("sha1");

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
                    searchStr.Add("Nintendo Gameboy Advance ");
                    break;
                case 1:
                    searchStr.Add("Nintendo Gameboy.xml");
                    searchStr.Add("Nintendo Gameboy Color ");
                    break;                
                case 3:
                    searchStr.Add("Atari Lynx ");
                    break;
                case 4:
                    searchStr.Add("Sega Genesis ");
                    break;
                case 5:
                    searchStr.Add("Sega GameGear ");
                    break;
                case 6:
                    searchStr.Add("SNK NeoGeo ");
                    break;
                case 7:
                    searchStr.Add("NEC PC Engine ");
                    break;
                case 10:
                    searchStr.Add("Sega Master System ");
                    break;
                case 11:
                    searchStr.Add("Nintendo NES ");
                    break;
                case 12:
                    searchStr.Add("Nintendo Super NES ");
                    break;
                case 14:
                    searchStr.Add("Nintendo Virtualboy");
                    break;
                case 15:
                    searchStr.Add("Bandai WonderSwan");
                    break;
            }

            List<string> data = new List<string>();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\OFFLINENOINTRO";

            // get all data files for this system
            if (!Directory.Exists(folder))
                return null;

            List<string> files = Directory.GetFiles(folder).ToList();
            List<string> f = new List<string>();
            foreach (string s in searchStr)
            {
                foreach (string b in files.Where(a => a.Contains(".xml")))
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
