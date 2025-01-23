using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.DataAccess.Entities;
using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        Operations operations;
        public RoleController(Operations operations)
        {
            this.operations = operations;
        }

        [HttpPost]
        // TODO : Request'i atanın Rolü administrator olan bunu çekebilir.
        public string GetAll([FromBody] RequestParameter parameter = null)
        {
            RequestResult result = new("Get All Role");
            result.Result = EnumRequestResult.Success;
            var objs = operations.Context.Roles.ToList();
            result.Object = objs;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        // TODO : Rolü administrator olan bunu ekleyebilir.
        public string AddRole([FromBody] RequestParameter parameter)
        {
            Role role = parameter.ConvertParameters<Role>();
            RequestResult result = new("Add role");
            role.IsActive = true;
            role.DBGuid = YGate.String.Operations.GuidGen.Generate("role");
            result.Result = EnumRequestResult.Success;
            operations.Context.Roles.Add(role);
            operations.Context.SaveChanges();
            result.Object = role;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }
        [HttpPost]
        // TODO : Rolü administrator olan bunu ekleyebilir.
        public async Task<string> DeleteRole([FromBody]RequestParameter parameter)
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

            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        
    }
}
