using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    public class CategoryTemplate : IDBObject
    {
        public CategoryTemplate()
        {
            DBGuid = YGate.String.Operations.GuidGen.Generate("CategoryTemplate");
            IsActive = true;
            Require = false;
        }
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public PropertyValueType ValueType { get; set; }
        public string? ValidateRegex { get; set; }
        public string CreatorGuid { get; set; }
        public string DBGuid { get; set; }
        public bool IsActive { get; set; }
        public bool Seo { get; set; } = false;
        public bool ValueValidation(object value)
        {
            switch (ValueType)
            {
                case PropertyValueType.String:
                    return value is string;
                case PropertyValueType.Integer:
                    return value is int || value is long || (value is string str && long.TryParse(str, out _));
                case PropertyValueType.Boolean:
                    if (value is bool)
                        return true;

                    if (value is string strValue)
                        return strValue.Equals("Yes", StringComparison.OrdinalIgnoreCase) || strValue.Equals("No", StringComparison.OrdinalIgnoreCase);

                    if (value is int intValue)
                        return intValue == 0 || intValue == 1;

                    return false;
                case PropertyValueType.CustomValidationRegex:
                    return Regex.IsMatch(value.ToString(), ValidateRegex);
                case PropertyValueType.ItemGroup:
                    return false; // Daha sonra halledilecek
                case PropertyValueType.ValueList: return false; // Daha sonra halledilecek.
                default:
                    return false;
            }
        }
        public bool Require { get; set; }
    }


    public enum PropertyValueType
    {
        String = 1,
        Integer = 2,
        Boolean = 4,
        CustomValidationRegex = 8,
        ItemGroup = 16,
        Unit = 32,
        Combos = 64,
        RichText = 128,
        ValueList = 256
    }
}
