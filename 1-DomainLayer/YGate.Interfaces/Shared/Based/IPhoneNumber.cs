using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IPhoneNumber
    {
        public string CountyCode { get; set; }
        public string Number { get; set; }
    }
}
