
using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MeasurementController : Controller
    {
        Operations operations;
        IBaseFacades baseFacades;
        public MeasurementController(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }


        [HttpPost]
        public string AddCategory([FromBody] RequestParameter parameter)
        {
            MeasurementCategory measurementCategory = parameter.ConvertParameters<MeasurementCategory>();
            RequestResult returned = new("Add Measurement Category");
            measurementCategory.DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementCategory");
            operations.Context.MeasurementCategories.Add(measurementCategory);
            operations.Context.SaveChanges();
            returned.Result = EnumRequestResult.Success;
            returned.Object = measurementCategory;
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public string GetAllCategory([FromBody] RequestParameter parameter = null) {
            RequestResult returned = new("Get Measurement Categories");
            returned.Result = EnumRequestResult.Success;
            List<MeasurementCategory> res = operations.Context.MeasurementCategories.Where(xd=> xd.IsActive == true).ToList();
            returned.Object = res;
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public string DeleteCategory([FromBody] RequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("Delete Measurement Categories");
            returned.Result = EnumRequestResult.Success;
            MeasurementCategory res = operations.Context.MeasurementCategories.FirstOrDefault(xd => xd.DBGuid == guid);
            operations.Context.MeasurementCategories.Remove(res);
            operations.Context.SaveChanges();
            return baseFacades.JsonSerializer.Serialize(returned);
        }


        [HttpPost]
        public async Task<string> GetAllUnit()
        {
            RequestResult returned = new($"Get Measurement Units");
            returned.Result = EnumRequestResult.Success;
            List<MeasurementUnit> resobj = operations.Context.MeasurementUnits.Where(xd=> xd.IsActive == true).ToList();
            returned.Object = resobj;
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public async Task<string> DeleteUnit([FromBody] RequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("Delete Measurement Unit");
            returned.Result = EnumRequestResult.Success;
            MeasurementUnit res = operations.Context.MeasurementUnits.FirstOrDefault(xd => xd.DBGuid == guid);
            operations.Context.MeasurementUnits.Remove(res);
            operations.Context.SaveChanges();
            return baseFacades.JsonSerializer.Serialize(returned);
        }

        [HttpPost]
        public async Task<string> AddUnit([FromBody] RequestParameter parameter)
        {
            MeasurementUnit model = parameter.ConvertParameters<MeasurementUnit>();
            RequestResult returned = new("Add Measurement Category");
            model.DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementUnit");
            operations.Context.MeasurementUnits.Add(model);
            operations.Context.SaveChanges();
            returned.Result = EnumRequestResult.Success;
            returned.Object = model;
            return baseFacades.JsonSerializer.Serialize(returned);
        }
    }
}
