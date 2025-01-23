using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Entities.BasedModel;

namespace YGate.ViewModel
{
    public class CategoryTemplateViewModel : CategoryTemplate
    {
        public List<CategoryTemplateValue> categoryTemplateValues { get; set; }
        public bool Added { get; set; } = true;
        public bool CheckData() {
            switch (this.ValueType)
            {
                case PropertyValueType.String:

                    break;
                case PropertyValueType.Integer:
                    break;
                case PropertyValueType.SelectableYesNo:
                    break;
                case PropertyValueType.CustomValidationRegex:
                    break;
                case PropertyValueType.ValueGroup:
                    break;
                case PropertyValueType.Unit:
                    dynamic dynamicobj = YGate.Json.Operations.JsonDeserialize<dynamic>.Deserialize(this.categoryTemplateValues[0].Value);
                    string val1 = dynamicobj["IntegerVal"];
                    string val2 = dynamicobj["UnitGuid"];
                    if (!string.IsNullOrEmpty(val1) && !string.IsNullOrEmpty(val2))
                        break;
                case PropertyValueType.Combos:
                    break;
                default:
                    break;
            }
        }
    }
}
