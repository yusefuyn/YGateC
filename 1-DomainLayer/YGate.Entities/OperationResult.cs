using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Advanced;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities
{
    public class OperationResult<T> : IOperationResult<T>
    {
        public OperationResult(string Description)
        {
            this.Result = EnumOperationResult.Error;
            this.LongDescription = Description;
        }
        public string? LongDescription { get; set; }
        public string? ShortDescription { get; set; }
        public EnumOperationResult Result { get; set; }
        public T Obj { get; set; }
    }

    public enum EnumOperationResult { 
        Error,
        Success,
        Stop
    }
}
