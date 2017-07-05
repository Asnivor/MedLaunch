using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace MedLaunch._Debug.DATDB.Platforms.PSXDATACENTER
{
    public class PsxDc
    {
        public MainWindow mw { get; set; }

        public PsxDc()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        public static List<PSX_Games> ScrapeInitialList(bool downloadFiles)
        {
            List<PSX_Games> list = new List<PSX_Games>();
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;

            List<string> urls = new List<string>
            {
                "http://psxdatacenter.com/ulist.html",
                "http://psxdatacenter.com/plist.html",
                "http://psxdatacenter.com/jlist.html",
            };

            List<string> locals = new List<string>
            {
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\_Debug\DATDB\DATFiles\PSXDATACENTER\ntscu.html",
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\_Debug\DATDB\DATFiles\PSXDATACENTER\pal.html",
                AppDomain.CurrentDomain.BaseDirectory + @"..\..\_Debug\DATDB\DATFiles\PSXDATACENTER\ntscj.html",
            };

            if (downloadFiles == true)
            {
                // download the webpages
                for (int i = 0; i < urls.Count(); i++)
                {
                    // download the latest version of the webpage
                    string webResult = "";
                    using (var client = new WebClient())
                    {
                        webResult = client.DownloadString(urls[i]);
                    }
                    // save to disk
                    File.WriteAllText(locals[i], webResult);
                }
            }
            

            for (int i = 0; i < urls.Count(); i++)
            {
                // load html file to string
                string s = File.ReadAllText(locals[i]);
                doc.LoadHtml(s);

                // get all tables

                var tables = doc.DocumentNode
                    .Descendants("table")
                    .Where(d => d.Attributes.Contains("class") &&
                    d.Attributes["class"].Value.Contains("sectiontable")).Cast<HtmlNode>();                    


                var findTables = from table in doc.DocumentNode.SelectNodes("//table")
                                  .Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.Contains("sectiontable")).Cast<HtmlNode>()
                                 from row in table.SelectNodes("tr").Cast<HtmlNode>()
                                 select row;

                // iterate through rows
                foreach (HtmlNode row in findTables)
                {
                    if (row.InnerText == "\r\n" || row.InnerText.Trim() == "")
                        continue;

                    var cells = row.SelectNodes("th|td").ToArray();

                    // Get all the serial numbers first and iterate through them (if no serial number present - ignore)
                    string[] snss = cells[1].InnerHtml.Split(new string[] { "<br>" }, StringSplitOptions.None);
                    string[] sns = snss.Where(a => a.Trim() != "").ToArray();

                    for (int sn = 0; sn < sns.Length; sn++)
                    {
                        PSX_Games record = new PSX_Games();
                        record.serial = sns[sn].Trim();

                        // get game name
                        string gText = cells[2].InnerText.Replace("&nbsp;", "").Replace("\r\n", "").Trim().Replace("  ", " ");
                        if (sns.Length > 1)
                        {
                            gText += " [Disc " + (sn + 1) + "]";
                        }
                        record.name = gText;

                        // Languages
                        string langs = cells[3].InnerText.Replace("\r\n", "").Replace("&nbsp;", "").Trim();
                        record.languages = langs;

                        // info url
                        if (cells[0].InnerHtml != "")
                        {
                            /*
                            int pFrom = cells[0].InnerHtml.IndexOf("<a href=") + "<a href=".Length;
                            int pTo = cells[0].InnerHtml.LastIndexOf(" target=");
                            string result = cells[0].InnerHtml.Substring(pFrom, pTo - pFrom).Replace("\"", "");
                            */

                            string[] arr1 = cells[0].InnerHtml.Split(new string[] { "href=\"games/" }, StringSplitOptions.None);
                            string result = "http://psxdatacenter.com/games/" + arr1[1].Replace("\">INFO</a>", "").Trim();


                            record.infoUrl = result;
                        }

                        // region
                        if (record.serial.Contains("SLPS") ||
                            record.serial.Contains("SCPS") ||
                            record.serial.Contains("SLPM") ||
                            record.serial.Contains("SIPS"))
                            record.region = "JAP";

                        if (record.serial.Contains("SLES") ||
                            record.serial.Contains("SCES"))
                            record.region = "EUR";

                        if (record.serial.Contains("SLUS") ||
                            record.serial.Contains("LSP") ||
                            record.serial.Contains("SCUS"))
                            record.region = "USA";


                        // add to master list
                        list.Add(record);

                    }

                }

            }

            // update in database
            int[] r = PSX_Games.SaveToDatabase(list);

            MessageBox.Show("Games Added: " + r[0] + 
                "\nGames Updated: " + r[1]);


            return list;
        }

        public static async void GetExtraDetail()
        {
            PsxDc c = new PsxDc();

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel",
                AnimateShow = false,
                AnimateHide = false
            };
            string output = "Getting games that need additional info scraped...\n";
            var controller = await c.mw.ShowProgressAsync("PSXDATACENTER Builder", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1);

            await Task.Run(() =>
            {
                // load games
                List<PSX_Games> games = PSX_Games.GetGames()
                .Where(
                    a => a.infoUrl != null && a.infoUrl.Trim() != "" &&
                    (
                        a.publisher == null ||
                        a.developer == null ||
                        a.year == null
                    )
                    )
                    .OrderBy(x => x.id)
                .ToList();

                int totalGamesToProcess = games.Count();

                //List<PSX_Games> gWorking = new List<PSX_Games>();

                output = "Performing lookups...\n";

                // iterate through each game and get extra details
                for (int i = 0; i < totalGamesToProcess; i++)
                {
                    controller.SetMessage(output + "\nGame " + i + " of " + totalGamesToProcess);
                    var re = ScrapeIndividualInfo(games[i]);
                    if (re == null)
                        continue;

                    // some arbitrary wait time to not spam the server
                    //System.Threading.Thread.Sleep(1000);

                    // add/update the database
                    PSX_Games.SaveToDatabase(re);
                }
                

            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await c.mw.ShowMessageAsync("DAT Builder", "Linking Cancelled");
            }
            else
            {
                await c.mw.ShowMessageAsync("DAT Builder", "Linking Completed");
            }
        }


        public static PSX_Games ScrapeIndividualInfo(PSX_Games record)
        {
            // load page to string
            string html = new WebClient().DownloadString(record.infoUrl);

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
                    record.developer = cells[1].InnerText.Replace("&nbsp;", "").Split('\t').Last().Trim().TrimEnd('.');
                    foundDev = true;
                }
                if (cells[0].InnerText.Contains("Publisher"))
                {
                    record.publisher = cells[1].InnerText.Replace("&nbsp;", "").Split('\t').Last().Trim().TrimEnd('.');
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
                        record.year = dArr.Last().Split('\t').Last().Trim();
                    }

                    foundYear = true;
                }
            }

            return record;
        }
    }
}
