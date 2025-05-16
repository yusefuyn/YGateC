using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.DataAccess.Entities;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Interfaces.DomainLayer;
using YGate.Server.Attributes;
using YGate.Server.Facades;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        Operations operations;
        IBaseFacades baseFacades;
        public RoleController(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }

        [HttpPost]
        [Authorized("Administrator")]
        public string GetAll([FromBody] RequestParameter parameter)
        {
            RequestResult result = new("Get All Role");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            var objs = operations.Context.Roles.ToList();
            result.Object = objs;
            return baseFacades.JsonSerializer.Serialize(result);
        }

        [HttpPost]
        [Authorized("Administrator")]
        public string AddRole([FromBody] RequestParameter parameter)
        {
            Role role = parameter.ConvertParameters<Role>();
            RequestResult result = new("Add role");
            result.To = EnumTo.Server;
            result.Result = EnumRequestResult.Success;

            role.IsActive = true;
            role.DBGuid = YGate.String.Operations.GuidGen.Generate("role");
            operations.Context.Roles.Add(role);
            operations.Context.SaveChanges();
            result.Object = role;
            return baseFacades.JsonSerializer.Serialize(result);
        }
        [HttpPost]
        [Authorized("Administrator")]
        public async Task<string> DeleteRole([FromBody] RequestParameter parameter)
        {
            string roledbguid = parameter.Parameters.ToString();
            RequestResult result = new($"Delete role {roledbguid}");
            result.Result = EnumRequestResult.Success;

            using (var transaction = await operations.Context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = operations.Context.Roles.FirstOrDefault(xd => xd.DBGuid == roledbguid);
                    operations.Context.Roles.Remove(obj);

                    var objs = operations.Context.AccountRoles.Where(xd => xd.RoleGuid == roledbguid);
                    operations.Context.AccountRoles.RemoveRange(objs);

                    operations.Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return baseFacades.JsonSerializer.Serialize(result);
        }



    }
}
