using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ResultModel
{
    public class CategoryRolesResultModel
    {
        public List<Role> AddedRoles { get; set; }
        public List<Role> AddeableRoles { get; set; }
        public Category Category { get; set; }
    }
}
