
using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MeasurementController : Controller
    {
        IMeasurementRepository measurementRepository;
        public MeasurementController(IMeasurementRepository measurementRepository)
        {
            this.measurementRepository = measurementRepository;
        }


        [HttpPost]
        public IRequestResult AddCategory([FromBody] RequestParameter parameter)
        {
            return measurementRepository.AddCategory(parameter);
        }

        [HttpPost]
        public IRequestResult GetAllCategory([FromBody] RequestParameter parameter = null)
        {
            return measurementRepository.GetAllCategory(parameter);
        }

        [HttpPost]
        public IRequestResult DeleteCategory([FromBody] RequestParameter parameter)
        {
            return measurementRepository.DeleteCategory(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> GetAllUnit()
        {
            return await measurementRepository.GetAllUnit();
        }

        [HttpPost]
        public async Task<IRequestResult> DeleteUnit([FromBody] RequestParameter parameter)
        {
            return await measurementRepository.DeleteUnit(parameter);   
        }

        [HttpPost]
        public async Task<IRequestResult> AddUnit([FromBody] RequestParameter parameter)
        {
            return await measurementRepository.AddUnit(parameter);
        }
    }
}
