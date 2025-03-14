using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Entities.BasedModel
{

    /// <summary>
    /// Dinamik sayfalara parametre göndermek için.
    /// </summary>
    public class DynamicPageDynamicParameter
    {
        public DynamicPageDynamicParameter(string pageName)
        {
            PageName = pageName;
            Parameters = new();
        }
        public string PageName { get; set; }
        public List<PageParameter> Parameters { get; set; }
    }
}
