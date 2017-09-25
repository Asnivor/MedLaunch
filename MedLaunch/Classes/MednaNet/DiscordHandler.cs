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
        public int TimerIntervalInSeconds { get; set; }

        public string Username { get; set; }
        public string InstallKey { get; set; }
        public string EndPointAddress { get; set; }
        public string EndPointPort { get; set; }

        public int HistoryInMinutes { get; set; }

        public bool IsConnected { get; set; }

        public bool UsersIsPolling { get; set; }
        public bool ChannelsIsPolling { get; set; }
        public bool MessagesIsPolling { get; set; }
        public bool MessageDBSync { get; set; }

        public List<Channels> Channels { get; set; }
        public List<Users> Users { get; set; }
        public List<DiscordMessages> MessageList { get; set; }
        public Dictionary<int, int> LastChannelMessages { get; set; }

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

            Channels = new List<MednaNetAPIClient.Models.Channels>();
            Users = new List<MednaNetAPIClient.Models.Users>();
            MessageList = new List<DiscordMessages>();

            LastChannelMessages = new Dictionary<int, int>();

            
            // setup polling bools
            UsersIsPolling = false;
            ChannelsIsPolling = false;
            MessagesIsPolling = false;
            MessageDBSync = false;
            IsConnected = false;

            // setup the timer
            TimerIntervalInSeconds = MednaNetSettings.GetPollTimerInterval();
            Timer.Tick += new EventHandler(Timer_Tick);
            //Timer.Interval = new TimeSpan(0, 0, 5);
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

            Instance.HistoryInMinutes = MednaNetSettings.GetChatHistoryInMinutes();

            Timer.Interval = new TimeSpan(0, 0, Instance.TimerIntervalInSeconds);

            DiscordHandler.Timer.Start();
            Instance.IsConnected = true;

        }

        public void Poll()
        {
            if (!IsConnected)
                return;

            GetUsersAsync();
            GetChannelsAsync();
            ChangeUsernameAsync();

            // allow only one poll of this type at a time

            if (MessagesIsPolling == false)
            {

                
            }

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
                Channels = (await Client.Channels.GetChannels()).ToList();

                // update visual handler once await has completed
                ChannelsIsPolling = false;

                // update channel list
                foreach (var c in Channels.OrderBy(a => a.discordId).ToList())
                {
                    DVH.channels.UpdateChannel(c.id, c.channelName);
                }

                // add invisible channel with ID 0 (for initial display)
                DVH.channels.UpdateChannel(0, "initChannel");

                // Update the UI
                DVH.UpdateChannelButtons();

                // make sure at least one channel is selected
                DVH.CheckChannelSelection();

            }
            catch (Exception ex) { APIDisconnected(ex); return; }
        }

        /// <summary>
        /// Message updating logic
        /// </summary>
        private async void GetMessagesAsync()
        {
            if (MessagesIsPolling == true)
                return;

            MessagesIsPolling = true;

            try
            {
                // get messages for all channels
                foreach (var c in Channels)
                {
                    // get last id from channel message ids dictionary
                    int lastId = 0;

                    if (LastChannelMessages.ContainsKey(c.id))
                    {
                        lastId = LastChannelMessages[c.id];
                    }
                        
                    // get messages frm API
                    if (lastId == 0)
                    {
                        // no local messageId - get all messages using the timeframe specified
                        var inc = (await Client.Channels.GetChannelMessagesFrom(c.id, DateTime.Now.AddMinutes(-3000))).ToList();
                        
                        var t = inc.OrderBy(a => a.id).ToList().LastOrDefault();
                        if (t != null)
                            LastChannelMessages[c.id] = t.id;

                        // write to textbox
                        DVH.WriteToTextBox(inc.OrderBy(a => a.id).ToList());

                        // save to database
                        //SaveToLoggingDatabase(inc);            
                    }
                    else
                    {
                        var inc = (await Client.Channels.GetChannelMessagesAfterMessageId(c.id, lastId)).ToList();
                        MessagesIsPolling = false;
                        var t = inc.OrderBy(a => a.id).ToList().LastOrDefault();
                        if (t != null)
                            LastChannelMessages[c.id] = t.id;

                        // write to textbox
                        DVH.WriteToTextBox(inc.OrderBy(a => a.id).ToList());

                        // save to database
                        //SaveToLoggingDatabase(inc);
                    }

                    MessagesIsPolling = false;
                }
            }
            catch (Exception ex) { APIDisconnected(ex); return; }

            MessagesIsPolling = false;
        }

        /*
        public void SaveToLoggingDatabase(List<Messages> messages)
        {
            var lookup = db.DiscordMessages.GetAllMessages();

            using (var db = new MednaLogDbContext())
            {
                List<DiscordMessages> mes = new List<DiscordMessages>();

                foreach (var m in messages)
                {
                    DiscordMessages me = new DiscordMessages();
                    me.APITimeReceived = m.postedOn;
                    me.ChannelId = m.channel;
                    me.LocalTimeReceived = DateTime.Now;
                    me.MessageId = m.id;
                    me.MessageString = m.message;
                    me.NickName = m.user.username;
                    me.UserId = m.user.id;
                    if (m.user.discordId == null || m.user.discordId == "")
                        me.IsAPIUser = true;
                    else
                        me.IsAPIUser = false;

                    mes.Add(me);
                }

                List<DiscordMessages> toAdd = new List<DiscordMessages>();
                List<DiscordMessages> toUpdate = new List<DiscordMessages>();

                foreach (var v in mes)
                {
                    var l = (from a in lookup
                            where a.MessageId == v.MessageId
                            select a).ToList().FirstOrDefault();

                    if (l == null)
                        toAdd.Add(v);
                    else
                        toUpdate.Add(v);
                }

                db.DiscordMessages.AddRange(toAdd);
                db.DiscordMessages.UpdateRange(toUpdate);
                db.SaveChanges();
            }
        }
        

        public List<Messages> GetFromDatabase(int daysHistory)
        {
            var data = DiscordMessages.GetAllMessages().Where(a => a.APITimeReceived > DateTime.Now.AddDays(-daysHistory));
        } 

        */

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


        /// <summary>
        /// keeps the sqlite log DB in sync with the local in-memory object
        /// </summary>
        private async Task SyncMessageListAsync()
        {
            // allow only one poll of this type at a time
            if (MessageDBSync == true)
                return;

            MessageDBSync = true;

            try
            {
                // get channels from MednaNet API
                Channels = (await Client.Channels.GetChannels()).ToList();

                // update visual handler once await has completed
                MessageDBSync = false;

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

        private void LocalPost(Messages msg)
        {
            // post return message straight away to the channel
            LastChannelMessages[msg.channel] = msg.id;

            // write to textbox
            DVH.WriteToTextBox(new List<Messages> { msg });
        }


        public async void SendMessage(string message)
        {
            DateTime dt = DateTime.Now;

            // create new API message
            try
            {
                Messages newMessage = await Client.Channels.CreateMessage(DVH.channels.ActiveChannel, new Messages
                {
                    channel = DVH.channels.ActiveChannel,
                    code = InstallKey,
                    message = message,  
                    postedOn = dt,
                    user = new MednaNetAPIClient.Models.Users
                    {
                         username = Username,
                    }
                    
                });

                // post locally (not wait for next poll)
                LocalPost(newMessage);

            }
            catch (Exception ex) { APIDisconnected(ex); return; }


            /*
            Timer.Stop();
            
            Timer.Start();
            */
        }

    }
}
