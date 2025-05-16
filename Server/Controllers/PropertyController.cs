using Microsoft.AspNetCore.Mvc;
using System;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PropertyController: Controller
    {
        Operations operations;
        IBaseFacades baseFacades;
        public PropertyController(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }

        [HttpPost]
        public async Task<string> GetAllPropertyViewModel([FromBody] RequestParameter parameter = null)
        {
            RequestResult returned = new("Get All Property But ViewModel");
            returned.Result = EnumRequestResult.Success;
            List<PropertyGroup> objs = operations.Context.PropertyGroups.ToList();
            List<PropertyGroupViewModel> returnedList = YGate.Mapping.Operations.ConvertToList<PropertyGroupViewModel>(objs);
            returnedList.ForEach(o => o.Values = operations.Context.PropertyGroupValues.Where(xd => xd.PropertyGroupGuid == o.DBGuid).ToList());
            returned.Object = returnedList;
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        

        [HttpPost]
        public async Task<string> AddGroup([FromBody] RequestParameter parameter)
        {
            PropertyGroup model = parameter.ConvertParameters<PropertyGroup>();
            RequestResult returned = new("Add PropertyGroup");
            returned.Result = EnumRequestResult.Success;
            model.DBGuid = YGate.String.Operations.GuidGen.Generate("PropertyGroup");
            operations.Context.PropertyGroups.Add(model);
            operations.Context.SaveChanges();
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public async Task<string> GetGroup([FromBody] RequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("get PropertyGroup");
            returned.Result = EnumRequestResult.Success;
            var res = operations.Context.PropertyGroups.FirstOrDefault(xd => xd.DBGuid == guid);
            returned.Object = res;
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public async Task<string> AddValues([FromBody] RequestParameter parameter)
        {
            List<PropertyGroupValue> values = parameter.ConvertParameters<List<PropertyGroupValue>>();
            RequestResult returned = new("Add PropertyGroupValue(s)");
            returned.Result = EnumRequestResult.Success;
            operations.Context.PropertyGroupValues.AddRange(values);
            operations.Context.SaveChanges();
            return baseFacades.JsonSerializer.Serialize(returned);
        }
    }
}
