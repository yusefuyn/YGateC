using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.OperationLayer
{
    public interface IJsonSerializer
    {
        public string Serialize(object obj);
        public T Deserialize<T>(string json);
    }
}
