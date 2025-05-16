using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer.Repositories
{
    public interface IStatisticsRepository
    {
        IRequestResult GetStatistics(IRequestParameter parameter = null);
        IRequestResult GetRequestLogs(IRequestParameter parameter = null);
        IRequestResult GetSiteName(IRequestParameter parameter = null);
    }
}
