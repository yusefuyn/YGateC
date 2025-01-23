using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels
{
    public class EntitieRequestViewModel
    {
        public EntitieViewModel MainModel { get; set; }
        public List<EntitieViewModel> SubModel { get; set; }
    }
}
