using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Json.Operations
{
    public static class JsonSerialize
    {
        public static string Serialize(object obj)
        {
            var returned = JsonConvert.SerializeObject(obj);
            return returned;
        }
    }
}
