using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.Based
{
    public class Language : IDescription
    {
        public Language()
        {
            ValuesAndDescriptions = new();
        }

        public string Description { get; set; }
        public List<LanguageValueAndDescription> ValuesAndDescriptions { get; set; }
    }
}
