using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IDBObject
    {
        public string OwnerGuid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DBGuid { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
