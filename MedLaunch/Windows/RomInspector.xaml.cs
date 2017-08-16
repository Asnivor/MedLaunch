using MahApps.Metro.SimpleChildWindow;
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
using System.Windows.Shapes;
using MedLaunch.Classes;
using MedLaunch.Models;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Extensions;
using MedLaunch.Classes.MasterScraper;
using MedLaunch.Classes.Scraper;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.Scanning;
using ucon64_wrapper;


namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ListBoxChildWindow.xaml
    /// </summary>
    public partial class RomInspector : ChildWindow
    {
        public string LaunchString { get; set; }
        public string[] DiscArray { get; set; }

        public RomInspector()
        {
            this.InitializeComponent();

            // get the mainwindow
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            int numRowsCount = mw.dgGameList.SelectedItems.Count;
            if (numRowsCount != 1)
                return;

            GamesLibraryModel drv = (GamesLibraryModel)mw.dgGameList.SelectedItem;
            if (drv == null)
                return;
            int romId = drv.ID;

            // get game object
            Game g = Game.GetGame(romId);

            // init ucon64 wrapper
            ucon64_wrapper.UconWrapper u = new ucon64_wrapper.UconWrapper(System.AppDomain.CurrentDomain.BaseDirectory + @"\ucon64-bin\ucon64.exe");
            string gamePath = Game.ReturnActualGamePath(g);

            UconResult obj = new UconResult();

            // is it a ROM or a DISC?
            string extension = System.IO.Path.GetExtension(gamePath);            

            if (extension.ToLower().Contains("m3u") ||
                extension.ToLower().Contains("cue") ||
                extension.ToLower().Contains("ccd") ||
                extension.ToLower().Contains("toc")
                )
            {
                DiscGameFile dgf = new DiscGameFile(gamePath, g.systemId);
                var files = DiscScan.ParseTrackSheetForImageFiles(dgf, g.systemId);

                // iterate through each image file and scan it
                int cnt = 0;
                foreach (var im in files)
                {
                    var res = u.ScanGame(im.FullPath, uConOps.GetSystemType(g.systemId));
                    tbInsResult.Text += res.RawOutput;
                    if (files.Count > 1)
                        tbInsResult.Text += "\n****************************\n";
                    if (cnt == 0)
                        obj = res;
                    cnt++;
                }
            }
            
            else
            {
                // check whether game is within 7zip or part of a multi-game archive
                if (g.gamePath.Contains("*/"))
                {
                    // need to extract game first before scanning
                    GameLauncher gl = new GameLauncher(g.gameId);
                    string tempPath = gl.BuildFullGamePath(Paths.GetSystemPath(g.systemId), g.gamePath);

                    // scan game
                    var re = u.ScanGame(tempPath, uConOps.GetSystemType(g.systemId));

                    // update textbox
                    tbInsResult.Text = re.RawOutput;
                    obj = re;
                }
                else
                {
                    // no extraction needed
                    var result = u.ScanGame(gamePath, uConOps.GetSystemType(g.systemId));

                    // update text box
                    tbInsResult.Text = result.RawOutput;
                    obj = result;
                }                
            }


            /* populate other textboxes */

            // raw
            tbIntSystem.Text = obj.Data.DetectedSystemType;
            tbIntGame.Text = obj.Data.DetectedGameName;
            tbIntPublisher.Text = obj.Data.DetectedPublisher;
            tbIntRegion.Text = obj.Data.DetectedRegion;
            tbIntType.Text = obj.Data.DetectedRomType;
            tbIntYear.Text = obj.Data.DetectedYear;
            tbIntChecksumString.Text = obj.Data.DetectedChecksumComparison;
            if (obj.Data.IsInterleaved == true)
                tbIntInterleaving.Text = "Yes";
            if (obj.Data.IsInterleaved == false)
                tbIntInterleaving.Text = "No";
            tbIntChecksumCrc.Text = obj.Data.CRC32;

            if (tbIntChecksumString.Text.Contains("Bad,"))
            {
                tbIntChecksumString.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (tbIntChecksumString.Text.Contains("OK,"))
            {
                tbIntChecksumString.Foreground = new SolidColorBrush(Colors.Green);
            }

            tbIntVersion.Text = obj.Data.DetectedVersion;
            tbIntPadding.Text = obj.Data.DetectedPadding;
            tbIntSize.Text = obj.Data.DetectedSize;

            // status box
            string resultMessage = obj.Status;

            if (obj.Data.systemType == SystemType.Genesis && obj.Data.romType == RomType.SMD)
            {
                if (obj.Data.IsChecksumValid == false)
                {
                    resultMessage = "Checksum invalid\n";
                    if (obj.Data.IsInterleaved == true)
                    {
                        // needs de-interleaving
                        resultMessage += "Also, this is a genesis SMD ROM that is interleaved. Mednafen will not run this game and MedLaunch cannot currently convert it.";
                    }
                    if (obj.Data.IsInterleaved == false)
                    {
                        // game will most probably play
                        resultMessage += "However this is a genesis ROM that is NOT interleaved. It should work in Mednafen.";
                    }
                }
                else
                {
                    resultMessage = "Checksum is valid\n";
                    if (obj.Data.IsInterleaved == true)
                    {
                        // needs de-interleaving
                        resultMessage += "This is a genesis SMD ROM that is interleaved. MedLaunch will auto-convert this game at launch-time so that it works with Mednafen.";
                    }
                    if (obj.Data.IsInterleaved == false)
                    {
                        // game will most probably play
                        resultMessage += "This is a genesis ROM that is NOT interleaved. It should work in Mednafen without conversion.";
                    }
                }                
            }

            tbIntResult.Text = resultMessage;

            

        }
        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
    
}
