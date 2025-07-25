﻿using Microsoft.AspNetCore.Components;
using YGate.Entities.ViewModels;
using YGate.Entities;
using YGate.Interfaces.OperationLayer;

namespace YGate.Client.Services.Administrator
{
    public class AdministratorService : IAdministratorService
    {
        HttpClientService httpClientService;
        IJsonSerializer jsonSerializer;

        public AdministratorService(HttpClientService httpClientService,IJsonSerializer jsonSerializer)
        {
            this.httpClientService = httpClientService;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task<RequestResult> ChangeRole(string UserGuid, string toGuid, string Role)
        {
            string para = jsonSerializer.Serialize(new { UserId = UserGuid, ToGuid = toGuid, Rol = Role });
            var res = await httpClientService.GetPostAsync<RequestResult>(para, "api/Administrator/ChangeRole");
            return res;
        }

        public async void DBUpdate()
        {
            RequestResult res = await httpClientService.GetPostAsync<RequestResult>("", "api/Administrator/UpdateDatabase");
        }

        public async Task<RequestResult> ChangeSiteName(string Name) {
            var res = await httpClientService.GetPostAsync<RequestResult>(Name, "api/Administrator/ChangeSiteName");
            return res;
        }


        public async Task<RequestResult> GetAllUser()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Administrator/GetAllUser");
            return res;
        }

        public async Task<RequestResult> GetUser(string Guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Administrator/GetUser");
            return res;
        }

        public async Task<RequestResult> UserBan(string Guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Administrator/BanUser");
            return res;
        }
        public async Task<RequestResult> VerifyUser(string Guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Administrator/VerifyUser");
            return res;
        }

        public async Task<RequestResult> UserIsActiveTrue(string Guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Administrator/UserIsActiveTrue");
            return res;
        }

        public async Task<RequestResult> UserIsActiveFalse(string Guid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(Guid, "api/Administrator/UserIsActiveFalse");
            return res;
        }

        public async Task<RequestResult> DeleteRoleAccountToObjectGuid(string AccountGuid, string RoleGuid)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(new { AccountGuid = AccountGuid, RoleGuid = RoleGuid }, "api/Administrator/DeleteRoleAccountToObjectGuid");
            return res;
        }

        public async Task<RequestResult> GetBlockedIpList() {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Administrator/GetBlockedIpList");
            return res;
        }
        public async Task<RequestResult> GetWhiteIpList()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Administrator/GetWhiteIpList");
            return res;
        }
        public async Task<RequestResult> GetConnectIpList()
        {
            var res = await httpClientService.GetPostAsync<RequestResult>("", "api/Administrator/GetConnectIpList");
            return res;
        }
        
        public async Task<RequestResult> RemoveBlockedListToIp(string ip) {
            var res = await httpClientService.GetPostAsync<RequestResult>(ip, "api/Administrator/RemoveBlockedIpList");
            return res;
        }
        public async Task<RequestResult> AddBlockedIpList(string ip)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(ip, "api/Administrator/AddBlockList");
            return res;
        }
        public async Task<RequestResult> AddWhiteIpList(string ip)
        {
            var res = await httpClientService.GetPostAsync<RequestResult>(ip, "api/Administrator/AddWhiteIpList");
            return res;
        }
        
    }
}
