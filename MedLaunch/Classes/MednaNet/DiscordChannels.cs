using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordChannels
    {
        public List<DiscordChannel> Data { get; set; }
        public int ActiveChannel { get; set; }

        public DiscordChannels()
        {
            Data = new List<DiscordChannel>();

            Data = new List<DiscordChannel>
            {
                new DiscordChannel()
                {
                    ChannelId = 1,
                    ChannelName = "general",
                    Paragraph = new Paragraph()
                },
                new DiscordChannel()
                {
                    ChannelId = 5,
                    ChannelName = "support",
                    Paragraph = new Paragraph()
                },
                new DiscordChannel()
                {
                    ChannelId = 2,
                    ChannelName = "notifications",
                    Paragraph = new Paragraph()
                },
                new DiscordChannel()
                {
                    ChannelId = 3,
                    ChannelName = "netplay",
                    Paragraph = new Paragraph()
                },
                new DiscordChannel()
                {
                    ChannelId = 4,
                    ChannelName = "development",
                    Paragraph = new Paragraph()
                },


            };

            /*
            // temp data
            for (int i = 1; i < 6; i++)
            {
                DiscordChannel d = new DiscordChannel();
                d.ChannelId = i;
                d.ChannelName = "Dynamic Channel " + i;
                d.Paragraph = new Paragraph();
                Data.Add(d);
            }
            */
        }

        /// <summary>
        /// Adds or Updates specific discord channel data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="paragraph"></param>
        public void UpdateChannel(int id, string name, Paragraph paragraph)
        {
            var lookup = Data.Where(a => a.ChannelId == id).FirstOrDefault();

            if (lookup == null)
            {
                // channel does not exist - add it to the collection
                DiscordChannel dc = new DiscordChannel();
                dc.ChannelId = id;
                dc.ChannelName = name;
                dc.Paragraph = new Paragraph();
                Data.Add(dc);                
            }
            else
            {
                // channel already exists - update the paragraph
                lookup.ChannelName = name;
                lookup.Paragraph = paragraph;
            }
        }
    }

    public class DiscordChannel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public Paragraph Paragraph { get; set; }
    }
}
