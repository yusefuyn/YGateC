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
using YGate.Interfaces.OperationLayer;

namespace YGate.Json.Operations
{
    public class TokenService : ITokenService
    {
        public string SecretKey;
        public int ValidityTime;
        IJsonSerializer serializer;
        public TokenService(IJsonSerializer serializer, int ValidityTime = 1, string SecretKey = "2524134145_yusuf_sado_m4m!")
        {
            this.serializer = serializer;
            this.ValidityTime = ValidityTime;
            this.SecretKey = SecretKey;
        }


        public string GenerateJwtToken(string userId, string name, List<string> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> myClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,userId),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, serializer.Serialize(roles)),
            };

            var tokenDescriptor = new JwtSecurityToken(claims: myClaims, expires: DateTime.Now.AddMinutes(ValidityTime), signingCredentials: credentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }

        public string GetUserIDFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
                return "";

            string userToken = string.Join(",", jsonToken?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString());

            return userToken;
        }

        public string GetUserRolesFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
                return "";
            string roles = string.Join(",", jsonToken?.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(xd => xd.Value.ToString().Replace("[", "").Replace("]", "").Replace("\"", "")) ?? Enumerable.Empty<string>());
            return roles;
        }

        public bool ValidateJwtToken(ref string token)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length).Trim();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token doğrulama hatası: {ex.Message}");
                return false;
            }
        }

    }

}
