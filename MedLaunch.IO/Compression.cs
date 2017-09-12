using SharpPhysFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.IO
{
    public class Compression
    {
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
        /// Extracts files from an archive
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

        public static CompressionResults ProcessArchive(string archivePath, string[] allowedExtensions)
        {
            CompressionResults crs = new CompressionResults(archivePath);

            // if file does not exist
            if (!File.Exists(archivePath))
                return null;

            // generate random mount-point name
            string mnt = Crypto.GetNotStrongRandomPhrase(8);

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
                        string hash = Crypto.GetMD5Hash(reader.BaseStream);
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

    public class CompressionResults
    {
        public List<CompressionResult> Results { get; set; }
        public string ArchivePath { get; set; }
        public string ArchiveMD5 { get; set; }

        public CompressionResults(string archivePath)
        {
            Results = new List<CompressionResult>();
            ArchivePath = archivePath;
            ArchiveMD5 = Crypto.GetMD5Hash(ArchivePath);
        }
    }

    public class CompressionResult
    {
        public string FileName { get; set; }
        public string InternalPath { get; set; }
        public string ArchivePath { get; set; }
        public string DBPathString { get; set; }
        public string MD5 { get; set; }
        public string CRC32 { get; set; }

        public CompressionResult CalculateDBPathString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ArchivePath);
            sb.Append("*/");
            sb.Append(InternalPath);
            DBPathString = sb.ToString();
            return this;
        }
    }
}
