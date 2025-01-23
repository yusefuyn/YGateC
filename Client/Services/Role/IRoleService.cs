using YGate.Entities;

namespace YGate.Client.Services.Role
{
    public interface IRoleService
    {
        public Task<RequestResult> GetAllRole();

        public Task<RequestResult> AddRole(YGate.Entities.BasedModel.Role role);
        public Task<RequestResult> DeleteRole(string dbguid);
    }
}
