using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Mapping;
using YGate.Server.Facades;
using YGate.Interfaces.OperationLayer.Repositories;

namespace YGate.BusinessLayer.EFCore.Concretes.Repositories
{
    public class AdministratorRepository : IAdministratorRepository
    {
        Operations operations { get; set; }
        IBaseFacades baseFacades { get; set; }

        public AdministratorRepository(Operations operations, IBaseFacades baseFacades)
        {
            this.operations = operations;
        }

        public async Task<IRequestResult> Setup(IRequestParameter test)
        {
            RequestResult result = new("Setup başarıyla yapıldı.");
            result.To = EnumTo.Server;
            result.Result = EnumRequestResult.Success;

            if (operations.Context.Accounts.Count() != 0)
            {
                result.Result = EnumRequestResult.Stop;
                result.LongDescription = "Setup zaten yapılmış.";
                return result;
            }

            string PassHash = String.Operations.Hash.SaltAndSHA512("219619yusuf_");
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
                        DBGuid = String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Standart Role",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = "Standart Role"
                    };
                    operations.Context.Roles.Add(UserRole);

                    var AllRole = new Role()
                    {
                        Name = "All",
                        DBGuid = String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "Misafirlerde dahil herkez",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = "Misafirlerde dahil herkez"
                    };
                    operations.Context.Roles.Add(AllRole);

                    var MarketUserRole = new Role()
                    {
                        Name = "MarketUser",
                        DBGuid = String.Operations.GuidGen.Generate("Role"),
                        IsActive = true,
                        LongDescription = "",
                        CreatorGuid = AdministratorUserDBGuid,
                        ShortDescription = ""
                    };
                    operations.Context.Roles.Add(MarketUserRole);

                    var MarketModRole = new Role()
                    {
                        Name = "MarketModRole",
                        DBGuid = String.Operations.GuidGen.Generate("Role"),
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
                        Status = AccountStatus.Verified,
                        Username = "Yussefuynstein"
                    };
                    operations.Context.Accounts.Add(AdminAccount);

                    var AdministratorAccountRole = new AccountRole()
                    {
                        DBGuid = String.Operations.GuidGen.Generate("AccountRole"),
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

                    DynamicPage dynamicPage = new()
                    {
                        CreatorGuid = AdministratorUserDBGuid,
                        DBGuid = String.Operations.GuidGen.Generate("DynamicPage"),
                        IsActive = true,
                        Name = "MainPage",
                        Index = "<div class='container'><h3>YGate otomatik ana sayfası. Panelden düzenleyebilirsiniz !</h3></div>"
                    };
                    operations.Context.DynamicPages.Add(dynamicPage);
                    operations.Context.SaveChanges();


                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.DisposeAsync();
                }
            }
            return result;
        }

        public async Task<IRequestResult> ChangeSiteName(IRequestParameter parameter)
        {
            RequestResult result = new("Change Site Name");
            result.Result = EnumRequestResult.Success;
            result.To = EnumTo.Server;
            return result;
        }

        public async Task<IRequestResult> GetAllUser(IRequestParameter pars)
        {
            RequestResult result = new("GetAllUser AdministratorModule");
            result.Result = EnumRequestResult.Success;
            var Accounts = operations.Context.Accounts.ToList();
            // TODO : Daha sonra rollerini çek listeleyelim.
            result.Object = Accounts;
            return result;
        }

        public async Task<IRequestResult> GetUser(IRequestParameter parameter)
        {
            RequestResult result = new($"GetUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            result.Object = operations.GetAdministratorUserItem(parameter.Parameters.ToString());
            return result;
        }

        public async Task<IRequestResult> VerifyUser(IRequestParameter parameter)
        {
            RequestResult result = new($"VerifyUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            var user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.Status = AccountStatus.Verified;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return result;
        }

        public async Task<IRequestResult> BanUser(IRequestParameter parameter)
        {
            RequestResult result = new($"BanUser {parameter.Parameters.ToString()} AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.Status = AccountStatus.Banned;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return result;
        }

        public async Task<IRequestResult> ChangeRole(IRequestParameter parameter)
        {
            dynamic parameters = baseFacades.JsonSerializer.Deserialize<dynamic>(parameter.Parameters.ToString());

            string guid = parameters["UserId"];
            string role = parameters["Rol"];
            string ToGuid = parameters["ToGuid"];

            RequestResult result = new($"ChangeRole User : {guid}  Role : {role} ToGuid : {ToGuid}");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == guid);
            Role roleo = operations.Context.Roles.SingleOrDefault(xd => xd.DBGuid == role);

            AccountRole accountRole = new()
            {
                DBGuid = String.Operations.GuidGen.Generate("AccountRole"),
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
            return result;
        }

        public async Task<IRequestResult> UserIsActiveFalse(IRequestParameter parameter)
        {
            RequestResult result = new($"IsActive User : {parameter.Parameters.ToString()}  Active : false AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.IsActive = false;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return result;
        }


        public async Task<IRequestResult> UserIsActiveTrue(IRequestParameter parameter)
        {
            RequestResult result = new($"IsActive User : {parameter.Parameters.ToString()}  Active : true AdministratorModule");
            result.Result = EnumRequestResult.Success;

            Account user = operations.Context.Accounts.FirstOrDefault(xd => xd.DBGuid == parameter.Parameters.ToString());
            user.IsActive = true;
            operations.Context.Accounts.Update(user);
            operations.Context.SaveChanges();

            result.Object = user;
            return result;
        }

        public async Task<IRequestResult> DeleteRoleAccountToObjctGuid(IRequestParameter parameter)
        {
            var dynobj = baseFacades.JsonSerializer.Deserialize<dynamic>(parameter.Parameters.ToString());
            string AccountGuid = dynobj.AccountGuid;
            string RoleGuid = dynobj.RoleGuid;

            RequestResult result = new($"DeleteRoleAccountToObjectGuid : {AccountGuid}  {RoleGuid}");
            result.Result = EnumRequestResult.Success;

            var aroles = operations.Context.AccountRoles.SingleOrDefault(xd => xd.FromGuid == AccountGuid && xd.RoleGuid == RoleGuid);

            operations.Context.AccountRoles.Remove(aroles);
            operations.Context.SaveChanges();

            result.Object = operations.GetAccount(AccountGuid);
            return result;
        }
    }
}
