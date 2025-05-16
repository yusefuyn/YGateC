using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.BusinessLayer.EFCore.Concretes
{
    public class MeasurementRepository : IMeasurementRepository
    {
        IBaseFacades baseFacades;
        Operations operations;

        public MeasurementRepository(IBaseFacades baseFacades, Operations operations)
        {
            this.baseFacades = baseFacades;
            this.operations = operations;
        }

        public IRequestResult AddCategory(IRequestParameter parameter)
        {
            MeasurementCategory measurementCategory = baseFacades.JsonSerializer.Deserialize<MeasurementCategory>(parameter.Parameters.ToString());
            RequestResult returned = new("Add Measurement Category");
            measurementCategory.DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementCategory");
            operations.Context.MeasurementCategories.Add(measurementCategory);
            operations.Context.SaveChanges();
            returned.Result = EnumRequestResult.Success;
            returned.Object = measurementCategory;
            return returned;
        }

        public async Task<IRequestResult> AddUnit(IRequestParameter parameter)
        {
            MeasurementUnit model = baseFacades.JsonSerializer.Deserialize<MeasurementUnit>(parameter.Parameters.ToString());
            RequestResult returned = new("Add Measurement Category");
            model.DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementUnit");
            operations.Context.MeasurementUnits.Add(model);
            operations.Context.SaveChanges();
            returned.Result = EnumRequestResult.Success;
            returned.Object = model;
            return returned;
        }

        public IRequestResult DeleteCategory(IRequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("Delete Measurement Categories");
            returned.Result = EnumRequestResult.Success;
            MeasurementCategory res = operations.Context.MeasurementCategories.FirstOrDefault(xd => xd.DBGuid == guid);
            operations.Context.MeasurementCategories.Remove(res);
            operations.Context.SaveChanges();
            return returned;
        }

        public async Task<IRequestResult> DeleteUnit(IRequestParameter parameter)
        {
            string guid = parameter.Parameters.ToString();
            RequestResult returned = new("Delete Measurement Unit");
            returned.Result = EnumRequestResult.Success;
            MeasurementUnit res = operations.Context.MeasurementUnits.FirstOrDefault(xd => xd.DBGuid == guid);
            operations.Context.MeasurementUnits.Remove(res);
            operations.Context.SaveChanges();
            return returned;
        }

        public IRequestResult GetAllCategory(IRequestParameter parameter = null)
        {
            RequestResult returned = new("Get Measurement Categories");
            returned.Result = EnumRequestResult.Success;
            List<MeasurementCategory> res = operations.Context.MeasurementCategories.Where(xd => xd.IsActive == true).ToList();
            returned.Object = res;
            return returned;
        }

        public async Task<IRequestResult> GetAllUnit()
        {
            RequestResult returned = new($"Get Measurement Units");
            returned.Result = EnumRequestResult.Success;
            List<MeasurementUnit> resobj = operations.Context.MeasurementUnits.Where(xd => xd.IsActive == true).ToList();
            returned.Object = resobj;
            return returned;
        }
    }
}
