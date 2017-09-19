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
using System.IO;
using MedLaunch.Classes.DAT;
using MedLaunch.Classes.Scraper.DBModels;
using MedLaunch.Classes.IO;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for ListBoxChildWindow.xaml
    /// </summary>
    public partial class RomInspector : ChildWindow
    {
        public string LaunchString { get; set; }
        public string[] DiscArray { get; set; }
        public Game GameObj { get; set; }
        public List<DAT_Rom> Roms { get; set; }

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
            GameObj = g;

            // populate tab textboxes with game name
            tbRawInspecResults.Text = g.gamePath;
            tbLibInspecResults.Text = g.gamePath;

            // init manual config button
            if (g.ManualEditSet == true)
            {
                btnToggleManualEdit.IsChecked = true;
                EnableControls();
            }
            else
            {
                btnToggleManualEdit.IsChecked = false;
                DisableControls();
            }

            // init ucon64 wrapper
            ucon64_wrapper.UconWrapper u = new ucon64_wrapper.UconWrapper(System.AppDomain.CurrentDomain.BaseDirectory + @"\ucon64-bin\ucon64.exe");
            string gamePath = Game.ReturnActualGamePath(g);

            UconResult obj = new UconResult();

            // is it a ROM or a DISC?
            string extension = System.IO.Path.GetExtension(gamePath);            

            if (extension.ToLower().Contains(".m3u") ||
                extension.ToLower().Contains(".cue") ||
                extension.ToLower().Contains(".ccd") ||
                extension.ToLower().Contains(".toc")
                )
            {

                if (g.systemId == 13)
                {
                    // saturn
                    DiscGameFile dgf = new DiscGameFile(gamePath, g.systemId);
                    // get saturn data from disk image
                    var file = DiscScan.ParseTrackSheetForImageFiles(dgf, g.systemId).FirstOrDefault();
                    if (file != null)
                    {
                        var satInfo = MedDiscUtils.GetSSData(file.FullPath);
                        string outp = "Initialising MedLaunch Sega Saturn DISC Interogator\n*******************************************\n\n";
                        outp += satInfo.Title + "\n";
                        outp += satInfo.SerialNumber + "\n";
                        outp += satInfo.Date + "\n";
                        outp += satInfo.Country + "\n";
                        outp += satInfo.JpnTitle + "\n";
                        outp += satInfo.PeriphCode + "\n";
                        outp += satInfo.CountryCode + "\n";

                        tbInsResult.Text = outp;

                        // pop tbs
                        tbIntGame.Text = satInfo.Title;
                        tbIntRegion.Text = satInfo.Country;
                        tbIntYear.Text = satInfo.Date;
                        tbIntVersion.Text = satInfo.SerialNumber;
                    }
                }
                else if (g.systemId == 9)
                {
                    // psx
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

                /*
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
                */
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


                // library data tab

                
            }

            if (GameObj.systemId == 9)
            {
                // get the playstation serial
                string ser = MedDiscUtils.GetPSXSerial(GameObj.gamePath);
                tbIntVersion.Text = ser;
            }

            PopulateLibraryData();
            LookupDAT();
            LookupScrapeMatched();


            /* populate other textboxes */

            // raw
            if (g.systemId != 13)
            {
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
            

            //populate rom field on library tab
            LookupROM();

            // add gameobj to mainwindow
            mw.InspGame = GameObj;

        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double winHeight = ChildWindowHeight;
            double dataHeight = (winHeight / 100.00) * 80.00;

            //InspectorRootGrid.Height = dataHeight;

            mainScroll.Height = dataHeight; // new GridLength(dataHeight);
        }

        private void LookupScrapeMatched()
        {
            if (GameObj.gdbId != null && GameObj.gdbId > 0)
            {
                var master = MasterView.GetMasterView();
                var entry = master.Where(a => a.gid == GameObj.gdbId).FirstOrDefault();
                if (entry == null)
                    return;

                tbScrapeData_gameName.Text = entry.GDBTitle;
                tbScrapeData_Year.Text = entry.GDBYear;

                // get disk data
                //ScrapeDB gd = new ScrapeDB();
                ScrapedGameObject o = ScrapeDB.GetScrapedGameObject(GameObj.gameId, GameObj.gdbId.Value);

                tbScrapeData_Year.Text = o.Data.Released;
                tbScrapeData_Developer.Text = o.Data.Developer;
                tbScrapeData_Publisher.Text = o.Data.Publisher;

                StringBuilder sbAT = new StringBuilder();
                if (o.Data.AlternateTitles == null)
                    o.Data.AlternateTitles = new List<string>();

                for (int i = 0; i < o.Data.AlternateTitles.Count(); i++)
                {
                    sbAT.Append(o.Data.AlternateTitles[i]);
                    if (i > 0 && i < (o.Data.AlternateTitles.Count() - 1))
                        sbAT.Append(", ");
                }
                tbScrapeData_AlternateTitles.Text = sbAT.ToString();

                tbScrapeData_Coop.Text = o.Data.Coop;
                tbScrapeData_ESRB.Text = o.Data.ESRB;

                StringBuilder sbGN = new StringBuilder();
                if (o.Data.Genres == null)
                    o.Data.Genres = new List<string>();

                for (int i = 0; i < o.Data.Genres.Count(); i++)
                {
                    sbGN.Append(o.Data.Genres[i]);
                    if (i > 0 && i < (o.Data.Genres.Count() - 1))
                        sbGN.Append(", ");                        
                }
                tbScrapeData_Genres.Text = sbGN.ToString();

                tbScrapeData_Overview.Text = o.Data.Overview;
                tbScrapeData_Players.Text = o.Data.Players;

                if (o.GdbId != 0)
                    tbScrapeData_gdbId.Text = o.GdbId.ToString();

            }
        }

        private void LookupDAT()
        {
            // initial lookup to create the object
            var lookup = DATMerge.GetDAT(GameObj.CRC32);

            // psx
            if (GameObj.systemId == 9)
            {
                lookup = DATMerge.GetDATsBySN(9, GameObj.OtherFlags);
            }

            // saturn
            if (GameObj.systemId == 13)
            {
                lookup = DATMerge.GetDATsBySN(13, GameObj.OtherFlags);
            }
            

            if (lookup == null)
            {
                // no match found
            }
            else
            {
                tbDatData_Copyright.Text = lookup.Copyright;
                tbDatData_Country.Text = lookup.Country;
                tbDatData_Developer.Text = lookup.Developer;
                tbDatData_DevelopmentStatus.Text = lookup.DevelopmentStatus;
                tbDatData_gameName.Text = lookup.GameName;
                tbDatData_Language.Text = lookup.Language ;
                tbDatData_OtherFlags.Text = lookup.OtherFlags;
                tbDatData_Publisher.Text = lookup.Publisher;
                tbDatData_Year.Text = lookup.Year;

                // get other roms
                Roms = DAT_Rom.GetRoms(lookup.MD5);

                var staticItem = cmbRomChooser.Items[0];
                cmbRomChooser.Items.Clear();
                cmbRomChooser.Items.Add(staticItem);

                for (int i = 0; i < Roms.Count(); i++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = Roms[i].romName;
                    cmbRomChooser.Items.Add(cbi);
                }
                cmbRomChooser.SelectedIndex = 0;
            }
        }

        private void LookupROM()
        {
            tbRomData_Country.Text = tbIntRegion.Text;
            tbRomData_Developer.Text = tbIntPublisher.Text;
            tbRomData_gameName.Text = tbIntGame.Text;
            tbRomData_OtherFlags.Text = tbIntVersion.Text;
            tbRomData_Publisher.Text = tbIntPublisher.Text;
            tbRomData_Year.Text = tbIntYear.Text;
        }

        private void PopulateLibraryData()
        {
            tbLibData_gameId.Text = GameObj.gameId.ToString();

            if (GameObj.ManualEditSet == true)
            {
                tbLibData_gameName.Text = GameObj.gameNameEdited;
            }
            else
            {
                if (GameObj.gameNameFromDAT != null && GameObj.gameNameFromDAT != "")
                {
                    tbLibData_gameName.Text = GameObj.gameNameFromDAT;
                }
                else
                {
                    tbLibData_gameName.Text = GameObj.gameName;
                }
            }
            


            tbLibData_Copyright.Text = GameObj.Copyright;
            tbLibData_Country.Text = GameObj.Country;
            tbLibData_Developer.Text = GameObj.Developer;
            tbLibData_Language.Text = GameObj.Language;
            tbLibData_OtherFlags.Text = GameObj.OtherFlags;
            tbLibData_Publisher.Text = GameObj.Publisher;
            tbLibData_Year.Text = GameObj.Year;
            tbLibData_gdbId.Text = GameObj.gdbId.ToString();
            tbLibData_isFavorite.IsChecked = GameObj.isFavorite;
            tbLibData_AlternateTitles.Text = GameObj.AlternateTitles;
            tbLibData_Genres.Text = GameObj.Genres;
            tbLibData_Coop.Text = GameObj.Coop;
            tbLibData_ESRB.Text = GameObj.ESRB;
            tbLibData_Players.Text = GameObj.Players;
            tbLibData_Overview.Text = GameObj.Overview;
                 
        }

        private void EnableControls()
        {
            tbLibData_gameName.IsReadOnly = false;
            tbLibData_Copyright.IsReadOnly = false;
            tbLibData_Country.IsReadOnly = false;
            tbLibData_Developer.IsReadOnly = false;
            tbLibData_Language.IsReadOnly = false;
            tbLibData_OtherFlags.IsReadOnly = false;
            tbLibData_Publisher.IsReadOnly = false;
            tbLibData_Year.IsReadOnly = false;
            tbLibData_AlternateTitles.IsReadOnly = false;
            tbLibData_Coop.IsReadOnly = false;
            tbLibData_ESRB.IsReadOnly = false;
            tbLibData_Genres.IsReadOnly = false;
            tbLibData_Overview.IsReadOnly = false;
            

            btnDatCopyData_Copyright.Visibility = Visibility.Visible;
            btnDatCopyData_Country.Visibility = Visibility.Visible;
            btnDatCopyData_Developer.Visibility = Visibility.Visible;
            btnDatCopyData_DevelopmentStatus.Visibility = Visibility.Visible;
            btnDatCopyData_gameName.Visibility = Visibility.Visible;
            btnDatCopyData_Language.Visibility = Visibility.Visible;
            btnDatCopyData_OtherFlags.Visibility = Visibility.Visible;
            btnDatCopyData_Publisher.Visibility = Visibility.Visible;
            btnDatCopyData_Year.Visibility = Visibility.Visible;

            btnScrapeCopyData_Copyright.Visibility = Visibility.Visible;
            btnScrapeCopyData_Country.Visibility = Visibility.Visible;
            btnScrapeCopyData_Developer.Visibility = Visibility.Visible;
            btnScrapeCopyData_DevelopmentStatus.Visibility = Visibility.Visible;
            btnScrapeCopyData_gameName.Visibility = Visibility.Visible;
            btnScrapeCopyData_Language.Visibility = Visibility.Visible;
            btnScrapeCopyData_OtherFlags.Visibility = Visibility.Visible;
            btnScrapeCopyData_Publisher.Visibility = Visibility.Visible;
            btnScrapeCopyData_Year.Visibility = Visibility.Visible;
            btnScrapeCopyData_AlternateTitles.Visibility = Visibility.Visible;
            btnScrapeCopyData_Coop.Visibility = Visibility.Visible;
            btnScrapeCopyData_ESRB.Visibility = Visibility.Visible;
            btnScrapeCopyData_Genres.Visibility = Visibility.Visible;
            btnScrapeCopyData_Overview.Visibility = Visibility.Visible;
            btnScrapeCopyData_Players.Visibility = Visibility.Visible;
            btnScrapeCopyData_gdbId.Visibility = Visibility.Visible;

            //btnRomCopyData_Copyright.Visibility = Visibility.Visible;
            btnRomCopyData_Country.Visibility = Visibility.Visible;
            btnRomCopyData_Developer.Visibility = Visibility.Visible;
            //btnRomCopyData_DevelopmentStatus.Visibility = Visibility.Visible;
            btnRomCopyData_gameName.Visibility = Visibility.Visible;
            btnRomCopyData_Language.Visibility = Visibility.Visible;
            btnRomCopyData_OtherFlags.Visibility = Visibility.Visible;
            btnRomCopyData_Publisher.Visibility = Visibility.Visible;
            btnRomCopyData_Year.Visibility = Visibility.Visible;
        }

        private void DisableControls()
        {
            tbLibData_gameName.IsReadOnly = true;
            tbLibData_Copyright.IsReadOnly = true;
            tbLibData_Country.IsReadOnly = true;
            tbLibData_Developer.IsReadOnly = true;
            tbLibData_Language.IsReadOnly = true;
            tbLibData_OtherFlags.IsReadOnly = true;
            tbLibData_Publisher.IsReadOnly = true;
            tbLibData_Year.IsReadOnly = true;
            tbLibData_AlternateTitles.IsReadOnly = true;
            tbLibData_Coop.IsReadOnly = true;
            tbLibData_ESRB.IsReadOnly = true;
            tbLibData_Genres.IsReadOnly = true;
            tbLibData_Overview.IsReadOnly = true;

            btnDatCopyData_Copyright.Visibility = Visibility.Collapsed;
            btnDatCopyData_Country.Visibility = Visibility.Collapsed;
            btnDatCopyData_Developer.Visibility = Visibility.Collapsed;
            btnDatCopyData_DevelopmentStatus.Visibility = Visibility.Collapsed;
            btnDatCopyData_gameName.Visibility = Visibility.Collapsed;
            btnDatCopyData_Language.Visibility = Visibility.Collapsed;
            btnDatCopyData_OtherFlags.Visibility = Visibility.Collapsed;
            btnDatCopyData_Publisher.Visibility = Visibility.Collapsed;
            btnDatCopyData_Year.Visibility = Visibility.Collapsed;

            btnScrapeCopyData_Copyright.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Country.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Developer.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_DevelopmentStatus.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_gameName.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Language.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_OtherFlags.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Publisher.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Year.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_AlternateTitles.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Coop.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_ESRB.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Genres.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Overview.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_Players.Visibility = Visibility.Collapsed;
            btnScrapeCopyData_gdbId.Visibility = Visibility.Collapsed;

            //btnRomCopyData_Copyright.Visibility = Visibility.Collapsed;
            btnRomCopyData_Country.Visibility = Visibility.Collapsed;
            btnRomCopyData_Developer.Visibility = Visibility.Collapsed;
            //btnRomCopyData_DevelopmentStatus.Visibility = Visibility.Collapsed;
            btnRomCopyData_gameName.Visibility = Visibility.Collapsed;
            btnRomCopyData_Language.Visibility = Visibility.Collapsed;
            btnRomCopyData_OtherFlags.Visibility = Visibility.Collapsed;
            btnRomCopyData_Publisher.Visibility = Visibility.Collapsed;
            btnRomCopyData_Year.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// cancel and save no changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseSec_OnClick(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        /// <summary>
        /// save any changes made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSec_OnClick(object sender, RoutedEventArgs e)
        {
            // togglebuttons
            GameObj.ManualEditSet = btnToggleManualEdit.IsChecked.Value;
            GameObj.isFavorite = tbLibData_isFavorite.IsChecked.Value;

            // set data from all textboxes
            GameObj.gameNameEdited = tbLibData_gameName.Text;
            GameObj.Year = tbLibData_Year.Text;
            GameObj.Developer = tbLibData_Developer.Text;
            GameObj.Publisher = tbLibData_Publisher.Text;
            GameObj.Country = tbLibData_Country.Text;
            GameObj.Language = tbLibData_Language.Text;
            GameObj.Copyright = tbLibData_Copyright.Text;
            GameObj.DevelopmentStatus = tbLibData_DevelopmentStatus.Text;
            GameObj.OtherFlags = tbLibData_OtherFlags.Text;
            GameObj.AlternateTitles = tbLibData_AlternateTitles.Text;
            GameObj.Genres = tbLibData_Genres.Text;
            GameObj.Coop = tbLibData_Coop.Text;
            GameObj.ESRB = tbLibData_ESRB.Text;
            GameObj.Players = tbLibData_Players.Text;
            GameObj.Overview = tbLibData_Overview.Text;

            string gdi = tbLibData_gdbId.Text;
            if (gdi != "" && gdi != "0")
            {
                GameObj.gdbId = Convert.ToInt32(gdi);
            }
            else
            {
                GameObj.gdbId = null;
            }

            // save game data
            Game.SaveToDatabase(new List<Game> { GameObj });
            
            // close childwindow
            this.Close();
        }
        
        private void btnToggleManualEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GameObj.ManualEditSet == true)
            {
                // click disables
                //Game.SetManualEdit(GameObj.gameId);
                GameObj.ManualEditSet = false;
                DisableControls();
            }
            else
            {
                // click enables
                //Game.UnSetManualEdit(GameObj.gameId);
                GameObj.ManualEditSet = true;
                EnableControls();
            }
        }

        private void btnRescanDat_Click(object sender, RoutedEventArgs e)
        {
            // get selected combobox item
            int index = cmbRomChooser.SelectedIndex;

            if (index == 0)
            {
                LookupDAT();
                return;
            }

            var rom = Roms[index - 1];

            tbDatData_Copyright.Text = rom.copyright;
            tbDatData_Country.Text = rom.country;
            tbDatData_Developer.Text = rom.developer;
            tbDatData_DevelopmentStatus.Text = rom.developmentStatus;
            tbDatData_gameName.Text = rom.name;
            tbDatData_Language.Text = rom.language;
            tbDatData_OtherFlags.Text = rom.otherFlags;
            tbDatData_Publisher.Text = rom.publisher;
            tbDatData_Year.Text = rom.year;

            // LookupDAT();
        }

        private void btnScrapeCopyData_Click(object sender, RoutedEventArgs e)
        {
            if (GameObj.ManualEditSet == true)
            {
                Button b = sender as Button;

                string libName = "tbLibData_";
                string datName = "tbScrapeData_";

                string n = b.Name.Split('_')[1];

                TextBox tb = (TextBox)this.FindName(libName + n);
                TextBox tbs = (TextBox)this.FindName(datName + n);

                tb.Text = tbs.Text;
                
            }
        }

        private void btnDatCopyData_Click(object sender, RoutedEventArgs e)
        {
            if (GameObj.ManualEditSet == true)
            {
                Button b = sender as Button;

                string libName = "tbLibData_";
                string datName = "tbDatData_";

                string n = b.Name.Split('_')[1];

                TextBox tb = (TextBox)this.FindName(libName + n);
                TextBox tbs = (TextBox)this.FindName(datName + n);

                tb.Text = tbs.Text;

                
            }
        }

        private void btnRomCopyData_Click(object sender, RoutedEventArgs e)
        {
            if (GameObj.ManualEditSet == true)
            {
                Button b = sender as Button;

                string libName = "tbLibData_";
                string datName = "tbRomData_";

                string n = b.Name.Split('_')[1];

                TextBox tb = (TextBox)this.FindName(libName + n);
                TextBox tbs = (TextBox)this.FindName(datName + n);

                tb.Text = tbs.Text;


            }
        }

        private void btnLookupScrapeDB_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            ScraperLookup.PickLocalGameInspector(GameObj.gameId, mw, this);

        }

        
        
    }
    
}
