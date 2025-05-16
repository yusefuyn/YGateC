using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;


namespace YGate.Interfaces.Application.Advanced
{
    public interface IProduct :IName, IPrice, IColor, IDescription, ITax, IComponent,IShow, IDBObject
    {
    }
}
