using YGate.Interfaces.Shared.Based;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Application.Advanced;

namespace YGate.Interfaces.Advanced
{
    public interface ISession : IIpaddress, IValue, ICreateDate, ICountry, ILanguage, ITheme
    {

    }
}
