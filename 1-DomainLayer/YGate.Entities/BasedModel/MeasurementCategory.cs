using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{

    // mhz, gb,mb,wat desi,adet,inç gibi birimleri kategorilemek ve bir arada tutmak amacıyla oluşturulmuştur.
    // Dünyavi hayatta kg,mg'nin ağırlık için kullanılan ölcü birimleri olduğunu bize öğrettiler.
    // bizde günümüz şartlarına uygun şekilde gruplandırabilmek için bu sınıfı kullanacağız.


    public class MeasurementCategory : IDBObject, IName, IDescription
    {
        public MeasurementCategory()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("MeasurementCategory");
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }
        public string CreatorGuid  { get; set; }
        public string DBGuid  { get; set; }
        public bool IsActive  { get; set; }
        public string Name { get; set; }
        public string? LongDescription  { get; set; }
        public string? ShortDescription  { get; set; }
    }
}
