using Asnitech.Launch.Common;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.IO;
using MedLaunch.Classes.Scraper.DAT.Models;
using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.Scanning
{
    public class RomScan : GameScanner
    {
        // Start ROM scan and import process for specific system
        public void BeginRomImport(int systemId, ProgressDialogController dialog)
        {
            // get path to ROM folder
            string romFolderPath = GetPath(systemId);
            //MessageBoxResult result2 = MessageBox.Show(romFolderPath);
            // get allowed file types for this particular system
            HashSet<string> exts = GSystem.GetAllowedFileExtensions(systemId);

            // get a list of games for this system currently already in the database
            List<Game> presentGames = (from g in Games
                                       where g.systemId == systemId
                                       select g).ToList();


            // get all files from romfolderpath and sub directories that have an allowed extension
            IEnumerable<string> romFiles = FileAndFolder.GetFiles(romFolderPath, true);

            // if romfiles is null break
            if (romFiles == null)
                return;

            List<string> allowedFiles = new List<string>();
            foreach (string s in exts)
            {

                foreach (string p in romFiles)
                {
                    if (p.EndsWith(s))
                    {
                        //MessageBoxResult result5 = MessageBox.Show(p);
                        allowedFiles.Add(p);
                    }

                }
                /*
                List<string> fi = (from a in romFiles
                         where a.EndsWith(s)
                         select a).ToList();
                if (fi == null || fi.Count < 1) { continue; }
                allowedFiles.AddRange(fi);       
                */
            }

            // calculate the number of files to be processed
            int numFiles = allowedFiles.Count;
            // set the progress bar limits
            //dialog.Minimum = 0;
            //dialog.Maximum = numFiles;
            int progress = 0;
            // set base dialog message
            string strBase = "Scanning: ";

            // create new final list to be populated with approved files
            List<Game> finalGames = new List<Game>();

            // now we have a list of allowed files, loop through them
            foreach (string file in allowedFiles)
            {
                // get the relative path
                string relPath = PathUtil.GetRelativePath(romFolderPath, file);
                // get just the filename
                string fileName = System.IO.Path.GetFileName(file);
                // get just the extension
                string extension = System.IO.Path.GetExtension(file).ToLower();
                // get rom name wihout extension
                string romName = fileName.Replace(extension, "");

                // update UI
                progress++;
                string uiUpdate = strBase + fileName + "\n(" + progress + " of " + numFiles + ")";
                dialog.SetMessage(uiUpdate);
                //dialog.SetProgress(progress);

                Game newGame = new Game();
                string hash = String.Empty;

                // inspect archive files
                if (extension == ".zip" || extension == ".7z")
                {
                    bool isAllowed = false;
                    try
                    {
                        Archiving arch = new Archiving(file, systemId);
                        arch.ProcessArchive();
                        hash = arch.MD5Hash;
                        isAllowed = arch.IsAllowed;
                        if (hash == null)
                        {
                            continue;
                        }
                    }
                    catch (System.IO.InvalidDataException ex)
                    {
                        // problem with the archive file
                    }
                    finally { }

                    if (isAllowed == false) { continue; }
                }
                else
                {
                    // file is not an archive - calculate md5
                    //hash = Crypto.Crc32.ComputeFileHash(file);
                    hash = Crypto.checkMD5(file);
                }


                // check whether game already exists (by gameName and systemId)
                Game chkGame = (from g in Games
                                where g.systemId == systemId && g.gameName == romName //&& g.archiveGame == archiveGame
                                select g).FirstOrDefault();

                // lookup game in master dat
                //var sysFilter = DAT.Where(p => p.SystemId == systemId);
                // var lookup = DAT.Where(p => p.Roms.Any(x => x.MD5.ToUpper().Trim() == hash.ToUpper().Trim())).ToList();

                //var lookup = DAT.Where(p => p.Roms.Any(x => x.MD5.ToUpper() == hash)).ToList();
                string nHash = hash.ToUpper().Trim().ToString();
                List<DATMerge> lookup = (from i in DAT
                                         where i.SystemId == systemId && i.Roms.Any(l => l.MD5.ToUpper().Trim() == hash)
                                         select i).ToList();

                if (chkGame == null)
                {
                    // does not already exist - create new game
                    newGame.configId = 1;

                    if (lookup != null && lookup.Count > 0)
                    {
                        newGame.gameNameFromDAT = lookup.First().GameName;
                        newGame.Publisher = lookup.First().Publisher;
                        newGame.Year = lookup.First().Year;

                        // get rom we are interested in
                        var rom = (from ro in lookup.First().Roms
                                   where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                                   select ro).First();
                        newGame.romNameFromDAT = rom.RomName;
                        newGame.Copyright = rom.Copyright;
                        newGame.Country = rom.Country;
                        newGame.DevelopmentStatus = rom.DevelopmentStatus;
                        newGame.Language = rom.Language;
                        newGame.OtherFlags = rom.OtherFlags;

                        if (rom.Year != null && rom.Year != "")
                        {
                            newGame.Year = rom.Year;
                        }
                        if (rom.Publisher != null && rom.Publisher != "")
                        {
                            newGame.Publisher = rom.Publisher;
                        }

                    }

                    newGame.gameName = romName;
                    newGame.gamePath = relPath;
                    newGame.hidden = false;
                    newGame.isDiskBased = false;
                    newGame.isFavorite = false;
                    newGame.systemId = systemId;
                    newGame.CRC32 = hash;

                    // add to finaGames list
                    RomsToAdd.Add(newGame);
                    // increment the added counter
                    AddedStats++;
                }
                else
                {
                    // matching game found - update it
                    if (chkGame.gamePath == relPath && chkGame.hidden == false && chkGame.CRC32 == hash)
                    {
                        //nothing to update - increment untouched counter
                        UntouchedStats++;
                    }
                    else
                    {
                        newGame = chkGame;
                        // update path in case it has changed location
                        newGame.gamePath = relPath;
                        // mark as not hidden
                        newGame.hidden = false;

                        newGame.CRC32 = hash;
                        if (lookup != null && lookup.Count > 0)
                        {
                            newGame.gameNameFromDAT = lookup.First().GameName;
                            newGame.Publisher = lookup.First().Publisher;
                            newGame.Year = lookup.First().Year;

                            // get rom we are interested in
                            var rom = (from ro in lookup.First().Roms
                                       where ro.MD5.ToUpper().Trim() == hash.ToUpper().Trim()
                                       select ro).First();
                            newGame.romNameFromDAT = rom.RomName;
                            newGame.Copyright = rom.Copyright;
                            newGame.Country = rom.Country;
                            newGame.DevelopmentStatus = rom.DevelopmentStatus;
                            newGame.Language = rom.Language;
                            newGame.OtherFlags = rom.OtherFlags;

                            if (rom.Year != null && rom.Year != "")
                            {
                                newGame.Year = rom.Year;
                            }
                            if (rom.Publisher != null && rom.Publisher != "")
                            {
                                newGame.Publisher = rom.Publisher;
                            }
                        }

                        // add to finalGames list
                        RomsToUpdate.Add(newGame);
                        // increment updated counter
                        UpdatedStats++;
                    }

                    // remove game from presentGames list - remaining games in this list will be marked as hidden at the end
                    presentGames.Remove(chkGame);

                }
            }

            // whatever games are left in the presentGames list should be marked as hidden as they have not been found
            if (presentGames.Count > 0)
            {
                foreach (Game g in presentGames)
                {
                    g.hidden = true;
                    RomsToUpdate.Add(g);

                }
            }

            GameListBuilder.UpdateFlag();

        }

        // mark all ROMS from a system as hidden (as long as it is not a disk based game)
        public void MarkAllRomsAsHidden(int systemId)
        {
            List<Game> gamesAll = (from g in Games
                                   where g.systemId == systemId
                                   select g).ToList();
            List<Game> games = (from g in gamesAll
                                where g.isDiskBased == false
                                select g).ToList();

            if (games == null)
            {
                // no games found
            }
            else
            {
                // iterate through each game
                foreach (Game game in games)
                {
                    Game newGame = game;
                    if (newGame.hidden == false)
                    {
                        newGame.hidden = true;
                        // add to GamesToUpdate to be processed later
                        RomsToUpdate.Add(newGame);
                        HiddenStats++;
                        GameListBuilder.UpdateFlag();

                    }
                    else
                    {
                        // game is already marked as hidden
                        UntouchedStats++;
                    }
                }
            }
        }

        // mark single game as hidden
        public void MarkRomAsHidden(int gameId)
        {
            Game game = (from g in Games
                         where g.gameId == gameId
                         select g).ToList().SingleOrDefault();
            if (game == null)
            {
                // no game found
            }
            else
            {
                Game newGame = game;
                if (newGame.hidden == false)
                {
                    newGame.hidden = true;
                    // add to GamesToUpdate to be processed later
                    RomsToUpdate.Add(newGame);
                    HiddenStats++;
                    GameListBuilder.UpdateFlag();
                }
                else
                {
                    // game is already hidden
                    UntouchedStats++;
                }
            }
        }
    }
}
