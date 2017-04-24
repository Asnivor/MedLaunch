using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.Scraper;
using MedLaunch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ScrapedDataAudit.xaml
    /// </summary>
    public partial class ScrapedDataAudit : ChildWindow
    {
        public List<FolderData> Data { get; set; }
        public string BaseFolder { get; set; }
        public List<ScraperMaster> Master { get; set; }

        public ScrapedDataAudit()
        {
            InitializeComponent();

            Data = new List<FolderData>();

            Master = ScraperMaster.GetMasterList();

            // enumerate game data folder
            BaseFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Games";

            var fol = Directory.GetDirectories(BaseFolder);
            foreach (var d in fol)
            {
                string fName = System.IO.Path.GetFileName(d);

                // check whether folder name is a valid number
                int n;
                bool isNumeric = int.TryParse(fName, out n);
                if (isNumeric == false)
                    continue;

                // build folderdata object
                FolderData fd = new FolderData();
                fd.FolderName = n;

                // find gdbid from Master
                var g = (from a in Master
                         where a.gid == n
                         select a).FirstOrDefault();

                // is result empty?
                if (g == null)
                    continue;

                fd.GameName = g.GDBTitle;
                fd.System = g.GDBPlatformName; //GSystem.GetSystemName(Convert.ToInt32(g.MedLaunchSystemId));

                // now check whether it is linked anywhere in the database
                var search = from a in Game.GetGames()
                             where a.gdbId == n
                             select a;

                if (search.Count() > 0)
                {
                    fd.IsLinked = true;
                }
                else
                {
                    fd.IsLinked = false;
                }

                // add to list
                Data.Add(fd);
            }

            Data.OrderBy(a => a.FolderName);

            // populate the datagrid
            dgAudit.ItemsSource = Data;
        }

        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLocal_Click(object sender, RoutedEventArgs e)
        {
            // get the id
            var r = (FolderData)dgAudit.SelectedItem;
            string gdbid = r.FolderName.ToString();

            // open the folder in windows explorer
            string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Games\" + gdbid;
            // check folder exists
            if (Directory.Exists(dirPath))
            {
                // open the folder
                Process.Start(dirPath);
            }
        }

        private void btnOnline_Click(object sender, RoutedEventArgs e)
        {
            // get the id
            var r = (FolderData)dgAudit.SelectedItem;
            string gdbid = r.FolderName.ToString();

            // open thegamesdb.net url in the default browser
            string url = @"http://thegamesdb.net/game/" + gdbid;
            Process.Start(url);
        }
    }

    public class FolderData
    {
        public int FolderName { get; set; }              // gdbid
        public string System { get; set; }
        public string GameName { get; set; }
        public bool IsLinked { get; set; }
    }
}
