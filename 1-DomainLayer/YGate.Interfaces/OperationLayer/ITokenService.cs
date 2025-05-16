using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.OperationLayer
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userId, string name, List<string> roles);
        string GetUserIDFromToken(string token);
        string GetUserRolesFromToken(string token);
        bool ValidateJwtToken(ref string token);
    }
}
