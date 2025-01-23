using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.Based
{
    public class LanguageValueAndDescription : IDescription, IValue
    {
        public string Value { get ; set ; }
        public string Description { get ; set ; }
    }
}
