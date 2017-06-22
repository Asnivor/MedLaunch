using HtmlAgilityPack;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes;
using MedLaunch.Classes.TheGamesDB;
using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch._Debug.ScrapeDB.ReplacementDocs
{
    public class RdScraper
    {
        public static void ScrapeBasicDocsList(ProgressDialogController controller)
        {
            List<ReplacementDocs> rdlist = new List<ReplacementDocs>();

            // iterate through mednafen systems
            var systems = GSystem.GetSystems();
            foreach (var sys in systems)
            {
                controller.SetMessage("Getting manual links for: " + sys.systemName + "\n");
                if (sys.systemId == 16 || sys.systemId == 17 || sys.systemId == 18)
                    continue;
                List<int> rdsystems = ConvertSystemId2RDSystemId(sys.systemId);

                // iterate through replacementdocs systems
                foreach (int s in rdsystems)
                {
                    // get the whole page for this system
                    WebOps wo = new WebOps();
                    wo.BaseUrl = "http://www.replacementdocs.com/download.php?";
                    wo.Params = "1.list." + s.ToString() + ".1000.download_name.ASC";
                    wo.Timeout = 20000;
                    string result = wo.ApiCall();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(result);

                    HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'fborder')]");

                    // iterate through each table row
                    foreach (HtmlNode row in table.ChildNodes)
                    {
                        if (row.ChildNodes.Count > 0)
                        {
                            HtmlNode[] cells = (from a in row.SelectNodes("td")
                                                select a).ToArray();
                            if (cells[0].InnerHtml.Contains("download.php?view."))
                            {
                                // this is a data cell
                                string title = cells[0].InnerText.Replace("\t", "").Trim();
                                string url = cells[0].InnerHtml.Replace("\t", "").Trim().Replace("<a href='download.php?view.", "");
                                string[] urlArr = url.Split('\'');
                                string fileId = urlArr[0];

                                var recordcheck = (from a in rdlist
                                                  where a.GameName == title && a.TGBSystemName == ConvertRDSystemId2TGBPlatformName(s)
                                                  select a).ToList();

                                if (recordcheck.Count > 0)
                                {
                                    ReplacementDocs r = recordcheck.FirstOrDefault();
                                    r.Urls.Add("http://www.replacementdocs.com/request.php?" + fileId);
                                    r.Urls.Distinct();
                                    rdlist.Add(r);
                                }
                                else
                                {
                                    ReplacementDocs r = new ReplacementDocs();
                                    r.GameName = title;
                                    r.TGBSystemName = ConvertRDSystemId2TGBPlatformName(s);
                                    r.Urls.Add("http://www.replacementdocs.com/request.php?" + fileId);
                                    r.Urls.Distinct();
                                    rdlist.Add(r);
                                }

                                rdlist.Distinct();                               
                            }
                        }
                    }
                }
            }

            // Add to scrapeDB
            foreach (var m in rdlist)
            {
                foreach (string entry in m.Urls)
                {
                    Game_Doc gd = new Game_Doc();
                    int pid = GDB_Platform.GetPlatforms().Where(a => a.name == m.TGBSystemName).FirstOrDefault().pid;
                    gd.pid = pid;
                    gd.gameName = m.GameName;
                    gd.downloadUrl = entry;

                    Game_Doc.AddDoc(gd);
                }
            }

            /*
            // save rdlist to json
            string json = JsonConvert.SerializeObject(rdlist, Formatting.Indented);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\replacementdocs-manuals.json", json);
            */
        }

        public static List<int> ConvertSystemId2RDSystemId(int systemId)
        {
            List<int> list = new List<int>();
            switch (systemId)
            {
                case 1:             // gameboy/gb color
                    list.Add(21);
                    list.Add(35);
                    break;
                case 2:             // gameboy advance
                    list.Add(8);
                    break;
                case 3:             // atari lynx
                    list.Add(23);
                    break;
                case 4:             // megadrive
                    list.Add(10);
                    break;
                case 5:             // gamegear
                    list.Add(45);
                    break;
                case 6:             // neogeo pocket/color
                    break;
                case 7:             // PC Engine
                    list.Add(18);
                    break;
                case 8:             // PCFX
                    break;
                case 9:             // playstation
                    list.Add(14);
                    break;
                case 10:            // master system
                    list.Add(27);
                    break;
                case 11:            // nes
                    list.Add(11);
                    break;
                case 12:            // snes
                    list.Add(16);
                    break;
                case 13:            // saturn
                    list.Add(26);
                    break;
                case 14:            // virtual boy
                    list.Add(46);
                    break;
                default:
                    // do nothing
                    break;
            }

            return list;
        }

        public static string ConvertRDSystemId2TGBPlatformName(int RdSysId)
        {
            switch (RdSysId)
            {
                case 21:             // gameboy
                    return "Nintendo Game Boy";
                case 35:            // gameboy color
                    return "Nintendo Game Boy Color";
                case 8:             // gameboy advance
                    return "Nintendo Game Boy Advance";
                case 23:             // atari lynx
                    return "Atari Lynx";
                case 10:             // megadrive
                    return "Sega Genesis";
                case 45:             // gamegear
                    return "Sega Game Gear";
                case 18:             // PC Engine
                    return "TurboGrafx 16";
                case 14:             // playstation
                    return "Sony Playstation";
                case 27:            // master system
                    return "Sega Master System";
                case 11:            // nes
                    return "Nintendo Entertainment System (NES)";
                case 16:            // snes
                    return "Super Nintendo (SNES)";
                case 26:            // saturn
                    return "Sega Saturn";
                case 46:            // virtual boy
                    return "Nintendo Virtual Boy";
                default:
                    return "";
            }
        }
    }

    public class ReplacementDocs
    {
        public string GameName { get; set; }
        public string TGBSystemName { get; set; }
        public List<string> Urls { get; set; }

        public ReplacementDocs()
        {
            Urls = new List<string>();
        }
    }
}
