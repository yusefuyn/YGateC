using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Entities;
using YGate.Interfaces.DomainLayer;
using YGate.Interfaces.OperationLayer;
using YGate.Interfaces.OperationLayer.Repositories;
using YGate.Server.Facades;

namespace YGate.BusinessLayer.EFCore.Concretes
{
    public class LoginRegisterRepository : ILoginRegisterRepository
    {
        Operations operations;
        IMailService mailServices;
        IBaseFacades facades;
        ITokenService tokenService;

        public LoginRegisterRepository(Operations operations,
            IMailService mailServices,
            IBaseFacades facades,
            ITokenService tokenService)
        {
            this.operations = operations;
            this.mailServices = mailServices;
            this.facades = facades;
            this.tokenService = tokenService;
        }

        public async Task<IRequestResult> Login(IRequestParameter parameter)
        {
            RequestResult returnedResult = new("Login V2");
            LoginViewModel model = facades.JsonSerializer.Deserialize<LoginViewModel>(parameter.Parameters.ToString());

            var result = operations.GetAccount(model.UserName, model.Password);
            var account = result.Obj;

            if (account == null)
            {
                returnedResult.Result = EnumRequestResult.Stop;
                returnedResult.ShortDescription = "There is no such user";
            }
            else if (account.Status == AccountStatus.NotVerified)
            {
                returnedResult.Result = EnumRequestResult.Stop;
                returnedResult.ShortDescription = "You are in the approval phase.";
            }
            else if (account.Status == AccountStatus.Banned)
            {
                returnedResult.Result = EnumRequestResult.Stop;
                returnedResult.ShortDescription = "Banned !_!";
            }
            else
            {
                LoginReplyViewModel reply = new()
                {
                    UserName = account.Username,
                    Token = tokenService.GenerateJwtToken(account.DBGuid, account.Username, account.Roles.Select(xd => xd.Name).ToList()),
                    UserID = account.DBGuid
                };
                returnedResult.Result = EnumRequestResult.Success;
                returnedResult.Object = reply;
                returnedResult.ShortDescription = "Successfuly";
                mailServices.Send(account.Email, "Your account has been logged in.", "Your account has been logged in.\r\n\r\nIf you have not done this, please change your password.\r\n\r\nContact a representative.\r\n\r\nYGate");
            }

            return returnedResult;
        }

        public async Task<IRequestResult> Register(IRequestParameter parameter)
        {

            var model = facades.JsonSerializer.Deserialize<RegisterViewModel>(parameter.Parameters.ToString());

            RequestResult returnedResult = new("Register V1");
            Account account = operations.Context.Accounts.FirstOrDefault(xd => xd.Username == model.UserName || xd.Email == model.Email);
            if (account != null)
            {
                returnedResult.Result = EnumRequestResult.Stop;
                returnedResult.ShortDescription = "You must have a unique username or email.";
                returnedResult.LongDescription = "You must have a unique username or email.";
                return returnedResult;
            }

            string userGuid = YGate.String.Operations.GuidGen.Generate("Account");
            var accountModel = new Account()
            {
                DBGuid = userGuid,
                Email = model.Email,
                IsActive = true,
                CreatorGuid = "Admin",
                Password = model.Password,
                Username = model.UserName,
                Status = AccountStatus.NotVerified
            };

            operations.Context.Accounts.Add(accountModel);

            var userRole = operations.Context.Roles.FirstOrDefault(xd => xd.Name == "User");

            AccountRole role = new AccountRole()
            {
                DBGuid = YGate.String.Operations.GuidGen.Generate("AccountRole"),
                FromGuid = userGuid,
                ToGuid = "Admin",
                IsActive = true,
                IssueDate = DateTime.Now,
                RoleGuid = userRole.DBGuid,
                CreatorGuid = "Admin",
            };

            operations.Context.AccountRoles.Add(role);

            var PasswordModel = new AccountPasswords()
            {
                CreateDate = DateTime.Now,
                CreatorGuid = accountModel.DBGuid,
                IsActive = true,
                Password = model.Password,
            };

            operations.Context.AccountsPasswords.Add(PasswordModel);

            operations.Context.SaveChanges();


            mailServices.Send(model.Email, "Successfully registered.", "Successfully registered.\r\n\r\nWelcome to YussefuynsteinChannel platform.\r\n\r\nYussefuynsteinChannel\r\n\r\nwww.yussefuynstein.com");

            returnedResult.Result = EnumRequestResult.Success;
            returnedResult.ShortDescription = "Successfully registered.";
            returnedResult.LongDescription = "Successfully registered.";
            return returnedResult;
        }
    }
}
