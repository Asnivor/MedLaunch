using MedLaunch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MednaNetAPIClient;
using MednaNetAPIClient.Models;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MedLaunch.Classes.MednaNet.db;
using System.Collections.Concurrent;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordHandler
    {
        public static DiscordHandler Instance { get; set; }

        private MainWindow MW { get; set; }
        public DiscordVisualHandler DVH { get; set; }

        public static DispatcherTimer Timer = new DispatcherTimer();

        public string Username { get; set; }
        public string InstallKey { get; set; }
        public string EndPointAddress { get; set; }
        public string EndPointPort { get; set; }

        public bool IsConnected { get; set; }

        public bool UsersIsPolling { get; set; }
        public bool ChannelsIsPolling { get; set; }
        public bool MessagesIsPolling { get; set; }

        public IEnumerable<Channels> Channels { get; set; }
        public IEnumerable<Users> Users { get; set; }

        public Client Client { get; set; }
        public Installs CurrentInstall = null;

        /// <summary>
        /// constructor
        /// </summary>
        public DiscordHandler()
        {
            // get username from database
            Username = Models.MednaNetSettings.GetUsername();

            // api endpoint information
            EndPointAddress = "mednanet.medlaunch.info";
            EndPointPort = "443";

            // get mainwindow instance
            MW = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (Username == null || Username.Trim() == "")
            {
                Username = MW.tbDiscordName.Text;
            }

            // get visual handler reference
            DVH = MW.DVH;
            
            // setup polling bools
            UsersIsPolling = false;
            ChannelsIsPolling = false;
            MessagesIsPolling = false;
            IsConnected = false;

            // setup the timer
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 2);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (IsConnected == false)
                return;

            Poll();
        }

        /// <summary>
        /// init the static instance of the DiscordHandler
        /// </summary>
        public static async void Initialize()
        {
            if (DiscordHandler.Instance == null)
            {
                Instance = new DiscordHandler();

                // setup the client
                bool installKeyPresent = false;
                string ik;

                ik = MednaNetSettings.GetInstallKey();
                if (ik != null && ik.Trim() != "")
                {
                    installKeyPresent = true;
                    Instance.InstallKey = ik;
                }

                // Instantiate client
                if (installKeyPresent == true)
                {
                    Instance.Client = new Client(Instance.EndPointAddress, Instance.EndPointPort, ik);
                }
                else
                {
                    Instance.Client = new Client(Instance.EndPointAddress, Instance.EndPointPort);
                }

                // get the current install object from the API
                if (installKeyPresent == true)
                {
                    try { Instance.CurrentInstall = await Instance.Client.Install.GetCurrentInstall(Instance.InstallKey); }
                    catch (Exception ex) { Instance.APIDisconnected(ex); return; }
                }

                if (installKeyPresent == false)
                {
                    try { Instance.CurrentInstall = await Instance.Client.Install.GetCurrentInstall(""); }
                    catch (Exception ex) { Instance.APIDisconnected(ex); return; }

                    Instance.InstallKey = Instance.CurrentInstall.code;
                    MednaNetSettings.SetInstallKey(Instance.InstallKey);
                }

                Instance.IsConnected = true;                
            }

            DiscordHandler.Timer.Start();

        }

        public void Poll()
        {
            if (!IsConnected)
                return;

            GetUsersAsync();
            GetChannelsAsync();
            GetMessagesAsync();
        }

        public void APIDisconnected(Exception exception)
        {
            Timer.Stop();
            IsConnected = false;
            DVH.SetConnectedStatus(false);
            DVH.PostLocalOnlyMessage("MednaNet has disconnected. Reason: " + exception.Message);
            DVH.PostLocalOnlyMessage("Please try reconnecting...");
        }

        /// <summary>
        /// retreives an updated userlist from the mednanet API asyncronously
        /// once the await has completed DiscordVisualHandler will asyncronously update the users UI
        /// </summary>
        private async void GetUsersAsync()
        {
            // allow only one poll of this type at a time
            if (UsersIsPolling == true)
                return;

            UsersIsPolling = true;

            try
            {
                // get users from MednaNet API
                Users = await Client.Users.GetAllUsers();

                // update visual handler once await has completed
                UsersIsPolling = false;

                foreach (var u in Users)
                {
                    ClientType ct = ClientType.discord;
                    if (u.discordId == null)
                        ct = ClientType.medlaunch;

                    DVH.users.UpdateUser(u.id, u.username, ct, true);
                }

                DVH.UpdateUsers();
            }
            catch (Exception ex) { APIDisconnected(ex); return; }
        }

        /// <summary>
        /// retreives an updated channellist from the mednanet API asyncronously
        /// once the await has completed DiscordVisualHandler will asyncronously update the channels UI
        /// </summary>
        private async void GetChannelsAsync()
        {
            // allow only one poll of this type at a time
            if (ChannelsIsPolling == true)
                return;

            ChannelsIsPolling = true;

            try
            {
                // get channels from MednaNet API
                Channels = await Client.Channels.GetChannels();

                // update visual handler once await has completed
                ChannelsIsPolling = false;

                // update channel list
                foreach (var c in Channels.OrderBy(a => a.discordId).ToList())
                {
                    DVH.channels.UpdateChannel(c.id, c.channelName);
                }

                // Update the UI
                DVH.UpdateChannelButtons();

                // make sure at least one channel is selected
                DVH.CheckChannelSelection();

            }
            catch (Exception ex) { APIDisconnected(ex); return; }
        }

        private async void GetMessagesAsync()
        {

        }

        /// <summary>
        /// checks whether the local Username differs from the CurrentInstall username (i.e., has been changed)
        /// if so, asyncronously send username change over the API
        /// on await return the username in the MedLaunch.db is updated
        /// </summary>
        private async void ChangeUsernameAsync()
        {
            // if the specified local username differs from API, set it
            if (Username != CurrentInstall.username)
            {
                CurrentInstall.username = Username;

                try { await Client.Install.UpdateUsername(CurrentInstall); }
                catch (Exception ex) { APIDisconnected(ex); return; }

                MednaNetSettings.SetUsername(Username);
            }
        }
        
    }
}
