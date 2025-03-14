using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.BasedModel
{
    public class PageParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ParameterType { get; set; }

        public string ToString()
        {
            return "]>{" + Name + '}';
        }
    }
}
