﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.Shared.Based;

namespace YGate.Entities.BasedModel
{
    /// <summary>
    /// Bu objeyi sayfalarda geçiş için kullanıyoruz.
    /// </summary>
    public class DynamicPageParameter
    {
        public string PageName { get; set; }
        public string ParameterPoolName { get; set; }
    }
}
