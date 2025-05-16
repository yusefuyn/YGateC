using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IPageRepository
    {
        Task<IRequestResult> GetPage(IRequestParameter parameter);
        Task<IRequestResult> MyParametersPages(IRequestParameter parameter);
        Task<IRequestResult> SavePageParameters(IRequestParameter parameter);
        Task<IRequestResult> GetPageForGuid(IRequestParameter parameter);
        Task<IRequestResult> GetPageForName(IRequestParameter parameter);
        Task<IRequestResult> UpdatePageObject(IRequestParameter parameter);
        Task<IRequestResult> GetPageButParameterPool(IRequestParameter parameter);
        Task<IRequestResult> AddPage(IRequestParameter parameter);
        Task<IRequestResult> GetAll(IRequestParameter parameter);
        Task<IRequestResult> Delete(IRequestParameter parameter);


    }
}
