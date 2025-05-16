using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using YGate.BusinessLayer.EFCore;
using YGate.Entities;
using YGate.Entities.BasedModel;
using YGate.Entities.ViewModels;
using YGate.Interfaces.DomainLayer;
using YGate.Json.Operations;
using YGate.Mail.Operations;

namespace YGate.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginRegisterController
    {
        Operations operations;
        MailServices mailServices;
        public LoginRegisterController(Operations operations, MailServices mailServices)
        {
            this.operations = operations;
            this.mailServices = mailServices;
        }


        [HttpPost]
        public async Task<RequestResult> Login([FromBody] RequestParameter parameter)
        {
            RequestResult returnedResult = new("Login V2");
            LoginViewModel model = parameter.ConvertParameters<LoginViewModel>();

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
                    Token = StaticTools.tokenService.GenerateJwtToken(account.DBGuid, account.Username, account.Roles.Select(xd => xd.Name).ToList()),
                    UserID = account.DBGuid
                };
                returnedResult.Result = EnumRequestResult.Success;
                returnedResult.Object = reply;
                returnedResult.ShortDescription = "Successfuly";
                mailServices.Send(account.Email, "Your account has been logged in.", "Your account has been logged in.\r\n\r\nIf you have not done this, please change your password.\r\n\r\nContact a representative.\r\n\r\nYGate");
            }

            return returnedResult;
        }

        [HttpPost]
        public RequestResult Register([FromBody] RequestParameter parameter)
        {

            var model = parameter.ConvertParameters<RegisterViewModel>();

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
