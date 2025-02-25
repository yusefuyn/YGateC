using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class AccountRole : IDBObject,IIndexable
    {
        [Key]
        public int Id { get; set; }
        public string CreatorGuid{ get; set; }
        public string DBGuid{ get; set; }
        [DefaultValue(true)]
        public bool IsActive{ get; set; }
        public string FromGuid { get; set; }//Kimden
        public string ToGuid { get; set; }//Kime
        public string RoleGuid { get; set; }//Role guidi    
        public DateTime IssueDate { get; set; }//Veriliş tarihi
        public DateTime? ValidityDate { get; set; }// Geçerlilik tarihi
    }
}
