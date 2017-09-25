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
    public class MednaNetAPI
    {
        public static MednaNetAPI Instance { get; private set; }
        readonly Thread UpdateThread;

        public bool isPolling = false;
        public bool ManualQueryInProgress = false;

        public bool isConnected { get; set; }
        public string LastError { get; set; }

        public int MessageHistoryInMinutes { get; set; }

        public ConcurrentDictionary<int, int> LastChannelMessages = new ConcurrentDictionary<int, int>();
        public List<Messages> AllMessages = new List<Messages>();

        private static DispatcherTimer Timer = new DispatcherTimer();

        public static bool AbortThread { private get; set; }

        public MainWindow MW { get; set; }
        public DiscordVisualHandler DVH { get; set; }

        public List<MessageLog> MessageArchive { get; set; }

        private string InstallKey { get; set; }
        public string EndPointAddress { get; set; }
        public string EndPointPort { get; set; }

        public string Username { get; set; }

        public Client Client { get; set; }
        public Installs CurrentInstall = null;

        private Dictionary<int, int> MessageIDHistory = new Dictionary<int, int>();

        public IEnumerable<Channels> Channels { get; set; }
        public List<Users> Users { get; set; }
        
        private int CurrentChannel = 0;

        public MednaNetAPI(string username)
        {
            MW = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            DVH = MW.DVH;

            isConnected = false;

            MessageHistoryInMinutes = -600;

            MessageArchive = new List<MessageLog>();

            Username = username;
            EndPointAddress = "mednanet.medlaunch.info";
            EndPointPort = "443";

            AbortThread = true;

            // setup the timer
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 2);

            // instantiate client
            LoadClient();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        public void Start()
        {
            isConnected = true;
            Timer.Start();
        }

        public void APIDisconnected(Exception exception)
        {
            isConnected = false;
            LastError = exception.Message;
            DVH.SetConnectedStatus(false);
            DVH.PostLocalOnlyMessage("MednaNet has disconnected. Reason: " + LastError);
            DVH.PostLocalOnlyMessage("Please try reconnecting...");
        }

        private async void LoadClient()
        {
            bool installKeyPresent = false;
            string ik;

            ik = MednaNetSettings.GetInstallKey();
            if (ik != null && ik.Trim() != "")
            {
                installKeyPresent = true;
                InstallKey = ik;
            }

            // Instantiate client
            if (installKeyPresent == true)
            {
                Client = new Client(EndPointAddress, EndPointPort, ik);
            }
            else
            {
                Client = new Client(EndPointAddress, EndPointPort);
            }
            

            // get the current install object from the API
            if (installKeyPresent == true)
            {
                try { CurrentInstall = await Client.Install.GetCurrentInstall(InstallKey); }
                catch (Exception ex) { APIDisconnected(ex); return; }                
            }
                

            if (installKeyPresent == false)
            {
                try { CurrentInstall = await Client.Install.GetCurrentInstall(""); }
                catch (Exception ex) { APIDisconnected(ex); return; }
                

                InstallKey = CurrentInstall.code;
                MednaNetSettings.SetInstallKey(InstallKey);
            }

            isConnected = true;

            DoPoll();

            // start the timer
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isConnected == false)
                return;

            //DoPoll();

            if (AbortThread == false)
            {
                DoPoll();
            }

            //MessageBox.Show("tick");
        }

        public async void ChangeUsername(string user)
        {
            Username = user;
            // if the specified local username differs from API, set it
            if (Username != CurrentInstall.username)
            {
                CurrentInstall.username = Username;

                try { await Client.Install.UpdateUsername(CurrentInstall); }
                catch (Exception ex) { APIDisconnected(ex); return; }                

                MednaNetSettings.SetUsername(user);
            }
        }

        public async void DoPoll()
        {
            AbortThread = true;
            isPolling = true;
            Timer.Stop();

            if (isConnected == false)
                return;

            /* Set Username */
            // if the specified local username differs from API, set it
            if (Username != CurrentInstall.username)
            {
                CurrentInstall.username = Username;

                try { await Client.Install.UpdateUsername(CurrentInstall); }
                catch (Exception ex) { APIDisconnected(ex); return; }

                MednaNetSettings.SetUsername(Username);
            }



            /* Update Channels */
            try { Channels = await Client.Channels.GetChannels(); }
            catch (Exception ex) { APIDisconnected(ex); return; }
            
            // update channel list
            foreach (var c in Channels.OrderBy(a => a.discordId))
            {
                DVH.channels.UpdateChannel(c.id, c.channelName);
            }

            // Update the UI
            DVH.UpdateChannelButtons();

            // make sure at least one channel is selected
            DVH.CheckChannelSelection();




            /* Update Users */

            // query API            
            try { Users = await Client.Users.GetAllUsers(); }
            catch (Exception ex) { APIDisconnected(ex); return; }

            // update user list
            foreach (var u in Users)
            {
                if (u.discordId == null) // && u.isOnline == false)
                {
                    //continue;
                    
                }
                    

                ClientType ct = ClientType.discord;
                if (u.discordId == null)
                    ct = ClientType.medlaunch;

                DVH.users.UpdateUser(u.id, u.username, ct, true);
            }

            // Update the UI
            DVH.UpdateUsers();


            /* Update messages */
            
            List<Messages> foundMessages = new List<Messages>();

            // loop through each channel
            foreach (var c in Channels)
            {
                // get the last received message from this channel
                //var lookup = DiscordMessages.GetLastMessageId(c.id);    
                int lookup = 0;     
                var lookupA = AllMessages.Where(a => a.channel == c.id).OrderBy(b => b.id).LastOrDefault();
                if (lookupA != null)
                    lookup = lookupA.id;

                if (lookup == 0)
                {
                    // no message found - get all messages using the timeframe specified
                    try
                    { foundMessages.AddRange((await Client.Channels.GetChannelMessagesFrom(c.id, DateTime.Now.AddMinutes(MessageHistoryInMinutes))).ToList()); }
                    catch (Exception ex) { APIDisconnected(ex); return; }
                }
                else
                {
                    // result found - get messages from the API after this message ID
                    try { foundMessages.AddRange((await Client.Channels.GetChannelMessagesAfterMessageId(c.id, lookup)).ToList()); }
                    catch (Exception ex) { APIDisconnected(ex); return; }
                }

                // get the last messageId for this channel already posted to the screen
                int lastWritten = 0;
                if (LastChannelMessages.ContainsKey(c.id))
                    lastWritten = LastChannelMessages[c.id];

                // get the last messageId received on this channel
                int lastId = lookup;
                //var look2 = DiscordMessages.GetLastMessageId(c.id);
                int look2 = 0;
                var look2a = AllMessages.Where(a => a.channel == c.id).OrderBy(b => b.id).LastOrDefault();
                if (look2a != null)
                    look2 = look2a.id;
                if (look2 > lookup)
                    lastId = look2;

                // update the dictionary
                LastChannelMessages[c.id] = lastId;
            }

            // add foundMessages to the database
            //DiscordMessages.SaveToDatabase(foundMessages);

            // add foundMessages to AllMessages
            AllMessages.AddRange(foundMessages);
            AllMessages.Distinct();

            // now post new messages based on the currently selected channel
            foreach (var c in Channels)
            {
                int last = LastChannelMessages[c.id];
                //var newMessages = DiscordMessages.GetAllMessages().Where(a => a.MessageId > last).OrderBy(b => b.MessageId).ToList();
                var newMessages = AllMessages.Where(a => a.id > last).OrderBy(b => b.id).ToList().OrderBy(d => d.id).ToList();

                foreach (var me in newMessages)
                {
                    // create discordmessage format
                    DiscordMessage d = new DiscordMessage();
                    d.channel = me.channel;
                    d.code = me.code;
                    d.message = me.message;
                    d.messageId = me.id;
                    //d.name = me.name;
                    d.postedOn = me.postedOn;

                    // write the message to the relevant local channel
                    MednaNetAPI.Instance.DVH.PostMessage(d);
                    //DVH.PostMessage(me, c.id);
                }
                
            }

            
            AbortThread = false;
            isPolling = false;
            Timer.Start();

            /*
            foreach (var c in Channels)
            {
                // get last message ID recorded for this channel
                var lookup = MessageLog.GetLastChannelMessage(c.id);

                if (lookup == null)
                {
                    // no messages found locally for this channel, get all messages from the last day                    
                    try
                    {
                        var messages = await Client.Channels.GetChannelMessagesFrom(c.id, DateTime.Now.AddMinutes(-600));

                        // add to new messages
                        foreach (var m in messages.OrderBy(a => a.id))
                        {
                            if (m.channel == 1)
                            {

                            }

                            // check that is isnt one that we have posted ourself
                            var allZeros = from a in MednaNetAPI.Instance.MessageArchive
                                           where a.APIMessage.channel == c.id && a.APIMessage.id == 0 && a.APIMessage.message == m.message && a.HasBeenParsed == true
                                           select a;

                            if (allZeros.Count() > 0)
                            {
                                // message found, do nothing
                            }
                            else
                            {
                                MessageLog.AddMessage(m);
                            }
                        }
                    }
                    catch (Exception ex) { APIDisconnected(ex); return; }

                    
                }
                else
                {
                    // query the API for newer messages
                    try
                    {
                        var messages = await Client.Channels.GetChannelMessagesAfterMessageId(c.id, lookup.id);
                        // add to new messages
                        foreach (var m in messages)
                        {
                            MessageLog.AddMessage(m);
                        }
                    }
                    catch (Exception ex) { APIDisconnected(ex); return; }                    
                }
            }

            // process new messages
            MessageLog.PostNewMessages();
            AbortThread = false;
            isPolling = false;
            Timer.Start();

            */
        }





        public static bool Initialize(string username)
        {
            if (MednaNetAPI.Instance == null)
            {
                Instance = new MednaNetAPI(username);
            }
            else
            {
                MednaNetAPI.Instance.LoadClient();
                MednaNetAPI.Instance.Start();
            }

            //MednaNetAPI.Instance.isConnected = false;
            

            if (Instance.isConnected == true) { return true; }
            else { return false; }
        }

        public void Dispose()
        {  
            AbortThread = true;
            Timer.Stop();
        }

        public void Update()
        {
            //TODO - for some reason, we may want to control when the next event processing step happens
            //so i will leave this method here for now..
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
                    //name = "",
                    postedOn = dt
                });
            }
            catch (Exception ex) { APIDisconnected(ex); return; }
            

            /*
            Timer.Stop();
            
            Timer.Start();
            */
        }
    }

    public class MessageLog
    {
        public int ID { get; set; }
        public Messages APIMessage { get; set; }
        public bool HasBeenParsed { get; set; }
        

        public static Messages GetLastChannelMessage(int channelId)
        {
            var logCheck = (from a in MednaNetAPI.Instance.MessageArchive
                            where a.APIMessage.channel == channelId
                            select a).ToList();

            var logTemp = logCheck.OrderBy(a => a.APIMessage.id).ToList();

            var msg = logTemp.LastOrDefault();

            if (msg != null)
                return msg.APIMessage;

            return null;
        }

        public static void AddMessage(Messages msg)
        {
            // check whether message already exists
            var logCheck = (from a in MednaNetAPI.Instance.MessageArchive
                            where a.APIMessage.id == msg.id
                            select a).FirstOrDefault();

            if (logCheck == null)
            {
                // this is a new message
                MessageLog ml = new MessageLog();
                ml.ID = msg.id;
                ml.APIMessage = msg;
                ml.HasBeenParsed = false;

                MednaNetAPI.Instance.MessageArchive.Add(ml);
            }
        }

        public static void AddMessage(Messages msg, bool alreadyParsed)
        {
            // check whether message already exists
            var logCheck = (from a in MednaNetAPI.Instance.MessageArchive
                            where a.APIMessage.id == msg.id
                            select a).FirstOrDefault();

            if (logCheck == null)
            {
                // this is a new message
                MessageLog ml = new MessageLog();
                ml.ID = msg.id;
                ml.APIMessage = msg;
                ml.HasBeenParsed = alreadyParsed;

                MednaNetAPI.Instance.MessageArchive.Add(ml);
            }
        }

        public static void PostNewMessages()
        {
            // get all messages that have not already been posted to the messagebox
            var logCheck = (from a in MednaNetAPI.Instance.MessageArchive.OrderBy(a => a.APIMessage.id)
                            where a.HasBeenParsed == false && a.APIMessage.id != 0
                            select a).ToList();

            var newLog = logCheck.OrderBy(a => a.APIMessage.id).ToList();

            foreach (var entry in newLog)
            {
                var para = MednaNetAPI.Instance.DVH.GetChannelParagraph(entry.APIMessage.channel);

                // create discordmessage format
                DiscordMessage d = new DiscordMessage();
                d.channel = entry.APIMessage.channel;
                d.code = entry.APIMessage.code;
                d.message = entry.APIMessage.message;
                d.messageId = entry.APIMessage.id;
                //d.name = entry.APIMessage.name;
                d.postedOn = entry.APIMessage.postedOn;

                // write the message to the relevant local channel
                MednaNetAPI.Instance.DVH.PostMessage(d);

                // update posted status
                var tmp = (from a in MednaNetAPI.Instance.MessageArchive
                           where a.ID == d.messageId
                           select a).FirstOrDefault();

                if (tmp != null)
                    tmp.HasBeenParsed = true;
            }
            
        }
    }
}
