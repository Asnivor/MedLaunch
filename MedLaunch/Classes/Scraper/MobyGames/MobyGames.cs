using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Asnitech.Launch.Common;
using System.Net;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System.IO;

namespace MedLaunch.Classes.MobyGames
{
    public class MobyGames
    {
        /*

        public static void DumpPlatformGamesToDisk()
        {
            // get all platform games
            List<MobyPlatformGame> games = MobyPlatformGame.GetGames();
            // set file path
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Data\System\MobyGames.json";
            //  dump file
            string json = JsonConvert.SerializeObject(games, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // main starting point for scraping all moby platformgames (basic list)
        public async static void ScrapeAllPlatformGames()
        {
            // get the main window
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // start progress dialog controller
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scraping",
                AnimateShow = false,
                AnimateHide = false
            };
            var controller = await mw.ShowProgressAsync("Scraping MobyGames Data", "Initialising...", true, settings: mySettings);
            controller.SetCancelable(true);
            await Task.Delay(100);

            List<MobyPlatformGame> games = new List<MobyPlatformGame>();

            await Task.Run(() =>
            {
                 games = ScrapeAllPlatformGames(controller);
            });
            string message = "";
            if (games == null || games.Count == 0)
            {
                // nothing returned
                message = "No games were scraped (possibly some kind of error occured)";
            }
            else
            {
                // save to json file
                controller.SetMessage("Saving to file...");
                // set file path
                string filePath = @"..\..\Data\System\MobyGames.json";
                //  dump file
                string json = JsonConvert.SerializeObject(games, Formatting.Indented);
                File.WriteAllText(filePath, json);

                //int[] counts = MobyPlatformGame.SaveToDatabase(games);
                message = "Scraping Completed - JSON data saved.";
                    //message = "Scraping Completed\n" + counts[0] + " new games added\n" + counts[1] + " existing games updated";

                // export to file
                controller.SetMessage("Exporting to flat file...");
            }

            

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("MobyGames Scraper", "Scraping Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("MobyGames Scraper", message);
            }



        }
        */
        
    }
}
