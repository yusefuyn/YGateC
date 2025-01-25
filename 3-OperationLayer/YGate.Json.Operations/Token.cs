using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Json.Operations
{
    public class Token
    {
        public string __secretkey = "2524134145_yusuf_sado_m4m!";
        public int ValidityTime = 1;

        public string GenerateJwtToken(string userId, string name, List<string> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(__secretkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> myClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,userId),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, YGate.Json.Operations.JsonSerialize.Serialize(roles)),
            };

            var tokenDescriptor = new JwtSecurityToken(claims: myClaims, expires: DateTime.Now.AddMinutes(ValidityTime), signingCredentials: credentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }

        public string GetUserRolesFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                string roles = string.Join(",", jsonToken?.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(xd => xd.Value.ToString().Replace("[","").Replace("]","").Replace("\"", "")) ?? Enumerable.Empty<string>());


                return roles;
            }
            return "";
        }

        public bool ValidateJwtToken(ref string token)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(__secretkey)); // Aynı gizli anahtar
            var tokenHandler = new JwtSecurityTokenHandler();

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim(); // "Bearer " kelimesinden sonrasını al
            }

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false, // İsterseniz doğrulayabilirsiniz
                    ValidateAudience = false, // İsterseniz doğrulayabilirsiniz
                    ClockSkew = TimeSpan.Zero // Geçerlilik süresi toleransı
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                // Token geçersiz
                Console.WriteLine($"Token doğrulama hatası: {ex.Message}");
                return false;
            }
        }

    }

}
