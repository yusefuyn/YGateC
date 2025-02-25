
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private Dictionary<string, string> UserProperties;
        HttpClientService httpClientService;
        CookieService cookieService;
        public ProfileService(CookieService cookieService, HttpClientService httpClientService)
        {
            UserProperties = new();

            this.httpClientService = httpClientService;
            this.cookieService = cookieService;
        }

        public async Task<bool> CheckMyDataAccordingToTheGivenRule(string Rule)
        {
            try
            {
                var options = ScriptOptions.Default.WithImports("System");
                var lambdaFunc = await CSharpScript.EvaluateAsync<Func<Dictionary<string, string>, bool>>(
                    $"(userProperties) => {Rule}", options);
                return lambdaFunc(UserProperties);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }


        public void AddProperties(string PropertiesName, string PropertiesValue)
        {
            UserProperties.Add(PropertiesName, PropertiesValue);
        }

        public async Task<string> GetPropertiesValue(string PropertiesName)
        {
            if (UserProperties.Count == 0)
                await GetMyProfile();

            return UserProperties.Keys.Contains(PropertiesName) ? UserProperties[PropertiesName] : "";
        }

        public async Task<RequestResult> AddPropertiesToMyProfile(string PropertiesName, string PropertiesValue)
        {
            string UserID = await this.GetPropertiesValue("Guid");
            if (string.IsNullOrEmpty(UserID))
            {
                return new("Lütfen tekrar giriş yapınız.")
                {
                    LongDescription = "Lütfen tekrar giriş yapınız.",
                    ShortDescription = "Lütfen tekrar giriş yapınız.",
                    Result = EnumRequestResult.Error
                };
            }

            var addedObj = new AccountProperties()
            {
                CreatorGuid = UserID,
                DBGuid = YGate.String.Operations.GuidGen.Generate("AProperties"),
                IsActive = true,
                PropertiesName = PropertiesName,
                PropertiesValue = PropertiesValue,
                Verified = false
            };

            RequestResult res = await httpClientService.GetPostAsync<RequestResult>(addedObj, "api/Profile/AddPropertiesToProfile");
            return res;
        }

        public async Task GetMyProfile()
        {
            string UserID = await cookieService.GetMyId(); // Cookie'den id'yi alır.
            string Username = await cookieService.GetMyUserName();
            string Role = await cookieService.GetMyRole();
            RequestResult res = await httpClientService.GetPostAsync<RequestResult>(UserID, "api/Profile/GetMyProperties");
            List<AccountProperties> myProperties = res.ConvertRequestObject<List<AccountProperties>>();
            this.UserProperties = new();
            myProperties.ForEach(xd => this.AddProperties(xd.PropertiesName, xd.PropertiesValue));
            this.AddProperties("Guid", UserID);
            this.AddProperties("Username", Username);
            this.AddProperties("Role", Role);
        }

        public void ClearMyProperties()
        {
            UserProperties = new();
        }

        public async Task<RequestResult> GetProfileByUserGuid(string UserGuid) { 
            RequestResult res = await httpClientService.GetPostAsync<RequestResult>(UserGuid, "api/Profile/GetProfileByUserGuid");
            return res;
        }

    }
}
