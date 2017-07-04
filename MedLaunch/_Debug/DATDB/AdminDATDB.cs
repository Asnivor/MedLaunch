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
                        Platforms.TOSEC.Models.ToSecCollection tosec = new Platforms.TOSEC.Models.ToSecCollection(platformId);
                        roms = tosec.Data;
                        break;

                    case ProviderType.ReDump:
                        break;

                    case ProviderType.PsxDataCenter:
                        Platforms.PSXDATACENTER.Models.PsxDataCenterCollection psxdc = new Platforms.PSXDATACENTER.Models.PsxDataCenterCollection();
                        roms = psxdc.Data;
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

        /// <summary>
        /// Creates / updates the top-level games table (DAT_Game)
        /// matching rom names and updating the gid in DAT_Rom
        /// </summary>
        public async void ProcessTopLevelGames()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Procedure",
                AnimateShow = false,
                AnimateHide = false
            };

            int gamesCreated = 0;
            int gamesUpdated = 0;
            int romsUpdated = 0;

            string output = "Processing DAT_Game Table \n";

            var controller = await mw.ShowProgressAsync("Top-Level Game Builder", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            await Task.Run(() =>
            {
                List<DAT_Game> AllGames = DAT_Game.GetGames();

                // iterate through each platform (system) and do processing
                foreach (DAT_System sys in DAT_System.GetSystems())
                {
                    int gamesCreatedProc = 0;
                    int gamesUpdatedProc = 0;
                    int romsMatchedProc = 0;

                    // instantiate working lists
                    List<DAT_Game> gWorking = new List<DAT_Game>();
                    List<DAT_Rom> rWorking = new List<DAT_Rom>();

                    // get all roms wihtout a gameid set
                    controller.SetMessage(output + "Loading ROMs for " + sys.platformName);
                    List<DAT_Rom> roms = DAT_Rom.GetRomsWithNoGameId(sys.pid);

                    // get all games for this system
                    controller.SetMessage(output + "Loading Games for " + sys.platformName);
                    List<DAT_Game> games = AllGames.Where(a => a.pid == sys.pid).ToList();

                    // iterate through each ROM
                    foreach (var rom in roms)
                    {
                        controller.SetMessage(output + "Processing ROMs for " + sys.platformName +
                            "\n\n***In-Progress Stats***" +
                            "\nGames Created (processing): " + gamesCreatedProc + 
                            "\nGames Updated (processing): " + gamesUpdatedProc +
                            "\nROMs Matched (processing): " + romsMatchedProc +
                            "\n\n***Database Stats***" + 
                            "\nGames Created: " + gamesCreated +
                            "\nGames Updated: " + gamesUpdated +
                            "\nROMs Updated: " + romsUpdated
                            );

                        // check whether game already exists
                        var gCheck = (from a in games
                                     where FormatName(a.gameName) == FormatName(rom.name)
                                     select a).FirstOrDefault();
                        if (gCheck == null)
                        {
                            // no game found - create one
                            DAT_Game dg = new DAT_Game();
                            dg.gameName = rom.name;
                            dg.pid = sys.pid;                           
                            if (rom.year != null && rom.year.Trim() != "")
                                dg.year = rom.year;
                            if (rom.publisher != null && rom.publisher.Trim() != "")
                                dg.publisher = rom.publisher;
                            if (rom.developer != null && rom.developer.Trim() != "")
                                dg.developer = rom.developer;

                            // get the latest availble gid and set this
                            var newg = AllGames.ToList().OrderByDescending(a => a.gid).FirstOrDefault();
                            int gid = 1;
                            if (newg != null)
                            {
                                gid = (newg.gid + 1);
                            }
                            // set gid on the game object
                            dg.gid = gid;
                            // add to working game set
                            gWorking.Add(dg);
                            // since we have created a new game - add it to the games list
                            games.Add(dg);
                            AllGames.Add(dg);
                            gamesCreatedProc++;

                            // update the gid in the rom record and add to working set
                            DAT_Rom dr = rom;
                            rom.gid = gid;
                            rWorking.Add(dr);
                                                    
                        }
                        else
                        {
                            // game has been found - update the game (if neccessary) and update the ROM with the gameId
                            DAT_Game dg = gCheck;
                            bool updateNeeded = false;
                            if (rom.year != null && rom.year.Trim() != "")
                            {
                                dg.year = rom.year;
                                updateNeeded = true;
                            }
                                
                            if (rom.publisher != null && rom.publisher.Trim() != "")
                            {
                                updateNeeded = true;
                                dg.publisher = rom.publisher;
                            }

                            if (rom.developer != null && rom.developer.Trim() != "")
                            {
                                updateNeeded = true;
                                dg.developer = rom.developer;
                            }

                            // update game directly
                            if (updateNeeded == true)
                            {
                                gWorking.Add(dg);
                                gamesUpdatedProc++;
                            }

                            // update the gid in the rom record and add to working set
                            DAT_Rom dr = rom;
                            rom.gid = dg.gid;
                            rWorking.Add(dr);
                        }

                        romsMatchedProc++;

                    }

                    // process working list of games
                    controller.SetMessage(output + "Updating Games in Database for " + sys.platformName +
                        "\n\n***In-Progress Stats***" +
                            "\nGames Created (processing): " + gamesCreatedProc +
                            "\nGames Updated (processing): " + gamesUpdatedProc +
                            "\nROMs Matched (processing): " + romsMatchedProc +
                            "\n\n***Database Stats***" +
                            "\nGames Created: " + gamesCreated +
                            "\nGames Updated: " + gamesUpdated +
                            "\nROMs Updated: " + romsUpdated
                            );
                    int[] resG = DAT_Game.SaveToDatabase(gWorking);

                    gamesCreated += resG[0];
                    gamesUpdated += resG[1];

                    // now process the working list of ROMs
                    controller.SetMessage(output + "Updating ROMs in Database for " + sys.platformName +
                        "\n\n***In-Progress Stats***" +
                            "\nGames Created (processing): " + gamesCreatedProc +
                            "\nGames Updated (processing): " + gamesUpdatedProc +
                            "\nROMs Matched (processing): " + romsMatchedProc +
                            "\n\n***Database Stats***" +
                            "\nGames Created: " + gamesCreated +
                            "\nGames Updated: " + gamesUpdated +
                            "\nROMs Updated: " + romsUpdated
                            );
                    DAT_Rom.UpdateRoms(rWorking);

                    romsUpdated += romsMatchedProc;
                }


            });


            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Completed\n\nGames Created/Updated: " + (gamesCreated + gamesUpdated) + "\nRoms Updated: " + romsUpdated);
            }
        }

        public async void CalculateYearsAndPublishers()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Procedure",
                AnimateShow = false,
                AnimateHide = false
            };

            int working = 0;
            string output = "Processing Years & Publishers \n";

            var controller = await mw.ShowProgressAsync("Searching Years & Publishers", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            await Task.Run(() =>
            {
                // get all games
                List<DAT_Game> AllGames = DAT_Game.GetGames();
                List<DAT_Rom> AllRoms = DAT_Rom.GetRoms();

                List<DAT_Game> gWorking = new List<DAT_Game>();

                // automated searching first

                int numGames = AllGames.Where(a => a.year == null || a.publisher == null || a.developer == null).Count();

                int skipped = 0;

                // iterate through each game that does not have a year or publisher set
                foreach (var game in AllGames.Where(a => a.year == null || a.publisher == null || a.developer == null))
                {
                    DAT_Game g = new DAT_Game();
                    g = game;                

                    controller.SetMessage(output + "Number of games to process: " + numGames + "\nFixed: " + gWorking.Count() + "\nSkipped: " + skipped);

                    bool updateNeeded = false;                    

                    // lookup all roms for this game
                    List<DAT_Rom> search = (from a in AllRoms.Where(x => x.gid == game.gid)
                                           select a).ToList();

                    if (search.Count() > 0)
                    {
                        var yearSearch = (from a in search
                                          where a.year != null
                                          select a).FirstOrDefault();

                        var devSearch = (from a in search
                                         where a.developer != null
                                         select a).FirstOrDefault();

                        var pubSearch = (from a in search
                                         where a.publisher != null
                                         select a).FirstOrDefault();


                        if (yearSearch != null)
                        {
                            g.year = yearSearch.year;
                            updateNeeded = true;
                        }
                        if (pubSearch != null)
                        {
                            g.publisher = pubSearch.publisher;
                            updateNeeded = true;
                        }
                        if (devSearch != null)
                        {
                            g.developer = pubSearch.developer;
                            updateNeeded = true;
                        }
                    }

                    if (updateNeeded == true)
                    {
                        gWorking.Add(g);
                    }
                    else
                    {
                        skipped++;
                    }
                }

                // db work
                DAT_Game.SaveToDatabase(gWorking);
                working = gWorking.Count();

            });


            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Cancelled");
            }
            else
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Completed - " + working + " games updated");
            }

        }

        public async void CalculateYearsAndPublishersManual()
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Procedure",
                AnimateShow = false,
                AnimateHide = false
            };


            string output = "Processing Years & Publishers (scrape lookup) \n";

            var controller = await mw.ShowProgressAsync("Searching Years & Publishers", output, true, settings: mySettings);
            controller.SetCancelable(true);
            controller.SetIndeterminate();
            await Task.Delay(1000);

            int working = 0;

            await Task.Run(() =>
            {
                // get all games
                List<DAT_Game> AllGames = DAT_Game.GetGames();

                // get from scrapeDB

                List<ScrapeDB.MasterView> scrapes = ScrapeDB.MasterView.GetMasterView();

                List<DAT_Game> gWorking = new List<DAT_Game>();

                
                int numGames = AllGames.Where(a => a.year == null).Count();

                int skipped = 0;

                // iterate through each game that does not have a year or publisher set
                foreach (var game in AllGames.Where(a => a.year == null))
                {
                    DAT_Game g = new DAT_Game();
                    g = game;

                    controller.SetMessage(output + "Number of games to process: " + numGames + "\nFixed: " + gWorking.Count() + "\nSkipped: " + skipped);

                    bool updateNeeded = false;

                    // lookup names for this game
                    var searchAll = (from a in scrapes
                                  where a.pid == game.pid
                                  select a).ToList();

                    var search = searchAll.Where(a => FormatName(a.GDBTitle) == FormatName(game.gameName)).ToList();

                    if (search.Count() > 0)
                    {
                        var yearSearch = (from a in search
                                          where a.GDBYear != null
                                          select a).FirstOrDefault();

                        if (yearSearch != null)
                        {
                            g.year = yearSearch.GDBYear;
                            updateNeeded = true;
                            gWorking.Add(g);
                        } 
                        else
                        {
                            skipped++;
                        }                       
                    }
                    else
                    {
                        skipped++;
                    }
                }

                // db work
                DAT_Game.SaveToDatabase(gWorking);
                working = gWorking.Count();

            });

            


            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Completed");
            }
            else
            {
                await mw.ShowMessageAsync("DAT_Game Builder", "Procedure Completed - " + working + " games updated");
            }

        }

        /// <summary>
        /// Sanitise a game name string
        /// </summary>
        /// <param name="originalName"></param>
        /// <returns></returns>
        public static string FormatName(string originalName)
        {
            string working = originalName.Trim()
                .ToUpper()
                // odd chars
                .Replace(":", "")
                .Replace("-", "")
                .Replace("'", "")
                .Replace(",", "")
                .Replace("  ", " ")

                // number conversion
                .Replace(" 2", " II")
                .Replace(" 3", " III")
                .Replace(" 4", " IV")
                .Replace(" 5", " V")
                .Replace(" 6", " VI")
                .Replace(" 7", " VII")
                .Replace(" 8", " VIII")
                .Replace(" 9", " IX")
                .Replace(" 10", " X")
                .Replace(" 11", " XI")
                .Replace(" 12", " XII")
                .Replace(" 13", " XIII")
                .Replace(" 14", " XIV")
                .Replace(" 15", " XV")
                .Replace(" 16", " XVI")
                .Replace(" 17", " XVII")
                .Replace(" 18", "XVIII")
                
                // ands
                .Replace("&", "AND")
                .Replace("&amp;", "AND")
                
                // remove the
                .Replace("THE", "")

                // remove discs in string (psxdatacenter)
                .Replace("[2 DISCS ]", "")
                .Replace("[3 DISCS ]", "")
                .Replace("[4 DISCS ]", "")
                .Replace("[5 DISCS ]", "")
                .Replace("[6 DISCS ]", "")
                .Replace("[7 DISCS ]", "")
                .Replace("[8 DISCS ]", "")
                .Replace("[9 DISCS ]", "")
                .Replace("[10 DISCS ]", "")

                .Replace("[ 2 DISCS ]", "")
                .Replace("[ 3 DISCS ]", "")
                .Replace("[ 4 DISCS ]", "")
                .Replace("[ 5 DISCS ]", "")
                .Replace("[ 6 DISCS ]", "")
                .Replace("[ 7 DISCS ]", "")
                .Replace("[ 8 DISCS ]", "")
                .Replace("[ 9 DISCS ]", "")
                .Replace("[ 10 DISCS ]", "")

                // final trim
                .Trim();

            return working;
        }
    }

    public enum ProviderType
    {
        NoIntro,
        ToSec,
        ReDump,
        TruRip,
        PsxDataCenter,
        Satakore
    }
}
