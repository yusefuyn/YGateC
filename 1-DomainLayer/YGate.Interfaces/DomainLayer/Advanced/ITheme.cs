using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Application.Advanced
{
    public interface ITheme
    {
        public Themes Theme { get; set; }
    }

    public enum Themes {
        Dark,
        Light
    }
}
