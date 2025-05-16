using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using YGate.Client.Services;
using YGate.Interfaces.OperationLayer;
using YGate.String.Operations;
using static System.Net.WebRequestMethods;

namespace YGate.Client
{
    public class CustomAuthStateProvider
    : AuthenticationStateProvider
    {
        private readonly HttpClient http;
        private CookieService cookieService;
        IJsonSerializer jsonSerializer;
        public CustomAuthStateProvider(CookieService cookieService, HttpClient http, IJsonSerializer jsonSerializer)
        {
            this.http = http;
            this.cookieService = cookieService;
            this.jsonSerializer = jsonSerializer;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await cookieService.GetCookie("Token");
            var identity = new ClaimsIdentity();
            http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }
                catch
                {
                    identity = new();
                }
            }
            var state = new AuthenticationState(new ClaimsPrincipal(identity));

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);



            foreach (var kvp in keyValuePairs)
            {
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
            }


            // Rol claims'si ["",""] şeklinde geliyor burada rol claimsini bulup silip değerleri parçalayım yeniden her birini bir claims olarak eliyoruz.
            var roleClaim = claims.SingleOrDefault(xd=> xd.Type == ClaimTypes.Role ||  xd.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            claims.Remove(roleClaim);
            List<string> Roles = jsonSerializer.Deserialize<List<string>>(roleClaim.Value.ToString());
            claims.AddRange(Roles.Select(xd => new Claim(ClaimTypes.Role,xd.ToString())));
            // Bu olmaz ise birden fazla rol sahibi olanlarda authorizeview'lar çalışmıyor. Şu anda bulduğum çözümüm bir tek bu.
            // Bu işe loginController'de ayrı claims olarak yaptım ama jwt onu birleştiriyor.
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }


    //public class CustomAuthStateProvider : AuthenticationStateProvider
    //{
    //    private readonly HttpClient http;
    //    private CookieService cookieService;
    //    private readonly JwtTokenService _jwtTokenService;
    //    public CustomAuthStateProvider(CookieService cookieService, HttpClient http)
    //    {
    //        this.http = http;
    //        this.cookieService = cookieService;
    //        _jwtTokenService = new();
    //    }

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        var token = await cookieService.GetCookie("Token");

    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    //        }

    //        var roles = _jwtTokenService.GetRolesFromToken(token);
    //        string name = _jwtTokenService.GetNameFromToken(token);
    //        string identifier = _jwtTokenService.GetIdentifierFromToken(token);
    //        string jwti = _jwtTokenService.GetJtiFromToken(token);
    //        string sub = _jwtTokenService.GetSubFromToken(token);


    //        var claims = new List<Claim>();

    //        if (!string.IsNullOrEmpty(identifier))
    //            claims.Add(new Claim(ClaimTypes.NameIdentifier, identifier));

    //        if (!string.IsNullOrEmpty(name))
    //            claims.Add(new Claim(ClaimTypes.Name, name));

    //        if (roles.Count > 0)
    //            claims.Add(new Claim(ClaimTypes.Role, baseFacades.JsonSerializer.Serialize(roles)));

    //        if (!string.IsNullOrEmpty(jwti))
    //            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jwti));

    //        if (!string.IsNullOrEmpty(sub))
    //            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, sub));

    //        var identity = new ClaimsIdentity(claims, "jwt");
    //        var user = new ClaimsPrincipal(identity);

    //        return new AuthenticationState(user);
    //    }
    //}
    //public class JwtTokenService
    //{
    //    JWT Token'dan Rollerı Çekmek
    //    public List<string> GetRolesFromToken(string token)
    //    {
    //        var roles = new List<string>();

    //        try
    //        {
    //            var handler = new JwtSecurityTokenHandler();
    //            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

    //            if (jwtToken != null)
    //            {
    //                "role" claim'lerinden roller alınır
    //                roles = jwtToken?.Claims
    //                    .Where(c => c.Type == "role" || c.Type == ClaimTypes.Role)
    //                    .Select(c => c.Value)
    //                    .ToList();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Hata loglama
    //            Console.WriteLine("Token parsing error: " + ex.Message);
    //        }

    //        return roles;
    //    }

    //    public string GetNameFromToken(string token)
    //    {
    //        string name = null;

    //        try
    //        {
    //            var handler = new JwtSecurityTokenHandler();
    //            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

    //            if (jwtToken != null)
    //            {
    //                "role" claim'lerinden roller alınır
    //                var obj = jwtToken?.Claims
    //                    .FirstOrDefault(c => c.Type == "name" || c.Type == ClaimTypes.Name);
    //                name = obj.Value.ToString();

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Hata loglama
    //            Console.WriteLine("Token parsing error: " + ex.Message);
    //        }

    //        return name;
    //    }

    //    public string GetSubFromToken(string token)
    //    {
    //        var handler = new JwtSecurityTokenHandler();
    //        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
    //        return jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
    //    }

    //    public string GetIdentifierFromToken(string token)
    //    {
    //        string identity = null;

    //        try
    //        {
    //            var handler = new JwtSecurityTokenHandler();
    //            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

    //            if (jwtToken != null)
    //            {
    //                var obj = jwtToken?.Claims
    //                    .FirstOrDefault(c => c.Type == "name" || c.Type == ClaimTypes.NameIdentifier);
    //                identity = obj.Value.ToString();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Hata loglama
    //            Console.WriteLine("Token parsing error: " + ex.Message);
    //        }

    //        return identity;
    //    }
    //    public string GetJtiFromToken(string token)
    //    {
    //        var handler = new JwtSecurityTokenHandler();
    //        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
    //        return jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
    //    }
    //}
}
