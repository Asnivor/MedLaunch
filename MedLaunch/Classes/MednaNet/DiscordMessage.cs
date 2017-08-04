using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLaunch.Classes.MednaNet
{
    public class DiscordMessage
    {
        public int messageId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public DateTime postedOn { get; set; }
        public string name { get; set; }
        public int channel { get; set; }
        public int userId { get; set; }
    }
}
