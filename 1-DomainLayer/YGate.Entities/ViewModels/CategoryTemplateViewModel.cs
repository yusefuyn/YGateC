using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public bool CheckData()
        {
            if (this.categoryTemplateValues == null && this.categoryTemplateValues.Count == 0)
                return false;

            string value = this.categoryTemplateValues[0]?.Value;

            if (string.IsNullOrEmpty(value))
                return false;

            switch (this.ValueType)
            {
                case PropertyValueType.String:
                    return true;
                case PropertyValueType.Integer:
                    if (int.TryParse(value, out int parsedValue))
                        return true;
                    else
                        return false;
                case PropertyValueType.Boolean:
                    // TODO SelectableYesNo True,False,1,0,"Yes","No",'Y','N' gibi değerler kabul görülebilir yapılacak
                    return true;
                case PropertyValueType.CustomValidationRegex:
                    return Regex.IsMatch(value, this.ValidateRegex);
                case PropertyValueType.ItemGroup:
                    // TODO ValueGroup Values'den kontrol edilecek.
                    return true;
                case PropertyValueType.Unit:
                    dynamic dynamicobj = YGate.Json.Operations.JsonDeserialize<dynamic>.Deserialize(value);
                    string val1 = dynamicobj?.IntegerVal;
                    string val2 = dynamicobj?.UnitGuid;
                    return !string.IsNullOrEmpty(val1) && !string.IsNullOrEmpty(val2);
                case PropertyValueType.Combos:
                    List<string> list = YGate.Json.Operations.JsonDeserialize<List<string>>.Deserialize(value);
                    if (list != null || list.Count() > 0)
                        return true;
                    else
                        return false;
                case PropertyValueType.RichText:
                    return true;
                default:
                    return false;
            }
        }
    }
}
