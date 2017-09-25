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

            /*
            // temp data
            Users = new List<DiscordUser>
            {
                new DiscordUser()
                {
                    UserId = 1,
                    UserName = "D. Hicks",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 2,
                    UserName = "W. Hudson",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 3,
                    UserName = "S. Gorman",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 4,
                    UserName = "J. Vasquez",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 100,
                    UserName = "A. Apone",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 200,
                    UserName = "C. Ferro",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 300,
                    UserName = "M. Drake",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 37,
                    UserName = "T. Crowe",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 3004,
                    UserName = "T. Wierzbowski",
                    clientType = ClientType.discord,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 375,
                    UserName = "R. Frost",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
                new DiscordUser()
                {
                    UserId = 37543,
                    UserName = "D. Spunkmeyer",
                    clientType = ClientType.medlaunch,
                    IsOnline = true
                },
            };
            */
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
