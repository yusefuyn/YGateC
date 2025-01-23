using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using YGate.Client.Services.Profile;
using YGate.Entities;
using YGate.Entities.ViewModels;
using YGate.Json.Operations;

namespace YGate.Client.Services.Login
{
    public class LoginAndRegisterService : ILoginAndRegisterService
    {

        HttpClientService httpClientService;

        public LoginAndRegisterService(HttpClientService httpClientService, IProfileService profile)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<RequestResult> LoginAsync(LoginViewModel loginViewModel)
        {
            var obj = await httpClientService.GetPostAsync<RequestResult>(loginViewModel, "api/LoginRegister/Login");
            if (obj.Result == EnumRequestResult.Success)
            {
                httpClientService.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", obj.ConvertRequestObject<LoginReplyViewModel>().Token);
            }
            return obj;
        }

        public async Task<RequestResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (string.IsNullOrEmpty(registerViewModel.UserName)
                || string.IsNullOrEmpty(registerViewModel.UserName)
                || string.IsNullOrEmpty(registerViewModel.UserName)
                || string.IsNullOrEmpty(registerViewModel.UserName))
                return new("Please fill in all fields") { LongDescription = "Please fill in all fields", Result = EnumRequestResult.Stop, ShortDescription = "Please fill in all fields" };
            if (registerViewModel.Password != registerViewModel.RPassword)
                return new("Password and password repeat do not match.") { LongDescription = "Password and password repeat do not match.", Result = EnumRequestResult.Stop, ShortDescription = "Password and password repeat do not match." };

            RequestResult obj = await httpClientService.GetPostAsync<RequestResult>(registerViewModel, "api/LoginRegister/Register");

            return obj;
        }
    }
}
