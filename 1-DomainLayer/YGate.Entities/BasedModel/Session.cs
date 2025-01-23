using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Advanced;
using YGate.Interfaces.Application.Advanced;

namespace YGate.Entities.Based
{
    public class Session : ISession
    {
        public string IPAddress { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public Themes Theme { get; set; }
    }
}
