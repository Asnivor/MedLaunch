using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace ReleaseGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // generate new release
            Release r = new Release();

            // Populate release object
            r.Version = Version.Text.Trim();
            r.Date = Convert.ToDateTime(Date.Text);
            r.Notes = Notes.Text;
            List<string> cl = new List<string>();
            string c = ChangeLog.Text;
            string[] cArr = c.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string s in cArr)
            {
                cl.Add(s);
            }
            r.Changelog = cl;

            // save to file with filename being the version
            string folderPath = @"..\..\Releases\";
            string fileName = r.Version + ".json";
            string fullPath = folderPath + fileName;

            string json = JsonConvert.SerializeObject(r, Formatting.Indented);
            File.WriteAllText(fullPath, json);

        }
    }
}
