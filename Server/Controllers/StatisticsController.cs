using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Entities.ViewModels;
using System.Data.Entity;
using YGate.Interfaces.DomainLayer;
using System.Threading.Tasks;
using YGate.Interfaces.OperationLayer.Repositories;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StatisticsController : Controller
    {
        IStatisticsRepository statisticsRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        [HttpPost]
        public IRequestResult GetStatistics([FromBody] RequestParameter parameter = null)
        { // TODO : Eş zamanlı sorunu var hem configure ederken data çekmeye çalışınca patlıyor.
          return statisticsRepository.GetStatistics(parameter);
        }

        [HttpPost]
        public IRequestResult GetRequestLogs([FromBody] RequestParameter parameter = null)
        {
            var returned = statisticsRepository.GetRequestLogs(parameter);
            returned.Object = StaticTools.IpAndDate;
            return returned;
        }

        [HttpPost]
        public IRequestResult GetSiteName([FromBody] RequestParameter parameter = null)
        {
            var returned = statisticsRepository.GetSiteName(parameter);
            returned.Object = StaticTools.SiteName;
            return returned;
        }
    }
}
