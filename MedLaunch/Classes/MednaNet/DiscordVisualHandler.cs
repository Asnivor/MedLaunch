using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordVisualHandler
    {
        public MainWindow mw { get; set; }
        public List<RadioButton> ChannelRadios { get; set; }
        public List<Button> UserButtons { get; set; }
        public TextBox tbDiscordName { get; set; }
        public Label lblConnectedStatus { get; set; }
        public Button btnDiscordConnect { get; set; }
        public ScrollViewer scrlDiscordChannels { get; set; }
        public ScrollViewer scrlDiscordUsers { get; set; }
        public TextBox tbDiscordMessageBox { get; set; }
        public Button btnDiscordChatSend { get; set; }
        public RichTextBox rtbDocument { get; set; }
        public StackPanel DiscordSelectorWrapPanel { get; set; }
        public StackPanel DiscordUserListWrapPanel { get; set; }

        private Paragraph paragraph { get; set; }

        public DiscordChannels channels { get; set; }
        public DiscordUsers users { get; set; }

        /// <summary>
        /// default contructor
        /// </summary>
        public DiscordVisualHandler()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            // get misc controls
            tbDiscordName = (TextBox)mw.FindName("tbDiscordName");
            btnDiscordConnect = (Button)mw.FindName("btnDiscordConnect");
            scrlDiscordChannels = (ScrollViewer)mw.FindName("scrlDiscordChannels");
            scrlDiscordUsers = (ScrollViewer)mw.FindName("scrlDiscordUsers");
            tbDiscordMessageBox = (TextBox)mw.FindName("tbDiscordMessageBox");
            btnDiscordChatSend = (Button)mw.FindName("btnDiscordChatSend");
            DiscordSelectorWrapPanel = (StackPanel)mw.FindName("DiscordSelectorWrapPanel");
            DiscordUserListWrapPanel = (StackPanel)mw.FindName("DiscordUserListWrapPanel");

            // chat text window
            rtbDocument = (RichTextBox)mw.FindName("rtbDocument");


            // get channel radios

            // get user buttons

            // get labels
            lblConnectedStatus = (Label)mw.FindName("lblConnectedStatus");


            // set connected status
            SetConnectedStatus(false);

            // initialise channels object (empty)
            channels = new DiscordChannels();

            // init user object (empty)
            users = new DiscordUsers();

            // initialise richtextbox
            paragraph = new Paragraph();
            rtbDocument.Document = new FlowDocument(paragraph);

            UpdateChannelButtons();
            UpdateUsers();

            
        }

        // non-static methods

        public Paragraph GetChannelParagraph(int channelId)
        {
            var chan = channels.Data.Where(a => a.ChannelId == channelId).FirstOrDefault();
            if (chan == null)
                return null;

            return chan.Paragraph;
        }

        /// <summary>
        /// Begin sending a message to the chatbox
        /// </summary>
        /// <param name="message"></param>
        public async void PostMessage(string message, int channelId)
        {
            // post message on another thread
            ThreadPool.QueueUserWorkItem(o =>
            {
                ChatUpdater(message, channelId);
            });
        }

        /// <summary>
        /// update the chatbox
        /// </summary>
        /// <param name="message"></param>
        private async void ChatUpdater(string message, int channelId)
        {
            await mw.Dispatcher.BeginInvoke((Action)(() =>
            {
                // get channel paragraph
                var para = GetChannelParagraph(channelId);

                para.Inlines.Add(message);
                para.Inlines.Add(new LineBreak());
            }));            
        }

        /// <summary>
        /// writes to the chatbox with a different color (for system messages, etc)
        /// </summary>
        /// <param name="message"></param>
        public void PostLocalOnlyMessage(string message)
        {
            // post to all paragraphs
            RefreshChannels();
            for (int i = 0; i < channels.Data.Count(); i++)
            {
                channels.Data[i].Paragraph.Inlines.Add(new Bold(new Italic(new Run("MedLaunch (local): " + message)))
                {
                    Foreground = Brushes.Red
                });

                channels.Data[i].Paragraph.Inlines.Add(new LineBreak());
            }
            /*
            paragraph.Inlines.Add(new Bold(new Italic(new Run("MedLaunch (local): " + message)))
            {
                Foreground = Brushes.Red
            });

            paragraph.Inlines.Add(new LineBreak());
            */
        }


        public void SetConnectedStatus(bool connectedstatus)
        {
            if (connectedstatus == true)
            {
                tbDiscordName.IsReadOnly = true;
                tbDiscordName.IsEnabled = false;
                lblConnectedStatus.Foreground = new SolidColorBrush(Colors.Green);
                lblConnectedStatus.Content = "CONNECTED";
                btnDiscordConnect.Content = "DISCONNECT";
                DiscordUIState(UIState.enabled);
            }
                
            else
            {
                tbDiscordName.IsEnabled = true;
                tbDiscordName.IsReadOnly = false;
                lblConnectedStatus.Foreground = new SolidColorBrush(Colors.Red);
                lblConnectedStatus.Content = "DISCONNECTED";
                btnDiscordConnect.Content = "CONNECT";
                DiscordUIState(UIState.disabled);
            }
        }

        private void DiscordUIState(UIState uiState)
        {
            if (uiState == UIState.enabled)
            {
                scrlDiscordChannels.Visibility = Visibility.Visible;
                scrlDiscordUsers.Visibility = Visibility.Visible;
                tbDiscordMessageBox.IsReadOnly = false;
                tbDiscordMessageBox.IsEnabled = true;
                btnDiscordChatSend.IsEnabled = true;
                return;
            }
            
            if (uiState == UIState.disabled)
            {
                scrlDiscordChannels.Visibility = Visibility.Collapsed;
                scrlDiscordUsers.Visibility = Visibility.Collapsed;
                tbDiscordMessageBox.IsReadOnly = true;
                tbDiscordMessageBox.IsEnabled = false;
                btnDiscordChatSend.IsEnabled = false;
                return;
            }
        }

        public enum UIState
        {
            enabled,
            disabled
        }

        public async void UpdateUsers()
        {
            // call the refreshusers method on a different thread
            ThreadPool.QueueUserWorkItem(o =>
            {
                RefreshUsers();
            });
        }

        public async void RefreshUsers()
        {
            await mw.Dispatcher.BeginInvoke((Action)(() =>
            {
                // get all user buttons
                var controls = UIHandler.GetChildren(DiscordUserListWrapPanel);
                UserButtons = controls.Buttons;

                // get a copy of the DiscordChannels data object
                var usrs = users.Users.OrderBy(a => a.UserName).ToList();

                List<Button> bTemp = new List<Button>();

                // iterate through each user
                for (int i = 0; i < usrs.Count(); i++)
                {
                    // if the user button already exists, update it
                    var cr = UserButtons.Where(a => a.Name == "btnDiscordUs" + usrs[i].UserId).FirstOrDefault();
                    if (cr != null)
                    {
                        if (cr.Content.ToString() != usrs[i].UserName)
                        {
                            cr.Content = usrs[i].UserName;
                            bTemp.Add(cr);
                        }

                        continue;
                    }

                    // update completed. now button creation..

                    // create the channel button
                    Button b = new Button();
                    b.Name = "btnDiscordUs" + usrs[i].UserId;
                    b.Content = usrs[i].UserName;
                    //b.AddHandler(Button.ClickEvent, new RoutedEventHandler(mw.btnDiscordChannel_Checked));

                    // width binding
                    /*
                    Binding b = new Binding("Width");
                    b.ElementName = "DiscordSelectorWrapPanel";
                    b.Path = new PropertyPath(DiscordSelectorWrapPanel.Width);
                    rb.SetBinding(FrameworkElement.WidthProperty, b);
                    */

                    // styling
                    /*
                    Style style = new Style(typeof(RadioButton));
                    style.BasedOn = (Style)mw.TryFindResource(typeof(ToggleButton));
                    rb.Style = style;
                    */

                    // add button to UI
                    DiscordUserListWrapPanel.Children.Add(b);

                    bTemp.Add(b);
                }

            }));
        }
        
        public async void UpdateChannelButtons()
        {
            // call the refreshchannels method on a different thread
            ThreadPool.QueueUserWorkItem(o =>
            {
                RefreshChannels();
            });
        }

        private async void RefreshChannels()
        {
            await mw.Dispatcher.BeginInvoke((Action)(() =>
            {
                // get all channel buttons
                var controls = UIHandler.GetChildren(DiscordSelectorWrapPanel);
                ChannelRadios = controls.RadioButtons;

                // get a copy of the DiscordChannels data object
                var chans = channels.Data.ToList();

                List<RadioButton> rbTemp = new List<RadioButton>();

                // iterate through each channel
                for (int i = 0; i < chans.Count(); i++)
                {
                    // if the channel button already exists, update it
                    var cr = ChannelRadios.Where(a => a.Name == "rbDiscordCh" + chans[i].ChannelId).FirstOrDefault();
                    if (cr != null)
                    {
                        if (cr.Content.ToString() != chans[i].ChannelName)
                        {
                            cr.Content = chans[i].ChannelName;
                            rbTemp.Add(cr);
                        }
                            
                        continue;
                    }

                    // update completed. now button creation..

                    // create the channel button
                    RadioButton rb = new RadioButton();
                    rb.Name = "rbDiscordCh" + chans[i].ChannelId;
                    rb.Content = chans[i].ChannelName;
                    rb.GroupName = "grpDiscordChannels";
                    rb.AddHandler(Button.ClickEvent, new RoutedEventHandler(mw.btnDiscordChannel_Checked));

                    // width binding
                    /*
                    Binding b = new Binding("Width");
                    b.ElementName = "DiscordSelectorWrapPanel";
                    b.Path = new PropertyPath(DiscordSelectorWrapPanel.Width);
                    rb.SetBinding(FrameworkElement.WidthProperty, b);
                    */

                    // styling
                    Style style = new Style(typeof(RadioButton));
                    style.BasedOn = (Style)mw.TryFindResource(typeof(ToggleButton));
                    rb.Style = style;

                    // add button to UI
                    DiscordSelectorWrapPanel.Children.Add(rb);

                    rbTemp.Add(rb);
                }

                /*
                // if active channel is not set, click the button
                if (channels.ActiveChannel == 0)
                {
                    var chan = channels.Data.FirstOrDefault();
                    if (chan != null)
                    {
                        RadioButton b = rbTemp.Where(a => a.Name == "rbDiscordCh" + chan.ChannelId).FirstOrDefault();
                        if (b != null)
                        {
                            b.IsChecked = true;
                        }
                    }
                }
                */
            }));
        }

        public void ChangeChannel(int channelId)
        {
            var chan = channels.Data.Where(a => a.ChannelId == channelId).FirstOrDefault();
            if (chan == null)
                return;

            channels.ActiveChannel = channelId;

            // set the flowdocument of the richtextbox to be the correct one for the selected channel
            paragraph = chan.Paragraph;
            rtbDocument.Document = new FlowDocument(paragraph);
        }

        // static methods

        public static void Disconnected()
        {

        }
    }
}
