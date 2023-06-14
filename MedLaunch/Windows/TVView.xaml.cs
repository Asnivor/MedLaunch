using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.IO;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Migrations;
using MedLaunch.Models;
using MedLaunch.Classes;
using MedLaunch.Common;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
using System.Threading;
using Medlaunch.Classes;
using System.Net;
using MedLaunch.Classes.MasterScraper;
using MedLaunch.Classes.TheGamesDB;
using MedLaunch.Classes.Scraper;
using MahApps.Metro;
using MedLaunch.Classes.Input;
using MedLaunch.Classes.GamesLibrary;
using System.Collections.ObjectModel;
using MedLaunch.Classes.Scraper.DAT.TOSEC.Models;
using MedLaunch.Classes.Scraper.DAT.NOINTRO.Models;
using MedLaunch.Classes.Scraper.DAT.Models;
using MedLaunch.Classes.Scraper.DAT.TRURIP.Models;
using MedLaunch.Classes.Scraper.DAT.REDUMP.Models;
using MedLaunch.Classes.VisualHandlers;
using Newtonsoft.Json.Linq;
using MedLaunch.Classes.IO;
using MahApps.Metro.SimpleChildWindow;
using MedLaunch.Classes.Controls.VirtualDevices;
using MedLaunch.Classes.Controls.InputManager;
using MedLaunch.Classes.Scanning;
using MedLaunch.Classes.Scraper.PSXDATACENTER;
using System.Windows.Threading;
using MedLaunch._Debug.ScrapeDB.ReplacementDocs;
using MedLaunch.Classes.MednaNet;
using ucon64_wrapper;
using Microsoft.Xaml.Behaviors;
using MedLaunch.Common.Eventing.Listeners;
using MedLaunch.Common.IO.Compression;
using MedLaunch.Windows;

namespace MedLaunch.Windows
{
    /// <summary>
    /// Interaction logic for TVView.xaml
    /// </summary>
    public partial class TVView
    {
        public TVView()
        {
            InitializeComponent();
        }
    }
}
