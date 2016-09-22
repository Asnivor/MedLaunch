using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MedLaunch.Classes
{
    class SetupDirectories
    {
        // create required directories in application folder if they do not exist

        public static void Go()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] directories = new string[] {
                "Data",
                "Data\\Graphics",
                "Data\\Graphics\\Icons",
                "Data\\Graphics\\Systems",
                "Data\\Settings",
                "Data\\System"
            };

        foreach (string d in directories)
            {
                try
                {
                    string path = currentDirectory + "\\" + d;
                    Directory.CreateDirectory(path);
                }
                finally
                {
                    // completed
                }
                

            }


        }
    }
}
