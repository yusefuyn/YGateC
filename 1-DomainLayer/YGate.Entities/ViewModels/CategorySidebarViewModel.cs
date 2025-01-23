using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.ViewModels
{
    public class CategorySidebarViewModel
    {
        public CategorySidebarViewModel()
        {
            CategoryRole = new() { };
        }
        public string CategoryGuid { get; set; }
        public string CategoryViewLink { get; set; }
        public string CategoryName { get; set; }
        public string CategorySymbol { get; set; } = "fa fa-map";
        public List<CategorySidebarViewModel> SubCategories { get; set; }
        public List<string> CategoryRole { get; set; }
    }
}
