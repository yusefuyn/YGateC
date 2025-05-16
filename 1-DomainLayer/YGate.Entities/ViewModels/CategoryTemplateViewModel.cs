using System.ComponentModel.DataAnnotations.Schema;
using YGate.Entities.BasedModel;

namespace YGate.Entities.ViewModels
{
    public class CategoryTemplateViewModel : CategoryTemplate
    {

        public CategoryTemplateViewModel()
        {
            categoryTemplateValues = new();
        }
        public List<CategoryTemplateValue> categoryTemplateValues { get; set; }
        [NotMapped]
        public object Values { get; set; }
        public bool Added { get; set; } = true;

        public void NewValueAdd() => categoryTemplateValues.Add(new()); 
        public void RemoveValue(CategoryTemplateValue temp) => categoryTemplateValues.Remove(temp); 
        public bool CheckData()
        {
            return true;
        }
    }
}
