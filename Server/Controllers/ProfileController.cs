using Microsoft.AspNetCore.Mvc;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProfileController : Controller
    {
        Operations operations;
        public ProfileController(Operations operations)
        {
            this.operations = operations;
        }

        [HttpPost]
        public string GetMyProperties([FromBody] RequestParameter parameter)
        {
            string UserID = parameter.Parameters.ToString();
            RequestResult result = new("GetMyProperties");
            var objs = operations.Context.AccountProperties.Where(xd => xd.OwnerGuid == UserID);
            result.Object = objs;
            result.Result = EnumRequestResult.Success;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string AddPropertiesToProfile([FromBody] RequestParameter parameter)
        {
            AccountProperties accountProperties = parameter.ConvertParameters<AccountProperties>();
            RequestResult result = new("Add Properties");
            operations.Context.AccountProperties.Add(accountProperties);
            operations.Context.SaveChanges();
            result.Result = EnumRequestResult.Success;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string GetProfileByUserGuid([FromBody] RequestParameter parameter)
        {

            string userGuid = parameter.Parameters.ToString();
            RequestResult returned = new($"Get Profile by {userGuid}");
            returned.Result = EnumRequestResult.Success;

            var user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == userGuid);
            var userReferance = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == user.OwnerGuid.ToString());
            List<string> userARole = operations.Context.AccountRoles
                .Where(xd => xd.ToGuid == user.DBGuid)
                .Select(xd=> xd.RoleGuid)
                .Distinct()
                .ToList();

            UserPublicViewModel model = new()
            {
                Username = user.Username,
                Status = user.Status.ToString(),
                ReferanceName = userReferance.Username.ToString(),
                PublicProperties = operations.Context.AccountProperties.Where(xd => xd.OwnerGuid == user.DBGuid).ToList(),
                PublicRoles = new(),
                PublicEntities = operations.GetEntitieListByUserDBGuid(userGuid)
            };

            userARole.ForEach(xd=> {
                var role = operations.Context.Roles.FirstOrDefault(rr =>  rr.DBGuid == xd);
                model.PublicRoles.Add(role);
            });

            returned.Object = model;

            return YGate.Json.Operations.JsonSerialize.Serialize(returned);
        }
    }
}
