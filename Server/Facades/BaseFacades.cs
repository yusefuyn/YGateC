using YGate.Interfaces.OperationLayer;

namespace YGate.Server.Facades
{
    public class BaseFacades : IBaseFacades
    {
        public BaseFacades(IJsonSerializer jsonSerializer)
        {
            this.JsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public IJsonSerializer JsonSerializer { get; private set; }

    }
}
