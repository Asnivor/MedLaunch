using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MednaNetAPIClient;
using MedLaunch.Models;
using MednaNetAPIClient.Models;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordVisualHandler
    {
        public MainWindow mw { get; set; }
        public List<RadioButton> ChannelRadios { get; set; }
        public List<Label> UserButtons { get; set; }
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
        public Expander expDiscordUsersOnline { get; set; }

        private Paragraph paragraph { get; set; }

        public DiscordChannels channels { get; set; }
        public DiscordUsers users { get; set; }

        public bool APIConnected { get; set; }

        /// <summary>
        /// default contructor
        /// </summary>
        public DiscordVisualHandler()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            APIConnected = false;

            // get misc controls
            tbDiscordName = (TextBox)mw.FindName("tbDiscordName");
            btnDiscordConnect = (Button)mw.FindName("btnDiscordConnect");
            scrlDiscordChannels = (ScrollViewer)mw.FindName("scrlDiscordChannels");
            scrlDiscordUsers = (ScrollViewer)mw.FindName("scrlDiscordUsers");
            tbDiscordMessageBox = (TextBox)mw.FindName("tbDiscordMessageBox");
            btnDiscordChatSend = (Button)mw.FindName("btnDiscordChatSend");
            DiscordSelectorWrapPanel = (StackPanel)mw.FindName("DiscordSelectorWrapPanel");
            DiscordUserListWrapPanel = (StackPanel)mw.FindName("DiscordUserListWrapPanel");
            expDiscordUsersOnline = (Expander)mw.FindName("expDiscordUsersOnline");

            // chat text window
            rtbDocument = (RichTextBox)mw.FindName("rtbDocument");

            // set username from database
            tbDiscordName.Text = MednaNetSettings.GetUsername();
       

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
            rtbDocument.IsDocumentEnabled = true;

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
        /// Begin sending a message to the chatbox
        /// </summary>
        /// <param name="message"></param>
        public async void PostMessage(DiscordMessage discordMessage)
        {
            // post message on another thread
            ThreadPool.QueueUserWorkItem(o =>
            {
                ChatUpdater(discordMessage);
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

                rtbDocument.ScrollToEnd();
            }));            
        }

        public void PostFromLocal(string message)
        {
            // build discordMessage object
            DiscordMessage dm = new DiscordMessage();
            dm.postedOn = DateTime.Now;
            dm.name = tbDiscordName.Text;
            dm.messageId = 0;
            dm.message = message;
            dm.channel = channels.ActiveChannel;
            

            // send to API
            //not yet implemented

            // send to chat window
            PostMessage(dm);

            rtbDocument.ScrollToEnd();
        }

        public void PostFromLocal(Messages message)
        {
            // build discordMessage object
            DiscordMessage dm = new DiscordMessage();
            dm.postedOn = DateTime.Now;
            dm.name = tbDiscordName.Text;
            dm.messageId = 0;
            dm.message = message.message;
            dm.channel = channels.ActiveChannel;


            // send to API
            //not yet implemented

            // send to chat window
            PostMessage(dm);

            rtbDocument.ScrollToEnd();
        }

        /// <summary>
        /// update the chatbox
        /// </summary>
        /// <param name="message"></param>
        private async void ChatUpdater(DiscordMessage discordMessage)
        {
            await mw.Dispatcher.BeginInvoke((Action)(() =>
            {
                // get channel paragraph
                var para = GetChannelParagraph(discordMessage.channel);

                // timestamp
                string stamp = discordMessage.postedOn.ToShortTimeString();
                para.Inlines.Add(new Run(" " + stamp + " ")
                {
                    Foreground = Brushes.Silver
                });

                // get user client type
                var usr = users.Users.Where(a => a.UserName == discordMessage.name).FirstOrDefault();

                // username formatting
                if (usr != null)
                {
                    if (usr.clientType == ClientType.discord)
                    {
                        para.Inlines.Add(new Run(" " + discordMessage.name + "   ")
                        {
                            Foreground = Brushes.CadetBlue
                        });
                    }
                    else if (usr.clientType == ClientType.medlaunch)
                    {
                        // see if this is YOUR username
                        if (usr.UserName == tbDiscordName.Text)
                        {
                            para.Inlines.Add(new Run(" " + discordMessage.name + "   ")
                            {
                                Foreground = Brushes.Red
                            });
                        }
                        else
                        {
                            para.Inlines.Add(new Run(" " + discordMessage.name + "   ")
                            {
                                Foreground = Brushes.ForestGreen
                            });
                        }

                        
                    }

                    else
                    {
                        // no formatting
                        para.Inlines.Add(new Run(" " + discordMessage.name + "   "));
                    }
                }
                else
                {
                    // user not listed in online users. local testing?
                    para.Inlines.Add(new Bold(new Run(" " + discordMessage.name + "   "))
                    {
                        Foreground = Brushes.DarkGray
                    });
                }

                //para.Inlines.Add(discordMessage.message);
                ParseMessage(para, discordMessage.message);
                para.Inlines.Add(new LineBreak());

                rtbDocument.ScrollToEnd();

            }));
        }

        public void ParseMessage(Paragraph para, string message)
        {
            DetectURLs(message, para);
        }

        private static readonly Regex UrlRegex = new Regex(@"(?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&amp;(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?");
        
        public void DetectURLs(string par, Paragraph para)
        {
            string paragraphText = par;

            // Split the paragraph by words
            string[] words = paragraphText.Split(' ');
            List<int> positionToReplace = new List<int>();

            //foreach (string word in paragraphText.Split(' ').ToList())

            // iterate through each word
            for (int i = 0; i < words.Length; i++)
            {
                // @mention parsing
                if (words[i].StartsWith("<@") && words[i].EndsWith(">"))
                {
                    // looks like this is a discord mention - trim it down and get the userid
                    positionToReplace.Add(i);

                    string mWord = words[i].Replace("<@", "").TrimEnd('>');

                    var user = MednaNetAPI.Instance.Users.Where(a => a.discordId == mWord).FirstOrDefault();
                    if (user != null)
                    {
                        //para.Inlines.Add("@" + user.username + " ");
                        para.Inlines.Add(new Bold(new Run("@" + user.username + " "))
                        {
                            Foreground = Brushes.BlueViolet
                        });

                        continue;
                    }                    
                }

                // #channel parsing
                if (words[i].StartsWith("<#") && words[i].EndsWith(">"))
                {
                    // looks like this is a discord mention - trim it down and get the userid
                    positionToReplace.Add(i);

                    string mWord = words[i].Replace("<#", "").TrimEnd('>');

                    var channel = MednaNetAPI.Instance.Channels.Where(a => a.discordId == mWord).FirstOrDefault();
                    if (channel != null)
                    {
                        //para.Inlines.Add("@" + user.username + " ");
                        para.Inlines.Add(new Bold(new Run("#" + channel.channelName + " "))
                        {
                            Foreground = Brushes.BlueViolet
                        });

                        continue;
                    }
                }

                // hyperlink parsing
                if (!IsHyperlink(words[i]))
                {
                    // word is not a hyperlink - just post it
                    para.Inlines.Add(words[i] + " ");
                }
                else
                {
                    // word is detected as a hyperlink
                    positionToReplace.Add(i);

                    Uri uri = new Uri(words[i], UriKind.RelativeOrAbsolute);

                    if (!uri.IsAbsoluteUri)
                    {
                        // Prepend it with http
                        uri = new Uri(@"http://" + words[i], UriKind.Absolute);
                    }

                    if (uri != null)
                    {
                        var link = new Hyperlink()
                        {
                            NavigateUri = uri,
                        };
                        link.IsEnabled = true;
                        link.Inlines.Add(words[i]);
                        link.Click += mw.Hyperlink_Click;

                        // post the hyperlink
                        para.Inlines.Add(link);
                        para.Inlines.Add(" ");
                    }
                    else
                    {
                        //just post word
                        para.Inlines.Add(words[i] + " ");
                    }

                    //
                }
            }
        }

        public static bool IsHyperlink(string word)
        {
            try
            {
                // First check to make sure the word has at least one of the characters we need to make a hyperlink
                if (word.IndexOfAny(@":.\/".ToCharArray()) != -1)
                {
                    if (Uri.IsWellFormedUriString(word, UriKind.Absolute))
                    {
                        // The string is an Absolute URI
                        return true;
                    }
                    else if (UrlRegex.IsMatch(word))
                    {
                        Uri uri = new Uri(word, UriKind.RelativeOrAbsolute);

                        if (!uri.IsAbsoluteUri)
                        {
                            // rebuild it it with http to turn it into an Absolute URI
                            uri = new Uri(@"http://" + word, UriKind.Absolute);
                        }

                        if (uri.IsAbsoluteUri)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        /*
                        try
                        {
                            Uri wordUri = new Uri(word);

                            // Check to see if URL is a network path
                            if (wordUri.IsUnc || wordUri.IsFile)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        */
                        
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
            
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
                UserButtons = controls.Labels;

                // get a copy of the DiscordUsers data object
                var usrs = users.Users.OrderBy(a => a.UserName).ToList();

                // remove old labels
                foreach (var l in UserButtons)
                {
                    DiscordUserListWrapPanel.Children.Remove(l);
                }
                

                // set online user count
                expDiscordUsersOnline.Header = "USERS ONLINE (" + usrs.Count() + ")";

                List<Label> bTemp = new List<Label>();

                // iterate through each user
                for (int i = 0; i < usrs.Count(); i++)
                {
                    // create image
                    Image img = new Image();
                    if (usrs[i].clientType == ClientType.discord)
                        img.Source = new BitmapImage(new Uri(@"Data/Graphics/Icons/usericon_discord.png", UriKind.Relative));
                    else
                        img.Source = new BitmapImage(new Uri(@"Data/Graphics/Icons/usericon_medlaunch.png", UriKind.Relative));
                    img.Height = 20;
                    img.Width = 20;
                    img.HorizontalAlignment = HorizontalAlignment.Left;

                    // textblock
                    TextBlock tb = new TextBlock();
                    tb.Text = usrs[i].UserName;
                    Thickness margin = tb.Margin;
                    margin.Left = 10;
                    tb.Margin = margin;
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.HorizontalAlignment = HorizontalAlignment.Left;


                    // button stackpanel
                    StackPanel stackPnl = new StackPanel();
                    stackPnl.Orientation = Orientation.Horizontal;
                    stackPnl.HorizontalAlignment = HorizontalAlignment.Left;
                    stackPnl.Children.Add(img);
                    stackPnl.Children.Add(tb);

                    Label l = new Label();
                    l.Name = "btnDiscordUs" + usrs[i].UserId;
                    l.Content = stackPnl;
                    l.HorizontalAlignment = HorizontalAlignment.Left;
                    l.HorizontalContentAlignment = HorizontalAlignment.Left;
                    Thickness mar = l.Margin;
                    mar.Bottom = 0;
                    mar.Top = 0;
                    mar.Left = 0;
                    mar.Right = 0;
                    l.Padding = new Thickness(0);

                    if (usrs[i].clientType == ClientType.discord)
                    {
                        l.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7289DA"));
                    }

                    else if (usrs[i].clientType == ClientType.medlaunch)
                    {
                        // get the current primary color and set the user label as this
                        var color = mw.discordChannelExpander.Background;
                        l.Foreground = color;
                    }

                    // if the user button already exists, update it
                    //var cr = UserButtons.Where(a => a.Name == "btnDiscordUs" + usrs[i].UserId).FirstOrDefault();
                    /*
                    if (cr != null)
                    {
                        if (cr.Content.ToString() != usrs[i].UserName)
                        {
                            cr.Content = usrs[i].UserName;
                            bTemp.Add(cr);
                        }

                        //continue;
                    }
                    else
                    {
                        // create new label
                    }
                    */

                    // add user back
                    DiscordUserListWrapPanel.Children.Add(l);





                    /*
                    // create the user button
                    Button b = new Button();
                    b.Name = "btnDiscordUs" + usrs[i].UserId;
                    b.Content = stackPnl;
                    b.HorizontalContentAlignment = HorizontalAlignment.Left;

                    DiscordUserListWrapPanel.Children.Add(b);
                    
                    bTemp.Add(b);
                    */
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
            rtbDocument.IsDocumentEnabled = true;
        }

        public void CheckChannelSelection()
        {
            UIHandler ui = UIHandler.GetChildren(DiscordSelectorWrapPanel);
            RadioButton rb = ui.RadioButtons.FirstOrDefault();

            if (rb == null)
            {
                // no channels have been populated yet
                return;
            }

            bool selected = false;

            foreach (RadioButton r in ui.RadioButtons)
            {
                if (r.IsChecked == true)
                {
                    selected = true;
                    break;
                }
            }

            if (selected == false)
            {
                // select the first channel
                string name = rb.Name;
                string idStr = name.Replace("rbDiscordCh", "");
                int id = Convert.ToInt32(idStr);
                ChangeChannel(id);
            }
        }

        // static methods

        public static void Disconnected()
        {

        }
    }
}
