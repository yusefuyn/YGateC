using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IComponent
    {
        [DefaultValue(true)]
        public bool Component { get; set; }
    }
}
