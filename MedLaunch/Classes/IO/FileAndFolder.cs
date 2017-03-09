using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.IO
{
    public class FileAndFolder
    {
        public static void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }

        public static string[] StreamAllLines(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            List<string> list = new List<string>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        list.Add(line);
                    }
                }
            }

            return list.ToArray();
        }

        public static string StreamAllText(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            List<string> list = new List<string>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        list.Add(line);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (string l in list)
            {
                sb.Append(l).Append("\n");
            }

            return sb.ToString();
        }

        // return number of files found in a directory and sub-directory (based on usingRecursion bool)
        public static int CountFiles(string path, bool usingRecursion)
        {
            int fileCount = 0;
            try
            {
                if (usingRecursion == true)
                    fileCount = Directory.EnumerateFiles(@path, "*.*", SearchOption.AllDirectories).Count();
                else
                    fileCount = Directory.EnumerateFiles(@path, "*.*").Count();
            }
            catch { }

            return fileCount;
        }

        // get a list of files from a directory and sub-directory (based on usingRecursion bool)
        public static System.Collections.Generic.IEnumerable<string> GetFiles(string path, bool usingRecursion)
        {
            if (usingRecursion == true)
            {
                //MessageBoxResult result = MessageBox.Show(path);

                // check first whether directory exists
                if (Directory.Exists(@path))
                {
                    var files = Directory.GetFiles(@path, "*.*", SearchOption.AllDirectories);
                    return files;
                }
                else
                {
                    // directory no longer exists - return null
                    return null;
                }

            }
            else
            {
                var files = Directory.GetFiles(@path, "*.*");
                return files;
            }

        }
    }
}
