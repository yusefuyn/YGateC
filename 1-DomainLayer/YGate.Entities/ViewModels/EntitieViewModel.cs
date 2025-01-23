using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class EntitieViewModel : Entitie
    {

        public EntitieViewModel()
        {
            Values = new();
        }

        [NotMapped]
        public List<EntitiePropertyValue> Values { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }

        [NotMapped]
        public CategoryHtmlTemplate HtmlTemplate { get; set; }

        [NotMapped]
        public string OwnerName { get; set; }

        [NotMapped]
        public List<EntitieViewModel> ChildEntitie { get; set; }
    }
}
