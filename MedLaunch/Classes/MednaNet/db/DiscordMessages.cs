using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using MednaNetAPIClient;

namespace MedLaunch.Classes.MednaNet.db
{
    public class DiscordMessages
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public bool IsAPIUser { get; set; }
        public string NickName { get; set; }
        public string MessageString { get; set; }
        public DateTime APITimeReceived { get; set; }
        public DateTime LocalTimeReceived { get; set; }
        public string MessageFormatted { get; set; }


        

        /* Get Methods */

        /// <summary>
        /// returns all messages
        /// </summary>
        /// <returns></returns>
        public static List<DiscordMessages> GetAllMessages()
        {
            using (var dCon = new MedLaunch.Classes.MednaNet.db.MednaLogDbContext())
            {
                var cData = (from g in dCon.DiscordMessages
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return list of all messages from a channel
        /// </summary>
        /// <returns></returns>
        public static List<DiscordMessages> GetAllMessages(int channelId)
        {
            using (var context = new MednaLogDbContext())
            {
                var cData = (from g in context.DiscordMessages
                             where g.ChannelId == channelId
                             select g);
                return cData.ToList();
            }
        }

        /// <summary>
        /// return the last message object
        /// </summary>
        /// <returns></returns>
        public static DiscordMessages GetLastMessage()
        {
            using (var context = new MednaLogDbContext())
            {
                var cData = (from g in context.DiscordMessages
                             orderby g.MessageId
                             select g).LastOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// return the last message object id
        /// </summary>
        /// <returns></returns>
        public static int GetLastMessageId()
        {
            using (var context = new MednaLogDbContext())
            {
                var cData = (from g in context.DiscordMessages
                             orderby g.MessageId
                             select g).LastOrDefault();

                if (cData == null)
                    return 0;

                else
                    return cData.MessageId;
            }
        }

        /// <summary>
        /// return the last message object based on channel
        /// </summary>
        /// <returns></returns>
        public static DiscordMessages GetLastMessage(int channelId)
        {
            using (var context = new MednaLogDbContext())
            {
                var cData = (from g in context.DiscordMessages
                             where g.ChannelId == channelId
                             orderby g.MessageId
                             select g).LastOrDefault();
                return cData;
            }
        }

        /// <summary>
        /// return the last message object id based on channel
        /// </summary>
        /// <returns></returns>
        public static int GetLastMessageId(int channelId)
        {
            using (var context = new MednaLogDbContext())
            {
                var cData = (from g in context.DiscordMessages
                             where g.ChannelId == channelId
                             orderby g.MessageId
                             select g).LastOrDefault();

                if (cData == null)
                    return 0;

                else
                    return cData.MessageId;
            }
        }

        


        /* Put Methods */

        /// <summary>
        /// takes an api message, converts it into a database format message
        /// </summary>
        /// <param name="apiMessages"></param>
        /// <returns></returns>
        public static void SaveToDatabase(List<MednaNetAPIClient.Models.Messages> apiMessages)
        {
            List<DiscordMessages> newList = new List<DiscordMessages>();

            foreach (var m in apiMessages)
            {
                DiscordMessages d = new DiscordMessages();
                d.APITimeReceived = m.postedOn;
                d.ChannelId = m.channel;
                d.IsAPIUser = false;
                d.LocalTimeReceived = DateTime.Now;
                d.MessageId = m.id;
                d.MessageString = m.message;
                d.NickName = m.name;
                d.UserId = 0;                       // waiting for api class update

                newList.Add(d);
            }

            SaveToDatabase(newList);
        }

        /// <summary>
        /// Saves to database
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static int[] SaveToDatabase(List<DiscordMessages> messages)
        {
            // get current rom list
            var current = DiscordMessages.GetAllMessages();

            int added = 0;
            int updated = 0;

            // create temp objects pre-database actions
            List<DiscordMessages> toAdd = new List<DiscordMessages>();
            List<DiscordMessages> toUpdate = new List<DiscordMessages>();

            // iterate through each incoming rom
            foreach (var r in messages)
            {
                // attempt rom lookup in current
                DiscordMessages l = (from a in current
                             where a.MessageId == r.MessageId
                             select a).ToList().FirstOrDefault();

                if (l == null)
                {
                    // no entry found
                    toAdd.Add(r);
                }
                else
                {
                    // entry found - update required fields
                    DiscordMessages dr = r;
                    dr.LocalTimeReceived = DateTime.Now;

                    toUpdate.Add(dr);
                }
            }

            using (var db = new MednaLogDbContext())
            {
                // check for duplicate keys
                var distinctToAdd = toAdd.GroupBy(x => x.MessageId).Select(g => g.OrderByDescending(x => x.MessageId).First());
                var distinctToUpdate = toUpdate.GroupBy(x => x.MessageId).Select(g => g.OrderByDescending(x => x.MessageId).First());

                // update existing entries
                db.DiscordMessages.UpdateRange(distinctToUpdate);
                // add new entries
                db.DiscordMessages.AddRange(distinctToAdd);

                db.SaveChanges();

                added = distinctToAdd.Count();
                updated = distinctToUpdate.Count();

                return new int[] { added, updated };
            }

        }
    }
}
