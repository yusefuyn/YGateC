using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGate.Interfaces.DomainLayer;

namespace YGate.Interfaces.OperationLayer
{
    public interface IMailService
    {
        IRequestResult Send(string Victim, string Title, string Content, bool ContentIsHtml = false);
    }
}
