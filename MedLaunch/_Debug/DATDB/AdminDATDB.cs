using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch._Debug.DATDB
{
    public class AdminDATDB
    {
        public MainWindow mw { get; set; }
        public List<DAT_System> systems { get; set; }

        public AdminDATDB()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            systems = DAT_System.GetSystems();
        }

        /// <summary>
        /// Main entry point to import DAT files
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="platformId"></param>
        public async void ImportRoutine(ProviderType providerType, int platformId)
        {
            string providerName = "All Platforms";
            if (platformId > 0)
                providerName = DAT_Provider.GetProviders().Where(a => a.datProviderId == platformId).FirstOrDefault().providerName;

            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Import",
                AnimateShow = false,
                AnimateHide = false
            };

            string output = "Scanning local DAT files for " + providerName + "\n";

            var controller = await mw.ShowProgressAsync("DAT Importer", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            // start import
            await Task.Run(() =>
            {
                // setup working list object
                List<DAT_Rom> roms = new List<DAT_Rom>();

                // populate roms based on provider
                switch (providerType)
                {
                    case ProviderType.NoIntro:                        
                        Platforms.NOINTRO.Models.NoIntroCollection nointro = new Platforms.NOINTRO.Models.NoIntroCollection(platformId);
                        roms = nointro.Data;
                        break;

                    case ProviderType.ToSec:
                        break;

                    case ProviderType.ReDump:
                        break;
                }

                controller.SetMessage(output + roms.Count + " Separate ROM files scraped. Starting database import procedure");

                int[] result = DAT_Rom.SaveToDatabase(roms);
                output = "ROMs Added: " + result[0] + "\nROMs Updated/Skipped: " + result[1];
            });

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("DAT Builder", "Import Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("DAT Builder", "Import Completed\n\n" + output);
            }

        }
    }

    public enum ProviderType
    {
        NoIntro,
        ToSec,
        ReDump,
    }
}
