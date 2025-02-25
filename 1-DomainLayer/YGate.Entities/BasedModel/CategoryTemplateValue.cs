using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class CategoryTemplateValue : IDBObject
    {
        [Key]
        public int Id { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string CategoryTemplateGuid { get; set; }
        public string? ValueGroupGuid { get; set; }
        public string? Value { get; set; }
 
        public CategoryTemplateValue()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("CategoryTemplateValue");
            IsActive = true;
            Value = "";
        }
    }
}
