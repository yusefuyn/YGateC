using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Entities.ViewModels;
using System.Data.Entity;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StatisticsController : Controller
    {
        Operations operations;
        public StatisticsController(Operations operations)
        {
            this.operations = operations;
        }

        [HttpPost]
        public RequestResult GetStatistics([FromBody] RequestParameter parameter = null)
        { // TODO : Eş zamanlı sorunu var hem configure ederken data çekmeye çalışınca patlıyor.
            RequestResult returned = new("GetStatistics");
            StatisticsViewModel model = new StatisticsViewModel();
            returned.To = EnumTo.Server;
            model.UserCount = operations.Context.Accounts.Count();
            model.IdentifiedEntityCount = operations.Context.Categories.Count();
            model.EntityPropertyCount = operations.Context.CategoryTemplateValues.Count();
            model.CreatedEntityCount = operations.Context.Entities.Count();
            returned.Result = EnumRequestResult.Success;
            returned.Object = model;
            return returned;
        }

        [HttpPost]
        public RequestResult GetRequestLogs([FromBody] RequestParameter parameter = null)
        {
            RequestResult returned = new("GetRequestLogs");
            returned.Result = EnumRequestResult.Success;
            returned.To = EnumTo.Server;
            returned.Object = StaticTools.IpAndDate;
            return returned;
        }
    }
}
