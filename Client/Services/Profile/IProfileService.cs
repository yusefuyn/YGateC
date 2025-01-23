using System.Security.Claims;
using YGate.Entities;

namespace YGate.Client.Services.Profile
{
    public interface IProfileService
    {
        Task<RequestResult> AddPropertiesToMyProfile(string PropertiesName, string PropertiesValue);
        Task<string> GetPropertiesValue(string PropertiesName);
        void AddProperties(string PropertiesName, string PropertiesValue);
        Task GetMyProfile();
        void ClearMyProperties();
        Task<RequestResult> GetProfileByUserGuid(string UserGuid);
    }
}
