
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IMeasurementRepository
    {
        IRequestResult AddCategory(IRequestParameter parameter);
        IRequestResult GetAllCategory(IRequestParameter parameter = null);
        IRequestResult DeleteCategory(IRequestParameter parameter);
        Task<IRequestResult> GetAllUnit();
        Task<IRequestResult> DeleteUnit(IRequestParameter parameter);
        Task<IRequestResult> AddUnit(IRequestParameter parameter);
    }
}
