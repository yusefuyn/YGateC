using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IEntitieRepository
    {
        Task<IRequestResult> AddEntitite(IRequestParameter parameter);
        Task<IRequestResult> Transfer(IRequestParameter parameter);
        Task<IRequestResult> GetEntitieViewModel(IRequestParameter parameter);
        Task<IRequestResult> GetAllEntitieViewModel(IRequestParameter parameter = null);
        Task<IRequestResult> GetAllMyEntitieViewModel(IRequestParameter parameter);
        Task<IRequestResult> GetAllEntitieButCategoryGuid(IRequestParameter parameter);
        Task<IRequestResult> GetAllEntitieButCategoryId(IRequestParameter parameter);
        Task<IRequestResult> GetEntitie(IRequestParameter parameter);
        Task<IRequestResult> Delete(IRequestParameter parameter);
    }
}
