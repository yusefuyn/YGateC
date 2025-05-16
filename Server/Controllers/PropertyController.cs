using Microsoft.AspNetCore.Mvc;
using System;
using YGate.BusinessLayer.EFCore;
using YGate.BusinessLayer.EFCore.Concretes;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PropertyController: Controller
    {
        IPropertyRepository propertyRepository;

        public PropertyController(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        [HttpPost]
        public async Task<IRequestResult> GetAllPropertyViewModel([FromBody] RequestParameter parameter = null)
        {
            return await propertyRepository.GetAllPropertyViewModel(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> AddGroup([FromBody] RequestParameter parameter)
        {
            return await propertyRepository.AddGroup(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> GetGroup([FromBody] RequestParameter parameter)
        {
            return await propertyRepository.GetGroup(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> AddValues([FromBody] RequestParameter parameter)
        {
            return await propertyRepository.AddValues(parameter);
        }
    }
}
