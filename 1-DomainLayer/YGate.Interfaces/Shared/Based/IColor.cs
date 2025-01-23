using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IColor
    {
        [DefaultValue("#FFFFFFFF")]
        public string Color_From_Hex { get; set; }
    }
}
