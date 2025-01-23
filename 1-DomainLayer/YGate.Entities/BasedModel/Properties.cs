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
    public class Properties :  IDBObject
    {
        public Properties()
        {
            Verified = false;
            DBGuid = YGate.String.Operations.GuidGen.Generate("Properties");
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }
        public string OwnerGuid { get; set; }
        public string DBGuid { get; set; }
        public string PropertiesName { get; set; }
        public string PropertiesValue { get; set; }
        public bool Verified { get; set; }
        public bool IsActive { get; set; }
    }
}
