using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace YGate.Client.Services
{
    public class CookieService
    {
        private readonly IJSRuntime _jsRuntime;

        public CookieService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetCookie(string name, string value, int minutes)
        {
            var expiry = DateTime.UtcNow.AddMinutes(minutes);
            var cookieValue = $"{name}={value}; expires={expiry:R}; path=/";
            await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = '{cookieValue}'");
        }

        public async Task<string> GetCookie(string name)
        {
            string cookie = null;
            try
            {
                cookie = await _jsRuntime.InvokeAsync<string>("eval", $"(function() {{ " +
               $"var nameEQ = '{name}='; " +
               $"var ca = document.cookie.split(';'); " +
               $"for(var i=0; i < ca.length; i++) {{ " +
               $"  var c = ca[i]; " +
               $"  while (c.charAt(0) == ' ') c = c.substring(1,c.length); " +
               $"  if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length); " +
               $"}} " +
               $"return null; " +
               $"}})()");
            }
            catch (Exception ex)
            {

            }
            return cookie;
        }


        public async Task<string> GetMyId() {
            return await GetTokenValue("sub");
        }

        public async Task<string> GetMyUserName() { 
            return await GetTokenValue(ClaimTypes.Name);
        }

        public async Task<string> GetMyRole() {
            return await GetTokenValue(ClaimTypes.Role);
        }

        public async Task<string> GetTokenValue(string ClaimTypeName)
        {
            var handler = new JwtSecurityTokenHandler();
            string jwtToken = await GetCookie("Token");

            if (string.IsNullOrEmpty(jwtToken))
                return null;

            if (handler.CanReadToken(jwtToken))
            {
                var jwtTokenObj = handler.ReadJwtToken(jwtToken);
                var sub = jwtTokenObj.Claims.First(claim => claim.Type == ClaimTypeName)?.Value;
                return sub;
            }

            return null;
        }

        public async void DeleteCookie(string key)
        {
            await SetCookie(key, "", -1); // Geçersiz bir tarih ile çerezi sil
        }
    }
}
