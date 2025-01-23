using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class MeasurementUnit : IDBObject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }  // Örneğin "Kilo", "Adet", "MHz"
        public string Symbol { get; set; }  // Örneğin "kg", "pcs", "MHz"
        public string MeasurementCategoryGuid { get; set; } // {guid = "123123123", name = "computer measurement",description = ""}
        public string DBGuid { get; set; }
        public string OwnerGuid { get; set; }
        public bool IsActive { get; set; }

        public MeasurementUnit()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementUnit");
            IsActive = true;
        }
    }
}
