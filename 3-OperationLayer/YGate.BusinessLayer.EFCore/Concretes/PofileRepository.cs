using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Mapping;
using YGate.Server.Facades;

namespace YGate.BusinessLayer.EFCore.Concretes
{
    public class ProfileRepository : IProfileRepository
    {
        Operations operations;
        IBaseFacades baseFacades;
        public ProfileRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }
        public IRequestResult AddPropertiesToProfile(IRequestParameter parameter)
        {
            AccountProperties accountProperties = baseFacades.JsonSerializer.Deserialize<AccountProperties>(parameter.Parameters.ToString());
            RequestResult result = new("Add Properties");
            operations.Context.AccountProperties.Add(accountProperties);
            operations.Context.SaveChanges();
            result.Result = EnumRequestResult.Success;
            return result;
        }

        public IRequestResult GetMyProperties(IRequestParameter parameter)
        {
            string UserID = parameter.Parameters.ToString();
            RequestResult result = new("GetMyProperties");
            var objs = operations.Context.AccountProperties.Where(xd => xd.CreatorGuid == UserID);
            result.Object = objs;
            result.Result = EnumRequestResult.Success;
            return result;
        }

        public IRequestResult GetProfileByUserGuid(IRequestParameter parameter)
        {
            string userGuid = parameter.Parameters.ToString();
            RequestResult returned = new($"Get Profile by {userGuid}");
            returned.Result = EnumRequestResult.Success;

            var user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == userGuid);
            Account userReferance = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == user.CreatorGuid.ToString());

            if (user.CreatorGuid.ToString() == "System")
            {
                userReferance = new() { Username = "System" };
            }
            List<string> userARole = operations.Context.AccountRoles
                .Where(xd => xd.ToGuid == user.DBGuid)
                .Select(xd => xd.RoleGuid)
                .Distinct()
                .ToList();


            UserPublicViewModel model = new()
            {
                Username = user.Username,
                Status = user.Status.ToString(),
                Guid = user.DBGuid.ToString(),
                ReferanceName = userReferance.Username.ToString(),
                PublicProperties = operations.Context.AccountProperties.Where(xd => xd.CreatorGuid == user.DBGuid).ToList(),
                PublicRoles = new(),
                PublicEntities = operations.GetEntitieListByUserDBGuid(userGuid)
            };

            userARole.ForEach(xd =>
            {
                var role = operations.Context.Roles.FirstOrDefault(rr => rr.DBGuid == xd);
                model.PublicRoles.Add(role);
            });

            returned.Object = model;

            return returned;
        }
    }
}
