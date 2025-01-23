using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IDiscount
    {
        [DefaultValue(0.0)]
        public double Discount { get; set; }
    }
}
