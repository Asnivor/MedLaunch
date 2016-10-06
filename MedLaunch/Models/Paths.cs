using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Asnitech.Launch.Common;
using System.IO;
using System.Windows;

namespace MedLaunch.Models
{
    public class Paths
    {
        public int pathId { get; set; }
        public string mednafenExe { get; set; }                     // Path to the Mednafen EXE

        //public string systemCdplay { get; set; }                    // CD-DA Player
        public string systemGb { get; set; }                        // Gameboy Color
        public string systemGba { get; set; }                       // Gamboy Advance
        public string systemLynx { get; set; }                      // Atari Lynx
        public string systemMd { get; set; }                        // Sega Genesis/MegaDrive
        public string systemGg { get; set; }                        // Sega Game Gear
        public string systemNgp { get; set; }                       // NeoGeo Pocket Color
        public string systemPce { get; set; }                       // PC Engine (CD)/TurboGrafx 16 (CD)/SuperGrafx
        public string systemPcfx { get; set; }                      // PC-FX
        public string systemPsx { get; set; }                       // Sony PlayStation 
        public string systemSms { get; set; }                       // Sega Master System
        public string systemNes { get; set; }                       // Nintendo Entertainment System/Famicon
        public string systemSnes { get; set; }                      // Super Nintendo Entertainment System/Super Famicom
        public string systemSs { get; set; }                        // Sega Saturn
        //public string systemSsfplay { get; set; }                   // Sega Saturn Sound Format Player
        public string systemVb { get; set; }                        // Virtual Boy
        public string systemWswan { get; set; }                     // WonderSwan


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
            TextBox tbPathNes, TextBox tbPathSnes, TextBox tbPathNgp, TextBox tbPathPce, TextBox tbPathPcfx, TextBox tbPathMs,
            TextBox tbPathVb, TextBox tbPathWswan) // TextBox tbPathPsx, TextBox tbPathSs)
        {
            Paths paths = GetPaths();

            tbPathMednafen.Text = paths.mednafenExe;
            tbPathGb.Text = paths.systemGb;
            tbPathGba.Text = paths.systemGba;
            tbPathGg.Text = paths.systemGg;
            tbPathLynx.Text = paths.systemLynx;
            tbPathMd.Text = paths.systemMd;
            tbPathMs.Text = paths.systemSms;
            tbPathNes.Text = paths.systemNes;
            tbPathNgp.Text = paths.systemNgp;
            tbPathPce.Text = paths.systemPce;
            tbPathPcfx.Text = paths.systemPcfx;
            //tbPathPsx.Text = paths.systemPsx;
            tbPathSnes.Text = paths.systemSnes;
            //tbPathSs.Text = paths.systemSs;
            tbPathVb.Text = paths.systemVb;
            tbPathWswan.Text = paths.systemWswan;
        }

        // save path settings from form
        public static void SavePathSettings(TextBox tbPathMednafen, TextBox tbPathGb, TextBox tbPathGba, TextBox tbPathGg, TextBox tbPathLynx, TextBox tbPathMd,
            TextBox tbPathNes, TextBox tbPathSnes, TextBox tbPathNgp, TextBox tbPathPce, TextBox tbPathPcfx, TextBox tbPathMs,
            TextBox tbPathVb, TextBox tbPathWswan) // TextBox tbPathPsx, TextBox tbPathSs)
        {
            Paths paths = GetPaths();
            paths.mednafenExe = tbPathMednafen.Text;
            paths.systemGb = tbPathGb.Text;
            paths.systemGba = tbPathGba.Text;
            paths.systemGg = tbPathGg.Text;
            paths.systemLynx = tbPathLynx.Text;
            paths.systemMd = tbPathMd.Text;
            paths.systemSms = tbPathMs.Text;
            paths.systemNes = tbPathNes.Text;
            paths.systemNgp = tbPathNgp.Text;
            paths.systemPce = tbPathPce.Text;
            paths.systemPcfx = tbPathPcfx.Text;
            //paths.systemPsx = tbPathPsx.Text;
            paths.systemSnes = tbPathSnes.Text;
            //paths.systemSs = tbPathSs.Text;
            paths.systemVb = tbPathVb.Text;
            paths.systemWswan = tbPathWswan.Text;

            SetPaths(paths);
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

        public static void SetMednafenPath(Button btnPathMednafen)
        {
            MessageBox.Show("Click OK to browse to your Mednafen directory", "Invalid Mednafen.exe path!");
            btnPathMednafen.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public static void MedPathRoutine(Button btnPathMednafen, TextBox tbPathMednafen)
        {
            bool pathWorking = Paths.isMednafenPathValid();
            if (pathWorking == false)
            {
                // problem with mednafen path - force user to set it
                SetMednafenPath(btnPathMednafen);
                using (var context = new MyDbContext())
                {
                    // set new path to database
                    Paths path = (from p in context.Paths
                                  where p.pathId == 1
                                  select p).SingleOrDefault();
                    path.mednafenExe = tbPathMednafen.Text;
                    context.SaveChanges();
                }

                // check path again
                if (Paths.isMednafenPathValid() == false)
                {
                    // rinse and repeat
                    SetMednafenPath(btnPathMednafen);
                }
                else
                {
                    // path is valid - break out
                    return;
                }
            }
            else
            {
                return;
            }
        }

    }


}
