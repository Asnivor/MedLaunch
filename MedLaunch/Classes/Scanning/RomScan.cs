using Asnitech.Launch.Common;
using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.DAT;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.IO;
using MedLaunch.Classes.Scraper.DAT.Models;
using MedLaunch.Common.Eventing.Listeners;
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
        public bool IsSingleRomInArchive { get; set; }

        public Common.IO.Compression.Archive archive { get; set; }

        public RomScan()
        {
            allowedFiles = new List<string>();
            finalGames = new List<Game>();
            presentGames = new List<Game>();
            ArchiveFiles = new List<Archiving>();

            archive = new Common.IO.Compression.Archive();
            
        }

        

        // Start ROM scan and import process for specific system
        public void BeginRomImport(int _systemId, ProgressDialogController _dialog)
        {
            allowedFiles = new List<string>();
            dialog = _dialog;
            systemId = _systemId;

            Common.Eventing.Listeners.ProgressDialogListener l = new Common.Eventing.Listeners.ProgressDialogListener(dialog, SignatureType.Archive);
            l.Subscribe(archive);

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
            numFiles = allowedFiles.Count;
            progress = 0;
            // set base dialog message
            strBase = "Scanning: ";

            // now we have a list of allowed files, loop through them
            foreach (string file in allowedFiles)
            {
                if (_dialog.IsCanceled)
                    return;
                ProcessFile(file);                
            }

            // whatever games are left in the presentGames list should be marked as hidden as they have not been found
            if (presentGames.Count > 0)
            {
                foreach (Game g in presentGames)
                {
                    g.hidden = true;
                    RomsToUpdate.Add(g);
                    HiddenStats++;
                }
            }

            //GameListBuilder.UpdateFlag();

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
                //bool isAllowed = false;
                try
                {
                    archive.ArchivePath = file;
                    var results = archive.ProcessArchive(GSystem.GetAllowedFileExtensions(systemId).ToArray());
                    
                    if (archive.AllowedFilesDetected == 0)
                    {
                        // no files returned
                        return;
                    }
                    else
                    {
                        // check how many allowed files are in the archive
                        int alcnt = archive.AllowedFilesDetected;

                        if (alcnt == 0)
                        {
                            // no allowed files
                            return;
                        }
                        else if (alcnt == 1 && extension == ".zip")
                        {
                            // 1 allowed file and 1 total files in a zip file - use the zip file rather than the embedded rom
                            var result = results.Results.First();

                            if (result.Extension == ".zip" || result.Extension == ".7z")
                                return;

                            //ArchiveFiles.Add(a);
                            // actual rom extension - not archive extension
                            string romExtension = result.Extension;
                            // generate relative path (normal archive path)
                            string romRelPath = relPath + "*/" + result.InternalPath;
                            // get romname without extension
                            string name = result.RomName;

                            ProcessGame(name, result.MD5, romRelPath, result.FileName, romExtension);
                          
                        }
                        else
                        {
                            // either multiple allowed files or
                            // 7zip archive
                            // - Add the individual embedded rom(s)
                            IsSingleRomInArchive = false;
                            foreach (var r in results.Results)
                            {
                                //ArchiveFiles.Add(a);
                                // actual rom extension - not archive extension
                                string romExtension = r.Extension;
                                // generate relative path (normal archive path + "*")
                                string romRelPath = relPath + "*/" + r.InternalPath;
                                // get romname without extension
                                string name = r.RomName;

                                ProcessGame(name, r.MD5, romRelPath, r.FileName, romExtension);
                            }
                        }

                        

                    }

                    /*

                    // retrieve the list of archive objects retrieved
                    if (Archiving.ArchiveMultiple.Count == 1)
                    {
                        IsSingleRomInArchive = true;
                        // single allowed rom file in archive detected - parse the archive as a normal game
                        hash = Archiving.ArchiveMultiple[0].MD5Hash;
                        isAllowed = Archiving.ArchiveMultiple[0].IsAllowed;
                        if (hash == null)
                        {
                            // hash could not be calculated - skip on general principle
                            return;
                        }

                        if (isAllowed == false)
                            return;

                        ProcessGame(romName, hash, relPath, fileName, extension);
                    }
                    else if (Archiving.ArchiveMultiple.Count == 0)
                    {
                        // no files returned
                        return;
                    }
                    else
                    {
                        IsSingleRomInArchive = false;
                        // multiple allowed rom files in archive detected - add to list to process later
                        foreach (var a in Archiving.ArchiveMultiple.Where(i => i.IsAllowed == true))
                        {
                            //ArchiveFiles.Add(a);
                            // actual rom extension - not archive extension
                            string romExtension = "." + (a.FileName.Split('.').Last());
                            // generate relative path (normal archive path + "*")
                            string romRelPath = relPath + "*" + a.FileName;
                            // get romname without extension
                            string name = a.FileName.Replace(romExtension, "");

                            ProcessGame(name, a.MD5Hash, romRelPath, a.FileName, romExtension);
                        }
                    }

                    */
                }
                catch (System.IO.InvalidDataException ex)
                {
                    // problem with the archive file
                    Console.WriteLine(ex);
                }
                finally { }

                //if (isAllowed == false) { return; }
            }
            else
            {
                // file is not an archive - calculate md5
                //hash = Crypto.Crc32.ComputeFileHash(file);
                hash = Crypto.checkMD5(file);

                /* process single game (no archive) */

                if (extension.ToLower().Contains("7z") || extension.ToLower().Contains("zip"))
                    return;

                ProcessGame(romName, hash, relPath, fileName, extension);
            }
   
        }

        public void ProcessGame(string romName, string hash, string relPath, string fileName, string extension)
        {
            Game newGame = new Game();

            // check whether game already exists (by gameName and systemId)
            Game chkGame = (from g in Games
                            where g.systemId == systemId && g.gameName == romName //&& g.archiveGame == archiveGame
                            select g).FirstOrDefault();

            // filter DAT by systemId
            List<DATMerge> lookup = DATMerge.FilterByMedLaunchSystemId(DAT, systemId);

            // lookup game in master dat - order by DATProviderId (so NoIntro first)
            string nHash = hash.ToUpper().Trim().ToString();
            List<DATMerge> look = lookup.Where(a => a.MD5.ToUpper().Trim() == nHash).OrderBy(a => a.DatProviderId).ToList();

            int subSysId = GSystem.GetSubSystemId(systemId, extension);

            if (chkGame == null)
            {
                // does not already exist - create new game
                newGame.configId = 1;

                if (look != null && look.Count > 0)
                {
                    newGame.gameNameFromDAT = look.First().GameName;
                    //newGame.Publisher = look.First().Publisher;
                    //newGame.Year = look.First().Year;
                    newGame.romNameFromDAT = look.First().RomName;
                    newGame.Copyright = look.First().Copyright;
                    newGame.Country = look.First().Country;
                    newGame.DevelopmentStatus = look.First().DevelopmentStatus;
                    newGame.Language = look.First().Language;
                    newGame.OtherFlags = look.First().OtherFlags;
                    //newGame.Publisher = look.First().Publisher;
                    //newGame.Year = look.First().Year;
                    //newGame.Developer = look.First().Developer;

                    if (look.First().Year != null && look.First().Year != "")
                    {
                        newGame.Year = look.First().Year;
                    }
                    if (look.First().Publisher != null && look.First().Publisher != "")
                    {
                        newGame.Publisher = look.First().Publisher;
                    }
                    if (look.First().Developer != null && look.First().Developer != "")
                    {
                        newGame.Developer = look.First().Developer;
                    }
                }

                newGame.gameName = romName;
                newGame.gamePath = relPath;
                newGame.hidden = false;
                newGame.isDiskBased = false;
                newGame.isFavorite = false;
                newGame.systemId = systemId;
                newGame.CRC32 = hash;

                // check for subsystemid
                if (subSysId > 0)
                {
                    // sub system found
                    newGame.subSystemId = subSysId;
                }

                // add to finaGames list
                RomsToAdd.Add(newGame);
                // increment the added counter
                AddedStats++;
            }
            else
            {
                // matching game found - update it
                if (chkGame.gamePath == relPath && chkGame.hidden == false && chkGame.CRC32 == hash && chkGame.subSystemId == subSysId)
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
                    if (look != null && look.Count > 0)
                    {
                        newGame.gameNameFromDAT = look.First().GameName;
                        //newGame.Publisher = look.First().Publisher;
                        //newGame.Year = look.First().Year;
                        //newGame.Developer = look.First().Developer;
                        newGame.romNameFromDAT = look.First().RomName;
                        newGame.Copyright = look.First().Copyright;
                        newGame.Country = look.First().Country;
                        newGame.DevelopmentStatus = look.First().DevelopmentStatus;
                        newGame.Language = look.First().Language;
                        newGame.OtherFlags = look.First().OtherFlags;

                        if (look.First().Year != null && look.First().Year != "")
                        {
                            newGame.Year = look.First().Year;
                        }
                        if (look.First().Publisher != null && look.First().Publisher != "")
                        {
                            newGame.Publisher = look.First().Publisher;
                        }
                        if (look.First().Developer != null && look.First().Developer != "")
                        {
                            newGame.Developer = look.First().Developer;
                        }
                    }

                    // check for subsystemid
                    if (subSysId > 0)
                    {
                        // sub system found
                        newGame.subSystemId = subSysId;
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
                        //GameListBuilder.UpdateFlag();

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
                    //GameListBuilder.UpdateFlag();
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
