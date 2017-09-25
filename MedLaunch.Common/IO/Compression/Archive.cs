using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common.Eventing.CustomEventArgs;
using MedLaunch.Common.Crypto;
using MedLaunch.Common.Streams;
using System.IO.Compression;
using System.Security.Cryptography;
using SevenZip;

namespace MedLaunch.Common.IO.Compression
{
    public class Archive
    {
        /* Properties */
        public string ArchivePath { get; set; }
        public int AllowedFilesDetected { get; set; }

        public delegate void MessageHandler(Archive a, ProgressDialogEventArgs e);
        public event MessageHandler Message;

        /* Constructors */
        public Archive()
        {
            AllowedFilesDetected = 0;
        }

        public Archive(string archivePath)
        {
            if (File.Exists(archivePath))
                ArchivePath = archivePath;

            AllowedFilesDetected = 0;
        }

        /* Methods */

        private void FireMessageEvent(ProgressDialogEventArgs message)
        {
            if (Message != null)
            {
                //ProgressDialogEventArgs status = new ProgressDialogEventArgs();
               // message.DialogText = message;
                Message(this, message);
            }
        }

        public CompressionResults ProcessArchive(string[] allowedExtensions)
        {
            CompressionResults crs = new CompressionResults(ArchivePath);
            
            // if file does not exist
            if (!File.Exists(ArchivePath))
                return null;

            // mount the archive

            using (Stream archiveStream = File.OpenRead(ArchivePath))
            {
                using (var ar = new SevenZipExtractor(archiveStream))
                {
                    var structure = ar.ArchiveFileData;

                    List<ArchiveFileInfo> allowedFiles = new List<ArchiveFileInfo>();

                    foreach (var s in structure)
                    {
                        if (s.IsDirectory == true)
                            continue;

                        string fileName = s.FileName;
                        string extension = Path.GetExtension(fileName);

                        if (allowedExtensions == null)
                        {
                            allowedFiles.Add(s);
                            continue;
                        }

                        foreach (var ext in allowedExtensions)
                        {
                            if (ext == extension)
                            {
                                allowedFiles.Add(s);
                                break;
                            }
                        }
                    }

                    AllowedFilesDetected = allowedFiles.Count();

                    // now we should have a list of allowed files
                    foreach (var file in allowedFiles)
                    {
                        CompressionResult cr = new CompressionResult();
                        cr.ArchivePath = ArchivePath;
                        cr.InternalPath = file.FileName.Replace("\\", "/");
                        cr.FileName = cr.InternalPath.Split('/').LastOrDefault();
                        cr.CRC32 = file.Crc.ToString("X");
                        cr.Extension = Path.GetExtension(cr.FileName);
                        cr.RomName = cr.FileName.Replace(cr.Extension, "");

                        //cr.CalculateDBPathString();
                        /*
                        if (Path.GetExtension(ArchivePath).ToLower() == ".zip")
                        {
                            // calculate md5 hash
                            using (var stream = new MemoryStream())
                            {
                                ar.ExtractFile(file.Index, stream);
                                byte[] data = new byte[stream.Length];
                                data = StreamTools.ReadToEnd(stream);

                                using (var md5 = MD5.Create())
                                {
                                    string hash = BitConverter.ToString(md5.ComputeHash(data)).Replace("-", string.Empty);
                                    cr.MD5 = hash;
                                }
                            }
                        }
                        */

                        // 7zip and zip extraction too slow, use CRC32 instead
                        cr.MD5 = cr.CRC32;

                        crs.Results.Add(cr);

                        // build status message
                        StringBuilder sb1 = new StringBuilder();
                        sb1.Append("Scanning: " + Path.GetFileName(ArchivePath) + "\n\n");
                        sb1.Append("Processing: " + cr.InternalPath);

                        ProgressDialogEventArgs eva = new ProgressDialogEventArgs();
                        eva.DialogText = sb1.ToString();
                        FireMessageEvent(eva);
                    }
                }
            }

               // var ar = new SevenZipExtractor(ArchivePath);
            
            
            return crs;
        }


        /* Static Methods */

