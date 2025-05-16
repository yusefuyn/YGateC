using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Mapping;
using YGate.Server.Facades;


namespace YGate.BusinessLayer.EFCore.Concretes
{

    public class RoleRepository : IRoleRepository
    {
        Operations operations;
        IBaseFacades baseFacades;
        public RoleRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
            this.baseFacades = baseFacades;
        }

        public IRequestResult AddRole(IRequestParameter parameter)
        {
            Role role = baseFacades.JsonSerializer.Deserialize<Role>(parameter.Parameters.ToString());
            RequestResult result = new("Add role");
            result.To = EnumTo.Server;
            result.Result = EnumRequestResult.Success;

            role.IsActive = true;
            role.DBGuid = YGate.String.Operations.GuidGen.Generate("role");
            operations.Context.Roles.Add(role);
            operations.Context.SaveChanges();
            result.Object = role;
            return result;
        }

        public async Task<IRequestResult> DeleteRole(IRequestParameter parameter)
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

            return result;
        }

        public IRequestResult GetAll(IRequestParameter parameter)
        {
            RequestResult result = new("Get All Role");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            var objs = operations.Context.Roles.ToList();
            result.Object = objs;
            return result;
        }
    }
}
