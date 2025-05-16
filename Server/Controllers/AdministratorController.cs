using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;
using YGate.BusinessLayer.EFCore;
using YGate.BusinessLayer.EFCore.Abstracts;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorized("Administrator")]
    public class AdministratorController : Controller
    {
        Operations operations;
        IHubContext<MyHub> hub;
        IAdministratorRepository administratorRepository;

        public AdministratorController(IHubContext<MyHub> hub)
        {
            this.hub = hub;
        }

        [HttpPost]
        public IActionResult UpdateDatabase([FromBody] RequestParameter test = null)
        {
            operations.DBUpdate();
            Console.WriteLine("Veri tabanı güncellendi.");
            return Ok();
        }

        [HttpPost]
        public async Task<IRequestResult> Setup([FromBody] RequestParameter test = null)
        {
            return await administratorRepository.Setup(test);
        }


        [HttpPost]
        public async Task<IRequestResult> ChangeSiteName([FromBody] RequestParameter parameter)
        {
            await hub.Clients.Groups("SideBar").SendAsync("RefreshSiteName");
            StaticTools.SiteName = parameter.Parameters.ToString();
            return await administratorRepository.ChangeSiteName(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> GetAllUser([FromBody] RequestParameter pars = null)
        {
            return await administratorRepository.GetAllUser(pars);
        }

        [HttpPost]
        public async Task<IRequestResult> GetUser([FromBody] RequestParameter parameter)
        {
            return await administratorRepository.GetUser(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> RemoveBlockedIpList([FromBody] RequestParameter parameter)
        {

            RequestResult result = new($"Remove Blocked Ip List To Ip {parameter.Parameters.ToString()}");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetBlockedIpAddress();
            var removedIp = StaticTools.BlockedIp.FirstOrDefault(xd => xd == parameter.Parameters.ToString());
            StaticTools.BlockedIp.Remove(removedIp);
            return result;
        }

        [HttpPost]
        public IRequestResult GetBlockedIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get Block List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetBlockedIpAddress();
            return result;
        }

        [HttpPost]
        public IRequestResult GetConnectIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get Connect Ip List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetConnectIpList();
            return result;
        }

        private List<string> GetConnectIpList() => StaticTools.IpAndDate.Select(xd => xd.Ip).Distinct().ToList();

        [HttpPost]
        public IRequestResult GetWhiteIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get White List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetWhiteIpList();
            return result;

        }

        [HttpPost]
        public IRequestResult AddBlockList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"AddBlockList {parameter.Parameters.ToString()}");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.BlockedIp.Add(parameter.Parameters.ToString());
            result.Object = GetBlockedIpAddress();
            return result;
        }

        [HttpPost]
        public IRequestResult AddWhiteIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"AddWhiteIpList {parameter.Parameters.ToString()}");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.WhiteList.Add(parameter.Parameters.ToString());
            result.Object = GetWhiteIpList();
            return result;
        }




        private List<string> GetWhiteIpList() => StaticTools.WhiteList.ToList();

        private List<string> GetBlockedIpAddress()
        {
            return StaticTools.BlockedIp.ToList();
        }


        [HttpPost]
        public async Task<IRequestResult> VerifyUser([FromBody] RequestParameter parameter)
        {
            return await administratorRepository.VerifyUser(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> BanUser([FromBody] RequestParameter parameter)
        {
            return await administratorRepository.BanUser(parameter);
        }


        [HttpPost]
        public async Task<IRequestResult> ChangeRole([FromBody] RequestParameter parameter)
        {
            return await administratorRepository.ChangeRole(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> UserIsActiveFalse([FromBody] RequestParameter parameter = null)
        {
            return await administratorRepository.UserIsActiveFalse(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> UserIsActiveTrue([FromBody] RequestParameter parameter = null)
        {
            return await administratorRepository.UserIsActiveTrue(parameter);
        }

        [HttpPost]
        public async Task<IRequestResult> DeleteRoleAccountToObjectGuid([FromBody] RequestParameter parameter)
        {
            return await administratorRepository.DeleteRoleAccountToObjctGuid(parameter);
        }
    }
}
