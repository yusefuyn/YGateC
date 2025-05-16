using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Json.Operations;
using YGate.Mail.Operations;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginRegisterController
    {
        ILoginRegisterRepository loginRegisterRepository;
        public LoginRegisterController(ILoginRegisterRepository loginRegisterRepository)
        {
            this.loginRegisterRepository = loginRegisterRepository;
        }


        [HttpPost]
        public async Task<IRequestResult> Login([FromBody] RequestParameter parameter)
        {
            return await loginRegisterRepository.Login(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> Register([FromBody] RequestParameter parameter)
        {
            return await loginRegisterRepository.Register(parameter);
        }
    }
}
