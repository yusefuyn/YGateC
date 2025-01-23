using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.Models.Chat
{
    public class User
    {
        public User(string username, string connectionID)
        {
            Username = username;
            ConnectionID = connectionID;
        }

        public string Username { get; set; }
        public string ConnectionID { get; set; }
    }
}
