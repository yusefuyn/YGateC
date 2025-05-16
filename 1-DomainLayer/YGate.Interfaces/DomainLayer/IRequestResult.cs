using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.DomainLayer
{
    public interface IRequestResult
    {
        public string? LongDescription { get; set; }
        public string? ShortDescription { get; set; }
        public EnumRequestResult Result { get; set; }
        public EnumTo To { get; set; }
        public object Object { get; set; }
    }

    public enum EnumTo
    {
        Client,
        Server
    }

    public enum EnumRequestResult
    {
        Error,
        Success,
        Stop
    }
}
