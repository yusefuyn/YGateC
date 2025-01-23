using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class CategoryViewModel : Category
    {
        public CategoryViewModel() : base()
        {

        }
        public List<CategoryViewModel> ChildCategories { get; set; }
        public List<CategoryTemplateViewModel> Template { get; set; }
        public bool IsExpanded { get; set; } = false;
    }
}
