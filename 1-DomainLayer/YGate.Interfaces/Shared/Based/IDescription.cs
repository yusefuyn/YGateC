using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGate.Interfaces.Shared.Based
{
    public interface IDescription
    {
        [Obsolete("Zengin metin barındırabilir.\n Yorum objesinde yorum olarak kullanılır.")]

        string? LongDescription { get; set; }
        [Obsolete("Basit düzeyde metin barındırabilir.\n Yorum objesinde bir katkısı yoktur.")]
        string? ShortDescription { get; set; }
    }
}
