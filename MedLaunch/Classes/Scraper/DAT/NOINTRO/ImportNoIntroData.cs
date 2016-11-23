using MedLaunch.Classes.Scraper.DAT.NOINTRO.Models;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scraper.DAT.NOINTRO
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

            // spit the string based on 2 new lines
            string[] split1 = dat.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
            // count the number of entities
            int count = split1.Length;
            // iterate over each entity (exluding the first)
            for (int i = 1; i < count; i++)
            {
                // now split the entity based on single new line
                string[] split2 = split1[i].Replace("\t", "").Split(new string[] { "\r\n" }, StringSplitOptions.None);

                // continue the loop if the entity is empty
                if (split2.Length < 2)
                    continue;

                NoIntroObject no = new NoIntroObject();
                no.SystemId = systemId;

                // process name line
                no.Name = split2[1].Replace("name", "").Replace("\"", "").Trim();
                // process description
                no.Description = split2[2].Replace("description", "").Replace("\"", "").Trim();
                // process big line
                string bigline = split2[3].Replace("rom ( name ", "").Replace("\"", "").Trim();
                // split based on size
                string[] split3 = bigline.Split(new string[] { "size" }, StringSplitOptions.None);
                no.Rom = split3[0].Trim();
                // split remaining details
                string[] split4 = split3[1].Trim().Split(' ');
                no.Size = split4[0].Trim();
                no.CRC = split4[2].Trim();
                no.MD5 = split4[4].Trim();
                no.SHA1 = split4[6].Replace(")", "").Trim();

                list.Add(no);
            }

            return list;
        }

        public static List<string> LoadDATs(int systemId)
        {
            List<string> searchStr = new List<string>();
            switch (systemId)
            {
                case 1:
                    searchStr.Add("Nintendo - Game Boy (");
                    searchStr.Add("Nintendo - Game Boy Color (");
                    break;
                case 2:
                    searchStr.Add("Nintendo - Game Boy Advance (");
                    break;
                case 3:
                    searchStr.Add("Atari - Lynx (");
                    break;
                case 4:
                    searchStr.Add("Sega - Mega Drive - Genesis (");
                    break;
                case 5:
                    searchStr.Add("Sega - Game Gear (");
                    break;
                case 6:
                    searchStr.Add("SNK - Neo Geo Pocket ");
                    break;
                case 7:
                    searchStr.Add("NEC - PC Engine");
                    searchStr.Add("NEC - Super");
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
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"\Data\System\DAT\NOINTRO";

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
