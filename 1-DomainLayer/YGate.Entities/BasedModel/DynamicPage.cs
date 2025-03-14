using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    /// <summary>
    /// Dinmaik sayfayı temsil eder.
    /// </summary>
    public class DynamicPage : IName, IDBObject, IIndexable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        public string CreatorGuid { get ; set ; }
        public string DBGuid { get ; set ; }
        public bool IsActive { get ; set ; }
    }
}
