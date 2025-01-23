using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels.Chat
{
    public class Message
    {
        public Message(string username, string message)
        {
            Username = username;
            Data = message;
            Date = DateTime.Now;
        }

        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string Data { get; set; }
    }
}
