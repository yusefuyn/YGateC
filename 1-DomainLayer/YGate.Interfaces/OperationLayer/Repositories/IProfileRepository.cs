using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IProfileRepository
    {
        IRequestResult GetMyProperties(IRequestParameter parameter);
        IRequestResult AddPropertiesToProfile(IRequestParameter parameter);
        IRequestResult GetProfileByUserGuid(IRequestParameter parameter);

    }
}
