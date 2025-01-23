using YGate.Entities;
using YGate.Entities.BasedModel;

namespace YGate.Client.Services.Property
{
    public interface IPropertyService
    {

        public Task<RequestResult> GetAllPropertyViewModel();
        public Task<RequestResult> AddGroup(PropertyGroup model);
        public Task<RequestResult> GetGroup(string guid);
        public Task<RequestResult> AddValues(List<PropertyGroupValue> values);
    }
}
