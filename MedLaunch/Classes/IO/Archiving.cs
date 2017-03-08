using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Classes;
using System.Security.Cryptography;
using MedLaunch.Models;
using System.IO;

namespace MedLaunch.Classes.IO
{
    public class Archiving
    {
        // properties
        public string ArchivePath { get; set; }
        public string InternalGamePath { get; set; }
        public int SystemId { get; set; }
        public bool IsAllowed { get; set; }
        public string ArchiveExtension { get; set; }
        public string MD5Hash { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public bool IsSingleFileInArchive { get; set; }

        public static List<Archiving> ArchiveMultiple { get; set; }

        // constructors
        public Archiving(string archivePath, int systemId)
        {
            //if (ArchiveMultiple == null)
            ArchiveMultiple = new List<Archiving>();

            IsAllowed = false;
            ArchivePath = archivePath;
            SystemId = systemId;
            ArchiveExtension = System.IO.Path.GetExtension(ArchivePath).ToLower();
            IsSingleFileInArchive = false;
        }

        public Archiving(string _hash, string filename, string archivePath, int systemId)
        {
            IsAllowed = true;
            ArchivePath = archivePath;
            SystemId = systemId;
            ArchiveExtension = System.IO.Path.GetExtension(ArchivePath).ToLower();
            IsSingleFileInArchive = false;
            FileName = filename;
            MD5Hash = _hash;
        }

        /* methods */
        

        

        /// <summary>
        /// Process the selected archive
        /// Identified the first allowed file based on systemid and populates FileName and MD5Hash properties
        /// </summary>
        public void ProcessArchive()
        {
            // determine archive type
            if (ArchiveExtension == ".zip")
            {
                using (ZipArchive zip = ZipFile.OpenRead(ArchivePath))
                {
                    // count the number of allowed files
                    int allowedCount = 0;
                    foreach (var t in zip.Entries)
                    {
                        if (GSystem.IsFileAllowed(t.FullName, SystemId) == true)
                        {
                            if (t.FullName.ToLower().Contains(".zip"))
                                continue;

                            if (!t.FullName.ToLower().Contains(".7z"))
                                continue;

                            allowedCount++;
                        }
                    }
                    if (allowedCount == 1)
                    {
                        // if only one allowed file is detected in the archive - set the flag
                        IsSingleFileInArchive = true;
                    }                    

                    // iterate through each entry
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        // if this file is actually an archive then skip it
                        if (entry.FullName.ToLower().Contains(".zip") ||
                            entry.FullName.ToLower().Contains(".7z"))
                        {
                            continue;
                        }

                        if (GSystem.IsFileAllowed(entry.FullName, SystemId) == true)
                        {
                            IsAllowed = true;

                            using (var md5 = MD5.Create())
                            {
                                using (var stream = entry.Open())
                                {
                                    string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                                    this.MD5Hash = h;
                                    this.FileName = entry.FullName;

                                    ArchiveMultiple.Add(new Archiving(h, entry.FullName, ArchivePath, SystemId));
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                
            }

            if (ArchiveExtension == ".7z")
            {
                var archive = ArchiveFactory.Open(ArchivePath);

                int allowedCount = 0;
                foreach (var t in archive.Entries)
                {
                    if (GSystem.IsFileAllowed(t.Key, SystemId) == true)
                    {
                        if (t.Key.ToLower().Contains(".zip"))
                            continue;

                        if (!t.Key.ToLower().Contains(".7z"))
                            continue;

                        allowedCount++;
                    }
                }

                if (allowedCount == 1)
                {
                    // if only one allowed file is detected in the archive - set the flag
                    //IsSingleFileInArchive = true; // dont do this for 7z as they need to be extracted anyways
                }

                foreach (SevenZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                        continue;

                    // if this file is actually an archive then skip it
                    if (entry.Key.ToLower().Contains(".zip") ||
                        entry.Key.ToLower().Contains(".7z"))
                    {
                        continue;
                    }

                    if (GSystem.IsFileAllowed(entry.Key, SystemId) == true && !entry.IsDirectory)
                    {
                        IsAllowed = true;

                        using (var md5 = MD5.Create())
                        {
                            using (var stream = entry.OpenEntryStream())
                            {
                                string h = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                                MD5Hash = h;
                                FileName = entry.Key;
                                FileSize = entry.Size;

                                ArchiveMultiple.Add(new Archiving(h, entry.Key, ArchivePath, SystemId));
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }                
            }
        }

        /// <summary>
        /// Extract the contents of the archive file to a specific directory
        /// </summary>
        /// <param name="destinationDirectory"></param>
        public void ExtractArchive(string destinationDirectory)
        {
            // determine archive type
            if (ArchiveExtension == ".zip")
            {
                ZipFile.ExtractToDirectory(ArchivePath, destinationDirectory);
            }

            if (ArchiveExtension == ".7z")
            {
                var archive = ArchiveFactory.Open(ArchivePath);
                foreach (var entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                        continue;

                    entry.WriteToDirectory(destinationDirectory, new SharpCompress.Readers.ExtractionOptions() { Overwrite = true });
                }
            }
        }


        public static List<string> GetSbiListFrom7z(string path)
        {
            List<string> tmp = new List<string>();

            if (Path.GetExtension(path) == ".7z")
            {
                var archive = ArchiveFactory.Open(path);
                foreach (SevenZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.IsDirectory)
                        continue;

                    if (!entry.Key.ToLower().Contains(".7z"))
                        continue;

                    tmp.Add(entry.Key);
                }
            }
            return tmp;
        }

        public static string SetupArchiveChild(string archivePath, string archiveFile, string outputDir)
        {
            string romPath = string.Empty;

            IArchive archive = ArchiveFactory.Open(archivePath);

            if (archive.Type == SharpCompress.Common.ArchiveType.SevenZip)
            {
                SevenZipArchive sev = (SevenZipArchive) archive;
                SevenZipArchiveEntry rom = (from a in sev.Entries
                                            where a.Key == archiveFile
                                            select a).FirstOrDefault();
                if (rom != null)
                {
                    try
                    {
                        rom.WriteToDirectory(outputDir, new SharpCompress.Readers.ExtractionOptions() { Overwrite = true });
                    }
                    catch { System.IO.IOException ex; }

                    return outputDir + "\\" + rom.Key;
                }
                          
            }
            if (archive.Type == SharpCompress.Common.ArchiveType.Zip)
            {
                SharpCompress.Archives.Zip.ZipArchive zip = (SharpCompress.Archives.Zip.ZipArchive) archive;
                SharpCompress.Archives.Zip.ZipArchiveEntry rom2 = (from a in zip.Entries
                                                                   where a.Key == archiveFile
                                                                   select a).FirstOrDefault();

                if (rom2 != null)
                {
                    try
                    {
                        rom2.WriteToDirectory(outputDir, new SharpCompress.Readers.ExtractionOptions() { Overwrite = true });
                    }

                    catch { System.IO.IOException ex; }
                    
                    return outputDir + "\\" + rom2.Key;
                }
            }

            return romPath;
        }
    }
}
