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
        public List<string> allowedFiles { get; set; }
        public List<Game> finalGames { get; set; }
        public string currentFilePath { get; set; }
        public List<Game> presentGames { get; set; }
        public ProgressDialogController dialog { get; set; }
        public int progress { get; set; }
        public int numFiles { get; set; }
        public string strBase { get; set; }
        public string romFolderPath { get; set; }
        public int systemId { get; set; }
        public List<Archiving> ArchiveFiles { get; set; }

        public RomScan()
        {
            allowedFiles = new List<string>();
            finalGames = new List<Game>();
            presentGames = new List<Game>();
            ArchiveFiles = new List<Archiving>();
        }

        

        // Start ROM scan and import process for specific system
        public void BeginRomImport(int _systemId, ProgressDialogController _dialog)
        {
            dialog = _dialog;
            systemId = _systemId;

            // get path to ROM folder
            romFolderPath = GetPath(systemId);
            // get allowed file types for this particular system
            HashSet<string> exts = GSystem.GetAllowedFileExtensions(systemId);
            // get a list of games for this system currently already in the database
            presentGames = (from g in Games
                                       where g.systemId == systemId
                                       select g).ToList();
            // get all files from romfolderpath and sub directories that have an allowed extension
            IEnumerable<string> romFiles = FileAndFolder.GetFiles(romFolderPath, true);
            // if romfiles is null break
            if (romFiles == null)
                return;

            // populate list of allowed file paths
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
            }

            // calculate the number of files to be processed
            int numFiles = allowedFiles.Count;
            progress = 0;
            // set base dialog message
            strBase = "Scanning: ";

            // now we have a list of allowed files, loop through them
            foreach (string file in allowedFiles)
            {
                ProcessFile(file);                
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

        public void ProcessFile(string file)
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
            string hash = String.Empty;

            // inspect archive files
            if (extension == ".zip" || extension == ".7z")
            {
                bool isAllowed = false;
                try
                {
                    // create an instance of the archiving class and process this archive
                    Archiving arch = new Archiving(file, systemId);
                    arch.ProcessArchive();

                    // retrieve the list of archive objects retrieved

                    if (arch.IsSingleFileInArchive == true)
                    {
                        // single allowed rom file in archive detected - parse the archive as a normal game
                        hash = arch.MD5Hash;
                        isAllowed = arch.IsAllowed;
                        if (hash == null)
                        {
                            // hash could not be calculated - skip on general principle
                            return;
                        }
                    }
                    else
                    {
                        // multiple allowed rom files in archive detected - add to list to process later
                        foreach (var a in Archiving.ArchiveMultiple.Where(i => i.IsAllowed == true))
                        {
                            ArchiveFiles.Add(a);
                        }
                    }
                }
                catch (System.IO.InvalidDataException ex)
                {
                    // problem with the archive file
                }
                finally { }

                if (isAllowed == false) { return; }
            }
            else
            {
                // file is not an archive - calculate md5
                //hash = Crypto.Crc32.ComputeFileHash(file);
                hash = Crypto.checkMD5(file);
            }

            /* process single game */
            ProcessGame(romName, hash, relPath, fileName, extension);

            /* process multi-archive games */
            foreach (var aGame in ArchiveFiles)
            {
                // actual rom extension - not archive extension
                string romExtension = "." + (aGame.FileName.Split('.').Last());
                // generate relative path (normal archive path + "*/")
                string romRelPath = relPath + "*/" + aGame.FileName;
                // get romname without extension
                string name = aGame.FileName.Replace(romExtension, "");

                ProcessGame(name, aGame.MD5Hash, romRelPath, aGame.FileName, romExtension);
            }
        }

        public void ProcessGame(string romName, string hash, string relPath, string fileName, string extension)
        {
            Game newGame = new Game();

            // check whether game already exists (by gameName and systemId)
            Game chkGame = (from g in Games
                            where g.systemId == systemId && g.gameName == romName //&& g.archiveGame == archiveGame
                            select g).FirstOrDefault();

            // lookup game in master dat
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
