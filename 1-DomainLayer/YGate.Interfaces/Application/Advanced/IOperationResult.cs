using YGate.Interfaces.Shared.Based;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Advanced
{
    public interface IOperationResult<T> : IDescription, IObject<T>
    {
    }
}
