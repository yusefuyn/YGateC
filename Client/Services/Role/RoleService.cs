using YGate.Entities;

namespace YGate.Client.Services.Role
{
    public class RoleService : IRoleService
    {
        HttpClientService httpClientService;
        public RoleService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<RequestResult> GetAllRole() {
            var res = await httpClientService.GetPostAsync<RequestResult>("","api/Role/GetAll");
            return res; 
        }

        public async Task<RequestResult> AddRole(YGate.Entities.BasedModel.Role role)
        {
            return await httpClientService.GetPostAsync<RequestResult>(role, "api/Role/AddRole");
        }

        public async Task<RequestResult> DeleteRole(string dbguid) { 
            return await httpClientService.GetPostAsync<RequestResult>(dbguid, "api/Role/DeleteRole");

        }

    }
}
