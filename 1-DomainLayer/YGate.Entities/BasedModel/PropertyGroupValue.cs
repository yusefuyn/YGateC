using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class PropertyGroupValue : IDBObject, IValue
    {
        public PropertyGroupValue()
        {
            IsActive = true;
            DBGuid = YGate.String.Operations.GuidGen.Generate("PropertyGroupValue");
        }
        [Key]
        public int Id { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// Değer
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Değerin bulunduğu grup Guid'i
        /// </summary>
        public string PropertyGroupGuid { get; set; }
    }
}
