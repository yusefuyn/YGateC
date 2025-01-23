using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Numerics;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;

namespace YGate.Server.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]/[action]")]
    public class AdministratorController : Controller
    {
        Operations operations;
        public AdministratorController(Operations operations)
        {
            this.operations = operations;
        }

        [HttpPost]
        public IActionResult UpdateDatabase([FromBody] RequestParameter test = null)
        {
            var request = HttpContext.Request;
            operations.DBUpdate();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Setup([FromBody] RequestParameter test = null)
        {

            string PassHash = YGate.String.Operations.Hash.SaltAndSHA512("219619yusuf_");
            string AdministratorRoleDbGuid = YGate.String.Operations.GuidGen.Generate("Role");

            using (var trans = await operations.Context.Database.BeginTransactionAsync())
            {
                try
                {

                    var AdministratorRole = new Role()
                    {
                        DBGuid = AdministratorRoleDbGuid,
                        IsActive = true,
                        LongDescription = "Administrator Role",
                        ShortDescription = "Administrator Role",
                        Name = "Administrator",
                        OwnerGuid = "Admin"
                    };
                    operations.Context.Roles.Add(AdministratorRole);

                    var UserRole = new Role()
                    {
                        Name = "User",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Standart Role",
                        OwnerGuid = "Admin",
                        ShortDescription = "Standart Role"
                    };
                    operations.Context.Roles.Add(UserRole);

                    var AllRole = new Role()
                    {
                        Name = "All",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Misafirlerde dahil herkez",
                        OwnerGuid = "Admin",
                        ShortDescription = "Misafirlerde dahil herkez"
                    };
                    operations.Context.Roles.Add(AllRole);

                    var AdminAccount = new Account()
                    {
                        Email = "Yussefuynstein@gmail.com",
                        IsActive = true,
                        OwnerGuid = "Admin",
                        DBGuid = "Admin",
                        Password = PassHash,
                        Status = Entities.BasedModel.AccountStatus.Verified,
                        Username = "Yussefuynstein"
                    };
                    operations.Context.Accounts.Add(AdminAccount);

                    var AdministratorAccountRole = new AccountRole()
                    {
                        DBGuid = YGate.String.Operations.GuidGen.Generate("AccountRole"),
                        FromGuid = "Admin",
                        IsActive = true,
                        IssueDate = DateTime.Now,
                        OwnerGuid = "Admin",
                        ToGuid = "Admin",
                        RoleGuid = AdministratorRoleDbGuid
                    };

                    operations.Context.AccountRoles.Add(AdministratorAccountRole);


                    operations.Context.AccountsPasswords.Add(new()
                    {
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        OwnerGuid = AdminAccount.DBGuid,
                        Password = PassHash
                    });


                    var PCBoyut = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Boyut",
                        ShortDescription = "Bilişim sistemlerinde boyut birimi.",
                        OwnerGuid = "Admin"
                    };
                    operations.Context.MeasurementCategories.Add(PCBoyut);

                    var Frekans = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Frekans",
                        OwnerGuid = "Admin"
                    };
                    operations.Context.MeasurementCategories.Add(Frekans);

                    var Adet = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Adet",
                        OwnerGuid = "Admin"
                    };
                    operations.Context.MeasurementCategories.Add(Adet);

                    var ParaBirimi = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Para birimi",
                        OwnerGuid = "Admin"
                    };
                    operations.Context.MeasurementCategories.Add(ParaBirimi);

                    operations.Context.SaveChanges();

                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "b",
                        Name = "Byte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "KB",
                        Name = "KiloByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "MB",
                        Name = "MegaByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "GB",
                        Name = "GigaByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "TB",
                        Name = "TeraByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        OwnerGuid = "Admin",
                        Symbol = "Hz",
                        Name = "Hertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        OwnerGuid = "Admin",
                        Symbol = "kHz",
                        Name = "KiloHertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        OwnerGuid = "Admin",
                        Symbol = "MHz",
                        Name = "MegaHertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        OwnerGuid = "Admin",
                        Symbol = "Adet",
                        Name = "Adet",
                        MeasurementCategoryGuid = Adet.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        MeasurementCategoryGuid = ParaBirimi.DBGuid,
                        Name = "Türk Lirası",
                        Symbol = "₺",
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        MeasurementCategoryGuid = ParaBirimi.DBGuid,
                        Name = "Dolar",
                        Symbol = "$",
                        OwnerGuid = "Admin"
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        MeasurementCategoryGuid = ParaBirimi.DBGuid,
                        Name = "Euro",
                        Symbol = "€",
                        OwnerGuid = "Admin"
                    });

                    operations.Context.SaveChanges();

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.DisposeAsync();
                }
            }


            return Ok();
        }


        [HttpPost]
        public string GetAllUser([FromBody] RequestParameter pars = null)
        {
            RequestResult result = new("GetAllUser AdministratorModule");
            result.Result = EnumRequestResult.Success;
            var Accounts = operations.Context.Accounts.ToList();
            // TODO : Daha sonra rollerini çek listeleyelim.
            result.Object = Accounts;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string GetUser([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"GetUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            result.Object = operations.GetAdministratorUserItem(parameter.Parameters.ToString());
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        // TODO : Admin kontrolü yapılacak
        public string RemoveBlockedIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Remove Blocked Ip List To Ip {parameter.Parameters.ToString()}");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            var removedIp = StaticTools.BlockedIp.FirstOrDefault(xd => xd == parameter.Parameters.ToString());
            StaticTools.BlockedIp.Remove(removedIp);
            result.Object = GetBlockedIpAddress();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        // TODO : Admin kontrolü yapılacak
        public string GetBlockedIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get Block List");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetBlockedIpAddress();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        // TODO : Admin kontrolü yapılacak
        public string AddBlockList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"AddBlockList {parameter.Parameters.ToString()}");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.BlockedIp.Add(parameter.Parameters.ToString());
            result.Object = GetBlockedIpAddress();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        private List<string> GetBlockedIpAddress() { 
            return StaticTools.BlockedIp.ToList();
        }


        [HttpPost]
        public string VerifyUser([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"VerifyUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            var user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.Status = AccountStatus.Verified;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string BanUser([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"BanUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.Status = AccountStatus.Banned;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        [HttpPost]
        public string ChangeRole([FromBody] RequestParameter parameter)
        {
            dynamic parameters = parameter.ConvertParameters<dynamic>();

            string guid = parameters["UserId"];
            string role = parameters["Rol"];
            string ToGuid = parameters["ToGuid"];

            RequestResult result = new($"ChangeRole User : {guid}  Role : {role} ToGuid : {ToGuid}");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == guid);
            Role roleo = operations.Context.Roles.SingleOrDefault(xd => xd.DBGuid == role);

            AccountRole accountRole = new()
            {
                DBGuid = YGate.String.Operations.GuidGen.Generate("AccountRole"),
                RoleGuid = roleo.DBGuid,
                OwnerGuid = "Admin",
                FromGuid = user.DBGuid,
                IsActive = true,
                IssueDate = DateTime.Now,
                ToGuid = ToGuid
            };

            operations.Context.AccountRoles.Add(accountRole);
            operations.Context.SaveChanges();

            result.Object = operations.GetAccount(user.DBGuid);
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string UserIsActiveFalse([FromBody] RequestParameter parameter = null)
        {
            RequestResult result = new($"IsActive User : {parameter.Parameters.ToString()}  Active : false AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.IsActive = false;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string UserIsActiveTrue([FromBody] RequestParameter parameter = null)
        {
            RequestResult result = new($"IsActive User : {parameter.Parameters.ToString()}  Active : true AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.IsActive = true;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string DeleteRoleAccountToObjectGuid([FromBody] RequestParameter parameter)
        {
            var dynobj = parameter.ConvertParameters<dynamic>();
            string AccountGuid = dynobj.AccountGuid;
            string RoleGuid = dynobj.RoleGuid;

            RequestResult result = new($"DeleteRoleAccountToObjectGuid : {AccountGuid}  {RoleGuid}");
            result.Result = EnumRequestResult.Success;

            var aroles = operations.Context.AccountRoles.SingleOrDefault(xd => xd.FromGuid == AccountGuid && xd.RoleGuid == RoleGuid);

            operations.Context.AccountRoles.Remove(aroles);
            operations.Context.SaveChanges();

            result.Object = operations.GetAccount(AccountGuid);
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }
    }
}
