using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IAdministratorRepository
    {
        Task<IRequestResult> BanUser(IRequestParameter parameter);
        Task<IRequestResult> ChangeRole(IRequestParameter parameter);
        Task<IRequestResult> ChangeSiteName(IRequestParameter parameter);
        Task<IRequestResult> DeleteRoleAccountToObjctGuid(IRequestParameter parameter);
        Task<IRequestResult> GetAllUser(IRequestParameter pars);
        Task<IRequestResult> GetUser(IRequestParameter parameter);
        Task<IRequestResult> Setup(IRequestParameter test);
        Task<IRequestResult> UserIsActiveFalse(IRequestParameter parameter);
        Task<IRequestResult> UserIsActiveTrue(IRequestParameter parameter);
        Task<IRequestResult> VerifyUser(IRequestParameter parameter);
    }
}
