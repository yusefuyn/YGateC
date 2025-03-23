using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class Comment : IDBObject
    {
        public Comment()
        {
            CreateDate = DateTime.UtcNow;
            DBGuid = YGate.String.Operations.GuidGen.Generate("Comment");
        }

        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string ObjectGuid { get; set; }
        public string Value { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
    }
}
