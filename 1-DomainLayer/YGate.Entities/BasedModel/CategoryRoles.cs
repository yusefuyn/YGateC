using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class CategoryRoles : IDBObject
    {
        [Key]
        public int Id { get; set; }
        public string OwnerGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string CategoryGuid { get; set; }
        public string RoleGuid { get; set; }
    }
}
