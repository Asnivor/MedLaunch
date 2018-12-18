using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MedLaunch.Common;
using System.IO;
using System.Windows;
using MedLaunch.Classes;

namespace MedLaunch.Models
{
    public class Paths
    {
        public int pathId { get; set; }
        public string mednafenExe { get; set; }                     // Path to the Mednafen EXE
        public string systemGb { get; set; }                        // Gameboy Color
        public string systemGba { get; set; }                       // Gamboy Advance
        public string systemLynx { get; set; }                      // Atari Lynx
        public string systemMd { get; set; }                        // Sega Genesis/MegaDrive
        public string systemGg { get; set; }                        // Sega Game Gear
        public string systemNgp { get; set; }                       // NeoGeo Pocket Color
        public string systemPce { get; set; }                       // PC Engine /TurboGrafx 16 /SuperGrafx
        public string systemPceCd { get; set; }                     // PC Engine (CD)/TurboGrafx 16 (CD)
        public string systemPcfx { get; set; }                      // PC-FX
        public string systemPsx { get; set; }                       // Sony PlayStation 
        public string systemSms { get; set; }                       // Sega Master System
        public string systemNes { get; set; }                       // Nintendo Entertainment System/Famicon
        public string systemSnes { get; set; }                      // Super Nintendo Entertainment System/Super Famicom
        public string systemSs { get; set; }                        // Sega Saturn
        public string systemVb { get; set; }                        // Virtual Boy
        public string systemWswan { get; set; }                     // WonderSwan
		public string systemApple2 { get; set; }					// Apple II

        public static string GetSystemPath(int systemId)
        {
            var paths = Paths.GetPaths();
            switch (systemId)
            {
                case 1:
                    return paths.systemGb;
                case 2:
                    return paths.systemGba;
                case 3:
                    return paths.systemLynx;
                case 4:
                    return paths.systemMd;
                case 5:
                    return paths.systemGg;
                case 6:
                    return paths.systemNgp;
                case 7:
                case 17:
                    return paths.systemPce;
                case 8:
                    return paths.systemPcfx;
                case 9:
                    return paths.systemPsx;
                case 10:
                    return paths.systemSms;
                case 11:
                    return paths.systemNes;
                case 16:
                case 12:                
                    return paths.systemSnes;
                case 13:
                    return paths.systemSs;
                case 14:
                    return paths.systemVb;
                case 15:
                    return paths.systemWswan;
                case 18:
                    return paths.systemPceCd;
				case 19:
					return paths.systemApple2;
                default:
                    return null;
            }
        }

        public static void SaveToDatabase(List<Paths> Configs)
        {
            using (var db = new MyDbContext())
            {
                // get current database context
                var current = db.Paths.AsNoTracking().ToList();

                List<Paths> toAdd = new List<Paths>();
                List<Paths> toUpdate = new List<Paths>();

                // iterate through the games list and separete out games to be added and games to be updated
                foreach (var g in Configs)
                {
                    Paths t = (from a in current
                                        where a.pathId == g.pathId
                                        select a).SingleOrDefault();
                    if (t == null) { toAdd.Add(g); }
                    else { toUpdate.Add(g); }
                }
                db.Paths.UpdateRange(toUpdate);
                db.Paths.AddRange(toAdd);
                db.SaveChanges();
            }
        }

        // return Paths entry from DB
        public static Paths GetPaths()
        {
            Paths paths = new Paths();
            using (var context = new MyDbContext())
            {
                var query = from s in context.Paths
                            where s.pathId == 1
                            select s;
                paths = query.FirstOrDefault();
            }
            return paths;
        }

