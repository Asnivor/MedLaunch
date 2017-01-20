using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.Toolkit;
using MahApps.Metro.Controls;
using MedLaunch.Classes.HtmlToXaml;

namespace MedLaunch.Classes
{
    public class ConfigToolTips
    {
        /// <summary>
        /// 1 = set tooltips
        /// 2 = unset tooltips
        /// </summary>
        /// <param name="SetOrUnset"></param>
        public static void SetToolTips(int SetOrUnset)
        {
            
            // get all config controls
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            // find the root grid
            Grid RootGrid = (Grid)mw.FindName("RootGrid");
            UIHandler ui = UIHandler.GetChildren(RootGrid);

            UIHandler u = new UIHandler();
            u.Buttons = ui.Buttons.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<Button>();
            u.CheckBoxes = ui.CheckBoxes.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<CheckBox>();
            u.Colorpickers = ui.Colorpickers.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<ColorPicker>();
            u.ComboBoxes = ui.ComboBoxes.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<ComboBox>();
            u.Labels = ui.Labels.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<Label>();
            u.NumericUpDowns = ui.NumericUpDowns.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<NumericUpDown>();
            u.RadioButtons = ui.RadioButtons.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<RadioButton>();
            u.Sliders = ui.Sliders.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<Slider>();
            u.TextBoxes = ui.TextBoxes.Where(a => a.Name.StartsWith("cfg_") || a.Name.StartsWith("cfglbl_")).ToList() ?? new List<TextBox>();
            u.ToggleButtons = new List<System.Windows.Controls.Primitives.ToggleButton>();

            // load json
            List<ToolTips> tips = JsonConvert.DeserializeObject<List<ToolTips>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Data\System\ToolTips.json"));

            // now iterate through each set of controls and assign the tooltips
            foreach (Button c in u.Buttons)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            foreach (CheckBox c in u.CheckBoxes)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            foreach (ColorPicker c in u.Colorpickers)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            foreach (ComboBox c in u.ComboBoxes)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            foreach (Label c in u.Labels)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }
            
            foreach (NumericUpDown c in u.NumericUpDowns)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }
            

            foreach (RadioButton c in u.RadioButtons)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            foreach (Slider c in u.Sliders)
            {
                string Name = c.Name.Replace("cfglbl_", "").Replace("cfg_", "").Replace("__", ".");
                // lookup description
                var bu = tips.Where(a => a.Command.Contains(Name)).ToList();
                if (bu.Count() > 0)
                {
                    ToolTip tool = new ToolTip();
                    TextBlock tb = new TextBlock();
                    tb.TextWrapping = TextWrapping.Wrap;
                    tb.MaxWidth = 800;
                    tb.Text = bu.First().Description;
                    tool.Content = tb;
                    if (c.ToolTip == null)
                    {
                        c.ToolTip = tool;
                    }
                }
            }

