using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class Category : IName, IDescription, IDBObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LongDescription { get; set; }
        public string? ShortDescription { get; set; }
        public string Icon { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string? Address { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category()
        {
            IsActive = true;
            DBGuid = YGate.String.Operations.GuidGen.Generate("Category");
        }
    }
}
