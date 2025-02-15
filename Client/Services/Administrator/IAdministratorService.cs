using YGate.Entities;

namespace YGate.Client.Services.Administrator
{
    public interface IAdministratorService
    {
        public void DBUpdate();
        public Task<RequestResult> GetAllUser();
        public Task<RequestResult> GetUser(string Guid);
        public Task<RequestResult> UserBan(string Guid);
        public Task<RequestResult> VerifyUser(string Guid);
        public Task<RequestResult> ChangeRole(string UserGuid,string ToGuid, string Role);
        public Task<RequestResult> UserIsActiveFalse(string Guid);
        public Task<RequestResult> UserIsActiveTrue(string Guid);
        public Task<RequestResult> DeleteRoleAccountToObjectGuid(string AccountGuid,string RoleGuid);
        public Task<RequestResult> GetBlockedIpList();
        public Task<RequestResult> GetWhiteIpList();
        public Task<RequestResult> GetConnectIpList();

        public Task<RequestResult> RemoveBlockedListToIp(string ip);
        public Task<RequestResult> AddBlockedIpList(string ip);
        public Task<RequestResult> AddWhiteIpList(string ip);
        
        public Task<RequestResult> ChangeSiteName(string Name);
    }
}
