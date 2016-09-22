using System;
using System.Collections.Generic;
using System.Linq;
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
using MedLaunch.ViewModels;
using Asnitech.Launch.Common;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using System.Data;

namespace MedLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            // make sure class libraries are built
            Asnitech.Launch.Common.Startup.Start();

            // initialise directories if they do not exist
            SetupDirectories.Go();

            // initialise SQLite db if it does not already exist
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                // populate stock data 
                DbEF.InitialSeed();
                context.SaveChanges();
            }

            InitializeComponent();

            // ensure 'show all' filter is checked on startup
            btnShowAll.IsChecked = true;

            // load globalsettings for front page
            DbEF.LoadGlobalSettings(chkEnableNetplay, chkFullScreen, chkBypassConfig);

            // load netplay settings for netplay page
            DbEF.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);

            // load path settings for paths page
            DbEF.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);

            // force location of Mednafen directory
            //var controller = this.ShowProgressAsync("Please wait...", "Progress Message");

            // initialise servers area

            // DbEF.PopulateServersCombo(cbServers);
            DbEF.PopulateServersRadio(rbSrv01);
            DbEF.PopulateServersRadio(rbSrv02);
            DbEF.PopulateServersRadio(rbSrv03);
            DbEF.PopulateServersRadio(rbSrv04);
            DbEF.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
            DbEF.GetSelectedServerCheckbox(rbSrv01, rbSrv02, rbSrv03, rbSrv04, rbSrvCustom);
            

        }

        private async void RescanRoms(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                NegativeButtonText = "Cancel Scanning",
                AnimateShow = true,
                AnimateHide = true
            };

            var controller = await this.ShowProgressAsync("Scanning ROM Directories", "Determining Paths and Counting Files...", settings: mySettings);
            controller.SetIndeterminate();

            await Task.Delay(500);

            controller.SetCancelable(true);

            // get systems
            List<GameSystem> systems = RomScanner.GetSystems();
            List<Rom> romSystem = new List<Rom>();
            
            // iterate through each system and check if ROM path exists
            foreach (var s in systems)
            {
                string path = RomScanner.GetPath(s.systemId);
                if (path == "" || path == null)
                {
                    //do nothing
                }
                else
                {
                    //MessageBoxResult result = MessageBox.Show("System: " + s.systemName + " - Path: " + path, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    Rom r = new Rom();
                    r.gameSystem = s;
                    r.path = path;
                    romSystem.Add(r);
                }
            }

            // we now have a List<Rom> that contains all systems that have a filesystem directory path set
            // iterate through each path and count the number of files

            int totalFiles = 0;
            int romsInserted = 0;
            int romsUpdated = 0;
            int romsSkipped = 0;
            foreach (var p in romSystem)
            {
                int systemFiles = RomScanner.CountFiles(p.path);
                totalFiles += systemFiles;
            }

            controller.SetMessage(totalFiles + " files found across all ROM directories");

            await Task.Delay(500);

            foreach (var item in romSystem)
            {
                // actually scan through ROMS
                string SystemName = item.gameSystem.systemName;
                controller.SetTitle("Importing " + SystemName + " Roms");

                // get a collection of files from each ROM directory
                var files = RomScanner.GetFiles(item.path);

                // iterate through each file
                foreach (var file in files)
                {
                    // get the relative path
                    string relPath = PathUtil.GetRelativePath(item.path, file);

                    // get just the filename
                    string fileName = System.IO.Path.GetFileName(file);

                    // get just the extension
                    string extension = System.IO.Path.GetExtension(file);

                    // get rom name wihout extension
                    string romName = fileName.Replace(extension, "");

                    //MessageBoxResult result = MessageBox.Show(romName, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // attempt to add Rom to database
                    int addRom = RomScanner.AddGame(item, file, relPath, fileName, extension, romName);

                    string romProcess = "";

                    switch (addRom)
                    {
                        case 1:
                            // Rom inserted
                            romsInserted++;
                            romProcess = "Added to Database";
                            break;
                        case 2:
                            // Rom updated
                            romsUpdated++;
                            romProcess = "Updated in Database";
                            break;
                        default:
                            // Rom skipped
                            romsSkipped++;
                            romProcess = "Has Been Skipped";
                            break;               
                    }

                    controller.SetMessage("ROM: " + romName + " " + romProcess);

                }
                await Task.Delay(100);
            }

            /*
            double i = 0.0;
            while (i < Convert.ToDouble(totalFiles + 1))
            {
                //double val = (i / 100.0) * totalFiles;
                double val = (100 / totalFiles) * i / 100;
                controller.SetProgress(val);
                
                controller.SetMessage("Scanning " +  + i + "...");

                if (controller.IsCanceled)
                    break; //canceled progressdialog auto closes.

                i += 1.0;

                await Task.Delay(2000);
            }
            */

            await controller.CloseAsync();

            if (controller.IsCanceled)
            {
                await this.ShowMessageAsync("The operation was cancelled!", romsInserted +  " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
            }
            else
            {
                await this.ShowMessageAsync("Operation completed", romsInserted + " ROMS have been added \n" + romsUpdated + " ROMS have been updated \n" + romsSkipped + " ROMS have been skipped");
            }

            //Update list
            // ensure 'show all' filter is checked on startup
            btnFavorites.IsChecked = true;
            btnShowAll.IsChecked = true;
        }

        /*********** 
         * EVENTS
         * *********/






        // Misc
        private void System_Image_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Game filter buttons
        private void btnShowAll_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 0, tbFilterDatagrid.Text);
        }

        private void btnFavorites_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, -1, tbFilterDatagrid.Text);
        }

        private void btnNes_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 11, tbFilterDatagrid.Text);
        }

        private void btnSnes_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 12, tbFilterDatagrid.Text);
        }

        private void btnSms_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 10, tbFilterDatagrid.Text);
        }

        private void btnMd_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 4, tbFilterDatagrid.Text);
        }

        private void btnSs_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 13, tbFilterDatagrid.Text);
        }

        private void btnPsx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 9, tbFilterDatagrid.Text);
        }

        private void btnPce_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 7, tbFilterDatagrid.Text);
        }

        private void btnPcfx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 8, tbFilterDatagrid.Text);
        }

        private void btnVb_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 14, tbFilterDatagrid.Text);
        }

        private void btnNgp_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 6, tbFilterDatagrid.Text);
        }

        private void btnWswan_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 15, tbFilterDatagrid.Text);
        }

        private void btnGb_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 1, tbFilterDatagrid.Text);
        }

        private void btnGba_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 2, tbFilterDatagrid.Text);
        }

        private void btnGg_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 5, tbFilterDatagrid.Text);
        }

        private void btnLynx_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.GetGames(dgGameList, 3, tbFilterDatagrid.Text);
        }

        // Games grid filter text box event
        private void tbFilterDatagrid_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            int system = 0;
            // determine which radiobutton is checked
            if (btnShowAll.IsChecked == true)
            {
                system = 0;
            }
            if (btnFavorites.IsChecked == true)
            {
                system = -1;
            }
            if (btnLynx.IsChecked == true)
            {
                system = 3;
            }
            if (btnGg.IsChecked == true)
            {
                system = 5;
            }
            if (btnGba.IsChecked == true)
            {
                system = 2;
            }
            if (btnGb.IsChecked == true)
            {
                system = 1;
            }
            if (btnWswan.IsChecked == true)
            {
                system = 15;
            }
            if (btnNgp.IsChecked == true)
            {
                system = 6;
            }
            if (btnVb.IsChecked == true)
            {
                system = 14;
            }
            if (btnPcfx.IsChecked == true)
            {
                system = 8;
            }
            if (btnPce.IsChecked == true)
            {
                system = 7;
            }
            /*
            if (btnPsx.IsChecked == true)
            {
                system = 9;
            }
            if (btnSs.IsChecked == true)
            {
                system = 13;
            }
            */
            if (btnMd.IsChecked == true)
            {
                system = 4;
            }
            if (btnSms.IsChecked == true)
            {
                system = 10;
            }
            if (btnSnes.IsChecked == true)
            {
                system = 12;
            }
            if (btnNes.IsChecked == true)
            {
                system = 11;
            }

            DbEF.GetGames(dgGameList, system, textbox.Text);
        }

        // Clear all filters button click
        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            btnShowAll.IsChecked = true;
            tbFilterDatagrid.Clear();
        }

        // Games Datagrid selection changed
        private void dgGameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // update right hand info
            var row = (DataGridGamesView)dgGameList.SelectedItem;
            if (dgGameList.SelectedItem != null)
            {
                DbEF.GetInfo(row.ID, lblSystemName, taSystemDescription, imgSystem);
            }
        }

        // generic TextBox_TextChanged handler
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
         
        }

        // generic Slider_ValueChanged handler
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var binding = ((Slider)sender).GetBindingExpression(Slider.ValueProperty);
            binding.UpdateSource();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Data.CollectionViewSource globalSettingsViewModelViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("globalSettingsViewModelViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // globalSettingsViewModelViewSource.Source = [generic data source]
        }

        // Netplay Settings - netplay page
        private void btnNetplaySaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.SaveNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            lblNpSettingsSave.Content = "***Netplay Settings Saved***";
        }

        private void btnNetplayCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.LoadNetplaySettings(tbNetplayNick, slLocalPlayersValue, slConsoleLinesValue, slConsoleScaleValue, resOne, resTwo, resThree, resFour, resFive);
            lblNpSettingsSave.Content = "***Netplay Settings Reverted***";
        }

        // Global Settings - Game page

        private void chkEnableNetplay_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateEnableNetplay(chkEnableNetplay);
        }
        private void chkEnableNetplay_Unchecked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateEnableNetplay(chkEnableNetplay);
        }

        private void chkFullScreen_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateFullScreen(chkFullScreen);
        }
        private void chkFullScreen_Unchecked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateFullScreen(chkFullScreen);
        }

        private void chkBypassConfig_Checked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateBypassConfig(chkBypassConfig);
        }
        private void chkBypassConfig_Unchecked(object sender, RoutedEventArgs e)
        {
            DbEF.UpdateBypassConfig(chkBypassConfig);
        }


        // Path Page button clicks
        private void btnPathMednafen_Click(object sender, RoutedEventArgs e)
        {            
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Mednafen Directory";
            path.ShowDialog();
            
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMednafen.Text = strPath;
            }
                      
        }

        private void btnPathGb_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Gameboy ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGb.Text = strPath;
            }
        }

        private void btnPathGba_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Gameboy Advance ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGba.Text = strPath;
            }
        }

        private void btnPathGg_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select GameGear ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathGg.Text = strPath;
            }
        }

        private void btnPathLynx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Atari Lynx ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathLynx.Text = strPath;
            }
        }

        private void btnPathMd_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Megadrive/Genesis ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMd.Text = strPath;
            }
        }

        private void btnPathNes_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select NES ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathNes.Text = strPath;
            }
        }

        private void btnPathSnes_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select SNES ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathSnes.Text = strPath;
            }
        }

        private void btnPathNgp_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select NeoGeo Pocket Color ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathNgp.Text = strPath;
            }
        }

        private void btnPathPce_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PC-Engine ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPce.Text = strPath;
            }
        }

        private void btnPathPcfx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PC-FX ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPcfx.Text = strPath;
            }
        }

        

        private void btnPathMs_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Sega Master System ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathMs.Text = strPath;
            }
        }
        /*
        private void btnPathPsx_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select PSX ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathPsx.Text = strPath;
            }
        }

        private void btnPathSs_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Sega Saturn ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathSs.Text = strPath;
            }
        }
        */
        private void btnPathVb_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select Virtual Boy ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathVb.Text = strPath;
            }
        }

        private void btnPathWswan_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog path = new VistaFolderBrowserDialog();
            path.ShowNewFolderButton = true;
            path.Description = "Select WonderSwan ROM Directory";
            path.ShowDialog();
            if (path.SelectedPath != "")
            {
                string strPath = path.SelectedPath;
                tbPathWswan.Text = strPath;
            }
        }

        // Path page save & restore

        private void btnPathsSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.SavePathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);
            lblPathsSettingsSave.Content = "***Path Settings Saved***";
        }

        private void btnPathsCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.LoadPathSettings(tbPathMednafen, tbPathGb, tbPathGba, tbPathGg, tbPathLynx, tbPathMd, tbPathNes, tbPathSnes, tbPathNgp, tbPathPce, tbPathPcfx, tbPathMs, tbPathVb, tbPathWswan); // tbPathPsx, tbPathSs);
            lblPathsSettingsSave.Content = "***Path Settings Reverted***";
        }

        // games list context menu
        private void dgGameList_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            dgGameList.Items.Refresh();
            dgGameList.InvalidateVisual();
            FrameworkElement fe = e.Source as FrameworkElement;
            ContextMenu cm = fe.ContextMenu;

            // get selected row data
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;

            string romName = drv.Game;
            int romId = drv.ID;

            // Replace Menu Items
            foreach (MenuItem mi in cm.Items)
            {
                // play game menu item
                if ((String)mi.Header == "Play Game")
                {
                    mi.Header = "Play " + romName;
                }

                // Favorites toggle
                if ((String)mi.Header == "Favorites")
                {
                    // check the favorite status
                    if (RomScanner.GetFavoriteStatus(romId) == 1)
                        mi.Header = "Add/Remove From Favorites";
                    else
                        mi.Header = "Add/Remove From Favorites";
                }
            }

            //fe.ContextMenu = CMenu.BuildGamesMenu(dgGameList);
        }

        private void MenuItemFavorite_Click(object sender, RoutedEventArgs e)
        {
            DataGridGamesView drv = (DataGridGamesView)dgGameList.SelectedItem;
            int romId = drv.ID;
            RomScanner.FavoriteToggle(romId);
            dgGameList.Items.Refresh();
            dgGameList.InvalidateVisual();
                               
            
        }


        // servers


        private void rbSrv01_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);
        }

        private void rbSrv02_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);
        }

        private void rbSrv03_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);
        }

        private void rbSrv04_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);
        }

        private void rbSrvCustom_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);

            // unhidden
            tbServerDesc.IsEnabled = true;
            tbServerDesc.Visibility = Visibility.Visible;
            tbHostname.IsEnabled = true;
            tbHostname.Visibility = Visibility.Visible;
            slServerPort.IsEnabled = true;
            slServerPort.Visibility = Visibility.Visible;
            tbPassword.IsEnabled = true;
            tbPassword.Visibility = Visibility.Visible;
            tbGameKey.IsEnabled = true;
            tbGameKey.Visibility = Visibility.Visible;

            lblServerDesc.Visibility = Visibility.Visible;
            lblHostname.Visibility = Visibility.Visible;
            lblServerPort.Visibility = Visibility.Visible;
            lblServerPortTxt.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            lblGamekey.Visibility = Visibility.Visible;

            btnServerSaveChanges.Visibility = Visibility.Visible;
            btnServerCancelChanges.Visibility = Visibility.Visible;
        }
        private void rbSrvCustom_UnChecked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            DbEF.SetSelectedServer(rb);

            // hidden
            tbServerDesc.IsEnabled = false;
            tbServerDesc.Visibility = Visibility.Collapsed;
            tbHostname.IsEnabled = false;
            tbHostname.Visibility = Visibility.Collapsed;
            slServerPort.IsEnabled = false;
            slServerPort.Visibility = Visibility.Collapsed;
            tbPassword.IsEnabled = false;
            tbPassword.Visibility = Visibility.Collapsed;
            tbGameKey.IsEnabled = false;
            tbGameKey.Visibility = Visibility.Collapsed;

            lblServerDesc.Visibility = Visibility.Collapsed;
            lblHostname.Visibility = Visibility.Collapsed;
            lblServerPort.Visibility = Visibility.Collapsed;
            lblServerPortTxt.Visibility = Visibility.Collapsed;
            lblPassword.Visibility = Visibility.Collapsed;
            lblGamekey.Visibility = Visibility.Collapsed;

            btnServerSaveChanges.Visibility = Visibility.Collapsed;
            btnServerCancelChanges.Visibility = Visibility.Collapsed;
        }

        private void btnServerSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.SaveCustomServerSettings(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
        }

        private void btnServerCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            DbEF.PopulateCustomServer(tbServerDesc, tbHostname, slServerPort, tbPassword, tbGameKey);
        }
    }
}
