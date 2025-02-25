using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class AccountPasswords : IPassword, IDBObject, IIndexable, ICreateDate
    {
        public AccountPasswords()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("AccountPassword");
            IsActive = true;
            CreateDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
