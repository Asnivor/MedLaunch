using MahApps.Metro.Controls.Dialogs;
using MedLaunch.Classes.DAT;
using MedLaunch.Classes.GamesLibrary;
using MedLaunch.Classes.IO;
using MedLaunch.Classes.Scraper.DAT.Models;
using MedLaunch.Classes.Scraper.PSXDATACENTER;
using MedLaunch.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedLaunch.Classes.Scanning
{
    public class DiscScan : GameScanner
    {       

        public App _App { get; set; }

        public DiscScan()
        {
            _App = (App)Application.Current;            
        }

        // Start Disc scan and import process for specific system
        public void BeginDiscImportImproved(int systemId, ProgressDialogController dialog)
        {
            // get path to root DISC folder
            string discFolderPath = GetPath(systemId);

            // get allowed file types for this particular system
            HashSet<string> exts = GSystem.GetAllowedFileExtensions(systemId);

            // get a list of games for this system currently already in the database
            List<Game> presentGames = (from g in Games
                                       where g.systemId == systemId
                                       select g).ToList();

            // check whether presentGames paths are valid - mark as hidden if not
            foreach (var g in presentGames.Where(a => a.hidden == false))
            {
                if (!File.Exists(g.gamePath))
                {
                    g.hidden = true;
                    DisksToUpdate.Add(g);
                    HiddenStats++;
                }
            }

            /* all individual disc game files MUST reside in the same folder, but that folder can be anywhere under the root path */

            // get all subfolders within the system folder
            if (!Directory.Exists(discFolderPath))
            {
                return;
            }

            // get all the sub-directories
            List<string> subs = Directory.GetDirectories(discFolderPath).ToList();

            // add the root directory
            subs.Add(discFolderPath);
            subs.Reverse();

            if (subs.Count < 2)
                return;

            int foldersFound = subs.Count;
            string strBase = "\nScanning Disc Folders: ";

            // iterate through each directory (there could be any number of games in each one)
            for (int i = 0; i < subs.Count; i++)
            {
                dialog.SetMessage(strBase + (i + 1) + " of " + subs.Count);

                if (dialog.IsCanceled == true)
                    return;

                var discGames = DetermineAllGamesInFolder(subs[i], systemId);



                List<DiscGameFile> game = DetermineDiscFileFromSubFolder(subs[i], systemId);

                // if none found skip
                if (game.Count == 0)
                    continue;

                // if a single sheet file in the List, add this to library
                if (game.Count == 1)
                {
                    InsertOrUpdateDisk(game.First(), systemId);
                    continue;
                }

                // if multiple, create m3u file
                if (game.Count > 0)
                {

                    string t = game.First().FolderPath + "\\" + game.First().GameName;
                    CreateM3uPlaylist(game.OrderBy(a => a.FileName).ToList(), game.First().FolderPath + "\\" + game.First().GameName + ".m3u", true);
                    // create a new discgamefile for the m3u and add it to library
                    InsertOrUpdateDisk(new DiscGameFile(game.First().FolderPath + "\\" + game.First().GameName + ".m3u", systemId), systemId);
                    continue;
                }
            }

            //GameListBuilder.UpdateFlag();

        }

        /// <summary>
        /// Examine a folder and return a List<List<DiscGameFile> List ( may contain singles or multiples depending on logic)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public static List<DiscGameFile> DetermineAllGamesInFolder(string folderPath, int sysId)
        {
            // master list to hold the different games found
            List<DiscGameFile> DetectedGames = new List<DiscGameFile>(); 

            //List<DiscGameFile> list = new List<DiscGameFile>();
            //DiscGameFile gdf = new DiscGameFile();

            // get all files in the folder (that have extensions we are interested in)
            List<string> cueFiles = (from a in FileAndFolder.GetFiles(folderPath, false)
                                     where a.ToLower().Contains(".m3u") ||
                                     a.ToLower().Contains(".cue") ||
                                     a.ToLower().Contains(".ccd") ||
                                     a.ToLower().Contains(".toc")
                                     select a).ToList();

            var cueFTemp = cueFiles.ToList();

            // deal with m3u's first
            foreach (string s in cueFTemp.Where(a => a.ToLower().EndsWith(".m3u")))
            {
                bool m3uValid = true;

                // interogate the m3u to get referenced cue files
                DiscGameFile d = new DiscGameFile(s, sysId);
                var result = ParseTrackSheet(d, CueType.m3u, sysId);

                // check that the referenced cue files are present
                foreach (var thing in result)
                {
                    if (!File.Exists(thing.FullPath))
                    {
                        // cuefile could not be found
                        m3uValid = false;
                        break;
                    } 
                }

                if (m3uValid == false)
                {
                    // problem with this m3u - remove from cueFiles
                    cueFiles.Remove(d.FullPath);
                    continue;
                }

                // if we get this far, all the referenced cue files in the m3u are valid - remove them from cuefiles
                foreach (var thing in result)
                {
                    cueFiles.Remove(thing.FullPath);
                }

                // now add to detected games
                DetectedGames.Add(d);
            }

            /* now cueFiles should only have cues remaining that are not referenced by M3U */
            cueFTemp = cueFiles.ToList();

            // separate the potential multi-disc games
            List<string> multiGames = new List<string>();
            List<string> singleGames = new List<string>();

            foreach (string s in cueFiles)
            {
                for (int disc = 1; disc < 10; disc++)
                {
                    if (s.ToLower().Contains("disc " + disc) ||
                    s.ToLower().Contains("cd " + disc) ||
                    s.ToLower().Contains("d" + disc) ||
                    s.ToLower().Contains("c" + disc) ||
                    s.ToLower().Contains("cd" + disc) ||
                    s.ToLower().Contains("disk" + disc) ||
                    s.ToLower().Contains("disk " + disc) ||
                    s.ToLower().Contains("cd" + disc) ||
                    s.ToLower().Contains("disc" + disc))
                    {
                        // add to multiGames to sort out later
                        multiGames.Add(s);
                        // remove from cueFTemp
                        cueFTemp.Remove(s);
                        break;
                    }
                }                
            }

            // cueFTemp should now only have single disc games
            // find cues that have the same name (possibly there are both .cue and .ccd for each game)
            var duplicateCues = cueFTemp.Where(a => a.Contains(Path.GetFileNameWithoutExtension(a)));
                              



            // count file types
            int cueCount = cueFiles.Where(a => a.ToLower().Contains(".cue")).ToList().Count();
            int ccdCount = cueFiles.Where(a => a.ToLower().Contains(".ccd")).ToList().Count();
            int tocCount = cueFiles.Where(a => a.ToLower().Contains(".toc")).ToList().Count();
            /*
            // quick win - deal with single games - start with single cue
            if (cueCount == 1 && ccdCount == 0 && tocCount == 0)
            {
                string cueFile = cueFiles.Where(a => a.ToLower().Contains(".cue")).ToList().First();
                gdf = new DiscGameFile(cueFile, sysId);
                list.Add(gdf);
                return list;
            }

            // now check for single ccd
            if (ccdCount == 1 && cueCount == 0 && tocCount == 0)
            {
                string ccdFile = cueFiles.Where(a => a.ToLower().Contains(".ccd")).ToList().First();
                gdf = new DiscGameFile(ccdFile, sysId);
                list.Add(gdf);
                return list;
            }

            // now check for single toc
            if (tocCount == 1 && cueCount == 0 && ccdCount == 0)
            {
                string tocFile = cueFiles.Where(a => a.ToLower().Contains(".toc")).ToList().First();
                list.Add(gdf);
                return list;
            }
            */
            /* done with m3us and single sheet files - now comes multiples logic */

            // get ALL sheet files (not including m3us)
            List<string> sheets = (from a in cueFiles
                                   where !a.ToLower().Contains(".m3u")
                                   select a).ToList();

            // instantiate working set
            List<DiscGameFile> working = new List<DiscGameFile>();

            List<string> disc1 = new List<string>();
            List<string> disc2 = new List<string>();
            List<string> disc3 = new List<string>();
            List<string> disc4 = new List<string>();
            List<string> disc5 = new List<string>();
            List<string> disc6 = new List<string>();

            // lookup based on disc number
            for (int i = 0; i < sheets.Count; i++)
            {
                for (int disc = 1; disc < 7; disc++)
                {
                    if (sheets[i].ToLower().Contains("disc " + disc) ||
                    sheets[i].ToLower().Contains("cd " + disc) ||
                    sheets[i].ToLower().Contains("d" + disc) ||
                    sheets[i].ToLower().Contains("c" + disc) ||
                    sheets[i].ToLower().Contains("cd" + disc) ||
                    sheets[i].ToLower().Contains("disk" + disc) ||
                    sheets[i].ToLower().Contains("disk " + disc) ||
                    sheets[i].ToLower().Contains("cd" + disc) ||
                    sheets[i].ToLower().Contains("disc" + disc))
                    {
                        if (disc == 1) { disc1.Add(sheets[i]); }
                        if (disc == 2) { disc2.Add(sheets[i]); }
                        if (disc == 3) { disc3.Add(sheets[i]); }
                        if (disc == 4) { disc4.Add(sheets[i]); }
                        if (disc == 5) { disc5.Add(sheets[i]); }
                        if (disc == 6) { disc6.Add(sheets[i]); }

                    }
                }

            }
            List<List<string>> combined = new List<List<string>>
            {
                disc1, disc2, disc3, disc4, disc5, disc6
            };

            // now loop through the combined object
            for (int disc = 0; disc < 6; disc++)
            {
                // if disc 1 is not present, then dont go any further
                if (combined[0].Count == 0)
                    break;

                // if there is a single disc only per disc list then add it to working
                if (combined[disc].Count == 1)
                    working.Add(new DiscGameFile(combined[disc].First(), sysId));

                // if there are multiple discs per disc list then we have to loop through each of them and check whether they are valid
                if (combined[disc].Count > 1)
                {
                    foreach (string s in combined[disc])
                    {
                        string test = null;

                        if (Path.GetExtension(s).ToLower() == ".cue") { test = ParseNonM3UTrackSheetString(s, CueType.cue, sysId); }
                        if (Path.GetExtension(s).ToLower() == ".ccd") { test = ParseNonM3UTrackSheetString(s, CueType.ccd, sysId); }
                        if (Path.GetExtension(s).ToLower() == ".toc") { test = ParseNonM3UTrackSheetString(s, CueType.toc, sysId); }


                        // continue if nothing returned
                        if (test == null || test == "")
                            continue;

                        // prefer cue
                        if (Path.GetExtension(s).ToLower() == ".cue")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                        // then ccd
                        if (Path.GetExtension(s).ToLower() == ".ccd")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                        // then toc
                        if (Path.GetExtension(s).ToLower() == ".toc")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                    }
                }
            }

            // now - working should contain all disc(s) for this particular game
            return working;

        }

        

        // Start Disc scan and import process for specific system
        public void BeginDiscImport(int systemId, ProgressDialogController dialog)
        {            
            // get path to root DISC folder
            string discFolderPath = GetPath(systemId);
            //MessageBoxResult result2 = MessageBox.Show(romFolderPath);
            // get allowed file types for this particular system
            HashSet<string> exts = GSystem.GetAllowedFileExtensions(systemId);

            // get a list of games for this system currently already in the database
            List<Game> presentGames = (from g in Games
                                       where g.systemId == systemId
                                       select g).ToList();
            
            // check whether presentGames paths are valid - mark as hidden if not
            foreach (var g in presentGames.Where(a => a.hidden == false))
            {
                if (!File.Exists(g.gamePath))
                {
                    g.hidden = true;
                    DisksToUpdate.Add(g);
                    HiddenStats++;
                }
            }
            

            /* disc games at the moment MUST reside in 1st level subfolders within the system folder */

        // get all subfolders within the system folder
        
        if (!Directory.Exists(discFolderPath))
        {
            return;                
        }

        List<string> subs = Directory.GetDirectories(discFolderPath).ToList();
        if (subs.Count == 0)
            return;

        int foldersFound = subs.Count;
        string strBase = "\nScanning Game Directory: ";

        // iterate through each sub-directory (should be one game in each)
        for (int i = 0; i < subs.Count; i++)
        {
            //string uiUpdate = strBase + "\nGames Found: " + gamesFound;
            //dialog.SetMessage(uiUpdate);
            dialog.SetMessage(strBase + (i + 1) + " of " + subs.Count);

            if (dialog.IsCanceled == true)
                return;

            List<DiscGameFile> game = DetermineDiscFileFromSubFolder(subs[i], systemId);

            // if none found skip
            if (game.Count == 0)
                continue;

            // if a single sheet file in the List, add this to library
            if (game.Count == 1)
            {
                InsertOrUpdateDisk(game.First(), systemId);
                continue;
            }

            // if multiple, create m3u file
            if (game.Count > 0)
            {

                string t = game.First().FolderPath + "\\" + game.First().GameName;
                CreateM3uPlaylist(game.OrderBy(a => a.FileName).ToList(), game.First().FolderPath + "\\" + game.First().GameName + ".m3u", true);
                // create a new discgamefile for the m3u and add it to library
                InsertOrUpdateDisk(new DiscGameFile(game.First().FolderPath + "\\" + game.First().GameName + ".m3u", systemId), systemId);
                continue;
            }
        }

        //GameListBuilder.UpdateFlag();

    }

    

        /// <summary>
        /// Manually choose a disc game and import into database/library
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public DiscGameFile SelectGameFile(int sysId)
        {
            // get allowed file types for this system
            List<string> exts = (GSystem.GetAllowedFileExtensions(sysId)).ToList();

            // convert allowed types to filter string
            string filter = "";
            string fStart = "Allowed Types (";
            string fEnd = "";
            foreach (string i in exts)
            {
                if (i == "") { continue; }

                fStart += "*" + i + ",";
                fEnd += "*" + i + ";";
            }
            char comma = ',';
            char semi = ';';
            filter = (fStart.TrimEnd(comma)) + ")|" + (fEnd.TrimEnd(semi));
            //MessageBox.Show(filter);

            // open the file dialog showing only allowed file types - multi-select enabled
            OpenFileDialog filePath = new OpenFileDialog();
            filePath.Multiselect = true;
            filePath.Filter = filter;
            filePath.Title = "Select a single (or multiple in the same directory) .cue or .ccd or .toc file(s)";
            filePath.ShowDialog();

            if (filePath.FileNames.Length > 0)
            {
                // file(s) have been selected
                List<string> files = filePath.FileNames.ToList();

                // single or multiple files?
                if (files.Count > 1)
                {

                    // Create a list of GameFile Objects to process
                    List<DiscGameFile> games = new List<DiscGameFile>();

                    // iterate through each game
                    foreach (string game in files)
                    {
                        // Create a new DiskGameFile instance with all path details
                        DiscGameFile g = new DiscGameFile(game, sysId);
                        // add to list
                        games.Add(g);
                    }

                    // process the list and create an m3u playlist file - all selected files have to be in the same directory
                    List<DiscGameFile> ordered = (from a in games
                                                  select a).OrderBy(a => a.FileName).ToList();

                    // check whether an m3u playlist file already exists
                    var firstEntry = (from a in ordered
                                      select a).First();

                    // create string for the new m3u path
                    string m3uPath = firstEntry.FolderPath + "\\" + firstEntry.GameName + ".m3u";
                    //MessageBox.Show(m3uPath);

                    // create GameFIle object for the m3u playlist
                    DiscGameFile mf = new DiscGameFile(m3uPath, sysId);

                    // Attempt M3U creation
                    bool create = CreateM3uPlaylist(ordered, m3uPath, false);

                    if (create == false)
                    {
                        // user cancelled import, return null
                        return null;
                    }
                    else
                    {
                        // method returned true - begin to import m3u to games library
                        return mf;
                    }
                }
                else
                {
                    // single file selected - create GameFile object and return it
                    DiscGameFile g = new DiscGameFile(files[0], sysId, true);
                    return g;
                }
            }
            else
            {
                // no files selected - return empty string
                return null;
            }
        }

        public void BeginManualImport(int sysId)
        {
            // Start manual import process for a game based on sysId
            DiscGameFile gameFile = SelectGameFile(sysId);
            if (gameFile == null)
            {
                MessageBox.Show("No valid file was selected", "MedLaunch: Error");
                return;
            }
            else
            {
                // Add or update the returned GameFile to the database
                Game g = InsertOrUpdateDisk(gameFile, sysId);
                SaveToDatabase();
                MessageBox.Show("Game: " + gameFile.FileName + " has added to (or updated in) the library", "MedLaunch: Import or Update Completed");

                // update GL view
                _App.GamesLibrary.AddUpdateEntry(g);

                // GameListBuilder.UpdateFlag();
            }

        }

        public Game ReturnMatchingGame(DiscGameFile f, int sysId)
        {
            Game lookupGame = new Game();

            // different kinds of lookup queries depending on system
            switch (sysId)
            {
                case 9:             // psx
                case 13:            // ss
                    // lookup with serial number
                    if (f.ExtraInfo != null && f.ExtraInfo != "")
                    {                        
                        lookupGame = (from g in Games
                                      where (g.systemId == sysId && g.gameName == f.GameName) || g.disks == f.ExtraInfo
                                      select g).SingleOrDefault();
                    }
                    else
                    {

                        lookupGame = (from g in Games
                                      where (g.systemId == sysId && g.gameName == f.GameName)
                                      select g).SingleOrDefault();
                    }  
                    break;

                default:            // everything else
                    lookupGame = (from g in Games
                                  where (g.systemId == sysId && g.gameName == f.GameName)
                                  select g).SingleOrDefault();
                    break;
            }

            if (lookupGame == null)
                return null;

            return lookupGame;
        }

        public Game PopulateGameFile(DiscGameFile f, int sysId, Game lookupGame)
        {
            Game newGame = new Game();
            string md5Hash = string.Empty;
            string serialNumber = string.Empty;
            string versionNumber = string.Empty;
            bool isNewGame = false;
            bool shouldAddUpdate = true;

            // sanity check
            if (f == null)
                return null;

            // filter DAT by systemId
            List<DATMerge> lookup = DATMerge.FilterByMedLaunchSystemId(DAT, sysId);            

            // get md5 hash of first disc cuefile
            if (f.Extension.ToLower() == ".m3u")
            {
                // playlist file - get first referenced cue
                string cuPath = ParseTrackSheet(f, CueType.m3u, sysId)[0].FullPath;
                md5Hash = Crypto.checkMD5(cuPath);
            }
            else
                md5Hash = Crypto.checkMD5(f.FullPath);

            // DAT lookup based on hash or serial
            DATMerge look = new DATMerge();

            // serial
            if (f.ExtraInfo != null && f.ExtraInfo != "")
                look = lookup.Where(a => a.OtherFlags == f.ExtraInfo).FirstOrDefault();

            if (look == null)
            {
                // no serial number located - lookup md5 in existing DAT structure
                look = (from a in lookup
                       where a.MD5 != null && a.MD5.ToUpper().Trim() == md5Hash.ToUpper().Trim()
                       select a).FirstOrDefault(); 
            }
                //look = lookup.Where(a => a.MD5.ToUpper().Trim() == md5Hash.ToUpper().Trim()).FirstOrDefault();

            //lookup.Where(a => a.MD5.ToUpper().Trim() == md5Hash.ToUpper().Trim() || a.OtherFlags.ToUpper().Trim() == f.ExtraInfo.ToUpper().Trim()).ToList();

            // populate / update game file
            if (lookupGame == null)
            {
                // no matching game found in database - create new
                isNewGame = true;
                newGame.configId = 1;
                newGame.isFavorite = false;
            }
            else
            {
                // modify existing game
                isNewGame = false;
                newGame = lookupGame;       
            }

            newGame.gameName = f.GameName;
            newGame.gamePath = f.FullPath;
            newGame.hidden = false;
            newGame.isDiskBased = true;
            newGame.systemId = sysId;
            newGame.disks = f.ExtraInfo;
            newGame.OtherFlags = f.ExtraInfo; // serial number to otherflags field for now as well

            if (look != null)
            {
                newGame.gameNameFromDAT = look.GameName;
                newGame.Publisher = look.Publisher;
                newGame.Developer = look.Developer;
                newGame.Year = look.Year;
                newGame.romNameFromDAT = look.RomName;
                newGame.Copyright = look.Copyright;
                newGame.Country = look.Country;
                newGame.DevelopmentStatus = look.DevelopmentStatus;
                newGame.Language = look.Language;
                newGame.CRC32 = look.MD5;
            }
                       

            // now add to added or update list
            if (isNewGame == true && shouldAddUpdate == true)
            {
                DisksToAdd.Add(newGame);
                AddedStats++;
                //GameListBuilder.UpdateFlag();
            }

            if (isNewGame == false && shouldAddUpdate == true)
            {
                DisksToUpdate.Add(newGame);
                UpdatedStats++;
               // GameListBuilder.UpdateFlag();
            }

            return newGame;
        }

        public Game InsertOrUpdateDisk(DiscGameFile f, int sysId)
        {
            string firstImage = string.Empty;
            string firstCue = string.Empty;
            // get first image filepath from the cue/ccd/m3u
            try
            {
                firstImage = ParseTrackSheetForImageFiles(f, sysId)[0].FullPath;
            }
            catch
            {
                // some problem obtaining the image file from the cue
                MessageBox.Show("There was an issue determining the disc image from the cue/ccd/toc file you are using (" + f.FileName + ").\nPlease check that the cuefile is pointing to a valid (case sensitive) location.\n\n Press OK to skip the import of this game and proceed...", "Disc Image Lookup Issue", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            firstCue = f.FullPath;

            // if m3u get the first cue
            if (f.Extension.ToLower() == ".m3u")
            {
                firstCue = ParseTrackSheet(f, CueType.m3u, sysId)[0].FullPath;
            }
            

            // lookup serial number from disc image
            string serial = string.Empty;

            if (sysId == 9) //psx
            {
                serial = MedDiscUtils.GetPSXSerial(firstCue);
                if (serial == null)
                    serial = "";
                f.ExtraInfo = serial;
            }
            if (sysId == 13) //ss
            {
                var ssInfo = MedDiscUtils.GetSSData(firstImage);
                if (ssInfo.SerialNumber != null && ssInfo.SerialNumber != "")
                    f.ExtraInfo = ssInfo.SerialNumber; // + "*/";
                /*
                if (ssInfo.Version != null && ssInfo.Version != "")
                    f.ExtraInfo += ssInfo.Version;
                    */
                
            }

            // check whether game already exists in the database
            Game chkGame = ReturnMatchingGame(f, sysId);

            // create a game file and process all details
            Game newGame = PopulateGameFile(f, sysId, chkGame);

            return newGame;
            
        }

        /// <summary>
        /// Examine a disc game folder and return a DiscGameFile List ( may contain singles or multiples depending on logic)
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static List<DiscGameFile> DetermineDiscFileFromSubFolder(string folderPath, int sysId)
        {
            List<DiscGameFile> list = new List<DiscGameFile>();
            DiscGameFile gdf = new DiscGameFile();

            // get all files in the folder (that have extensions we are interested in)
            List<string> cueFiles = (from a in FileAndFolder.GetFiles(folderPath, false)
                                     where a.ToLower().Contains(".m3u") ||
                                     a.ToLower().Contains(".cue") ||
                                     a.ToLower().Contains(".ccd") ||
                                     a.ToLower().Contains(".toc")
                                     select a).ToList();

            // count file types
            int m3uCount = cueFiles.Where(a => a.ToLower().Contains(".m3u")).ToList().Count();
            int cueCount = cueFiles.Where(a => a.ToLower().Contains(".cue")).ToList().Count();
            int ccdCount = cueFiles.Where(a => a.ToLower().Contains(".ccd")).ToList().Count();
            int tocCount = cueFiles.Where(a => a.ToLower().Contains(".toc")).ToList().Count();

            // check for m3u first and return this (first one) if found
            if (m3uCount > 0)
            {
                string m3uFile = cueFiles.Where(a => a.ToLower().Contains(".m3u")).ToList().First();
                gdf = new DiscGameFile(m3uFile, sysId);
                list.Add(gdf);
                return list;
            }

            // if we have got this far then no m3u was detected - now check for SINGLE cue files (denoting one game)
            if (cueCount == 1 && ccdCount == 0 && tocCount == 0)
            {
                string cueFile = cueFiles.Where(a => a.ToLower().Contains(".cue")).ToList().First();
                gdf = new DiscGameFile(cueFile, sysId);
                list.Add(gdf);
                return list;
            }

            // now check for single ccd
            if (ccdCount == 1 && cueCount == 0 && tocCount == 0)
            {
                string ccdFile = cueFiles.Where(a => a.ToLower().Contains(".ccd")).ToList().First();
                gdf = new DiscGameFile(ccdFile, sysId);
                list.Add(gdf);
                return list;
            }

            // now check for single toc
            if (tocCount == 1 && cueCount == 0 && ccdCount == 0)
            {
                string tocFile = cueFiles.Where(a => a.ToLower().Contains(".toc")).ToList().First();
                list.Add(gdf);
                return list;
            }

            /* done with m3us and single sheet files - now comes multiples logic */

            // get ALL sheet files (not including m3us)
            List<string> sheets = (from a in cueFiles
                                   where !a.ToLower().Contains(".m3u")
                                   select a).ToList();

            // instantiate working set
            List<DiscGameFile> working = new List<DiscGameFile>();

            List<string> disc1 = new List<string>();
            List<string> disc2 = new List<string>();
            List<string> disc3 = new List<string>();
            List<string> disc4 = new List<string>();
            List<string> disc5 = new List<string>();
            List<string> disc6 = new List<string>();

            // lookup based on disc number
            for (int i = 0; i < sheets.Count; i++)
            {
                for (int disc = 1; disc < 7; disc++)
                {
                    if (sheets[i].ToLower().Contains("disc " + disc) ||
                    sheets[i].ToLower().Contains("cd " + disc) ||
                    sheets[i].ToLower().Contains("d" + disc) ||
                    sheets[i].ToLower().Contains("c" + disc) ||
                    sheets[i].ToLower().Contains("cd" + disc) ||
                    sheets[i].ToLower().Contains("disk" + disc) ||
                    sheets[i].ToLower().Contains("disk " + disc) ||
                    sheets[i].ToLower().Contains("cd" + disc) ||
                    sheets[i].ToLower().Contains("disc" + disc))
                    {
                        if (disc == 1) { disc1.Add(sheets[i]); }
                        if (disc == 2) { disc2.Add(sheets[i]); }
                        if (disc == 3) { disc3.Add(sheets[i]); }
                        if (disc == 4) { disc4.Add(sheets[i]); }
                        if (disc == 5) { disc5.Add(sheets[i]); }
                        if (disc == 6) { disc6.Add(sheets[i]); }

                    }
                }

            }
            List<List<string>> combined = new List<List<string>>
            {
                disc1, disc2, disc3, disc4, disc5, disc6
            };

            // now loop through the combined object
            for (int disc = 0; disc < 6; disc++)
            {
                // if disc 1 is not present, then dont go any further
                if (combined[0].Count == 0)
                    break;

                // if there is a single disc only per disc list then add it to working
                if (combined[disc].Count == 1)
                    working.Add(new DiscGameFile(combined[disc].First(), sysId));

                // if there are multiple discs per disc list then we have to loop through each of them and check whether they are valid
                if (combined[disc].Count > 1)
                {
                    foreach (string s in combined[disc])
                    {
                        string test = null;

                        if (Path.GetExtension(s).ToLower() == ".cue") { test = ParseNonM3UTrackSheetString(s, CueType.cue, sysId); }
                        if (Path.GetExtension(s).ToLower() == ".ccd") { test = ParseNonM3UTrackSheetString(s, CueType.ccd, sysId); }
                        if (Path.GetExtension(s).ToLower() == ".toc") { test = ParseNonM3UTrackSheetString(s, CueType.toc, sysId); }
                        

                        // continue if nothing returned
                        if (test == null || test == "")
                            continue;

                        // prefer cue
                        if (Path.GetExtension(s).ToLower() == ".cue")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                        // then ccd
                        if (Path.GetExtension(s).ToLower() == ".ccd")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                        // then toc
                        if (Path.GetExtension(s).ToLower() == ".toc")
                        {
                            working.Add(new DiscGameFile(s, sysId));
                            break;
                        }
                    }
                }
            }

            // now - working should contain all disc(s) for this particular game
            return working;

        }

        /// <summary>
        /// checks the contents of a sheet file (m3u, cue, etc) - returns true if files it is pointing to exist
        /// </summary>
        /// <param name="sheetPath"></param>
        /// <returns></returns>
        public static bool IsSheetValid(string sheetPath)
        {
            DiscGameFile dgf = new DiscGameFile();
            switch (Path.GetExtension(sheetPath).ToLower())
            {
                case ".m3u":
                    break;

                case ".cue":
                    break;

                case ".ccd":
                    break;
            }

            return false;
        }

        public static string ParseNonM3UTrackSheetString(string trackSheet, CueType sheetType, int systemId)
        {
            List<DiscGameFile> r = ParseTrackSheet(new DiscGameFile(trackSheet, systemId), sheetType, systemId);
            if (r.Count == 0)
                return null;
            else
            {
                return r.First().FullPath;
            }
        }

        /// <summary>
        /// Takes a cue, m3u, ccd or toc and returns a List<DiscGameFile> object containing all the referenced (or implied) files
        /// </summary>
        /// <param name="trackSheet"></param>
        /// <param name="sheetType"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public static List<DiscGameFile> ParseTrackSheet(DiscGameFile trackSheet, CueType sheetType, int systemId)
        {
            List<DiscGameFile> working = new List<DiscGameFile>();

            switch (sheetType)
            {
                case CueType.cue:
                    // get referenced image from cue file
                    string[] cFile = File.ReadAllLines(trackSheet.FullPath);
                    foreach (string l in cFile)
                    {
                        if (l == "" || l == " ")
                            continue;

                        if (l.Contains("FILE") || l.ToLower().Contains("file"))
                        {
                            // this is the line we are interested in
                            string filename = GetImageNameFromCue(l);

                            if (File.Exists(trackSheet.FolderPath + "\\" + filename))
                            {
                                DiscGameFile dgfc = new DiscGameFile(trackSheet.FolderPath + "\\" + filename, systemId);
                                working.Add(dgfc);
                            }                            
                            break;
                        }
                    }
                    break;

                case CueType.ccd:
                    // ccd files dont appear to have any reference to the image filename - im going to assume they just have to be named the same
                    string ccdFilePath = trackSheet.FullPath;
                    string imgFilePath = ccdFilePath.ToLower().Replace(".ccd", ".img");
                    
                    // check whether this file actuall exists
                    if (File.Exists(imgFilePath))
                    {
                        DiscGameFile dgfi = new DiscGameFile(imgFilePath, systemId);
                        working.Add(dgfi);
                    }
                    break;

                case CueType.toc:
                    // not implemented at the moment
                    break;

                case CueType.m3u:
                    // get all referenced files from m3u
                    string[] mFiles = File.ReadAllLines(trackSheet.FullPath);
                    foreach (string l in mFiles)
                    {
                        if (l == "" || l == " ")
                            continue;

                        // create a discgamefile
                        DiscGameFile dgf = new DiscGameFile(trackSheet.FolderPath + "\\" + l, systemId);
                        // add it to working list
                        working.Add(dgf);
                        break;
                    }
                    break;
            }

            return working;
        }

        public static string GetImageNameFromCue(string cueLine)
        {
            string bin = cueLine;

            if (bin.Contains('"'))
            {
                // path is encapsulated with ""
                bin = bin.Replace("FILE \"", "")
                    .Replace("File \"", "")
                    .Replace("file \"", "")
                    .Replace("\" BINARY", "")
                    .Replace("\" Binary", "")
                    .Replace("\" binary", "");
                
            }
            else
            {
                // path does not contain ""
                string[] arr = bin.Split(' ');
                // build a new string leaving out first and last array elements
                string newStr = "";
                for (int i = 1; i < arr.Length - 1; i++)
                {
                    if (i > 1)
                        newStr += " ";

                    newStr += arr[i];

                    if (i < arr.Length - 2)
                        newStr += " ";

                    bin = newStr;
                }
            }

            return bin;
        }

        public static List<DiscGameFile> ParseTrackSheetForImageFiles(DiscGameFile trackSheet, int systemId)
        {
            CueType cueType = new CueType();
            if (trackSheet.FullPath.ToLower().Contains(".m3u"))
                cueType = CueType.m3u;
            if (trackSheet.FullPath.ToLower().Contains(".cue"))
                cueType = CueType.cue;
            if (trackSheet.FullPath.ToLower().Contains(".ccd"))
                cueType = CueType.ccd;
            if (trackSheet.FullPath.ToLower().Contains(".toc"))
                cueType = CueType.toc;

            List<DiscGameFile> working = new List<DiscGameFile>();

            switch (cueType)
            {
                case CueType.cue:
                    // get referenced image from cue file
                    string[] cFile = File.ReadAllLines(trackSheet.FullPath);
                    foreach (string l in cFile)
                    {
                        if (l == "" || l == " ")
                            continue;

                        if (l.Contains("FILE") || l.ToLower().Contains("file"))
                        {
                            // this is the line we are interested in
                            string filename = GetImageNameFromCue(l);

                            if (File.Exists(trackSheet.FolderPath + "\\" + filename))
                            {
                                DiscGameFile dgfc = new DiscGameFile(trackSheet.FolderPath + "\\" + filename, systemId);
                                working.Add(dgfc);
                            }
                            break;
                        }
                    }
                    break;

                case CueType.ccd:
                    // ccd files dont appear to have any reference to the image filename - im going to assume they just have to be named the same
                    string ccdFilePath = trackSheet.FullPath;
                    string imgFilePath = ccdFilePath.ToLower().Replace(".ccd", ".img");

                    // check whether this file actuall exists
                    if (File.Exists(imgFilePath))
                    {
                        DiscGameFile dgfi = new DiscGameFile(imgFilePath, systemId);
                        working.Add(dgfi);
                    }
                    break;

                case CueType.toc:
                    // not implemented at the moment
                    break;

                case CueType.m3u:
                    // get all referenced cue files from m3u
                    string[] mFiles = File.ReadAllLines(trackSheet.FullPath);
                    foreach (string l in mFiles)
                    {
                        if (l == "" || l == " ")
                            continue;

                        // we actually want to return referenced image files, so recursively pass these back
                        List<DiscGameFile> rec = ParseTrackSheetForImageFiles(new DiscGameFile(trackSheet.FolderPath + "\\" + l, systemId), systemId);
                        if (rec.Count > 0)
                            working.AddRange(rec);
                    }
                    break;
            }

            return working;
        }

        public static string[] ParseM3UFile(string path)
        {
            if (!File.Exists(path))
                return null;

            string[] arr = (from line in File.ReadAllLines(path).Where(item => !String.IsNullOrWhiteSpace(item))
                           select line).ToArray();

            return arr;                        
               
        }

        public static bool CreateM3uPlaylist(List<DiscGameFile> files, string m3uPath, bool overwrite)
        {
            // does the file already exist
            if (!File.Exists(m3uPath))
            {
                // file does not exist - create file and populate
                using (StreamWriter sw = File.CreateText(m3uPath))
                {
                    foreach (var f in files)
                    {
                        sw.WriteLine(f.FileName);
                    }
                }
                return true;
            }
            else
            {
                if (overwrite == false)
                {
                    // file already exists - check whether we want to overwrite or not
                    MessageBoxResult result = MessageBox.Show("File Name: " + Path.GetFileName(m3uPath) + "\n\nDo you want to replace this file?\n(Yes overwrites, No uses existing file, Cancel aborts the import process)", "M3U Playlist File Already Exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        // dont overwrite, just return
                        return true;
                    }
                    if (result == MessageBoxResult.Cancel)
                    {
                        // cancel import process
                        return false;
                    }
                    if (result == MessageBoxResult.Yes)
                    {
                        // overwrite

                        // clear file first
                        File.Create(m3uPath).Close();

                        // create and populate file
                        using (StreamWriter sw = File.CreateText(m3uPath))
                        {
                            foreach (var f in files)
                            {
                                sw.WriteLine(f.FileName);
                            }
                        }
                        return true;
                    }
                }

                else
                {
                    // clear file first
                    File.Create(m3uPath).Close();

                    // create and populate file
                    using (StreamWriter sw = File.CreateText(m3uPath))
                    {
                        foreach (var f in files)
                        {
                            sw.WriteLine(f.FileName);
                        }
                    }
                    return true;
                }

                return false;
            }
        }
    }

    public enum CueType
    {
        cue,
        ccd,
        toc,
        m3u
    }

    public class PsxName
    {
        public string serial { get; set; }
        public string name { get; set; }
    }
}
