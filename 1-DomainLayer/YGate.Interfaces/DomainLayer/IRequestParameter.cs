using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.DomainLayer
{
    public interface IRequestParameter
    {
        public string Address { get; set; }
        public int Supply { get; set; }
        public object Parameters { get; set; }
        public string? Token { get; set; }
        public DateTime DateTimeUTC { get; set; }
        public string ParameterHash { get; set; }

    }
}