        // write paths object to DB
        public static void SetPaths(Paths paths)
        {
            using (var context = new MyDbContext())
            {
                context.Paths.Attach(paths);
                var entry = context.Entry(paths);
                entry.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        // Populate path forms with DB data
        public static void LoadPathSettings(TextBox tbPathMednafen, TextBox tbPathGb, TextBox tbPathGba, TextBox tbPathGg, TextBox tbPathLynx, TextBox tbPathMd,
            TextBox tbPathNes, TextBox tbPathSnes, TextBox tbPathNgp, TextBox tbPathPce, TextBox tbPathPcfx, TextBox tbPathSms,
            TextBox tbPathVb, TextBox tbPathWswan, TextBox tbPathPsx, TextBox tbPathSs, TextBox tbPathPceCd, TextBox tbPathApple2)
        {
            Paths paths = GetPaths();

            tbPathMednafen.Text = paths.mednafenExe;
            tbPathGb.Text = paths.systemGb;
            tbPathGba.Text = paths.systemGba;
            tbPathGg.Text = paths.systemGg;
            tbPathLynx.Text = paths.systemLynx;
            tbPathMd.Text = paths.systemMd;
            tbPathSms.Text = paths.systemSms;
            tbPathNes.Text = paths.systemNes;
            tbPathNgp.Text = paths.systemNgp;
            tbPathPce.Text = paths.systemPce;
            tbPathPcfx.Text = paths.systemPcfx;
            tbPathPsx.Text = paths.systemPsx;
            tbPathSnes.Text = paths.systemSnes;
            tbPathSs.Text = paths.systemSs;
            tbPathVb.Text = paths.systemVb;
            tbPathWswan.Text = paths.systemWswan;
            tbPathPceCd.Text = paths.systemPceCd;
			tbPathApple2.Text = paths.systemApple2;
        }

        // save path settings from form
        public static void SavePathSettings(TextBox tbPathMednafen, TextBox tbPathGb, TextBox tbPathGba, TextBox tbPathGg, TextBox tbPathLynx, TextBox tbPathMd,
            TextBox tbPathNes, TextBox tbPathSnes, TextBox tbPathNgp, TextBox tbPathPce, TextBox tbPathPcfx, TextBox tbPathSms,
            TextBox tbPathVb, TextBox tbPathWswan, TextBox tbPathPsx, TextBox tbPathSs, TextBox tbPathPceCd, TextBox tbPathApple2)
        {
            Paths paths = GetPaths();
            paths.mednafenExe = tbPathMednafen.Text;
            paths.systemGb = tbPathGb.Text;
            paths.systemGba = tbPathGba.Text;
            paths.systemGg = tbPathGg.Text;
            paths.systemLynx = tbPathLynx.Text;
            paths.systemMd = tbPathMd.Text;
            paths.systemSms = tbPathSms.Text;
            paths.systemNes = tbPathNes.Text;
            paths.systemNgp = tbPathNgp.Text;
            paths.systemPce = tbPathPce.Text;
            paths.systemPcfx = tbPathPcfx.Text;
            paths.systemPsx = tbPathPsx.Text;
            paths.systemSnes = tbPathSnes.Text;
            paths.systemSs = tbPathSs.Text;
            paths.systemVb = tbPathVb.Text;
            paths.systemWswan = tbPathWswan.Text;
            paths.systemPceCd = tbPathPceCd.Text;
			paths.systemApple2 = tbPathApple2.Text;

            SetPaths(paths);
        }

        public static void SaveMednafenPath(string path)
        {
            Paths paths = Paths.GetPaths();
            paths.mednafenExe = path;
            SaveToDatabase(new List<Paths> { paths });
        }

        public static bool isMednafenPathValid()
        {
            bool pathWorking = false;

            using (var context = new MyDbContext())
            {
                // get mednafen folder path from DB
                string medPath = (from p in context.Paths
                                  where p.pathId == 1
                                  select p.mednafenExe).SingleOrDefault();

                if (medPath == "" || medPath == null)
                {
                    // path is not set in database
                    return pathWorking;
                }
                else
                {
                    // path is present - check that 'mednafen.exe' is present in this directory
                    string pathToMedExe = medPath + @"\mednafen.exe";
                    if (File.Exists(pathToMedExe)) { pathWorking = true; }
                    
                }
            }
            return pathWorking;
        }

        /// <summary>
        /// Cause mednafen to run with an incorrect rom string in order that it generates all of its files
        /// </summary>
        public static void InitMednafen()
        {
            // get mednafen path from database
            string medFolderPath = Paths.GetPaths().mednafenExe;
            string medConfigFile = medFolderPath + @"\mednafen-09x.cfg";
            string medConfigFileNew = medFolderPath + @"\mednafen.cfg";

            // check for existence of config file (if it is not there, mednafen needs initialising)
            if (!File.Exists(medConfigFile) && !File.Exists(medConfigFileNew))
            {
                LogParser.Instance.ParseData();

                /*

                System.Diagnostics.Process mProc = new System.Diagnostics.Process();
                mProc.StartInfo.UseShellExecute = true;
                mProc.StartInfo.RedirectStandardOutput = false;
                mProc.StartInfo.FileName = medFolderPath + @"\mednafen.exe";
                mProc.StartInfo.CreateNoWindow = true;
                mProc.StartInfo.Arguments = "init";
                mProc.Start();
                mProc.WaitForExit();

    */
            }
        }

        public static void SetMednafenPath(Button btnPathMednafen)
        {
            MessagePopper.ShowMessageDialog("Click OK to browse to either an existing Mednafen directory, or a new directory in order to download the latest compatible Mednafen version",
                "Invalid Mednafen.exe path!");
            //MessageBox.Show("Click OK to browse to either an existing Mednafen directory, or a new directory in order to download the latest compatible Mednafen version", "Invalid Mednafen.exe path!");
            btnPathMednafen.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public static bool DownloadCheck()
        {
            var result = MessagePopper.ShowMessageDialog("The selected folder (" + Paths.GetPaths().mednafenExe + ")\nDoes not contain a mednafen executable.\n\nClick YES: Download and extract the latest Mednafen version to this folder\nClick NO: Choose another folder",
                "Mednafen NOT Detected", MessagePopper.DialogButtonOptions.YESNO);

            //MessageBoxResult result = MessageBox.Show("The selected folder (" + Paths.GetPaths().mednafenExe + ")\nDoes not contain a mednafen executable.\n\nClick YES: Download and extract the latest Mednafen version to this folder\nClick NO: Choose another folder", "Mednafen NOT Detected", MessageBoxButton.YesNo);
            if (result == MessagePopper.ReturnResult.Affirmative)
            {
                // download mednafen to this folder
                return true;
            }
            else
            {
                // choose another folder
                return false;
            }
        }

        public static void MedPathRoutineContinued(Button btnPathMednafen, TextBox tbPathMednafen)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            bool pathWorking = Paths.isMednafenPathValid();
            if (pathWorking == false)
            {
                // problem with mednafen path
                if (DownloadCheck() == false)
                {
                    SetMednafenPath(btnPathMednafen);
                    MedPathRoutineContinued(btnPathMednafen, tbPathMednafen);
                }
                else
                {
                    mw.DownloadMednafenNoAsync();
                    MedPathRoutineContinued(btnPathMednafen, tbPathMednafen);
                }
            }
            else
            {
                // path is valid and working
                InitM();
            }
        }

        public static void InitM()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            GlobalSettings gs = GlobalSettings.GetGlobals();
            
            InitMednafen();

            // check whether bypassconfig is set
            if (gs.bypassConfig == false)
            {
                //ask to import configs

                var result = MessagePopper.ShowMessageDialog("Do you want to import data from any Mednafen config files in this directory?\n(This will overwrite any config data stored in MedLaunch)\n\nYou will only be prompted once for this, but you can control automatic import of mednafen config files from the SETTINGS tab.",
                "Config Import", MessagePopper.DialogButtonOptions.YESNO);

                //MessageBoxResult result = MessageBox.Show("Do you want to import data from any Mednafen config files in this directory?\n(This will overwrite any config data stored in MedLaunch)\n\nYou will only be prompted once for this, but you can control automatic import of mednafen config files from the SETTINGS tab.", "Config Import", MessageBoxButton.YesNo);
                if (result == MessagePopper.ReturnResult.Affirmative)
                {
                    ConfigImport ci = new ConfigImport();
                    ci.ImportAll(null);

                    // set bypassconfig to 1
                    gs.bypassConfig = true;
                    GlobalSettings.SetGlobals(gs);
                }

                else
                {
                    gs.bypassConfig = true;
                    GlobalSettings.SetGlobals(gs);
                }
            }
            
            // if option is selected make a backup of the mednafen config file
            BackupConfig.BackupMain();
            // mednafen versions
            mw.UpdateCheckMednafen();
        }

        public static void MedPathRoutine(Button btnPathMednafen, TextBox tbPathMednafen)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            bool pathWorking = Paths.isMednafenPathValid();
            if (pathWorking == false)
            {
                SetMednafenPath(btnPathMednafen);
                using (var context = new MyDbContext())
                {
                    // set new path to database
                    Paths path = (from p in context.Paths
                                  where p.pathId == 1
                                  select p).SingleOrDefault();
                    path.mednafenExe = tbPathMednafen.Text;
                    context.SaveChanges();

                    //System.Threading.Thread.Sleep(1500);

                    MedPathRoutineContinued(btnPathMednafen, tbPathMednafen);

                    
                }
            }
            else
            {
                // path is valid and working
                InitM();
            }


            
        }

    }


}
