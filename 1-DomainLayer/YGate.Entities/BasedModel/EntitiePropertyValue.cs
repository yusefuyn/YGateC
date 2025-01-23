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
    public class EntitiePropertyValue : IDBObject
    {
        public EntitiePropertyValue()
        {
            IsActive = true;
            DBGuid = YGate.String.Operations.GuidGen.Generate("EntitieProperty");
        }

        [Key]
        public int Id { get; set; }
        public string OwnerGuid {get; set;}
        public string DBGuid {get; set;}
        public bool IsActive {get; set;}
        /// <summary>
        /// CategoryTemplate objesinin name'i gelecek
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Seçilen yada yazılan değer gelecek
        /// </summary>
        public string PropertyValue { get; set; }
        /// <summary>
        /// ItemGroup'ın dbguid'i gelecek
        /// </summary>
        public string? PropertyDBGuid { get; set; }
        /// <summary>
        /// Kategorinin Template Guid'i gelecek
        /// </summary>
        public string CategoryTemplateGuid { get; set; } 
        /// <summary>
        /// Varlığın guid'i gelecek. SubEntitie yada Entitie
        /// </summary>
        public string EntitieDbGuid { get; set; }

        [NotMapped]
        public PropertyValueType Type { get; set; }

        [NotMapped]
        public bool Seo { get; set; } = false;
    }
}
