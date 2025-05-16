using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IPropertyRepository
    {
        Task<IRequestResult> GetAllPropertyViewModel(IRequestParameter parameter = null);
        Task<IRequestResult> AddGroup(IRequestParameter parameter);
        Task<IRequestResult> GetGroup(IRequestParameter parameter);
        Task<IRequestResult> AddValues(IRequestParameter parameter);
    }
}
