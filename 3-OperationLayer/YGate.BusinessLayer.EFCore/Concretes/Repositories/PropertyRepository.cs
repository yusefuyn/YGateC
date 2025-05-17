using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.ViewModels;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Mapping;
using YGate.Server.Facades;
using YGate.Entities.BasedModel;

namespace YGate.BusinessLayer.EFCore.Concretes.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        Operations operations;
        IBaseFacades baseFacades;
        public PropertyRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }
        public async Task<IRequestResult> AddGroup(IRequestParameter parameter)
        {
            PropertyGroup model = baseFacades.JsonSerializer.Deserialize<PropertyGroup>(parameter.Parameters.ToString());
            RequestResult returned = new("Add PropertyGroup");
            returned.Result = EnumRequestResult.Success;
            model.DBGuid = String.Operations.GuidGen.Generate("PropertyGroup");
            operations.Context.PropertyGroups.Add(model);
            operations.Context.SaveChanges();
            return returned;
        }

        public async Task<IRequestResult> AddValues(IRequestParameter parameter)
        {
            List<PropertyGroupValue> values = baseFacades.JsonSerializer.Deserialize<List<PropertyGroupValue>>(parameter.Parameters.ToString());
            RequestResult returned = new("Add PropertyGroupValue(s)");
            returned.Result = EnumRequestResult.Success;
            operations.Context.PropertyGroupValues.AddRange(values);
            operations.Context.SaveChanges();
            return returned;
        }

        public async Task<IRequestResult> GetAllPropertyViewModel(IRequestParameter parameter = null)
        {
            RequestResult returned = new("Get All Property But ViewModel");
            returned.Result = EnumRequestResult.Success;
            List<PropertyGroup> objs = operations.Context.PropertyGroups.ToList();
            List<PropertyGroupViewModel> returnedList = Mapping.Operations.ConvertToList<PropertyGroupViewModel>(objs);
            returnedList.ForEach(o => o.Values = operations.Context.PropertyGroupValues.Where(xd => xd.PropertyGroupGuid == o.DBGuid).ToList());
            returned.Object = returnedList;
            return returned;
        }

        public async Task<IRequestResult> GetGroup(IRequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("get PropertyGroup");
            returned.Result = EnumRequestResult.Success;
            var res = operations.Context.PropertyGroups.FirstOrDefault(xd => xd.DBGuid == guid);
            returned.Object = res;
            return returned;
        }
    }
}
