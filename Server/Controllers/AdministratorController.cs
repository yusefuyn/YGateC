using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data;
using System.Numerics;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Server.Attributes;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorized("Administrator")]
    public class AdministratorController : Controller
    {
        Operations operations;
        IHubContext<MyHub> hub;
        public AdministratorController(IHubContext<MyHub> hub,Operations operations)
        {
            this.operations = operations;
            this.hub = hub;
        }

        [HttpPost]
        public IActionResult UpdateDatabase([FromBody] RequestParameter test = null)
        {
            var request = HttpContext.Request;
            operations.DBUpdate();
            return Ok();
        }

        [HttpPost]
        public async Task<string> Setup([FromBody] RequestParameter test = null)
        {
            RequestResult result = new("Setup başarıyla yapıldı.");
            result.To = EnumTo.Server;
            result.Result = EnumRequestResult.Success;

            if (operations.Context.Accounts.Count() != 0)
            {
                result.Result = EnumRequestResult.Stop;
                result.LongDescription = "Setup zaten yapılmış.";
                return YGate.Json.Operations.JsonSerialize.Serialize(result);
            }



            string PassHash = YGate.String.Operations.Hash.SaltAndSHA512("219619yusuf_");
            string AdministratorRoleDbGuid = $"AdministratorRole-{DateTime.UtcNow.ToString().Replace(" ", "")}";
            string AdministratorUserDBGuid = "Administrator";

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
                        CreatorGuid = "System"
                    };
                    operations.Context.Roles.Add(AdministratorRole);

                    var UserRole = new Role()
                    {
                        Name = "User",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Standart Role",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = "Standart Role"
                    };
                    operations.Context.Roles.Add(UserRole);

                    var AllRole = new Role()
                    {
                        Name = "All",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Misafirlerde dahil herkez",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = "Misafirlerde dahil herkez"
                    };
                    operations.Context.Roles.Add(AllRole);

                    var MarketUserRole = new Role()
                    {
                        Name = "MarketUser",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = ""
                    };
                    operations.Context.Roles.Add(MarketUserRole);

                    var MarketModRole = new Role()
                    {
                        Name = "MarketModRole",
                        DBGuid = YGate.String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = ""
                    };
                    operations.Context.Roles.Add(MarketModRole);

                    var AdminAccount = new Account()
                    {
                        Email = "Yussefuynstein@gmail.com",
                        IsActive = true,
                        CreatorGuid = "System",
                        DBGuid = "Administrator",
                        Password = PassHash,
                        Status = Entities.BasedModel.AccountStatus.Verified,
                        Username = "Yussefuynstein"
                    };
                    operations.Context.Accounts.Add(AdminAccount);

                    var AdministratorAccountRole = new AccountRole()
                    {
                        DBGuid = YGate.String.Operations.GuidGen.Generate("AccountRole"),
                        FromGuid = "System",
                        IsActive = true,
                        IssueDate = DateTime.Now,
                        CreatorGuid = "System",
                        ToGuid = "Administrator",
                        RoleGuid = AdministratorRoleDbGuid
                    };

                    operations.Context.AccountRoles.Add(AdministratorAccountRole);


                    operations.Context.AccountsPasswords.Add(new()
                    {
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        CreatorGuid = AdminAccount.DBGuid,
                        Password = PassHash
                    });


                    var PCBoyut = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Boyut",
                        ShortDescription = "Bilişim sistemlerinde boyut birimi.",
                        CreatorGuid = AdministratorUserDBGuid
                    };
                    operations.Context.MeasurementCategories.Add(PCBoyut);

                    var Frekans = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Frekans",
                        CreatorGuid = AdministratorUserDBGuid
                    };
                    operations.Context.MeasurementCategories.Add(Frekans);

                    var Adet = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Adet",
                        CreatorGuid = AdministratorUserDBGuid
                    };
                    operations.Context.MeasurementCategories.Add(Adet);

                    var ParaBirimi = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Para birimi",
                        CreatorGuid = AdministratorUserDBGuid
                    };
                    operations.Context.MeasurementCategories.Add(ParaBirimi);


                    var AlanBirimi = new MeasurementCategory()
                    {
                        IsActive = true,
                        Name = "Alan",
                        CreatorGuid = AdministratorUserDBGuid
                    };
                    operations.Context.MeasurementCategories.Add(AlanBirimi);

                    operations.Context.SaveChanges();


                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "m2",
                        Name = "Metrekare",
                        MeasurementCategoryGuid = AlanBirimi.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "b",
                        Name = "Byte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "KB",
                        Name = "KiloByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "MB",
                        Name = "MegaByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "GB",
                        Name = "GigaByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        Symbol = "TB",
                        Name = "TeraByte",
                        MeasurementCategoryGuid = PCBoyut.DBGuid,
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        CreatorGuid = AdministratorUserDBGuid,
                        Symbol = "Hz",
                        Name = "Hertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        CreatorGuid = AdministratorUserDBGuid,
                        Symbol = "kHz",
                        Name = "KiloHertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        CreatorGuid = AdministratorUserDBGuid,
                        Symbol = "MHz",
                        Name = "MegaHertz",
                        MeasurementCategoryGuid = Frekans.DBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        CreatorGuid = AdministratorUserDBGuid,
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
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        MeasurementCategoryGuid = ParaBirimi.DBGuid,
                        Name = "Dolar",
                        Symbol = "$",
                        CreatorGuid = AdministratorUserDBGuid
                    });
                    operations.Context.MeasurementUnits.Add(new()
                    {
                        IsActive = true,
                        MeasurementCategoryGuid = ParaBirimi.DBGuid,
                        Name = "Euro",
                        Symbol = "€",
                        CreatorGuid = AdministratorUserDBGuid
                    });

                    operations.Context.SaveChanges();

                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.DisposeAsync();
                }
            }


            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }


        [HttpPost]
        public async Task<string> ChangeSiteName([FromBody] RequestParameter parameter)
        {
            RequestResult result = new("Change Site Name");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.SiteName = parameter.Parameters.ToString();

            await hub.Clients.Groups("SideBar").SendAsync("RefreshSiteName");

            return YGate.Json.Operations.JsonSerialize.Serialize(result);
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
        public string GetBlockedIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get Block List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetBlockedIpAddress();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string GetConnectIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get Connect Ip List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetConnectIpList();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        private List<string> GetConnectIpList() => StaticTools.IpAndDate.Select(xd => xd.Ip).Distinct().ToList();

        [HttpPost]
        public string GetWhiteIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"Get White List");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            result.Object = GetWhiteIpList();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);

        }

        [HttpPost]
        public string AddBlockList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"AddBlockList {parameter.Parameters.ToString()}");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.BlockedIp.Add(parameter.Parameters.ToString());
            result.Object = GetBlockedIpAddress();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        [HttpPost]
        public string AddWhiteIpList([FromBody] RequestParameter parameter)
        {
            RequestResult result = new($"AddWhiteIpList {parameter.Parameters.ToString()}");


            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            StaticTools.WhiteList.Add(parameter.Parameters.ToString());
            result.Object = GetWhiteIpList();
            return YGate.Json.Operations.JsonSerialize.Serialize(result);
        }

        


        private List<string> GetWhiteIpList() => StaticTools.WhiteList.ToList();

        private List<string> GetBlockedIpAddress()
        {
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
                CreatorGuid = user.DBGuid,
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
