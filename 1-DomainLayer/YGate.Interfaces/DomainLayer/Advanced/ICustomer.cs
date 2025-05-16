using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;


namespace YGate.Interfaces.Advanced
{
    public interface ICustomer : IIndexable, IName, IDescription, IAddress,ICountry, IDBObject
    {
    }
}