        /// <summary>
        /// Extract a single file from an archive using an archivestream
        /// </summary>
        /// <param name="archiveStream"></param>
        /// <param name="internalFileName"></param>
        /// <param name="outputDirectory"></param>
        private static string Extract(Stream archiveStream, string internalFileName, string outputDirectory)
        {
            using (SevenZipExtractor extr = new SevenZipExtractor(archiveStream))
            {
                foreach (ArchiveFileInfo archiveFileInfo in extr.ArchiveFileData.Where(a => a.FileName == internalFileName))
                {
                    if (!archiveFileInfo.IsDirectory)
                    {
                        using (var mem = new MemoryStream())
                        {
                            extr.ExtractFile(archiveFileInfo.Index, mem);

                            string shortFileName = Path.GetFileName(archiveFileInfo.FileName);
                            byte[] content = mem.ToArray();
                            File.WriteAllBytes(outputDirectory + @"\" + shortFileName, content);

                            return outputDirectory + @"\" + shortFileName;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Extract a single file from an archive using an archivestream and rename it
        /// </summary>
        /// <param name="archiveStream"></param>
        /// <param name="internalFileName"></param>
        /// <param name="outputDirectory"></param>
        public static string Extract(Stream archiveStream, int index, string newFileName, string outputDirectory)
        {
            using (SevenZipExtractor extr = new SevenZipExtractor(archiveStream))
            {
                foreach (ArchiveFileInfo archiveFileInfo in extr.ArchiveFileData.Where(a => a.Index == index))
                {
                    if (!archiveFileInfo.IsDirectory)
                    {
                        using (var mem = new MemoryStream())
                        {
                            extr.ExtractFile(archiveFileInfo.Index, mem);

                            string shortFileName = Path.GetFileName(archiveFileInfo.FileName);
                            byte[] content = mem.ToArray();
                            File.WriteAllBytes(outputDirectory + @"\" + newFileName, content);

                            return outputDirectory + @"\" + shortFileName;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Extract a single file (as a stream) from an archive using an archivestream based on internal filename
        /// </summary>
        /// <param name="archiveStream"></param>
        /// <param name="internalFileName"></param>
        /// <param name="outputDirectory"></param>
        public static Stream ExtractAsStream(Stream archiveStream, string internalFileName)
        {
            MemoryStream ms = new MemoryStream();

            using (SevenZipExtractor extr = new SevenZipExtractor(archiveStream))
            {
                foreach (ArchiveFileInfo archiveFileInfo in extr.ArchiveFileData.Where(a => a.FileName == internalFileName))
                {
                    if (!archiveFileInfo.IsDirectory)
                    {
                        using (var mem = new MemoryStream())
                        {
                            extr.ExtractFile(archiveFileInfo.Index, mem);

                            mem.CopyTo(ms);
                            return ms;
                            
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Extract a single file (as a stream) from an archive using an archivestream based on internal index
        /// </summary>
        /// <param name="archiveStream"></param>
        /// <param name="internalFileName"></param>
        /// <param name="outputDirectory"></param>
        public static Stream ExtractAsStream(Stream archiveStream, int index)
        {
            MemoryStream ms = new MemoryStream();

            using (SevenZipExtractor extr = new SevenZipExtractor(archiveStream))
            {
                foreach (ArchiveFileInfo archiveFileInfo in extr.ArchiveFileData.Where(a => a.Index == index))
                {
                    if (!archiveFileInfo.IsDirectory)
                    {
                        using (var mem = new MemoryStream())
                        {
                            extr.ExtractFile(archiveFileInfo.Index, mem);

                            mem.CopyTo(ms);
                            return ms;

                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Extracts a specific file from an archive
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="internalPath"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        public static string ExtractFile(string archivePath, string internalPath, string outputDirectory)
        {
            string[] result = ExtractFile(archivePath, new string[] { internalPath }, outputDirectory);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Extracts multiple files from an archive
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="internalPath"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        public static string[] ExtractFile(string archivePath, string[] internalPath, string outputDirectory)
        {
            List<string> outputs = new List<string>();

            // check whether archive and output directory exists
            if (!File.Exists(archivePath) || !Directory.Exists(outputDirectory))
                return new string[] { "" };

            // open the archive
            using (Stream archiveStream = File.OpenRead(archivePath))
            {
                foreach (var s in internalPath)
                {
                    string internalConverted = s.Replace("/", "\\");
                    string extractedFilePath = Extract(archiveStream, internalConverted, outputDirectory);

                    if (extractedFilePath != null)
                        outputs.Add(extractedFilePath);
                }
            }

            if (outputs.Count() == 0)
            {
                return new List<string> { "" }.ToArray();
            }

            return outputs.ToArray();           
        }        

        /// <summary>
        /// extract entire zip archive using sevenzipsharp
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="outputDirectory"></param>
        /// <param name="maintainFolderStructure"></param>
        /// <returns></returns>
        public static void ExtractEntireZip(string archivePath, string outputDirectory)
        {
            try
            {
                using (SevenZipExtractor archive = new SevenZipExtractor(archivePath))
                {
                    archive.ExtractArchive(outputDirectory);
                }

                /*
                using (ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(outputDirectory, true);
                }
                */
            }
            catch
            {
                return;
            }
            
        }

        /*
        private static List<string> GetArchiveStructure(PhysFS pfs, string directoryLevel)
        {
            List<string> structure = new List<string>();
            string level = directoryLevel;

            var test = pfs.GetSearchPath();

            // enumerate files
            var files = pfs.EnumerateFiles(level + "/");

            // iterate through
            foreach (var file in files)
            {
                if (pfs.IsDirectory(level + "/" + file))
                {
                    // file is a directory - recursively get all files/folder etc
                    var recurse = GetArchiveStructure(pfs, level + "/" + file);
                    // add the result to the master list
                    structure.AddRange(recurse);
                }
                else
                {
                    // file is a file
                    structure.Add(level + "/" + file);
                }
            }
            return structure;
        }
        */
    }
}
