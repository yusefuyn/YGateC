using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.String.Operations
{
    public static class GuidGen
    {
        public static string Generate(string salt = "")
        {
            return salt + $"-{DateTime.Now.ToString("dd-MM-yyyy")}-" + Guid.NewGuid().ToString();
        }
    }
}
