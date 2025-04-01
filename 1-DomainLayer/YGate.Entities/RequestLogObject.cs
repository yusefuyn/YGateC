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
        public DateTime RequestTime { get; set; }
        public RequestLogObject(string Ip,string Path,string Data)
        {
            RequestTime = DateTime.UtcNow;
            this.Ip = Ip;
        }
    }
}
