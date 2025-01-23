using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class PropertyGroupViewModel : PropertyGroup
    {
        public PropertyGroupViewModel()
        {
            Values = new();
        }
        public List<PropertyGroupValue> Values { get; set; }
    }
}
