using SharpPhysFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLaunch.Common.Eventing.CustomEventArgs;
using MedLaunch.Common.Crypto;
using MedLaunch.Common.Streams;

namespace MedLaunch.Common.IO.Compression
{
    public class Archive
    {
        /* Properties */
        public string ArchivePath { get; set; }

        public delegate void MessageHandler(Archive a, ProgressDialogEventArgs e);
        public event MessageHandler Message;

        /* Constructors */
        public Archive()
        {

        }

        public Archive(string archivePath)
        {
            if (File.Exists(archivePath))
                ArchivePath = archivePath;
        }

        /* Methods */

        public void FireMessageEvent(ProgressDialogEventArgs message)
        {
            if (Message != null)
            {
                //ProgressDialogEventArgs status = new ProgressDialogEventArgs();
               // message.DialogText = message;
                Message(this, message);
            }
        }


        /* Static Methods */

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
        /// Extracts all files from an archive
        /// </summary>
        /// <param name="archivePath"></param>
        /// <param name="internalPath"></param>
        /// <param name="outputDirectory"></param>
        /// <returns></returns>
        public static string[] ExtractFile(string archivePath, string[] internalPath, string outputDirectory)
        {
            List<string> outputs = new List<string>();

            // check whether archive and output directory exists
            if (!File.Exists(archivePath) || !File.Exists(outputDirectory))
                return new string[] { "" };

            // generate random mount-point name
            string mnt = Converters.GetNotStrongRandomPhrase(8);

            // open the archive
            using (var pfs = new PhysFS(""))
            {
                // set write dir
                pfs.SetWriteDir(outputDirectory);

                // mount the archive
                pfs.Mount(archivePath, mnt, false);

                // extract the files
                foreach (string s in internalPath)
                {
                    string intStr = mnt + "/" + s;
                    // test whether file exists
                    if (pfs.Exists(intStr))
                    {
                        using (var reader = new BinaryReader(pfs.OpenRead(intStr)))
                        {
                            // save to disk
                            try
                            {
                                StreamTools.SaveStreamToDisk(reader.BaseStream, pfs.GetWriteDir() + "\\" + s);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }

                            // add destination path to list
                            outputs.Add(pfs.GetWriteDir() + "\\" + s);
                        }
                    }
                }

                if (outputs.Count() == 0)
                {
                    return new List<string> { "" }.ToArray();
                }

                return outputs.ToArray();
            }
        }
        /*
        public static string[] ExtractEntireArchive(string archivePath, string outputDirectory, bool maintainFolderStructure)
        {
            List<string> outputs = new List<string>();

            // check whether archive and output directory exists
            if (!File.Exists(archivePath) || !File.Exists(outputDirectory))
                return new string[] { "" };

            // generate random mount-point name
            string mnt = Crypto.GetNotStrongRandomPhrase(8);

            // open the archive
            using (var pfs = new PhysFS(""))
            {
                // set write dir
                pfs.SetWriteDir(outputDirectory);

                // mount the archive
                pfs.Mount(archivePath, mnt, false);

                // extract the files
                foreach (string s in internalPath)
                {
                    string intStr = mnt + "/" + s;
                    // test whether file exists
                    if (pfs.Exists(intStr))
                    {
                        using (var reader = new BinaryReader(pfs.OpenRead(intStr)))
                        {
                            // save to disk
                            try
                            {
                                Streams.SaveStreamToDisk(reader.BaseStream, pfs.GetWriteDir() + "\\" + s);
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }

                            // add destination path to list
                            outputs.Add(pfs.GetWriteDir() + "\\" + s);
                        }
                    }
                }

                if (outputs.Count() == 0)
                {
                    return new List<string> { "" }.ToArray();
                }

                return outputs.ToArray();
            }
        }
        */

        

