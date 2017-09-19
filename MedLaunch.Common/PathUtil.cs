using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Common
{
    public class PathUtil
    {
        static public string NormalizeFilepath(string filepath)
        {
            string result = System.IO.Path.GetFullPath(filepath).ToLowerInvariant();

            result = result.TrimEnd(new[] { '\\' });

            return result;
        }

        public static string GetRelativePath(string rootPath, string fullPath)
        {
            rootPath = NormalizeFilepath(rootPath);
            fullPath = NormalizeFilepath(fullPath);

            if (!fullPath.StartsWith(rootPath))
                throw new Exception("Could not find rootPath in fullPath when calculating relative path.");

            return "." + fullPath.Substring(rootPath.Length);
        }
    }
}
