using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Advanced;
using YGate.Interfaces.Shared.Based;

namespace YGate.Interfaces.Application.Advanced
{
    public interface IProperty: IValue, IShow, IDBObject, IIndexable, IObjectGuid , IImage,IPrice
    {

    }
}
