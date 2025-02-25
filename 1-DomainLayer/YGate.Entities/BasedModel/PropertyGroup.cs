using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class PropertyGroup : IDBObject, IName
    {
        public PropertyGroup()
        {
            IsActive = true;
            DBGuid = YGate.String.Operations.GuidGen.Generate("PropertyGroup");
        }
        [Key]
        public int Id { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid {  get; set; }
        public bool IsActive {  get; set; }
        public string Name {  get; set; }
    }
}
