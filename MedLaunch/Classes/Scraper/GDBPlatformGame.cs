using System;
using MedLaunch.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace MedLaunch.Models
{
    public class GDBPlatformGame
    { 
        public int id { get; set; }
        public int SystemId { get; set; }
        public string GDBPlatformName { get; set; }
        //public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string ReleaseDate { get; set; }
    }
}
