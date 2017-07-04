using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace MedLaunch.Classes.Scraper.PSXDATACENTER
{
    public class PsxDc
    {
        public string Name { get; set; }
        public string Serial { get; set; }
        public string Region { get; set; }
        public string Languages { get; set; }
        public string InfoUrl { get; set; }
        public string Year { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }


        public static List<PsxDc> ScrapeInitialList()
        {
            List<PsxDc> list = new List<PsxDc>();
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;

            List<string> urls = new List<string>
            {
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\PSXDATACENTER\ntscu.html",
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\PSXDATACENTER\pal.html",
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\DAT\PSXDATACENTER\ntscj.html",
            };

            foreach (string url in urls)
            {
                // load html file to string
                string s = File.ReadAllText(url);
                doc.LoadHtml(s);

                // get all tables
                var findTables = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                                 from row in table.SelectNodes("tr").Cast<HtmlNode>()
                                 select row;

                // iterate through rows
                foreach (HtmlNode row in findTables)
                {         
                    var cells = row.SelectNodes("th|td").ToArray();

                    // Get all the serial numbers first and iterate through them (if no serial number present - ignore)
                    string[] snss = cells[1].InnerText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string[] sns = snss.Where(a => a.Trim() != "").ToArray();

                    for (int sn = 0; sn < sns.Length; sn++)
                    {
                        PsxDc record = new PsxDc();
                        record.Serial = sns[sn].Trim();

                        // get game name
                        string gText = cells[2].InnerText.Replace("&nbsp;", "").Replace("\r\n", "").Trim().Replace("  ", " ");
                        if (sns.Length > 1)
                        {
                            gText += " [Disc " + (sn + 1) + "]";
                        }
                        record.Name = gText;

                        // Languages
                        string langs = cells[3].InnerText.Replace("\r\n", "").Replace("&nbsp;", "").Trim();
                        record.Languages = langs;

                        // info url
                        if (cells[0].InnerHtml != "")
                        {
                            int pFrom = cells[0].InnerHtml.IndexOf("<a href=") + "<a href=".Length;
                            int pTo = cells[0].InnerHtml.LastIndexOf(" target=");
                            string result = cells[0].InnerHtml.Substring(pFrom, pTo - pFrom).Replace("\"", "");
                            record.InfoUrl = result;
                        }

                        // region
                        if (record.Serial.Contains("SLPS") ||
                            record.Serial.Contains("SCPS") ||
                            record.Serial.Contains("SLPM") ||
                            record.Serial.Contains("SIPS"))
                            record.Region = "JAP";

                        if (record.Serial.Contains("SLES") ||
                            record.Serial.Contains("SCES"))
                            record.Region = "EUR";

                        if (record.Serial.Contains("SLUS") ||
                            record.Serial.Contains("LSP") ||
                            record.Serial.Contains("SCUS"))
                            record.Region = "USA";


                        // add to master list
                        list.Add(record);                       

                    }

                }

            }


            return list;
        }

        public static PsxDc ScrapeIndividualInfo(PsxDc record)
        {
            // load page to string
            string html = new WebClient().DownloadString(record.InfoUrl);

            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.LoadHtml(html);

            // get all tables
            var findTables = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                             from row in table.SelectNodes("tr").Cast<HtmlNode>()
                             select row;
            bool foundDev = false;
            bool foundPub = false;
            bool foundYear = false;

            foreach (HtmlNode row in findTables)
            {
                if (foundDev && foundPub && foundYear)
                    break;

                var cells = row.SelectNodes("th|td").ToArray();
                if (cells[0].InnerText.Contains("Developer"))
                {
                    record.Developer = cells[1].InnerText.Replace("&nbsp;", "").Split('\t').Last().Trim().TrimEnd('.');
                    foundDev = true;
                }
                if (cells[0].InnerText.Contains("Publisher"))
                {
                    record.Publisher = cells[1].InnerText.Replace("&nbsp;", "").Split('\t').Last().Trim().TrimEnd('.');
                    foundPub = true;
                }
                if (cells[0].InnerText.Contains("Date Released"))
                {
                    int da = 0;
                    string[] dArr = cells[1].InnerText.Split(' ');
                    // test whether numeric value
                    bool result = int.TryParse(dArr.Last().Split('\t').Last().Trim(), out da);
                    if (result == true)
                    {
                        record.Year = dArr.Last().Split('\t').Last().Trim();
                    }
                    
                    foundYear = true;
                }
            }

            return record;
        }
    }

    
}
