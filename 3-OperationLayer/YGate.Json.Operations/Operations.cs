using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YGate.Interfaces.OperationLayer;
namespace YGate.Json
{
    public class JsonOperations : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            var returned = JsonConvert.SerializeObject(obj);
            return returned;
        }
        public T Deserialize<T>(string json)
        {
            var returned = JsonConvert.DeserializeObject<T>(json);
            return returned;
        }
    }
}
