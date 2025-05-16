using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.DataAccess.Entities;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {

        IRoleRepository roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        [HttpPost]
        [Authorized("Administrator")]
        public IRequestResult GetAll([FromBody] RequestParameter parameter)
        {
            return roleRepository.GetAll(parameter);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public IRequestResult AddRole([FromBody] RequestParameter parameter)
        {
            return roleRepository.AddRole(parameter);

        }
        [HttpPost]
        [Authorized("Administrator")]
        public async Task<IRequestResult> DeleteRole([FromBody] RequestParameter parameter)
        {
            return await roleRepository.DeleteRole(parameter);

        }



    }
}