        public CompressionResults ProcessArchive(string[] allowedExtensions)
        {
            CompressionResults crs = new CompressionResults(ArchivePath);

            // if file does not exist
            if (!File.Exists(ArchivePath))
                return null;

            // generate random mount-point name
            string mnt = Converters.GetNotStrongRandomPhrase(8);

            // open the archive
            using (var pfs = new PhysFS(""))
            {
                // set write dir
                //pfs.SetWriteDir(outputDirectory);

                // mount the archive
                pfs.Mount(ArchivePath, mnt, false);

                // build the internal structure
                var structure = GetArchiveStructure(pfs, mnt);

                // iterate through each file
                foreach (var s in structure)
                {
                    // check whether it is an allowed file
                    bool allowed = false;
                    foreach (var al in allowedExtensions)
                    {
                        if (s.ToUpper().EndsWith(al.ToUpper()))
                        {
                            allowed = true;
                            break;
                        }
                    }

                    if (allowed == false)
                        continue;

                    // get MD5 hash and build compressionresult object
                    using (var reader = new BinaryReader(pfs.OpenRead(s)))
                    {
                        string hash = Converters.GetMD5Hash(reader.BaseStream);
                        CompressionResult cr = new CompressionResult();
                        cr.ArchivePath = ArchivePath;
                        cr.FileName = s;
                        //cr.InternalPath = s.TrimStart((mnt).ToCharArray()).TrimStart('/').Replace("", "");
                        cr.InternalPath = s.Replace(mnt, "").TrimStart('/');
                        cr.MD5 = hash;
                        cr.CRC32 = pfs.GetHashCode().ToString();
                        cr.CalculateDBPathString();

                        crs.Results.Add(cr);

                        // build status message
                        StringBuilder sb1 = new StringBuilder();
                        sb1.Append("Scanning: " + Path.GetFileName(ArchivePath) + "\n\n");
                        sb1.Append("Processing: " + cr.InternalPath);

                        ProgressDialogEventArgs eva = new ProgressDialogEventArgs();
                        eva.DialogText = sb1.ToString();
                        FireMessageEvent(eva);
                        //string newtest = cr.InternalPath;
                       
                            
                    }
                }

                return crs;
            }
        }

        public static CompressionResults ProcessArchive(string archivePath, string[] allowedExtensions)
        {
            CompressionResults crs = new CompressionResults(archivePath);

            // if file does not exist
            if (!File.Exists(archivePath))
                return null;

            // generate random mount-point name
            string mnt = Converters.GetNotStrongRandomPhrase(8);

            // open the archive
            using (var pfs = new PhysFS(""))
            {
                // set write dir
                //pfs.SetWriteDir(outputDirectory);

                // mount the archive
                pfs.Mount(archivePath, mnt, false);

                // build the internal structure
                var structure = GetArchiveStructure(pfs, mnt);

                // iterate through each file
                foreach (var s in structure)
                {
                    // check whether it is an allowed file
                    bool allowed = false;
                    foreach (var al in allowedExtensions)
                    {
                        if (s.ToUpper().EndsWith(al.ToUpper()))
                        {
                            allowed = true;
                            break;
                        }
                    }

                    if (allowed == false)
                        continue;                   

                    // get MD5 hash and build compressionresult object
                    using (var reader = new BinaryReader(pfs.OpenRead(s)))
                    {  
                        string hash = Converters.GetMD5Hash(reader.BaseStream);
                        CompressionResult cr = new CompressionResult();
                        cr.ArchivePath = archivePath;
                        cr.FileName = s;
                        cr.InternalPath = s.TrimStart((mnt + "/").ToCharArray());
                        cr.MD5 = hash;
                        cr.CRC32 = pfs.GetHashCode().ToString();
                        cr.CalculateDBPathString();

                        crs.Results.Add(cr);
                    }
                }

                return crs;                
            }
        }

        public static List<string> GetArchiveStructure(PhysFS pfs, string directoryLevel)
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
    }
}
