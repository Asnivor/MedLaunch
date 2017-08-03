using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordUsers
    {
        public List<DiscordUser> Users { get; set; }

        public DiscordUsers()
        {
            Users = new List<DiscordUser>();

            // temp data
            Users = new List<DiscordUser>
            {
                new DiscordUser()
                {
                    UserId = 1,
                    UserName = "JoE BloGgs",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 2,
                    UserName = "ff399jf f 39",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 3,
                    UserName = "R2D2",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
            };
        }

        public void UpdateUser(int userId, string userName, ClientType clientType, bool isOnline)
        {
            var lookup = Users.Where(a => a.UserId == userId).FirstOrDefault();

            if (lookup == null)
            {
                // user does not exist - create
                DiscordUser du = new DiscordUser();
                du.UserId = userId;
                du.UserName = userName;
                du.clientType = clientType;
                du.IsOnline = isOnline;
                Users.Add(du);
            }
            else
            {
                // user already exists - update
                lookup.UserName = userName;
                lookup.clientType = clientType;
                lookup.IsOnline = isOnline;
            }
        }
    }

    public class DiscordUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ClientType clientType { get; set; }
        public bool IsOnline { get; set; }
    }

    public enum ClientType
    {
        medlaunch,
        discord
    }
}
