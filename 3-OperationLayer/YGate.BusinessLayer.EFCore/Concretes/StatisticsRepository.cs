using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.ViewModels;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;

namespace YGate.BusinessLayer.EFCore.Concretes
{
    public class StatisticsRepository : IStatisticsRepository
    {
        Operations operations;
        public StatisticsRepository(Operations operations)
        {
            this.operations = operations;
        }
        public IRequestResult GetRequestLogs(IRequestParameter parameter = null)
        {
            RequestResult returned = new("GetRequestLogs");
            returned.Result = EnumRequestResult.Success;
            returned.To = EnumTo.Server;
            return returned;
        }

        public IRequestResult GetSiteName(IRequestParameter parameter = null)
        {
            RequestResult returned = new("GetSiteName");
            returned.Result = EnumRequestResult.Success;
            returned.To = EnumTo.Server;
            return returned;
        }

        public IRequestResult GetStatistics(IRequestParameter parameter = null)
        {
            RequestResult returned = new("GetStatistics");
            StatisticsViewModel model = new StatisticsViewModel();
            returned.To = EnumTo.Server;

            YGate.Operation.Runner.TryCatchRunner(() =>
            {
                model.UserCount = operations.Context.Accounts.Count();
                model.IdentifiedEntityCount = operations.Context.Categories.Count();
                model.EntityPropertyCount = operations.Context.CategoryTemplateValues.Count();
                model.CreatedEntityCount = operations.Context.Entities.Count();
            }, () =>
            {
                model.UserCount = 0;
                model.IdentifiedEntityCount = 0;
                model.EntityPropertyCount = 0;
                model.CreatedEntityCount = 0;
            });
            returned.Result = EnumRequestResult.Success;
            returned.Object = model;
            return returned;
        }
    }
}
