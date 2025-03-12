using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.BasedModel
{

    public class EntitieOwnerTransfer
    {
        [Key]
        public int Id { get; set; }
        public string EntitieGuid { get; set; }
        public string OldOwnerGuid { get; set; }
        public string NewOwnerGuid { get; set; }
        public DateTime DateTimeUTC { get; set; }
        public string Hash { get; set; }
    }
}
