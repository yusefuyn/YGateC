using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.ViewModels;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class Entitie : IDBObject
    {

        public Entitie()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("Entitie");
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        public string CategoryDBGuid { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string? ParentEntitieDBGuid { get; set; }
        public DateTime SharedDateUTC { get; set; }
    }
}
