using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Interfaces.Application.Advanced
{
    public interface IAccount :IPassword, IMail, IUsername, IDBObject, IIndexable
    {
    }
}
//Yussefuynstein