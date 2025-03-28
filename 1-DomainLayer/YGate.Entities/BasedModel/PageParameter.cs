using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class PageParameter : IDBObject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ParameterType { get; set; }
        public string? CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string PageName { get; set; }
        public DateTime CreateDate { get; set; }

        public PageParameter()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("PageParameters");
            CreateDate = DateTime.UtcNow;
            IsActive = true;
        }
        public string ToString()
        {
            return "]>{" + Name + '}';
        }
    }
}
