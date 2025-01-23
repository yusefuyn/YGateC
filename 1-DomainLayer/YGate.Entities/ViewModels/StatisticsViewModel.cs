using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels
{
    public class StatisticsViewModel
    {
        public int UserCount { get; set; }
        public int IdentifiedEntityCount { get; set; }
        public int EntityPropertyCount { get; set; }
        public int CreatedEntityCount { get; set; }
    }
}