            // skip textboxes

        }

        public static List<ToolTips> GetDocumentationStrings()
        {
            string baseUrl = "https://mednafen.github.io/documentation/";
            List<ToolTips> tt = new List<ToolTips>();

            // get all webpages
            string documentation = new WebClient().DownloadString(baseUrl);
            string lynx = new WebClient().DownloadString(baseUrl + "lynx.html");
            string gb = new WebClient().DownloadString(baseUrl + "gb.html");
            string gba = new WebClient().DownloadString(baseUrl + "gba.html");
            string ngp = new WebClient().DownloadString(baseUrl + "ngp.html");
            string nes = new WebClient().DownloadString(baseUrl + "nes.html");
            string pce_fast = new WebClient().DownloadString(baseUrl + "pce_fast.html");
            string pce = new WebClient().DownloadString(baseUrl + "pce.html");
            string gg = new WebClient().DownloadString(baseUrl + "gg.html");
            string md = new WebClient().DownloadString(baseUrl + "md.html");
            string sms = new WebClient().DownloadString(baseUrl + "sms.html");
            string ss = new WebClient().DownloadString(baseUrl + "ss.html");
            string psx = new WebClient().DownloadString(baseUrl + "psx.html");
            string snes_faust = new WebClient().DownloadString(baseUrl + "snes_faust.html");
            string snes = new WebClient().DownloadString(baseUrl + "snes.html");
            string vb = new WebClient().DownloadString(baseUrl + "vb.html");
            string wswan = new WebClient().DownloadString(baseUrl + "wswan.html");


            // parse individual system documentation
            ParseSystems(tt, lynx);
            ParseSystems(tt, gb);
            ParseSystems(tt, gba);
            ParseSystems(tt, ngp);
            ParseSystems(tt, nes);
            ParseSystems(tt, pce_fast);
            ParseSystems(tt, pce);
            ParseSystems(tt, gg);
            ParseSystems(tt, md);
            ParseSystems(tt, sms);
            ParseSystems(tt, ss);
            ParseSystems(tt, psx);
            ParseSystems(tt, snes_faust);
            ParseSystems(tt, snes);
            ParseSystems(tt, vb);
            ParseSystems(tt, wswan);

            // parse main documentation
            ParseMain(tt, documentation);

            // now tt contains all documentation config commands - output to json
            string saveLocation = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Data\System\ToolTips.json";
            string json = JsonConvert.SerializeObject(tt, Formatting.Indented);
            File.WriteAllText(saveLocation, json);

            return tt;

        }

        public static void ParseMain(List<ToolTips> ttList, string str)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            var tds = doc.DocumentNode.SelectNodes("//td[@class='ColE']");
            foreach (HtmlNode node in tds)
            {
                string html = node.InnerHtml;
                string[] arr = html.Split('>');
                string command = arr[0].Replace("<a name=\"", "").Replace("\"", "");

                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < arr.Length; i++)
                {
                    sb.Append(arr[i]);
                    sb.Append(">");
                }

                string h = sb.ToString();
                string desc = h.Replace("</a>", "")
                    .Replace("<p>", "\n\n")
                    //.Replace("</p>", "\n\n")
                    .Replace("<br>", "\n")
                    .Replace("<ul>", "\n")
                    .Replace("<li>", "\n")
                    .Replace("</li>", "")
                    .Replace("</ul>", "")
                    .Replace("</ul", "")
                    .Replace("</p", "")
                    .Replace("<b>", "")
                    .Replace("</b>", "")
                    .TrimEnd('>');


                //string desc = node.InnerText.Replace("<p>", "&#x0a;");

                ToolTips t = new ToolTips();
                t.Command = command;
                t.Description = desc;

                ttList.Add(t);
            }
        }

        public static void ParseSystems(List<ToolTips> ttList, string str)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            var tds = doc.DocumentNode.SelectNodes("//td[@class='ColE']");
            foreach (HtmlNode node in tds)
            {
                string html = node.InnerHtml;
                string[] arr = html.Split('>');
                string command = arr[0].Replace("<a name=\"", "").Replace("\"", "");

                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < arr.Length; i++)
                {
                    sb.Append(arr[i]);
                    sb.Append(">");
                }

                string h = sb.ToString();
                string desc = h.Replace("</a>", "")
                    .Replace("<p>", "\n\n")
                    //.Replace("</p>", "\n\n")
                    .Replace("<br>", "\n")
                    .Replace("<ul>", "\n")
                    .Replace("<li>", "\n")
                    .Replace("</li>", "")
                    .Replace("</ul>", "")
                    .Replace("</ul", "")
                    .Replace("</p", "")
                    .Replace("<b>", "")
                    .Replace("</b>", "")
                    .TrimEnd('>');

                string test = HtmlToXamlConverter.ConvertHtmlToXaml(h, false);


                //string desc = node.InnerText.Replace("<p>", "&#x0a;");

                ToolTips t = new ToolTips();
                
                t.Command = command;
                t.Description = desc;

                ttList.Add(t);
            }
           
        }


    }

    public class ToolTips
    {
        public string Command { get; set; }
        public string Description { get; set; }
    }
}
