using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities
{
    public class RequestLogObject
    {
        public string Ip { get; set; }
        public List<DateTime> RequestTime { get; set; }
        public RequestLogObject(string Ip)
        {
            RequestTime = new List<DateTime>();
            this.Ip = Ip;
            AddRequest();
        }

        public void AddRequest() { RequestTime.Add(DateTime.UtcNow); }
    }
}
