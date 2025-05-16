using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IRoleRepository
    {
        IRequestResult GetAll(IRequestParameter parameter);
        IRequestResult AddRole(IRequestParameter parameter);
        Task<IRequestResult> DeleteRole(IRequestParameter parameter);

    }
}
