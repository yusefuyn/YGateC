using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface ILoginRegisterRepository
    {
        Task<IRequestResult> Login(IRequestParameter parameter);
        Task<IRequestResult> Register(IRequestParameter parameter);
    }
}
