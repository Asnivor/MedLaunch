using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MedLaunch.Common.IO.Compression;
using System.IO;
using System.Xml;

namespace MedLaunch._Debug.skeletonKey
{
    public class AdminSkeletonKey
    {
        public List<SK_System> systems { get; set; }
        public MainWindow mw { get; set; }

        public AdminSkeletonKey()
        {
            systems = SK_System.GetSystems();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        /// <summary>
        /// main entry point for DAT importing
        /// </summary>
        public async void EntryPoint()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Import",
                AnimateShow = false,
                AnimateHide = false
            };

            string output = "Scanning local SkeletonKey archive files - ";

            var controller = await mw.ShowProgressAsync("SkeletonKey Importer", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            // start import
            await Task.Run(() =>
            {
                // setup working list object
                List<SK_Game> roms = new List<SK_Game>();

                // get all datfile archives
                var archives = System.IO.Directory.GetFiles(@"..\..\_Debug\skeletonKey\DATFiles");

                string outputDir = AppDomain.CurrentDomain.BaseDirectory + @"_xmls";
                Directory.CreateDirectory(outputDir);

                // iterate through each DAT
                foreach (var arch in archives)
                {
                    int count = 0;
                    Archive a = new Archive(arch);
                    string[] allowed = { ".xml" };
                    var crs = a.ProcessArchive(allowed);

                    // determine system
                    string system = Path.GetFileNameWithoutExtension(crs.ArchivePath);
                    controller.SetMessage(output + system + "\n\n");
                    int sysId = GetSystemID(system);
                    if (sysId == 0)
                        continue;

                    foreach (var c in crs.Results)
                    {
                        string extractedPath = outputDir + @"\" + c.FileName;
                        Archive.ExtractFile(crs.ArchivePath, c.InternalPath, outputDir);

                        // process xml file
                        count++;
                        controller.SetMessage(output + system + "\n\nProcessing: " + count);
                        var data = ProcessXML(extractedPath);
                        data.pid = sysId;

                        // delete xml file
                        try
                        {
                            File.Delete(extractedPath);
                        }
                        catch (Exception ex)
                        {
                            // do nothing
                            string e = ex.ToString();
                        }

                    }  
                }                
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("SK Builder", "Import Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("SK Builder", "Import Completed\n\n" + output);
            }

        }

        private SK_Game ProcessXML(string xmlPath)
        {
            SK_Game sk = new SK_Game();

            string xmlStr = File.ReadAllText(xmlPath);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            return sk;
        }

        private int GetSystemID(string archiveName)
        {
            switch (archiveName)
            {
                case "Atari - Lynx":
                    return 4923;
                case "Bandai - WonderSwan Color":
                    return 4926;
                case "Bandai - WonderSwan":
                    return 4925;
                case "NEC - PC Engine - TurboGrafx 16":
                case "NEC - PC Engine SuperGrafx":
                    return 34;
                case "NEC - PC Engine CD - TurboGrafx CD":
                    return 4955;
                case "NEC - PC-FX":
                    return 4930;
                case "Nintendo - Famicom":
                    return 4936;
                case "Nintendo - Game Boy Advance":
                    return 5;
                case "Nintendo - Game Boy Color":
                    return 42;
                case "Nintendo - Game Boy":
                    return 4;
                case "Nintendo - Nintendo Entertainment System":
                    return 7;
                case "Nintendo - Super Nintendo Entertainment System":
                    return 6;
                case "Nintendo - Virtual Boy":
                    return 4918;
                case "Sega - Game Gear":
                    return 20;
                case "Sega - Master System - Mark III":
                    return 20;
                case "Sega - Mega Drive - Genesis":
                    return 18;
                case "Sega - Saturn":
                    return 17;
                case "SNK - Neo Geo Pocket Color":
                    return 4923;
                case "SNK - Neo Geo Pocket":
                    return 4922;
                case "Sony - Playstation":
                    return 10;
                default:
                    return 0;
            }
        }
    }
    
}
