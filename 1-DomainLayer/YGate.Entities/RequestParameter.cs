using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Entities
{
    public class RequestParameter : IRequestParameter
    {
        public RequestParameter()
        {
            DateTimeUTC = DateTime.Now;
            Supply = 1;
            ParameterHash = "";
        }

        /// <summary>
        /// /api/{Controller}/{Action}/{Parameter}
        /// adres buraya gelecek.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Kaçla kaç arası istenen varlığı gösterir. Supply 1'ise 1x20-20 ile 1x20 arası varlılar gelir.
        /// </summary>
        public int Supply { get; set; }
        /// <summary>
        /// Aksiyonun istediği Parametreler.
        /// </summary>
        public object Parameters { get; set; }
        /// <summary>
        /// Geçerlilik tokeni.
        /// Sunucu istenilecek tokeni bir önceki istek ile göndermiş olabilir veya bağlantı başka bir yerden istenebilir.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// İsteğin yapıldığı saat.
        /// </summary>
        public DateTime DateTimeUTC { get; set; }
        /// <summary>
        /// ParameterHash boşken varlığın json halinin imzası.
        /// </summary>
        public string ParameterHash { get; set; }


    }
}
