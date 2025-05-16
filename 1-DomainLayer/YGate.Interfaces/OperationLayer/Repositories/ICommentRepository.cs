using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface ICommentRepository
    {
        Task<IRequestResult> Gets(IRequestParameter parameter);
        Task<IRequestResult> GetAll(IRequestParameter parameter);
        Task<IRequestResult> Delete(IRequestParameter parameter);
        Task<IRequestResult> Add(IRequestParameter parameter);

    }
}
