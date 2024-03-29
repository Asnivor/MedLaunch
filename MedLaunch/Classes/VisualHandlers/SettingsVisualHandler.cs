﻿using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedLaunch.Classes
{
    public class SettingsVisualHandler
    {
        // Constructor
        public SettingsVisualHandler()
        {
            // get an instance of the MainWindow
            MWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get 'show all settings' button and click it
            RadioButton btnAllSettings = (RadioButton)MWindow.FindName("btnAllSettings");
            //btnAllSettings.IsChecked = true;

            // get settings grid
            WrapPanel wpSettingsLeftPane = (WrapPanel)MWindow.FindName("wpSettingsLeftPane");

            // get all filter buttons from the settings page
            List<RadioButton> _filterButtons = UIHandler.GetLogicalChildCollection<RadioButton>(wpSettingsLeftPane);//.Where(r => r.GroupName == "grpSettings").ToList();
            FilterButtons = _filterButtons;

            // setting grid containing right hand content
            Grid SettingGrid = (Grid)MWindow.FindName("SettingGrid");

            // get all settings panels
            //AllSettingPanels = UIHandler.GetLogicalChildCollection<Border>("SettingGrid").ToList();

            AllSettingPanels = UIHandler.GetLogicalChildCollection<Border>(SettingGrid).ToList();


            // iterate through each panel and match the border x:name to the class property name
            foreach (Border b in AllSettingPanels)
            {
                // remove any trailing numerals from the control name
                string name = StripTrailingNumerals(b.Name);                

                // if the control name matches a property name in this class, add it to that list
                PropertyInfo property = typeof(SettingsVisualHandler).GetProperty(name);
                if (property == null)
                {
                    // no property matched
                    continue;
                }

                // add the border control to the correct List
                switch (property.Name)
                {
                    case "MednafenPaths":
                        MednafenPaths.Add(b);
                        break;
                    case "GameFolders":
                        GameFolders.Add(b);
                        break;
                    case "SystemBios":
                        SystemBios.Add(b);
                        break;
                    case "Netplay":
                        Netplay.Add(b);
                        break;
                    case "Emulator":
                        Emulator.Add(b);
                        break;
                    case "MedLaunch":
                        MedLaunch.Add(b);
                        break;
                    case "Library":
                        Library.Add(b);
                        break;
                    case "ScrapingSettings":
                        ScrapingSettings.Add(b);
                        break;
                    case "MednaNet":
                        MednaNet.Add(b);
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
        }

        // Methods

        private static string StripTrailingNumerals(string input)
        {
            //string input = b.Name;
            string pattern = @"\d+$";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            string output = rgx.Replace(input, replacement);
            return output;
        }
        public static void ButtonClick()
        {
            SettingsVisualHandler svh = new SettingsVisualHandler();
            svh.SetFilter();
        }

        public void SetFilter()
        {

            // get active button
            RadioButton _activeRadio = FilterButtons.Where(a => a.IsChecked == true).Single();

            string name = _activeRadio.Name.Replace("btn", "");

            // get all borders that have names that match the above string
            string brdName = "brd" + name;
            List<Border> _borders = (from b in AllSettingPanels
                                    where b.Name.Contains(brdName)
                                    select b).ToList();
            
            if (name == "AllSettings")
            {
                // all settings - show all
                foreach (Border b in AllSettingPanels)
                {
                    b.Visibility = Visibility.Visible;
                    if (b.Name == "MednafenPaths" || b.Name == "SystemBios")
                        b.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                // active the border boxes relating to this filter button and deactivate the rest
                List<Border> newList = new List<Border>();
                newList = AllSettingPanels.ToList();

                foreach (Border b in _borders)
                {
                    // set visibility visible
                    b.Visibility = Visibility.Visible;
                    // remove from AllSettingsPanels
                    newList.Remove(b);
                }
                foreach (Border b in newList)
                {
                    // set visibility collapsed
                    b.Visibility = Visibility.Collapsed;
                }
            }
        }

        public static void PopulateServers(DataGrid lvServers)
        {
            // get all servers
            var servers = ConfigServerSettings.GetServers()
                .Where(a => a.netplay__host != null &&
                a.netplay__gamekey != null &&
                a.netplay__password != null &&
                a.netplay__port != null).ToList();

            if (servers == null || servers.Count == 0)
                return;

            // get selected server id
            GlobalSettings gs = GlobalSettings.GetGlobals();
            int sid = gs.serverSelected.Value;

            List<ServersListView> list = new List<ServersListView>();

            // populate list
            foreach (var s in servers)
            {
                ServersListView srv = new ServersListView();
                srv.ID = s.ConfigServerId;
                srv.Name = s.ConfigServerDesc;
                srv.Host = s.netplay__host;
                srv.Port = s.netplay__port.Value;
                srv.Password = s.netplay__password;
                srv.Gamekey = s.netplay__gamekey;

                if (sid == srv.ID)
                {
                    srv.Selected = true;
                }
                else
                {
                    srv.Selected = false;
                }

                list.Add(srv);
            }

            lvServers.ItemsSource = list;
        }

        public static void ServerSettingsInitialButtonHide()
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Button btnServersSelect = (Button)mw.FindName("btnServersSelect");
            Button btnServersDelete = (Button)mw.FindName("btnServersDelete");
            Button btnServersSaveEdit = (Button)mw.FindName("btnServersSaveEdit");
            btnServersSelect.Visibility = Visibility.Collapsed;
            btnServersDelete.Visibility = Visibility.Collapsed;
            btnServersSaveEdit.Visibility = Visibility.Collapsed;
        }


        // Properties
        public MainWindow MWindow { get; set; }
        public List<RadioButton> FilterButtons { get; set; }
        public List<Border> AllSettingPanels { get; set; }
        public List<Border> MednafenPaths { get; set; }
        public List<Border> GameFolders { get; set; }
        public List<Border> SystemBios { get; set; }
        public List<Border> Netplay { get; set; }
        public List<Border> Emulator { get; set; }
        public List<Border> MedLaunch { get; set; }
        public List<Border> Library { get; set; }
        public List<Border> ScrapingSettings { get; set; }
        public List<Border> MednaNet { get; set; }
    }

    public class ServersListView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string Gamekey { get; set; }
        public bool Selected { get; set; }
    }
}
